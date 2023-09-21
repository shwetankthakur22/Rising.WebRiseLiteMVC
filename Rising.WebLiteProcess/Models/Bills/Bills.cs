using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;
using System.Data;
using System.Web.Mvc;

namespace Rising.WebRise.Models
{
    public class Bills
    {

        public string Exchange { get; set; }

        [Required]
        [Display(Name = "Trade Date")]
        public DateTime TrDateFrom { get; set; }

        public DateTime TrDateTo { get; set; }

        public string Client { get; set; }

        public string Branch { get; set; }

        public string Group { get; set; }

        [Display(Name = "Future MTM")]
        public bool FutureMTM { get; set; }

        [Display(Name = "Option Premium")]
        public bool OptionPremium { get; set; }

        [Display(Name = "Future Expiry Bill")]
        public bool FutureExpiryBill { get; set; }

        [Display(Name = "Option Daily Bill")]
        public bool OptionDailyBill { get; set; }

        [Display(Name = "Option Expiry Bill")]
        public bool OptionExpiryBill { get; set; }

        public string SessionId { get; set; }
 
        public System.Data.DataSet result { get; set; }

    }
}
