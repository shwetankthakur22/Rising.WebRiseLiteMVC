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
    public class SecurityController : Controller
    {
        string dbuser = ConfigurationManager.AppSettings["DBUSER"];
        

        [HttpGet]
        public ActionResult ScripDetails()
        {
            ScripDetails model = new ScripDetails();
            return View(model);
        }


        [HttpGet]
        public ActionResult StockEntryModification()
        {
            StockEntryModification model = new StockEntryModification();
            model.Date = DateTime.Parse(Session["FinYearFrom"].ToString());    
            return View(model);
        }

        [HttpGet]
        public ActionResult StockStatus()
        {
            StockEntryModification model = new StockEntryModification();
            model.DateFrom = DateTime.Parse(Session["FinYearFrom"].ToString());
            model.DateTo = DateTime.Parse(Session["FinYearTo"].ToString());
            return View(model);
        }

        [HttpGet]
        public ActionResult ClosingRateImport()
        {
            return View();
        }

        [HttpGet]
        public ActionResult StockValuationDateRange()
        {
            StockEntryModification model = new StockEntryModification();
            model.DateFrom = DateTime.Parse(Session["FinYearFrom"].ToString());
            model.DateTo = DateTime.Parse(Session["FinYearTo"].ToString());
            return View(model);
        }

        [HttpGet]
        public ActionResult StockValuationAson()
        {
            StockEntryModification model = new StockEntryModification();
            model.AsOn = DateTime.Parse(Session["FinYearFrom"].ToString());
            model.ClosRateDate = DateTime.Parse(Session["FinYearTo"].ToString());
            return View(model);
        }


    }
}