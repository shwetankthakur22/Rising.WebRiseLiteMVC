using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;
using System.Web.Mvc;



namespace Rising.WebRise.Models
{
    public class TradeConfInput
    {
        [Required]
        [Display(Name = "Client Code")]
        public string ClientCodeFrom { get; set; }

        [Required]
        [Display(Name = "Code Name")]
        public string ClientName { get; set; }
        public string ClientCodeTo { get; set; }

        [Required]
        [Display(Name = "Date From")]
        public DateTime DateFrom { get; set; }

        [Required]
        [Display(Name = "Date To")]
        public DateTime DateTo { get; set; }

        [Required]
        [Display(Name = "Fin.As On")]
        public DateTime FinAsOn { get; set; }

        [Required]
        [Display(Name = "SymbolList")]
        public List<SelectListItem> SymbolList { get; set; }

        [Required]
        [Display(Name = "Contract")]
        public string Contract { get; set; }

        [Required]
        [Display(Name = "UserID")]
        public string UserId { get; set; }

        [Required]
        [Display(Name = "CTCLID")]
        public string CtclId { get; set; }
     
        [Required]
        [Display(Name = "Group")]
        public string Group { get; set; }

        [Required]
        [Display(Name = "Branch")]
        public string Branch { get; set; }

        [Required]
        [Display(Name = "Include CL Trxn")]
        public bool IncludeCLTrxn { get; set; }

        [Required]
        [Display(Name = "Ignore BF/CF")]
        public bool IgnoreBFCF { get; set; }

        [Required]
        [Display(Name = "Exchange Trades")]
        public bool ExchangeTrades { get; set; }

        [Required]
        [Display(Name = "Show Actual Qty")]
        public bool ShowActualQty { get; set; }

        [Required]
        [Display(Name = "Without Taxes")]
        public bool WithoutTaxes { get; set; }

        [Required]
        [Display(Name = "Merge Same Mkt Rate")]
        public bool MergeSameMktRate { get; set; }

        [Required]
        [Display(Name = "Outstanding Trade")]
        public bool OutstandingTrade { get; set; }

        [Required]
        [Display(Name = "Custodian Trade")]
        public bool CustodianTrade { get; set; }



        [Display(Name = "Index")]
        public enumIndexLists Index { get; set; }

        public enumexchanges Exchange { get; set; }

        [Display (Name = "SessionId")]
        public string SessionId { get; set; }

        [Display(Name = "IgnoreId")]
        public bool IgnoreId { get; set; }

        [Display (Name = "Expire Date")]
        public string ExpireDate { get; set; }

        [Display(Name = "DateContractWise")]
        public bool DateContractWise { get; set; }

        [Display (Name = "ContractDateWise")]
        public bool ContractDateWise { get; set; }

        [Display (Name = "MsOutlook")]
        public bool MsOutlook { get; set; }

        [Display (Name = "OutlookExpress")]
        public bool OutlookExpress { get; set; }
    }

    public enum enumexchanges
    {
        NSE,
        BSE,
        MCX,
        NFO,
        BFO,
        NCOM,
        BCOM,
        MCOM,
        NDEX,
        ICEX,
        NCD,
        BCD,
        MCD,
        INX

    }



    public enum enumIndexLists
    {
        ALL,
        FUTIDX,
        OPTIDX,
        OPTSTK,
        FUTSTK

    }
}