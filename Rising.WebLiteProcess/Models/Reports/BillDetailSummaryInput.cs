using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Rising.WebRise.Models
{
    
  
    public class BillDetailSummaryInput
    {
        public string Exchange { get; set; }
        public string SessionId { get; set; }
        [Required]
        [Display(Name = "DateFrom")]
        public DateTime TrDate { get; set; }

        [Required]
        [Display(Name = "DateTo")]
        public DateTime ToDate { get; set; }
        public string Branch { get; set; }
        [Display(Name = "ClientCode")]
        public string ClientCodeFrom { get; set; }
        public string ClientName { get; set; }
        public string ACGroup { get; set; }
        public string Group { get; set; }
        public string BillSelection { get; set; }
        [Required]
        [Display(Name = "On Date")]
        public DateTime OnDate { get; set; }

        [Required]
        [Display(Name = "Close Price Date")]
        public DateTime ClosePriceDate { get; set; }

    }

  
}
