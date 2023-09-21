using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;

namespace Rising.WebRise.Controllers.DP
{
    using Rising.WebRise.Models;
    using OracleDBHelper;

    public class DPController : Controller
    {
        // GET: DP
        public ActionResult Holding()
        {
            List<DBList> selectedDBLists = Session["SelectedDBLists"] as List<DBList>;
            if (selectedDBLists.FindAll(a => a.Group == "DP").Count == 0)
            {
                TempData["AlertMessage"] = "Select Atleast One DP Segment...";
                return RedirectToAction("Index", "ClientHome");
            }
            DPHoldingInput model = new DPHoldingInput();
            model.AsOnDate = DateTime.Parse(Session["FinYearTo"].ToString());
            return View(model);
        }


        public static DPHoldingOutput lstOut;

        public ActionResult DPHoldingReport(string activeMenu, DPHoldingInput model)
        {
            //try
            //{
            WebUser webUser = Session["WebUser"] as WebUser;
            List<Oracle.ManagedDataAccess.Client.OracleParameter> lst = new List<Oracle.ManagedDataAccess.Client.OracleParameter>();
            lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("clientcode_", webUser.DPACNO, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
            lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("AsOnDate_", model.AsOnDate, Oracle.ManagedDataAccess.Client.OracleDbType.Date, ParameterDirection.Input));
            DataSet ds = MvcApplication.OracleDBHelperCore().CustomHelper.CustomDataSet("SYSADM.N$GET_DPHolding", lst, Session["SelectedConn"].ToString());

            lstOut = new DPHoldingOutput();
            lstOut.listDPHoldingOutputRow = new List<DPHoldingOutputRow>();
            lstOut.ClientCode = webUser.UserID;
            lstOut.ClientName = webUser.UserName;

            int c = 0;
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                if (c == 0)
                {
                    lstOut.ClientCode = row["Clientid"].ToString();
                    lstOut.ClientName = row["Name"].ToString();
                }
                c++;
                DPHoldingOutputRow bdo = new DPHoldingOutputRow();
                bdo.ScripISIN = row["ISIN"].ToString();
                bdo.ScripName = row["SERIES"].ToString();
                bdo.Pledge = int.Parse(row["Pledge"].ToString());
                bdo.Free = int.Parse(row["Free"].ToString());
                bdo.Demat = int.Parse(row["Demat"].ToString());
                bdo.Lockin = int.Parse(row["Lockin"].ToString());
                bdo.Emark = int.Parse(row["Emark"].ToString());
                bdo.NetStock = int.Parse(row["NetStock"].ToString());
                lstOut.listDPHoldingOutputRow.Add(bdo);
            }
            Session["ReportHeader1"] = "DP Holding";
            Session["ReportHeader2"] = "As On Date : " + model.AsOnDate.ToString("dd/MM/yyyy");

            return View(lstOut);

            //}
            //catch (Exception ex)
            //{
            //    return View();
            //}
        }


        public ActionResult ClientMasterInput()
        {
            List<DBList> selectedDBLists = Session["SelectedDBLists"] as List<DBList>;
            if (selectedDBLists.FindAll(a => a.Group == "Demat").Count == 0)
            {
                TempData["AlertMessage"] = "Select Atleast One DP Segment...";
                return RedirectToAction("Index", "ClientHome");
            }
            DPClientCodeInput model = new DPClientCodeInput();
            
            return View(model);
        }

