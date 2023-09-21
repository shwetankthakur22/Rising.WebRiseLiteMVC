using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;

namespace Rising.WebRise.Models
{
    using Rising.OracleDBHelper;
    public class WebUserModel
    {
        [Required]
        [Display(Name = "UserID")]
        public string UserID { get; set; }

        [Required]
        [Display(Name = "UserName")]
        public string UserName { get; set; }

        [Required]
        [Display(Name = "EmailID")]
        public string EmailID { get; set; }

        [Required]
        [Display(Name = "MobileNo")]
        public string MobileNo { get; set; }

        [Required]
        [Display(Name = "DisableStatus")]
        public bool DisableStatus { get; set; }
               
        [Required]
        [Display(Name = "DisableDate")]
        public DateTime DisableDate { get; set; }

        [Required]
        [Display(Name = "AllowMultiLogin")]
        public bool AllowMultiLogin { get; set; }

        [Required]
        [Display(Name = "LoginStatus")]
        public bool LoginStatus { get; set; }

        [Required]
        [Display(Name = "UserType")]
        public UserType UserType { get; set; }

        [Required]
        [Display(Name = "MachineName")]
        public string MachineName { get; set; }
        
        [Required]
        [Display(Name = "ResetPassword")]
        public string ResetPassword { get; set; }

        [Required]
        [Display(Name = "ResetStatus")]
        public bool ResetStatus{ get; set; }

        [Required]
        [Display(Name = "ResetDate")]
        public DateTime ResetDate { get; set; }

        [Required]
        [Display(Name = "RequiredPasswordPolicy")]
        public bool RequiredPasswordPolicy { get; set; }

        [Required]
        [Display(Name = "UserRights")]
        public string UserRights { get; set; }
    }


    public class admin_query
    {
        public string query { get; set; }

        public string queryKey { get; set; }

        public bool update { get; set; }

        public System.Data.DataSet result { get; set; }
    }
}