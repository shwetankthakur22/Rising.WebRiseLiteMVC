using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Rising.WebRise.Models.Reports
{
    public class IncomeTaxTurnOver
    {

        public string Exchange { get; set; }

        public string Branch { get; set; }

        public string To { get; set; }

        [Required]
        [Display(Name = "Date From")]
        public string DateFrm { get; set; }
        public string ClientName { get; set; }

        public string TradeDate { get; set; }

        public string ClientCode { get; set; }

        public string Address { get; set; }

        public string Pincode { get; set; }

        [Required]
        [Display(Name = "Selling Turnover")]
        public string SellingTurnover { get; set; }

        [Required]
        [Display(Name = "Buying Turnover")]
        public string BuyingTurnover { get; set; }

        [Required]
        [Display(Name = "Total Turnover")]
        public string TotalTurnover { get; set; }

        [Required]
        [Display(Name = "Pan No.")]
        public string PanNo { get; set; }

        public string FName { get; set; }

        [Required]
        [Display(Name = "Ref No.")]
        public string RefNo { get; set; }

        public bool Exclude { get; set; }

        public bool Consolidated { get; set; }


        [Required]
        [Display(Name = "Include Custodian")]
        public bool IncludeCustod { get; set; }

        public string Select { get; set; }

        public System.Data.DataSet result { get; set; }

    }
}