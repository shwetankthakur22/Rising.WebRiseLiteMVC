using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;
using System.Data;
using System.Web.Mvc;

namespace Rising.WebRise.Models
{
    public class ScripDetails
    {

        [Display(Name = "Scrip Code")]
        public string ScripCode { get; set; }
        [Display(Name = "NSE Symbol")]
        public string NseSymbol { get; set; }
        public string Limit { get; set; }
        [Display(Name = "BSE Symbol")]
        public string BseSymbol { get; set; }
        [Display(Name = "ISIN Code")]
        public string ISINCode { get; set; }
        public string Series { get; set; }
        [Display(Name = "Variance %")]
        public string Variance { get; set; }
        public string Iliquid { get; set; }

        public System.Data.DataSet result { get; set; }

    }
}
