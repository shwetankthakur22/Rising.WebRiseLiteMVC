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

    public class TradeConfController : Controller
    {
        // GET: TradeConf
        string dbuser = ConfigurationManager.AppSettings["DBUSER"];
        public ActionResult Index(string code)
        {

            if (Session["WebUser"] == null)
            {
                TempData["AlertMessage"] = "Session Time Out Please Login Again";
                return RedirectToAction("Index", "Login");

            }
            else
            {
                try
                { //for date range
                    if (Session["BranchSearchCode"] != null) code = Session["BranchSearchCode"].ToString();
                    TradeConfInput model = new TradeConfInput();
                    model.DateFrom = DateTime.Parse(Session["FinYearFrom"].ToString());
                    model.DateTo = DateTime.Parse(Session["FinYearTo"].ToString());
                    model.FinAsOn = DateTime.Parse(Session["FinYearTo"].ToString());
                    model.ClientCodeFrom = code;
                    model.ClientCodeTo = code;
                    SelectList lst4 = new SelectList(Enum.GetValues(typeof(enumIndexLists)).Cast<enumIndexLists>().Select(v => v.ToString()).ToList());
                    ViewBag.enumIndexLists = lst4;
                    //---------get symbol list
                    List<DBList> selectedDBLists = Session["SelectedDBLists"] as List<DBList>;
                    List<Oracle.ManagedDataAccess.Client.OracleParameter> lst = new List<Oracle.ManagedDataAccess.Client.OracleParameter>();
                    string exchanges = String.Join(",", selectedDBLists.Select(o => o.DBName));
                    lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("Exchanges_", exchanges, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                    DataSet ds = MvcApplication.OracleDBHelperCore().CustomHelper.CustomDataSet("" + dbuser + ".N$GET_SymbolList", lst, Session["SelectedConn"].ToString());
                    model.SymbolList = new List<SelectListItem>();
                    model.SymbolList.Add(new SelectListItem { Text = "All", Value = "-1" });
                    for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                    {
                        model.SymbolList.Add(new SelectListItem { Text = ds.Tables[0].Rows[i][0].ToString(), Value = ds.Tables[0].Rows[i][1].ToString() });
                    }


                    return View(model);
                }
                catch (Exception ex)
                {
                    TempData["AlertMessage"] = ex;
                    return RedirectToAction("Index", "TradeConf");
                }
            }
        }

        public static TradeConfOutput lstOut;

        public ActionResult Confirmation(TradeConfInput model)
        {
            if (model.DateFrom > model.DateTo)
            {
                TempData["AlertMessage"] = "Invalid Date";
                return RedirectToAction("Index", "TradeConf");
            }
            if (model.ClientCodeFrom != null) model.ClientCodeFrom = model.ClientCodeFrom.ToUpper();
            if (Session["WebUser"] == null)
            {
                TempData["AlertMessage"] = "Session Time Out Please Login Again";
                return RedirectToAction("Index", "Login");

            }
            else
            {
                try
                {
                    string nul = null;

                    WebUser webUser = Session["WebUser"] as WebUser;
                    Session["ReportClientCode"] = webUser.UserID;
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
                            lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("CLIENTCODE_", webUser.UserID, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                            lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("BRANCHCODE_", nul, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                            lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("SUBBRANCHCODE_", nul, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                            lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("GROUPCODE_", nul, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                            lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("RMCODE_", nul, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));

                            Session["ReportClientCode"] = webUser.UserID;
                            Session["ReportClientName"] = webUser.UserName;
                        }
                        else if (webUser.UserType == UserType.Branch)
                        {
                            if (model.ClientCodeFrom != null)
                            {
                                model.ClientCodeTo = model.ClientCodeFrom;
                                DataSet ds1 = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet("SELECT * FROM " + dbuser + ".partymst where par_code='" + model.ClientCodeFrom + "' and branchind='" + webUser.UserID + "'", Session["SelectedConn"].ToString());
                                if (ds1.Tables[0].Rows.Count == 0)
                                {
                                    TempData["AlertMessage"] = "Invalid Code : " + model.ClientCodeFrom;
                                    return RedirectToAction("Index", "TradeConf");
                                }

                                Session["ReportClientCode"] = ds1.Tables[0].Rows[0]["par_code"].ToString();
                                Session["ReportClientName"] = ds1.Tables[0].Rows[0]["par_name"].ToString();
                                Session["ReportClientPanNo"] = ds1.Tables[0].Rows[0]["itaxno"].ToString();
                            }
                            lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("CLIENTCODE_", model.ClientCodeFrom, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                            lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("BRANCHCODE_", webUser.UserID, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                            lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("SUBBRANCHCODE_", nul, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                            lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("GROUPCODE_", nul, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                            lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("RMCODE_", nul, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                        }

                        else if (webUser.UserType == UserType.RM)
                        {
                            model.ClientCodeTo = model.ClientCodeFrom;
                            DataSet ds1 = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet("SELECT * FROM " + dbuser + ".partymst where par_code='" + model.ClientCodeFrom + "' and RMCODE='" + webUser.UserID + "'", Session["SelectedConn"].ToString());
                            if (ds1.Tables[0].Rows.Count == 0)
                            {
                                TempData["AlertMessage"] = "Invalid Code : " + model.ClientCodeFrom;
                                return RedirectToAction("Index", "TradeConf");
                            }
                            Session["ReportClientCode"] = ds1.Tables[0].Rows[0]["par_code"].ToString();
                            Session["ReportClientName"] = ds1.Tables[0].Rows[0]["par_name"].ToString();
                            Session["ReportClientPanNo"] = ds1.Tables[0].Rows[0]["itaxno"].ToString();
                            lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("CLIENTCODE_", model.ClientCodeFrom, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                            lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("BRANCHCODE", nul, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                            lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("SUBBRANCHCODE", nul, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                            lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("GROUPCODE", nul, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                            lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("RMCODE", nul, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                        }
                        else if (webUser.UserType == UserType.Admin)
                        {
                            model.ClientCodeTo = model.ClientCodeFrom;

                            DataSet ds1 = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet("SELECT * FROM " + dbuser + ".cupartymst where par_code='" + model.ClientCodeFrom + "'", Session["SelectedConn"].ToString());
                            if (ds1.Tables[0].Rows.Count == 0)
                            {
                                TempData["AlertMessage"] = "Invalid Code : " + model.ClientCodeFrom;
                                return RedirectToAction("Index", "TradeConf");
                            }

                            Session["ReportClientCode"] = ds1.Tables[0].Rows[0]["par_code"].ToString();
                            Session["ReportClientName"] = ds1.Tables[0].Rows[0]["par_name"].ToString();
                            Session["ReportClientPanNo"] = ds1.Tables[0].Rows[0]["itaxno"].ToString();


                            model.ClientCodeTo = model.ClientCodeFrom;
                            lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("CLIENTCODE_", model.ClientCodeFrom, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                            lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("BRANCHCODE", nul, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                            lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("SUBBRANCHCODE", nul, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                            lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("GROUPCODE", nul, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                            lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("RMCODE", nul, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));


                        }

                        lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("DATEFROM_", model.DateFrom, Oracle.ManagedDataAccess.Client.OracleDbType.Date, ParameterDirection.Input));
                        lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("DATETO_", model.DateTo, Oracle.ManagedDataAccess.Client.OracleDbType.Date, ParameterDirection.Input));                
                        lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("INSTRUMENTTYPE_", model.Index.ToString() == "ALL" ? null : model.Index.ToString(), Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                        // string exchanges = String.Join(",", selectedDBLists.Select(o => o.Exchange));
                        lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("EXCHANGES_", model.Exchange.ToString(), Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                        List<DBList> selectedDBList = Session["SelectedDBLists"] as List<DBList>;
                        lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("USER_", (selectedDBList.Count() == 0 ? "IFSC" : selectedDBList[0].FinDBUser), Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));

                       
                           
                            string qry = "select TRADE_TIME TradeTime,CLNT ClientCode,pAR_NAME ClientName, INS_TYP,STK ScripIsin,a.SYMBOL SCRIPNAME,a.EXPIRYDATE SCRIPCODE,TRADE_DATE TradeDate,TR_ST,a.MULTIPLIER,TRN_QTY Qty,TRADEPRICE TradeRate,NETPRICE NetRate,a.BROK Brokrage,PVALUE,SVALUE,OPTIONTYPE,CONTRACT,GROUPCLIENT,COMPANY,ADD1,ADD2,ADD3, ADD4,PHNOS, BRANCHIND,a.EXCHANGE FLAG,TRN_SLNO,  sum(nvl(PT1.stax,0))+A.STAX stax ,sum(nvl(PT1.wnsetax,0))+A.NSETAX nsetax,sum(nvl(PT1.stamp,0))+A.STAMP stamp,sum(nvl(PT1.staxturn,0))+A.STAX_TURN stax_turn,a.userid userid,A.TRADENO TradeNo,A.ORDERNO OrderNo  from (  select TRADE_TIME,CLNT,pAR_NAME, INS_TYP,STK,a.SYMBOL,a.EXPIRYDATE,TRADE_DATE,TR_ST,a.MULTIPLIER,TRN_QTY,TRADEPRICE,NETPRICE  ,a.BROK ,PVALUE,SVALUE,OPTIONTYPE,CONTRACT,GROUPCLIENT,COMPANY,ADD1,ADD2,ADD3, ADD4,PHNOS, BRANCHIND,a.EXCHANGE,TRN_SLNO,  sum(nvl(pt.stax,0)) stax ,sum(nvl(pt.wnsetax,0)) nsetax,sum(nvl(pt.stamp,0)) stamp,sum(nvl(pt.staxturn,0)) stax_turn ,a.userid userid,A.TRADENO,A.ORDERNO  from (  select f.trade_time,f.clientcode clnt,pt.par_name, f.Instrument_type AS  ins_typ,F.STRIKEPRICE AS STK ,f.symbol,f.expirydate, f.trade_date,f.tradestatus tr_st,f.multiplier ,f.trn_qty/C.Mktlot trn_qty ,f.tradeprice,f.netprice,f.TRN_BROK brok, ABS(decode(sign(f.trn_qty),1,decode(p.BROKERAGEMETHOD,'Pay',(tradeprice*trn_qty*multiplier)+f.trn_brok,  'Charge',(tradeprice*trn_qty*f.multiplier)+trn_brok,(tradeprice*trn_qty*f.multiplier)),0)) PValue,  ABS(decode(sign(trn_qty),-1,decode(p.brokeragemethod,'Pay',(tradeprice*trn_qty*f.multiplier)+trn_brok,  'Charge',  (tradeprice*trn_qty*f.multiplier)+trn_brok,(tradeprice*trn_qty*f.multiplier)),0))  SVALUE, f.OPTIONTYPE,f.securityname as  contract, groupclient,PA.COMPANY,PA.ADD1,PA.ADD2,PA.ADD3,PA.ADD4,PA.PHNOS,PT.BRANCHIND,f.exchange,f.trn_slno,0 stax,0 nsetax,0 stamp,0 stax_turn,nvl(f.userid,0) userid,tradeno,substr(orderno,1,19) orderno  from  " + dbuser + ".CUTRNMAST f," + dbuser + ".CUPARTYMST_fixes p ," + dbuser + ".CUPARTYMST pt," + dbuser + ".CUPARA PA," + dbuser + ".cUcontracts C  Where f.clientcode = p.party_cd And p.Exchange = PA.Exchange And pt.par_code = p.party_cd And f.Exchange = p.Exchange  and  trade_date>=to_date('" + model.DateFrom.ToString("ddMMMyyyy") + "') and trade_date<=to_date('" + model.DateTo.ToString("ddMMMyyyy") + "')  and (remark <>'PS03' or remark is null) and C.exchange(+)=f.exchange and f.securityname=C.contname(+)  and f.clientcode<>'" + model.Exchange.ToString() + "'  and p.exchange='"+model.Exchange.ToString()+"' and pt.par_code>='" + model.ClientCodeFrom + "'  and pt.par_code<='" + model.ClientCodeFrom + "' and f.tradestatus not in('BF','CF') and f.tradestatus <>'CL' ) A,SYSADM.PARTYTRN pt where tr_date(+)>=TO_DATE('" + model.DateFrom.ToString("ddMMMyyyy") + "')  AND TR_DATE(+)<=TO_DATE('" + model.DateTo.ToString("ddMMMyyyy") + "')  and pt.exchange(+)=a.exchange and a.clnt=pt.party_cd(+) and substr(pt.billno(+),1,6)='DLYMTM'  group by TRADE_TIME,CLNT,pAR_NAME, INS_TYP,STK,a.SYMBOL,a.EXPIRYDATE,TRADE_DATE,TR_ST,a.MULTIPLIER,TRN_QTY,TRADEPRICE,NETPRICE  ,a.BROK,PVALUE,SVALUE,OPTIONTYPE,CONTRACT,GROUPCLIENT,COMPANY,ADD1,ADD2,ADD3, ADD4,PHNOS, BRANCHIND,a.EXCHANGE,TRN_SLNO,a.userid ,A.TRADENO,A.ORDERNO ) A,SYSADM.PARTYTRN pt1 where tr_date(+)>=TO_DATE('" + model.DateFrom.ToString("ddMMMyyyy") + "')  AND TR_DATE(+)<=TO_DATE('" + model.DateTo.ToString("ddMMMyyyy") + "') and pt1.exchange(+)=a.exchange and a.clnt=pt1.party_cd(+) and substr(pt1.billno(+),1,6)='DLYEXP'  group by TRADE_TIME,CLNT,pAR_NAME, INS_TYP,STK,a.SYMBOL,a.EXPIRYDATE,TRADE_DATE,TR_ST,a.MULTIPLIER,TRN_QTY,TRADEPRICE,NETPRICE ,a.BROK,PVALUE,SVALUE,OPTIONTYPE,CONTRACT,GROUPCLIENT,COMPANY,ADD1,ADD2,ADD3, ADD4,PHNOS, BRANCHIND,a.EXCHANGE,TRN_SLNO  ,a.stax,a.nsetax,a.stamp,a.stax_turn,a.userid,A.TRADENO,A.ORDERNO  order by a.trade_date,a.ins_typ,a.stk,symbol,expirydate";

                            DataSet ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet(qry, Session["SelectedConn"].ToString());
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
                                bdo.ClientCode = row["ClientCode"].ToString();
                                bdo.ClientName = row["ClientName"].ToString();
                                bdo.ScripCode = row["ScripCode"].ToString();
                                bdo.ScripIsin = row["ScripIsin"].ToString();
                                bdo.ScripName = row["ScripName"].ToString()==null?"" : row["ScripName"].ToString();
                                bdo.TradeDate = DateTime.Parse(row["TradeDate"].ToString());
                                bdo.TradeTime = DateTime.Parse(row["TradeTime"].ToString()) ==null ? DateTime.Now : DateTime.Parse(row["TradeTime"].ToString());
                               // bdo.OrderTime = DateTime.Parse(row["OrderTime"].ToString());
                                bdo.OrderNo = row["OrderNo"].ToString();
                                bdo.TradeNo = row["TradeNo"].ToString();
                                

                             // bdo.Qty = int.Parse(row["Qty"].ToString())==null? "" : int.Parse(row["Qty"].ToString());
                              bdo.TradeRate = decimal.Parse(row["TradeRate"].ToString());
                            bdo.NetRate = decimal.Parse(row["NetRate"].ToString());
                                //bdo.TradeValue = decimal.Parse(row["TradeValue"].ToString());
                             // bdo.Brokrage = decimal.Parse(row["Brokrage"].ToString());

                                bdo.Flag = row["Flag"].ToString();
                                lstOut.listTradeConfOutputRow.Add(bdo);
                            }
                        }
                        Session["ReportHeader1"] = "Trade Confirmation";
                        Session["ReportHeader2"] = "Date Range : " + model.DateFrom.ToString("dd/MM/yyyy") + " - " + model.DateTo.ToString("dd/MM/yyyy");

                        return View(lstOut);
                   
                }
                catch (Exception ex)
                {
                    TempData["AlertMessage"] = ex;
                    return RedirectToAction("Index", "TradeConf");
                }
            }

        }
    }
}