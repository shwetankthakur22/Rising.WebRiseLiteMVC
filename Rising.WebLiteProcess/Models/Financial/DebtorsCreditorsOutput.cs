using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rising.WebRise.Models
{
    public class DebtorsCreditorsOutputRow
    {
        public DateTime OnDate { get; set; }        
        public string Branch { get; set; }
        public string Code { get; set; }
        public string Name { get; set; }
        public string Debit { get; set; }
        public string Credit { get; set; }
        public string Balance { get; set; }
        public string Exchange { get; set; }
      
    }

    public class DebtorsCreditorsOutput
    {

        public string Exchange { get; set; }
        public string AccountGroup { get; set; }
        public DateTime OnDate { get; set; }
        public List<DebtorsCreditorsOutputRow> listDebtorsCreditorsOutputRow { get; set; }
    }
}
