using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Rising.WebRise.Models
{  
    public class ClientCodeInput
    {
        [Required]
        [Display(Name = "UserID")]
        
        public string UserID { get; set; }
        
    }
}
