using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;


namespace Rising.WebRise.Models
{  
    public class CodeSearchFilter
    {
        [Required]
        [Display(Name = "Search ")]
        public List<string> Search { get; set; }


        [Required]
        [Display(Name = "Code From")]
        public string ClientCodeFrom { get; set; }

        [Required]
        [Display(Name = "Code To")]
        public string ClientCodeTo { get; set; }

        public DataSet ds { get; set; }
    }
}