        public ActionResult ClientMaster()
        {
            //try
            //{
          
            WebUser webUser = Session["WebUser"] as WebUser;
            if (webUser.UserType != UserType.Client) return RedirectToAction("ClientMasterInput", "DP");
                List<Oracle.ManagedDataAccess.Client.OracleParameter> lst = new List<Oracle.ManagedDataAccess.Client.OracleParameter>();
            lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("CLIENTID_", webUser.UserID, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
            DataSet ds = MvcApplication.OracleDBHelperCore().CustomHelper.CustomDataSet("SYSADM.N$GET_CDSL_CLIENTMASTER", lst, Session["SelectedConn"].ToString());

            Session["ReportHeader1"] = "Client Master";
            Session["ReportHeader2"] = "";


            ViewBag.DS = ds;
            return View(ViewBag);

            //}
            //catch (Exception ex)
            //{
            //    return View();
            //}
        }

        public ActionResult ClientMasterBranch(DPClientCodeInput model)
        {
            //try
            //{
            
            List<Oracle.ManagedDataAccess.Client.OracleParameter> lst = new List<Oracle.ManagedDataAccess.Client.OracleParameter>();
            lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("CLIENTID_", model.ClientCodeFrom, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
            DataSet ds = MvcApplication.OracleDBHelperCore().CustomHelper.CustomDataSet("SYSADM.N$GET_CDSL_CLIENTMASTER", lst, Session["SelectedConn"].ToString());

            Session["ReportHeader1"] = "Client Master";
            Session["ReportHeader2"] = "";


            ViewBag.DS = ds;
            return View(ViewBag);

            //}
            //catch (Exception ex)
            //{
            //    return View();
            //}
        }

        public ActionResult TrxnStatement()
        {
            DPTrnsactionInput model = new DPTrnsactionInput();
            model.DateFrom = DateTime.Parse(Session["FinYearFrom"].ToString());
            model.DateTo = DateTime.Parse(Session["FinYearTo"].ToString());
            return View(model);
        }

        public static DPTrnsactionOutput lstOut1;
        public ActionResult Ledger(string activeMenu, DPTrnsactionInput model)
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
                    lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("CLIENTID_", webUser.UserID, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                }
                else if (webUser.UserType == UserType.Branch)
                {
                    model.ClientCodeTo = model.ClientCodeFrom;
                    DataSet ds1 = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet("SELECT * FROM sysadm.partymst where par_code='" + model.ClientCodeFrom + "' and branchind='" + webUser.UserID + "'", Session["SelectedConn"].ToString());
                    if (ds1.Tables[0].Rows.Count == 0)
                    {
                        TempData["AlertMessage"] = "Invalid Code : " + model.ClientCodeFrom;
                        return RedirectToAction("TrxnStatement", "DP");

                    }

                    lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("CLIENTID_", model.ClientCodeFrom, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                }

                else if (webUser.UserType == UserType.RM)
                {
                    model.ClientCodeTo = model.ClientCodeFrom;
                    DataSet ds1 = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet("SELECT * FROM sysadm.partymst where par_code='" + model.ClientCodeFrom + "' and rmcode='" + webUser.UserID + "'", Session["SelectedConn"].ToString());
                    if (ds1.Tables[0].Rows.Count == 0)
                    {
                        TempData["AlertMessage"] = "Invalid Code : " + model.ClientCodeFrom;
                        return RedirectToAction("TrxnStatement", "DP");

                    }

                    lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("CLIENTID_", model.ClientCodeFrom, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                }
                else if (webUser.UserType == UserType.Admin)
                {
                    model.ClientCodeTo = model.ClientCodeFrom;
                   
                    lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("CLIENTID_", model.ClientCodeFrom, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                }
                lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("DateFrom", model.DateFrom, Oracle.ManagedDataAccess.Client.OracleDbType.Date, ParameterDirection.Input));
                lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("DateTo", model.DateTo, Oracle.ManagedDataAccess.Client.OracleDbType.Date, ParameterDirection.Input));

                List<DBList> selectedDBLists = Session["SelectedDBLists"] as List<DBList>;
                lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("USER_", (selectedDBLists.Count() == 0 ? "sysadm" : selectedDBLists[0].FinDBUser), Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                DataSet ds = MvcApplication.OracleDBHelperCore().CustomHelper.CustomDataSet("SYSADM.N$GET_CDSL_Transaction", lst, Session["SelectedConn"].ToString());

                lstOut1 = new DPTrnsactionOutput();
                lstOut1.DateFrom = model.DateFrom;
                lstOut1.DateTo = model.DateTo;
                lstOut1.listDPTrnsactionOutputRow = new List<DPTrnsactionOutputRow>();
                lstOut1.ClientCode = webUser.UserID;
                lstOut1.ClientName = webUser.UserName;

                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    DPTrnsactionOutputRow flo = new DPTrnsactionOutputRow();
                    flo.Date = DateTime.Parse(row["WDATE"].ToString());
                    flo.Narration = row["NARR"].ToString();
                    flo.RefNo = row["REFNO"].ToString();
                    flo.ISIN = row["ISIN"].ToString();
                    flo.ShareName = row["SCRIP"].ToString();
                    flo.Debit = decimal.Parse(row["DEBIT"].ToString().Replace(" .00", "0.00"));
                    flo.Credit = decimal.Parse(row["CREDIT"].ToString().Replace(" .00", "0.00"));
                    flo.ClientCode = row["CLIENT"].ToString();
                    lstOut1.listDPTrnsactionOutputRow.Add(flo);
                }
                Session["ReportHeader1"] = "Client Demat Transaction";
                Session["ReportHeader2"] = "Date Range : " + model.DateFrom.ToString("dd/MM/yyyy") + " - " + model.DateTo.ToString("dd/MM/yyyy");
            }
            return View(lstOut1);
            //}
            //catch (Exception ex)
            //{
            //    return View();
            //}
        }
    }
}