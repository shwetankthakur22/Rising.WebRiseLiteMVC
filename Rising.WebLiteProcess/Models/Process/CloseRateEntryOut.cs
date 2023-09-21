using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rising.WebRise.Models
{
    public class CloseRateEntryOutRow
    {

        public string SymbolList { get; set; }

        public string ContName { get; set; }

        public string PriceUnit { get; set; }

        public string SetTTPrice { get; set; }

        public string UM { get; set; }

        public string Exchange { get; set; }

        public string SessionId { get; set; }
        public DateTime TrDate { get; set; }

        public DateTime ExpiryDate { get; set; }
        public string Symbol { get; set; }
        public string ClosePrice { get; set; }
        public string StrikePrice { get; set; }
        public string OptionType { get; set; }
        public string InstrumentType { get; set; }
        public string Rate { get; set; }
        public string C { get; set; }
    }

    public class CloseRateEntryOut
    {

        public string SymbolList { get; set; }

        public string OptionType { get; set; }

        public string ContName { get; set; }

        public string PriceUnit { get; set; }

        public string SetTTPrice { get; set; }

        public string UM { get; set; }

        public string Exchange { get; set; }
        public DateTime ExpiryDate { get; set; }
        public string SessionId { get; set; }
        public DateTime TrDate { get; set; }
        public List<CloseRateEntryOutRow> listCloseRateEntryOutRow { get; set; }
    }
}
