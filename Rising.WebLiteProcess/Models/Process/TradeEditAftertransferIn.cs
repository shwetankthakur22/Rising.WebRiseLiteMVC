using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;


namespace Rising.WebRise.Models
{
    public class TradeEditAftertransferIn
    {
        public string Exchange { get; set; }
        public DateTime TradeDate { get; set; }

        [Display(Name = "Session ID")]
        public string Session { get; set; }

        [Display(Name = "Broker Code")]
        public string brokercode { get; set; }


        public string Index { get; set; }
        public string Contract { get; set; }
        public string Custodian { get; set; }
        [Display(Name = "CTCL ID")]
        public string CtclId { get; set; }
        public string Search { get; set; }
        [Display(Name = "User ID")]
        public string userid { get; set; }
        [Display(Name = "Buy Trxn")]
        public string buytrxn { get; set; }
        [Display(Name = "Sale Trxn")]
        public string saletrxn { get; set; }
        public string All { get; set; }
        [Display(Name = "Option Type")]
        public string option { get; set; }
        [Display(Name = "Trade Price")]
        public string tradepp { get; set; }
        [Display(Name = "Order By")]
        public string orderby { get; set; }
        [Display(Name = "Original Client ID")]
        public string originalcid { get; set; }
        [Display(Name = "Buy Qty")]
        public string buyqty { get; set; }
        [Display(Name = "Sale Qty")]
        public string saleqty { get; set; }

        [Display(Name = "Replace Code With")]
        public string replacecode { get; set; }

        [Display(Name = "Show Null Client Code")]
        public string showClient { get; set; }
        public string Client { get; set; }
        [Display(Name = "Show CM A/C Transactions")]
        public string showTransactions { get; set; }
    }
}