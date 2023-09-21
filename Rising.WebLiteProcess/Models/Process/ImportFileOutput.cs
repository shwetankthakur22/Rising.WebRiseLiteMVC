using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Rising.WebRise.Models
{
    public class ImportFileOutputRow
    {

        public string FilePath { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime TradeDate { get; set; }

        [Display(Name = "Import Basis")]
        public string ImportBasis { get; set; }

        public string FileType { get; set; }

        public string Actual_Code { get; set; }

        public string Exchange { get; set; }

        public string FileName { get; set; }

        public string Session { get; set; }

        public string Records { get; set; }

        [Display(Name = "Pro Code")]
        public string procode { get; set; }
        [Display(Name = "Broker Code")]
        public string brokercode { get; set; }

        [Display(Name = "Calculate Brokerage")]
        public bool calbrok { get; set; }

        [Display(Name = "User Id Wise Transfer")]
        public string useridwt { get; set; }

        public string Imported { get; set; }

        public string Rejected { get; set; }
        [Display(Name = "Member ID")]
        public string memberid { get; set; }

        public string File { get; set; }

        [Required]
        [Display(Name = "Symbol")]
        // public string SymbolList { get; set; }

        public List<WebRiseProcess.Models.Symbol> SymbolList { get; set; }
        //[Display(Name = "Expiry Date")]
        //public DateTime ExpiryDate { get; set; }


        [Display(Name = "Client Code")]
        public string ClientCode { get; set; }


        [Display(Name = "Session Id")]
        public string SessionId { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime Date { get; set; }

        [Display(Name = "Delete Only")]
        public bool DeleteOnly { get; set; }

        public DateTime POSITIONDATE { get; set; }

        public string SEGMENT_INDICATOR { get; set; }

        public string SETTLEMENT_TYPE { get; set; }

        public string CLEARING_MEMBER_CODE { get; set; }

        public string MEMBER_TYPE { get; set; }

        public string TRADING_MEMBER_CODE { get; set; }

        public string ACCOUNT_TYPE { get; set; }

        public string CLIENT_ACCOUNTCODE { get; set; }

        public string INSTRUMENT_TYPE { get; set; }

        public string SYMBOL { get; set; }

        public DateTime EXPIRY_DATE { get; set; }

        public string STRIKE_PRICE { get; set; }

        public string OPTION_TYPE { get; set; }

        public string CA_LEVEL { get; set; }

        public string BROUGHT_FORWARD_LONG_QUANTITY { get; set; }

        public string BROUGHT_FORWARD_LONG_VALUE { get; set; }

        public string BROUGHT_FORWARD_SHORT_QUANTITY { get; set; }

        public string BROUGHT_FORWARD_SHORT_VALUE { get; set; }

        public string DAY_BUY_OPEN_QUANTITY { get; set; }

        public string DAY_BUY_OPEN_VALUE { get; set; }

        public string DAY_SELL_OPEN_QUANTITY { get; set; }

        public string DAY_SELL_OPEN_VALUE { get; set; }

        public string PRE_EXASSGN_LONG_QUANTITY { get; set; }

        public string PRE_EXASSGN_LONG_VALUE { get; set; }

        public string PRE_EXASSGN_SHORT_QUANTITY { get; set; }

        public string PRE_EXASSGN_SHORT_VALUE { get; set; }

        public string EXERCISED_QUANTITY { get; set; }

        public string ASSIGNED_QUANTITY { get; set; }

        public string POST_EXASSGN_LONG_QUANTITY { get; set; }

        public string POST_EXASSGN_LONG_VALUE { get; set; }

        public string POST_EXASSGN_SHORT_QUANTITY { get; set; }

        public string POST_EXASSGN_SHORT_VALUE { get; set; }

        public string SETTLEMENT_PRICE { get; set; }

        public string NET_PREMIUM { get; set; }

        public string DAILY_MTM_SETTLEMENT_VALUE { get; set; }
        public string ourmtm { get; set; }
        public string FUTURES_FINAL_SETTLEMENT_VALUE { get; set; }

        public string EXERCISEDASSIGNED_VALUE { get; set; }
        public string exchmtm { get; set; }

    }


    public class ImportFileOutput
    {
        public string FilePath { get; set; }
        public string ourmtm { get; set; }
        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime TradeDate { get; set; }
        public string exchmtm { get; set; }

        [Display(Name = "Import Basis")]
        public string ImportBasis { get; set; }

        public string FileType { get; set; }

        public string Actual_Code { get; set; }

        public string Exchange { get; set; }

        public string FileName { get; set; }

        public string Session { get; set; }

        public string Records { get; set; }

        [Display(Name = "Pro Code")]
        public string procode { get; set; }
        [Display(Name = "Broker Code")]
        public string brokercode { get; set; }

        [Display(Name = "Calculate Brokerage")]
        public bool calbrok { get; set; }

        [Display(Name = "User Id Wise Transfer")]
        public string useridwt { get; set; }

        public string Imported { get; set; }

        public string Rejected { get; set; }
        [Display(Name = "Member ID")]
        public string memberid { get; set; }

        public string File { get; set; }

        [Required]
        [Display(Name = "Symbol")]
        // public string SymbolList { get; set; }

        public List<WebRiseProcess.Models.Symbol> SymbolList { get; set; }
        //[Display(Name = "Expiry Date")]
        //public DateTime ExpiryDate { get; set; }


        [Display(Name = "Client Code")]
        public string ClientCode { get; set; }


        [Display(Name = "Session Id")]
        public string SessionId { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime Date { get; set; }

        [Display(Name = "Delete Only")]
        public bool DeleteOnly { get; set; }

        public DateTime POSITIONDATE { get; set; }

        public string SEGMENT_INDICATOR { get; set; }

        public string SETTLEMENT_TYPE { get; set; }

        public string CLEARING_MEMBER_CODE { get; set; }

        public string MEMBER_TYPE { get; set; }

        public string TRADING_MEMBER_CODE { get; set; }

        public string ACCOUNT_TYPE { get; set; }

        public string CLIENT_ACCOUNTCODE { get; set; }

        public string INSTRUMENT_TYPE { get; set; }

        public string SYMBOL { get; set; }

        public DateTime EXPIRY_DATE { get; set; }

        public string STRIKE_PRICE { get; set; }

        public string OPTION_TYPE { get; set; }

        public string CA_LEVEL { get; set; }

        public string BROUGHT_FORWARD_LONG_QUANTITY { get; set; }

        public string BROUGHT_FORWARD_LONG_VALUE { get; set; }

        public string BROUGHT_FORWARD_SHORT_QUANTITY { get; set; }

        public string BROUGHT_FORWARD_SHORT_VALUE { get; set; }

        public string DAY_BUY_OPEN_QUANTITY { get; set; }

        public string DAY_BUY_OPEN_VALUE { get; set; }

        public string DAY_SELL_OPEN_QUANTITY { get; set; }

        public string DAY_SELL_OPEN_VALUE { get; set; }

        public string PRE_EXASSGN_LONG_QUANTITY { get; set; }

        public string PRE_EXASSGN_LONG_VALUE { get; set; }

        public string PRE_EXASSGN_SHORT_QUANTITY { get; set; }

        public string PRE_EXASSGN_SHORT_VALUE { get; set; }

        public string EXERCISED_QUANTITY { get; set; }

        public string ASSIGNED_QUANTITY { get; set; }

        public string POST_EXASSGN_LONG_QUANTITY { get; set; }

        public string POST_EXASSGN_LONG_VALUE { get; set; }

        public string POST_EXASSGN_SHORT_QUANTITY { get; set; }

        public string POST_EXASSGN_SHORT_VALUE { get; set; }

        public string SETTLEMENT_PRICE { get; set; }

        public string NET_PREMIUM { get; set; }

        public string DAILY_MTM_SETTLEMENT_VALUE { get; set; }

        public string FUTURES_FINAL_SETTLEMENT_VALUE { get; set; }

        public string EXERCISEDASSIGNED_VALUE { get; set; }

        public List<ImportFileOutputRow> lstImportFileOutputRow { get; set; }

    }
}