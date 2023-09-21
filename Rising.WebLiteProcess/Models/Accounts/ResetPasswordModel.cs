using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;
using System.Web.Security;

namespace Rising.WebRise.Models
{  
    public class ResetPasswordModel
    {        
        [Display(Name = "UserID")]
        
        public string UserID { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Old Password")]
        public string OldPassword { get; set; }
               

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 8)]
        [DataType(DataType.Password)]
        [MembershipPassword()]
        [Display(Name = "New Password")]
        public string NewPassword { get; set; }


        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("NewPassword", ErrorMessage = "The password and confirmation password do not match.")]
   
        public string ConfirmPassword { get; set; }


    }
}
