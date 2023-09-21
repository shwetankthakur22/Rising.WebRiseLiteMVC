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

    public class FinancialController : Controller
    {

        string dbuser = ConfigurationManager.AppSettings["DBUSER"];


        public ActionResult DebtorsCreditors(string code)
        {
            if (Session["WebUser"] == null)
            {
                TempData["AlertMessage"] = "Session Time Out Please Login Again";
                return RedirectToAction("Index", "Login");

            }
            else
            {
                if (Session["BranchSearchCode"] != null) code = Session["BranchSearchCode"].ToString();
                DebtorsCreditorsInput model = new DebtorsCreditorsInput();
                model.OnDate = DateTime.Parse(Session["FinYearFrom"].ToString());
                return View(model);


            }
        }

        public static DebtorsCreditorsOutput lstDcOut;

        public ActionResult DebtorCredtorList(DebtorsCreditorsOutput model)
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
                    WebUser webUser = Session["WebUser"] as WebUser;
                    if (webUser == null) return null;
                    if (model.Exchange.ToString() == "NSE" || model.Exchange.ToString() == "BSE")
                    {

                        string qry = "select to_date('" + model.OnDate.ToString("ddMMMyyyy") + "') asondate,party_cd,par_name,branchind, decode(sign(sum(credit-debit+opbal)),-1,abs(sum(credit-debit+opbal)),0) Debit  ,decode(sign(sum(credit-debit+opbal)),1,abs(sum(credit-debit+opbal)),0) Credit from (  select a.party_cd,par_name,NVL(b.branchind,'Not Defined') as branchind,  sum(nvl(a.debit,0)) Debit,sum(nvl(a.credit,0)) Credit,0 opbal  from SYSADM.PARTYTRN a ,IFSC.CUPARTYMST b where par_code=a.party_cd  and wdate<=to_date('" + model.OnDate.ToString("ddMMMyyyy") + "')  AND  b.grouplevel1='Sundry Debtors/Creditors' and b.par_name not like 'DM-%' and b.par_name not like 'CM-%'  and b.par_name not like 'SM-%' and ( BILLNO not in('CDSMarg') or BILLNO is null)  and a.narr not like 'Op.Bal%'  group by a.party_cd,par_name,b.branchind  Union All  select party_cd,par_name,NVL(branchind,'Not Defined') as branchind,0 debit,0 credit,nvl(opbal,0) opbal  from " + dbuser + ".CUPARTYMST p1," + dbuser + ".CUPARTYMST_fixes p2 where p1.par_code=p2.party_cd  AND  grouplevel1='" + model.AccountGroup + "'  and par_name not like 'DM-%' and par_name not like 'CM-%'  and par_name not like 'SM-%'  and opbal<>0  )  group by party_cd,par_name,branchind having sum(credit-debit+opbal)<>0  order by party_Cd,par_name";

                        DataSet ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet(qry, Session["SelectedConn"].ToString());
                        lstDcOut = new DebtorsCreditorsOutput();
                        lstDcOut.listDebtorsCreditorsOutputRow = new List<DebtorsCreditorsOutputRow>();
                        lstDcOut.OnDate = model.OnDate;

                        int c = 0;
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            if (c == 0)
                            {
                                lstDcOut.OnDate = model.OnDate;

                                // lstDcOut.ClientName = row["PAR_NAME"].ToString();
                            }
                            c++;
                            DebtorsCreditorsOutputRow dco = new DebtorsCreditorsOutputRow();
                            dco.OnDate = DateTime.Parse(row["asondate"].ToString());
                            dco.Code = row["party_cd"].ToString();
                            dco.Branch = row["branchind"].ToString();
                            dco.Name = row["par_name"].ToString();
                            dco.Debit = row["Debit"].ToString();
                            dco.Credit = row["Credit"].ToString();



                            lstDcOut.listDebtorsCreditorsOutputRow.Add(dco);
                        }
                    }

                    return View(lstDcOut);
                }


                catch (Exception ex)
                {
                    TempData["AlertMessage"] = ex;
                    return RedirectToAction("BillSummary", "Report");
                }
            }
        }


        public ActionResult TrialBalanceInput()
        {
            if (Session["WebUser"] == null)
            {
                TempData["AlertMessage"] = "Session Time Out Please Login Again";
                return RedirectToAction("Index", "Login");

            }
            else
            {
                FinancialLedgerInput model = new FinancialLedgerInput();
                model.DateFrom = DateTime.Parse(Session["FinYearFrom"].ToString());
                model.DateTo = DateTime.Parse(Session["FinYearTo"].ToString());
                return View(model);

            }
        }


        public ActionResult TrialBalance(FinancialLedgerInput model)
        {
            if (Session["WebUser"] == null)
            {
                TempData["AlertMessage"] = "Session Time Out Please Login Again";
                return RedirectToAction("Index", "Login");

            }
            else
            {
                string nul = null;
                WebUser webUser = Session["WebUser"] as WebUser;
                if (webUser != null)
                {
                    if (webUser.LoginValidationStatus == true)
                    {
                        List<DBList> selectedDBLists = Session["SelectedDBLists"] as List<DBList>;


                        string qry = "SELECT 'NSE' Exchange,'ASON' REPTYPE,TO_DATE('" + model.DateFrom.ToString("ddMMMyyyy") + "') FDATE,TO_DATE('" + model.DateTo.ToString("ddMMMyyyy") + "') TDATE, code,name,sum(opbal) opbal,sum(debit) debit,sum(credit) credit,gr,gr1,gr2  ,decode(sign(sum(CREDIT)-sum(DEBIT)+sum(OPBAL)),-1,abs(sum(CREDIT)-sum(DEBIT)+sum(OPBAL)),0) FINALDEBIT,  decode(sign(sum(CREDIT)-sum(DEBIT)+sum(OPBAL)),1,abs(sum(CREDIT)-sum(DEBIT)+sum(OPBAL)),0) FINALCREDIT  from ( select pt.party_cd code ,par_name name ,0 opbal ,SUM(NVL(pt.debit,0)) Debit,sum(NVL(PT.CREDIT,0)) Credit ,groupcode gr,grouplevel1 gr1,grouplevel2 gr2 from SYSADM.PARTYTRN pt ,IFSC.CUPARTYMST p where par_code=pt.party_cd   and wdate<=to_date('" + model.DateFrom.ToString("ddMMMyyyy") + "') and pt.exchange='NSE' and (narr not like'Op.B%' or narr is null) group by pt.party_cd,par_name,groupcode,grouplevel1,grouplevel2 union all select pt1.party_cd code ,par_name name , NVL(PT1.CREDIT,0)-NVL(pt1.debit,0) opbal ,0 Debit,0 Credit ,groupcode gr,grouplevel1 gr1,grouplevel2 gr2 from IFSC.CUPARTYMST p,SYSADM.PARTYTRN pt1 where par_code=pt1.party_cd and pt1.narr like 'Op.B%' and NVL(PT1.CREDIT,0)-NVL(pt1.debit,0)<>0 and pt1.exchange='NSE' union all select shdiffaccount,par_name,0,sum(nvl(c1.debit,0)) Debit,sum(nvl(c1.credit,0)) Credit, groupcode gr,grouplevel1 gr1,grouplevel2 gr2  from IFSC.CUSTOCKTRN c1,IFSC.CUPARA,IFSC.CUPARTYMST P where par_code=shdiffaccount and c1.exchange=CUpara.exchange  and wdate<=to_date('" + model.DateTo.ToString("ddMMMyyyy") + "') and c1.exchange='NSE' group by shdiffaccount,par_name,groupcode,grouplevel1,grouplevel2  ) group by code,name,gr,gr1,gr2  having sum(credit)-sum(debit)+sum(opbal)<>0";
                        DataSet ds1 = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet(qry, Session["SelectedConn"].ToString());

                        TrialBalanceInOut newModel = new TrialBalanceInOut();
                        newModel.DateFrom = model.DateFrom;
                        newModel.DateTo = model.DateTo;

                        newModel.dsOut = ds1;

                        Session["ReportClientCode"] = "";
                        Session["ReportClientName"] = "";
                        Session["ReportHeader1"] = "Trial Balance Report";
                        Session["ReportHeader2"] = "Date Range : " + model.DateFrom.ToString("dd/MM/yyyy") + " - " + model.DateTo.ToString("dd/MM/yyyy");
                        return View(newModel);




                    }
                }
                return View();
            }
        }


        //---------------Service tax Register----------------------
        [HttpGet]
        public ActionResult ServiceTaxRegister()
        {
            FinancialInput model = new FinancialInput();
            model.FinancialFrom = DateTime.Parse(Session["FinYearFrom"].ToString());
            model.FinancialTo = DateTime.Parse(Session["FinYearTo"].ToString());
            return View();
        }


        //---------------Stamp Duty Registor----------------------
        [HttpGet]
        public ActionResult StampDutyRegistor()
        {
            FinancialInput model = new FinancialInput();
            model.DateFrom = DateTime.Parse(Session["FinYearFrom"].ToString());
            model.DateTo = DateTime.Parse(Session["FinYearTo"].ToString());
            return View();
        }

        //---------------Profit Loss----------------------
        [HttpGet]
        public ActionResult ProftLossAccount()
        {

            FinancialInput model = new FinancialInput();
            model.DateFrom = DateTime.Parse(Session["FinYearFrom"].ToString());
            return View(model);
        }


    }

}