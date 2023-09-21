using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.IO;
using System.Configuration;



namespace Rising.WebRise.Controllers
{
    using Rising.WebRise.Models;
    using OracleDBHelper;


    public class ContractNoteController : Controller
    {
        // GET: ContractNote
        public ActionResult Index()
        {
            WebUser webUser = Session["WebUser"] as WebUser;
            if (webUser == null) return null;

            //for date range
            ContractNoteInput model = new ContractNoteInput();
            model.DateFrom = DateTime.Parse(Session["FinYearFrom"].ToString());
            model.DateTo = System.DateTime.Now;
           
            return View(model);
        }

        public ActionResult ContractNoteList(ContractNoteInput model)
        {
            WebUser webUser = Session["WebUser"] as WebUser;
            if (webUser == null) return null;

            if (webUser.UserType == UserType.Client)
            {
                model.ClientCodeTo = model.ClientCodeFrom;

                Dictionary<string, string> tempList = GetAllFiles(webUser.UserID, model.DateFrom, model.DateTo);
                ViewBag.DataList = tempList;
                return View();
            }
            else if (webUser.UserType == UserType.Branch)
            {
                model.ClientCodeTo = model.ClientCodeFrom;

                DataSet ds1 = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet("SELECT * FROM sysadm.partymst where par_code='" + model.ClientCodeFrom + "' and branchind='" + webUser.UserID + "'", Session["SelectedConn"].ToString());
                if (ds1.Tables[0].Rows.Count == 0)
                {
                    TempData["AlertMessage"] = "Invalid Code : " + model.ClientCodeFrom;
                    return RedirectToAction("Index", "ContractNote");
                }

                Dictionary<string, string> tempList = GetAllFiles(model.ClientCodeFrom, model.DateFrom, model.DateTo);
                ViewBag.DataList = tempList;
                return View();
            }

            else if (webUser.UserType == UserType.RM)
            {
                model.ClientCodeTo = model.ClientCodeFrom;

                DataSet ds1 = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet("SELECT * FROM sysadm.partymst where par_code='" + model.ClientCodeFrom + "' and rmcode='" + webUser.UserID + "'", Session["SelectedConn"].ToString());
                if (ds1.Tables[0].Rows.Count == 0)
                {
                    TempData["AlertMessage"] = "Invalid Code : " + model.ClientCodeFrom;
                    return RedirectToAction("Index", "ContractNote");
                }

                Dictionary<string, string> tempList = GetAllFiles(model.ClientCodeFrom, model.DateFrom, model.DateTo);
                ViewBag.DataList = tempList;
                return View();
            }
            else if (webUser.UserType == UserType.Admin)
            {
                model.ClientCodeTo = model.ClientCodeFrom;

                Dictionary<string, string> tempList = GetAllFiles(model.ClientCodeFrom, model.DateFrom, model.DateTo);
                ViewBag.DataList = tempList;
                return View();
            }


            return RedirectToAction("Index", "ContractNote");
        }

        public FileResult Download(string filePath, string fileName)
        {
            string cAbsPath = ConfigurationManager.AppSettings["ContractNotaAbsolutePath"];
            var file = File(cAbsPath + filePath, System.Net.Mime.MediaTypeNames.Text.Html, fileName + ".html");
            return file;
        }


        public Dictionary<string, string> GetAllFiles(string code, DateTime sDate, DateTime eDate)
        {
            List<String> tmpList = new List<string>();           
            string cAbsPath = ConfigurationManager.AppSettings["ContractNotaAbsolutePath"];

            //string path = Server.MapPath("~/"+ cAbsPath);
            tmpList = Directory.GetFiles(cAbsPath, "*_" + code + "_*.html", SearchOption.AllDirectories).ToList();

            Dictionary<string, string> finalList = new Dictionary<string, string>();
            foreach (string itm in tmpList)
            {
                if(itm.Split('_').Count()>2)
                {
                    try
                    {
                        string itm1 = itm.Replace(cAbsPath, ""); itm1 = itm1.Split('_')[3];
                        itm1 = itm1.Replace(".html", "");
                        DateTime dt = DateTime.ParseExact(itm1, "ddMMyyyy", System.Globalization.CultureInfo.InvariantCulture);
                        if (dt >= sDate && dt <= eDate) finalList.Add(itm1, itm.Replace(cAbsPath, ""));
                    }
                    catch 
                    {
                        
                    }
                   
                }
            }
            return finalList;
        }
    }
}