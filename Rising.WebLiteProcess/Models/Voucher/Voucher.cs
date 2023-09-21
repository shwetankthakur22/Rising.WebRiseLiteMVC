using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;
using System.Data;
using System.Web.Mvc;

namespace Rising.WebRise.Models
{
    public class Voucher
    {

        public string Exchange { get; set; }

        [Required]
        [Display(Name = "Voucher Date")]
        public DateTime VoucherDate { get; set; }

        public DateTime BillDate { get; set; }

        public string Balance { get; set; }

        [Display(Name = "Voucher Type")]
        public string VoucherType { get; set; }

        [Display(Name = "Voucher No")]
        public string VoucherNo { get; set; }

        [Display(Name = "Bank/Cash Code")]
        public string BankCode { get; set; }

        [Display(Name = "Branch Code")]
        public string BranchCode { get; set; }

        [Display(Name = "Dollar Rt.Dt.")]
        public DateTime DollarRtDt { get; set; }

        [Display(Name = "Dollar Rt.")]
        public DateTime DollarRt { get; set; }


        public string SessionId { get; set; }
 
        public System.Data.DataSet result { get; set; }

    }
}
