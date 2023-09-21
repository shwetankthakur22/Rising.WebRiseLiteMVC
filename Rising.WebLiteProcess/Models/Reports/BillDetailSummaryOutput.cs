using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Rising.WebRise.Models
{

    public class BillDetailSummaryOutput
    {
     
        public DateTime TrDate { get; set; }
        public DateTime ToDate { get; set; }
        public string ClientCodeFrom { get; set; }
        public string ClientName { get; set; }

        public List<BillDetailSummaryOutputRow> listBillDetailSummaryOutputRow { get; set; }
    }


    public class BillDetailSummaryOutputRow
    {
        public string Client { get; set; }
        public string ClientName { get; set; }
        public string Branch { get; set; }
        public string GroupClient { get; set; }
        public string BillNo { get; set; }
        public string Exchange { get; set; }
        public decimal GrossAmount { get; set; }
        public decimal Brok { get; set; }
        public decimal ServiceTax { get; set; }
        public decimal TrxnTax { get; set; }
        public string StampDuty { get; set; }
        public string Tax1 { get; set; }
        public string STaxon { get; set; }
        public string STaxTax1 { get; set; }
        public string Tax2 { get; set; }
        public string Tax3 { get; set; }
        public string Tax4 { get; set; }
        public string STaxTax2 { get; set; }
        public string SBCTax { get; set; }
        public string KKCTax { get; set; }
        public decimal Debit { get; set; }
        public decimal Credit { get; set; }
        public string GST { get; set; }
        public string NseTax { get; set; }
        public decimal TurnOver { get; set; }
        public string SttAmt { get; set; }


    }




}
