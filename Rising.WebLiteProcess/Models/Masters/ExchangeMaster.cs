using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Rising.WebRise.Models
{  
    public class ExchangeMaster
    {
        
        public string Station { get; set; }
        [Required]
        [Display(Name = "Station Name")]
        public string StationName { get; set; }
        public string Member { get; set; }
        public string MemberId { get; set; }

        [Required]
        [Display(Name = "Sebi Reg. No.")]
        public string SebiRegNo { get; set; }
        public string DpId { get; set; }
         public string CNSE { get; set; }
        [Required]
        [Display(Name = "Clg Bank")]
        public string ClgBank { get; set; }
       
        [Display(Name = "Pool A/C Id")]
        public string PoolAcId { get; set; }

        [Display(Name = "Stamp Duty Ind")]
        public string StampDutyInd { get; set; }

        [Display(Name = "Stamp Ctrl Code")]
        public string StampCtrlCode { get; set; }

        [Display(Name = "CMBP ID")]
        public string CmbpID { get; set; }

        [Display(Name = "Scrip Auto")]
        public string ScripAuto { get; set; }

        [Display(Name = "Delv100% Post")]
        public string Delv { get; set; }

        [Display(Name = "S.Tax RegNo")]
        public string STaxRegNo { get; set; }

        [Display(Name = "NSDL Charges")]
        public string NSDLCharges { get; set; }

        [Display(Name = "Calculate Brok on Trade Import")]
        public string CalTrdImport { get; set; }

        [Display(Name = "Default Trade Import Basis")]
        public string DefaultTradeImport { get; set; }

        [Display(Name = "A/C Code")]
        public string AccCode { get; set; }

        [Display(Name = "A/C Code Buy Back")]
        public string AccCodeBB { get; set; }

        [Display(Name = "Ndlv Clg Rate")]
        public string NdlvRate { get; set; }

        [Display(Name = "Trxn. Tax")]
        public string TrxnTax { get; set; }

      
        public string Mapin { get; set; }   

        [Display(Name = "Round Off")]
        public string RoundOff { get; set; }

        [Display(Name = "CDSL A/C-3")]
        public string CDSLAcc { get; set; }

        [Display(Name = "CDSL A/C-2")]
        public string CDSLAc { get; set; }

        [Display(Name = "CM ID")]
        public string CMID { get; set; }

        [Display(Name = "Lines for Contract Note")]
        public string LContractNote { get; set; }

        [Display(Name = "CDSL A/C-4")]
        public string CDSLAccc { get; set; }

        [Display(Name = "Pro Code")]
        public string ProCode { get; set; }

        public System.Data.DataSet result { get; set; }

    }
}
