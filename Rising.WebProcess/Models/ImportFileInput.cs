using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;


namespace Rising.WebRise.Models
{  
    public class ImportFileInput
    {
        [Required]
        [Display(Name = "UserID")]
        
        public string FilePath { get; set; }

        public DateTime TradeDate { get; set; }

    }
}
