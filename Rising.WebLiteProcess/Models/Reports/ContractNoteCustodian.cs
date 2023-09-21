using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Rising.WebRise.Models.Reports
{
    public class ContractNoteCustodian
    {
        public string Exchange { get; set; }

        [Required]
        [Display(Name = "Trade Date")]
        public string TradeDate { get; set; }

        [Required]
        [Display(Name = "Client Code")]
        public string ClientCode { get; set; }


    }
}