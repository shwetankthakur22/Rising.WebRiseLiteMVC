using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Rising.WebRise.Models.Reports
{
    public class PortfolioAndExposure
    {
        [Required]
        [Display(Name = "Client Code")]
        public string ClientCode { get; set; }

        public string Exchange { get; set; }

        [Required]
        [Display(Name = "On Date")]
        public string OnDate { get; set; }

        [Required]
        [Display(Name = "Close Price Date")]
        public string ClosePriceDate { get; set; }

        public string Group { get; set; }

        public string Branch { get; set; }

        [Required]
        [Display(Name = "Show Exchange")]
        public bool ShowExchange { get; set; }

        public bool Report { get; set; }

        [Required]
        [Display(Name = "Session ID")]
        public string SessionId { get; set; }

    }
}