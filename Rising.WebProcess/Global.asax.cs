using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;



namespace Rising.WebRise
{
    using Rising.OracleDBHelper;
    using Rising.WebRise.Models;

    public class MvcApplication : System.Web.HttpApplication
    {       
        protected void Application_Start()
        {
            AreaRegistration.RegisterAllAreas();
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);
            ViewDic = new Dictionary<string, object>();
        }

        //----------------globals items-------------------------
        public static string CompanyName { get; set; }
        public static DateTime FinYearFrom { get; set; }
        public static DateTime FinYearTo { get; set; }
        public static string CurrentDBUser { get; set; }
        public static WebUser WebUser { get; set; }
        public static List<DBList> SelectedDBLists { get; set; }



        //--------------fix items
        public static List<DBList> DBLists { get; set; }
        public static List<MenuItem> MenuItems { get; set; }
        public static Dictionary<string, object> ViewDic { get; set; }
        public static OracleDBHelperCore __OracleDBHelperCore { get; set; }
        public static OracleDBHelperCore OracleDBHelperCore()
        {
            if (__OracleDBHelperCore == null) __OracleDBHelperCore = new OracleDBHelperCore("MainConn", "MainConn2", "MainConn3");
            return __OracleDBHelperCore;
        }

        //------------------------------------------------------------------
        protected void Session_Start()
        {
        }
        //------------------------------

       
    }
}
