using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rising.WebRise.Models
{
    public class DematHoldingOutput
    {
    
        public DateTime AsOnDate { get; set; }       
        public string ClientCode { get; set; }
        public string ClientName { get; set; }
       
        public List<DematHoldingOutputRow> listDematHoldingOutputRow { get; set; }
    }


    public class DematHoldingOutputRow
    {     
        public string ScripName { get; set; }
        public string ScripCode { get; set; }
        public string ScripIsin { get; set; }
        public decimal Qty { get; set; }
        public decimal Stock { get; set; }
        public decimal LockQty { get; set; }
        public decimal CDSLQty { get; set; }
        public decimal NSDLQty { get; set; }
        public decimal TotalQty { get; set; }
        public decimal Rate { get; set; }
        public decimal Value { get; set; }
        public decimal VarPer { get; set; }
        public decimal VarValue { get; set; }
    }
}