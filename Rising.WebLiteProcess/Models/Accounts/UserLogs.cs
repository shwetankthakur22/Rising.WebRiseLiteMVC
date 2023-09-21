using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rising.WebRise.Models
{
    public class UserLogs
    {
        public string UserID { get; set; }
        public string UserType { get; set; }
        public DateTime LogDateTime { get; set; }
        public string MacID { get; set; }
        public string PublicIP { get; set; }
        public string MenuName { get; set; }
        public string ViewCode { get; set; }
    }
}