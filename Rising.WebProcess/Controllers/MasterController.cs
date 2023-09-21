using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Web.Mvc;
using System.Diagnostics;
using System.Management;
using Rising.WebRise.Models;
using System.Data;
using System.Configuration;

namespace Rising.WebRise.Controllers
{
    public class MasterController : Controller
    {
        string dbuser = ConfigurationManager.AppSettings["DBUSER"];
        // GET: Process
        public static List<SettlementSchedule> Settlementbind = new List<SettlementSchedule>();

        public static List<SettlementSchedule> SettlementEntry = new List<SettlementSchedule>();

        public static List<ScripMaster> Scripnameentry = new List<ScripMaster>();

        //------------------------User Maintenance Start---------------------------------
        public ActionResult UserIDMaintenance(TradeUserIds model)
        {

            if (model.BranchCode == null)
            {
                DataSet ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet("SELECT  branchcode,clientid,EXCHANGE,proclient,brokercode,ctclid,prefix FROM " + dbuser + ".CUBRANCHFILE ", Session["SelectedConn"].ToString()); model.result = ds;
                return View(model);
            }
            else
            {
                DataSet ds1 = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet("SELECT  branchcode,clientid,EXCHANGE,proclient,brokercode,ctclid,prefix FROM " + dbuser + ".CUBRANCHFILE where clientid='" + model.ClientCodeFrom + "' ", Session["SelectedConn"].ToString());
                model.result = ds1;
                if (ds1.Tables[0].Rows.Count == 0)
                {
                    MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteNonQuery("insert into " + dbuser + ".CUBRANCHFILE(branchcode,clientid,EXCHANGE,proclient,brokercode,ctclid,prefix)   SELECT '" + model.BranchCode + "' , '" + model.ClientCodeFrom + "' ,'" + model.Exchange + "' ,'" + model.Proclient + "' ,'" + model.BrokerCode + "' ,'" + model.CTCLID + "' ,'" + model.Prefix + "' FROM DUAL", Session["SelectedConn"].ToString());
                    TempData["AlertMessage"] = "Data Saved Successfully";
                }
                else
                {
                    TempData["AlertMessage"] = "Data Already Found";
                    RedirectToAction("UserIDMaintenance", "Master", model);
                }
            }

            return View(model);
        }
        public ActionResult UserIDMaintenanceedit(TradeUserIds model, string submit)
        {

            switch (submit)
            {
                case "Delete":
                    return (UserIDdelete(model));
                case "Update":
                    return (UserIDUpdate(model));
            }


            return View(model);
        }

        public ActionResult UserIDdelete(TradeUserIds model)
        {
            MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteNonQuery("delete from  " + dbuser + ".CUBRANCHFILE where ClientId='" + model.ClientCodeFrom + "' and  BRANCHCODE='" + model.BranchCode + "'", Session["SelectedConn"].ToString());
            ModelState.Clear();
            TempData["AlertMessage"] = "Data Deleted Successfully";
            return Json(true, JsonRequestBehavior.AllowGet);
        }


