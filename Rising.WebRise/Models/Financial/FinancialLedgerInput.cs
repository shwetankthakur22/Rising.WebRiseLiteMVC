using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;
using System.Data;

namespace Rising.WebRise.Models
{
    public class FinancialLedgerInput
    {

        public DataSet dsOut { get; set; }

        [Required]
        [Display(Name = "Search ")]
        public List<string> Search { get; set; }
                

        [Required]
        [Display(Name = "Client Code")]
        public string ClientCodeFrom { get; set; }

        [Required]
        [Display(Name = "Code To")]
        public string ClientCodeTo { get; set; }

        [Required]
        [Display(Name = "Date From")]       
        public DateTime DateFrom { get; set; }

        [Required]
        [Display(Name = "Date To")]        
        public DateTime DateTo { get; set; }

        [Required]
        [Display(Name = "Include UnRelease Voucher")]
        public bool IncludeUnReleaseVoucher { get; set; }

        [Required]
        [Display(Name = "Exclude MG13 Entries")]
        public bool ExcludeMG13Entries { get; set; }

        [Required]
        [Display(Name = "Tranxaction Type")]
        public enumFinancialTranxactionType FinancialTranxactionType { get; set; }
    }

    public enum enumFinancialTranxactionType
    {      
        AllVouchers,
        BankVouchers,
        BankPaymentVouchers,
        BankReceiptVouchers,
        JournalVouchers,
        FutureEntries,
        OptionEntries,
        DifferenceBills,
        CombineBills,       
        DebitEntries,
        CreditEntries,
        AuctionEntries,
        SquaringEntries,
        SttOnly,
        SttNSE,
        SttBSE,
        SttFO
    }
}