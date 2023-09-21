using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;
using System.Data;

namespace Rising.WebRise.Models
{
    public class DebtorsCreditorsInput
    {

        public DataSet dsOut { get; set; }

        [Required]
        [Display(Name = "Search ")]
        public List<string> Search { get; set; }
                    

        [Required]
        [Display(Name = "Date To")]        
        public DateTime OnDate { get; set; }

        public enumexchange Exchange { get; set; }

        [Required]
        [Display(Name = "Include Capital Balance")]
        public bool IncludeCapitalBalance { get; set; }

        [Required]
        [Display(Name = "Merge With Code")]
        public bool MergeWithCode { get; set; }

        [Required]
        [Display(Name = "Show Margin Code")]
        public bool ShowMarginCode { get; set; }

        [Required]
        [Display(Name = "Filter Value")]
        public string FilterValue { get; set; }

        [Required]
        [Display(Name = "Group")]
        public string Group { get; set; }


        public string Selection { get; set; }

        [Required]
        [Display(Name = "Branch")]
        public string Branch { get; set; }

        [Required]
        [Display(Name = "Account Group")]
        public enumAccountGroup AccountGroup { get; set; }
    }

    public enum enumAccountGroup
    {
        SundryDebtorsCreditors,
        Revenue,
        BankAccounts,
        MarginAc
    }
}