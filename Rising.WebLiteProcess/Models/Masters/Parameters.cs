using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Rising.WebRise.Models
{
    public class Parameters
    {
        //------------Parameter grp 1----------------

        [Display(Name = "FA Year From")]
        public DateTime FAYearFrom { get; set; }
        [Display(Name = "FA Year To")]
        public DateTime FAYearTo { get; set; }
        [Display(Name = "Sebi Reg.No.")]
        public string SebiReg { get; set; }
        [Display(Name = "Expiry Billing Start Date")]
        public string ExpiryBillDate { get; set; }
        [Display(Name = "Service Tax Reg No.")]
        public string ServiceTaxRegNo { get; set; }
        [Display(Name = "Data Lock Date")]
        public string DataLockDate { get; set; }
        [Display(Name = "MTM Posting Intervals")]
        public string MTM { get; set; }      
        [Display(Name = "Phone Nos.")]
        public string PhoneNo { get; set; }      
        [Display(Name = "Fax No.")]
        public string FaxNo { get; set; }     
        public string Juridiction { get; set; }
        [Display(Name = "Trader Id")]
        public string TraderId { get; set; }
        [Display(Name = "Default Exchange")]
        public string DefaultExchange { get; set; }
        [Display(Name = "Brok.A/c Code(Pay)")]
        public string BrokAcCode { get; set; }     
        [Display(Name = "Brok.A/c(Charge)")]
        public string BrokAc { get; set; }       
        [Display(Name = "PRO Code")]
        public string ProCode { get; set; }       
        [Display(Name = "Clg.Account Code)")]
        public string ClgAccCode { get; set; }      
        [Display(Name = "Clearing Bank Account")]
        public string ClearingBankAcc { get; set; }       
        [Display(Name = "Daily Margin Control Account Code")]
        public string DailyMargin { get; set; }

        //------------Parameter grp 2-----------------

        [Display(Name = "S.Tax% [Old Rate]")]
        public string STaxOldRate { get; set; }
        [Display(Name = "S.Tax% [New Rate]")]
        public string STaxNewRate { get; set; }
        [Display(Name = "S.Tax Payable Code")]
        public string STaxPayCode { get; set; }
        [Display(Name = "S.Tax Chargeable Code")]
        public string STaxChargeCode { get; set; }
        [Display(Name = "S.Tax New Applied Date")]
        public DateTime STaxNewADate { get; set; }
        [Display(Name = "Expiry Billing System(Future)(Y/N)")]
        public string EBS { get; set; }
        [Display(Name = "Contract Initialization Date")]
        public DateTime ContractDate { get; set; }
        [Display(Name = "Transaction Fee Control Account")]
        public string TFeeCA { get; set; }
        [Display(Name = "MTM Control Account-Cumulative System")]
        public string MTMCaCs { get; set; }
        [Display(Name = "Stam Duty Control Account")]
        public string StampDutyCA { get; set; }
        [Display(Name = "Stamp Duty Narration")]
        public string StampDutyNarration { get; set; }
        [Display(Name = "Stamp Duty %")]
        public string StampDutyPercent { get; set; }
        [Display(Name = "Voucher Release System")]
        public string VoucherReleaseSystem { get; set; }
        [Display(Name = "S.Tax on Transaction Fee Control A/c")]
        public string StaxOnTTax { get; set; }
        [Display(Name = "STT On Bill Effect Data")]
        public string SttOnBillEffect { get; set; }
        [Display(Name = "Unique Member Code")]
        public string UniqueMemberCode { get; set; }
        [Display(Name = "Tax1 Control Account")]
        public string Tax1ControlAccount { get; set; }
        [Display(Name = "Tax1 Narration")]
        public string Tax1Narration { get; set; }
        [Display(Name = "Stax on TAX1 Control A/c")]
        public string StaxonTax1ControlAc { get; set; }
        [Display(Name = "Tax2 Control Account")]
        public string Tax2ControlAccount { get; set; }
        [Display(Name = "Tax2 Narration")]
        public string Tax2Narration { get; set; }
        [Display(Name = "Stax on TAX2 Control A/c")]
        public string StaxonTax2ControlAc { get; set; }
        [Display(Name = "Continous Vou No")]
        public string ContinousVouNo { get; set; }
        [Display(Name = "Vou Entry In SYSADM User")]
        public string VouEntryInUser { get; set; }
        [Display(Name = "StampDuty Round Off")]
        public string StampDutyRoundOff { get; set; }
        [Display(Name = "Bank A/c Mandatory in Vou.Entry")]
        public string BankAccount { get; set; }
        [Display(Name = "Editable TradeNo/OrderNo in Trade Edit")]
        public string TradeEdit { get; set; }

        //------------Parameter grp 3-----------------

        [Display(Name = "Introducer Control Account")]
        public string IntroControlAcc { get; set; }
        [Display(Name = "Branch File Data Folder")]
        public string BranchFileData { get; set; }
        [Display(Name = "Default Trade File Type")]
        public string DefaultTradeFile { get; set; }
        [Display(Name = "Default Trade File Import")]
        public string DefaultTradeImport { get; set; }
        [Display(Name = "Default Closing Price Rate")]
        public string DefaultClosingPrate { get; set; }
        [Display(Name = "Margin Type")]
        public string MarginType { get; set; }
        [Display(Name = "Date Folder Sequence")]
        public string DateFolderSequence { get; set; }
        [Display(Name = "PLACE(HTML Contract Note)")]
        public string Place { get; set; }
        [Display(Name = "Posting of ShortFall Penality Interval(1/2)")]
        public string PostingShortfall { get; set; }
        [Display(Name = "ShortFall Control Code")]
        public string ShortfallCCode { get; set; }
        [Display(Name = "Advance Brokerage Code")]
        public string AdvBrkCode { get; set; }
        [Display(Name = "Advance Brok System Enable")]
        public string AdvBrkSystemEnbl { get; set; }
        [Display(Name = "Single Entry Of Reverse Margin in Financial ")]
        public string ReverseMargin { get; set; }
        [Display(Name = "Combine Max Stamp on Bill")]
        public string CombineMaxStamp { get; set; }
        [Display(Name = "Stamp On Expiry")]
        public string StampOnExpiry { get; set;}
        [Display(Name = "Turn Tax On Expiry")]
        public string TurnTax { get; set;}
        [Display(Name = "Tax2 On CL Trades")]
        public string Tax2ClTrades { get; set;}
        [Display(Name = "Stamp On CL Trades")]
        public string StampOnClTrades { get; set;}
        [Display(Name = "Tax1 On CL Trades")]
        public string Tax1OnClTrades { get; set;}
        [Display(Name = "Active Passive Flag")]
        public string ActivePassiveFlag { get; set; }
        [Display(Name = "Show Value Date in Financial")]
        public string ShowDateInfinancial { get; set; }
        [Display(Name = "Show All Exchange")]
        public string AllExchange { get; set; }
        [Display(Name = "Check Taxes During Billing")]
        public string CheckTaxes{ get; set; }
        [Display(Name = "A/c Open Charge Control Account")]
        public string AccOpenCharge { get; set; }
        [Display(Name = "Voucher Entry Date")]
        public DateTime VouEntryDate { get; set; }
        [Display(Name = "Option MTM Control A/c")]
        public string OptionMTM { get; set; }
        [Display(Name = "Tax4 Control Account")]
        public string Tax4ControlAccount { get; set; }
        [Display(Name = "Tax4 Narration")]
        public string Tax4Narration { get; set;}
        [Display(Name = "Tax3 Control Account")]
        public string Tax3ControlAccount { get; set; }
        [Display(Name = "Tax3 Narration")]
        public string Tax3Narration { get; set; }
        [Display(Name = "Stax on Tax3 Control A/c")]
        public string StaxonTax3 { get; set; }
        [Display(Name = "SBC Control Code")]
        public string SBCControlCode { get; set; }
        [Display(Name = "SBC Applied Date")]
        public DateTime SBCAppliedDate { get; set; }
        [Display(Name = "Call & Trade Control Ac(In Posting)")]
        public string CallTradeControl { get; set; }
        [Display(Name = "CIN No")]
        public string CINNo { get; set; }
        [Display(Name = "Batch Time")]
        public string BatchTime { get; set; }
        [Display(Name = "Local Ip")]
        public string LocalIp { get; set; }
        [Display(Name = "Tax3 On Expiry")]
        public string Tax3OnExpiry { get; set; }



        public string Rwid { get; set; }

        public System.Data.DataSet result { get; set; }

      
 

    }
}