        public ActionResult UserIDUpdate(TradeUserIds model)
        {

            MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteNonQuery("update " + dbuser + ".CUBRANCHFILE set branchcode='" + model.BranchCode + "',EXCHANGE='" + model.Exchange + "',proclient='" + model.Proclient + "',brokercode='" + model.BrokerCode + "',ctclid='" + model.CTCLID + "',prefix='" + model.Prefix + "' where clientid='" + model.ClientCodeFrom + "'", Session["SelectedConn"].ToString());
            ModelState.Clear();
            TempData["AlertMessage"] = "Data Updated Successfully";
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public ActionResult UserIDGetdata(string id)
        {
            TradeUserIds user = new TradeUserIds();

            DataSet ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet("SELECT  branchcode,clientid,EXCHANGE,proclient,brokercode,ctclid,prefix FROM " + dbuser + ".CUBRANCHFILE where clientid ='" + id + "' ", Session["SelectedConn"].ToString());
            if (ds.Tables[0].Rows.Count != 0)
            {

                user.BranchCode = ds.Tables[0].Rows[0]["BRANCHCODE"].ToString();
                user.Exchange = ds.Tables[0].Rows[0]["EXCHANGE"].ToString();
                user.BrokerCode = ds.Tables[0].Rows[0]["BROKERCODE"].ToString();
                user.CTCLID = ds.Tables[0].Rows[0]["CTCLID"].ToString();
                user.Proclient = ds.Tables[0].Rows[0]["PROCLIENT"].ToString();
                user.Prefix = ds.Tables[0].Rows[0]["PREFIX"].ToString();
                user.ClientCodeFrom = ds.Tables[0].Rows[0]["CLIENTID"].ToString();


            }



            return Json(user, JsonRequestBehavior.AllowGet);


        }

        //------------------------User Maintenance End---------------------------------



        //------------------------Bank Master Start---------------------------------

        [HttpGet]
        public ActionResult BankMaster()
        {
            return View();
        }

        [HttpPost]
        public ActionResult BankMaster(BankMaster model)
        {
            DataSet ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet("SELECT * FROM  " + dbuser + ".BANKMASTER WHERE BANKCODE='" + model.BankCode + "'", Session["SelectedConn"].ToString());
            model.result = ds;
            if (ds.Tables[0].Rows.Count == 0)
            {
                MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteNonQuery("INSERT INTO " + dbuser + ".BANKMASTER (BANKCODE,BANKNAME,MICR) VALUES('" + model.BankCode + "','" + model.BankName + "','" + model.Micr + "')", Session["SelectedConn"].ToString());
                ModelState.Clear();
                TempData["AlertMessage"] = "Data Saved Successfully";
                return RedirectToAction("BankMaster", "Master", model);
            }
            else
            {
                TempData["AlertMessage"] = "Data Not Found";
                ModelState.Clear();
                RedirectToAction("BankMaster", "Master", model);
            }

            return View(model);
        }

        //------------------------Bank Master End---------------------------------


        //------------------------State Master Start---------------------------------

        [HttpGet]
        public ActionResult StateMaster()
        {
            return View();
        }

        [HttpPost]
        public ActionResult StateMaster(StateMaster model)
        {
            DataSet ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet("SELECT * FROM  " + dbuser + ".STATEMST WHERE CODE='" + model.StateCode + "'", Session["SelectedConn"].ToString());
            model.result = ds;
            if (ds.Tables[0].Rows.Count == 0)
            {
                MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteNonQuery("INSERT INTO " + dbuser + ".STATEMST (CODE,NAME) VALUES('" + model.StateCode + "','" + model.StateName + "')", Session["SelectedConn"].ToString());
                TempData["AlertMessage"] = "Data Saved Successfully";              
                return RedirectToAction("StateMaster", "Master", model);
            }
            else
            {
                TempData["AlertMessage"] = "Data Already Found";
                ModelState.Clear();
                RedirectToAction("StateMaster", "Master", model);
            }

            return View(model);
        }

        //------------------------State Master End-----------------------------------------

        //------------------------Branch Maintenance Start---------------------------------

        public ActionResult BranchMaintenance(BranchMaintenance model)
        {
            return View(model);
        }

        //------------------------Branch Maintenance End------------------------------------


        //------------------------Contract Specification Start---------------------------------
        [HttpGet]
        public ActionResult ContractSpecification()
        {
            if (Session["WebUser"] == null)
            {
                TempData["AlertMessage"] = "Session Time Out Please Login Again";
                return RedirectToAction("Index", "Login");

            }
            else
            {

                ContractSpecification model = new ContractSpecification();
      
                model.ExpiryDate = DateTime.Parse(Session["FinYearTo"].ToString());
               
                return View(model);


            }
        }

        [HttpPost]
        public ActionResult ContractSpecification(ContractSpecification model)
        {
            string contname = model.InstrumentType + model.Symbol + model.ExpiryDate;
            MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteNonQuery("INSERT INTO " + dbuser + ".CUCONTRACTS(INSTRUMENT_TYPE, SYMBOL, EXPIRYDATE, CONTNAME, STRIKEPRICE, MKTLOT, EXCHANGE, COMULTIPLIER) VALUES('" + model.InstrumentType + "', '" + model.Symbol + "', '" + model.ExpiryDate + "', '" + contname + "', .00, '" + model.LotSize + "', '" + model.Exchange + "', 1.000000000)", Session["SelectedConn"].ToString());
            ModelState.Clear();

            TempData["AlertMessage"] = "Data Saved Sucessfully";
            return View(model);

        }

      

        //------------------------Contract Specification End-----------------------------------


        //------------------------Client Master Start---------------------------------
        [HttpGet]
        public ActionResult ClientMaster()
        {
            return View();
        }


        //------------------------Client Master End-----------------------------------

        //------------------------Exchange Master Start---------------------------------

        public ActionResult ExchangeMaster()
        {

            return View();
        }
        //------------------------Exchange Master End---------------------------------



        //------------------------Bank Account Details Start---------------------------------


        public ActionResult BankAccountDetails()
        {

            return View();
        }
       
        
        //------------------------Bank Account Details End---------------------------------

        public ActionResult TMMaster()
        {

            return View();
        }

        //------------------------Holiday Master---------------------------------


        public ActionResult HolidayMaster(HolidayMaster model)
        {

            if (model.Holiday == null)
            {
                DataSet ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet("SELECT  to_char( hdate,'dd-mm-yyyy') hdate, reason, exchange FROM " + dbuser + ".Holiday ", Session["SelectedConn"].ToString()); model.result = ds;
                return View(model);
            }
            else
            {
                DataSet ds1 = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet("SELECT  to_char( hdate,'dd-mm-yyyy') hdate, reason, exchange FROM " + dbuser + ".Holiday  where REASON = '" + model.Holiday + "' And exchange='" + model.Exchange + "'  ", Session["SelectedConn"].ToString());
                model.result = ds1;
                if (ds1.Tables[0].Rows.Count != 0)
                {
                    MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteNonQuery("insert into " + dbuser + ".Holiday(hdate,reason,exchange)   SELECT '" + model.StartDate + "' , '" + model.Holiday + "' ,'" + model.Exchange + "'  FROM DUAL", Session["SelectedConn"].ToString());
                    TempData["AlertMessage"] = "Data Saved Successfully";
                    // ScriptManager.RegisterStartupScript(this, GetType(), "Popup", "successalert();", true);
                    // ScriptManager.RegisterStartupScript(this, typeof(Page), "MyScript", "alert('Hello World!');", true);


                }
                else
                {
                    TempData["AlertMessage"] = "Data Already Found";
                    RedirectToAction("HolidayMaster", "Master", model);
                }
            }

            return View(model);
        }

        public ActionResult Holidayedit(HolidayMaster model, string submit)
        {

            switch (submit)
            {
                case "Delete":
                    return (Holidaydelete(model));
                case "Update":
                    return (HolidayUpdate(model));
            }


            return View(model);
        }


        public ActionResult Holidaydelete(HolidayMaster model)
        {
            MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteNonQuery("delete from  " + dbuser + ".Holiday where REASON='" + model.Holiday + "' and exchange='" + model.Exchange + "'", Session["SelectedConn"].ToString());

            TempData["AlertMessage"] = "Data Deleted Successfully";
            return Json(true, JsonRequestBehavior.AllowGet);
        }


        public ActionResult HolidayUpdate(HolidayMaster model)
        {

            MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteNonQuery("update  " + dbuser + ".Holiday set  REASON='" + model.Holiday + "' ,hdate='" + model.StartDate + "'  where REASON='" + model.Holiday + "' and exchange='" + model.Exchange + "'", Session["SelectedConn"].ToString());
            TempData["AlertMessage"] = "Data Updated Successfully";
            return Json(true, JsonRequestBehavior.AllowGet);
        }

        public ActionResult HolidayGetdata(string id)
        {
            HolidayMaster holiday = new HolidayMaster();

            DataSet ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet("SELECT  to_char( hdate,'dd-mm-yyyy') hdate ,reason, exchange FROM " + dbuser + ".Holiday where REASON ='" + id + "' ", Session["SelectedConn"].ToString());
            if (ds.Tables[0].Rows.Count != 0)
            {
                holiday.Holiday = ds.Tables[0].Rows[0]["reason"].ToString();
                holiday.Exchange = ds.Tables[0].Rows[0]["exchange"].ToString();
                holiday.Sdate = ds.Tables[0].Rows[0]["hdate"].ToString();
            }
            return Json(holiday, JsonRequestBehavior.AllowGet);
        }


        //------------------------Holiday Master End---------------------------------


              //Settlement Master
        [HttpGet]
        public ActionResult SettlementMaster()
        {
            SettlementSchedule objuser = new SettlementSchedule();
            DataSet ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet("SELECT STN_NAME,STN_code FROM SYSADM.STNMAST ", Session["SelectedConn"].ToString());


            Dictionary<string, string> stat = new Dictionary<string, string>();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                stat[ds.Tables[0].Rows[i]["STN_code"].ToString()] = ds.Tables[0].Rows[i]["STN_NAME"].ToString();
            }
            ViewBag.States = stat;
            return View();
        }

