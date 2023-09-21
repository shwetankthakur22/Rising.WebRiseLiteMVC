using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Rising.WebRise.Models.Reports
{
    public class ContractNote
    {

        [Required]
        [Display(Name = "Pre Printed")]
        public bool PrePrinted { get; set; }

        [Required]
        [Display(Name = "Date Range")]
        public bool DateRange { get; set; }

        [Required]
        [Display(Name = "Without Taxes")]
        public bool WithoutTaxes { get; set; }

        [Required]
        [Display(Name = "Client Code")]
        public string ClientCode { get; set; }

        public string Group { get; set; }

        public string Branch { get; set; }

        public string To { get; set; }

        [Required]
        [Display(Name = "Trade Date Fr")]
        public string TradeDateFr { get; set; }



    }
}