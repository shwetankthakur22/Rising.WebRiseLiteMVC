using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rising.WebRise.Models
{
    public class DBList
    {
        public string DBName { get; set; }
        public string FinDBUser { get; set; }
        public string OtherDBUser { get; set; }
        public string Exchange { get; set; }
        public bool Visible { get; set; }
        public string Group { get; set; }
        public string CompGroup { get; set; }


        public override string ToString()
        {
            return this.Group + ">>" + this.DBName;
        }
    }
}