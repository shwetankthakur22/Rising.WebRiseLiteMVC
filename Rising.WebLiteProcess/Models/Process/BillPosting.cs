using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;
using System.Data;
using System.Web.Mvc;

namespace Rising.WebRise.Models
{
    public class BillPosting
    {
        public DataSet dsOut { get; set; }         
        [Display(Name = "Trade Date")]        
        public DateTime TrDate { get; set; }
        public String Exchange { get; set; }
        [Display(Name = "Date To")]
        public DateTime DateTo { get; set; }
        [Display(Name = "Posting Date")]
        public DateTime PostingDate { get; set; }
        [Display(Name = "Symbol")]
        public string SymbolList { get; set; }
        public string SessionId { get; set; }
        [Display(Name = "Selective Client")]
        public string SelectiveClient { get; set; }
        [Display(Name = "Expiry Date")]
        public DateTime ExpiryDate { get; set; }  
        public string Client { get; set; }

    }
    
}