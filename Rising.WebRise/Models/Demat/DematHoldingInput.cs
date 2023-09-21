using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;
using System.Web.Mvc;

namespace Rising.WebRise.Models
{
    public class DematHoldingInput
    {

        [Required]
        [Display(Name = "Client Code")]
        public string ClientCodeFrom { get; set; }

        [Required]
        [Display(Name = "Code To")]
        public string ClientCodeTo { get; set; }

        [Required]
        [Display(Name = "AsOnDate")]
        public DateTime AsOnDate { get; set; }

    }
}