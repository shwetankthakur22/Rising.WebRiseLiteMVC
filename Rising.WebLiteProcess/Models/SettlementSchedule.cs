using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Rising.WebRise.Models
{


    public class SettlementSchedule
    {

        [Display(Name = "Station Name")]
        public string StationName { get; set; }

        public string SettType { get; set; }

        public string Station { get; set; }

        [Display(Name = "Settlement No")]
        public string SettNo { get; set; }

        public string ExchSett { get; set; }

        [Required]
        [Display(Name = "Period From")]
         public DateTime PeriodFrom { get; set; }
        //public string PeriodFrom { get; set; }

        [Required]
        [Display(Name = "Period To")]
         public DateTime PeriodTo { get; set; }
       // public string PeriodTo { get; set; }

        [Required]
        [Display(Name = "Payin Date")]
        public DateTime PayinDate { get; set; }
        //public string PayinDate { get; set; }

        [Required]
        [Display(Name = "Payout Date")]
         public DateTime PayoutDate { get; set; }
        //public string PayoutDate { get; set; }

        [Required]
        [Display(Name = "DelIn Date")]
        public DateTime DelinDate { get; set; }

        [Required]
        [Display(Name = "DelOut Date")]
        public DateTime DeloutDate { get; set; }

        [Required]
        [Display(Name = "Date")]
        public DateTime StartDate { get; set; }

        public enumexchange Exchange { get; set; }

        [Display(Name = "File Name")]
        public string FilePath { get; set; }

        public bool NewFormat { get; set; }

        [Display(Name = "Description Of Settlement")]
        public string DescOfSettlement { get; set; }

        [Display(Name = "Settlement Period Gap")]
        public string SettlementPeriodGap { get; set; }

        [Display(Name = "Payin Gap")]
        public string PayinGap { get; set; }

        [Display(Name = "Payout Gap")]
        public string PayoutGap { get; set; }

        [Display(Name = "Holiday For")]
        public string Holiday { get; set; }

        public string BrokrageDebitNote { get; set; }

        public System.Data.DataSet result { get; set; }

        public List<SettlementSchedule> StationInfo { get; set; }

    }

    public enum enumexchange
    {
        NSE,
        BSE
    }





}