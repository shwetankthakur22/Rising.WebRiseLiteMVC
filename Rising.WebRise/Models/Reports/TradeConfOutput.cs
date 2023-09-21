using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rising.WebRise.Models
{
    public class TradeConfOutput
    {
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }       
        public string ClientCode { get; set; }
        public string ClientName { get; set; }   
       
        public List<TradeConfOutputRow> listTradeConfOutputRow { get; set; }
    }


    public class TradeConfOutputRow
    {
        public string Flag { get; set; }

        public string ScripName { get; set; }
        public string ScripCode { get; set; }
        public string ScripIsin { get; set; }

        public DateTime TradeDate { get; set; }
        public DateTime TradeTime { get; set; }
        public DateTime OrderTime { get; set; }
        public string TradeNo { get; set; }
        public string OrderNo { get; set; }

        public int Qty { get; set; }
        public decimal TradeRate { get; set; }
        public decimal NetRate { get; set; }
        public decimal TradeValue { get; set; }

        public decimal Brokrage { get; set; }
    }
}