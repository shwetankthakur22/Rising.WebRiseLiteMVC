using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Rising.WebRise.Models.Reports
{
    public class UserIdWiseSummary
    {
        public string Exchange { get; set; }

        [Required]
        [Display(Name = "Date from")]
        public string Datefrm { get; set; }

        public string To { get; set; }

        [Required]
        [Display(Name = "User ID")]
        public string UserID { get; set; }

        [Display(Name = "Include CL Trasaction")]
        public bool IncludeCLTrx { get; set; }



    }
}