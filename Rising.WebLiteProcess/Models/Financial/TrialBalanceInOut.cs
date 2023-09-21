using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data;


namespace Rising.WebRise.Models
{
    public class TrialBalanceInOut
    {
       
        public DateTime DateFrom { get; set; }

       
        public DateTime DateTo { get; set; }

        public DataSet dsOut { get; set; }
    }
}