        [HttpPost]
        public ActionResult SettlementMaster(SettlementSchedule model)
        {
            SettlementSchedule objuser = new SettlementSchedule();
            DataSet ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet("SELECT STN_NAME,STN_code FROM SYSADM.STNMAST ", Session["SelectedConn"].ToString());


            Dictionary<string, string> stat = new Dictionary<string, string>();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                stat[ds.Tables[0].Rows[i]["STN_code"].ToString()] = ds.Tables[0].Rows[i]["STN_NAME"].ToString();
            }
            ViewBag.States = stat;


            return View(model);
        }

        //settlement Entry/Edit

        [HttpGet]
        public ActionResult SettlementTypeEntry()
        {
            SettlementSchedule model = new SettlementSchedule();
            DataSet ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet("SELECT STATION,SETTTYPE,SETTDES,TOGAP,PAYINGAP,PAYOUTGAP,rowid ID FROM SYSADM.settletype", Session["SelectedConn"].ToString());
            model.result = ds;
            return View(model);

        }

        [HttpPost]
        public ActionResult SettlementTypeEntry(SettlementSchedule model)
        {
            DataSet ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet("SELECT STATION,SETTTYPE,SETTDES,TOGAP,PAYINGAP,PAYOUTGAP,delvbrokapplied,rowid ID FROM SYSADM.settletype  where setttype= '" + model.SettType + "' and station='" + model.StationName + "'", Session["SelectedConn"].ToString());
            model.result = ds;

            if (ds.Tables[0].Rows.Count != 0)
            {
                MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteNonQuery("Update sysadm.settletype set SETTTYPE= '" + model.SettType + "', SETTDES = '" + model.DescOfSettlement + "',TOGAP = '" + model.SettlementPeriodGap + "',PAYINGAP = '" + model.PayinGap + "',PAYOUTGAP = '" + model.PayoutGap + "' ,delvbrokapplied= '" + model.BrokrageDebitNote + "' where setttype= '" + model.SettType + "' and station='" + model.StationName + "'", Session["SelectedConn"].ToString());

            }
            else
            {
                MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteNonQuery("insert into  sysadm.Settletype (SETTTYPE,SETTDES,TOGAP,PAYINGAP,PAYOUTGAP,delvbrokapplied) select '" + model.SettType + "', '" + model.DescOfSettlement + "' ,'" + model.SettlementPeriodGap + "','" + model.PayinGap + "','" + model.PayoutGap + "','" + model.BrokrageDebitNote + "' FROM DUAL", Session["SelectedConn"].ToString());
            }

            ViewData["error"] = "Data Update successfully";
            RedirectToAction("SettlementTypeEntry", "Master");
            return View(model);

        }

