using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;
using System.Web.Mvc;

namespace Rising.WebRise.Models
{
    public class DPHoldingInput
    {
        [Required]
        [Display(Name = "AsOnDate")]
        public DateTime AsOnDate { get; set; }

    }
}