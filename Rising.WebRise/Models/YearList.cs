using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rising.WebRise.Models
{
    public class YearList
    {
        public string YearName { get; set; }
        public string ConnName { get; set; }
        
        public override string ToString()
        {
            return this.YearName + ">>" + this.ConnName;
        }
    }
}