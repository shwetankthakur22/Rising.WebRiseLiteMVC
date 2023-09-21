using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;
using System.Data;
using System.Web.Mvc;

namespace Rising.WebRise.Models
{
    public class CloseRateEntryIn
    {


        public DataSet dsOut { get; set; }

        [Required]
        [Display(Name = "Trade Date")]
        public DateTime TrDate { get; set; }

        public string Exchange { get; set; }

        [Required]
        [Display(Name = "Symbol")]
        //public string SymbolList { get; set; }
        public List<WebRiseProcess.Models.Symbol> SymbolList { get; set; }
        public string SessionId { get; set; }

        [Required]
        [Display(Name = "Expiry Date")]
        public DateTime ExpiryDate { get; set; }

        public CloseRateEntryOut closeRateEntryOut { get; set; }

    }


}