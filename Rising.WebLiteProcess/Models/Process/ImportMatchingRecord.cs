using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rising.WebRise.Models.Process
{
    public class ImportMatchingRecord
    {
        public string symbol { get; set; }
        public string expirydate { get; set; }
        public string ourmtm { get; set; }
        public string exchmtm { get; set; }
        public string CLIENTCODE { get; set; }
        public string EXCHANGE { get; set; }
        public string sessionid { get; set; }
        public string PDATE { get; set; }
        public List<ImportMatchingRecordRow> lstImportMatchingrecordRow { get; set; }
    }
    public class ImportMatchingRecordRow
    {
        public string symbol { get; set; }
        public string expirydate { get; set; }
        public string ourmtm { get; set; }
        public string exchmtm { get; set; }
        public string CLIENTCODE { get; set; }
        public string EXCHANGE { get; set; }
        public string sessionid { get; set; }
        public string PDATE { get; set; }
    }
}