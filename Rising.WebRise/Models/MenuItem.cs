using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rising.WebRise.Models
{
    public class MenuItem
    {
        public string Key { get; set; }
        public string MenuName { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public string Group { get; set; }       

        public string Logo { get; set; }
        public bool isView { get; set; }
        public bool isEdit { get; set; }
        public bool isDelete { get; set; }
        public bool isExport { get; set; }   
        public List<MenuItem> SubMenuItems { get; set; }
        public override string ToString()
        {
            return this.Group+ ">>" + this.MenuName;
        }
    }
}