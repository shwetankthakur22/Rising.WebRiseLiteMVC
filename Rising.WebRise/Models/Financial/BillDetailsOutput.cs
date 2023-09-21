using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rising.WebRise.Models
{
    public class BillDetailsOutputRow
    {
        public string ScripName { get; set; }
        public string ScripCode { get; set; }
        public decimal SaleValue { get; set; }
        public int SaleQty { get; set; }
        public decimal SaleMktRate { get; set; }
        public decimal SaleNetRate { get; set; }
        public decimal PurValue { get; set; }
        public int PurQty { get; set; }
        public decimal PurMktRate { get; set; }
        public decimal PurNetRate { get; set; }
        public string Flag { get; set; }
    }

    public class BillDetailsOutput
    {
        public DateTime TrnDate { get; set; }
        public string TrnDelvSettno { get; set; }
        public DateTime DelvDate { get; set; }
        public string BillNo { get; set; }
        public string ClientCode { get; set; }
        public string ClientName { get; set; }
        public string ClientAddress1 { get; set; }
        public string ClientAddress2 { get; set; }
        public string ClientAddress3 { get; set; }
        public string ClientAddress4 { get; set; }

        public decimal NSETax { get; set; }
        public decimal STaxNSETax { get; set; }
        public string NSETaxNarration { get; set; }

        public decimal StampDuty { get; set; }
        public decimal STaxStampDuty { get; set; }
        public string StampDutyNarration { get; set; }

        public decimal Brok { get; set; }
        public decimal STaxBrok { get; set; }
        public string BrokNarration { get; set; }

        public decimal Demat { get; set; }
        public decimal STaxDemat { get; set; }
        public string DematNarration { get; set; }

        public decimal Tax1 { get; set; }
        public decimal STaxTax1 { get; set; }
        public string Tax1Narration { get; set; }

        public decimal Tax2 { get; set; }
        public decimal STaxTax2 { get; set; }
        public string Tax2Narration { get; set; }

        public decimal Tax3 { get; set; }
        public decimal STaxTax3 { get; set; }
        public string Tax3Narration { get; set; }

        public decimal STTAmt { get; set; }
        public decimal CGST { get; set; }
        public decimal SGST { get; set; }
        public decimal IGST { get; set; }

        public string Header { get; set; }

        public decimal NetBalance { get; set; }

        public List<BillDetailsOutputRow> listBillDetailsOutputRow { get; set; }
    }
}
