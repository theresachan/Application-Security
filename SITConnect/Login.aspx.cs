using Newtonsoft.Json;
using SITConnect.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SITConnect
{
    public partial class Login : System.Web.UI.Page
    {
        private string recaptchaSecret = "6LeCOkUeAAAAAK1OYJRBOMwUQ5Qhek-SZUPQGGws";  /*Secret Key --- 6LerAgceAAAAAM7gGAKouqHnz7w9KwrI25OnIjyw*/
        private string Token = string.Empty;
        private ResponseToken response = new ResponseToken();
        protected void Page_Load(object sender, EventArgs e)
        {
            if (Request.QueryString["id"] != null)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "alert('Your account is verified successfully. Please login.');", true);

            }
        }
        public ResponseToken CaptchaVerify()
        {
            //It should only call once
            if (response.score == 0)
            {
                Token = hf_token.Value;
                var responseString = RecaptchaVerify(Token);
                response = JsonConvert.DeserializeObject<ResponseToken>(responseString.Result);

            }
            return response;


        }
        private string apiAddress = "https://www.google.com/recaptcha/api/siteverify";


        private async Task<string> RecaptchaVerify(string recaptchaToken)
        {
            string url = $"{apiAddress}?secret={recaptchaSecret}&response={recaptchaToken}";
            using (HttpClient httpClient = new HttpClient())
            {
                try
                {

                    string responseString = httpClient.GetStringAsync(url).Result;
                    return responseString;

                }

                catch (Exception ex)
                {
                    throw new Exception(ex.Message);
                }
            }
        }


        public class ResponseToken
        {

            public DateTime challenge_ts { get; set; }
            public float score { get; set; }
            public List<string> ErrorCodes { get; set; }
            public bool Success { get; set; }
            public string hostname { get; set; }
        }
        protected void btnLogin_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "alert('Please validate the page.');", true);
            }
            txtEmail.Text = txtEmail.Text.Trim().ToLower();
            if (CaptchaVerify().Success) {
                string strcon = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                SqlConnection con = new SqlConnection(strcon);
                con.Open();
                var encPass = Encryption.EncryptRecord(txtPassword.Text);

                SqlDataAdapter da = new SqlDataAdapter("select *, CASE WHEN blocked=1 THEN (datediff(MINUTE, BlockDate, ('"+DateTime.Now.ToString("MM/dd/yyyy")+ "')))  ELSE 0  END as blockminutes from tblusers where Email='" + txtEmail.Text.ToLower().Trim() + "' and Password='" + encPass + "' and active=1 ", con);
                DataTable dt = new DataTable();
                da.Fill(dt);
                if (dt.Rows.Count > 0)
                {
                    if (dt.Rows[0]["blockminutes"].ToString()=="True" && Convert.ToInt32(dt.Rows[0]["blockminutes"].ToString())<1)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "ShowAccountMsg();", true);
                    }
                    else
                    {
                        Session["UserID"] = dt.Rows[0]["UserID"].ToString();
                        Session["Email"] = dt.Rows[0]["Email"].ToString();
                        Session["Name"] = dt.Rows[0]["FirstName"].ToString() + " " + dt.Rows[0]["LastName"].ToString();
                        // unblock user
                        if (dt.Rows[0]["blockminutes"].ToString() == "True")
                        {
                            SqlCommand cmd3 = new SqlCommand("update tblusers set blocked=0 where Email='" + txtEmail.Text.ToLower().Trim() + "'", con);
                            cmd3.ExecuteNonQuery();
                        }


                        string query = "INSERT INTO Log(LogTime, LogAction, LogNote) values('" + DateTime.Now.ToString("MM/dd/yyyy") + "','Login','User " + txtEmail.Text + " login Successfully.')";
                        SqlCommand cmd = new SqlCommand(query, con);
                        cmd.ExecuteNonQuery();
                        Response.Redirect("/Dashboard.aspx");
                    }
                   
                }
                else
                {
                    var isBlocked = false;
                    if (Session["counter"] == null)
                    {
                        Session["counter"] = 1;
                        Session["userEmail"] = txtEmail.Text;
                    }
                    else
                    {
                     
                        if (Convert.ToInt32(Session["counter"].ToString()) >= 2)
                        {
                            SqlCommand cmd = new SqlCommand("update tblUsers set blocked=1,BlockDate='" + DateTime.Now.ToString("MM/dd/yyyy")+"' where email='" + txtEmail.Text+ "'",con);
                            if (cmd.ExecuteNonQuery() > 0)
                            {
                                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "ShowAccountMsg();", true);
                                isBlocked = true;
                                SqlCommand cmdd = new SqlCommand("INSERT INTO Log(LogTime, LogAction, LogNote) values('" + DateTime.Now.ToString("MM/dd/yyyy") + "','Login','User " + txtEmail.Text + "  Blocked due to wrong attempt.')", con);
                                cmdd.ExecuteNonQuery();
                            }
                          
                        }
                        else
                        {
                            if (Session["userEmail"].ToString() == txtEmail.Text)
                            {
                                Session["counter"] = Convert.ToInt32(Session["counter"]) + 1;
                            }
                            else
                            {
                                Session["counter"] = 1;
                                Session["userEmail"] = txtEmail.Text;
                            }
                        }
                            
                    }
                    if (!isBlocked)
                    {
                        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);

                        SqlCommand cmd = new SqlCommand("INSERT INTO Log(LogTime, LogAction, LogNote) values('" + DateTime.Now.ToString("MM/dd/yyyy") + "','Login','User " + txtEmail.Text + "  Login Failed.')", con);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            else
            {
                ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "ShowalertRecaptcha();", true);
            }
           

        }
    }
}