        public ActionResult ScheduleSelect(string rid)
        {
            try
            {
                SettlementSchedule stype = new SettlementSchedule();
                SettlementEntry = new List<SettlementSchedule>();
                DataSet ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet("SELECT STATION,SETTTYPE,SETTDES,TOGAP,PAYINGAP,PAYOUTGAP,delvbrokapplied FROM SYSADM.settletype where rowID='" + rid + "'", Session["SelectedConn"].ToString());
                if (ds.Tables[0].Rows.Count != 0)
                {
                    stype.SettType = ds.Tables[0].Rows[0]["SETTTYPE"].ToString();
                    stype.PayoutGap = ds.Tables[0].Rows[0]["PAYOUTGAP"].ToString();
                    stype.DescOfSettlement = ds.Tables[0].Rows[0]["SETTDES"].ToString();
                    stype.SettlementPeriodGap = ds.Tables[0].Rows[0]["TOGAP"].ToString();
                    stype.PayinGap = ds.Tables[0].Rows[0]["PAYINGAP"].ToString();
                    stype.StationName = ds.Tables[0].Rows[0]["STATION"].ToString();
                    stype.BrokrageDebitNote = ds.Tables[0].Rows[0]["delvbrokapplied"].ToString();

                    SettlementEntry.Add(stype);
                }
                return Json(stype, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

            }

            return View();
        }

        [HttpGet]
        public ActionResult SettlementMasterImport()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SettlementMasterImport(SettlementSchedule model, HttpPostedFileBase TradeFile)
        {
            try
            {

                string filename = TradeFile.FileName;
                var fileName = System.IO.Path.GetFileName(filename);
                var extn = System.IO.Path.GetExtension(filename);
                string newfileName = "e:\\app\\" + Guid.NewGuid().ToString() + extn;
                TradeFile.SaveAs(newfileName);

                string dbname = MvcApplication.OracleDBHelperCore().OracleDBManager.getDBName("MainConn");
                MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteNonQuery("Truncate table  sysadm.settfile_temp", Session["SelectedConn"].ToString());
                bool status = MvcApplication.OracleDBHelperCore().LoaderHelper.runLoader("NSE2005", "sysadm.settfile_temp", newfileName, " Settlement_Type,Settlement_No,Trade_Start_Date,Trade_End_Date,Funds_Payin_Date,Funds_Payout_Date,Security_Payin_Date,Security_Payout_Date,Final_Obligation_Date,Settlement_Merge_Number,Settlement_active,Settlement_Special");
                MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteNonQuery("insert into  sysadm.settfile (SETTNO,NSESETTNO,PERIODFROM,PERIODTO,payindate,payoutdate) select  'N'||SETTLEMENT_TYPE||SUBSTR(SettlemenT_NO,4,4),Settlement_No,Trade_Start_Date,Trade_End_Date,Funds_Payin_Date ,Funds_Payout_Date from sysadm.settfile_temp", Session["SelectedConn"].ToString());
                MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteNonQuery("Truncate table  sysadm.settfile_temp", Session["SelectedConn"].ToString());
                // ViewBag.Message = "Record Import Successfully!";

            }
            catch (Exception ex)
            {
            }
            return RedirectToAction("SettlementMaster", "Master", model);
        }

        public ActionResult SettlementSechduleSave(SettlementSchedule model)
        {
            SettlementSchedule objuser = new SettlementSchedule();
            DataSet ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet("SELECT STN_NAME,STN_code FROM SYSADM.STNMAST ", Session["SelectedConn"].ToString());


            Dictionary<string, string> stat = new Dictionary<string, string>();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                stat[ds.Tables[0].Rows[i]["STN_code"].ToString()] = ds.Tables[0].Rows[i]["STN_NAME"].ToString();
            }
            ViewBag.States = stat;

            string set = model.StationName.Substring(0, 1) + model.SettType.Substring(0, 1) + model.SettNo;
            MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteNonQuery("insert into SYSADM.SETTFILE (SETTNO,NSESETTNO,PERIODFROM,PERIODTO,PAYINDATE,PAYOUTDATE,DELVINDATE,DELVOUTDATE) SELECT '" + set + "','" + model.ExchSett + "', '" + model.PeriodFrom.ToString() + "', '" + model.PeriodTo.ToString() + "' ,'" + model.PayinDate.ToString() + "','" + model.PayoutDate.ToString() + "','" + model.PayinDate.ToString() + "','" + model.PayoutDate.ToString() + "','" + model.DelinDate.ToString() + "','" + model.DeloutDate.ToString() + "' FROM DUAL", Session["SelectedConn"].ToString());
            ModelState.Clear();
            return View();
        }

