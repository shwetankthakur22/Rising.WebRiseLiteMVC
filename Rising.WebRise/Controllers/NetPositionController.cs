using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;


namespace Rising.WebRise.Controllers
{
    using Rising.WebRise.Models;
    using OracleDBHelper;


    public class NetPositionController : Controller
    {
        // GET: NetPosition
        public ActionResult DATERANGE(string code)
        {
            //for date range
            NetPositionInput model = new NetPositionInput();
            model.DateFrom = DateTime.Parse(Session["FinYearFrom"].ToString());
            model.DateTo = System.DateTime.Now;
            model.DateClosing = System.DateTime.Now;
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

            //---------get expirydate list
            lst = new List<Oracle.ManagedDataAccess.Client.OracleParameter>();
            exchanges = String.Join(",", selectedDBLists.Select(o => o.DBName));
            lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("Exchanges_", exchanges, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
            ds = MvcApplication.OracleDBHelperCore().CustomHelper.CustomDataSet("SYSADM.N$GET_ExpiryDatelList", lst, Session["SelectedConn"].ToString());
            model.ExpiryDateList = new List<SelectListItem>();
            model.ExpiryDateList.Add(new SelectListItem { Text = "Select", Value = "-1" });
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                model.ExpiryDateList.Add(new SelectListItem { Text = ds.Tables[0].Rows[i][0].ToString(), Value = ds.Tables[0].Rows[i][0].ToString() });
            }

            return View(model);
        }

        public ActionResult ASONDATE(string code)
        {
            List<DBList> selectedDBLists = Session["SelectedDBLists"] as List<DBList>;
            if (selectedDBLists.FindAll(a=>a.Exchange=="NSE" || a.Exchange == "BSE" || a.Group == "MCX").Count!=0)
            {                
                return RedirectToAction("Index", "ClientHome");
            }
            //for as on date
            NetPositionInput model = new NetPositionInput();
            model.DateFrom = DateTime.Parse(Session["FinYearFrom"].ToString());
            model.DateTo = System.DateTime.Now;
            model.DateClosing = System.DateTime.Now;
            model.ClientCodeFrom = code;
            model.ClientCodeTo = code;

            //---------get symbol list
            List<Oracle.ManagedDataAccess.Client.OracleParameter> lst = new List<Oracle.ManagedDataAccess.Client.OracleParameter>();
            string exchanges = String.Join(",", selectedDBLists.Select(o => o.Exchange));
            lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("Exchanges_", exchanges, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
            DataSet ds = MvcApplication.OracleDBHelperCore().CustomHelper.CustomDataSet("SYSADM.N$GET_SymbolList", lst, Session["SelectedConn"].ToString());
            model.SymbolList = new List<SelectListItem>();
            model.SymbolList.Add(new SelectListItem { Text = "Select", Value = "-1" });
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                model.SymbolList.Add(new SelectListItem { Text = ds.Tables[0].Rows[i][0].ToString(), Value = ds.Tables[0].Rows[i][1].ToString() });
            }

            //---------get expirydate list
            lst = new List<Oracle.ManagedDataAccess.Client.OracleParameter>();
            exchanges = String.Join(",", selectedDBLists.Select(o => o.Exchange));
            lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("Exchanges_", exchanges, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
            ds = MvcApplication.OracleDBHelperCore().CustomHelper.CustomDataSet("SYSADM.N$GET_ExpiryDatelList", lst, Session["SelectedConn"].ToString());
            model.ExpiryDateList = new List<SelectListItem>();
            model.ExpiryDateList.Add(new SelectListItem { Text = "Select", Value = "-1" });
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                model.ExpiryDateList.Add(new SelectListItem { Text = ds.Tables[0].Rows[i][0].ToString(), Value = ds.Tables[0].Rows[i][0].ToString() });
            }

            return View(model);
        }

        public static NetPositionOutput lstOut;

        public ActionResult Position(string activeMenu, NetPositionInput model)
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
                    DataSet ds1 = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet("SELECT * FROM sysadm.partymst where par_code='" + model.ClientCodeFrom + "' and branchind='" + webUser.UserID + "'", Session["SelectedConn"].ToString());
                    if (ds1.Tables[0].Rows.Count == 0)
                    {
                        TempData["AlertMessage"] = "Invalid Code : " + model.ClientCodeFrom;
                        if (model.AsOnDate == false)
                        {
                            return RedirectToAction("DATERANGE", "NetPosition");
                        }
                        else
                        {
                            return RedirectToAction("AsOnDate", "NetPosition");
                        }
                    }

                    lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("clientcode_", model.ClientCodeFrom, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                }

