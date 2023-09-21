using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Rising.WebRise.Models
{
    public class Transaction
    {
        public string Exchange { get; set; }

        [Required]
        [Display(Name = "User Id")]
        public string UserID { get; set; }

        [Required]
        [Display(Name = "Session Id")]
        public string SessionID { get; set; }

        [Required]
        [Display(Name = "Transaction Date")]
        public DateTime TrDate { get; set; }

        public string Index { get; set; }

        [Required]
        [Display(Name = "Custodian Trades")]
        public bool CustodianTrades { get; set; }

        [Required]
        [Display(Name = "Old Expiry Date")]
        public DateTime OldExpDate { get; set; }

        [Required]
        [Display(Name = "New Expiry Date")]
        public DateTime NewExpDate { get; set; }

        public System.Data.DataSet result { get; set; }

        public bool deleteOnly { get; set; }

        [Required]
        [Display(Name = "On & After Date")]
        public DateTime OnDate { get; set; }

        [Required]
        [Display(Name = "ExpiryDateList")]
        public List<SelectListItem> ExpiryDateList { get; set; }


        [Required]
        [Display(Name = "ExpiryDate")]
        public string ExpiryDate { get; set; }


        [Display(Name = "Index")]
        public enumIndexList IndexList { get; set; }



    }
    public enum enumIndexList
    {
        All,
        FUTIDX,
        OPTIDX,
        OPTSTK,
        FUTSTK

    }

}