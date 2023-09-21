using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;
using System.Web.Mvc;
using System.Data;


namespace Rising.WebRise.Models
{
    public class OpeningStock
    {
        [Required]
        [Display(Name = "ClientCode")]
        public string ClientCode { get; set; }

        [Required]
        [Display(Name = "OpeningDate")]
        public DateTime OpeningDate { get; set; }

        [Required]
        [Display(Name = "ScripCode")]
        public string ScripCode { get; set; }

        [Required]
        [Display(Name = "Quantity")]
        public string Quantity { get; set; }

        [Required]
        [Display(Name = "Rate")]
        public string Rate { get; set; }

        [Required]
        [Display(Name = "Exchange")]
        public string Exchange { get; set; }

        [Required]
        [Display(Name = "Remarks")]
        public string Remarks { get; set; }


        public DataSet DsOut1 { get; set; }
    }

    
}