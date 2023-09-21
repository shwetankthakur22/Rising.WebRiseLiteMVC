using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rising.WebRise.Models
{
    public class BillDetails
    {
        public string EXCHANGE { get; set; }
        public string GR { get; set; }
        public string BILLDATE { get; set; }
        public string COMPANY { get; set; }
        public string ADD1 { get; set; }
        public string ADD2 { get; set; }
        public string ADD3 { get; set; }
        public string ADD4 { get; set; }
        public string PAR_CODE { get; set; }
        public string PAR_NAME { get; set; }
        public string PAR_ADD1 { get; set; }
        public string PAR_ADD2 { get; set; }
        public string PAR_ADD3 { get; set; }
        public string PAR_ADD4 { get; set; }
        public string BILLNO { get; set; }
        public string BDATE { get; set; }
        public string CONTRACT { get; set; }
        public string TRADE_DATE { get; set; }
        public string OPTIONTYPE { get; set; }
        public string MULTIPLIER { get; set; }
        public string TRN_QTY { get; set; }
        public string TRADEPRICE { get; set; }
        public string STRIKEPRICE { get; set; }
        public string NETPRICE { get; set; }
        public string TRADESTATUS { get; set; }
        public string DIFF { get; set; }
        public string PVALUE { get; set; }
        public string SVALUE { get; set; }
        public string STAX { get; set; }
        public string NSETAX { get; set; }
        public string STAMP { get; set; }
        public string STAX_TURN { get; set; }
        public string STAX_STAMP { get; set; }
        public string TAX1 { get; set; }
        public string TAX2 { get; set; }
        public string TAX3 { get; set; }
        public string TAX4 { get; set; }
        public string TAX1NARR { get; set; }
        public string TAX2NARR { get; set; }
        public string TAX3NARR { get; set; }
        public string TAX4NARR { get; set; }
        public string TAX1CONTROL { get; set; }
        public string TAX2CONTROL { get; set; }
        public string TAX3CONTROL { get; set; }
        public string TAX4CONTROL { get; set; }
        public string STAMPDUTYNARR { get; set; }
        public string SBC_STAX { get; set; }
        public string KKC_STAX { get; set; }

        public List<BillDetails> bd = new List<BillDetails>();

    }

   

}