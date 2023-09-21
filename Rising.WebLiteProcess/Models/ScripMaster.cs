using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System.Linq;
using System.Web;

namespace Rising.WebRise.Models
{
    public class ScripMaster
    {
        [Display(Name = "Scrip Code")]
        public string ScripCode { get; set; }

        [Display(Name = "Scrip Name")]
        public string ScripName { get; set; }

        [Display(Name = "ISIN Code")]
        public string ISINCode { get; set; }

        [Display(Name = "Short Name")]
        public string ShortName { get; set; }

        [Display(Name = "ISIN Code Old")]
        public string ISINCodeOld { get; set; }

        [Display(Name = "Demat Scrip")]
        public string DematScrip { get; set; }

        public string Variance { get; set; }

        [Display(Name = "Lot Size")]
        public string LotSize { get; set; }

        [Display(Name = "Bond Scrip")]
        public string BondScrip { get; set; }

        [Display(Name = "Gross Exposure Times")]
        public string GrossExposure { get; set; }

        public string Iliquid { get; set; }

        [Display(Name = "Group CD")]
        public string GroupCD { get; set; }

        [Display(Name = "MTF Movement")]
        public string MTFMovement { get; set; }

        [Display(Name = "STT Calculate")]
        public string STTCalculate { get; set; }

        [Display(Name = "Margin Sale")]
        public string MarginSale { get; set; }

        [Display(Name = "Margin Buy")]
        public string MarginBuy { get; set; }

        public string Pledgeable { get; set; }
       

        [Display(Name = "Closing Stock(Phy)")]
        public string ClosingStockPhy { get; set; }

        [Display(Name = "Clo Stock(Demat)")]
        public string ClosingStockDemat { get; set; }

        [Display(Name = "Fund Flag")]
        public string FundFlag { get; set; }

        public string Remark { get; set; }

        [Display(Name = "Eq. Oriented Fund")]
        public string EquityOrientedFund { get; set; }

        [Display(Name = "No Of Share in Nse")]
        public string ShareInNse { get; set; }

        [Display(Name = "No Of Share in Bse")]
        public string ShareInBse { get; set; }

        [Display(Name = "Ignore Trnx Tax")]
        public string IgnoreTrnxTax { get; set; }

        [Display(Name="Start Date")]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:yyyy-MM-dd}")]
        public DateTime StartDate { get; set; }

        [Display(Name = "Custom Variance")]
        public string CustomVariance { get; set; }

        [Display(Name = "Delivery Trade From FO")]
        public string DelTradeFO { get; set; }

        [Display(Name = "Share Type")]
        public string ShareType { get; set; }

        [Display(Name = "DP Preference")]
        public string DPPreference { get; set; }

        [Display(Name = "Trnx Scrip Type")]
        public string TrnxScripType { get; set; }
     
        public string Symbol { get; set; }
    
        public string Series { get; set; }
     
        public string SettNo { get; set; }

        public string NodeFrom { get; set; }

        public string NodeTo { get; set; }

        public string Record { get; set; }

        public string FaceValue { get; set; }

        public string ExDiv { get; set; }

        public string BkCloseTo { get; set; }

        public string BkCloseFrom { get; set; }

        public string RowId { get; set; }

        public string LoginId { get; set; }

        public string MachineIp { get; set; }

        public DateTime ScripOpenDate { get; set; }

        public DataSet Scripdata { get; set; }


    }

    public enum enumPledgeable
    {
        Y,
        N
    }

}