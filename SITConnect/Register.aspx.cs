using SITConnect.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.IO;
using System.Linq;
using System.Net.Mail;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

namespace SITConnect
{
    public partial class Register : System.Web.UI.Page
    {

        protected void Page_Load(object sender, EventArgs e)
        {

        }

        protected void Button1_Click(object sender, EventArgs e)
        {
            if (!Page.IsValid)
            {
                Message.Text = "Page is not valid.";
            }
            else
            {
                string strcon = ConfigurationManager.ConnectionStrings["ConnectionString"].ConnectionString;
                SqlConnection con = new SqlConnection(strcon);
                string dpBath = "";
                //con.Open();
                if (FileUpload1.HasFile)
                {
                    var mPath = Server.MapPath("/Content/Images");
                    var fileName = Path.GetFileName(FileUpload1.FileName);
                    var fullPath = mPath + "/" + fileName;
                    FileUpload1.SaveAs(fullPath);
                    dpBath = "/Content/Images/" + fileName;
                }
                var encPass = Encryption.EncryptRecord(txtPassword.Text);
                txtCredCard.Text = Encryption.EncryptRecord(txtCredCard.Text);
            // To avoid SQLi we are using the parameters
                SqlCommand cmd = new SqlCommand("insert into tblUsers(FirstName, LastName, CrdCardInfo, Email, Password, DOB, ImagePath) values(@fname,@lname,@creditcard,@email,@password,@dob,@dbpath)", con);
                cmd.Parameters.AddWithValue("fname", txtFName.Text);
                cmd.Parameters.AddWithValue("lname", txtLName.Text);
                cmd.Parameters.AddWithValue("creditcard", txtCredCard.Text);
                cmd.Parameters.AddWithValue("email", txtEmail.Text);
                cmd.Parameters.AddWithValue("password", encPass);
                cmd.Parameters.AddWithValue("dob", txtDOB.Text);
                cmd.Parameters.AddWithValue("dbpath", txtDOB.Text);
                //int res = cmd.ExecuteNonQuery();

                try
                {
                    con.Open();
                    int res = cmd.ExecuteNonQuery();
                    if (res > 0)
                    {
                        var emailSent = sendMail(txtEmail.Text);
                        if (emailSent)
                        {
                            ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                        }
                    }
                    //con.Close();
                }
                catch (Exception ex)
                {
                    //throw new Exception(ex.ToString());
                    //lb_error1.Text = ex.ToString();
                    lb_error1.Text = "Error!";
                    // Response.Redirect(ErrorPage);
                }
                finally
                {
                    con.Close();
                }

                //if (res > 0)
                //{
                //    var emailSent = sendMail(txtEmail.Text);
                //    if (emailSent)
                //    {
                //        ScriptManager.RegisterStartupScript(this, GetType(), "displayalertmessage", "Showalert();", true);
                //    }
                //}
            }
         
        }

        public bool sendMail(string recEmail)
        {
            string baseUrl = Request.Url.Scheme + "://" + Request.Url.Authority +
    Request.ApplicationPath.TrimEnd('/') + "/";
            MailMessage mail = new MailMessage();
            mail.To.Add(recEmail);
            mail.From = new MailAddress("sapp6887@gmail.com");
            mail.Subject = "SITConnect Account Verification";
            string Body = "Please click on the link below for verification for login at SITConnect <a href='"+ baseUrl+"Verify.aspx?id="+Encryption.EncryptRecord(txtEmail.Text) + "'> Click Here</a>";
            mail.Body = Body;
            mail.IsBodyHtml = true;
            SmtpClient smtp = new SmtpClient();
            smtp.Host = "smtp.gmail.com";
            smtp.Port = 587;
            smtp.UseDefaultCredentials = false;
            smtp.Credentials = new System.Net.NetworkCredential("sapp6887@gmail.com", "Billiegates@77650");
            smtp.EnableSsl = true;
            try
            {
                smtp.Send(mail);
                return true;
            }
            catch
            {
                return false;
            }


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
    }
}