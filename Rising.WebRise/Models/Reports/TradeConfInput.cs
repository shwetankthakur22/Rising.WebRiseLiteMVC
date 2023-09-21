using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;
using System.Web.Mvc;


namespace Rising.WebRise.Models
{
    public class TradeConfInput
    {
        [Required]
        [Display(Name = "Client Code")]
        public string ClientCodeFrom { get; set; }

        [Required]
        [Display(Name = "Code To")]
        public string ClientCodeTo { get; set; }

        [Required]
        [Display(Name = "Date From")]
        public DateTime DateFrom { get; set; }

        [Required]
        [Display(Name = "Date To")]
        public DateTime DateTo { get; set; }        

        [Required]
        [Display(Name = "SymbolList")]
        public List<SelectListItem> SymbolList { get; set; }

        [Required]
        [Display(Name = "Symbol")]
        public string Symbol { get; set; }
      
    }
}