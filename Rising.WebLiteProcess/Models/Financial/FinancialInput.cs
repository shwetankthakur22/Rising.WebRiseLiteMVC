using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;
using System.Data;

namespace Rising.WebRise.Models
{
    public class FinancialInput
    {

        public DataSet dsOut { get; set; }

        [Display(Name = "Search ")]
        public List<string> Search { get; set; }
                    
        [Display(Name = "Financial From")]        
        public DateTime FinancialFrom { get; set; }

        [Display(Name = "Financial To")]
        public DateTime FinancialTo { get; set; }

        public string Exchange { get; set; }

        [Display(Name = "Include Stax")]
        public bool IncludeStax { get; set; }

        public string Client { get; set; }

        public string Branch { get; set; }

        public string Group { get; set; }

        [Display(Name = "Date From")]
        public DateTime DateFrom { get; set; }

        [Display(Name = "Date To")]
        public DateTime DateTo { get; set; }

        [Display(Name = "Include CL")]
        public bool IncludeCL { get; set; }

        [Display(Name = "Include Exchange/Broker Code")]
        public bool IncludeExchangeBrokerCode { get; set; }

    }


}