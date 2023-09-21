using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Rising.WebRise.Models
{
    public class MenuItem
    {

        //public MenuItem(string key, string mname, string trg, string logo, string pname, List<string> exch, List<string> seg)
        //{
        //    this.PageUrl = key;
        //    this.MenuName = mname;
        //    this.Target = trg;
        //    this.Logo = logo;
        //    this.ParentMenuName = pname;
        //    this.Exchanges = exch;
        //    this.Segments = seg;
        //    this.isView = true;
        //    this.isEdit = true;
        //    this.isDelete = true;
        //    this.isExport = true;
        //}
        public string PageUrl { get; set; }
        public string Key { get; set; }
        public string Target { get; set; }
        public string MenuName { get; set; }
        public string ControllerName { get; set; }
        public string ActionName { get; set; }
        public string Group { get; set; }
        public string ParentMenuName { get; set; }
        public string Logo { get; set; }
        public bool isView { get; set; }
        public bool isEdit { get; set; }
        public bool isDelete { get; set; }
        public bool isExport { get; set; }
        public List<string> Exchanges { get; set; }
        public List<string> Segments { get; set; }
        public List<MenuItem> SubMenuItems { get; set; }
        public override string ToString()
        {
            return this.Group+ ">>" + this.MenuName;
        }
    }

    public class _Exchange
    {
        public _Exchange(string exch, string seg, string catg)
        {
            this.Exchange = exch;
            this.Segment = seg;
            this.Category = catg;
        }

        public string Exchange { get; set; }
        public string Segment { get; set; }
        public string Category { get; set; }

        public override string ToString()
        {
            return this.Exchange + ">>" + this.Category + ">>" + this.Category;
        }
    }

}