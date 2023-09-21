using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Rising.WebRise.Models
{
    public class BrokrageMaster
    {
        public string Exchange { get; set; }
        [Display(Name = "Client Code")]
        public string ClientCode { get; set; }
        public string Symbol { get; set; }
        [Display(Name = "Brok.Type")]
        public string BrokType { get; set; }
        [Display(Name = "Instrument Type")]
        public string InstrumentType { get; set;}
        [Display(Name = "Date Range")]
        public string DateRange { get; set;}
        [Display(Name = "Brokrage On")]
        public string BrokrageOn { get; set; }
        [Display(Name = "Fixed Brok")]
        public string FixedBrok { get; set; }
        [Display(Name = "Expiry Brok")]
        public string ExpiryBrok { get; set; }
        [Display(Name = "Fix Min")]
        public string FixMin { get; set; }
        [Display(Name = "Client Id")]
        public bool ClientId { get; set; }
        public bool SlabId { get; set; }
        public bool Common { get; set; }
        [Display(Name = "Share Rate")]
        public string ShareRate { get; set; }
        [Display(Name = "Fixed Brok")]
        public string Fixedbrok { get; set; }
        [Display(Name = "Fixed Min")]
        public string FixedMin { get; set; }
        public string Rwid { get; set; }
    }
}