                else if (webUser.UserType == UserType.RM)
                {
                    DataSet ds1 = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet("SELECT * FROM sysadm.partymst where par_code='" + model.ClientCodeFrom + "' and RMCODE='" + webUser.UserID + "'", Session["SelectedConn"].ToString());
                    if (ds1.Tables[0].Rows.Count == 0)
                    {
                        TempData["AlertMessage"] = "Invalid Code : " + model.ClientCodeFrom;
                        if (model.AsOnDate == false)
                        {
                            return RedirectToAction("DATERANGE", "NetPosition");
                        }
                        else
                        {
                            return RedirectToAction("AsOnDate", "NetPosition");
                        }
                    }

                    lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("clientcode_", model.ClientCodeFrom, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                }
                else if (webUser.UserType == UserType.Admin)
                {
                    lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("clientcode_", model.ClientCodeFrom, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                }


                lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("DateFrom_", model.DateFrom, Oracle.ManagedDataAccess.Client.OracleDbType.Date, ParameterDirection.Input));
                lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("DateTo_", model.DateTo, Oracle.ManagedDataAccess.Client.OracleDbType.Date, ParameterDirection.Input));
                lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("ClosingRateDate_", model.DateClosing, Oracle.ManagedDataAccess.Client.OracleDbType.Date, ParameterDirection.Input));
                lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("ExpiryDate_", ((model.ExpiryDate == "-1" || model.ExpiryDate == null) ? null : model.ExpiryDate), Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("Symbol_", (model.Symbol == "-1" ? null : model.Symbol), Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("OpenPosition_", model.OpenPosition.ToString(), Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                if (model.AsOnDate == false)
                {
                    lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("ReportType_", "DATERANGE", Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                }
                if (model.AsOnDate == true)
                {
                    lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("ReportType_", "ASONDATE", Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                }

                string exchanges = String.Join(",", selectedDBLists.Select(o => o.Exchange));
                lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("Exchanges_", exchanges, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));

                DataSet ds = MvcApplication.OracleDBHelperCore().CustomHelper.CustomDataSet("SYSADM.N$GET_NETPOSITION", lst, Session["SelectedConn"].ToString());

                lstOut = new NetPositionOutput();
                lstOut.listNetPositionOutputRow = new List<NetPositionOutputRow>();
                lstOut.DateFrom = model.DateFrom;
                lstOut.DateTo = model.DateTo;
                lstOut.ClientCode = webUser.UserID;
                lstOut.ClientName = webUser.UserName;
                lstOut.AsOnDate = model.AsOnDate;


                int c = 0;
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    if (c == 0)
                    {
                        lstOut.NSETax = decimal.Parse(row["NSETax"].ToString());
                        lstOut.StampDuty = decimal.Parse(row["StampDuty"].ToString());
                        lstOut.Brok = decimal.Parse(row["Brok"].ToString());
                        lstOut.Demat = decimal.Parse(row["Demat"].ToString());
                        lstOut.Tax1 = decimal.Parse(row["Tax1"].ToString());
                        lstOut.Tax2 = decimal.Parse(row["Tax2"].ToString());
                        lstOut.Tax3 = decimal.Parse(row["Tax3"].ToString());
                        lstOut.STTAmt = decimal.Parse(row["STTAmt"].ToString());
                        lstOut.IGST = decimal.Parse(row["IGST"].ToString());
                        lstOut.CGST = decimal.Parse(row["CGST"].ToString());
                        lstOut.SGST = decimal.Parse(row["SGST"].ToString());
                        lstOut.isCapital = (exchanges.Contains("NSE") == true || exchanges.Contains("BSE") == true || exchanges.Contains("Mcx") == true) ? true : false;
                    }
                    c++;
                    NetPositionOutputRow bdo = new NetPositionOutputRow();
                    bdo.ScripCode = row["ScripCode"].ToString();
                    bdo.ScripExpiry = row["ExpiryDate"].ToString();
                    bdo.ScripOption = row["OptionType"].ToString();
                    bdo.ScripInstrument = row["Instrument_Type"].ToString();
                    bdo.ScripStrike = row["StrikePrice"].ToString();
                    bdo.ScripName = row["ScripName"].ToString();
                    bdo.BFAvgRate = decimal.Parse(row["BFAvgRate"].ToString());
                    bdo.BFValue = decimal.Parse(row["BFValue"].ToString());
                    bdo.BFQty = int.Parse(row["BFQty"].ToString());
                    bdo.SaleAvgRate = decimal.Parse(row["SaleAvgRate"].ToString());
                    bdo.SaleValue = decimal.Parse(row["SaleValue"].ToString());
                    bdo.SaleQty = int.Parse(row["SaleQty"].ToString());
                    bdo.PurAvgRate = decimal.Parse(row["PurAvgRate"].ToString());
                    bdo.PurValue = decimal.Parse(row["PurValue"].ToString());
                    bdo.PurQty = int.Parse(row["PurQty"].ToString());
                    bdo.NetAvgRate = decimal.Parse(row["NetAvgRate"].ToString());
                    bdo.NetValue = decimal.Parse(row["NetValue"].ToString());
                    bdo.NetQty = int.Parse(row["NetQty"].ToString());
                    bdo.CloseRate = decimal.Parse(row["CloseRate"].ToString());
                    bdo.MTM = decimal.Parse(row["MTM"].ToString());
                    bdo.Flag = row["Flag"].ToString();
                    lstOut.listNetPositionOutputRow.Add(bdo);
                }
                DataSet ds11 = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet("SELECT to_char(MAX(WDATE),'ddMONyyyy')  FROM SYSADM.FOMARGIN  WHERE WDATE<='" + model.DateClosing.ToString("ddMMMyyyy") + "'", Session["SelectedConn"].ToString());
                if (ds11.Tables[0].Rows.Count > 0)
                {
                    lstOut.ActualDateClosing = DateTime.Parse(ds11.Tables[0].Rows[0][0].ToString());
                }


                Session["ReportHeader1"] = "Net Position";
                if (model.AsOnDate == false)
                {
                    Session["ReportHeader2"] = "Date Range : " + model.DateFrom.ToString("dd/MM/yyyy") + " - " + model.DateTo.ToString("dd/MM/yyyy");
                }
                if (model.AsOnDate == true)
                {
                    Session["ReportHeader2"] = "As On Date : " + model.DateTo.ToString("dd/MM/yyyy");
                }

                return View(lstOut);
            }
            return RedirectToAction("AsOnDate", "NetPosition");
            //}
            //catch (Exception ex)
            //{
            //    return null;
            //}
        }


        public ActionResult ScripWisepPosition(string code)
        {
            //for date range
            NetPositionInput model = new NetPositionInput();
            model.DateFrom = DateTime.Parse(Session["FinYearFrom"].ToString());
            model.DateTo = System.DateTime.Now;
            model.DateClosing = System.DateTime.Now;
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


        public ActionResult ScripWisepPositionDetails(string activeMenu, NetPositionInput model)
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
                    DataSet ds1 = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet("SELECT * FROM sysadm.partymst where par_code='" + model.ClientCodeFrom + "' and branchind='" + webUser.UserID + "'", Session["SelectedConn"].ToString());
                    if (ds1.Tables[0].Rows.Count == 0)
                    {
                        TempData["AlertMessage"] = "Invalid Code : " + model.ClientCodeFrom;
                        return RedirectToAction("ScripWisepPosition", "NetPosition");
                    }

                    lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("clientcode_", model.ClientCodeFrom, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                }
                else if (webUser.UserType == UserType.RM)
                {
                    DataSet ds1 = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet("SELECT * FROM sysadm.partymst where par_code='" + model.ClientCodeFrom + "' and rmcode='" + webUser.UserID + "'", Session["SelectedConn"].ToString());
                    if (ds1.Tables[0].Rows.Count == 0)
                    {
                        TempData["AlertMessage"] = "Invalid Code : " + model.ClientCodeFrom;
                        return RedirectToAction("ScripWisepPosition", "NetPosition");
                    }

                    lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("clientcode_", model.ClientCodeFrom, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                }
                else if (webUser.UserType == UserType.Admin)
                {
                    lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("clientcode_", model.ClientCodeFrom, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                }


                lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("DateFrom_", model.DateFrom, Oracle.ManagedDataAccess.Client.OracleDbType.Date, ParameterDirection.Input));
                lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("DateTo_", model.DateTo, Oracle.ManagedDataAccess.Client.OracleDbType.Date, ParameterDirection.Input));
                lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("Symbol_", (model.Symbol == "-1" ? null : model.Symbol), Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                string exchanges = String.Join(",", selectedDBLists.Select(o => o.Exchange));
                lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("Exchanges_", exchanges, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));

                DataSet ds = MvcApplication.OracleDBHelperCore().CustomHelper.CustomDataSet("SYSADM.N$GET_SCRIPWISE_NETPOSITION", lst, Session["SelectedConn"].ToString());

                lstOut = new NetPositionOutput();
                lstOut.listNetPositionOutputRow = new List<NetPositionOutputRow>();
                lstOut.DateFrom = model.DateFrom;
                lstOut.DateTo = model.DateTo;
                lstOut.ClientCode = webUser.UserID;
                lstOut.ClientName = webUser.UserName;
                lstOut.AsOnDate = model.AsOnDate;


                int c = 0;
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    if (c == 0)
                    {
                        lstOut.DateFrom = model.DateFrom;
                        lstOut.DateTo = model.DateTo;
                        lstOut.ClientCode = row["Code"].ToString();
                        lstOut.ClientName = row["Name"].ToString();
                        lstOut.AsOnDate = model.AsOnDate;

                        lstOut.NSETax = row["NSETax"].ToString() == "" ? 0 : decimal.Parse(row["NSETax"].ToString());
                        lstOut.StampDuty = row["Stamp"].ToString() == "" ? 0 : decimal.Parse(row["Stamp"].ToString());
                        lstOut.Brok = row["Brok"].ToString() == "" ? 0 : decimal.Parse(row["Brok"].ToString());
                        lstOut.Demat = row["Demchrg"].ToString() == "" ? 0 : decimal.Parse(row["Demchrg"].ToString());
                        lstOut.Tax1 = row["Tax1"].ToString() == "" ? 0 : decimal.Parse(row["Tax1"].ToString());
                        lstOut.Tax2 = row["Tax2"].ToString() == "" ? 0 : decimal.Parse(row["Tax2"].ToString());
                        lstOut.Tax3 = row["Tax3"].ToString() == "" ? 0 : decimal.Parse(row["Tax3"].ToString());
                        lstOut.STTAmt = row["totalSTT"].ToString() == "" ? 0 : decimal.Parse(row["totalSTT"].ToString());
                        lstOut.IGST = row["IGST"].ToString() == "" ? 0 : decimal.Parse(row["IGST"].ToString());
                        lstOut.CGST = row["CGST"].ToString() == "" ? 0 : decimal.Parse(row["CGST"].ToString());
                        lstOut.SGST = row["SGST"].ToString() == "" ? 0 : decimal.Parse(row["SGST"].ToString());
                        lstOut.isCapital = (exchanges.Contains("NSE") == true || exchanges.Contains("BSE") == true || exchanges.Contains("Mcx") == true) ? true : false;
                    }
                    c++;
                    NetPositionOutputRow bdo = new NetPositionOutputRow();
                    bdo.ScripCode = row["Scrip"].ToString();
                    bdo.ScripExpiry = row["ExpiryDate"].ToString();
                    bdo.ScripOption = row["OptionType"].ToString();
                    bdo.ScripInstrument = row["Instrument_Type"].ToString();
                    bdo.ScripStrike = row["StrikePrice"].ToString();
                    bdo.ScripName = row["Sh_Name"].ToString();
                    bdo.BFAvgRate = 0;//decimal.Parse(row["BFAvgRate"].ToString());
                    bdo.BFValue = 0;//decimal.Parse(row["BFValue"].ToString());
                    bdo.BFQty = 0;//int.Parse(row["BFQty"].ToString());
                    bdo.SaleAvgRate =  decimal.Parse(row["SALE_AVG_RATE"].ToString());
                    bdo.SaleValue = decimal.Parse(row["Sale_Value"].ToString());
                    bdo.SaleQty = decimal.Parse(row["SaleQty"].ToString());
                    bdo.PurAvgRate = decimal.Parse(row["PUR_AVG_RATE"].ToString());
                    bdo.PurValue = decimal.Parse(row["Pur_Value"].ToString());
                    bdo.PurQty = decimal.Parse(row["PurQty"].ToString());
                    bdo.NetAvgRate = decimal.Parse(row["AVGRATE"].ToString());
                    bdo.NetValue = decimal.Parse(row["Net_Value"].ToString());
                    bdo.NetQty = decimal.Parse(row["Net_Qty"].ToString());
                    bdo.CloseRate = 0;//decimal.Parse(row["CloseRate"].ToString());
                    bdo.MTM = 0;//decimal.Parse(row["MTM"].ToString());
                    bdo.Flag = row["settno"].ToString();
                    bdo.TradeDate = DateTime.Parse(row["trdate"].ToString()).ToString("dd-MM-yy");
                    lstOut.listNetPositionOutputRow.Add(bdo);
                }

                Session["ReportHeader1"] = "Scrip Wise Settlment Wise Ledger";

                Session["ReportHeader2"] = "Date Range : " + model.DateFrom.ToString("dd/MM/yyyy") + " - " + model.DateTo.ToString("dd/MM/yyyy");


                return View(lstOut);
            }
            return RedirectToAction("ScripWisepPosition", "NetPosition");
            //}
            //catch (Exception ex)
            //{
            //    return null;
            //}
        }
    }
}