using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Rising.WebRise.Models.Reports
{
    public class DaySummary
    {
        [Required]
        [Display(Name = "Client Code")]
        public string ClientCode { get; set; }

        public string Exchange { get; set; }

        [Required]
        [Display(Name = "Date From")]
        public string DateFrom { get; set; }

        public string Group { get; set; }
        public string To { get; set; }

        public string Branch { get; set; }
    }
}