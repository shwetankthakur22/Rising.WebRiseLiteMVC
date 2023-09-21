using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Rising.WebRise.Models
{
    public class BranchMaintenance
    {
           
        public string Name { get; set; }
        [Required]
        [Display(Name = "Branch Code")]
        public string BranchCode { get; set; }
        public string Manager { get; set; }
        [Required]
        [Display(Name = "Address")]
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        public string Address4 { get; set; }
        [Required]
        [Display(Name = "Phone Nos.")]
        public string Phone { get; set; }
      
        [Display(Name = "Fax Nos.")]
        public string FaxNo { get; set; }
     
        [Display(Name = "Email Id")]
        public string EmailId { get; set; }
      
        [Display(Name = "Gst RegNo")]
        public string GstRegNo { get; set; }

        [Display(Name = "Intro Code")]
        public string IntroCode { get; set; }

        [Display(Name = "Introducer %")]
        public string Introducer { get; set; }
      
        [Display(Name = "Group Code")]
        public string GroupCode { get; set; }
       
        [Display(Name = "A/c Open Charges(Y/N)")]
        public string AccOpenCharges { get; set; }
       
        [Display(Name = "A/c Open Charge)")]
        public string AccOpenCharg { get; set; }
      
        [Display(Name = "Group Admin")]
        public string GroupAdmin { get; set; }
       
        [Display(Name = "Region Code")]
        public string RegionCode { get; set; }


        [Display(Name = "Region Desc")]
        public string RegionDesc { get; set; }

        [Display(Name = "RM Code")]
        public string RMCode { get; set; } 

        public string Zone { get; set; }

        public string Rwid { get; set; }

        public System.Data.DataSet result { get; set; }

        [Display(Name = "Sub Branch Code")]
        public string SubBranchCode { get; set; }
        [Display(Name = "Sub Branch Name")]
        public string SubBranchName { get; set; }
   
        public string Enable { get; set; }

        public string RMDesc { get; set; }

        public string Designation { get; set; }

        public string Department { get; set; }

        public string MobileNo { get; set; }

        public string Salary { get; set; }

        public DateTime JoiningDate { get; set; }

        public string City { get; set; }

        public string PinCode { get; set; }

        public string State { get; set; }

        public DateTime ClosingDate { get; set; }

        public string PerMonthTarget { get; set; }
 

    }
}
