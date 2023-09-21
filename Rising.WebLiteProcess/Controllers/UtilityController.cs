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
    public class UtilityController : Controller
    {
        string dbuser = ConfigurationManager.AppSettings["DBUSER"];

        [HttpGet]
        public ActionResult ExpiryUpdate()
        {

            if (Session["WebUser"] == null)
            {
                TempData["AlertMessage"] = "Session Time Out Please Login Again";
                return RedirectToAction("Index", "Login");

            }
            else
            {
                SelectList lst4 = new SelectList(Enum.GetValues(typeof(enumIndexList)).Cast<enumIndexList>().Select(v => v.ToString()).ToList());
                ViewBag.enumIndexList = lst4;
                List<Oracle.ManagedDataAccess.Client.OracleParameter> lst = new List<Oracle.ManagedDataAccess.Client.OracleParameter>();
                Transaction model = new Transaction();
                model.OldExpDate= DateTime.Parse(Session["FinYearFrom"].ToString());
                model.NewExpDate= DateTime.Parse(Session["FinYearTo"].ToString());
                return View(model);

            }


        }

        [HttpPost]
        public ActionResult ExpiryUpdate(Transaction model)
        {
            DataSet ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet("SELECT * from IFSC.cucontracts WHERE EXPIRYDATE=TO_DATE('" + model.OldExpDate.ToString("ddMMMyyyy") + "') AND EXCHANGE='" + model.Exchange + "' AND INSTRUMENT_TYPE='" + model.IndexList + "' ", Session["SelectedConn"].ToString());
            model.result = ds;

            if (ds.Tables[0].Rows.Count != 0)
            {
                MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteNonQuery("update IFSC.cucontracts set expirydate = to_date('" + model.NewExpDate.ToString("ddMMMyyyy") + "') WHERE EXPIRYDATE = TO_DATE('" + model.OldExpDate.ToString("ddMMMyyyy") + "') AND EXCHANGE = '" + model.Exchange + "' AND INSTRUMENT_TYPE = '" + model.IndexList + "' ", Session["SelectedConn"].ToString());

            }
            else
            {
                TempData["AlertMessage"] = "Data Not Found in CUCONTRACTS";
            }

            DataSet ds2 = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet("SELECT * from IFSC.CuTRNMAST WHERE EXPIRYDATE=TO_DATE('" + model.OldExpDate.ToString("ddMMMyyyy") + "') AND EXCHANGE='" + model.Exchange + "' AND INSTRUMENT_TYPE='" + model.IndexList + "' ", Session["SelectedConn"].ToString());
            model.result = ds2;
            if (ds2.Tables[0].Rows.Count != 0)
            {
                MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteNonQuery("update IFSC.CuTRNMAST set expirydate = to_date('" + model.NewExpDate.ToString("ddMMMyyyy") + "') WHERE EXPIRYDATE = TO_DATE('" + model.OldExpDate.ToString("ddMMMyyyy") + "') AND EXCHANGE = '" + model.Exchange + "' AND INSTRUMENT_TYPE = '" + model.IndexList + "' ", Session["SelectedConn"].ToString());

            }
            else
            {
                TempData["AlertMessage"] = "Data Not Found in CUTRNMST";
            }

            DataSet ds3 = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet("SELECT * from IFSC.CUDAILYPRICE WHERE EXPIRYDATE=TO_DATE('" + model.OldExpDate.ToString("ddMMMyyyy") + "') AND EXCHANGE='" + model.Exchange + "' AND INSTRUMENT_TYPE='" + model.IndexList + "' ", Session["SelectedConn"].ToString());
            model.result = ds3;
            if (ds3.Tables[0].Rows.Count != 0)
            {
                MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteNonQuery("update IFSC.CUDAILYPRICE set expirydate = to_date('" + model.NewExpDate.ToString("ddMMMyyyy") + "')  WHERE EXPIRYDATE = TO_DATE('" + model.OldExpDate.ToString("ddMMMyyyy") + "') AND EXCHANGE = '" + model.Exchange + "' AND INSTRUMENT_TYPE = '" + model.IndexList + "' ", Session["SelectedConn"].ToString());

            }
            else
            {
                TempData["AlertMessage"] = "Data Not Found in CUDAILYPRICE";
            }

            DataSet ds4 = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet("SELECT * from IFSC.CUSTOCKTRN WHERE EXPIRYDATE=TO_DATE('" + model.OldExpDate.ToString("ddMMMyyyy") + "') AND EXCHANGE='" + model.Exchange + "' AND INSTRUMENT_TYPE='" + model.IndexList + "' ", Session["SelectedConn"].ToString());
            model.result = ds4;
            if (ds4.Tables[0].Rows.Count != 0)
            {
                MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteNonQuery("UPDATE IFSC.CUSTOCKTRN SET EXPIRYDATE=TO_DATE('" + model.NewExpDate.ToString("ddMMMyyyy") + "') WHERE EXPIRYDATE=TO_DATE('" + model.OldExpDate.ToString("ddMMMyyyy") + "') AND EXCHANGE='" + model.Exchange + "' AND INSTRUMENT_TYPE='" + model.IndexList + "'", Session["SelectedConn"].ToString());

            }
            else
            {
                TempData["AlertMessage"] = "Data Not Found in CUDAILYPRICE";
            }

            DataSet ds5 = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet("SELECT * from IFSC.CUPOSITION WHERE EXPIRY_DATE=TO_DATE('" + model.OldExpDate.ToString("ddMMMyyyy") + "') AND EXCHANGE='" + model.Exchange + "' AND INSTRUMENT_TYPE='" + model.IndexList + "' ", Session["SelectedConn"].ToString());
            model.result = ds5;
            if (ds5.Tables[0].Rows.Count != 0)
            {
                MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteNonQuery("UPDATE IFSC.CUPOSITION SET EXPIRY_DATE=TO_DATE('" + model.NewExpDate.ToString("ddMMMyyyy") + "') WHERE EXPIRY_DATE=TO_DATE('" + model.OldExpDate.ToString("ddMMMyyyy") + "') AND EXCHANGE='" + model.Exchange + "' AND INSTRUMENT_TYPE='" + model.IndexList + "'", Session["SelectedConn"].ToString());

            }
            else
            {
                TempData["AlertMessage"] = "Data Not Found in CUDAILYPRICE";
            }
            return View(model);
        }


        [HttpGet]
        public ActionResult TransactionDelete()
        {

            if (Session["WebUser"] == null)
            {
                TempData["AlertMessage"] = "Session Time Out Please Login Again";
                return RedirectToAction("Index", "Login");

            }
            else
            {
                Transaction model = new Transaction();
                model.TrDate = DateTime.Parse(Session["FinYearFrom"].ToString());
                return View(model);

            }

        }

        [HttpPost]
        public ActionResult TransactionDelete(Transaction model)
        {
            DataSet ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet("SELECT * from IFSC.cutrnmast where trade_date=to_date('" + model.TrDate.ToString("ddMMMyyyy") + "') and exchange='NSE' and tradestatus not in('BF','CF','CL')", Session["SelectedConn"].ToString());
            model.result = ds;
            if (ds.Tables[0].Rows.Count != 0)
            {
                MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteNonQuery("delete from IFSC.cutrnmast where trade_date=to_date('" + model.TrDate.ToString("ddMMMyyyy") + "') and exchange='NSE' and tradestatus not in('BF','CF','CL')", Session["SelectedConn"].ToString());
                ModelState.Clear();
                return RedirectToAction("TransactionDelete", "Utility", model);
            }
            else
            {
                TempData["AlertMessage"] = "Data Not Found";
                ModelState.Clear();
                RedirectToAction("TransactionDelete", "Utility", model);
            }


            return View(model);
        }


        [HttpGet]
        public ActionResult ContractDelete()
        {

            if (Session["WebUser"] == null)
            {
                TempData["AlertMessage"] = "Session Time Out Please Login Again";
                return RedirectToAction("Index", "Login");

            }
            else
            {
                Transaction model = new Transaction();
                model.OnDate = DateTime.Parse(Session["FinYearFrom"].ToString());
                return View(model);

            }


        }

        [HttpPost]
        public ActionResult ContractDelete(Transaction model)
        {

            DataSet ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet("select * from IFSC.cupartycont where pdate>=to_date('" + model.OnDate.ToString("ddMMMyyyy") + "') and exchange='NSE'", Session["SelectedConn"].ToString());
            model.result = ds;
            if (ds.Tables[0].Rows.Count != 0)
            {
                MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteNonQuery("delete from IFSC.cupartycont where pdate>=to_date('" + model.OnDate.ToString("ddMMMyyyy") + "') and exchange='NSE'", Session["SelectedConn"].ToString());
                ModelState.Clear();
                return RedirectToAction("ContractDelete", "Utility", model);
            }
            else
            {
                TempData["AlertMessage"] = "Data Not Found";
                ModelState.Clear();
                RedirectToAction("ContractDelete", "Utility", model);
            }


            return View(model);

        }



    }
}