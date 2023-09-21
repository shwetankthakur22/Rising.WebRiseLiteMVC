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

    public class DematController : Controller
    {
        public ActionResult DematHolding(string code)
        {
            List<DBList> selectedDBLists = Session["SelectedDBLists"] as List<DBList>;


            DematHoldingInput model = new DematHoldingInput();
            model.AsOnDate = DateTime.Parse(Session["FinYearTo"].ToString());
            model.ClientCodeFrom = code;
            model.ClientCodeTo = code;
            return View(model);
        }

        public static DematHoldingOutput lstOut;

        public ActionResult DematHoldingReport(string activeMenu, DematHoldingInput model)
        {
            //try
            //{
            WebUser webUser = Session["WebUser"] as WebUser;
            List<Oracle.ManagedDataAccess.Client.OracleParameter> lst = new List<Oracle.ManagedDataAccess.Client.OracleParameter>();

            DataSet ds8 = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet("select * from sysadm.ratefile where rdate<='"+ model.AsOnDate.ToString("ddMMMyyyy") + "' ", Session["SelectedConn"].ToString());
            if(ds8.Tables[0].Rows.Count==0)
            {
                TempData["AlertMessage"] = "Rate not found for selected date";
                return RedirectToAction("DematHolding", "Demat");
            }
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
                    return RedirectToAction("DematHolding", "Demat");

                }

                lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("clientcode_", model.ClientCodeFrom, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
            }
            else if (webUser.UserType == UserType.RM)
            {
                model.ClientCodeTo = model.ClientCodeFrom;
                DataSet ds1 = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet("SELECT * FROM sysadm.partymst where par_code='" + model.ClientCodeFrom + "' and rmcode='" + webUser.UserID + "'", Session["SelectedConn"].ToString());
                if (ds1.Tables[0].Rows.Count == 0)
                {
                    TempData["AlertMessage"] = "Invalid Code : " + model.ClientCodeFrom;
                    return RedirectToAction("DematHolding", "Demat");

                }

                lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("clientcode_", model.ClientCodeFrom, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
            }
            else if (webUser.UserType == UserType.Admin)
            {
                model.ClientCodeTo = model.ClientCodeFrom;
               
                lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("clientcode_", model.ClientCodeFrom, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
            }

            lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("AsOnDate_", model.AsOnDate, Oracle.ManagedDataAccess.Client.OracleDbType.Date, ParameterDirection.Input));
            DataSet ds = MvcApplication.OracleDBHelperCore().CustomHelper.CustomDataSet("SYSADM.N$GET_DematHolding", lst, Session["SelectedConn"].ToString());

            lstOut = new DematHoldingOutput();
            lstOut.listDematHoldingOutputRow = new List<DematHoldingOutputRow>();
            lstOut.ClientCode = webUser.UserID;
            lstOut.ClientName = webUser.UserName;
            lstOut.AsOnDate = model.AsOnDate;

            int c = 0;
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                if (c == 0)
                {
                    lstOut.ClientCode = row["Code"].ToString();
                    lstOut.ClientName = row["Name"].ToString();
                }
                c++;
                DematHoldingOutputRow bdo = new DematHoldingOutputRow();

              
                bdo.ScripCode = row["SCRIPCD"].ToString();
                bdo.ScripName = row["SH_NAME"].ToString();
                bdo.ScripIsin = row["ISINCODE"].ToString();
                bdo.Qty = decimal.Parse(row["Qty"].ToString());
                bdo.Stock = decimal.Parse(row["Stock"].ToString());
                bdo.LockQty = decimal.Parse(row["Qty_lock"].ToString());
                bdo.CDSLQty = decimal.Parse(row["cdsl_qty"].ToString());
                bdo.NSDLQty = decimal.Parse(row["nsdl_qty"].ToString());
                bdo.TotalQty = decimal.Parse(row["tot_qty"].ToString());
                bdo.Rate = decimal.Parse(row["rate"].ToString());
                bdo.Value = decimal.Parse(row["stk_val"].ToString());
                bdo.VarPer = decimal.Parse(row["rate_var"].ToString());
                bdo.VarValue = decimal.Parse(row["varvalue"].ToString());
                lstOut.listDematHoldingOutputRow.Add(bdo);
            }
            Session["ReportHeader1"] = "Demat Holding";
            Session["ReportHeader2"] = "As On Date : " + model.AsOnDate.ToString("dd/MM/yyyy");

            return View(lstOut);

            //}
            //catch (Exception ex)
            //{
            //    return View();
            //}
        }

        public ActionResult ShareTransferRequest()
        {
            WebUser webUser = Session["WebUser"] as WebUser;
            if (webUser.UserType != UserType.Branch)
            {
                return View();
            }
            ShareTransferRequest model = new Models.ShareTransferRequest();
            model.AsOnDate = DateTime.Parse(Session["FinYearTo"].ToString());
            model.TransferDate = DateTime.Parse(Session["FinYearTo"].ToString());
            model.BenCodes = new Dictionary<string, string>();

            DataSet ds1 = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet("select distinct clientcd from SYSADM.dematlgr s   where trxtype='B' and clientcd is not null", Session["SelectedConn"].ToString());
            if (ds1.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow dr in ds1.Tables[0].Rows)
                {
                    model.BenCodes.Add(dr[0].ToString(), "");
                }
            }

            return View(model);
        }

        public ActionResult ShareTransferInput(string activeMenu, ShareTransferRequest model)
        {

            WebUser webUser = Session["WebUser"] as WebUser;
            if (model.AsOnDate > DateTime.Parse("01jan1900"))
            {
                string str = "select D.CLIENTTO ClientCode,p.par_name ClientName,d.scripcd ScripCode,sh_name ScripName,sh.isincode ScripISIN,sum(nvl(d.debit, 0) - NVL(d.credit, 0)) Holding,P.DPCODE DPCode, P.DPACCOUNTNO DPAcNo, d.CLIENTCD BenCode, p1.dpcode BENDPCODE, p1.DPACCOUNTNO BENDPACCOUNTNO     from SYSADM.dematlgr d, SYSADM.sharemst sh, SYSADM.partymst p, sysadm.partymst p1   where sh_code = scripcd  AND TRXTYPE = 'B' and d.clientto = p.par_code  and p1.par_code=d.CLIENTCD    ";


                List<Oracle.ManagedDataAccess.Client.OracleParameter> lst = new List<Oracle.ManagedDataAccess.Client.OracleParameter>();
                if (webUser.UserType == UserType.Client)
                {
                    str = str + "  and  clientto = '" + webUser.UserID + "' ";
                }
                else if (webUser.UserType == UserType.Branch)
                {
                    //if (model.Search[0] == "Client")
                    //{
                    if (model.ClientCodeFrom != null)
                    {
                        DataSet ds2 = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet("SELECT * FROM sysadm.partymst where par_code='" + model.ClientCodeFrom + "' and branchind='" + webUser.UserID + "'", Session["SelectedConn"].ToString());
                        if (ds2.Tables[0].Rows.Count == 0)
                        {
                            TempData["AlertMessage"] = "Invalid Code : " + model.ClientCodeFrom;
                            return RedirectToAction("ShareTransferRequest", "dEMAT");
                        }
                    }
                    str = str + "  and  clientto = '" + model.ClientCodeFrom + "'  and branchind='" + webUser.UserID + "'";
                    //}
                    //else if (model.Search[0] == "RM")
                    //{
                    //    str = str + "  and  clientto = '" + webUser.UserID + "' ";
                    //}
                    //else if (model.Search[0] == "Group")
                    //{
                    //    str = str + "  and  clientto = '" + webUser.UserID + "' ";
                    //}
                    //else if (model.Search[0] == "SubBranch")
                    //{
                    //    str = str + "  and  clientto = '" + webUser.UserID + "' ";
                    //}
                    //else if (model.Search[0] == "Branch")
                    //{
                    //    str = str + "  and  clientto = '" + webUser.UserID + "' ";
                    //}
                }

                str = str + " group by d.scripcd,sh_name,sh.isincode,D.CLIENTTO,p.par_name,P.DPCODE,P.DPACCOUNTNO,D.CLIENTCD, p1.dpcode, p1.DPACCOUNTNO  Having Sum(NVL(d.DEBIT, 0) - NVL(d.CREDIT, 0)) > 0    ";


                DataSet ds1 = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet(str, Session["SelectedConn"].ToString());

                model.listShareTransferOutputRow = new List<ShareTransferOutputRow>();

                if (ds1.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow dr in ds1.Tables[0].Rows)
                    {
                        ShareTransferOutputRow itm = new ShareTransferOutputRow();
                        itm.ClientCode = dr["ClientCode"].ToString();
                        itm.ClientName = dr["ClientName"].ToString();
                        itm.DPCode = dr["DPCode"].ToString();
                        itm.DPAcNo = dr["DPAcNo"].ToString();
                        itm.BenCode = dr["BenCode"].ToString();
                        itm.ScripCode = dr["ScripCode"].ToString();
                        itm.ScripName = dr["ScripName"].ToString();
                        itm.ScripISIN = dr["ScripISIN"].ToString();
                        itm.Holding = decimal.Parse(dr["Holding"].ToString());
                        itm.BenDPCode = dr["BenDPCode"].ToString();
                        itm.BenDPAcNo = dr["BenDPAccountno"].ToString();
                        itm.TransferQty = 0;

                        model.listShareTransferOutputRow.Add(itm);
                    }
                }
            }
            if (model.BenCodes == null) model.BenCodes = new Dictionary<string, string>();
            return View("ShareTransferRequest", model);
        }

        public ActionResult ShareTransferSave(ShareTransferRequest model)
        {
            int i = 0;
            foreach (ShareTransferOutputRow itm in model.listShareTransferOutputRow)
            {
                if (itm.TransferQty > 0)
                {
                    i++;
                    string str = "select SYSADM.sysdbsequence.nextval from dual";
                    DataSet ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet(str, Session["SelectedConn"].ToString());
                    string ctlno = ds.Tables[0].Rows[0][0].ToString();

                    str = "insert into SYSADM.dematlgr(scrollno,ddate,clientcd,scripcd, debit,credit,scrolldate,clientdp,acno,clientto,CONTROLNO, ISINCODE, mktto, TrxType,TRF_REASON,PURPOSE,CONSIDERATION) values('" + i.ToString() + "','" + model.TransferDate.ToString("ddMMMyyyy") + "','" + itm.BenCode + "','" + itm.ScripCode + "',0," + itm.TransferQty + ",'" + model.TransferDate.ToString("ddMMMMyyyy") + "','" + itm.DPCode + "','" + itm.DPAcNo + "','" + itm.ClientCode + "'," + ctlno + ",'" + itm.ScripISIN + "','CC','B',  '','','')";
                    MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteNonQuery(str, Session["SelectedConn"].ToString());

                    str = "insert into SYSADM.dematlgr(scrollno,ddate,clientcd,scripcd, credit,debit, scrolldate,clientdp,acno,clientto,CONTROLNO, ISINCODE, mktto, TrxType,TRF_REASON,PURPOSE,CONSIDERATION) values('" + i.ToString() + "','" + model.TransferDate.ToString("ddMMMyyyy") + "','" + itm.ClientCode + "','" + itm.ScripCode + "',0," + itm.TransferQty + ",'" + model.TransferDate.ToString("ddMMMMyyyy") + "','','',null," + ctlno + ",'" + itm.ScripISIN + "','CC','C',  '','','')";
                    MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteNonQuery(str, Session["SelectedConn"].ToString());
                }
            }
            return View();
        }
    }
}