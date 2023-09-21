using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Rising.WebRise.Models
{
    public class StateMaster
    {

        [Required]
        [Display(Name = "State Code")]
        public string StateCode { get; set; }

        [Required]
        [Display(Name = "State Name")]
        public string StateName { get; set; }

        [Required]
        [Display(Name = "State ID")]
        public string StateId { get; set; }

        [Required]
        [Display(Name = "Union Territory")]
        public string UnionTerritory { get; set; }

        [Required]
        [Display(Name = "GSTIN NO")]
        public string GSTINNO { get; set; }

        public string Rwid { get; set; }

        
        public System.Data.DataSet result { get; set; }

    }
}