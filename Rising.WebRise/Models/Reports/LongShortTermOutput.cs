using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;

namespace Rising.WebRise.Models
{
    public class LongShortTermOutput
    {
        public DateTime DateFrom { get; set; }
        public DateTime DateTo { get; set; }       
        public string ClientCode { get; set; }
        public string ClientName { get; set; }   
       
        public DataSet DsOut1 { get; set; }
        public DataSet DsOut2 { get; set; }
        public DataSet DsOut3 { get; set; }
        public DataSet DsOut4 { get; set; }
        public DataSet DsOut5 { get; set; }
        public System.Data.DataSet result { get; set; }
    }
}