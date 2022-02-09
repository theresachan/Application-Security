using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SITConnect.Models
{
    public class Users
    {
        public int UserID { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string CrdCardInfo { get; set; }
        public string Email { get; set; }
        public string Password { get; set; }
        public string DOB { get; set; }
        public string ImagePath { get; set; }
        public HttpPostedFileBase uploadFile { get; set; }
        public string Active { get; set; }
        public string Blocked { get; set; }

    }
}