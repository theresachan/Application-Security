using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace SITConnect.Model
{
    public static class GoogleReCaptchaVariables
    {
        public static string ReCaptchaSiteKey = ConfigurationManager.AppSettings["reCaptchaSiteKey"]?.ToString() ?? string.Empty;
        public static string ReCaptchaSecretKey = ConfigurationManager.AppSettings["reCaptchaSecretKey"]?.ToString() ?? string.Empty;
        public static string InputName = "g-recaptcha-response";
    }
}