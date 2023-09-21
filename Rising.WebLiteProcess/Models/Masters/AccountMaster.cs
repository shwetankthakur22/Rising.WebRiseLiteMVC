using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Web.Mvc;

namespace Rising.WebRise.Models
{
    public class AccountMaster
    {

        [Required]
        [Display(Name = "Account Code")]
        public string AccountCode { get; set; }

        [Display(Name = "Account Description")]
        public string CodeName { get; set; }

        [Display(Name = "Existing Group")]
        public string ExistingGroup { get; set; }

        [Display(Name = "Existing Level")]
        public string ExistingGroup1 { get; set; }

        [Display(Name = "New Group")]
        public List<SelectListItem> NewGroup { get; set; }
       // public string NewGroup { get; set; }

         [Display(Name = " ")]
         public List<SelectListItem> NewGroup1 { get; set; }
        //public string NewGroup1 { get; set; }
        public int? ItemId { get; set; }
        public string Rwid { get; set; }


        public System.Data.DataSet result { get; set; }

        public System.Data.DataSet result1 { get; set; }

    }
}
