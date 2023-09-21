using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Rising.WebRise.Models
{  
    public class HolidayMaster
    {


        [Required]
        [Display(Name = "Date")]
        public DateTime StartDate { get; set; }

        public string Exchange { get; set; }
        [Display(Name = "Date")]
        public string Sdate { get; set; }

        [Display(Name = "Holiday For")]
        public string Holiday { get; set; }

        public string oldholiday { get; set; }

        public System.Data.DataSet result { get; set; }

    }
}
