using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rising.WebRise.Models
{
    public class NetPositionOutput
    {
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public DateTime DateClosing { get; set; }
        public bool AsOnDate { get; set; }

        public DateTime ActualDateClosing { get; set; }

        public bool isCapital { get; set; }

        public string ClientCode { get; set; }
        public string ClientName { get; set; }      

        public decimal NSETax { get; set; } 
        public decimal StampDuty { get; set; }
        public decimal Brok { get; set; }
        public decimal Demat { get; set; }
        public decimal Tax1 { get; set; }
        public decimal Tax2 { get; set; }
        public decimal Tax3 { get; set; }
        public decimal STTAmt { get; set; }
        public decimal CGST { get; set; }
        public decimal SGST { get; set; }
        public decimal IGST { get; set; }
        public List<NetPositionOutputRow> listNetPositionOutputRow { get; set; }
    }


    public class NetPositionOutputRow
    {
        public string Flag { get; set; }

        public string ScripName { get; set; }
        public string ScripExpiry { get; set; }
        public string ScripOption { get; set; }
        public string ScripStrike { get; set; }
        public string ScripInstrument { get; set; }
        public string ScripCode { get; set; }

        public decimal BFQty { get; set; }
        public decimal BFAvgRate { get; set; }
        public decimal BFValue { get; set; }

        public decimal SaleQty { get; set; }
        public decimal SaleAvgRate { get; set; }
        public decimal SaleValue { get; set; }
               
        public decimal PurQty { get; set; }
        public decimal PurAvgRate { get; set; }
        public decimal PurValue { get; set; }

        
        public decimal NetQty { get; set; }
        public decimal NetAvgRate { get; set; }
        public decimal NetValue { get; set; }

        public decimal CloseRate { get; set; }
        public decimal MTM { get; set; }

        public string TradeDate { get; set; }

    }
}