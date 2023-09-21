using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Rising.WebRise.Models
{
    public class BankMaster
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

        //[Required]
        //[MinLength(9, ErrorMessage = "The first name field must be at least 9 characters long")]
        [Display(Name = "MICR No.")]
        public string Micr { get; set; }

        //[StringLength(11, MinimumLength = 11, ErrorMessage = "Name must be 11 char")]
        //[Required]
        //[MinLength(11, ErrorMessage = "The first name field must be at least 11 characters long")]
        [Display(Name = "IFSC Code")]

        public string IFSC { get; set; }

        [Display(Name = "Bank ID")]
        public string BankID { get; set; }

        [Display(Name = "State Code")]
        public string StateCode { get; set; }

        [Display(Name = "Select File")]
        public string FileUpload { get; set; }

        public string city { get; set; }

        public string Name { get; set; }
        public string Password { get; set; }

        public string Rwid { get; set; }

        //------account head detail--------------

        public string pincode { get; set; }

        public System.Data.DataSet result { get; set; }
        public string ClientCode { get; set; }

        [Required]
        [Display(Name = "Client Name")]
        public string ClientName { get; set; }



        [Required]
        [Display(Name = "Bank A/C No.")]
        public string BankAc { get; set; }

        [Required]
        [Display(Name = "Principal Bank A/C No.")]
        public string PBankAc { get; set; }








        //------account head detail--------------


        public string Exchange { get; set; }

        public string AccountCode { get; set; }

        public string AccountDesc { get; set; }

        public string Group { get; set; }

        public string GroupDesc { get; set; }

        public string OpeningBal { get; set; }

        public string Branch { get; set; }

        public string SubBranch { get; set; }

        public string Remarks { get; set; }

        public string Grouplvl2 { get; set; }

        public string Grouplvl3 { get; set; }


    }
}
