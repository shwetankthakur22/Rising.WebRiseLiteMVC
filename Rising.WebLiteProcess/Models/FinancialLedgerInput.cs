using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;
using System.Data;

namespace Rising.WebRise.Models
{


    public class FinancialLedgerInput
    {

        [Display(Name = "Segment")]
        public List<WebRiseProecss.Models.Exchange> ExchangeList { get; set; }
        public DataSet dsOut { get; set; }

        [Required]
        [Display(Name = "Search ")]
        public List<string> Search { get; set; }


        //public string Query { get; set; }

        [Required]

        [Display(Name = "Client Code")]
        public string ClientCodeFrom { get; set; }

        public string ClientCodeTo { get; set; }

        [Required]
        [Display(Name = "Code Name")]
        public string ClientName { get; set; }

        [Required]
        [Display(Name = "Date From")]
        public DateTime DateFrom { get; set; }

        [Required]
        [Display(Name = "Date To")]
        public DateTime DateTo { get; set; }

        [Required]
        [Display(Name = "Branch")]
        public string Branch { get; set; }

        [Display(Name = "Order By")]
        public string OrderBy { get; set; }

        [Required]
        [Display(Name = "Group")]
        public string Group { get; set; }

        public string Exchange { get; set; }

        [Required]
        [Display(Name = "Inc.UnRelease Vou")]
        public bool IncludeUnReleaseVoucher { get; set; }

        [Required]
        [Display(Name = "Exclude MG13")]
        public bool ExcludeMG13Entries { get; set; }

        [Required]
        [Display(Name = "Ignore Opn. Bal.")]
        public bool IngnoreOpeningBalance { get; set; }

        [Required]
        [Display(Name = "Inc.Nse Cash")]
        public bool IncludeNseCash { get; set; }


        [Required]
        [Display(Name = "Show Margin")]
        public bool ShowMargin { get; set; }

        [Required]
        [Display(Name = "Inc.Sec.Val")]
        public bool IncludeSecurityVal { get; set; }

        [Required]
        [Display(Name = "Consider INR_DRCR")]
        public bool ConsiderINR { get; set; }

        [Required]
        [Display(Name = "On Real Date Basis")]
        public bool OnRealDateBasis { get; set; }


        [Required]
        [Display(Name = "Opening Bal.")]
        public string OpeningBalance { get; set; }

        [Required]
        [Display(Name = "Ignore Nil Bal.")]
        public bool IgnoreNilBalance { get; set; }

        [Display(Name = "Security Bal.")]
        public string SecurityBalance { get; set; }
        [Display(Name = "Closing Bal.")]
        public string ClosingBalance { get; set; }
        [Display(Name = "Inc.0 Trn.")]
        public bool Include0Trn { get; set; }
        [Display(Name = "Inc.MTF Code Bal")]
        public bool IncludeMTFCodebal { get; set; }
        [Display(Name = "Bg FD Amt.")]
        public bool BgFDRamt { get; set; }
        [Display(Name = "Show Last Margin")]
        public bool Showlastmargin { get; set; }
        [Display(Name = "Inc.Early Payin")]
        public bool IncludeEarlypayin { get; set; }
        [Display(Name = "Last Margin")]
        public bool Lastmargin { get; set; }
        [Display(Name = "Last Peak Margin")]
        public bool Lastpeakmargin { get; set; }
        [Display(Name = "Ignore Opt.MTM Bal")]
        public bool IgnoreOptMTMBal { get; set; }
        [Display(Name = "Demat Value")]
        public bool Dematvalue { get; set; }
        [Display(Name = "Inc.Margin Ac")]
        public bool IncludemarginAc { get; set; }


        [Display(Name = "Daily Margin Balance")]
        public string DailyMarginBalance { get; set; }

        [Display(Name = "Mtm A/c Bal")]
        public string MtmAcBal { get; set; }
        [Display(Name = "UnReleased Voucher Bal")]
        public bool UnReleasedVoucherBal { get; set; }

        [Display(Name = "Cash Margin Bal")]
        public string CashMarginBal { get; set; }

        [Display(Name = "Closing Bal Other Bal")]
        public string ClosingBalOtherBal { get; set; }



        [Required]
        [Display(Name = "Search.Narr")]
        public string SearchNarration { get; set; }

        [Required]
        [Display(Name = "Trx Type")]
        public enumFinancialTranxactionType FinancialTranxactionType { get; set; }

        public FinancialLedgerOutput financialLedgerOutputs { get; set; }
    }

    public enum enumFinancialTranxactionType
    {
        All,
        AdditionalMargin,
        Margin,
        BankEntrys,
        BankReceive,
        BankPayment,
        CashBook,
        JournalEntrys,
        FutureEntries,
        VoucherAll,
        CUBills,
        DailyMTMBills,
        OptionBills,
        ExpiryBills

    }

    //public class GetExchange
    //{
    //    private string v;

    //    public GetExchange(string v)
    //    {
    //        this.v = v;
    //    }

    //    public string Exchange { get; set; }

    //}

}