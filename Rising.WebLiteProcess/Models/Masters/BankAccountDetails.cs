using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Rising.WebRise.Models.Masters
{
    public class BankAccountDetails
    {

        [Required]
        [Display(Name = "Bank Code")]
        public string BankCode { get; set; }

        [Required]
        [Display(Name = "Bank Name")]
        public string BankName { get; set; }

        [Display(Name = "Bank Address")]
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }

        
        [Display(Name = "MICR No.")]
        public string Micr { get; set; }

       
        [Display(Name = "IFSC Code")]

        public string IFSC { get; set; }

        public string Rwid { get; set; }

        

        //------account head detail--------------

        public System.Data.DataSet result { get; set; }

        [Required]
        [Display(Name = "Client Code")]
        public string ClientCodeFrom { get; set; }

        [Required]
        [Display(Name = "Client Name")]
        public string ClientName { get; set; }

        [Required]
        [Display(Name = "Bank A/C Type.")]
        public string BankAcctype { get; set; }
        [Required]
        [Display(Name = "Primary Account")]
        public bool PrimaryAcc { get; set; }

        [Required]
        [Display(Name = "Bank A/C No.")]
        public string BankAc { get; set; }
        public string AccountCode { get; internal set; }
        public string AccountDesc { get; internal set; }
        public string Group { get; internal set; }
        public string Branch { get; internal set; }
        public string Grouplvl2 { get; internal set; }
        public string Grouplvl3 { get; internal set; }
    }
}