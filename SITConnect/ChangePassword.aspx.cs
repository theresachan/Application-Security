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
    public partial class ChangePassword : System.Web.UI.Page
    {
        protected void Page_Load(object sender, EventArgs e)
        {

        }


        protected void CustomValidator1_ServerValidate(object source, ServerValidateEventArgs args)
        {
            HashSet<char> specialCharacters = new HashSet<char>() { '%', '$', '#', '@', '&', '*' };
            string password = txtPassword.Text;
            if (password == "")
            {
                args.IsValid = false;
                CustomValidator1.ForeColor = System.Drawing.Color.Red;
                CustomValidator1.ErrorMessage = "Please enter password.";
                Message.Text = "Please enter password.";
            }

            else
            {
                if (password.Any(char.IsLower) && //Lower case 
                password.Any(char.IsUpper) &&
                password.Any(char.IsDigit) &&
                password.Any(specialCharacters.Contains))
                {
                    args.IsValid = true;
                }
                else
                {
                    args.IsValid = false;
                    CustomValidator1.ForeColor = System.Drawing.Color.Red;
                    CustomValidator1.ErrorMessage = "Password must be the combinition of uppper case, lower case, special characters and digits.";
                    Message.Text = "Password must be the combinition of uppper case, lower case, special characters and digits.";
                }
                if (password.Length < 12)
                {
                    args.IsValid = false;
                    CustomValidator1.ForeColor = System.Drawing.Color.Red;
                    Message.Text = "Password length must be the greater than or equal to 12.";
                    CustomValidator1.ErrorMessage = "Password length must be the greater than or equal to 12.";
                }
            }





        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            if (Session["UserID"] == null)
                Response.Redirect("/Login.aspx");
            if (!Page.IsValid)
            {
                Message.Text = "Page is not valid.";
            }
            else
            {

                string strcon = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                SqlConnection con = new SqlConnection(strcon);
                con.Open();
                var password = Encryption.EncryptRecord(txtOldPassword.Text);
                var newPassword = Encryption.EncryptRecord(txtPassword.Text);
                SqlDataAdapter da = new SqlDataAdapter("select *, isnull((datediff(MINUTE, PasswordChangeDate, ('"+DateTime.Now.ToString("MM/dd/yyyy") +"'))),-1) as lastUpdated from tblUsers where userID=" + Session["UserID"] + " and password='" + password + "'", con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    if(Convert.ToInt32(dt.Rows[0]["lastUpdated"].ToString())>=0 && Convert.ToInt32(dt.Rows[0]["lastUpdated"].ToString()) < 4)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "alert('You cannot change the password within 4 minutes');", true);
                        return;

                    }
                 else   if (dt.Rows[0]["Password"].ToString() == newPassword)
                    {
                        //ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "ShowMsg();", true);
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "alert('Your password matched with your current password. Please change different password');", true);

                        return;
                    }
                 else   if (dt.Rows[0]["OldPassword"].ToString() == newPassword || dt.Rows[0]["SecondOldPassword"].ToString() == newPassword)
                    {
                        //ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "ShowMsg();", true);
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "alert('Your password matched with one of your previous password. Please try different password');", true);
                        // Message.Text = "Your password matched with one of your previous password. Please change different password";
                        return;
                    }
                    else
                    {
                        SqlCommand cmd = new SqlCommand("update tblUsers set OldPassword=(select Password from tblUsers where UserID="+ Session["UserID"] + "),  SecondOldPassword=(select OldPassword from tblUsers where UserID="+ Session["UserID"] + "), password='"+ newPassword + "', PasswordChangeDate='"+DateTime.Now.ToString("MM/dd/yyyy") + "' where userid=" + Session["UserID"] + "",con);
                        if (cmd.ExecuteNonQuery() > 0)
                        {
                            SqlCommand cmdd = new SqlCommand("INSERT INTO Log(LogTime, LogAction, LogNote) values('" + DateTime.Now.ToString("MM/dd/yyyy") + "','Change password','User " + Session["Email"].ToString() + "  Password changed.')", con);
                            cmdd.ExecuteNonQuery();
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "alert('Password updated successfully');", true);

                        }

                    }
                }
                else
                {
                    ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "alert('Your password does not match with your current password.');", true);

                }

            }
        }
    }
}