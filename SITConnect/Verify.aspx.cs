using SITConnect.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SITConnect
{
    public partial class Verify : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            var verify = Encryption.DecryptRecord(Request.QueryString["id"]);
            string strcon = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
            SqlConnection con = new SqlConnection(strcon);
            con.Open();

            SqlDataAdapter da = new SqlDataAdapter("Select * from tblUsers where Email='" + verify.ToLower().Trim() + "'", con);
            DataTable dt = new DataTable();
            da.Fill(dt);
            if (dt.Rows.Count > 0)
            {
                SqlCommand cmd = new SqlCommand("update tblUsers set active=1 where email='" + verify + "'", con);
                if (cmd.ExecuteNonQuery() > 0)
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "alert('Account is verified successfully. Please login.');", true);
                    Response.Redirect("/Login.aspx?id=verified");
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "alert('some thing went wrong.');", true);

                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "alert('some thing went wrong.');", true);

            }
        }
    }
}