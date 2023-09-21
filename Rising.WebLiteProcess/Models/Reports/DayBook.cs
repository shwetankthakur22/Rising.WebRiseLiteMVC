using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Rising.WebRise.Models.Reports
{
    public class DayBook
    {

        public string Exchange { get; set; }

        [Required]
        [Display(Name = "Date")]
        public string Date { get; set; }

        public string Contract { get; set; }

        [Display(Name = "Include BF/CF")]
        public bool IncludeBfCf { get; set; }


    }
}