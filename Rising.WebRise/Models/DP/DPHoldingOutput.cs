using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rising.WebRise.Models
{
    public class DPHoldingOutput
    {
    
        public DateTime AsOnDate { get; set; }       
        public string ClientCode { get; set; }
        public string ClientName { get; set; }
       
        public List<DPHoldingOutputRow> listDPHoldingOutputRow { get; set; }
    }


    public class DPHoldingOutputRow
    {     
        public string ScripName { get; set; }
        public string ScripISIN { get; set; }

        public int Pledge { get; set; }
        public int Free { get; set; }
        public int Demat { get; set; }
        public int Lockin { get; set; }
        public int Emark { get; set; }
        public int NetStock { get; set; }
    }
}