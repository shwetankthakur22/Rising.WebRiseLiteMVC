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

    public class BranchHomeController : Controller
    {
        // GET: BranchHome
        public ActionResult Index(DashBoardOutPut model)
        {
            model.DateFrom = DateTime.Parse(Session["FinYearFrom"].ToString());
            model.DateTo = DateTime.Parse(Session["FinYearTo"].ToString());
            model.DateClosing = DateTime.Parse(System.DateTime.Now.ToString("ddMMMyyyy"));
            model.OpenPosition = true;

            string nul = null;
            WebUser webUser = Session["WebUser"] as WebUser;
            if (webUser != null)
            {
                if (webUser.LoginValidationStatus == true)
                {
                    List<DBList> selectedDBLists = Session["SelectedDBLists"] as List<DBList>;

                    if (webUser.UserType == UserType.Branch || webUser.UserType == UserType.RM)
                    {
                        List<Oracle.ManagedDataAccess.Client.OracleParameter> lst = new List<Oracle.ManagedDataAccess.Client.OracleParameter>();
                        lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("CLIENTCODE_", webUser.UserID, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));

                        // branch profile

                        lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("USER_", (selectedDBLists.Count() == 0 ? "SYSADM" : selectedDBLists[0].OtherDBUser), Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));

                        DataSet ds = MvcApplication.OracleDBHelperCore().CustomHelper.CustomDataSet("SYSADM.N$GET_BRANCHPROFILE", lst, Session["SelectedConn"].ToString());
                        model.ds = ds;

                        if (webUser.UserType == UserType.RM)
                        {
                            string qryy = "SELECT RMCODE CODE, RMDESC NAME, '' ADDRESS, MOBILENO, EMAILNO EMAIL, RH_NAME MANAGER FROM SYSADM.RMMASTER WHERE RMCODE='" + webUser.UserID + "'";
                            DataSet dsy = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet(qryy, Session["SelectedConn"].ToString());

                            model.ds = dsy;
                        }



                        //TURNOVER
                        string qry = "SELECT TO_CHAR(TRN_DATE,'YYYY') YEAR, SUM(JOBB_TURN) AS JOBB_TURN, SUM(JOBB_BROK)AS JOBB_BROK, SUM(DELV_TURN) AS DELV_TURN,SUM(DELV_BROK) AS DELV_BROK, SUM(JOBB_TURN) + SUM(DELV_TURN)  TOTALTURN, SUM(JOBB_BROK) + SUM(DELV_BROK) TOTALBROK, TO_CHAR(TRN_DATE,'YYYY')  FROM(SELECT  TRN_DATE, 'All' EXCHANGE, p.branchind branch, p.subbranchind subbranch,T.TRN_CLIENTCD CODE, P.PAR_NAME NAME, DECODE(DECODE(NEWBROKTYPE, 'DL', 'D', DECODE(TRN_BROKTYPE, 'DL', 'D', 'J')), 'J', SUM(ABS(TRN_QTY) * TRN_MKTRATE), 0) JOBB_TURN, DECODE(DECODE(NEWBROKTYPE, 'DL', 'D', DECODE(TRN_BROKTYPE, 'DL', 'D', 'J')), 'J', SUM(trn_brok), 0) JOBB_BROK,DECODE(DECODE(NEWBROKTYPE, 'DL', 'D', DECODE(TRN_BROKTYPE, 'DL', 'D', 'J')), 'D', SUM(ABS(TRN_QTY) * TRN_MKTRATE), 0) DELV_TURN,DECODE(DECODE(NEWBROKTYPE, 'DL', 'D', DECODE(TRN_BROKTYPE, 'DL', 'D', 'J')), 'D', SUM(trn_brok), 0) DELV_BROK, B.NAME BRNAME  FROM SYSADM.trnmast T, SYSADM.PARTYMST P, SYSADM.BRANCHMST B WHERE TRN_DATE >= to_date('" + model.DateFrom.ToString("dd-MM-yyyy") + "', 'dd-mm-yyyy') AND TRN_DATE <= to_date('" + model.DateTo.ToString("dd-MM-yyyy") + "', 'dd-mm-yyyy') AND T.TRN_CLIENTCD = P.PAR_CODE AND P.BRANCHIND = B.CODE  and trn_clientcd not in (select accode from SYSADM.stnmast  where accode is not null) ";

                        if (webUser.UserType == UserType.Branch)
                        {
                            qry = qry + " AND BRANCHIND = '" + webUser.UserID + "' ";
                        }

                        if (webUser.UserType == UserType.RM)
                        {
                            qry = qry + " AND p.RMCODE = '" + webUser.UserID + "' ";
                        }

                        qry = qry+ " GROUP BY TRN_DATE, p.branchind, p.subbranchind, T.TRN_CLIENTCD, P.PAR_NAME, B.NAME, DECODE(NEWBROKTYPE, 'DL', 'D', DECODE(TRN_BROKTYPE, 'DL', 'D', 'J'))) GROUP BY  TO_CHAR(TRN_DATE, 'YYYY')   ORDER BY 1 DESC";
                        DataSet ds1 = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet(qry, Session["SelectedConn"].ToString());

                        model.dsOut1 = ds1;


                        qry = "SELECT TO_CHAR(TRN_DATE,'MON-YYYY') YEAR, SUM(JOBB_TURN) AS JOBB_TURN, SUM(JOBB_BROK)AS JOBB_BROK, SUM(DELV_TURN) AS DELV_TURN,SUM(DELV_BROK) AS DELV_BROK, SUM(JOBB_TURN) + SUM(DELV_TURN)  TOTALTURN, SUM(JOBB_BROK) + SUM(DELV_BROK) TOTALBROK, TO_CHAR(TRN_DATE,'YYYYMM') FROM(SELECT  TRN_DATE, 'All' EXCHANGE, p.branchind branch, p.subbranchind subbranch,T.TRN_CLIENTCD CODE, P.PAR_NAME NAME, DECODE(DECODE(NEWBROKTYPE, 'DL', 'D', DECODE(TRN_BROKTYPE, 'DL', 'D', 'J')), 'J', SUM(ABS(TRN_QTY) * TRN_MKTRATE), 0) JOBB_TURN, DECODE(DECODE(NEWBROKTYPE, 'DL', 'D', DECODE(TRN_BROKTYPE, 'DL', 'D', 'J')), 'J', SUM(trn_brok), 0) JOBB_BROK,DECODE(DECODE(NEWBROKTYPE, 'DL', 'D', DECODE(TRN_BROKTYPE, 'DL', 'D', 'J')), 'D', SUM(ABS(TRN_QTY) * TRN_MKTRATE), 0) DELV_TURN,DECODE(DECODE(NEWBROKTYPE, 'DL', 'D', DECODE(TRN_BROKTYPE, 'DL', 'D', 'J')), 'D', SUM(trn_brok), 0) DELV_BROK, B.NAME BRNAME  FROM SYSADM.trnmast T, SYSADM.PARTYMST P, SYSADM.BRANCHMST B WHERE TRN_DATE >= to_date('" + model.DateFrom.ToString("dd-MM-yyyy") + "', 'dd-mm-yyyy') AND TRN_DATE <= to_date('" + model.DateTo.ToString("dd-MM-yyyy") + "', 'dd-mm-yyyy') AND T.TRN_CLIENTCD = P.PAR_CODE AND P.BRANCHIND = B.CODE  and trn_clientcd not in (select accode from SYSADM.stnmast  where accode is not null)  ";

                        if (webUser.UserType == UserType.Branch)
                        {
                            qry = qry + " AND BRANCHIND = '" + webUser.UserID + "' ";
                        }

                        if (webUser.UserType == UserType.RM)
                        {
                            qry = qry + " AND p.RMCODE = '" + webUser.UserID + "' ";
                        }

                        qry = qry + "  GROUP BY TRN_DATE, p.branchind, p.subbranchind, T.TRN_CLIENTCD, P.PAR_NAME, B.NAME, DECODE(NEWBROKTYPE, 'DL', 'D', DECODE(TRN_BROKTYPE, 'DL', 'D', 'J'))) GROUP BY  TO_CHAR(TRN_DATE,'MON-YYYY'), TO_CHAR(TRN_DATE,'YYYYMM') ORDER BY 8 DESC";
                        DataSet ds2 = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet(qry, Session["SelectedConn"].ToString());

                        model.dsOut2 = ds2;


                        qry = "SELECT TO_CHAR(TRN_DATE,'DD-MON-YYYY') YEAR, SUM(JOBB_TURN) AS JOBB_TURN,SUM(JOBB_BROK) AS JOBB_BROK,SUM(DELV_TURN) AS DELV_TURN, SUM(DELV_BROK) AS DELV_BROK, SUM(JOBB_TURN) + SUM(DELV_TURN)  TOTALTURN, SUM(JOBB_BROK) + SUM(DELV_BROK) TOTALBROK, TO_CHAR(TRN_DATE, 'YYYYMMDD') FROM(SELECT  TRN_DATE, 'All' EXCHANGE, p.branchind branch, p.subbranchind subbranch,T.TRN_CLIENTCD CODE, P.PAR_NAME NAME, DECODE(DECODE(NEWBROKTYPE, 'DL', 'D', DECODE(TRN_BROKTYPE, 'DL', 'D', 'J')), 'J', SUM(ABS(TRN_QTY) * TRN_MKTRATE), 0) JOBB_TURN, DECODE(DECODE(NEWBROKTYPE, 'DL', 'D', DECODE(TRN_BROKTYPE, 'DL', 'D', 'J')), 'J', SUM(trn_brok), 0) JOBB_BROK,DECODE(DECODE(NEWBROKTYPE, 'DL', 'D', DECODE(TRN_BROKTYPE, 'DL', 'D', 'J')), 'D', SUM(ABS(TRN_QTY) * TRN_MKTRATE), 0) DELV_TURN,DECODE(DECODE(NEWBROKTYPE, 'DL', 'D', DECODE(TRN_BROKTYPE, 'DL', 'D', 'J')), 'D', SUM(trn_brok), 0) DELV_BROK, B.NAME BRNAME  FROM SYSADM.trnmast T, SYSADM.PARTYMST P, SYSADM.BRANCHMST B WHERE TRN_DATE >= to_date('" + model.DateFrom.ToString("dd-MM-yyyy") + "', 'dd-mm-yyyy') AND TRN_DATE <= to_date('" + model.DateTo.ToString("dd-MM-yyyy") + "', 'dd-mm-yyyy') AND T.TRN_CLIENTCD = P.PAR_CODE AND P.BRANCHIND = B.CODE  and trn_clientcd not in (select accode from SYSADM.stnmast  where accode is not null)  ";

                        if (webUser.UserType == UserType.Branch)
                        {
                            qry = qry + " AND BRANCHIND = '" + webUser.UserID + "' ";
                        }

                        if (webUser.UserType == UserType.RM)
                        {
                            qry = qry + " AND p.RMCODE = '" + webUser.UserID + "' ";
                        }

                        qry = qry + "   GROUP BY TRN_DATE, p.branchind, p.subbranchind, T.TRN_CLIENTCD, P.PAR_NAME, B.NAME, DECODE(NEWBROKTYPE, 'DL', 'D', DECODE(TRN_BROKTYPE, 'DL', 'D', 'J')))   GROUP BY  TO_CHAR(TRN_DATE,'DD-MON-YYYY'), TO_CHAR(TRN_DATE,'YYYYMMDD') ORDER BY 8 DESC";
                        DataSet ds3 = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet(qry, Session["SelectedConn"].ToString());

                        model.dsOut3 = ds3;

                        qry = "SELECT PAR_CODE CODE, PAR_NAME NAME , SUM(JOBB_TURN) AS JOBB_TURN,SUM(JOBB_BROK) AS JOBB_BROK,SUM(DELV_TURN) AS DELV_TURN, SUM(DELV_BROK) AS DELV_BROK, SUM(JOBB_TURN) + SUM(DELV_TURN)  TOTALTURN, SUM(JOBB_BROK) + SUM(DELV_BROK) TOTALBROK FROM(SELECT  P.PAR_CODE, P.PAR_NAME, 'All' EXCHANGE, p.branchind branch, p.subbranchind subbranch, T.TRN_CLIENTCD CODE, P.PAR_NAME NAME,DECODE(DECODE(NEWBROKTYPE, 'DL', 'D', DECODE(TRN_BROKTYPE, 'DL', 'D', 'J')), 'J', SUM(ABS(TRN_QTY) * TRN_MKTRATE), 0) JOBB_TURN,DECODE(DECODE(NEWBROKTYPE, 'DL', 'D', DECODE(TRN_BROKTYPE, 'DL', 'D', 'J')), 'J', SUM(trn_brok), 0) JOBB_BROK,DECODE(DECODE(NEWBROKTYPE, 'DL', 'D', DECODE(TRN_BROKTYPE, 'DL', 'D', 'J')), 'D', SUM(ABS(TRN_QTY) * TRN_MKTRATE), 0) DELV_TURN,DECODE(DECODE(NEWBROKTYPE, 'DL', 'D', DECODE(TRN_BROKTYPE, 'DL', 'D', 'J')), 'D', SUM(trn_brok), 0) DELV_BROK, B.NAME BRNAME  FROM SYSADM.trnmast T, SYSADM.PARTYMST P, SYSADM.BRANCHMST B  WHERE TRN_DATE >= to_date('" + model.DateFrom.ToString("dd-MM-yyyy") + "', 'dd-mm-yyyy') AND TRN_DATE <= to_date('" + model.DateTo.ToString("dd-MM-yyyy") + "', 'dd-mm-yyyy') AND T.TRN_CLIENTCD = P.PAR_CODE AND P.BRANCHIND = B.CODE  and trn_clientcd not in (select accode from SYSADM.stnmast  where accode is not null)  ";

                        if (webUser.UserType == UserType.Branch)
                        {
                            qry = qry + " AND BRANCHIND = '" + webUser.UserID + "' ";
                        }

                        if (webUser.UserType == UserType.RM)
                        {
                            qry = qry + " AND p.RMCODE = '" + webUser.UserID + "' ";
                        }

                        qry = qry + "  GROUP BY P.PAR_CODE, P.PAR_NAME , p.branchind, p.subbranchind, T.TRN_CLIENTCD, B.NAME, DECODE(NEWBROKTYPE, 'DL', 'D', DECODE(TRN_BROKTYPE, 'DL', 'D', 'J')))   GROUP BY PAR_CODE, PAR_NAME ORDER BY 1 DESC";
                        DataSet ds4 = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet(qry, Session["SelectedConn"].ToString());

                        model.dsOut4 = ds4;

                        //TOP CLIENT
                        qry = "select * from (SELECT p.branchind, T.TRN_CLIENTCD CODE, P.PAR_NAME NAME,SUM(ABS(TRN_QTY) * TRN_MKTRATE) turn   FROM SYSADM.trnmast T, SYSADM.PARTYMST P, SYSADM.BRANCHMST B   WHERE TRN_DATE >= to_date('" + model.DateFrom.ToString("dd-MM-yyyy") + "', 'dd-mm-yyyy') AND TRN_DATE <= to_date('" + model.DateTo.ToString("dd-MM-yyyy") + "', 'dd-mm-yyyy') AND T.TRN_CLIENTCD = P.PAR_CODE AND P.BRANCHIND = B.CODE  and   trn_clientcd not in (select accode from SYSADM.stnmast  where accode is not null)  ";

                        if (webUser.UserType == UserType.Branch)
                        {
                            qry = qry + " AND BRANCHIND = '" + webUser.UserID + "' ";
                        }

                        if (webUser.UserType == UserType.RM)
                        {
                            qry = qry + " AND p.RMCODE = '" + webUser.UserID + "' ";
                        }

                        qry = qry + "    GROUP BY p.branchind ,T.TRN_CLIENTCD, P.PAR_NAME order by turn desc   ) where rownum< 11";
                        DataSet ds5 = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet(qry, Session["SelectedConn"].ToString());

                        model.dsOut5 = ds5;

                        qry = "select* from (SELECT  p.branchind, s.sh_code CODE, s.sh_name NAME, SUM(ABS(TRN_QTY) * TRN_MKTRATE) turn   FROM SYSADM.trnmast T, SYSADM.PARTYMST P, SYSADM.BRANCHMST B, sysadm.sharemst s   WHERE TRN_DATE >=  to_date('" + model.DateFrom.ToString("dd-MM-yyyy") + "', 'dd-mm-yyyy') AND TRN_DATE <= to_date('" + model.DateTo.ToString("dd-MM-yyyy") + "', 'dd-mm-yyyy')  AND T.TRN_CLIENTCD = P.PAR_CODE AND P.BRANCHIND = B.CODE  and  t.trn_scrip = s.sh_code and   trn_clientcd not in (select accode from SYSADM.stnmast  where accode is not null)  ";

                        if (webUser.UserType == UserType.Branch)
                        {
                            qry = qry + " AND BRANCHIND = '" + webUser.UserID + "' ";
                        }

                        if (webUser.UserType == UserType.RM)
                        {
                            qry = qry + " AND p.RMCODE = '" + webUser.UserID + "' ";
                        }

                        qry = qry + "    GROUP BY p.branchind ,s.sh_name, s.sh_code order by turn desc   )  where rownum< 11";
                        DataSet ds6 = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet(qry, Session["SelectedConn"].ToString());

                        model.dsOut6 = ds6;


                    }


                }
            }
            return View(model);
        }



        public ActionResult ClientGlobalReport(DashBoardOutPut model, FormCollection form)
        {
            model.DateFrom = DateTime.Parse(Session["FinYearFrom"].ToString());
            model.DateTo = DateTime.Parse(Session["FinYearTo"].ToString());
            model.DateClosing = DateTime.Parse(System.DateTime.Now.ToString("ddMMMyyyy"));
            model.AsonDate = DateTime.Parse(Session["FinYearTo"].ToString());
            model.OpenPosition = true;

            string code = form["ClientCodeFrom"];
            string nul = null;
            WebUser webUser = Session["WebUser"] as WebUser;
            if (webUser != null)
            {
                if (webUser.UserType != UserType.Branch && webUser.UserType != UserType.RM) return RedirectToAction("Index", "ClientHome");
                if (webUser.LoginValidationStatus == true)
                {
                    List<DBList> selectedDBLists = Session["SelectedDBLists"] as List<DBList>;

                    if (webUser.UserType == UserType.Branch)
                    {
                        DataSet ds20 = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet("SELECT * FROM sysadm.partymst where par_code='" + code + "' and branchind='" + webUser.UserID + "'", Session["SelectedConn"].ToString());
                        if (ds20.Tables[0].Rows.Count == 0)
                        {
                            TempData["AlertMessage"] = "Invalid Code : " + model.ClientCodeFrom;
                            return RedirectToAction("Index", "BranchHome");
                        }
                    }
                    else if (webUser.UserType == UserType.RM)
                    {
                        DataSet ds20 = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet("SELECT * FROM sysadm.partymst where par_code='" + code + "' and rmcode='" + webUser.UserID + "'", Session["SelectedConn"].ToString());
                        if (ds20.Tables[0].Rows.Count == 0)
                        {
                            TempData["AlertMessage"] = "Invalid Code : " + model.ClientCodeFrom;
                            return RedirectToAction("Index", "BranchHome");
                        }
                    }
                    if (1 == 1)
                    {
                        // client profile
                        List<Oracle.ManagedDataAccess.Client.OracleParameter> lst = new List<Oracle.ManagedDataAccess.Client.OracleParameter>();
                        lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("CLIENTCODE_", code, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
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
                            lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("CLIENTCODEFROM", code, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                            lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("CLIENTCODETO", code, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
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
                            lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("CLIENTCODEFROM", code, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                            lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("CLIENTCODETO", code, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
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
                        if (selectedDBLists[0].Group == "Cash")
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
                        if (System.Diagnostics.Debugger.IsAttached == true)
                        {
                            //...
                        }
                        else
                        {
                            lst = new List<Oracle.ManagedDataAccess.Client.OracleParameter>();
                            lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("clientcode_", code, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
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
                            lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("clientcode_", code, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
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
                            lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("clientcode_", code, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
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
                            lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("clientcode_", code, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
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
                        model.dsFinancialSummary = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet("select NSEBAL+ BSEBAL NSEBAL, MCX_FIN+NCDEX_FIN COMMBAL,  FO_MGN_POSTED + CUR_MGN_POSTED NSEMARGIN, COM_MGN_POSTED COMMMARGIN, BEN_STOCK, CDSL_VAR, CLRBAL TOTBALANCE, NSEBAL + BSEBAL + MCX_FIN + NCDEX_FIN WTMARGINBALANCE, FO_MGN_POSTED + CUR_MGN_POSTED + COM_MGN_POSTED TOTALMARGIN from sysadm.cappcr WHERE code ='" + code + "'", Session["SelectedConn"].ToString());


                        //DEMAT LEDGER
                        //model.listDematHoldingOutputRow = new List<DematHoldingOutputRow>();
                        //if (System.Diagnostics.Debugger.IsAttached == true)
                        //{
                        //    //...
                        //}
                        //else
                        //{
                        //    lst = new List<Oracle.ManagedDataAccess.Client.OracleParameter>();
                        //    lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("clientcode_", code, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                        //    lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("AsOnDate_", model.DateClosing, Oracle.ManagedDataAccess.Client.OracleDbType.Date, ParameterDirection.Input));
                        //    DataSet ds4 = MvcApplication.OracleDBHelperCore().CustomHelper.CustomDataSet("SYSADM.N$GET_DematHolding", lst, Session["SelectedConn"].ToString());

                        //    model.listDematHoldingOutputRow = new List<DematHoldingOutputRow>();

                        //    int c = 0;
                        //    foreach (DataRow row in ds4.Tables[0].Rows)
                        //    {
                        //        c++;
                        //        DematHoldingOutputRow bdo = new DematHoldingOutputRow();
                        //        bdo.ScripCode = row["SCRIPCD"].ToString();
                        //        bdo.ScripName = row["SH_NAME"].ToString();
                        //        bdo.ScripIsin = row["ISINCODE"].ToString();
                        //        bdo.Qty = decimal.Parse(row["Qty"].ToString());
                        //        bdo.Stock = decimal.Parse(row["Stock"].ToString());
                        //        bdo.LockQty = decimal.Parse(row["Qty_lock"].ToString());
                        //        bdo.CDSLQty = decimal.Parse(row["cdsl_qty"].ToString());
                        //        bdo.NSDLQty = decimal.Parse(row["nsdl_qty"].ToString());
                        //        bdo.TotalQty = decimal.Parse(row["tot_qty"].ToString());
                        //        bdo.Rate = decimal.Parse(row["rate"].ToString());
                        //        bdo.Value = decimal.Parse(row["stk_val"].ToString());
                        //        bdo.VarPer = decimal.Parse(row["rate_var"].ToString());
                        //        bdo.VarValue = decimal.Parse(row["varvalue"].ToString());
                        //        model.listDematHoldingOutputRow.Add(bdo);
                        //    }
                        //}
                    }

                    return View(model);
                }
                else return RedirectToAction("Index", "Login");
            }
            else { return RedirectToAction("Index", "Login"); }
        }



        public ActionResult gotoReport(string reportType, string id)
        {
            forceSegment(id);

            if (reportType == "NetPosition")
            {
                if (id == "1") return RedirectToAction("DateRange", "NetPosition");
                else return RedirectToAction("AsOnDate", "NetPosition");
            }

            if (reportType == "FinancialLedger")
            {
                return RedirectToAction("Index", "FinancialLedger");
            }

            if (reportType == "TradeConf")
            {
                return RedirectToAction("Index", "TradeConf");
            }


            return View();
        }



        public ActionResult TrialBalanceInput()
        {
            FinancialLedgerInput model = new FinancialLedgerInput();
            model.DateFrom = DateTime.Parse(Session["FinYearFrom"].ToString());
            model.DateTo = DateTime.Parse(Session["FinYearTo"].ToString());
            return View(model);
        }



        public ActionResult TrialBalance(FinancialLedgerInput model)
        {
            string nul = null;
            WebUser webUser = Session["WebUser"] as WebUser;
            if (webUser != null)
            {
                if (webUser.LoginValidationStatus == true)
                {
                    List<DBList> selectedDBLists = Session["SelectedDBLists"] as List<DBList>;

                    if (webUser.UserType == UserType.Branch || webUser.UserType == UserType.RM)
                    {
                        if (model.DateFrom > DateTime.Parse("01jan1900"))
                        {
                            string qry = "select party_cd ,substr(par_name,1,20) as par_name,branchind,NVL(groupclient,'NotDefine') groupclient,NVL(groupnm,'NotDefine') groupnm, sum(debitop) debitop,sum(creditop) creditop,sum(debit) debit,sum(credit) credit, (case when  (sum(creditop) +sum(credit) -sum(debitop) -sum(debit))<0 then sum(debitop) +sum(debit)-sum(creditop) -sum(credit) else 0 end) netdebit,  (case when  (sum(creditop) +sum(credit) -sum(debitop) -sum(debit))>0 then sum(creditop) +sum(credit)-sum(debitop) -sum(debit) else 0 end) netcredit    from (   Select party_cd,substr(par_name,1,20) par_name,branchind,NVL(groupclient,'NotDefine') groupclient,g.groupnm,0 CreditOP,0 DebitOP,  decode(sign(sum(NVL(a.credit,0)-NVL(a.debit,0))),1,sum(NVL(a.credit,0)-NVL(a.debit,0)),'0') as Credit,  decode(sign(sum(NVL(a.credit,0)-NVL(a.debit,0))),-1,sum(NVL(a.debit,0)-NVL(a.credit,0)),'0') as Debit  from  sysadm.partytrn a ,sysadm.partymst b,sysadm.groupclntmst g  where b.groupclient=g.groupcode(+) and par_code=party_cd AND PAR_NAME NOT LIKE 'DM-%' and wdate>='" + model.DateFrom.ToString("ddMMMyyyy") + "' and wdate<='" + model.DateTo.ToString("ddMMMyyyy") + "'  and b.groupcode='Sundry Debtors/Creditors' and b.grouplevel1='Sundry Debtors/Creditors'     ";

                            if (webUser.UserType == UserType.Branch) qry = qry + " and b.branchind in (select MAP_CODE from SYSADM.LINKBR where REGIONAL_USER='" + webUser.UserID + "' AND REGIONAL_TYPE='B' AND MAP_TYPE='B'  union select '" + webUser.UserID + "' from dual )        ";
                            else if (webUser.UserType == UserType.RM) qry = qry + " and b.RMCODE = BRANCHCODE_ ";

                           
                            qry = qry + " group by party_cd,par_name, branchind,groupclient,groupnm    having sum(NVL(a.credit,0)-NVL(a.debit,0))<>0    union all    Select party_cd,substr(par_name,1,20) par_name,branchind,NVL(groupclient,'NotDefine') groupclient,g.groupnm,  decode(sign(sum(NVL(a.credit,0)-NVL(a.debit,0))),1,sum(NVL(a.credit,0)-NVL(a.debit,0)),'0') as CreditOP,  decode(sign(sum(NVL(a.credit,0)-NVL(a.debit,0))),-1,sum(NVL(a.debit,0)-NVL(a.credit,0)),'0') as DebitOP ,0,0  from  sysadm.partytrn a ,sysadm.partymst b,sysadm.groupclntmst g  where b.groupclient=g.groupcode(+) and par_code=party_cd AND PAR_NAME NOT LIKE 'DM-%' and wdate<'" + model.DateFrom.ToString("ddMMMyyyy") + "'  and b.groupcode='Sundry Debtors/Creditors' and b.grouplevel1='Sundry Debtors/Creditors'      ";
                            if (webUser.UserType == UserType.Branch) qry = qry + " and  b.branchind in (select MAP_CODE from SYSADM.LINKBR where REGIONAL_USER='" + webUser.UserID + "' AND REGIONAL_TYPE='B' AND MAP_TYPE='B'     union select '" + webUser.UserID + "' from dual )       ";
                            else if (webUser.UserType == UserType.RM) qry = qry + " and b.RMCODE = BRANCHCODE_ ";
                                qry = qry + " group by party_cd,par_name, branchind,groupclient,groupnm    having sum(NVL(a.credit,0)-NVL(a.debit,0))<>0      ) group by party_cd,par_name, branchind,groupclient,groupnm      ORDER BY branchind,groupclient,party_cd,par_name";
                            DataSet ds1 = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet(qry, Session["SelectedConn"].ToString());

                            TrialBalanceInOut newModel = new TrialBalanceInOut();
                            newModel.DateFrom = model.DateFrom;
                            newModel.DateTo = model.DateTo;

                            newModel.dsOut = ds1;

                            Session["ReportHeader1"] = "Trial Balance Report";
                            Session["ReportHeader2"] = "Date Range : " + model.DateFrom.ToString("dd/MM/yyyy") + " - " + model.DateTo.ToString("dd/MM/yyyy");
                            return View(newModel);

                        }

                    }
                }
            }
            return View();
        }



        public void forceSegment(string id)
        {
            string str2 = id.Replace("---", "&");

            List<DBList> tempDBLists = new List<DBList>();
            List<DBList> selectedDBLists = Session["SelectedDBLists"] as List<DBList>;
            foreach (DBList db in selectedDBLists) { tempDBLists.Add(db); }
            selectedDBLists = new List<DBList>();

            var distinct = tempDBLists[0].Group;

            foreach (DBList dbl9 in MvcApplication.DBLists)
            {
                if (dbl9.CompGroup == str2)
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
        }



        public ActionResult ClientProfileList(GenericModel model)
        {
            WebUser webUser = Session["WebUser"] as WebUser;
            model.DateFrom = DateTime.Parse(Session["FinYearFrom"].ToString());
            model.DateTo = DateTime.Parse(Session["FinYearTo"].ToString());
            if (webUser != null)
            {
                if (webUser.LoginValidationStatus == true)
                {
                    List<DBList> selectedDBLists = Session["SelectedDBLists"] as List<DBList>;

                    if (webUser.UserType == UserType.Branch)
                    {
                        string qry = "SELECT CODE, P. PAR_NAME, BRANCH, SUBBR, GRP, MOBILE, EMAILNO, to_char(INTRODATE,'dd-mm-yyyy')INTRO_DATE,  LASTWORKINGDATE, DRDAYS, c.POA, ACSTATUS    FROM SYSADM.CAPPCR C, SYSADM.PARTYMST P WHERE P.PAR_CODE=C.CODE AND BRANCH = '" + webUser.UserID + "'";
                        if (model.ClientCode != null && model.ClientCode != "") qry = qry + " AND (CODE LIKE '%" + model.ClientCode.ToUpper() + "%' or P. PAR_NAME LIKE '%" + model.ClientCode.ToUpper() + "%') ";
                        DataSet ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet(qry, Session["SelectedConn"].ToString());

                        model.dsOut = ds;
                        Session["ReportHeader1"] = "Client Report";
                        Session["ReportHeader2"] = "Date Range : " + model.DateFrom.ToString("dd/MM/yyyy") + " - " + model.DateTo.ToString("dd/MM/yyyy");
                        return View(model);

                    }
                    else if (webUser.UserType == UserType.RM)
                    {
                        string qry = "SELECT CODE, P. PAR_NAME, BRANCH, SUBBR, GRP, MOBILE, EMAILNO, to_char(INTRODATE,'dd-mm-yyyy')INTRO_DATE,  LASTWORKINGDATE, DRDAYS, c.POA, ACSTATUS    FROM SYSADM.CAPPCR C, SYSADM.PARTYMST P WHERE P.PAR_CODE=C.CODE AND rmcode = '" + webUser.UserID + "'";
                        if (model.ClientCode != null && model.ClientCode != "") qry = qry + " AND (CODE LIKE '%" + model.ClientCode.ToUpper() + "%' or P. PAR_NAME LIKE '%" + model.ClientCode.ToUpper() + "%') ";
                        DataSet ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet(qry, Session["SelectedConn"].ToString());

                        model.dsOut = ds;
                        Session["ReportHeader1"] = "Client Report";
                        Session["ReportHeader2"] = "Date Range : " + model.DateFrom.ToString("dd/MM/yyyy") + " - " + model.DateTo.ToString("dd/MM/yyyy");
                        return View(model);

                    }
                }
            }
            return RedirectToAction("Index", "TimeOut");
        }


        public ActionResult ClientDRCRInput()
        {
            GenericModel model = new GenericModel();
            model.DateFrom = DateTime.Parse(Session["FinYearFrom"].ToString());
            model.DateTo = DateTime.Parse(Session["FinYearTo"].ToString());
            return View(model);
        }


        public ActionResult ClientDRCRList(GenericModel model)
        {
            WebUser webUser = Session["WebUser"] as WebUser;
            if (webUser != null)
            {
                if (webUser.LoginValidationStatus == true)
                {
                    List<DBList> selectedDBLists = Session["SelectedDBLists"] as List<DBList>;

                    if (webUser.UserType == UserType.Branch)
                    {
                        string qry = "select party_cd ,substr(par_name,1,20) as par_name,branchind,NVL(groupclient,'NotDefine') groupclient,NVL(groupnm,'NotDefine') groupnm,sum(credit) credit,sum(debit) debit,sum(mtfcredit) mtfcredit,sum(mtfdebit) mtfdebit,sum((mtfcredit+credit)-(mtfdebit+debit)) NETAMT from (         Select party_cd, substr(par_name,1, 20) par_name,branchind,NVL(groupclient, 'NotDefine') groupclient,g.groupnm,decode(sign(sum(NVL(a.credit, 0) - NVL(a.debit, 0))), 1, sum(NVL(a.credit, 0) - NVL(a.debit, 0)), '0') as Credit,        decode(sign(sum(NVL(a.credit, 0) - NVL(a.debit, 0))), -1, sum(NVL(a.debit, 0) - NVL(a.credit, 0)), '0') as Debit,0 MTFCREDIT,0 MTFDEBIT from         sysadm.partytrn a, sysadm.partymst b, sysadm.groupclntmst g where b.branchind = '" + webUser.UserID + "' and b.groupclient = g.groupcode(+) and par_code = party_cd and          wdate<= to_date('" + model.DateTo.ToString("dd-MM-yyyy") + "', 'dd-mm-yyyy') AND NVL(TRN_TYPE,'D')<> 'M' AND(BILLNO != 'F&OMarg' OR BILLNO IS NULL)  and b.groupcode = 'Sundry Debtors/Creditors' group by party_cd,par_name, branchind,groupclient,groupnm having sum(NVL(a.credit, 0) - NVL(a.debit, 0)) <> 0   UNION ALL Select PAR_CODE AS party_cd, substr(par_name,1, 20) par_name,branchind,NVL(groupclient, 'NotDefine') groupclient,g.groupnm,0 CREDIT,0 DEBIT,decode(sign(sum(NVL(a.credit, 0) - NVL(a.debit, 0))), 1, sum(NVL(a.credit, 0) - NVL(a.debit, 0)), '0') as MTFCredit,        decode(sign(sum(NVL(a.credit, 0) - NVL(a.debit, 0))), -1, sum(NVL(a.debit, 0) - NVL(a.credit, 0)), '0') as MTFDebit from                   sysadm.partytrn a ,sysadm.partymst b, sysadm.groupclntmst g where b.branchind = '" + webUser.UserID + "' and b.groupclient = g.groupcode(+) and mgncode = party_cd and               wdate<= to_date('" + model.DateTo.ToString("dd-MM-yyyy") + "', 'dd-mm-yyyy') and b.groupcode = 'Sundry Debtors/Creditors' group by PAR_CODE,par_name, branchind,groupclient,groupnm   having sum(NVL(a.credit, 0) - NVL(a.debit, 0)) <> 0  ) group by party_cd,par_name, branchind,groupclient,groupnm       ORDER BY branchind, groupclient, party_cd, par_name";
                        DataSet ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet(qry, Session["SelectedConn"].ToString());
                        model.dsOut = ds;
                        Session["ReportHeader1"] = "Client DR/CR List";
                        Session["ReportHeader2"] = "As on Date :  " + model.DateTo.ToString("dd/MM/yyyy");
                        return View(model);

                    }

                    if (webUser.UserType == UserType.RM)
                    {
                        string qry = "select party_cd ,substr(par_name,1,20) as par_name,branchind,NVL(groupclient,'NotDefine') groupclient,NVL(groupnm,'NotDefine') groupnm,sum(credit) credit,sum(debit) debit,sum(mtfcredit) mtfcredit,sum(mtfdebit) mtfdebit,sum((mtfcredit+credit)-(mtfdebit+debit)) NETAMT from (         Select party_cd, substr(par_name,1, 20) par_name,branchind,NVL(groupclient, 'NotDefine') groupclient,g.groupnm,decode(sign(sum(NVL(a.credit, 0) - NVL(a.debit, 0))), 1, sum(NVL(a.credit, 0) - NVL(a.debit, 0)), '0') as Credit,        decode(sign(sum(NVL(a.credit, 0) - NVL(a.debit, 0))), -1, sum(NVL(a.debit, 0) - NVL(a.credit, 0)), '0') as Debit,0 MTFCREDIT,0 MTFDEBIT from         sysadm.partytrn a, sysadm.partymst b, sysadm.groupclntmst g where b.rmcode = '" + webUser.UserID + "' and b.groupclient = g.groupcode(+) and par_code = party_cd and          wdate<= to_date('" + model.DateTo.ToString("dd-MM-yyyy") + "', 'dd-mm-yyyy') AND NVL(TRN_TYPE,'D')<> 'M' AND(BILLNO != 'F&OMarg' OR BILLNO IS NULL)  and b.groupcode = 'Sundry Debtors/Creditors' group by party_cd,par_name, branchind,groupclient,groupnm having sum(NVL(a.credit, 0) - NVL(a.debit, 0)) <> 0   UNION ALL Select PAR_CODE AS party_cd, substr(par_name,1, 20) par_name,branchind,NVL(groupclient, 'NotDefine') groupclient,g.groupnm,0 CREDIT,0 DEBIT,decode(sign(sum(NVL(a.credit, 0) - NVL(a.debit, 0))), 1, sum(NVL(a.credit, 0) - NVL(a.debit, 0)), '0') as MTFCredit,        decode(sign(sum(NVL(a.credit, 0) - NVL(a.debit, 0))), -1, sum(NVL(a.debit, 0) - NVL(a.credit, 0)), '0') as MTFDebit from                   sysadm.partytrn a ,sysadm.partymst b, sysadm.groupclntmst g where b.branchind = '" + webUser.UserID + "' and b.groupclient = g.groupcode(+) and mgncode = party_cd and               wdate<= to_date('" + model.DateTo.ToString("dd-MM-yyyy") + "', 'dd-mm-yyyy') and b.groupcode = 'Sundry Debtors/Creditors' group by PAR_CODE,par_name, branchind,groupclient,groupnm   having sum(NVL(a.credit, 0) - NVL(a.debit, 0)) <> 0  ) group by party_cd,par_name, branchind,groupclient,groupnm       ORDER BY branchind, groupclient, party_cd, par_name";
                        DataSet ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet(qry, Session["SelectedConn"].ToString());
                        model.dsOut = ds;
                        Session["ReportHeader1"] = "Client DR/CR List";
                        Session["ReportHeader2"] = "As on Date :  " + model.DateTo.ToString("dd/MM/yyyy");
                        return View(model);

                    }
                }
            }
            return RedirectToAction("Index", "TimeOut");
        }


        public ActionResult SttReportInput(string code)
        {
            //input datefrom, dateto, clientcode, type(M,E)
            GenericModel model = new GenericModel();
            model.DateFrom = DateTime.Parse(Session["FinYearFrom"].ToString());
            model.DateTo = DateTime.Parse(Session["FinYearTo"].ToString());
            model.ClientCode = code;
            return View(model);
        }


        public ActionResult SttReportList(GenericModel model)
        {
            WebUser webUser = Session["WebUser"] as WebUser;
            if (webUser != null)
            {
                if (webUser.LoginValidationStatus == true)
                {
                    List<DBList> selectedDBLists = Session["SelectedDBLists"] as List<DBList>;

                    if (webUser.UserType == UserType.Branch)
                    {
                        string qry = "SELECT SETTNO,to_char(STTDATE,'dd-mm-yyyy') STTDATE,CLIENTCODE,P1.PAR_NAME as PAR_NAME, nvl(SYMBOL, '-') SYMBOL,NVL(SERIES, '-') AS SERIES, TOTALSTT FROM SYSADM.STTFILECAP S, SYSADM.PARTYMST P1 WHERE RECORDTYPE = '30'  AND TYPE='" + model.Type + "'    AND S.CLIENTCODE = P1.PAR_CODE AND STTDATE>= TO_DATE('" + model.DateFrom.ToString("dd-MM-yyyy") + "', 'DD-MM-YYYY')                    AND STTDATE<= TO_DATE('" + model.DateTo.ToString("dd-MM-yyyy") + "', 'DD-MM-YYYY') AND P1.BRANCHIND = '" + webUser.UserID + "' ";
                        if (model.ClientCode != null) { if (model.ClientCode != "") qry = qry + " AND CLIENTCODE='" + model.ClientCode + "' "; }

                        qry = qry + " union all         SELECT SETTNO, to_char(STTDATE,'dd-mm-yyyy') STTDATE,CLIENTCODE,P1.PAR_NAME, NVL(S1.SH_SYMBOL, '-') AS SYMBOL, NVL(S1.SH_SERIES, '-') AS SERIES, TOTALSTT FROM SYSADM.STTFILEBSE S, SYSADM.PARTYMST P1, SYSADM.SHAREMST S1   WHERE S.SCRIPCODE = S1.SH_CODE AND RECORDTYPE = '30' AND TYPE='" + model.Type + "'     AND S.CLIENTCODE = P1.PAR_CODE AND STTDATE>= TO_DATE('" + model.DateFrom.ToString("dd-MM-yyyy") + "', 'DD-MM-YYYY')   AND STTDATE<= TO_DATE('" + model.DateTo.ToString("dd-MM-yyyy") + "', 'DD-MM-YYYY')  AND P1.BRANCHIND = '" + webUser.UserID + "'";
                        if (model.ClientCode != null) { if (model.ClientCode != "") qry = qry + " AND CLIENTCODE='" + model.ClientCode + "' "; }

                        DataSet ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet(qry, Session["SelectedConn"].ToString());

                        model.dsOut = ds;

                        Session["ReportHeader1"] = "STT Report";
                        Session["ReportHeader2"] = "Date Range : " + model.DateFrom.ToString("dd/MM/yyyy") + " - " + model.DateTo.ToString("dd/MM/yyyy");
                        return View(model);
                    }

                    if (webUser.UserType == UserType.RM)
                    {
                        string qry = "SELECT SETTNO,to_char(STTDATE,'dd-mm-yyyy') STTDATE,CLIENTCODE,P1.PAR_NAME as PAR_NAME, nvl(SYMBOL, '-') SYMBOL,NVL(SERIES, '-') AS SERIES, TOTALSTT FROM SYSADM.STTFILECAP S, SYSADM.PARTYMST P1 WHERE RECORDTYPE = '30'  AND TYPE='" + model.Type + "'    AND S.CLIENTCODE = P1.PAR_CODE AND STTDATE>= TO_DATE('" + model.DateFrom.ToString("dd-MM-yyyy") + "', 'DD-MM-YYYY')                    AND STTDATE<= TO_DATE('" + model.DateTo.ToString("dd-MM-yyyy") + "', 'DD-MM-YYYY') AND P1.BRANCHIND = '" + webUser.UserID + "' ";
                        if (model.ClientCode != null) { if (model.ClientCode != "") qry = qry + " AND CLIENTCODE='" + model.ClientCode + "' "; }

                        qry = qry + " union all         SELECT SETTNO, to_char(STTDATE,'dd-mm-yyyy') STTDATE,CLIENTCODE,P1.PAR_NAME, NVL(S1.SH_SYMBOL, '-') AS SYMBOL, NVL(S1.SH_SERIES, '-') AS SERIES, TOTALSTT FROM SYSADM.STTFILEBSE S, SYSADM.PARTYMST P1, SYSADM.SHAREMST S1   WHERE S.SCRIPCODE = S1.SH_CODE AND RECORDTYPE = '30' AND TYPE='" + model.Type + "'     AND S.CLIENTCODE = P1.PAR_CODE AND STTDATE>= TO_DATE('" + model.DateFrom.ToString("dd-MM-yyyy") + "', 'DD-MM-YYYY')   AND STTDATE<= TO_DATE('" + model.DateTo.ToString("dd-MM-yyyy") + "', 'DD-MM-YYYY')  AND P1.rmcode = '" + webUser.UserID + "'";
                        if (model.ClientCode != null) { if (model.ClientCode != "") qry = qry + " AND CLIENTCODE='" + model.ClientCode + "' "; }

                        DataSet ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet(qry, Session["SelectedConn"].ToString());

                        model.dsOut = ds;

                        Session["ReportHeader1"] = "STT Report";
                        Session["ReportHeader2"] = "Date Range : " + model.DateFrom.ToString("dd/MM/yyyy") + " - " + model.DateTo.ToString("dd/MM/yyyy");
                        return View(model);
                    }
                }
            }
            return RedirectToAction("Index", "TimeOut");
        }


        public ActionResult BranchTurnOverInput()
        {
            GenericModel model = new GenericModel();
            model.DateFrom = DateTime.Parse(Session["FinYearFrom"].ToString());
            model.DateTo = DateTime.Parse(Session["FinYearTo"].ToString());
            return View(model);
        }


        public ActionResult BranchTurnOver(GenericModel model)
        {
            WebUser webUser = Session["WebUser"] as WebUser;
            if (webUser != null)
            {
                if (webUser.LoginValidationStatus == true)
                {
                    List<DBList> selectedDBLists = Session["SelectedDBLists"] as List<DBList>;

                    if (webUser.UserType == UserType.Branch)
                    {
                        string qry = "SELECT TO_CHAR(TRN_DATE,'YYYY') YEAR, SUM(JOBB_TURN) AS JOBB_TURN, SUM(JOBB_BROK)AS JOBB_BROK, SUM(DELV_TURN) AS DELV_TURN,SUM(DELV_BROK) AS DELV_BROK, SUM(JOBB_TURN) + SUM(DELV_TURN)  TOTALTURN, SUM(JOBB_BROK) + SUM(DELV_BROK) TOTALBROK, TO_CHAR(TRN_DATE,'YYYY')  FROM(SELECT  TRN_DATE, 'All' EXCHANGE, p.branchind branch, p.subbranchind subbranch,T.TRN_CLIENTCD CODE, P.PAR_NAME NAME, DECODE(DECODE(NEWBROKTYPE, 'DL', 'D', DECODE(TRN_BROKTYPE, 'DL', 'D', 'J')), 'J', SUM(ABS(TRN_QTY) * TRN_MKTRATE), 0) JOBB_TURN, DECODE(DECODE(NEWBROKTYPE, 'DL', 'D', DECODE(TRN_BROKTYPE, 'DL', 'D', 'J')), 'J', SUM(trn_brok), 0) JOBB_BROK,DECODE(DECODE(NEWBROKTYPE, 'DL', 'D', DECODE(TRN_BROKTYPE, 'DL', 'D', 'J')), 'D', SUM(ABS(TRN_QTY) * TRN_MKTRATE), 0) DELV_TURN,DECODE(DECODE(NEWBROKTYPE, 'DL', 'D', DECODE(TRN_BROKTYPE, 'DL', 'D', 'J')), 'D', SUM(trn_brok), 0) DELV_BROK, B.NAME BRNAME  FROM SYSADM.trnmast T, SYSADM.PARTYMST P, SYSADM.BRANCHMST B WHERE TRN_DATE >= to_date('" + model.DateFrom.ToString("dd-MM-yyyy") + "', 'dd-mm-yyyy') AND TRN_DATE <= to_date('" + model.DateTo.ToString("dd-MM-yyyy") + "', 'dd-mm-yyyy') AND T.TRN_CLIENTCD = P.PAR_CODE AND P.BRANCHIND = B.CODE  and trn_clientcd not in (select accode from SYSADM.stnmast  where accode is not null)  ";

                        if (webUser.UserType == UserType.Branch)
                        {
                            qry = qry + " AND BRANCHIND = '" + webUser.UserID + "' ";
                        }

                        if (webUser.UserType == UserType.RM)
                        {
                            qry = qry + " AND RMCODE = '" + webUser.UserID + "' ";
                        }

                        qry = qry + "   GROUP BY TRN_DATE, p.branchind, p.subbranchind, T.TRN_CLIENTCD, P.PAR_NAME, B.NAME, DECODE(NEWBROKTYPE, 'DL', 'D', DECODE(TRN_BROKTYPE, 'DL', 'D', 'J'))) GROUP BY  TO_CHAR(TRN_DATE, 'YYYY')   ORDER BY 1 DESC";
                        DataSet ds1 = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet(qry, Session["SelectedConn"].ToString());

                        model.dsOut = ds1;


                        qry = "SELECT TO_CHAR(TRN_DATE,'MON-YYYY') YEAR, SUM(JOBB_TURN) AS JOBB_TURN, SUM(JOBB_BROK)AS JOBB_BROK, SUM(DELV_TURN) AS DELV_TURN,SUM(DELV_BROK) AS DELV_BROK, SUM(JOBB_TURN) + SUM(DELV_TURN)  TOTALTURN, SUM(JOBB_BROK) + SUM(DELV_BROK) TOTALBROK, TO_CHAR(TRN_DATE,'YYYYMM') FROM(SELECT  TRN_DATE, 'All' EXCHANGE, p.branchind branch, p.subbranchind subbranch,T.TRN_CLIENTCD CODE, P.PAR_NAME NAME, DECODE(DECODE(NEWBROKTYPE, 'DL', 'D', DECODE(TRN_BROKTYPE, 'DL', 'D', 'J')), 'J', SUM(ABS(TRN_QTY) * TRN_MKTRATE), 0) JOBB_TURN, DECODE(DECODE(NEWBROKTYPE, 'DL', 'D', DECODE(TRN_BROKTYPE, 'DL', 'D', 'J')), 'J', SUM(trn_brok), 0) JOBB_BROK,DECODE(DECODE(NEWBROKTYPE, 'DL', 'D', DECODE(TRN_BROKTYPE, 'DL', 'D', 'J')), 'D', SUM(ABS(TRN_QTY) * TRN_MKTRATE), 0) DELV_TURN,DECODE(DECODE(NEWBROKTYPE, 'DL', 'D', DECODE(TRN_BROKTYPE, 'DL', 'D', 'J')), 'D', SUM(trn_brok), 0) DELV_BROK, B.NAME BRNAME  FROM SYSADM.trnmast T, SYSADM.PARTYMST P, SYSADM.BRANCHMST B WHERE TRN_DATE >= to_date('" + model.DateFrom.ToString("dd-MM-yyyy") + "', 'dd-mm-yyyy') AND TRN_DATE <= to_date('" + model.DateTo.ToString("dd-MM-yyyy") + "', 'dd-mm-yyyy') AND T.TRN_CLIENTCD = P.PAR_CODE AND P.BRANCHIND = B.CODE  and trn_clientcd not in (select accode from SYSADM.stnmast  where accode is not null)  ";

                        if (webUser.UserType == UserType.Branch)
                        {
                            qry = qry + " AND BRANCHIND = '" + webUser.UserID + "' ";
                        }

                        if (webUser.UserType == UserType.RM)
                        {
                            qry = qry + " AND RMCODE = '" + webUser.UserID + "' ";
                        }

                        qry = qry + "   GROUP BY TRN_DATE, p.branchind, p.subbranchind, T.TRN_CLIENTCD, P.PAR_NAME, B.NAME, DECODE(NEWBROKTYPE, 'DL', 'D', DECODE(TRN_BROKTYPE, 'DL', 'D', 'J'))) GROUP BY  TO_CHAR(TRN_DATE,'MON-YYYY'), TO_CHAR(TRN_DATE,'YYYYMM') ORDER BY 8 DESC";
                        DataSet ds2 = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet(qry, Session["SelectedConn"].ToString());

                        model.dsOut2 = ds2;


                        qry = "SELECT TO_CHAR(TRN_DATE,'DD-MON-YYYY') YEAR, SUM(JOBB_TURN) AS JOBB_TURN,SUM(JOBB_BROK) AS JOBB_BROK,SUM(DELV_TURN) AS DELV_TURN, SUM(DELV_BROK) AS DELV_BROK, SUM(JOBB_TURN) + SUM(DELV_TURN)  TOTALTURN, SUM(JOBB_BROK) + SUM(DELV_BROK) TOTALBROK, TO_CHAR(TRN_DATE, 'YYYYMMDD') FROM(SELECT  TRN_DATE, 'All' EXCHANGE, p.branchind branch, p.subbranchind subbranch,T.TRN_CLIENTCD CODE, P.PAR_NAME NAME, DECODE(DECODE(NEWBROKTYPE, 'DL', 'D', DECODE(TRN_BROKTYPE, 'DL', 'D', 'J')), 'J', SUM(ABS(TRN_QTY) * TRN_MKTRATE), 0) JOBB_TURN, DECODE(DECODE(NEWBROKTYPE, 'DL', 'D', DECODE(TRN_BROKTYPE, 'DL', 'D', 'J')), 'J', SUM(trn_brok), 0) JOBB_BROK,DECODE(DECODE(NEWBROKTYPE, 'DL', 'D', DECODE(TRN_BROKTYPE, 'DL', 'D', 'J')), 'D', SUM(ABS(TRN_QTY) * TRN_MKTRATE), 0) DELV_TURN,DECODE(DECODE(NEWBROKTYPE, 'DL', 'D', DECODE(TRN_BROKTYPE, 'DL', 'D', 'J')), 'D', SUM(trn_brok), 0) DELV_BROK, B.NAME BRNAME  FROM SYSADM.trnmast T, SYSADM.PARTYMST P, SYSADM.BRANCHMST B WHERE TRN_DATE >= to_date('" + model.DateFrom.ToString("dd-MM-yyyy") + "', 'dd-mm-yyyy') AND TRN_DATE <= to_date('" + model.DateTo.ToString("dd-MM-yyyy") + "', 'dd-mm-yyyy') AND T.TRN_CLIENTCD = P.PAR_CODE AND P.BRANCHIND = B.CODE  and trn_clientcd not in (select accode from SYSADM.stnmast  where accode is not null)  ";

                        if (webUser.UserType == UserType.Branch)
                        {
                            qry = qry + " AND BRANCHIND = '" + webUser.UserID + "' ";
                        }

                        if (webUser.UserType == UserType.RM)
                        {
                            qry = qry + " AND RMCODE = '" + webUser.UserID + "' ";
                        }

                        qry = qry + "   GROUP BY TRN_DATE, p.branchind, p.subbranchind, T.TRN_CLIENTCD, P.PAR_NAME, B.NAME, DECODE(NEWBROKTYPE, 'DL', 'D', DECODE(TRN_BROKTYPE, 'DL', 'D', 'J')))   GROUP BY  TO_CHAR(TRN_DATE,'DD-MON-YYYY'), TO_CHAR(TRN_DATE,'YYYYMMDD') ORDER BY 8 DESC";
                        DataSet ds3 = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet(qry, Session["SelectedConn"].ToString());

                        model.dsOut3 = ds3;


                        qry = "SELECT PAR_CODE CODE, PAR_NAME NAME , SUM(JOBB_TURN) AS JOBB_TURN,SUM(JOBB_BROK) AS JOBB_BROK,SUM(DELV_TURN) AS DELV_TURN, SUM(DELV_BROK) AS DELV_BROK, SUM(JOBB_TURN) + SUM(DELV_TURN)  TOTALTURN, SUM(JOBB_BROK) + SUM(DELV_BROK) TOTALBROK FROM(SELECT  P.PAR_CODE, P.PAR_NAME, 'All' EXCHANGE, p.branchind branch, p.subbranchind subbranch, T.TRN_CLIENTCD CODE, P.PAR_NAME NAME,DECODE(DECODE(NEWBROKTYPE, 'DL', 'D', DECODE(TRN_BROKTYPE, 'DL', 'D', 'J')), 'J', SUM(ABS(TRN_QTY) * TRN_MKTRATE), 0) JOBB_TURN,DECODE(DECODE(NEWBROKTYPE, 'DL', 'D', DECODE(TRN_BROKTYPE, 'DL', 'D', 'J')), 'J', SUM(trn_brok), 0) JOBB_BROK,DECODE(DECODE(NEWBROKTYPE, 'DL', 'D', DECODE(TRN_BROKTYPE, 'DL', 'D', 'J')), 'D', SUM(ABS(TRN_QTY) * TRN_MKTRATE), 0) DELV_TURN,DECODE(DECODE(NEWBROKTYPE, 'DL', 'D', DECODE(TRN_BROKTYPE, 'DL', 'D', 'J')), 'D', SUM(trn_brok), 0) DELV_BROK, B.NAME BRNAME  FROM SYSADM.trnmast T, SYSADM.PARTYMST P, SYSADM.BRANCHMST B  WHERE TRN_DATE >= to_date('" + model.DateFrom.ToString("dd-MM-yyyy") + "', 'dd-mm-yyyy') AND TRN_DATE <= to_date('" + model.DateTo.ToString("dd-MM-yyyy") + "', 'dd-mm-yyyy') AND T.TRN_CLIENTCD = P.PAR_CODE AND P.BRANCHIND = B.CODE  and trn_clientcd not in (select accode from SYSADM.stnmast  where accode is not null)  ";

                        if (webUser.UserType == UserType.Branch)
                        {
                            qry = qry + " AND BRANCHIND = '" + webUser.UserID + "' ";
                        }

                        if (webUser.UserType == UserType.RM)
                        {
                            qry = qry + " AND RMCODE = '" + webUser.UserID + "' ";
                        }

                        qry = qry + "  GROUP BY P.PAR_CODE, P.PAR_NAME , p.branchind, p.subbranchind, T.TRN_CLIENTCD, B.NAME, DECODE(NEWBROKTYPE, 'DL', 'D', DECODE(TRN_BROKTYPE, 'DL', 'D', 'J')))  GROUP BY PAR_CODE, PAR_NAME ORDER BY 1 DESC";
                        DataSet ds4 = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet(qry, Session["SelectedConn"].ToString());

                        model.dsOut4 = ds4;


                        Session["ReportHeader1"] = "Branch Turnover Report";
                        Session["ReportHeader2"] = "Date Range : " + model.DateFrom.ToString("dd/MM/yyyy") + " - " + model.DateTo.ToString("dd/MM/yyyy");
                        return View(model);

                    }

                    if (webUser.UserType == UserType.RM)
                    {
                        string qry = "SELECT TO_CHAR(TRN_DATE,'YYYY') YEAR, SUM(JOBB_TURN) AS JOBB_TURN, SUM(JOBB_BROK)AS JOBB_BROK, SUM(DELV_TURN) AS DELV_TURN,SUM(DELV_BROK) AS DELV_BROK, SUM(JOBB_TURN) + SUM(DELV_TURN)  TOTALTURN, SUM(JOBB_BROK) + SUM(DELV_BROK) TOTALBROK, TO_CHAR(TRN_DATE,'YYYY')  FROM(SELECT  TRN_DATE, 'All' EXCHANGE, p.rmcode branch, p.subbranchind subbranch,T.TRN_CLIENTCD CODE, P.PAR_NAME NAME, DECODE(DECODE(NEWBROKTYPE, 'DL', 'D', DECODE(TRN_BROKTYPE, 'DL', 'D', 'J')), 'J', SUM(ABS(TRN_QTY) * TRN_MKTRATE), 0) JOBB_TURN, DECODE(DECODE(NEWBROKTYPE, 'DL', 'D', DECODE(TRN_BROKTYPE, 'DL', 'D', 'J')), 'J', SUM(trn_brok), 0) JOBB_BROK,DECODE(DECODE(NEWBROKTYPE, 'DL', 'D', DECODE(TRN_BROKTYPE, 'DL', 'D', 'J')), 'D', SUM(ABS(TRN_QTY) * TRN_MKTRATE), 0) DELV_TURN,DECODE(DECODE(NEWBROKTYPE, 'DL', 'D', DECODE(TRN_BROKTYPE, 'DL', 'D', 'J')), 'D', SUM(trn_brok), 0) DELV_BROK, B.NAME BRNAME  FROM SYSADM.trnmast T, SYSADM.PARTYMST P, SYSADM.BRANCHMST B WHERE TRN_DATE >= to_date('" + model.DateFrom.ToString("dd-MM-yyyy") + "', 'dd-mm-yyyy') AND TRN_DATE <= to_date('" + model.DateTo.ToString("dd-MM-yyyy") + "', 'dd-mm-yyyy') AND T.TRN_CLIENTCD = P.PAR_CODE AND P.rmcode = B.CODE  and trn_clientcd not in (select accode from SYSADM.stnmast  where accode is not null)  ";

                        if (webUser.UserType == UserType.Branch)
                        {
                            qry = qry + " AND BRANCHIND = '" + webUser.UserID + "' ";
                        }

                        if (webUser.UserType == UserType.RM)
                        {
                            qry = qry + " AND RMCODE = '" + webUser.UserID + "' ";
                        }

                        qry = qry + "   GROUP BY TRN_DATE, p.rmcode, p.subbranchind, T.TRN_CLIENTCD, P.PAR_NAME, B.NAME, DECODE(NEWBROKTYPE, 'DL', 'D', DECODE(TRN_BROKTYPE, 'DL', 'D', 'J'))) GROUP BY  TO_CHAR(TRN_DATE, 'YYYY')   ORDER BY 1 DESC";
                        DataSet ds1 = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet(qry, Session["SelectedConn"].ToString());

                        model.dsOut = ds1;


                        qry = "SELECT TO_CHAR(TRN_DATE,'MON-YYYY') YEAR, SUM(JOBB_TURN) AS JOBB_TURN, SUM(JOBB_BROK)AS JOBB_BROK, SUM(DELV_TURN) AS DELV_TURN,SUM(DELV_BROK) AS DELV_BROK, SUM(JOBB_TURN) + SUM(DELV_TURN)  TOTALTURN, SUM(JOBB_BROK) + SUM(DELV_BROK) TOTALBROK, TO_CHAR(TRN_DATE,'YYYYMM') FROM(SELECT  TRN_DATE, 'All' EXCHANGE, p.rmcode branch, p.subbranchind subbranch,T.TRN_CLIENTCD CODE, P.PAR_NAME NAME, DECODE(DECODE(NEWBROKTYPE, 'DL', 'D', DECODE(TRN_BROKTYPE, 'DL', 'D', 'J')), 'J', SUM(ABS(TRN_QTY) * TRN_MKTRATE), 0) JOBB_TURN, DECODE(DECODE(NEWBROKTYPE, 'DL', 'D', DECODE(TRN_BROKTYPE, 'DL', 'D', 'J')), 'J', SUM(trn_brok), 0) JOBB_BROK,DECODE(DECODE(NEWBROKTYPE, 'DL', 'D', DECODE(TRN_BROKTYPE, 'DL', 'D', 'J')), 'D', SUM(ABS(TRN_QTY) * TRN_MKTRATE), 0) DELV_TURN,DECODE(DECODE(NEWBROKTYPE, 'DL', 'D', DECODE(TRN_BROKTYPE, 'DL', 'D', 'J')), 'D', SUM(trn_brok), 0) DELV_BROK, B.NAME BRNAME  FROM SYSADM.trnmast T, SYSADM.PARTYMST P, SYSADM.BRANCHMST B WHERE TRN_DATE >= to_date('" + model.DateFrom.ToString("dd-MM-yyyy") + "', 'dd-mm-yyyy') AND TRN_DATE <= to_date('" + model.DateTo.ToString("dd-MM-yyyy") + "', 'dd-mm-yyyy') AND T.TRN_CLIENTCD = P.PAR_CODE AND P.rmcode = B.CODE  and trn_clientcd not in (select accode from SYSADM.stnmast  where accode is not null)  ";

                        if (webUser.UserType == UserType.Branch)
                        {
                            qry = qry + " AND BRANCHIND = '" + webUser.UserID + "' ";
                        }

                        if (webUser.UserType == UserType.RM)
                        {
                            qry = qry + " AND RMCODE = '" + webUser.UserID + "' ";
                        }

                        qry = qry + "   GROUP BY TRN_DATE, p.rmcode, p.subbranchind, T.TRN_CLIENTCD, P.PAR_NAME, B.NAME, DECODE(NEWBROKTYPE, 'DL', 'D', DECODE(TRN_BROKTYPE, 'DL', 'D', 'J'))) GROUP BY  TO_CHAR(TRN_DATE,'MON-YYYY'), TO_CHAR(TRN_DATE,'YYYYMM') ORDER BY 8 DESC";
                        DataSet ds2 = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet(qry, Session["SelectedConn"].ToString());

                        model.dsOut2 = ds2;


                        qry = "SELECT TO_CHAR(TRN_DATE,'DD-MON-YYYY') YEAR, SUM(JOBB_TURN) AS JOBB_TURN,SUM(JOBB_BROK) AS JOBB_BROK,SUM(DELV_TURN) AS DELV_TURN, SUM(DELV_BROK) AS DELV_BROK, SUM(JOBB_TURN) + SUM(DELV_TURN)  TOTALTURN, SUM(JOBB_BROK) + SUM(DELV_BROK) TOTALBROK, TO_CHAR(TRN_DATE, 'YYYYMMDD') FROM(SELECT  TRN_DATE, 'All' EXCHANGE, p.rmcode branch, p.subbranchind subbranch,T.TRN_CLIENTCD CODE, P.PAR_NAME NAME, DECODE(DECODE(NEWBROKTYPE, 'DL', 'D', DECODE(TRN_BROKTYPE, 'DL', 'D', 'J')), 'J', SUM(ABS(TRN_QTY) * TRN_MKTRATE), 0) JOBB_TURN, DECODE(DECODE(NEWBROKTYPE, 'DL', 'D', DECODE(TRN_BROKTYPE, 'DL', 'D', 'J')), 'J', SUM(trn_brok), 0) JOBB_BROK,DECODE(DECODE(NEWBROKTYPE, 'DL', 'D', DECODE(TRN_BROKTYPE, 'DL', 'D', 'J')), 'D', SUM(ABS(TRN_QTY) * TRN_MKTRATE), 0) DELV_TURN,DECODE(DECODE(NEWBROKTYPE, 'DL', 'D', DECODE(TRN_BROKTYPE, 'DL', 'D', 'J')), 'D', SUM(trn_brok), 0) DELV_BROK, B.NAME BRNAME  FROM SYSADM.trnmast T, SYSADM.PARTYMST P, SYSADM.BRANCHMST B WHERE TRN_DATE >= to_date('" + model.DateFrom.ToString("dd-MM-yyyy") + "', 'dd-mm-yyyy') AND TRN_DATE <= to_date('" + model.DateTo.ToString("dd-MM-yyyy") + "', 'dd-mm-yyyy') AND T.TRN_CLIENTCD = P.PAR_CODE AND P.rmcode = B.CODE  and trn_clientcd not in (select accode from SYSADM.stnmast  where accode is not null)  ";

                        if (webUser.UserType == UserType.Branch)
                        {
                            qry = qry + " AND BRANCHIND = '" + webUser.UserID + "' ";
                        }

                        if (webUser.UserType == UserType.RM)
                        {
                            qry = qry + " AND RMCODE = '" + webUser.UserID + "' ";
                        }

                        qry = qry + "   GROUP BY TRN_DATE, p.rmcode, p.subbranchind, T.TRN_CLIENTCD, P.PAR_NAME, B.NAME, DECODE(NEWBROKTYPE, 'DL', 'D', DECODE(TRN_BROKTYPE, 'DL', 'D', 'J')))   GROUP BY  TO_CHAR(TRN_DATE,'DD-MON-YYYY'), TO_CHAR(TRN_DATE,'YYYYMMDD') ORDER BY 8 DESC";
                        DataSet ds3 = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet(qry, Session["SelectedConn"].ToString());

                        model.dsOut3 = ds3;


                        qry = "SELECT PAR_CODE CODE, PAR_NAME NAME , SUM(JOBB_TURN) AS JOBB_TURN,SUM(JOBB_BROK) AS JOBB_BROK,SUM(DELV_TURN) AS DELV_TURN, SUM(DELV_BROK) AS DELV_BROK, SUM(JOBB_TURN) + SUM(DELV_TURN)  TOTALTURN, SUM(JOBB_BROK) + SUM(DELV_BROK) TOTALBROK FROM(SELECT  P.PAR_CODE, P.PAR_NAME, 'All' EXCHANGE, p.rmcode branch, p.subbranchind subbranch, T.TRN_CLIENTCD CODE, P.PAR_NAME NAME,DECODE(DECODE(NEWBROKTYPE, 'DL', 'D', DECODE(TRN_BROKTYPE, 'DL', 'D', 'J')), 'J', SUM(ABS(TRN_QTY) * TRN_MKTRATE), 0) JOBB_TURN,DECODE(DECODE(NEWBROKTYPE, 'DL', 'D', DECODE(TRN_BROKTYPE, 'DL', 'D', 'J')), 'J', SUM(trn_brok), 0) JOBB_BROK,DECODE(DECODE(NEWBROKTYPE, 'DL', 'D', DECODE(TRN_BROKTYPE, 'DL', 'D', 'J')), 'D', SUM(ABS(TRN_QTY) * TRN_MKTRATE), 0) DELV_TURN,DECODE(DECODE(NEWBROKTYPE, 'DL', 'D', DECODE(TRN_BROKTYPE, 'DL', 'D', 'J')), 'D', SUM(trn_brok), 0) DELV_BROK, B.NAME BRNAME  FROM SYSADM.trnmast T, SYSADM.PARTYMST P, SYSADM.BRANCHMST B  WHERE TRN_DATE >= to_date('" + model.DateFrom.ToString("dd-MM-yyyy") + "', 'dd-mm-yyyy') AND TRN_DATE <= to_date('" + model.DateTo.ToString("dd-MM-yyyy") + "', 'dd-mm-yyyy') AND T.TRN_CLIENTCD = P.PAR_CODE AND P.rmcode = B.CODE  and trn_clientcd not in (select accode from SYSADM.stnmast  where accode is not null)  ";

                        if (webUser.UserType == UserType.Branch)
                        {
                            qry = qry + " AND BRANCHIND = '" + webUser.UserID + "' ";
                        }

                        if (webUser.UserType == UserType.RM)
                        {
                            qry = qry + " AND RMCODE = '" + webUser.UserID + "' ";
                        }

                        qry = qry + "  GROUP BY P.PAR_CODE, P.PAR_NAME , p.rmcode, p.subbranchind, T.TRN_CLIENTCD, B.NAME, DECODE(NEWBROKTYPE, 'DL', 'D', DECODE(TRN_BROKTYPE, 'DL', 'D', 'J')))  GROUP BY PAR_CODE, PAR_NAME ORDER BY 1 DESC";
                        DataSet ds4 = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet(qry, Session["SelectedConn"].ToString());

                        model.dsOut4 = ds4;


                        Session["ReportHeader1"] = "Branch Turnover Report";
                        Session["ReportHeader2"] = "Date Range : " + model.DateFrom.ToString("dd/MM/yyyy") + " - " + model.DateTo.ToString("dd/MM/yyyy");
                        return View(model);

                    }
                }
            }
            return RedirectToAction("Index", "TimeOut");
        }


        public ActionResult AgeingReportInput()
        {
            GenericModel model = new GenericModel();
            model.DateFrom = DateTime.Parse(Session["FinYearFrom"].ToString());
            model.DateTo = DateTime.Parse(Session["FinYearTo"].ToString());
            model.AsOnDate = System.DateTime.Now;
            return View(model);
        }


        public ActionResult AgeingReport(GenericModel model)
        {
            WebUser webUser = Session["WebUser"] as WebUser;
            if (webUser != null)
            {
                if (webUser.LoginValidationStatus == true)
                {
                    List<DBList> selectedDBLists = Session["SelectedDBLists"] as List<DBList>;

                    if (webUser.UserType == UserType.Branch)
                    {
                        string qry = "select branch brcode,'' brname,nvl(mobilenos,'-') mobilenos, TO_DATE('" + model.AsOnDate.ToString("dd-MM-yyyy") + "','DD-MM-YYYY') asondate,party_cd,substr(par_name,1,35) name,sum(clos_bal) Clos_Bal, decode(sign(sum(decode(range,'Range1',bal,0))),1,0,sum(decode(range,'Range1',bal,0))) Range1, decode(sign(sum(decode(range,'Range2',bal,0))),1,0,sum(decode(range,'Range2',bal,0))) Range2, decode(sign(sum(decode(range,'Range3',bal,0))),1,0,sum(decode(range,'Range3',bal,0))) Range3, decode(sign(sum(decode(range,'Range4',bal,0))),1,0,sum(decode(range,'range4',bal,0))) Range4, decode(sign(sum(decode(range,'Range8',bal,0))),1,0,sum(decode(range,'Range8',bal,0))) Range8,sum(holdval) holdval from ( select range,branch,mobilenos,party_cd,par_name,sum(bal) bal,0 clos_bal,wdate,0 holdval from ( select 'Range1' Range,nvl(p2.branchind,'HO') Branch,p2.mobilenos,party_cd,p2.par_name,sum(nvl(p1.credit,0)-nvl(p1.debit,0)) Bal,0 clos_bal,P1.WDATE  from sysadm.partytrn p1,sysadm.partymst p2  where  p1.party_cd=p2.par_code and  p2.branchind='" + webUser.UserID + "' AND wdate>=TO_DATE('" + model.AsOnDate.ToString("dd-MM-yyyy") + "','DD-MM-YYYY')-(" + model.Day1 + "-1)  and wdate<=TO_DATE('" + model.DateTo.ToString("dd-MM-yyyy") + "','DD-MM-YYYY') and p2.groupcode='Sundry Debtors/Creditors'  and ( scripcd not in('F&OMAR','F&OMT') or scripcd is null) group by P1.WDATE,nvl(p2.branchind,'HO'),p2.mobilenos,party_cd,p2.par_name having sum(nvl(p1.credit,0)-nvl(p1.debit,0))<> 0 ) group by wdate,range,branch,mobilenos,party_cd,par_name  having sum(bal)< 0 Union select range,branch,mobilenos,party_cd,par_name,sum(bal) bal,0 clos_bal,wdate, 0 holdval from ( select 'Range2' Range,nvl(p2.branchind,'HO') Branch,p2.mobilenos,party_cd,p2.par_name,sum(nvl(p1.credit,0)-nvl(p1.debit,0)) Bal, 0 clos_bal,p1.wdate  from sysadm.partytrn p1,sysadm.partymst p2  where p1.party_cd=p2.par_code  AND p2.branchind='" + webUser.UserID + "' and wdate<=TO_DATE('" + model.AsOnDate.ToString("dd-MM-yyyy") + "','DD-MM-YYYY')-" + model.Day1 + " and wdate>=TO_DATE('" + model.AsOnDate.ToString("dd-MM-yyyy") + "','DD-MM-YYYY')-(" + model.Day2 + "-1)  and p2.groupcode='Sundry Debtors/Creditors'  and ( scripcd not in('F&OMAR','F&OMT') or scripcd is null) group by p1.wdate,nvl(p2.branchind,'HO'),p2.mobilenos,party_cd,p2.par_name  having sum(nvl(p1.credit,0)-nvl(p1.debit,0))<> 0 ) group by wdate,range,branch,mobilenos,party_cd,par_name  having sum(bal) < 0 Union select range,branch,mobilenos,party_cd,par_name,sum(bal) bal,0 clos_bal,wdate,0 holdval from ( select 'Range3' Range,nvl(p2.branchind,'HO') Branch,p2.mobilenos,party_cd,p2.par_name,sum(nvl(p1.credit,0)-nvl(p1.debit,0)) Bal,0 clos_bal,p1.wdate  from sysadm.partytrn p1,sysadm.partymst p2  where p1.party_cd=p2.par_code and wdate<=TO_DATE('" + model.AsOnDate.ToString("dd-MM-yyyy") + "','DD-MM-YYYY')-" + model.Day2 + " and  p2.branchind='" + webUser.UserID + "' AND wdate>=TO_DATE('" + model.AsOnDate.ToString("dd-MM-yyyy") + "','DD-MM-YYYY')-(" + model.Day3 + "-1)  and p2.groupcode='Sundry Debtors/Creditors' and ( scripcd not in('F&OMAR','F&OMT') or scripcd is null)  group by p1.wdate,nvl(p2.branchind,'HO'),p2.mobilenos,party_cd,p2.par_name  having sum(nvl(p1.credit,0)-nvl(p1.debit,0))<> 0 ) group by wdate,range,branch,mobilenos ,party_cd,par_name  having sum(bal) < 0 Union select range,branch,mobilenos,party_cd,par_name,sum(bal) bal,0 clos_bal,wdate,0 holdval from ( select 'Range4' Range,nvl(p2.branchind,'HO') Branch,p2.mobilenos,party_cd,p2.par_name,sum(nvl(p1.credit,0)-nvl(p1.debit,0)) Bal,0 clos_bal,p1.wdate  from sysadm.partytrn p1,sysadm.partymst p2  where p1.party_cd=p2.par_code and wdate<=TO_DATE('" + model.AsOnDate.ToString("dd-MM-yyyy") + "','DD-MM-YYYY')-(" + model.Day3 + "-1) and  p2.branchind='" + webUser.UserID + "' AND wdate>=TO_DATE('" + model.AsOnDate.ToString("dd-MM-yyyy") + "','DD-MM-YYYY')-" + model.Day4 + " and p2.groupcode='Sundry Debtors/Creditors' and ( scripcd not in('F&OMAR','F&OMT') or scripcd is null)  group by p1.wdate,nvl(p2.branchind,'HO'),p2.mobilenos ,party_cd,p2.par_name having sum(nvl(p1.credit,0)-nvl(p1.debit,0))<> 0 ) group by wdate,range,branch,mobilenos,party_cd,par_name  having sum(bal) < 0 Union select range,branch,mobilenos,party_cd,par_name,sum(bal) bal,0 clos_bal,wdate,0 holdval from ( select 'Range8' Range,nvl(p2.branchind,'HO') Branch,p2.mobilenos,party_cd,p2.par_name,sum(nvl(p1.credit,0)-nvl(p1.debit,0)) Bal,0 clos_bal,p1.wdate  from sysadm.partytrn p1,sysadm.partymst p2  where p1.party_cd=p2.par_code and wdate<=TO_DATE('" + model.AsOnDate.ToString("dd-MM-yyyy") + "','DD-MM-YYYY')-(" + model.Day4 + "+1)  and  p2.branchind='" + webUser.UserID + "' AND p2.groupcode='Sundry Debtors/Creditors' and ( scripcd not in('F&OMAR','F&OMT') or scripcd is null)  group by p1.wdate,nvl(p2.branchind,'HO'),p2.mobilenos,party_cd,p2.par_name having sum(nvl(p1.credit,0)-nvl(p1.debit,0))<> 0 ) group by  wdate,range,branch,mobilenos,party_cd,par_name  having sum(bal) < 0 Union select 'Range123' Range,nvl(p2.branchind,'HO') Branch,p2.mobilenos,party_cd,p2.par_name,0 Bal,sum(nvl(p1.credit,0)-nvl(p1.debit,0)) clos_bal,TO_DATE('" + model.AsOnDate.ToString("dd-MM-yyyy") + "','DD-MM-YYYY') wdate,0 holdval  from sysadm.partytrn p1,sysadm.partymst p2  where p1.party_cd=p2.par_code and wdate<=TO_DATE('" + model.AsOnDate.ToString("dd-MM-yyyy") + "','DD-MM-YYYY')  AND p2.branchind='" + webUser.UserID + "' and p2.groupcode='Sundry Debtors/Creditors'  and ( scripcd not in('F&OMAR','F&OMT') or scripcd is null)  group by nvl(p2.branchind,'HO'),p2.mobilenos ,party_cd,p2.par_name  having sum(nvl(p1.credit,0)-nvl(p1.debit,0))<> 0 ) group by BRANCH,party_cd,par_name,mobilenos  having sum(clos_bal)<0 order by 7 ";
                        DataSet ds1 = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet(qry, Session["SelectedConn"].ToString());

                        model.dsOut = ds1;

                        Session["ReportHeader1"] = "Ageing Report";
                        Session["ReportHeader2"] = "Date Range : " + model.DateFrom.ToString("dd/MM/yyyy") + " - " + model.DateTo.ToString("dd/MM/yyyy");
                        return View(model);
                    }

                    if (webUser.UserType == UserType.RM)
                    {
                        string qry = "select branch brcode,'' brname,nvl(mobilenos,'-') mobilenos, TO_DATE('" + model.AsOnDate.ToString("dd-MM-yyyy") + "','DD-MM-YYYY') asondate,party_cd,substr(par_name,1,35) name,sum(clos_bal) Clos_Bal, decode(sign(sum(decode(range,'Range1',bal,0))),1,0,sum(decode(range,'Range1',bal,0))) Range1, decode(sign(sum(decode(range,'Range2',bal,0))),1,0,sum(decode(range,'Range2',bal,0))) Range2, decode(sign(sum(decode(range,'Range3',bal,0))),1,0,sum(decode(range,'Range3',bal,0))) Range3, decode(sign(sum(decode(range,'Range4',bal,0))),1,0,sum(decode(range,'range4',bal,0))) Range4, decode(sign(sum(decode(range,'Range8',bal,0))),1,0,sum(decode(range,'Range8',bal,0))) Range8,sum(holdval) holdval from ( select range,branch,mobilenos,party_cd,par_name,sum(bal) bal,0 clos_bal,wdate,0 holdval from ( select 'Range1' Range,nvl(p2.branchind,'HO') Branch,p2.mobilenos,party_cd,p2.par_name,sum(nvl(p1.credit,0)-nvl(p1.debit,0)) Bal,0 clos_bal,P1.WDATE  from sysadm.partytrn p1,sysadm.partymst p2  where  p1.party_cd=p2.par_code and  p2.rmcode='" + webUser.UserID + "' AND wdate>=TO_DATE('" + model.AsOnDate.ToString("dd-MM-yyyy") + "','DD-MM-YYYY')-(" + model.Day1 + "-1)  and wdate<=TO_DATE('" + model.DateTo.ToString("dd-MM-yyyy") + "','DD-MM-YYYY') and p2.groupcode='Sundry Debtors/Creditors'  and ( scripcd not in('F&OMAR','F&OMT') or scripcd is null) group by P1.WDATE,nvl(p2.branchind,'HO'),p2.mobilenos,party_cd,p2.par_name having sum(nvl(p1.credit,0)-nvl(p1.debit,0))<> 0 ) group by wdate,range,branch,mobilenos,party_cd,par_name  having sum(bal)< 0 Union select range,branch,mobilenos,party_cd,par_name,sum(bal) bal,0 clos_bal,wdate, 0 holdval from ( select 'Range2' Range,nvl(p2.branchind,'HO') Branch,p2.mobilenos,party_cd,p2.par_name,sum(nvl(p1.credit,0)-nvl(p1.debit,0)) Bal, 0 clos_bal,p1.wdate  from sysadm.partytrn p1,sysadm.partymst p2  where p1.party_cd=p2.par_code  AND p2.rmcode='" + webUser.UserID + "' and wdate<=TO_DATE('" + model.AsOnDate.ToString("dd-MM-yyyy") + "','DD-MM-YYYY')-" + model.Day1 + " and wdate>=TO_DATE('" + model.AsOnDate.ToString("dd-MM-yyyy") + "','DD-MM-YYYY')-(" + model.Day2 + "-1)  and p2.groupcode='Sundry Debtors/Creditors'  and ( scripcd not in('F&OMAR','F&OMT') or scripcd is null) group by p1.wdate,nvl(p2.branchind,'HO'),p2.mobilenos,party_cd,p2.par_name  having sum(nvl(p1.credit,0)-nvl(p1.debit,0))<> 0 ) group by wdate,range,branch,mobilenos,party_cd,par_name  having sum(bal) < 0 Union select range,branch,mobilenos,party_cd,par_name,sum(bal) bal,0 clos_bal,wdate,0 holdval from ( select 'Range3' Range,nvl(p2.branchind,'HO') Branch,p2.mobilenos,party_cd,p2.par_name,sum(nvl(p1.credit,0)-nvl(p1.debit,0)) Bal,0 clos_bal,p1.wdate  from sysadm.partytrn p1,sysadm.partymst p2  where p1.party_cd=p2.par_code and wdate<=TO_DATE('" + model.AsOnDate.ToString("dd-MM-yyyy") + "','DD-MM-YYYY')-" + model.Day2 + " and  p2.rmcode='" + webUser.UserID + "' AND wdate>=TO_DATE('" + model.AsOnDate.ToString("dd-MM-yyyy") + "','DD-MM-YYYY')-(" + model.Day3 + "-1)  and p2.groupcode='Sundry Debtors/Creditors' and ( scripcd not in('F&OMAR','F&OMT') or scripcd is null)  group by p1.wdate,nvl(p2.branchind,'HO'),p2.mobilenos,party_cd,p2.par_name  having sum(nvl(p1.credit,0)-nvl(p1.debit,0))<> 0 ) group by wdate,range,branch,mobilenos ,party_cd,par_name  having sum(bal) < 0 Union select range,branch,mobilenos,party_cd,par_name,sum(bal) bal,0 clos_bal,wdate,0 holdval from ( select 'Range4' Range,nvl(p2.branchind,'HO') Branch,p2.mobilenos,party_cd,p2.par_name,sum(nvl(p1.credit,0)-nvl(p1.debit,0)) Bal,0 clos_bal,p1.wdate  from sysadm.partytrn p1,sysadm.partymst p2  where p1.party_cd=p2.par_code and wdate<=TO_DATE('" + model.AsOnDate.ToString("dd-MM-yyyy") + "','DD-MM-YYYY')-(" + model.Day3 + "-1) and  p2.rmcode='" + webUser.UserID + "' AND wdate>=TO_DATE('" + model.AsOnDate.ToString("dd-MM-yyyy") + "','DD-MM-YYYY')-" + model.Day4 + " and p2.groupcode='Sundry Debtors/Creditors' and ( scripcd not in('F&OMAR','F&OMT') or scripcd is null)  group by p1.wdate,nvl(p2.branchind,'HO'),p2.mobilenos ,party_cd,p2.par_name having sum(nvl(p1.credit,0)-nvl(p1.debit,0))<> 0 ) group by wdate,range,branch,mobilenos,party_cd,par_name  having sum(bal) < 0 Union select range,branch,mobilenos,party_cd,par_name,sum(bal) bal,0 clos_bal,wdate,0 holdval from ( select 'Range8' Range,nvl(p2.branchind,'HO') Branch,p2.mobilenos,party_cd,p2.par_name,sum(nvl(p1.credit,0)-nvl(p1.debit,0)) Bal,0 clos_bal,p1.wdate  from sysadm.partytrn p1,sysadm.partymst p2  where p1.party_cd=p2.par_code and wdate<=TO_DATE('" + model.AsOnDate.ToString("dd-MM-yyyy") + "','DD-MM-YYYY')-(" + model.Day4 + "+1)  and  p2.rmcode='" + webUser.UserID + "' AND p2.groupcode='Sundry Debtors/Creditors' and ( scripcd not in('F&OMAR','F&OMT') or scripcd is null)  group by p1.wdate,nvl(p2.branchind,'HO'),p2.mobilenos,party_cd,p2.par_name having sum(nvl(p1.credit,0)-nvl(p1.debit,0))<> 0 ) group by  wdate,range,branch,mobilenos,party_cd,par_name  having sum(bal) < 0 Union select 'Range123' Range,nvl(p2.branchind,'HO') Branch,p2.mobilenos,party_cd,p2.par_name,0 Bal,sum(nvl(p1.credit,0)-nvl(p1.debit,0)) clos_bal,TO_DATE('" + model.AsOnDate.ToString("dd-MM-yyyy") + "','DD-MM-YYYY') wdate,0 holdval  from sysadm.partytrn p1,sysadm.partymst p2  where p1.party_cd=p2.par_code and wdate<=TO_DATE('" + model.AsOnDate.ToString("dd-MM-yyyy") + "','DD-MM-YYYY')  AND p2.rmcode='" + webUser.UserID + "' and p2.groupcode='Sundry Debtors/Creditors'  and ( scripcd not in('F&OMAR','F&OMT') or scripcd is null)  group by nvl(p2.branchind,'HO'),p2.mobilenos ,party_cd,p2.par_name  having sum(nvl(p1.credit,0)-nvl(p1.debit,0))<> 0 ) group by BRANCH,party_cd,par_name,mobilenos  having sum(clos_bal)<0 order by 7 ";
                        DataSet ds1 = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet(qry, Session["SelectedConn"].ToString());

                        model.dsOut = ds1;

                        Session["ReportHeader1"] = "Ageing Report";
                        Session["ReportHeader2"] = "Date Range : " + model.DateFrom.ToString("dd/MM/yyyy") + " - " + model.DateTo.ToString("dd/MM/yyyy");
                        return View(model);
                    }
                }
            }
            return RedirectToAction("Index", "TimeOut");
        }

    }
}