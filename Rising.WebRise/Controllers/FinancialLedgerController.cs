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

    public class FinancialLedgerController : Controller
    {       
        public ActionResult Index(string code)
        {
            FinancialLedgerInput model = new FinancialLedgerInput();
            model.DateFrom = DateTime.Parse(Session["FinYearFrom"].ToString());
            model.DateTo = DateTime.Parse(Session["FinYearTo"].ToString());
            model.ClientCodeFrom = code;
            model.ClientCodeTo = code;
            return View(model);
        }


        public static FinancialLedgerOutput lstOut;
        public ActionResult Ledger(string activeMenu, FinancialLedgerInput model)
        {
            //try
            //{
            string nul = null;
            WebUser webUser = Session["WebUser"] as WebUser;
            if (model.DateFrom > DateTime.Parse("01jan1900"))
            {
                List<Oracle.ManagedDataAccess.Client.OracleParameter> lst = new List<Oracle.ManagedDataAccess.Client.OracleParameter>();
                if (webUser.UserType == UserType.Client)
                {
                    lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("CLIENTCODEFROM", webUser.UserID, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                    lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("CLIENTCODETO", webUser.UserID, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                    lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("BRANCHCODE", nul, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                    lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("SUBBRANCHCODE", nul, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                    lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("GROUPCODE", nul, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                    lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("RMCODE", nul, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                }
                else if (webUser.UserType == UserType.Branch)
                {
                    model.ClientCodeTo = model.ClientCodeFrom;

                    DataSet ds1 = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet("SELECT * FROM sysadm.partymst where par_code='" + model.ClientCodeFrom + "' and branchind='" + webUser.UserID + "'", Session["SelectedConn"].ToString());
                    if (ds1.Tables[0].Rows.Count == 0)
                    {
                        TempData["AlertMessage"] = "Invalid Code : " + model.ClientCodeFrom;
                        return RedirectToAction("Index", "FinancialLedger");
                    }

                    if (1 == 1)
                    {
                        lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("CLIENTCODEFROM", model.ClientCodeFrom, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                        lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("CLIENTCODETO", model.ClientCodeTo, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                        lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("BRANCHCODE", webUser.UserID, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                        lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("SUBBRANCHCODE", nul, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                        lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("GROUPCODE", nul, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                        lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("RMCODE", nul, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                    }
                }

                else if (webUser.UserType == UserType.RM)
                {
                    model.ClientCodeTo = model.ClientCodeFrom;

                    DataSet ds1 = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet("SELECT * FROM sysadm.partymst where par_code='" + model.ClientCodeFrom + "' and RMCODE='" + webUser.UserID + "'", Session["SelectedConn"].ToString());
                    if (ds1.Tables[0].Rows.Count == 0)
                    {
                        TempData["AlertMessage"] = "Invalid Code : " + model.ClientCodeFrom;
                        return RedirectToAction("Index", "FinancialLedger");
                    }

                    if (1 == 1)
                    {
                        lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("CLIENTCODEFROM", model.ClientCodeFrom, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                        lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("CLIENTCODETO", model.ClientCodeTo, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                        lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("BRANCHCODE", nul, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                        lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("SUBBRANCHCODE", nul, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                        lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("GROUPCODE", nul, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                        lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("RMCODE", nul, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                    }
                }

                else if (webUser.UserType == UserType.Admin)
                {
                    model.ClientCodeTo = model.ClientCodeFrom;

                    lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("CLIENTCODEFROM", model.ClientCodeFrom, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                    lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("CLIENTCODETO", model.ClientCodeTo, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                    lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("BRANCHCODE", nul, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                    lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("SUBBRANCHCODE", nul, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                    lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("GROUPCODE", nul, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                    lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("RMCODE", nul, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                }

                lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("DateFrom", model.DateFrom, Oracle.ManagedDataAccess.Client.OracleDbType.Date, ParameterDirection.Input));
                lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("DateTo", model.DateTo, Oracle.ManagedDataAccess.Client.OracleDbType.Date, ParameterDirection.Input));
                lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("IncludeUnReleaseVoucher", model.IncludeUnReleaseVoucher, Oracle.ManagedDataAccess.Client.OracleDbType.Char, ParameterDirection.Input));
                lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("ExcludeMG13Entries", model.ExcludeMG13Entries, Oracle.ManagedDataAccess.Client.OracleDbType.Char, ParameterDirection.Input));
                lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("FinancialTranxactionType", nul, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));

                List<DBList> selectedDBLists = Session["SelectedDBLists"] as List<DBList>;
                lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("USER_", (selectedDBLists.Count() == 0 ? "sysadm" : selectedDBLists[0].FinDBUser), Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                DataSet ds = MvcApplication.OracleDBHelperCore().CustomHelper.CustomDataSet("SYSADM.N$GET_FinLedger", lst, Session["SelectedConn"].ToString());

                lstOut = new FinancialLedgerOutput();
                lstOut.listFinancialLedgerOutputRow = new List<FinancialLedgerOutputRow>();
                lstOut.DateFrom = model.DateFrom;
                lstOut.DateTo = model.DateTo;
                int c = 0;
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    if (c == 0) lstOut.OpeningBalance = row["RUNBAL"].ToString();
                    else lstOut.ClosingBalance = row["RUNBAL"].ToString();
                    c++;
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
                    lstOut.listFinancialLedgerOutputRow.Add(flo);
                }
                Session["ReportHeader1"] = "Client Financial Ledger";
                Session["ReportHeader2"] = "Date Range : " + model.DateFrom.ToString("dd/MM/yyyy") + " - " + model.DateTo.ToString("dd/MM/yyyy");
                return View(lstOut);
            }
            return RedirectToAction("Index", "FinancialLedger");
            //}
            //catch
            //{
            //    return View();
            //}
        }
        

        public static BillDetailsOutput lstBillDetailOut;
        public ActionResult BillDetail(string billno, DateTime trndate, string segment, string cd)
        {
            //try
            //{
            WebUser webUser = Session["WebUser"] as WebUser;
            Dictionary<string, string> dict = new Dictionary<string, string>();

            if (segment != null) dict[segment] = segment;
            dict["MCX-SX"] = "MCD";
            dict["NSE-SX"] = "NCD";
            dict["BSE-SX"] = "BCD";

            dict["MCXCOM"] = "MCOM";



            List<Oracle.ManagedDataAccess.Client.OracleParameter> lst = new List<Oracle.ManagedDataAccess.Client.OracleParameter>();
            if (webUser.UserType == UserType.Client)
            {
                lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("clientcode_", webUser.UserID, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
            }
            else if (webUser.UserType == UserType.Branch)
            {
                lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("clientcode_", cd, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
            }
            else if (webUser.UserType == UserType.RM)
            {
                lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("clientcode_", cd, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
            }
            else if (webUser.UserType == UserType.Admin)
            {
                lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("clientcode_", cd, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
            }
            lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("billNo_", billno.Replace("__", "-").Split('-')[2], Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
            lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("billtype_", billno.Replace("__", "-").Split('-')[0].Substring(0, 3), Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
            lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("insttype_", billno.Replace("__", "-").Split('-')[0].Substring(0, 3).Replace("FDL", "FUT"), Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
            lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("tradedate_", trndate, Oracle.ManagedDataAccess.Client.OracleDbType.Date, ParameterDirection.Input));
            lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("USER_", dict[segment], Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));

            DataSet ds = MvcApplication.OracleDBHelperCore().CustomHelper.CustomDataSet("SYSADM.N$GET_BillDetails", lst, Session["SelectedConn"].ToString());

            lstBillDetailOut = new BillDetailsOutput();
            lstBillDetailOut.listBillDetailsOutputRow = new List<BillDetailsOutputRow>();

            int c = 0;
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                if (c == 0)
                {
                    lstBillDetailOut.TrnDate = DateTime.Parse(row["TrnDate"].ToString());
                    lstBillDetailOut.DelvDate = DateTime.Parse(row["DelvDate"].ToString());
                    lstBillDetailOut.TrnDelvSettno = row["TrnDelvSettno"].ToString();
                    lstBillDetailOut.BillNo = row["BillNo"].ToString();
                    lstBillDetailOut.ClientCode = row["ClientCode"].ToString();
                    lstBillDetailOut.ClientName = row["ClientName"].ToString();
                    lstBillDetailOut.ClientAddress1 = row["ClientAddress1"].ToString();
                    lstBillDetailOut.ClientAddress2 = row["ClientAddress3"].ToString();
                    lstBillDetailOut.ClientAddress3 = row["ClientAddress3"].ToString();
                    lstBillDetailOut.ClientAddress4 = row["ClientAddress4"].ToString();

                    lstBillDetailOut.NSETax = decimal.Parse(row["NSETax"].ToString());
                    lstBillDetailOut.STaxNSETax = decimal.Parse(row["STaxNSETax"].ToString());
                    lstBillDetailOut.NSETaxNarration = "NSE Tax";// row["NSETaxNarration"].ToString();

                    lstBillDetailOut.StampDuty = decimal.Parse(row["StampDuty"].ToString());
                    lstBillDetailOut.STaxStampDuty = decimal.Parse(row["STaxStampDuty"].ToString());
                    lstBillDetailOut.StampDutyNarration = row["StampDutyNarration"].ToString();

                    lstBillDetailOut.Brok = decimal.Parse(row["Brok"].ToString());
                    lstBillDetailOut.STaxBrok = decimal.Parse(row["STaxBrok"].ToString());
                    lstBillDetailOut.BrokNarration = "Brok";// row["BrokNarration"].ToString();

                    lstBillDetailOut.Demat = decimal.Parse(row["Demat"].ToString());
                    lstBillDetailOut.STaxDemat = decimal.Parse(row["STaxDemat"].ToString());
                    lstBillDetailOut.DematNarration = "Demat Charge";// row["DematNarration"].ToString();

                    lstBillDetailOut.Tax1 = decimal.Parse(row["Tax1"].ToString());
                    lstBillDetailOut.STaxTax1 = decimal.Parse(row["STaxTax1"].ToString());
                    lstBillDetailOut.Tax1Narration = row["Tax1Narration"].ToString();

                    lstBillDetailOut.Tax2 = decimal.Parse(row["Tax2"].ToString());
                    lstBillDetailOut.STaxTax2 = decimal.Parse(row["STaxTax2"].ToString());
                    lstBillDetailOut.Tax2Narration = row["Tax2Narration"].ToString();

                    lstBillDetailOut.Tax3 = decimal.Parse(row["Tax3"].ToString());
                    lstBillDetailOut.STaxTax3 = decimal.Parse(row["STaxTax3"].ToString());
                    lstBillDetailOut.Tax3Narration = row["Tax3Narration"].ToString();


                    lstBillDetailOut.STTAmt = decimal.Parse(row["STTAmt"].ToString());
                    lstBillDetailOut.IGST = decimal.Parse(row["IGST"].ToString());
                    lstBillDetailOut.CGST = decimal.Parse(row["CGST"].ToString());
                    lstBillDetailOut.SGST = decimal.Parse(row["SGST"].ToString());

                    lstBillDetailOut.Header = row["Header"].ToString();

                    lstBillDetailOut.NetBalance = decimal.Parse(row["NetBalance"].ToString());
                }
                c++;
                BillDetailsOutputRow bdo = new BillDetailsOutputRow();
                bdo.ScripCode = row["ScripCode"].ToString();
                bdo.ScripName = row["ScripName"].ToString();
                bdo.SaleMktRate = decimal.Parse(row["SaleMktRate"].ToString());
                bdo.SaleNetRate = decimal.Parse(row["SaleNetRate"].ToString());
                bdo.SaleValue = decimal.Parse(row["SaleValue"].ToString());
                bdo.SaleQty = int.Parse(row["SaleQty"].ToString());
                bdo.PurMktRate = decimal.Parse(row["PurMktRate"].ToString());
                bdo.PurNetRate = decimal.Parse(row["PurNetRate"].ToString());
                bdo.PurValue = decimal.Parse(row["PurValue"].ToString());
                bdo.PurQty = int.Parse(row["PurQty"].ToString());
                bdo.Flag = row["Flag"].ToString();
                lstBillDetailOut.listBillDetailsOutputRow.Add(bdo);
            }
            Session["ReportHeader1"] = "Client Bill Detail";
            Session["ReportHeader2"] = "Trade Date : " + lstBillDetailOut.TrnDate.ToString("dd/MM/yyyy");

            return View(lstBillDetailOut);
            //}
            //catch (Exception ex)
            //{
            //    return null;
            //}
        }

        public ActionResult AuctionBills(FinancialLedgerInput model)
        {
            //try
            //{
                string nul = null;
                WebUser webUser = Session["WebUser"] as WebUser;
                model.DateFrom = DateTime.Parse(Session["FinYearFrom"].ToString());
                model.DateTo = DateTime.Parse(Session["FinYearTo"].ToString());

                if (model.DateFrom > DateTime.Parse("01jan1900"))
                {
                    List<Oracle.ManagedDataAccess.Client.OracleParameter> lst = new List<Oracle.ManagedDataAccess.Client.OracleParameter>();

                    lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("CLIENTCODEFROM", webUser.UserID, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                    lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("CLIENTCODETO", webUser.UserID, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                    lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("BRANCHCODE", nul, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                    lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("SUBBRANCHCODE", nul, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                    lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("GROUPCODE", nul, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                    lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("RMCODE", nul, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                    lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("DateFrom", model.DateFrom, Oracle.ManagedDataAccess.Client.OracleDbType.Date, ParameterDirection.Input));
                    lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("DateTo", model.DateTo, Oracle.ManagedDataAccess.Client.OracleDbType.Date, ParameterDirection.Input));
                    lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("IncludeUnReleaseVoucher", model.IncludeUnReleaseVoucher, Oracle.ManagedDataAccess.Client.OracleDbType.Char, ParameterDirection.Input));
                    lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("ExcludeMG13Entries", model.ExcludeMG13Entries, Oracle.ManagedDataAccess.Client.OracleDbType.Char, ParameterDirection.Input));
                    lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("FinancialTranxactionType", "AuctionBill", Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));

                    List<DBList> selectedDBLists = Session["SelectedDBLists"] as List<DBList>;
                    lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("USER_", (selectedDBLists.Count() == 0 ? "sysadm" : selectedDBLists[0].FinDBUser), Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                    DataSet ds = MvcApplication.OracleDBHelperCore().CustomHelper.CustomDataSet("SYSADM.N$GET_AUCTIONFINLEDGER", lst, Session["SelectedConn"].ToString());

                    lstOut = new FinancialLedgerOutput();
                    lstOut.listFinancialLedgerOutputRow = new List<FinancialLedgerOutputRow>();

                    int c = 0;
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        if (c == 0) lstOut.OpeningBalance = row["RUNBAL"].ToString();
                        else lstOut.ClosingBalance = row["RUNBAL"].ToString();
                        c++;
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
                        lstOut.listFinancialLedgerOutputRow.Add(flo);
                    }
                    Session["ReportHeader1"] = "Client Financial Ledger";
                    Session["ReportHeader2"] = "Date Range : " + model.DateFrom.ToString("dd/MM/yyyy") + " - " + model.DateTo.ToString("dd/MM/yyyy");
                }
                return View(lstOut);
            //}
            //catch (Exception ex)
            //{
            //    return View();
            //}
        }


        public ActionResult DetailFinancialLedger()
        {
            FinancialLedgerInput model = new FinancialLedgerInput();
            model.DateFrom = DateTime.Parse(Session["FinYearFrom"].ToString());
            model.DateTo = DateTime.Parse(Session["FinYearTo"].ToString());
            return View(model);
        }


        public ActionResult DetailFinancialLedgerReport(FinancialLedgerInput model)
        {
            //try
            //{
                string nul = null;
                WebUser webUser = Session["WebUser"] as WebUser;
               
                if (model.DateFrom > DateTime.Parse("01jan1900"))
                {
                    List<Oracle.ManagedDataAccess.Client.OracleParameter> lst = new List<Oracle.ManagedDataAccess.Client.OracleParameter>();

                if (webUser.UserType == UserType.Client)
                {
                    lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("CLIENTCODEFROM", webUser.UserID, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                    lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("CLIENTCODETO", webUser.UserID, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                    lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("BRANCHCODE", nul, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                    lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("SUBBRANCHCODE", nul, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                    lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("GROUPCODE", nul, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                    lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("RMCODE", nul, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));

                }
                else if (webUser.UserType == UserType.Branch)
                {
                    model.ClientCodeTo = model.ClientCodeFrom;

                    DataSet ds1 = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet("SELECT * FROM sysadm.partymst where par_code='" + model.ClientCodeFrom + "' and branchind='" + webUser.UserID + "'", Session["SelectedConn"].ToString());
                    if (ds1.Tables[0].Rows.Count == 0)
                    {
                        TempData["AlertMessage"] = "Invalid Code : " + model.ClientCodeFrom;
                        return RedirectToAction("DetailFinancialLedger", "FinancialLedger");
                    }
                    lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("CLIENTCODEFROM", model.ClientCodeFrom, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                    lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("CLIENTCODETO", model.ClientCodeTo, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));

                    lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("BRANCHCODE", nul, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                    lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("SUBBRANCHCODE", nul, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                    lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("GROUPCODE", nul, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                    lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("RMCODE", nul, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                }
                else if (webUser.UserType == UserType.RM)
                {
                    model.ClientCodeTo = model.ClientCodeFrom;

                    DataSet ds1 = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet("SELECT * FROM sysadm.partymst where par_code='" + model.ClientCodeFrom + "' and rmcode='" + webUser.UserID + "'", Session["SelectedConn"].ToString());
                    if (ds1.Tables[0].Rows.Count == 0)
                    {
                        TempData["AlertMessage"] = "Invalid Code : " + model.ClientCodeFrom;
                        return RedirectToAction("DetailFinancialLedger", "FinancialLedger");
                    }
                    lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("CLIENTCODEFROM", model.ClientCodeFrom, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                    lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("CLIENTCODETO", model.ClientCodeTo, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));

                    lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("BRANCHCODE", nul, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                    lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("SUBBRANCHCODE", nul, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                    lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("GROUPCODE", nul, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                    lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("RMCODE", nul, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                }
                else if (webUser.UserType == UserType.Admin)
                {
                    model.ClientCodeTo = model.ClientCodeFrom;
                    
                    lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("CLIENTCODEFROM", model.ClientCodeFrom, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                    lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("CLIENTCODETO", model.ClientCodeTo, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));

                    lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("BRANCHCODE", nul, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                    lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("SUBBRANCHCODE", nul, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                    lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("GROUPCODE", nul, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                    lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("RMCODE", nul, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                }

                lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("DateFrom", model.DateFrom, Oracle.ManagedDataAccess.Client.OracleDbType.Date, ParameterDirection.Input));
                    lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("DateTo", model.DateTo, Oracle.ManagedDataAccess.Client.OracleDbType.Date, ParameterDirection.Input));
                    lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("IncludeUnReleaseVoucher", model.IncludeUnReleaseVoucher, Oracle.ManagedDataAccess.Client.OracleDbType.Char, ParameterDirection.Input));
                    lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("ExcludeMG13Entries", model.ExcludeMG13Entries, Oracle.ManagedDataAccess.Client.OracleDbType.Char, ParameterDirection.Input));
                    lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("FinancialTranxactionType", "DETAILFIN", Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
               

                    List<DBList> selectedDBLists = Session["SelectedDBLists"] as List<DBList>;
                    lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("USER_", (selectedDBLists.Count() == 0 ? "sysadm" : selectedDBLists[0].FinDBUser), Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                    DataSet ds = MvcApplication.OracleDBHelperCore().CustomHelper.CustomDataSet("SYSADM.N$GET_DETAILFINLEDGER", lst, Session["SelectedConn"].ToString());

                    lstOut = new FinancialLedgerOutput();
                    lstOut.listFinancialLedgerOutputRow = new List<FinancialLedgerOutputRow>();

                lstOut.DateFrom = model.DateFrom;
                lstOut.DateTo = model.DateTo;
                int c = 0;
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        if (c == 0) lstOut.OpeningBalance = row["RUNBAL"].ToString();
                        else lstOut.ClosingBalance = row["RUNBAL"].ToString();
                        c++;
                        FinancialLedgerOutputRow flo = new FinancialLedgerOutputRow();
                        flo.Date = DateTime.Parse(row["DT"].ToString());
                        flo.ValueDate = row["DT1"].ToString()=="" ? DateTime.Parse(row["DT"].ToString()) : DateTime.Parse(row["DT1"].ToString());
                        flo.Narration = row["NARR"].ToString();
                        flo.BillNo = row["BILLNO"].ToString();
                        flo.CHQNO = row["CHQNO"].ToString();
                        flo.Debit = row["DEBIT"].ToString().Replace(" .00", "0.00");
                        flo.Credit = row["CREDIT"].ToString().Replace(" .00", "0.00");
                        flo.RUNBAL = row["RUNBAL"].ToString().Replace(" .00", "0.00");
                        flo.Segment = row["Segment"].ToString();
                        flo.ClientCode = row["PARTY_CD"].ToString();
                        lstOut.listFinancialLedgerOutputRow.Add(flo);
                    }
                    Session["ReportHeader1"] = "Detail Financial Ledger";
                    Session["ReportHeader2"] = "Date Range : " + model.DateFrom.ToString("dd/MM/yyyy") + " - " + model.DateTo.ToString("dd/MM/yyyy");
                }
                return View(lstOut);
            //}
            //catch (Exception ex)
            //{
            //    return View();
            //}
        }


        public ActionResult ScripWiseLedger()
        {
            return View();
        }


        public ActionResult M2MLedger()
        {
            return View();
        }


        public ActionResult ClientMargin()
        {
            return View();
        }
    }
}