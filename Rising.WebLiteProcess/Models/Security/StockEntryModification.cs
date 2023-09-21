using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;
using System.Data;
using System.Web.Mvc;

namespace Rising.WebRise.Models
{
    public class StockEntryModification
    {

        public string Exchange { get; set; }
        public DateTime Date { get; set; }
        [Display(Name = "Client Code")]
        public string ClientCode { get; set; }
        [Display(Name = "Client Name")]
        public string ClientName { get; set; }
        [Display(Name = "Dp ID")]
        public string DpId { get; set; }
        [Display(Name = "DP A/c No.")]
        public string DpAcNo { get; set; }
        [Display(Name = "Scrip Code")]
        public string ScripCode { get; set; }
        [Display(Name = "Scrip Name")]
        public string ScripName { get; set; }
        [Display(Name = "Delv. Type")]
        public string DelvType { get; set; }
        public string Stock { get; set; }
        [Display(Name = "Ref.No.")]
        public string RefNo { get; set; }
        public string Purpose { get; set; }
        [Display(Name = "Iss/Rec.Qty")]
        public string IssQty { get; set; }
        public bool Recevied { get; set; }
        public bool Issued { get; set; }
        [Display(Name = "Date From")]
        public DateTime DateFrom { get; set; }
        [Display(Name = "Date To")]
        public DateTime DateTo { get; set; }
        [Display(Name = "Clos Rate Date")]
        public DateTime ClosRateDate { get; set; }
        [Display(Name = "As On")]
        public DateTime AsOn { get; set; }
        public System.Data.DataSet result { get; set; }
    }
}
