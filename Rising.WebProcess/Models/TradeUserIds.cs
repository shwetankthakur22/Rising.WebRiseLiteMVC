using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Rising.WebRise.Models
{  
    public class TradeUserIds
    {
        
        public string UserID { get; set; }

        public string BranchCode { get; set; }

        public string RecordType { get; set; }

        public string Station { get; set; }

        public string Proclient { get; set; }

        public string Remark { get; set; }

        public string FixedPrefix { get; set; }

        public string TR_OrderCV { get; set; }

        public string RemarkPro { get; set; }

        public string BrokerCode { get; set; }

        public string CTCLID { get; set; }

        public string Prefix { get; set; }

        public System.Data.DataSet result { get; set; }

        public bool deleteOnly { get; set; }
    }
}
