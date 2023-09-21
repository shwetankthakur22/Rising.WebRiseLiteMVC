using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rising.WebRise.Models
{
    public class DPTrnsactionOutputRow
    {
        public DateTime Date { get; set; }        
        public string Narration { get; set; }
        public string RefNo { get; set; }
        public string ISIN { get; set; }
        public string ShareName { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public string ClientCode { get; set; }
        public int slno { get; set; }
    }

    public class DPTrnsactionOutput
    {
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public string ClientCode { get; set; }
        public string ClientName { get; set; }
        
        public List<DPTrnsactionOutputRow> listDPTrnsactionOutputRow { get; set; }
    }
}
