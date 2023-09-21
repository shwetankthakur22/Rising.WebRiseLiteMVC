using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;
using System.Web.Mvc;


namespace Rising.WebRise.Models
{
    public class NetPositionInput
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
        [Display(Name = "Closing Rate Date")]
        public DateTime DateClosing { get; set; }

        [Required]
        [Display(Name = "ExpiryDateList")]
        public List<SelectListItem> ExpiryDateList { get; set; }

        [Required]
        [Display(Name = "ExpiryDate")]
        public string ExpiryDate { get; set; }       
                
        [Required]
        [Display(Name = "Open Position Only")]
        public bool OpenPosition { get; set; }

        [Required]
        [Display(Name = "SymbolList")]
        public List<SelectListItem> SymbolList { get; set; }

        [Required]
        [Display(Name = "Symbol")]
        public string Symbol { get; set; }

        [Required]
        [Display(Name = "As On Date")]
        public bool AsOnDate { get; set; }

    }
}