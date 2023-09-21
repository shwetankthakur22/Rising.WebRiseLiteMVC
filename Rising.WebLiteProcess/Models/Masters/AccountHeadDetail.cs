using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Rising.WebRise.Models
{
    public class AccountHeadDetail
    {

        public string Rwid { get; set; }
        public string CONTRACT { get; set; }


        public System.Data.DataSet result { get; set; }


        public string Exchange { get; set; }

        public string AccountCode { get; set; }

        public string AccountDesc { get; set; }

        public string Group { get; set; }

        public string GroupDesc { get; set; }

        public string OpeningBal { get; set; }

        public string Branch { get; set; }

        public string SubBranch { get; set; }

        public string Remarks { get; set; }

        public string Grouplvl2 { get; set; }

        public string Grouplvl3 { get; set; }
        public object ClientCode { get; internal set; }
    }
}