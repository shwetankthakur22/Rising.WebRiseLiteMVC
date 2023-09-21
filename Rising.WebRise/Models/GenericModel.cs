using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Data;
using System;

namespace Rising.WebRise.Models
{  
    public class GenericModel
    {
        public DateTime AsOnDate { get; set; }

        public DateTime DateFrom { get; set; }
               
        public DateTime DateTo { get; set; }

        public string ClientCode { get; set; }

        public DataSet dsOut { get; set; }

        public DataSet dsOut1 { get; set; }

        public DataSet dsOut2 { get; set; }

        public DataSet dsOut3 { get; set; }

        public DataSet dsOut4 { get; set; }

        public DataSet dsOut5 { get; set; }

        public string Type { get; set; }

        public string Day1 { get; set; }
        public string Day2 { get; set; }
        public string Day3 { get; set; }
        public string Day4 { get; set; }
    }
}
