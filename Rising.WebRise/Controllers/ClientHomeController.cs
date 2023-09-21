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

    public class ClientHomeController : Controller
    {
        // GET: ClientHome
        public ActionResult Index(DashBoardOutPut model)
        {
            model.DateFrom = DateTime.Parse(Session["FinYearFrom"].ToString());
            model.DateTo = DateTime.Parse(Session["FinYearTo"].ToString());
            model.DateClosing = DateTime.Parse( System.DateTime.Now.ToString("ddMMMyyyy"));
            model.AsonDate = DateTime.Parse(Session["FinYearTo"].ToString());
            model.OpenPosition = true;

            string nul = null;
            WebUser webUser = Session["WebUser"] as WebUser;
            if (webUser != null)
            {
                if (webUser.UserType == UserType.Branch) return RedirectToAction("Index", "BranchHome");
                if (webUser.UserType == UserType.RM) return RedirectToAction("Index", "BranchHome");
                if (webUser.UserType == UserType.Admin) return RedirectToAction("Index", "Admin");
                if (webUser.LoginValidationStatus == true)
                {
                    List<DBList> selectedDBLists = Session["SelectedDBLists"] as List<DBList>;

                    if (webUser.UserType == UserType.Client)
                    {
                        // client profile
                        List<Oracle.ManagedDataAccess.Client.OracleParameter> lst = new List<Oracle.ManagedDataAccess.Client.OracleParameter>();
                        lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("CLIENTCODE_", webUser.UserID, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                        lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("USER_", (selectedDBLists.Count() == 0 ? "SYSADM" : selectedDBLists[0].OtherDBUser), Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));

                        DataSet ds = MvcApplication.OracleDBHelperCore().CustomHelper.CustomDataSet("SYSADM.N$GET_CLIENTPROFILE", lst, Session["SelectedConn"].ToString());
                        model.ds = ds;





                        //financial ledger
                        model.listFinancialLedgerOutputRow = new List<FinancialLedgerOutputRow>();
                        //if (System.Diagnostics.Debugger.IsAttached == true)
                        //{
                        //    //...
                        //}
                        //else
                        {
                            lst = new List<Oracle.ManagedDataAccess.Client.OracleParameter>();
                            lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("CLIENTCODEFROM", webUser.UserID, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                            lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("CLIENTCODETO", webUser.UserID, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                            lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("BRANCHCODE", nul, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                            lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("SUBBRANCHCODE", nul, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                            lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("GROUPCODE", nul, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                            lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("RMCODE", nul, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                            lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("DateFrom", model.DateFrom, Oracle.ManagedDataAccess.Client.OracleDbType.Date, ParameterDirection.Input));
                            lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("DateTo", model.DateTo, Oracle.ManagedDataAccess.Client.OracleDbType.Date, ParameterDirection.Input));
                            lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("IncludeUnReleaseVoucher", false, Oracle.ManagedDataAccess.Client.OracleDbType.Char, ParameterDirection.Input));
                            lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("ExcludeMG13Entries", false, Oracle.ManagedDataAccess.Client.OracleDbType.Char, ParameterDirection.Input));
                            lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("FinancialTranxactionType", nul, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));

                            lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("USER_", (selectedDBLists.Count() == 0 ? "sysadm" : selectedDBLists[0].FinDBUser), Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                            DataSet ds1 = MvcApplication.OracleDBHelperCore().CustomHelper.CustomDataSet("SYSADM.N$GET_FinLedger", lst, Session["SelectedConn"].ToString());
                                                       

                            int i = ds1.Tables[0].Rows.Count;
                            foreach (DataRow row in ds1.Tables[0].Rows)
                            {
                                FinancialLedgerOutputRow flo = new FinancialLedgerOutputRow();
                                flo.Date = DateTime.Parse(row["DT"].ToString());
                                flo.ValueDate = DateTime.Parse(row["DT1"].ToString());
                                flo.Narration = row["NARR"].ToString();
                                flo.BillNo = row["BILLNO"].ToString();
                                flo.CHQNO = row["CHQNO"].ToString();
                                flo.Debit = row["DEBIT"].ToString().Replace(" .00", "0.00");
                                flo.Credit = row["CREDIT"].ToString().Replace(" .00", "0.00");
                                flo.RUNBAL = row["RUNBAL"].ToString().Replace(" .00", "0.00");
                                flo.Segment = row["Segment"].ToString();
                                flo.ClientCode = row["PARTY_CD"].ToString();
                                flo.slno = i;
                                i--;
                                model.listFinancialLedgerOutputRow.Add(flo);
                            }
                           

                            model.listFinancialLedgerOutputRow.Reverse();
                        }



                        //financial ledger - SEGMENT WISE - NSE
                        model.listSegmentWiseFinancialLedgerOutputRow = new List<FinancialLedgerOutputRow>();
                        if (System.Diagnostics.Debugger.IsAttached == true)
                        {
                            //...
                        }
                        else
                        {
                            lst = new List<Oracle.ManagedDataAccess.Client.OracleParameter>();
                            lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("CLIENTCODEFROM", webUser.UserID, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                            lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("CLIENTCODETO", webUser.UserID, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                            lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("BRANCHCODE", nul, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                            lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("SUBBRANCHCODE", nul, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                            lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("GROUPCODE", nul, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                            lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("RMCODE", nul, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                            lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("DateFrom", model.DateFrom, Oracle.ManagedDataAccess.Client.OracleDbType.Date, ParameterDirection.Input));
                            lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("DateTo", model.DateTo, Oracle.ManagedDataAccess.Client.OracleDbType.Date, ParameterDirection.Input));
                            lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("IncludeUnReleaseVoucher", false, Oracle.ManagedDataAccess.Client.OracleDbType.Char, ParameterDirection.Input));
                            lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("ExcludeMG13Entries", false, Oracle.ManagedDataAccess.Client.OracleDbType.Char, ParameterDirection.Input));
                            lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("FinancialTranxactionType", model.Segment.ToString(), Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));

                            lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("USER_", (selectedDBLists.Count() == 0 ? "sysadm" : selectedDBLists[0].FinDBUser), Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                            DataSet ds2 = MvcApplication.OracleDBHelperCore().CustomHelper.CustomDataSet("SYSADM.N$GET_FinLedger", lst, Session["SelectedConn"].ToString());
                                                      

                            foreach (DataRow row in ds2.Tables[0].Rows)
                            {
                                FinancialLedgerOutputRow flo = new FinancialLedgerOutputRow();
                                flo.Date = DateTime.Parse(row["DT"].ToString());
                                flo.ValueDate = DateTime.Parse(row["DT1"].ToString());
                                flo.Narration = row["NARR"].ToString();
                                flo.BillNo = row["BILLNO"].ToString();
                                flo.CHQNO = row["CHQNO"].ToString();
                                flo.Debit = row["DEBIT"].ToString().Replace(" .00", "0.00");
                                flo.Credit = row["CREDIT"].ToString().Replace(" .00", "0.00");
                                flo.RUNBAL = row["RUNBAL"].ToString().Replace(" .00", "0.00");
                                flo.Segment = row["Segment"].ToString();
                                flo.ClientCode = row["PARTY_CD"].ToString();
                                model.listSegmentWiseFinancialLedgerOutputRow.Add(flo);
                            }
                        }
                        SelectList lstenumSegment = new SelectList(Enum.GetValues(typeof(enumCashSegment)).Cast<enumCashSegment>().Select(v => v.ToString()).ToList());
                        ViewBag.Segment = lstenumSegment;
                        if(selectedDBLists[0].Group=="Cash")
                        {
                             lstenumSegment = new SelectList(Enum.GetValues(typeof(enumCashSegment)).Cast<enumCashSegment>().Select(v => v.ToString()).ToList());
                            ViewBag.Segment = lstenumSegment;
                        }
                        if (selectedDBLists[0].Group == "Future & Option")
                        {
                            lstenumSegment = new SelectList(Enum.GetValues(typeof(enumFoSegment)).Cast<enumFoSegment>().Select(v => v.ToString()).ToList());
                            ViewBag.Segment = lstenumSegment;
                        }
                        if (selectedDBLists[0].Group == "Commodity")
                        {
                            lstenumSegment = new SelectList(Enum.GetValues(typeof(enumCommSegment)).Cast<enumCommSegment>().Select(v => v.ToString()).ToList());
                            ViewBag.Segment = lstenumSegment;
                        }
                        if (selectedDBLists[0].Group == "Currency")
                        {
                            lstenumSegment = new SelectList(Enum.GetValues(typeof(enumCurrSegment)).Cast<enumCurrSegment>().Select(v => v.ToString()).ToList());
                            ViewBag.Segment = lstenumSegment;
                        }



                        //cash net position
                        model.listCashNetPositionOutputRow = new List<NetPositionOutputRow>();
                        if (System.Diagnostics.Debugger.IsAttached==true)
                        {
                            //...
                        }
                        else
                        {
                            lst = new List<Oracle.ManagedDataAccess.Client.OracleParameter>();
                            lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("clientcode_", webUser.UserID, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                            lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("DateFrom_", model.DateFrom, Oracle.ManagedDataAccess.Client.OracleDbType.Date, ParameterDirection.Input));
                            lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("DateTo_", model.DateTo, Oracle.ManagedDataAccess.Client.OracleDbType.Date, ParameterDirection.Input));
                            lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("ClosingRateDate_", model.DateClosing, Oracle.ManagedDataAccess.Client.OracleDbType.Date, ParameterDirection.Input));
                            lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("ExpiryDate_", null, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                            lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("Symbol_", null, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                            lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("OpenPosition_", model.OpenPosition.ToString(), Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));

                            lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("ReportType_", "DATERANGE", Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));

                            string exchanges = String.Join(",", selectedDBLists.Select(o => o.Exchange));
                            exchanges = "NSE,BSE";
                            lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("Exchanges_", exchanges, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));

                            DataSet ds3 = MvcApplication.OracleDBHelperCore().CustomHelper.CustomDataSet("SYSADM.N$GET_NETPOSITION", lst, Session["SelectedConn"].ToString());



                            foreach (DataRow row in ds3.Tables[0].Rows)
                            {
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
                                model.listCashNetPositionOutputRow.Add(bdo);
                            }
                        }

                        //fo net position

                        model.listFoNetPositionOutputRow = new List<NetPositionOutputRow>();
                        if (System.Diagnostics.Debugger.IsAttached == true)
                        {
                            //...
                        }
                        else
                        {
                            lst = new List<Oracle.ManagedDataAccess.Client.OracleParameter>();
                            lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("clientcode_", webUser.UserID, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                            lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("DateFrom_", model.DateFrom, Oracle.ManagedDataAccess.Client.OracleDbType.Date, ParameterDirection.Input));
                            lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("DateTo_", model.DateClosing, Oracle.ManagedDataAccess.Client.OracleDbType.Date, ParameterDirection.Input));
                            lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("ClosingRateDate_", model.DateClosing, Oracle.ManagedDataAccess.Client.OracleDbType.Date, ParameterDirection.Input));
                            lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("ExpiryDate_", null, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                            lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("Symbol_", null, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                            lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("OpenPosition_", model.OpenPosition.ToString(), Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));

                            lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("ReportType_", "ASONDATE", Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));

                            string exchanges = String.Join(",", selectedDBLists.Select(o => o.Exchange));
                            exchanges = "NFO";
                            lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("Exchanges_", exchanges, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));

                            DataSet ds3 = MvcApplication.OracleDBHelperCore().CustomHelper.CustomDataSet("SYSADM.N$GET_NETPOSITION", lst, Session["SelectedConn"].ToString());



                            foreach (DataRow row in ds3.Tables[0].Rows)
                            {
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
                                model.listFoNetPositionOutputRow.Add(bdo);
                            }
                        }


                        //comm net position
                        model.listCommNetPositionOutputRow = new List<NetPositionOutputRow>();
                        if (System.Diagnostics.Debugger.IsAttached == true)
                        {
                            //...
                        }
                        else
                        {
                            lst = new List<Oracle.ManagedDataAccess.Client.OracleParameter>();
                            lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("clientcode_", webUser.UserID, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                            lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("DateFrom_", model.DateFrom, Oracle.ManagedDataAccess.Client.OracleDbType.Date, ParameterDirection.Input));
                            lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("DateTo_", model.DateClosing, Oracle.ManagedDataAccess.Client.OracleDbType.Date, ParameterDirection.Input));
                            lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("ClosingRateDate_", model.DateClosing, Oracle.ManagedDataAccess.Client.OracleDbType.Date, ParameterDirection.Input));
                            lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("ExpiryDate_", null, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                            lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("Symbol_", null, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                            lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("OpenPosition_", model.OpenPosition.ToString(), Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));

                            lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("ReportType_", "ASONDATE", Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));

                            string exchanges = String.Join(",", selectedDBLists.Select(o => o.Exchange));
                            exchanges = "MCOM,NDEX";
                            lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("Exchanges_", exchanges, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));

                            DataSet ds3 = MvcApplication.OracleDBHelperCore().CustomHelper.CustomDataSet("SYSADM.N$GET_NETPOSITION", lst, Session["SelectedConn"].ToString());



                            foreach (DataRow row in ds3.Tables[0].Rows)
                            {
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
                                model.listCommNetPositionOutputRow.Add(bdo);
                            }
                        }


                        //Curr net position
                        model.listCurrNetPositionOutputRow = new List<NetPositionOutputRow>();
                        if (System.Diagnostics.Debugger.IsAttached == true)
                        {
                            //...
                        }
                        else
                        {
                            lst = new List<Oracle.ManagedDataAccess.Client.OracleParameter>();
                            lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("clientcode_", webUser.UserID, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                            lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("DateFrom_", model.DateFrom, Oracle.ManagedDataAccess.Client.OracleDbType.Date, ParameterDirection.Input));
                            lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("DateTo_", model.DateClosing, Oracle.ManagedDataAccess.Client.OracleDbType.Date, ParameterDirection.Input));
                            lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("ClosingRateDate_", model.DateClosing, Oracle.ManagedDataAccess.Client.OracleDbType.Date, ParameterDirection.Input));
                            lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("ExpiryDate_", null, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                            lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("Symbol_", null, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                            lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("OpenPosition_", model.OpenPosition.ToString(), Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));

                            lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("ReportType_", "ASONDATE", Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));

                            string exchanges = String.Join(",", selectedDBLists.Select(o => o.Exchange));
                            exchanges = "NCD,BCD";
                            lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("Exchanges_", exchanges, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));

                            DataSet ds3 = MvcApplication.OracleDBHelperCore().CustomHelper.CustomDataSet("SYSADM.N$GET_NETPOSITION", lst, Session["SelectedConn"].ToString());



                            foreach (DataRow row in ds3.Tables[0].Rows)
                            {
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
                                model.listCurrNetPositionOutputRow.Add(bdo);
                            }
                        }

                        //financial summary
                        model.dsFinancialSummary = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet("select NSEBAL+ BSEBAL NSEBAL, MCX_FIN+NCDEX_FIN COMMBAL,  FO_MGN_POSTED + CUR_MGN_POSTED NSEMARGIN, COM_MGN_POSTED COMMMARGIN, BEN_STOCK, CDSL_VAR, CLRBAL TOTBALANCE, NSEBAL + BSEBAL + MCX_FIN + NCDEX_FIN WTMARGINBALANCE, FO_MGN_POSTED + CUR_MGN_POSTED + COM_MGN_POSTED TOTALMARGIN from sysadm.cappcr WHERE code ='"+webUser.UserID+"'", Session["SelectedConn"].ToString());



                        //DEMAT LEDGER
                        model.listDematHoldingOutputRow = new List<DematHoldingOutputRow>();
                        if (System.Diagnostics.Debugger.IsAttached == true)
                        {
                            //...
                        }
                        else
                        {
                            //lst = new List<Oracle.ManagedDataAccess.Client.OracleParameter>();
                            //lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("clientcode_", webUser.UserID, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                            //lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("AsOnDate_", model.DateClosing, Oracle.ManagedDataAccess.Client.OracleDbType.Date, ParameterDirection.Input));
                            //DataSet ds4 = MvcApplication.OracleDBHelperCore().CustomHelper.CustomDataSet("SYSADM.N$GET_DematHolding", lst, Session["SelectedConn"].ToString());

                            //model.listDematHoldingOutputRow = new List<DematHoldingOutputRow>();

                            //int c = 0;
                            //foreach (DataRow row in ds4.Tables[0].Rows)
                            //{
                            //    c++;
                            //    DematHoldingOutputRow bdo = new DematHoldingOutputRow();
                            //    bdo.ScripCode = row["SCRIPCD"].ToString();
                            //    bdo.ScripName = row["SH_NAME"].ToString();
                            //    bdo.ScripIsin = row["ISINCODE"].ToString();
                            //    bdo.Qty = decimal.Parse(row["Qty"].ToString());
                            //    bdo.Stock = decimal.Parse(row["Stock"].ToString());
                            //    bdo.LockQty = decimal.Parse(row["Qty_lock"].ToString());
                            //    bdo.CDSLQty = decimal.Parse(row["cdsl_qty"].ToString());
                            //    bdo.NSDLQty = decimal.Parse(row["nsdl_qty"].ToString());
                            //    bdo.TotalQty = decimal.Parse(row["tot_qty"].ToString());
                            //    bdo.Rate = decimal.Parse(row["rate"].ToString());
                            //    bdo.Value = decimal.Parse(row["stk_val"].ToString());
                            //    bdo.VarPer = decimal.Parse(row["rate_var"].ToString());
                            //    bdo.VarValue = decimal.Parse(row["varvalue"].ToString());
                            //    model.listDematHoldingOutputRow.Add(bdo);
                            //}
                        }
                    }

                    return View(model);
                }
                else return RedirectToAction("Index", "Login");
            }
            else { return RedirectToAction("Index", "Login"); }
        }
       

        public ActionResult LogOut()
        {
            WebUser webUser = Session["WebUser"] as WebUser;
            if (webUser.LoginValidationStatus == true)
            {
                webUser.LoginValidationStatus = false;
                webUser.LoginStatus = false;
                webUser.SaveWebUser(Session["SelectedConn"].ToString());                
                return RedirectToAction("Index", "Login");
            }
            return RedirectToAction("Index", "Login");
        }
               
        public ActionResult FillSegmentLedger(string seg)
        {
            DateTime DateFrom = DateTime.Parse(Session["FinYearFrom"].ToString());
            DateTime DateTo = DateTime.Parse(Session["FinYearTo"].ToString());
            List<DBList> selectedDBLists = Session["SelectedDBLists"] as List<DBList>;

            
            string nul = null;
            WebUser webUser = Session["WebUser"] as WebUser;
            if (webUser != null && seg !=null)
            {
                List<FinancialLedgerOutputRow> lstData = new List<FinancialLedgerOutputRow>();


                List<Oracle.ManagedDataAccess.Client.OracleParameter> lst = new List<Oracle.ManagedDataAccess.Client.OracleParameter>();
                lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("CLIENTCODEFROM", webUser.UserID, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("CLIENTCODETO", webUser.UserID, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("BRANCHCODE", nul, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("SUBBRANCHCODE", nul, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("GROUPCODE", nul, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("RMCODE", nul, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("DateFrom", DateFrom, Oracle.ManagedDataAccess.Client.OracleDbType.Date, ParameterDirection.Input));
                lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("DateTo", DateTo, Oracle.ManagedDataAccess.Client.OracleDbType.Date, ParameterDirection.Input));
                lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("IncludeUnReleaseVoucher", false, Oracle.ManagedDataAccess.Client.OracleDbType.Char, ParameterDirection.Input));
                lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("ExcludeMG13Entries", false, Oracle.ManagedDataAccess.Client.OracleDbType.Char, ParameterDirection.Input));
                lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("FinancialTranxactionType", seg.ToString()=="All" ? nul : seg.ToString(), Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));

                lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("USER_", (selectedDBLists.Count() == 0 ? "sysadm" : selectedDBLists[0].FinDBUser), Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                DataSet ds2 = MvcApplication.OracleDBHelperCore().CustomHelper.CustomDataSet("SYSADM.N$GET_FinLedger", lst, Session["SelectedConn"].ToString());

                lstData = new List<FinancialLedgerOutputRow>();

                foreach (DataRow row in ds2.Tables[0].Rows)
                {
                    FinancialLedgerOutputRow flo = new FinancialLedgerOutputRow();
                    flo.Date = DateTime.Parse(row["DT"].ToString());
                    flo.ValueDate = DateTime.Parse(row["DT1"].ToString());
                    flo.Narration = row["NARR"].ToString();
                    flo.BillNo = row["BILLNO"].ToString();
                    flo.CHQNO = row["CHQNO"].ToString();
                    flo.Debit = row["DEBIT"].ToString().Replace(" .00", "0.00");
                    flo.Credit = row["CREDIT"].ToString().Replace(" .00", "0.00");
                    flo.RUNBAL = row["RUNBAL"].ToString().Replace(" .00", "0.00");
                    flo.Segment = row["Segment"].ToString();
                    flo.ClientCode = row["PARTY_CD"].ToString();
                    lstData.Add(flo);
                }

                DashBoardOutPut model = new DashBoardOutPut();
                model.listSegmentWiseFinancialLedgerOutputRow = lstData;
                return PartialView("_segmentLedger", model);             
            }
            return null;
        }
           
        [HttpPost]
        public ActionResult changeSegment(FormCollection formcollection)
        {
            var str = formcollection["DBUser"];
            var str1 = str.Split(',');
            List<DBList> tempDBLists = new List<DBList>();
            List<DBList> selectedDBLists = Session["SelectedDBLists"] as List<DBList>;
            foreach (DBList db in selectedDBLists) { tempDBLists.Add(db); }
            selectedDBLists = new List<DBList>();

            var distinct = tempDBLists[0].Group;

            foreach (var str2 in str1)
            {
                if (str2 != "")
                {
                    if (str2 == distinct)
                    { }
                    else
                    {
                        foreach (DBList dbl9 in MvcApplication.DBLists)
                        {
                            if (dbl9.Group == str2)
                            {
                                selectedDBLists.Add(dbl9);
                            }
                        }
                    }
                }
            }
            if(selectedDBLists.Count==0)
            {
                foreach (DBList dbl in MvcApplication.DBLists)
                {
                    if (dbl.CompGroup == "1")
                    {
                        if (selectedDBLists.Find(a => a.Exchange == dbl.Exchange) == null)
                        {
                            selectedDBLists.Add(dbl);
                        }
                    }
                }
            }
            Session["SelectedDBGroup"] = selectedDBLists[0].Group;

            
            Session["SelectedDBLists"] = selectedDBLists;
            List<Oracle.ManagedDataAccess.Client.OracleParameter> lst = new List<Oracle.ManagedDataAccess.Client.OracleParameter>();
            lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("USER_", (selectedDBLists.Count() == 0 ? "SYSADM" : selectedDBLists[0].OtherDBUser), Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
            DataSet ds = MvcApplication.OracleDBHelperCore().CustomHelper.CustomDataSet("SYSADM.N$GET_CompanyDetails", lst, Session["SelectedConn"].ToString());

            Session["CompanyName"] = ds.Tables[0].Rows[0][0].ToString();
            Session["FinYearFrom"] = ds.Tables[0].Rows[0][1].ToString();
            Session["FinYearTo"] =ds.Tables[0].Rows[0][2].ToString();


            return Redirect(Request.UrlReferrer.ToString());
        }
       
        public ActionResult changeSegment1(string id)
        {
            string str2 = id.Replace("---", "&");

            List<DBList> tempDBLists = new List<DBList>();
            List<DBList> selectedDBLists = Session["SelectedDBLists"] as List<DBList>;
            foreach (DBList db in selectedDBLists) { tempDBLists.Add(db); }
            selectedDBLists = new List<DBList>();

            var distinct = tempDBLists[0].Group;

            foreach (DBList dbl9 in MvcApplication.DBLists)
            {
                if (dbl9.Group == str2)
                {
                    selectedDBLists.Add(dbl9);
                }
            }


            if (selectedDBLists.Count == 0)
            {
                foreach (DBList dbl in MvcApplication.DBLists)
                {
                    if (dbl.CompGroup == "1")
                    {
                        if (selectedDBLists.Find(a => a.Exchange == dbl.Exchange) == null)
                        {
                            selectedDBLists.Add(dbl);
                        }
                    }
                }
            }
            Session["SelectedDBGroup"] = selectedDBLists[0].Group;


            Session["SelectedDBLists"] = selectedDBLists;
            List<Oracle.ManagedDataAccess.Client.OracleParameter> lst = new List<Oracle.ManagedDataAccess.Client.OracleParameter>();
            lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("USER_", (selectedDBLists.Count() == 0 ? "SYSADM" : selectedDBLists[0].OtherDBUser), Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
            DataSet ds = MvcApplication.OracleDBHelperCore().CustomHelper.CustomDataSet("SYSADM.N$GET_CompanyDetails", lst, Session["SelectedConn"].ToString());

            Session["CompanyName"] = ds.Tables[0].Rows[0][0].ToString();
            Session["FinYearFrom"] = ds.Tables[0].Rows[0][1].ToString();
            Session["FinYearTo"] = ds.Tables[0].Rows[0][2].ToString();

            return RedirectToAction("Index", "ClientHome");
            //return Redirect(Request.UrlReferrer.ToString());
        }

        public ActionResult changeYear(string id)
        {
            Session["SelectedYear"] = MvcApplication.YearLists.Find(a=>a.ConnName==id).YearName;
            Session["SelectedConn"] = MvcApplication.YearLists.Find(a => a.ConnName == id).ConnName;

            List<Oracle.ManagedDataAccess.Client.OracleParameter> lst = new List<Oracle.ManagedDataAccess.Client.OracleParameter>();
            lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("USER_", "SYSADM", Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
            DataSet ds = MvcApplication.OracleDBHelperCore().CustomHelper.CustomDataSet("SYSADM.N$GET_CompanyDetails", lst, Session["SelectedConn"].ToString());

            Session["CompanyName"] = ds.Tables[0].Rows[0][0].ToString();
            Session["FinYearFrom"] = ds.Tables[0].Rows[0][1].ToString();
            Session["FinYearTo"] = ds.Tables[0].Rows[0][2].ToString();


            return Redirect(Request.UrlReferrer.ToString());
        }


        public ActionResult FileUploadSnap(HttpPostedFileBase files, string method)
        {
            WebUser webUser = Session["WebUser"] as WebUser;
            if (webUser != null)
            {
                if (files != null)
                {

                    var path = System.IO.Path.Combine(Server.MapPath("~/ClientSnaps/") + webUser.UserID+"_.jpg");
                    files.SaveAs(path);
                }
            }
            return RedirectToAction("Index", "ClientHome");
        }


        public ActionResult TradingHolidays(HttpPostedFileBase files, string method)
        {
            WebUser webUser = Session["WebUser"] as WebUser;
            if (webUser != null)
            {
                return View();
            }
            return RedirectToAction("Index", "Login");
        }

        //Client Margin

        public ActionResult ClientMarginInput()
        {
            GenericModel model = new GenericModel();
            model.DateFrom = DateTime.Parse(Session["FinYearFrom"].ToString());
            model.DateTo = DateTime.Parse(Session["FinYearTo"].ToString());
            model.AsOnDate = System.DateTime.Now;
            return View(model);
        }


        public ActionResult ClientMargin(GenericModel model)
        {
            WebUser webUser = Session["WebUser"] as WebUser;
            if (webUser != null)
            {
                if (webUser.LoginValidationStatus == true)
                {
                    List<DBList> selectedDBLists = Session["SelectedDBLists"] as List<DBList>;

                    if (webUser.UserType == UserType.Client)
                    {
                       
                        

                        Session["ReportHeader1"] = "Client Margin Report";
                        Session["ReportHeader2"] = "Date Range : " + model.DateFrom.ToString("dd/MM/yyyy") + " - " + model.DateTo.ToString("dd/MM/yyyy");
                        return View(model);
                    }

                }
            }
            return RedirectToAction("Index", "TimeOut");
        }
    }
}