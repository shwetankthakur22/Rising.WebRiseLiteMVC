using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rising.WebRise.Models
{
    public class FinancialLedgerOutputRow
    {
        public DateTime Date { get; set; }

        public DateTime ValueDate { get; set; }
        public string Narration { get; set; }
        public string BillNo { get; set; }
        public string SETTNO { get; set; }
        public string Debit { get; set; }
        public string Credit { get; set; }
        public string RUNBAL { get; set; }
        public string Segment { get; set; }
        public string ClientCode { get; set; }
        public string ClientName { get; set; }
        public int slno { get; set; }
        //public string Query { get; set; }
    }

    public class FinancialLedgerOutput
    {
        public string OpeningBalance { get; set; }
        public string ClosingBalance { get; set; }
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }
        public bool IncludeUnReleaseVoucher { get; set; }

        public string ClientCode { get; set; }

       // public enumexchange Exchange { get; set; }
        public List<FinancialLedgerOutputRow> listFinancialLedgerOutputRow { get; set; }
    }
}
