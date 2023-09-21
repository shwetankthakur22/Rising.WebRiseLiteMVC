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
    using System.Configuration;
    using Models.Reports;
    using Rising.WebRise.Repositories.Implementation;
    using System.IO;

    public class ReportController : Controller
    {
        string dbuser = ConfigurationManager.AppSettings["DBUSER"];
        public ActionResult Index(FinancialLedgerInput model)
        {

            CommonRef commref = new CommonRef();
            string code = null;
            var dt = TempData["modeldata"];
            model.financialLedgerOutputs = (FinancialLedgerOutput)dt;
            if (model.financialLedgerOutputs !=null)
            {
                model.ClientCodeFrom = model.financialLedgerOutputs.ClientCode;
                model.ClosingBalance = model.financialLedgerOutputs.ClosingBalance;
                model.OpeningBalance = model.financialLedgerOutputs.OpeningBalance;
                model.DateFrom = model.financialLedgerOutputs.DateFrom;
                model.DateTo = model.financialLedgerOutputs.DateTo;
                //model.IncludeUnReleaseVoucher = model.UnReleasedVoucherBal;
              
                //model.UnReleasedVoucherBal = model.financialLedgerOutputs.IncludeUnReleaseVoucher;

            }
            if (Session["WebUser"] == null)
            {
                TempData["AlertMessage"] = "Session Time Out Please Login Again";
                return RedirectToAction("Index", "Login");

            }
            else
            {
                if (Session["BranchSearchCode"] != null) code = Session["BranchSearchCode"].ToString();
                //FinancialLedgerInput model = new FinancialLedgerInput();
                model.DateFrom = DateTime.Parse(Session["FinYearFrom"].ToString());
                model.DateTo = DateTime.Parse(Session["FinYearTo"].ToString());
                model.ClientCodeFrom = code;
                model.ClientCodeTo = code;
                model.ClientName = code;
                SelectList lst4 = new SelectList(Enum.GetValues(typeof(enumFinancialTranxactionType)).Cast<enumFinancialTranxactionType>().Select(v => v.ToString()).ToList());
                ViewBag.enumFinancialTranxactionType = lst4; 
               var comref = commref.GetExchange();
                ViewBag.ExchangeList = new SelectList(comref,"","ExchangeName");

                return View(model);
            }
        }

        public ActionResult GetData(string code)
        {

            FinancialLedgerInput model = new FinancialLedgerInput();
            //model.DateFrom = DateTime.Parse(Session["FinYearFrom"].ToString());
            //model.DateTo = DateTime.Parse(Session["FinYearTo"].ToString());
            model.ClientCodeFrom = code;
            // model.ClientName = "Rahul";
            if (code != "")
            {
                string nul = null;
                WebUser webUser = Session["WebUser"] as WebUser;
                Session["ReportClientCode"] = webUser.UserID;


                List<Oracle.ManagedDataAccess.Client.OracleParameter> lst = new List<Oracle.ManagedDataAccess.Client.OracleParameter>();
                if (webUser.UserType == UserType.Client)
                {
                    lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("CLIENTCODEFROM", webUser.UserID, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                    lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("CLIENTCODETO", webUser.UserID, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                    lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("BRANCHCODE", nul, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                    lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("SUBBRANCHCODE", nul, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                    lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("GROUPCODE", nul, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                    lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("RMCODE", nul, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));

                    Session["ReportClientCode"] = webUser.UserID;
                    Session["ReportClientName"] = webUser.UserName;
                }
                else if (webUser.UserType == UserType.Branch)
                {
                    model.ClientCodeTo = model.ClientCodeFrom;

                    if (model.ClientCodeFrom != null)
                    {

                        DataSet ds1 = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet("SELECT * FROM " + dbuser + ".partymst where par_code='" + model.ClientCodeFrom + "' and branchind='" + webUser.UserID + "'", Session["SelectedConn"].ToString());
                        if (ds1.Tables[0].Rows.Count == 0)
                        {
                            TempData["AlertMessage"] = "Invalid Code : " + model.ClientCodeFrom;
                            return RedirectToAction("Index", "FinancialLedger");
                        }

                        Session["ReportClientCode"] = ds1.Tables[0].Rows[0]["par_code"].ToString();
                        Session["ReportClientName"] = ds1.Tables[0].Rows[0]["par_name"].ToString();
                        Session["ReportClientPanNo"] = ds1.Tables[0].Rows[0]["itaxno"].ToString();
                    }


                }

                else if (webUser.UserType == UserType.RM)
                {
                    model.ClientCodeTo = model.ClientCodeFrom;
                    if (model.ClientCodeFrom != null)
                    {
                        DataSet ds1 = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet("SELECT * FROM " + dbuser + ".partymst where par_code='" + model.ClientCodeFrom + "' and RMCODE='" + webUser.UserID + "'", Session["SelectedConn"].ToString());
                        if (ds1.Tables[0].Rows.Count == 0)
                        {
                            TempData["AlertMessage"] = "Invalid Code : " + model.ClientCodeFrom;
                            return RedirectToAction("Index", "FinancialLedger");
                        }

                        Session["ReportClientCode"] = ds1.Tables[0].Rows[0]["par_code"].ToString();
                        Session["ReportClientName"] = ds1.Tables[0].Rows[0]["par_name"].ToString();
                        Session["ReportClientPanNo"] = ds1.Tables[0].Rows[0]["itaxno"].ToString();
                    }

                }
                else if (webUser.UserType == UserType.Admin)
                {
                    model.ClientCodeTo = model.ClientCodeFrom;
                    if (model.ClientCodeFrom != null)
                    {
                        DataSet ds1 = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet("SELECT * FROM " + dbuser + ".cupartymst where par_code='" + model.ClientCodeFrom + "'", Session["SelectedConn"].ToString());
                        if (ds1.Tables[0].Rows.Count == 0)
                        {
                            //TempData["AlertMessage"] = "Invalid Code : " + model.ClientCodeFrom;
                            return RedirectToAction("Index", "FinancialLedger");
                        }

                        Session["ReportClientCode"] = ds1.Tables[0].Rows[0]["par_code"].ToString();
                        Session["ReportClientName"] = ds1.Tables[0].Rows[0]["par_name"].ToString();
                        Session["ReportClientPanNo"] = ds1.Tables[0].Rows[0]["itaxno"].ToString();
                    }

                }
                model.ClientName = Session["ReportClientName"].ToString();

            }
            else
            {
                model.ClientCodeFrom = "";
                model.ClientName = "";
            }





            return Json(model, JsonRequestBehavior.AllowGet);
        }


        public static FinancialLedgerOutput lstOut;
        public ActionResult Ledger(string activeMenu, FinancialLedgerInput model)
            
        {
            string OrderBy = model.OrderBy;


            if(OrderBy.Equals("Date"))
            {
                OrderBy = "dt";
            }
            else if(OrderBy.Equals("Narration"))
            {
                OrderBy = "narr";
            }
            else if(OrderBy.Equals("Value Date"))
            {
                OrderBy = "dt1";
            }
            else
                  {
                OrderBy = "BillNo";
            }
            if (model.ClientCodeFrom != null) model.ClientCodeFrom = model.ClientCodeFrom.ToUpper();

            

            if (Session["WebUser"] == null)
            {
                TempData["AlertMessage"] = "Session Time Out Please Login Again";
                return RedirectToAction("Index", "Login");
            }
            else
            {

                //string dd = model.DateTo.ToString("ddMMMyyyy");
                //try
                //{  
                string nul = null;
                WebUser webUser = Session["WebUser"] as WebUser;
                Session["ReportClientCode"] = webUser.UserID;

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

                        Session["ReportClientCode"] = webUser.UserID;
                        Session["ReportClientName"] = webUser.UserName;
                    }
                    else if (webUser.UserType == UserType.Branch)
                    {
                        model.ClientCodeTo = model.ClientCodeFrom;

                        if (model.ClientCodeFrom != null)
                        {

                            DataSet ds1 = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet("SELECT * FROM " + dbuser + ".partymst where par_code='" + model.ClientCodeFrom + "' and branchind='" + webUser.UserID + "'", Session["SelectedConn"].ToString());
                            if (ds1.Tables[0].Rows.Count == 0)
                            {
                                TempData["AlertMessage"] = "Invalid Code : " + model.ClientCodeFrom;
                                return RedirectToAction("Index", "FinancialLedger");
                            }

                            Session["ReportClientCode"] = ds1.Tables[0].Rows[0]["par_code"].ToString();
                            Session["ReportClientName"] = ds1.Tables[0].Rows[0]["par_name"].ToString();
                            Session["ReportClientPanNo"] = ds1.Tables[0].Rows[0]["itaxno"].ToString();
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
                        if (model.ClientCodeFrom != null)
                        {
                            DataSet ds1 = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet("SELECT * FROM " + dbuser + ".partymst where par_code='" + model.ClientCodeFrom + "' and RMCODE='" + webUser.UserID + "'", Session["SelectedConn"].ToString());
                            if (ds1.Tables[0].Rows.Count == 0)
                            {
                                TempData["AlertMessage"] = "Invalid Code : " + model.ClientCodeFrom;
                                return RedirectToAction("Index", "FinancialLedger");
                            }

                            Session["ReportClientCode"] = ds1.Tables[0].Rows[0]["par_code"].ToString();
                            Session["ReportClientName"] = ds1.Tables[0].Rows[0]["par_name"].ToString();
                            Session["ReportClientPanNo"] = ds1.Tables[0].Rows[0]["itaxno"].ToString();
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
                        model.Exchange = model.Exchange;
                        if (model.ClientCodeFrom != null)
                        {
                            DataSet ds1 = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet("SELECT * FROM " + dbuser + ".cupartymst where par_code='" + model.ClientCodeFrom + "'", Session["SelectedConn"].ToString());
                            if (ds1.Tables[0].Rows.Count == 0)
                            {
                                TempData["AlertMessage"] = "Invalid Code : " + model.ClientCodeFrom;
                                return RedirectToAction("Index", "FinancialLedger");
                            }

                            Session["ReportClientCode"] = ds1.Tables[0].Rows[0]["par_code"].ToString();
                            Session["ReportClientName"] = ds1.Tables[0].Rows[0]["par_name"].ToString();
                            Session["ReportClientPanNo"] = ds1.Tables[0].Rows[0]["itaxno"].ToString();
                        }
                        if (1 == 1)
                        {
                            model.ClientCodeTo = model.ClientCodeFrom;

                            lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("CLIENTCODEFROM", model.ClientCodeFrom, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                            Session["ReportClientCode"] = model.ClientCodeFrom;
                            Session["ReportClientName"] = model.ClientCodeFrom;

                            lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("CLIENTCODETO", model.ClientCodeTo, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                            lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("BRANCHCODE", nul, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                            lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("SUBBRANCHCODE", nul, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                            lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("GROUPCODE", nul, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                            lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("RMCODE", nul, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));


                        }
                    }

                    lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("DateFrom", model.DateFrom, Oracle.ManagedDataAccess.Client.OracleDbType.Date, ParameterDirection.Input));
                    lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("DateTo", model.DateTo, Oracle.ManagedDataAccess.Client.OracleDbType.Date, ParameterDirection.Input));
                    lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("IncludeUnReleaseVoucher", model.IncludeUnReleaseVoucher, Oracle.ManagedDataAccess.Client.OracleDbType.Char, ParameterDirection.Input));
                    lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("ExcludeMG13Entries", model.ExcludeMG13Entries, Oracle.ManagedDataAccess.Client.OracleDbType.Char, ParameterDirection.Input));
                    lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("FinancialTranxactionType", nul, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));

                    List<DBList> selectedDBLists = Session["SelectedDBLists"] as List<DBList>;
                    lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("USER_", (selectedDBLists.Count() == 0 ? "sysadm" : selectedDBLists[0].FinDBUser), Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                    //DataSet ds = MvcApplication.OracleDBHelperCore().CustomHelper.CustomDataSet("SYSADM.N$GET_FinLedger", lst, Session["SelectedConn"].ToString());

                    string qry = "SELECT X.Catg,PARTY_CD,DT, DT1,NARR, BILLNO, settno,TO_CHAR(X.DEBIT, '99,99,99,999.99') DEBIT, TO_CHAR(X.CREDIT, '99,99,99,999.99') CREDIT," +
                                 "TO_CHAR(SUM(X.CREDIT - X.DEBIT) OVER(PARTITION BY PARTY_CD ORDER BY PARTY_CD,"+ OrderBy+"    ROWS BETWEEN UNBOUNDED PRECEDING AND CURRENT ROW), '99,99,99,999.99')  RUNBAL, EXCHANGE,par_name " +
                                 "FROM(" +
                        " SELECT Catg,PARTY_CD,DT, DT1,NARR, BILLNO,  settno,DEBIT, CREDIT,     EXCHANGE FROM ( ";

                    qry = qry + " SELECT 1 Catg,PARTY_CD,TO_DATE('" + model.DateFrom.ToString("ddMMMyyyy") + "','DDMONYYYY')-1 DT,TO_DATE('" + model.DateFrom.ToString("ddMMMyyyy") + "','DDMONYYYY')-1 DT1,'Opening Balance' NARR,'' BILLNO,'-' settno   ,DECODE(SIGN(SUM(NVL(CREDIT,0)-NVL(DEBIT,0))),-1,ABS(SUM(NVL(CREDIT,0)-NVL(DEBIT,0))),0) DEBIT     ,DECODE(SIGN(SUM(NVL(CREDIT,0)-NVL(DEBIT,0))),-1,0,ABS(SUM(NVL(CREDIT,0)-NVL(DEBIT,0)))) CREDIT, '' EXCHANGE  " +
                    " FROM " + dbuser + ".PARTYTRN WHERE WDATE<'" + model.DateFrom.ToString("ddMMMyyyy") + "'    ";
                    if (!model.Exchange.Equals("ALL"))
                    { qry = qry + "  AND exchange= '" + model.Exchange + "' "; }

                    if (webUser.UserType == UserType.Client)
                    { qry = qry + " and    PARTY_CD='" + webUser.UserID + "'  "; }
                    else if (webUser.UserType == UserType.Branch)
                    {
                        model.ClientCodeTo = model.ClientCodeFrom;

                        if (model.ClientCodeFrom != null)
                        { qry = qry + " and    PARTY_CD='" + model.ClientCodeFrom + "'  "; }
                        else { }
                    }
                    else if (webUser.UserType == UserType.Admin)
                    { qry = qry + " and    PARTY_CD='" + model.ClientCodeFrom + "'  "; }
                    if (model.ExcludeMG13Entries == true) { qry = qry + "  AND trn_type<>'M'   "; }
                    qry = qry + "GROUP BY PARTY_CD UNION ALL ";

                    qry = qry + "SELECT 2 Catg, PARTY_CD,WDATE DT, VALUE_DATE DT1,NARR,  CASE WHEN TRN_TYPE='M' AND EXCHANGE='NSE' THEN 'CAPMARG' ELSE BILLNO END BILLNO,nvl(settno,trn_flag) settno,NVL(DEBIT,0) DEBIT,NVL(CREDIT,0) CREDIT, NVL(EXCHANGE, 'NSE') EXCHANGE    " +
                        " FROM  " + dbuser + ".PARTYTRN WHERE WDATE>='" + model.DateFrom.ToString("ddMMMyyyy") + "' AND WDATE<='" + model.DateTo.ToString("ddMMMyyyy") + "'  ";

                    if (model.IngnoreOpeningBalance != false)
                    {
                        qry = qry + " and    narr not like '%Op.Balance%'  ";
                    }
                    if (webUser.UserType == UserType.Client)
                    {
                        qry = qry + " AND    PARTY_CD ='" + webUser.UserID + "' ";
                    }
                    else if (webUser.UserType == UserType.Branch)
                    {
                        model.ClientCodeTo = model.ClientCodeFrom;

                        if (model.ClientCodeFrom != null)
                        {
                            qry = qry + " and    PARTY_CD='" + model.ClientCodeFrom + "'  ";
                        }
                        else
                        {

                        }
                    }

                    else if (webUser.UserType == UserType.Admin)
                    {
                        qry = qry + " and    PARTY_CD='" + model.ClientCodeFrom + "'  ";
                    }
                    if (!model.Exchange.Equals("ALL"))
                    { qry = qry + "  AND exchange= '" + model.Exchange + "' "; }
                    if (model.ExcludeMG13Entries == true) { qry = qry + "  AND trn_type<>'M'   "; }
                
                    if (model.FinancialTranxactionType == enumFinancialTranxactionType.VoucherAll)
                    { qry = qry + "  AND SUBSTR(BILLNO,1,2) IN ('JV','BR','BP') "; }
                    else if (model.FinancialTranxactionType == enumFinancialTranxactionType.BankEntrys)
                    { qry = qry + "  AND SUBSTR(BILLNO,1,2) IN ('BR','BP') "; }
                    else if (model.FinancialTranxactionType == enumFinancialTranxactionType.BankPayment)
                    { qry = qry + "  AND SUBSTR(BILLNO,1,2) IN ('BP') "; }
                    else if (model.FinancialTranxactionType == enumFinancialTranxactionType.BankReceive)
                    { qry = qry + "  AND SUBSTR(BILLNO,1,2) IN ('BR') "; }
                    else if (model.FinancialTranxactionType == enumFinancialTranxactionType.JournalEntrys)
                    { qry = qry + "  AND SUBSTR(BILLNO,1,2) IN ('JV') "; }
                    else if (model.FinancialTranxactionType == enumFinancialTranxactionType.FutureEntries)
                    { qry = qry + "  AND SUBSTR(BILLNO,1,6) IN ('DLYMTM','FUTEXP') "; }
                    else if (model.FinancialTranxactionType == enumFinancialTranxactionType.DailyMTMBills)
                    { qry = qry + "  AND SUBSTR(BILLNO,1,6) IN ('DLYMTM','OPTDLY') "; }
                    else if (model.FinancialTranxactionType == enumFinancialTranxactionType.CashBook)
                    { qry = qry + "  AND SUBSTR(BILLNO,1,2) IN ('JV') "; }
                    else if (model.FinancialTranxactionType == enumFinancialTranxactionType.CUBills)
                    { qry = qry + "  AND SUBSTR(BILLNO,1,6)  IN ('DLYMTM','FUTEXP','OPTDLY','OPTEXP') "; }
                    else if (model.FinancialTranxactionType == enumFinancialTranxactionType.AdditionalMargin)
                    { qry = qry + "  AND SUBSTR(BILLNO,1,4) IN ('CDSM') and TRN_TYPE='M' "; }
                    else if (model.FinancialTranxactionType == enumFinancialTranxactionType.Margin)
                    { qry = qry + "  AND SUBSTR(BILLNO,1,4) IN ('CDSM') and TRN_TYPE='M' "; }
                    else if (model.FinancialTranxactionType == enumFinancialTranxactionType.OptionBills)
                    { qry = qry + "  AND SUBSTR(BILLNO,1,6) IN  ('OPTDLY','OPTEXP') "; }
                    else if (model.FinancialTranxactionType == enumFinancialTranxactionType.ExpiryBills)
                    { qry = qry + "  AND SUBSTR(BILLNO,1,6) IN  ('FUTEXP','OPTEXP') "; }
                    else
                    { qry = qry; }

                    if (model.SearchNarration != "" && model.SearchNarration != null) qry = qry + "  AND narr like '%" + model.SearchNarration + "%' ";

                    if(model.IncludeUnReleaseVoucher==true)
                    {
                        qry = qry + " union all " +
                                    " select 2 Catg,clientcode, voudate, voudate, NARR, VOUNO, nvl(settno,'-') settno, debit, credit, EXCHANGE from sysadm.voufile " +
                                    " where STATUS is null and VOUNO = 'UnPosted' and voudate>= '"+ model.DateFrom.ToString("ddMMMyyyy") + "' AND voudate<= '"+ model.DateTo.ToString("ddMMMyyyy") + "' and clientcode = '"+model.ClientCodeFrom.ToUpper()+"' ";
                        if (!model.Exchange.Equals("ALL"))
                        { qry = qry + "  AND exchange= '" + model.Exchange + "' "; }
                    }



               
                   
                    qry = qry + "    )  Order By Catg," + OrderBy + " ) X, " + dbuser+".CUPARTYMST Y WHERE X.PARTY_CD=Y.PAR_CODE AND Y.GROUPLEVEL1='Sundry Debtors/Creditors' ";

                    
                    if (webUser.UserType == UserType.Branch)
                    {
                        model.ClientCodeTo = model.ClientCodeFrom;

                        if (model.ClientCodeFrom != null)
                        {
                            qry = qry + " and    PARTY_CD='" + model.ClientCodeFrom + "'  ";
                        }
                        else
                        {
                            qry = qry + " and    Y.BRANCHIND ='" + webUser.UserID + "'  ";
                        }
                    }
                    if (webUser.UserType == UserType.RM)
                    {
                        model.ClientCodeTo = model.ClientCodeFrom;

                        if (model.ClientCodeFrom != null)
                        {
                            qry = qry + " and    PARTY_CD='" + model.ClientCodeFrom + "'  ";
                        }
                        else
                        {
                            qry = qry + " and    Y.rmcode ='" + webUser.UserID + "'  ";
                        }
                    }


                    if (selectedDBLists[0].Group == "Commodity")
                    {
                        qry = qry.Replace("SYSADM.", "COMM.");
                    }
                    if (selectedDBLists[0].Group == "Currency123")
                    {
                        qry = qry.Replace("SYSADM.", "CURR.");
                    }
                    
                    DataSet ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet(qry, Session["SelectedConn"].ToString());


                    StreamWriter sw = new StreamWriter("E:\\Query.txt");
                    sw.WriteLine(qry);
                    sw.Close();



                    lstOut = new FinancialLedgerOutput();
                    lstOut.listFinancialLedgerOutputRow = new List<FinancialLedgerOutputRow>();
                    lstOut.DateFrom = model.DateFrom;
                    lstOut.DateTo = model.DateTo;
                    lstOut.ClientCode = model.ClientCodeFrom;
                    int c = 0;
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        if (c == 0)
                            lstOut.OpeningBalance = row["RUNBAL"].ToString();
                        else
                            lstOut.ClosingBalance = row["RUNBAL"].ToString();
                        c++;
                        FinancialLedgerOutputRow flo = new FinancialLedgerOutputRow();
                        flo.Date = DateTime.Parse(row["DT"].ToString());
                        flo.ValueDate = DateTime.Parse(row["DT1"].ToString());
                        flo.Narration = row["NARR"].ToString();
                        flo.BillNo = row["BILLNO"].ToString();
                        flo.SETTNO = row["SETTNO"].ToString();
                        flo.Debit = row["DEBIT"].ToString().Replace(" .00", "0.00");
                        flo.Credit = row["CREDIT"].ToString().Replace(" .00", "0.00");
                        flo.RUNBAL = row["RUNBAL"].ToString().Replace(" .00", "0.00");
                        flo.Segment = row["EXCHANGE"].ToString();
                        flo.ClientCode = row["PARTY_CD"].ToString();
                        flo.ClientName = row["par_name"].ToString();
                        lstOut.listFinancialLedgerOutputRow.Add(flo);
                    }

                   

                    Session["ReportHeader1"] = "Client Financial Ledger";
                    Session["ReportHeader2"] = "Date Range : " + model.DateFrom.ToString("dd-MM-yyyy") + " - " + model.DateTo.ToString("dd-MM-yyyy");
                    //return View(lstOut);
                    //return PartialView("_Ledger", lstOut);

                    model.financialLedgerOutputs = lstOut;
                    TempData["modeldata"] = model.financialLedgerOutputs;
                    //return View(model);
                    return RedirectToAction("Index", "Report");
                }

                 return RedirectToAction("Index", "Report");

                //return PartialView("_Ledger", lstOut);


            }
           
        }

        //public ActionResult Ledger(string code)
        //{
        //    FinancialLedgerInput financial = new FinancialLedgerInput();
        //    string qry ="" ;
        //    DataSet ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet(qry, Session["SelectedConn"].ToString());
        //    if (ds.Tables[0].Rows.Count != 0)
        //    {
        //        //financial.OpeningBalance = ds.Tables[0].Rows[""].ToString;
        //        //financial.ClosingBalance = ds.Tables[0].Rows[""].ToString;
        //        //financial.SecurityBalance = ds.Tables[0].Rows[""].ToString;
        //        //financial.DailyMarginBalance = ds.Tables[0].Rows[""].ToString;
        //        //financial.UnReleasedVoucherBal = ds.Tables[0].Rows[""].ToString;
        //        //financial.MtmAcBal = ds.Tables[0].Rows[""].ToString;
        //        //financial.CashMarginBal = ds.Tables[0].Rows[""].ToString;
        //        //financial.ClosingBalOtherBal = ds.Tables[0].Rows[""].ToString;
        //    }
        //    return Json(financial, JsonRequestBehavior.AllowGet);
        //}

        

        // GET: Report
        public ActionResult DematLedger(string code)
        {
            if (Session["WebUser"] == null)
            {
                TempData["AlertMessage"] = "Session Time Out Please Login Again";
                return RedirectToAction("Index", "Login");

            }
            else
            {
                if (Session["BranchSearchCode"] != null) code = Session["BranchSearchCode"].ToString();
                DematLedgerInput model = new DematLedgerInput();
                model.DateFrom = DateTime.Parse(Session["FinYearFrom"].ToString());
                model.DateTo = DateTime.Parse(Session["FinYearTo"].ToString());
                model.ClientCodeFrom = code;
                return View(model);


            }
        }


        public ActionResult DematLedgerReport(DematLedgerInput model)
        {
            try
            {
                WebUser webUser = Session["WebUser"] as WebUser;
                if (webUser == null) return null;
                model.DSOut = new DataSet();
                string str = "select slno,to_char(ddate, 'dd-MM-yyyy') ddate,scripcd,sh_name,status,settno,debit,credit,scrollno,clientdp,acno,isincode,clientto,scrollno scrollno1,clientcd,rowid1,mktto from (  SELECT '2' slno,d.DDATE , d.scripcd,s.SH_NAME,  decode(scrolldate,Null,decode(d.credit,0,'SALE','PUR'),decode(d.debit,0,'RECV','DELV')) AS status,  d.SETTNO, nvl(d.DEBIT,0) Debit, nvl(d.CREDIT,0) Credit, d.SCROLLNO,  d.ACNO, d.ISINCODE,DECODE('N','Y',DECODE(ACSTATUS,'Disabled','',CLIENTTO),CLIENTTO) CLIENTTO,DECODE('N','Y',DECODE(ACSTATUS,'Disabled','', clientdp), clientdp) clientdp,DECODE('N','Y',DECODE(ACSTATUS,'Disabled','',CLIENTcd),CLIENTcd) clientcd,cast(d.rowid as varchar2(20)) rowid1,mktto FROM   " + dbuser + ".dematlgr d, " + dbuser + ".sharemst s," + dbuser + ".PARTYMST p  WHERE TRXTYPE='C' AND d.SCRIPCD=s.SH_CODE AND d.clientcd=p.PAR_CODE  AND  ddate >=to_date('" + model.DateFrom.ToString("dd-MM-yyyy") + "','dd-mm-yyyy')  and ddate<=to_date('" + model.DateTo.ToString("dd-MM-yyyy") + "','dd-mm-yyyy')  ";

                if (model.ClientCodeFrom != null) { if (model.ClientCodeFrom != "") str = str + " and d.clientcd='" + model.ClientCodeFrom + "'  "; }

                str = str + " and (scrollno not like 'PP%' or scrollno is null) union all  select '1' slno,to_date('" + model.DateFrom.ToString("dd-MM-yyyy") + "','dd-mm-yyyy') ddate,d.scripcd,sh.sh_name,'****','OPBAL',sum(nvl(d.debit,0)) Debit,sum(nvl(d.credit,0)) Credit  ,null,null,null,null,null,DECODE('N','Y',DECODE(ACSTATUS,'Disabled','',CLIENTCD),CLIENTCD) clientcd,null,null  from " + dbuser + ".dematlgr d," + dbuser + ".sharemst sh," + dbuser + ".PARTYMST P  where d.ddate<to_date('" + model.DateTo.ToString("dd-MM-yyyy") + "','dd-mm-yyyy') and d.scripcd=sh.sh_code AND d.clientCD=p.PAR_CODE   ";

                if (model.ClientCodeFrom != null) { if (model.ClientCodeFrom != "") str = str + " and d.clientcd='" + model.ClientCodeFrom + "'  "; }

                str = str + " and (scrollno not like 'PP%' or scrollno is null) and trxtype='C' group by scripcd,sh_name,DECODE('N','Y',DECODE(ACSTATUS,'Disabled','',CLIENTCD),CLIENTCD) having sum(nvl(d.debit,0))<>0 or sum(nvl(d.credit,0))<>0 ) order by SH_NAME,slno,ddate ";
                model.DSOut = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet(str, Session["SelectedConn"].ToString());

                return View(model);
            }
            catch (Exception ex)
            {
                return View();
            }
        }



        public ActionResult BillSummary(string code)
        {
            if (Session["WebUser"] == null)
            {
                TempData["AlertMessage"] = "Session Time Out Please Login Again";
                return RedirectToAction("Index", "Login");

            }
            else
            {
                if (Session["BranchSearchCode"] != null) code = Session["BranchSearchCode"].ToString();
                BillDetailSummaryInput model = new BillDetailSummaryInput();
                //string Branch = "Select Code From ifsc.branchmst";
                //model.TrDate = DateTime.Parse(Session["FinYearFrom"].ToString());
                //model.ToDate = DateTime.Parse(Session["FinYearTo"].ToString());

                model.TrDate = DateTime.Today;
                model.ToDate = DateTime.Today;
                return View(model);


            }
        }

        public static BillDetailSummaryOutput lstBillOut;
        public ActionResult BillSummaryDetail(BillDetailSummaryInput model)
        {

            if (Session["WebUser"] == null)
            {
                TempData["AlertMessage"] = "Session Time Out Please Login Again";
                return RedirectToAction("Index", "Login");

            }
            else
            {
                try
                {
                    string qry = null;
                    DataSet ds = null;
                    int c = 0;
                    WebUser webUser = Session["WebUser"] as WebUser;
                    if (webUser == null) return null;
                    
                    else 
                    {

                        string str = model.Exchange;
                        if (str.Equals("All"))
                        {
                            str = "NSE";
                        }
                        else
                        {
                            str = model.Exchange;
                        }
                        qry = "SELECT clientid PARTY_CD,P1.PAR_NAME,nvl(p1.branchind,'-') BRANCHIND,exchange,sum(nvl(nsetax,0)) trxchrg,sum(nvl(sttamt,0)) stt," +
                        "s.billno BillNo ,sum(s.BROK) brok,sum(s.grossamt) GrossAmount, sum(decode(sign(netamt),-1,abs(netamt),0)) Debit," +
                        "sum(decode(sign(netamt),1,abs(netamt),0)) Credit, sum(s.stamp) stamp,sum(nvl(cgst,0)+nvl(igst,0)+nvl(sgst,0)+nvl(utt,0)) gst, sum(s.tax1) tax1 ,sum(s.tax2) tax2,sum(s.tax3) tax3,sum(s.tax4) tax4,sum(TURNOVER) TURNOVER, a.*"+
                        "from (select nvl(tax1narr, 'tax1')ntax1,nvl(tax2narr,'Tax2')ntax2,nvl(tax3narr,'Tax3') ntax3,nvl(tax4narr,'Tax4') ntax4 "+
                        "from " + dbuser + ".CUPARA	where exchange = '"+str+"')A," + dbuser + ".CUPARTYMST P1, " + dbuser + ".CUBILL S WHERE  s.clientid=P1.PAR_CODE  and bdate>=to_date('" + model.TrDate.ToString("ddMMMyyyy") + "')  and bdate<=to_date('" + model.ToDate.ToString("ddMMMyyyy") + "')  ";
                        if (model.Exchange.ToUpper() == "ALL") { }
                        else { qry = qry+ " and exchange='" + model.Exchange + "'"; }
                        if (model.Branch.ToUpper() == "ALL") { }
                        else { qry = qry + " and p1.branchind='" + model.Branch + "' "; }
                        //if (model.BillSelection.ToUpper() == "ALL") { }
                        //else { qry = qry + " and substr(s.billno,1,6)='" + model.Branch + "' "; }
                        if (model.ClientCodeFrom !=null) { qry = qry + " and clientid='" + model.ClientCodeFrom.ToUpper() + "' "; }

                        if (model.BillSelection == "Future MTM") { qry = qry + "and billno like'FUTMTM%'"; }

                        else if (model.BillSelection == "Future Expiry") { qry = qry + "and billno like'FUTEXP%'"; }

                        else if (model.BillSelection == "Option Daily") { qry = qry +"and billno like 'OPTDLY%'"; }

                        else if (model.BillSelection == "Option Expiry") { qry = qry + "and billno like 'OPTEXP%'"; }

                        else if (model.BillSelection == "Daily MTM") { qry = qry + "and billno like 'DLYMTM%'"; }

                        else { qry = qry ; }


                        qry = qry + " GROUP BY clientid,exchange,par_name,nvl(p1.branchind,'-'),groupclient ,s.billno,ntax1,ntax2,ntax3,ntax4 ORDER BY exchange,clientid,PAR_NAME ";
                        ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet(qry, Session["SelectedConn"].ToString());
                        lstBillOut = new BillDetailSummaryOutput();
                        lstBillOut.listBillDetailSummaryOutputRow = new List<BillDetailSummaryOutputRow>();
                        lstBillOut.TrDate = model.TrDate;
                        lstBillOut.ToDate = model.ToDate;
                        c = 0;
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            if (c == 0)
                            {
                                lstBillOut.TrDate = model.TrDate;
                                lstBillOut.ToDate = model.ToDate;
                                lstBillOut.ClientCodeFrom = row["PARTY_CD"].ToString();
                                lstBillOut.ClientName = row["PAR_NAME"].ToString();
                            }
                            c++;
                            BillDetailSummaryOutputRow bdo = new BillDetailSummaryOutputRow();
                            bdo.Client = row["PARTY_CD"].ToString();
                            bdo.ClientName = row["PAR_NAME"].ToString();
                            bdo.Branch = row["BRANCHIND"].ToString();
                            bdo.Exchange = row["exchange"].ToString();
                            bdo.BillNo = row["BillNo"].ToString();

                            bdo.GrossAmount = decimal.Parse(row["GrossAmount"].ToString());
                            bdo.Brok = decimal.Parse(row["brok"].ToString());
                            /*bdo.ServiceTax = decimal.Parse(row["stax"].ToString());*/
                            bdo.NseTax = row["trxchrg"].ToString();
                            bdo.SttAmt = row["stt"].ToString();
                            bdo.StampDuty = row["stamp"].ToString();

                            bdo.GST = row["gst"].ToString();
                            bdo.Tax1 = row["tax1"].ToString();
                            //bdo.STaxon = row["stax_turn"].ToString();
                            //bdo.STaxTax1 = row["stax_tax1"].ToString();
                            bdo.Tax2 = row["tax2"].ToString();
                            bdo.Tax3 = row["tax3"].ToString();
                            bdo.Tax4 = row["tax4"].ToString();
                            //bdo.STaxTax2 = row["stax_tax2"].ToString();

                            //bdo.SBCTax = row["sbc_stax"].ToString();
                            //bdo.KKCTax = row["kkc_stax"].ToString();
                            bdo.Debit = decimal.Parse(row["Debit"].ToString());
                            bdo.Credit = decimal.Parse(row["Credit"].ToString());
                            bdo.TurnOver = decimal.Parse(row["TURNOVER"].ToString());
                            lstBillOut.listBillDetailSummaryOutputRow.Add(bdo);
                        }

                        return View(lstBillOut);
                    }

                    
                   
                }


                catch (Exception ex)
                {
                    TempData["AlertMessage"] = ex;
                    return RedirectToAction("BillSummary", "Report");
                }
            }


        }

        public ActionResult ContractNote()
        {
            BillDetailSummaryInput model = new BillDetailSummaryInput();
            model.TrDate = DateTime.Parse(Session["FinYearFrom"].ToString());
            model.ToDate = DateTime.Parse(Session["FinYearTo"].ToString());
            return View(model);
        }

        public ActionResult ContractNoteDetails()
        {
             return View();
        }

        public ActionResult CustodianContractNote()
        {
            BillDetailSummaryInput model = new BillDetailSummaryInput();
            model.TrDate = DateTime.Parse(Session["FinYearFrom"].ToString());
            return View(model);
        }

        public ActionResult ExposurePortfolioInput()
        {
            BillDetailSummaryInput model = new BillDetailSummaryInput();
            model.OnDate = DateTime.Parse(Session["FinYearFrom"].ToString());
            model.ClosePriceDate = DateTime.Parse(Session["FinYearFrom"].ToString());
            return View(model);
        }



        //Dummy Controller created by rajnish 

        public ActionResult ContractNote(ContractNote model)
        {
            return View(model);
        }

        public ActionResult ContractNoteCustodian(ContractNoteCustodian model)
        {
            return View(model);
        }

        public ActionResult GSTReport(GSTReport model)
        {
            return View(model);
        }

        public ActionResult DayBook(DayBook model)
        {
            return View(model);
        }

        public ActionResult IncomeTaxTurnOver(IncomeTaxTurnOver model)
        {


            return View(model);
        }

        public ActionResult UserIdWiseSummary(UserIdWiseSummary model)
        {
            return View(model);
        }

        public ActionResult PortfolioAndExposure(PortfolioAndExposure model)
        {
            return View(model);
        }

       

        public ActionResult DaySummary(DaySummary model)
        {
            return View(model);
        }


    }



    public class DematLedgerInput
    {
        public string ClientCodeFrom { get; set; }

        public string ScripCode { get; set; }

        public string ISINCode { get; set; }

        public string SettNo { get; set; }

        public string RefNo { get; set; }

        public DateTime DateFrom { get; set; }

        public DateTime DateTo { get; set; }

        public DataSet DSOut { get; set; }

    }
}