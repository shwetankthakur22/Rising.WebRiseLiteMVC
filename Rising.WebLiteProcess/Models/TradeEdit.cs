using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;
using System.Data;


namespace Rising.WebRise.Models
{  
    public class TradeEdit
    {  
        public string SettNo { get; set; }

        public DateTime TradeDate { get; set; }

        public string UserID { get; set; }

        public string CtclID { get; set; }

        public string ClientCodeFrom { get; set; }

        public string ScripCode { get; set; }

        public List<TradeEditRow> TradeEditRows { get; set; }

    }

    public class TradeEditRow
    {
        public string ClientCode { get; set; }

        public string ClientName { get; set; }

        public string ScripCode { get; set; }

        public string ScripName { get; set; }

        public float Qty { get; set; }

        public float MktRate { get; set; }

        public float NetRate { get; set; }

        public float OrderNo { get; set; }

        public float TradeNo { get; set; }

        public DateTime OrderTime { get; set; }

        public DateTime TradeTime { get; set; }

        public string OrgClient { get; set; }

        public string UserID { get; set; }

        public string CtclID { get; set; }

        public string RowID { get; set; }
    }
}