        //for 2nd dropdown binding
        public ActionResult SettlementSechdulelist(string stncode)
        {
            DataSet ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet("SELECT SETTTYPE,SETTDES FROM SYSADM.SETTLETYPE where STATION='" + stncode + "'", Session["SelectedConn"].ToString());
            Dictionary<string, string> stat = new Dictionary<string, string>();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                stat[ds.Tables[0].Rows[i]["SETTTYPE"].ToString()] = ds.Tables[0].Rows[i]["SETTDES"].ToString();
            }
            // ViewBag.States = stat;
            return Json(stat, JsonRequestBehavior.AllowGet);


        }



        public ActionResult GetSettData(string Exchcode)
        {

            try
            {

                SettlementSchedule bnk = new SettlementSchedule();
                if (bnk == null)
                {
                    TempData["AlertMessage"] = TempData["AlertMessage"] + " " + "Code Not Found";
                    return RedirectToAction("SettlementMaster", "Master");
                }

                Settlementbind = new List<SettlementSchedule>();
                //DataSet ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet("select Settno, to_char(Periodfrom,'dd-mm-yyyy') Periodfrom,to_char(periodto,'dd-mm-yyyy')periodto,to_char(payindate,'dd-mm-yyyy')payindate,to_char(payoutdate,'dd-mm-yyyy' )payoutdate,to_char(DELVINDATE,'dd-mm-yyyy')DELVINDATE,to_char(DELVOUTDATE,'dd-mm-yyyy')DELVOUTDATE,Nsesettno from sysadm.settfile   where SUBSTR(SETTNO,3,4)='" + Exchcode + "'", Session["SelectedConn"].ToString());
                DataSet ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet("select Settno, to_char(Periodfrom,'dd-mm-yyyy') Periodfrom,to_char(periodto,'dd-mm-yyyy')periodto,to_char(payindate,'dd-mm-yyyy')payindate,to_char(payoutdate,'dd-mm-yyyy' )payoutdate,to_char(DELVINDATE,'dd-mm-yyyy')DELVINDATE,to_char(DELVOUTDATE,'dd-mm-yyyy')DELVOUTDATE,Nsesettno from sysadm.settfile   where SETTNO='" + Exchcode + "'", Session["SelectedConn"].ToString());
                if (ds.Tables[0].Rows.Count != 0)
                {


                    //bnk.PeriodFrom = ds.Tables[0].Rows[0]["Periodfrom"].ToString();

                    //bnk.PeriodTo = Convert.ToDateTime(ds.Tables[0].Rows[0]["periodto"].ToString();
                    //bnk.PayinDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["payindate"].ToString());
                    //bnk.PayoutDate = Convert.ToDateTime(ds.Tables[0].Rows[0]["payoutdate"].ToString());
                    bnk.SettNo = ds.Tables[0].Rows[0]["SettNo"].ToString();
                    bnk.PeriodFrom = ds.Tables[0].Rows[0]["Periodfrom"].ToString();
                    bnk.PeriodTo = ds.Tables[0].Rows[0]["periodto"].ToString();
                    bnk.PayinDate = ds.Tables[0].Rows[0]["payindate"].ToString();
                    bnk.PayoutDate = ds.Tables[0].Rows[0]["PayoutDate"].ToString();
                    bnk.DelinDate = ds.Tables[0].Rows[0]["DELVINDATE"].ToString();
                    bnk.DeloutDate = ds.Tables[0].Rows[0]["DELVOUTDATE"].ToString();
                    bnk.ExchSett = ds.Tables[0].Rows[0]["Nsesettno"].ToString();


                    Settlementbind.Add(bnk);
                }
                else
                {

                    bnk.SettNo = Exchcode;
                }


                return Json(bnk, JsonRequestBehavior.AllowGet);

            }
            catch (Exception ex)
            {


            }
            return View();


        }


        //Scrip Master


        [HttpGet]
        public ActionResult ScripMaster()
        {
            ScripMaster model = new ScripMaster();
            DataSet ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet("select sh_NAME,sh_code ,sh_symbol ,sh_series , isincode,to_char(sh_nodfrom,'dd-mm-yyyy')NODFROM , to_char(sh_nodto,'dd-mm-yyyy')NODTO ,sh_settno ,SH_REMARK,SH_LOTSIZE ,SH_FACEVAL,CUSTOM_VAR,SH_MARGSALE, SH_MARGBUY,GROUPCD,DEPOSITORY,DEMATSTOCK,Pledgeable,STTCAL,TRX_TYPE,ILIQUID,DPPREFER,MTF_MOVE_ALLOW,BONDSCRIP,FUNDFLAG,IGNORE_TRNX,EQUITY_ORIENTED,DL_TRADE,RowID ID from SYSADM.sharemst", Session["SelectedConn"].ToString());
            model.Scripdata = ds;
            return View(model);
        }

        [HttpPost]
        public ActionResult ScripMaster(ScripMaster model)
        {
            DataSet ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet("select  sh_NAME,sh_code ,sh_symbol ,sh_series , isincode,to_char(sh_nodfrom,'dd-mm-yyyy')NODFROM , to_char(sh_nodto,'dd-mm-yyyy')NODTO ,sh_settno ,SH_REMARK,SH_LOTSIZE ,SH_FACEVAL,SH_MARGSALE, SH_MARGBUY,GROUPCD,DEPOSITORY,DEMATSTOCK,PLEDGEABLE,STTCAL,TRX_TYPE,ILIQUID,DPPREFER,MTF_MOVE_ALLOW,BONDSCRIP,FUNDFLAG,IGNORE_TRNX,EQUITY_ORIENTED,DL_TRADE,RowID ID from SYSADM.sharemst where Rowid= '" + model.RowId + "'", Session["SelectedConn"].ToString());
            model.Scripdata = ds;
            model.ScripOpenDate = System.DateTime.Now;
            if (ds.Tables[0].Rows.Count == 0)
            {

                MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteNonQuery("INSERT INTO SYSADM.SHAREMST(SH_CODE, SH_NAME, SH_SYMBOL, SH_SERIES, SH_FACEVAL, SH_LOTSIZE," +
                                                                                " SH_MARGSALE, SH_MARGBUY, SH_REMARK, CLOS_STOCK, GROUPCD, DEPOSITORY," +
                                                                                " DEMATSTOCK, ISINCODE, GETIMES, VARIANCE, SH_SHORTNM, PLEDGEABLE, STTCAL, SCRIPOPENDATE, ILIQUID, BONDSCRIP, ISIN_OLD," +
                                                                                " IGNORE_TRNX, TRNX_STARTDATE, DL_TRADE, DPPREFER, CUSTOM_VAR, SHARE_TYPE, TRX_TYPE,EQUITY_ORIENTED,FUNDFLAG) " +
                                                                                " SELECT '" + model.ScripCode + "', '" + model.ScripName + "','" + model.Symbol + "', '" + model.Series + "', '" + model.FaceValue + "', '" + model.LotSize + "', '" + model.MarginSale + "', '" + model.MarginBuy + "', " +
                                                                                " '" + model.Remark + "', '" + model.ClosingStockPhy + "', '" + model.GroupCD + "', '" + model.DematScrip + "', '" + model.ClosingStockDemat + "', '" + model.ISINCode + "', '" + model.GrossExposure + "', '" + model.Variance + "', '" + model.ShortName + "', '" + model.Pledgeable + "', '" + model.STTCalculate + "','" + model.ScripOpenDate + "', " +
                                                                                " '" + model.Iliquid + "', '" + model.BondScrip + "', '" + model.ISINCodeOld + "', '" + model.IgnoreTrnxTax + "', '" + model.StartDate + "', '" + model.DelTradeFO + "', '" + model.DPPreference + "', '" + model.CustomVariance + "', '" + model.ShareType + "', '" + model.TrnxScripType + "','" + model.EquityOrientedFund + "','" + model.FundFlag + "' FROM DUAL", Session["SelectedConn"].ToString());
            }
            else
            {
                MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteNonQuery("UPDATE SYSADM.SHAREMST SET SH_CODE = '" + model.ScripCode + "', SH_NAME = '" + model.ScripName + "', SH_SYMBOL = '" + model.Symbol + "', SH_SERIES = '" + model.Series + "', " +
                    " SH_FACEVAL = '" + model.FaceValue + "', SH_LOTSIZE = '" + model.LotSize + "', SH_MARGSALE = '" + model.MarginSale + "', SH_MARGBUY = '" + model.MarginBuy + "', " +
                    " SH_REMARK = '" + model.Remark + "', CLOS_STOCK = '" + model.ClosingStockPhy + "', GROUPCD = '" + model.GroupCD + "'," +
                    " DEPOSITORY = '" + model.DematScrip + "', DEMATSTOCK = '" + model.ClosingStockDemat + "', ISINCODE = '" + model.ISINCode + "', GETIMES = '" + model.GrossExposure + "', VARIANCE = '" + model.Variance + "', SH_SHORTNM = '" + model.ShortName + "', STTCAL = '" + model.STTCalculate + "'," +
                    " PLEDGEABLE = '" + model.Pledgeable + "', SCRIPOPENDATE = '', ILIQUID = '" + model.Iliquid + "', BONDSCRIP = '" + model.BondScrip + "', ISIN_OLD = '" + model.ISINCodeOld + "', SH_STTPER = '', FUNDFLAG = '" + model.FundFlag + "', EQUITY_ORIENTED = '" + model.EquityOrientedFund + "'," +
                    " IGNORE_TRNX = '" + model.IgnoreTrnxTax + "', DL_TRADE = '" + model.DelTradeFO + "', DPPREFER = '" + model.DPPreference + "', MTF_MOVE_ALLOW = '" + model.MTFMovement + "', CUSTOM_VAR = '" + model.CustomVariance + "', SHARE_TYPE = '" + model.ShareType + "', TRX_TYPE = '" + model.TrnxScripType + "' " +
                    " where Rowid= '" + model.RowId + "'", Session["SelectedConn"].ToString());
            }

            //MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteNonQuery("INSERT INTO SYSADM.SHAREMST_LOG(SH_CODE, SH_NAME, SH_SYMBOL, SH_SERIES, SH_FACEVAL, SH_LOTSIZE," +
            //                                                                  " SH_OPSTOCK, SH_MARGSALE, SH_MARGBUY, SH_REMARK, CLOS_STOCK, GROUPCD, DEPOSITORY," +
            //                                                                  " DEMATSTOCK, ISINCODE, GETIMES, VARIANCE, SH_SHORTNM, PLEDGEABLE, STTCAL, SCRIPOPENDATE, ILIQUID, BONDSCRIP, ISIN_OLD," +
            //                                                                  " STATUS, CHNGDATE, CHNGTIME, LOGIN_USERID, MACHINEIP, IGNORE_TRNX, TRNX_STARTDATE, DL_TRADE, DPPREFER, CUSTOM_VAR, SHARE_TYPE, TRX_TYPE) " +
            //                                                                  " Select '" + model.ScripCode + "', '" + model.ScripName + "', '" + model.Symbol + "', '" + model.Series + "', '" + model.FaceValue + "', '" + model.LotSize + "', '" + model.ClosingStockDemat + "', '" + model.MarginSale + "', '" + model.MarginBuy + "', '" + model.Remark + "', " +
            //                                                                  " '" + model.ClosingStockPhy + "', '" + model.GroupCD + "', '" + model.DematScrip + "', '" + model.ClosingStockDemat + "', '" + model.ISINCode + "', '" + model.ClosingStockPhy + "', '" + model.Variance + "', '" + model.ShortName + "', '" + model.Pledgeable + "', '" + model.STTCalculate + "', '" + model.ScripOpenDate + "', " +
            //                                                                  " '" + model.Iliquid + "', '" + model.BondScrip + "', '" + model.ISINCodeOld + "', 'Y', '" + model.ScripOpenDate + "', '" + model.ScripOpenDate + "', '" + model.LoginId + "', '" + model.MachineIp + "', '" + model.IgnoreTrnxTax + "', '" + model.StartDate + "', '" + model.DelTradeFO + "', '" + model.DPPreference + "', '" + model.CustomVariance + "', '" + model.ShareType + "', '" + model.TrnxScripType + "' FROM DUAL", Session["SelectedConn"].ToString());

            return RedirectToAction("ScripMaster", "Master");
            //return View(model);
        }

        public ActionResult ScripMasterSelect(string rid)
        {
            try
            {
                ScripMaster sm = new ScripMaster();
                Scripnameentry = new List<ScripMaster>();
                DataSet ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet("select sh_NAME ScripName,sh_code ScripCode,sh_symbol Symbol,sh_series Series, isincode,sh_nodfrom NODFROM, sh_nodto NODTO,sh_settno settno,SH_REMARK,SH_LOTSIZE,VARIANCE,CUSTOM_VAR,SH_MARGSALE, SH_MARGBUY,GROUPCD,DEPOSITORY,DEMATSTOCK,Pledgeable,STTCAL,TRX_TYPE,ILIQUID,DPPREFER,MTF_MOVE_ALLOW,BONDSCRIP,FUNDFLAG,IGNORE_TRNX,EQUITY_ORIENTED,DL_TRADE,rowID ID from SYSADM.sharemst  where rowID='" + rid + "'", Session["SelectedConn"].ToString());
                if (ds.Tables[0].Rows.Count != 0)
                {
                    //SelectList lst = new SelectList(Enum.GetValues(typeof(enumPledgeable)).Cast<enumPledgeable>().Select(v => v.ToString()).ToList());
                    //ViewBag.enumPledgeable = lst;

                    sm.ScripName = ds.Tables[0].Rows[0]["ScripName"].ToString();
                    sm.ScripCode = ds.Tables[0].Rows[0]["ScripCode"].ToString();
                    sm.ISINCode = ds.Tables[0].Rows[0]["isincode"].ToString();
                    sm.Remark = ds.Tables[0].Rows[0]["SH_REMARK"].ToString();
                    sm.LotSize = ds.Tables[0].Rows[0]["SH_LOTSIZE"].ToString();
                    sm.Variance = ds.Tables[0].Rows[0]["VARIANCE"].ToString();
                    sm.CustomVariance = ds.Tables[0].Rows[0]["CUSTOM_VAR"].ToString();
                    sm.MarginSale = ds.Tables[0].Rows[0]["SH_MARGSALE"].ToString();
                    sm.MarginBuy = ds.Tables[0].Rows[0]["SH_MARGBUY"].ToString();
                    sm.GroupCD = ds.Tables[0].Rows[0]["GROUPCD"].ToString();
                    sm.ClosingStockDemat = ds.Tables[0].Rows[0]["DEMATSTOCK"].ToString();
                    sm.DematScrip = ds.Tables[0].Rows[0]["DEPOSITORY"].ToString();
                    sm.Pledgeable = ds.Tables[0].Rows[0]["PLEDGEABLE"].ToString();
                    sm.STTCalculate = ds.Tables[0].Rows[0]["STTCAL"].ToString();
                    sm.TrnxScripType = ds.Tables[0].Rows[0]["TRX_TYPE"].ToString();
                    sm.Iliquid = ds.Tables[0].Rows[0]["ILIQUID"].ToString();
                    sm.DPPreference = ds.Tables[0].Rows[0]["DPPREFER"].ToString();
                    sm.MTFMovement = ds.Tables[0].Rows[0]["MTF_MOVE_ALLOW"].ToString();
                    sm.BondScrip = ds.Tables[0].Rows[0]["BONDSCRIP"].ToString();
                    sm.FundFlag = ds.Tables[0].Rows[0]["FUNDFLAG"].ToString();
                    sm.IgnoreTrnxTax = ds.Tables[0].Rows[0]["IGNORE_TRNX"].ToString();
                    sm.EquityOrientedFund = ds.Tables[0].Rows[0]["EQUITY_ORIENTED"].ToString();
                    sm.DelTradeFO = ds.Tables[0].Rows[0]["DL_TRADE"].ToString();
                    sm.RowId = ds.Tables[0].Rows[0]["ID"].ToString();

                    Scripnameentry.Add(sm);
                }
                return Json(sm, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {

            }

            return View();
        }

    }
}