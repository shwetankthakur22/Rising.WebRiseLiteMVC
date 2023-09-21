using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Rising.WebRise.Models
{  
    public class TMMaster
    {
        

        public string TMId { get; set; }

        public string UccCode { get; set; }

        public string ClientCodeFrom { get; set; }

        public string ClientName { get; set; }

        public string DPCodePhy { get; set; }

        public string DPCodePool { get; set; }

        public string CBMPID { get; set; }

        public string DPAcnoPhy { get; set; }

        public string DPAcnoPool { get; set; }

        public string ClientCodeCash { get; set; }

        public string Rwid { get; set; }

        public System.Data.DataSet result { get; set; }
     
        public string Exchange { get; set; }

        


    }
}
