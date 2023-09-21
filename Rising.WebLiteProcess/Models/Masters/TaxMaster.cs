using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Rising.WebRise.Models
{
    public class TaxMaster
    {
      
        [Display(Name = "Client Code")]
        public string ClientCode { get; set; }
        [Display(Name = "Tax Type")]
        public string TaxType { get; set; }
        [Display(Name = "Date From")]
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        [Display(Name = "Future %")]
        public string Future { get; set; }
        [Display(Name = "Option %")]
        public string Option { get; set; }
        [Display(Name = "Fut Interest %")]
        public string FutInterest { get; set; }
        [Display(Name = "Tax %")]
        public string Tax { get; set; }
        public string Maximum { get; set; }
        [Display(Name = "Fut Passive Tax %")]
        public string FutPassiveTax { get; set; }
        [Display(Name = "Opt Passive Tax %")]
        public string OptPassiveTax { get; set; }
        public string Rwid { get; set; }
        public string Exchange { get; set; }
        public bool Premium { get; set; }
        [Display(Name = "Strike Price")]
        public bool Strikeprice { get; set; }
        [Display(Name = "Strikeprice + Premium")]
        public bool StrikePremium { get; set; }
        public bool Client { get; set; }
        public bool State { get; set; }
        public bool Branch { get; set; }
        public bool Common { get; set; }
        public System.Data.DataSet result { get; set; }

    }
}
