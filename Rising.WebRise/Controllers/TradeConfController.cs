using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;

namespace Rising.WebRise.Controllers.Reports
{
    using Rising.WebRise.Models;
    using OracleDBHelper;

    public class TradeConfController : Controller
    {
        // GET: TradeConf
        public ActionResult Index(string code)
        {
            //for date range
            TradeConfInput model = new TradeConfInput();
            model.DateFrom = System.DateTime.Now;
            model.DateTo = DateTime.Parse(Session["FinYearTo"].ToString());
            model.ClientCodeFrom = code;
            model.ClientCodeTo = code;

            //---------get symbol list
            List<DBList> selectedDBLists = Session["SelectedDBLists"] as List<DBList>;
            List<Oracle.ManagedDataAccess.Client.OracleParameter> lst = new List<Oracle.ManagedDataAccess.Client.OracleParameter>();
            string exchanges = String.Join(",", selectedDBLists.Select(o => o.DBName));
            lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("Exchanges_", exchanges, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
            DataSet ds = MvcApplication.OracleDBHelperCore().CustomHelper.CustomDataSet("SYSADM.N$GET_SymbolList", lst, Session["SelectedConn"].ToString());
            model.SymbolList = new List<SelectListItem>();
            model.SymbolList.Add(new SelectListItem { Text = "Select", Value = "-1" });
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                model.SymbolList.Add(new SelectListItem { Text = ds.Tables[0].Rows[i][0].ToString(), Value = ds.Tables[0].Rows[i][1].ToString() });
            }


            return View(model);
        }

        public static TradeConfOutput lstOut;

        public ActionResult Confirmation(TradeConfInput model)
        {
            //try
            //{
            WebUser webUser = Session["WebUser"] as WebUser;
            List<DBList> selectedDBLists = Session["SelectedDBLists"] as List<DBList>;
            if (selectedDBLists.Count == 0)
            {
                TempData["AlertMessage"] = "No Segment Selected...";
                return RedirectToAction("Index", "ClientHome");
            }
            if (model.DateFrom > DateTime.Parse("01jan1900") || model.DateTo > DateTime.Parse("01jan1900"))
            {
                List<Oracle.ManagedDataAccess.Client.OracleParameter> lst = new List<Oracle.ManagedDataAccess.Client.OracleParameter>();
                if (webUser.UserType == UserType.Client)
                {
                    lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("clientcode_", webUser.UserID, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                }
                else if (webUser.UserType == UserType.Branch)
                {
                    model.ClientCodeTo = model.ClientCodeFrom;
                    DataSet ds1 = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet("SELECT * FROM sysadm.partymst where par_code='" + model.ClientCodeFrom + "' and branchind='" + webUser.UserID + "'", Session["SelectedConn"].ToString());
                    if (ds1.Tables[0].Rows.Count == 0)
                    {
                        TempData["AlertMessage"] = "Invalid Code : " + model.ClientCodeFrom;
                        return RedirectToAction("Index", "TradeConf");

                    }

                    lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("clientcode_", model.ClientCodeFrom, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                }

                else if (webUser.UserType == UserType.RM)
                {
                    model.ClientCodeTo = model.ClientCodeFrom;
                    DataSet ds1 = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet("SELECT * FROM sysadm.partymst where par_code='" + model.ClientCodeFrom + "' and RMCODE='" + webUser.UserID + "'", Session["SelectedConn"].ToString());
                    if (ds1.Tables[0].Rows.Count == 0)
                    {
                        TempData["AlertMessage"] = "Invalid Code : " + model.ClientCodeFrom;
                        return RedirectToAction("Index", "TradeConf");

                    }

                    lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("clientcode_", model.ClientCodeFrom, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                }
                else if (webUser.UserType == UserType.Admin)
                {
                    model.ClientCodeTo = model.ClientCodeFrom;
                   
                    lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("clientcode_", model.ClientCodeFrom, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                }

                lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("DateFrom_", model.DateFrom, Oracle.ManagedDataAccess.Client.OracleDbType.Date, ParameterDirection.Input));
                lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("DateTo_", model.DateTo, Oracle.ManagedDataAccess.Client.OracleDbType.Date, ParameterDirection.Input));
                lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("Symbol_", model.Symbol == "-1" ? null : model.Symbol, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                string exchanges = String.Join(",", selectedDBLists.Select(o => o.Exchange));
                lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("Exchanges_", exchanges, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));

                DataSet ds = MvcApplication.OracleDBHelperCore().CustomHelper.CustomDataSet("SYSADM.N$GET_TRADECONF", lst, Session["SelectedConn"].ToString());

                lstOut = new TradeConfOutput();
                lstOut.listTradeConfOutputRow = new List<TradeConfOutputRow>();
                lstOut.DateFrom = model.DateFrom;
                lstOut.DateTo = model.DateTo;
                lstOut.ClientCode = webUser.UserID;
                lstOut.ClientName = webUser.UserName;


                int c = 0;
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    if (c == 0)
                    {
                        lstOut.DateFrom = model.DateFrom;
                        lstOut.DateTo = model.DateTo;
                        lstOut.ClientCode = row["ClientCode"].ToString();
                        lstOut.ClientName = row["ClientName"].ToString();
                    }
                    c++;
                    TradeConfOutputRow bdo = new TradeConfOutputRow();
                    bdo.ScripCode = row["ScripCode"].ToString();
                    bdo.ScripIsin = row["ScripIsin"].ToString();
                    bdo.ScripName = row["ScripName"].ToString();
                    bdo.TradeDate = DateTime.Parse(row["TradeDate"].ToString());

                    bdo.TradeTime = DateTime.Parse(row["TradeTime"].ToString());
                    bdo.OrderTime = DateTime.Parse(row["OrderTime"].ToString());
                    bdo.OrderNo = row["OrderNo"].ToString();
                    bdo.TradeNo = row["TradeNo"].ToString();

                    bdo.Qty = int.Parse(row["Qty"].ToString());
                    bdo.TradeRate = decimal.Parse(row["TradeRate"].ToString());
                    bdo.NetRate = decimal.Parse(row["NetRate"].ToString());
                    bdo.TradeValue = decimal.Parse(row["TradeValue"].ToString());
                    bdo.Brokrage = decimal.Parse(row["Brokrage"].ToString());

                    bdo.Flag = row["Flag"].ToString();
                    lstOut.listTradeConfOutputRow.Add(bdo);
                }
                Session["ReportHeader1"] = "Trade Confirmation";
                Session["ReportHeader2"] = "Date Range : " + model.DateFrom.ToString("dd/MM/yyyy") + " - " + model.DateTo.ToString("dd/MM/yyyy");

                return View(lstOut);
            }
            return RedirectToAction("Index", "TradeConf");
            //}
            //catch (Exception ex)
            //{
            //    return View();
            //}
        }

    }
}