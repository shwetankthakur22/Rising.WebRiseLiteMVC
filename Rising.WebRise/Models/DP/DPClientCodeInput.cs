using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;
using System.Web.Mvc;

namespace Rising.WebRise.Models
{
    public class DPClientCodeInput
    {
        [Required]
        [Display(Name = "ClientCode")]
        public string ClientCodeFrom { get; set; }

    }
}