using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Web.Mvc;
using System.Diagnostics;
using System.Management;
using Rising.WebRise.Models;
using System.Data;
using System.Configuration;

namespace Rising.WebRise.Controllers
{
    public class BillsController : Controller
    {
        string dbuser = ConfigurationManager.AppSettings["DBUSER"];
        

        [HttpGet]
        public ActionResult DailyMTMBills()
        {
            Bills model = new Bills();
            model.TrDateFrom = DateTime.Parse(Session["FinYearFrom"].ToString());
            model.TrDateTo = DateTime.Parse(Session["FinYearTo"].ToString());
            return View(model);
        }


        [HttpGet]
        public ActionResult FutureExpiryBills()
        {
            Bills model = new Bills();
            model.TrDateFrom = DateTime.Parse(Session["FinYearFrom"].ToString());
            model.TrDateTo = DateTime.Parse(Session["FinYearTo"].ToString());
            return View(model);
        }

  

    }
}