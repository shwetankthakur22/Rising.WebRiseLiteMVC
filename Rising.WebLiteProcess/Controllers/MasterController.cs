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
using Oracle.ManagedDataAccess.Client;
using Rising.WebRise.Models.Masters;
using Rising.OracleDBHelper;

namespace Rising.WebRise.Controllers
{
    public class MasterController : Controller
    {
        string dbuser = ConfigurationManager.AppSettings["DBUSER"];
        // GET: Process
        public static List<SettlementSchedule> Settlementbind = new List<SettlementSchedule>();
        public static List<AccountHeadDetail> bdetails = new List<AccountHeadDetail>();
        public static List<SettlementSchedule> SettlementEntry = new List<SettlementSchedule>();
        public static List<ContractSpecification> Contract = new List<ContractSpecification>();
        public static List<BankMaster> BankDetails = new List<BankMaster>();
        public static List<AccountMaster> AccountDetails = new List<AccountMaster>();
        public static List<StateMaster> StateDetails = new List<StateMaster>();
        public static List<BranchMaintenance> BranchDetails = new List<BranchMaintenance>();
        public static List<BranchMaintenance> SubBranchDetails = new List<BranchMaintenance>();
        public static List<TMMaster> TmDetails = new List<TMMaster>();
        public static List<ClientMaster> ClientDetails = new List<ClientMaster>();
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
                    TempData["Message"] = "Data Saved Successfully";
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
            TempData["DeleteMessage"] = "Data Deleted Successfully";
            return RedirectToAction("UserIDMaintenance", "Master", model);
        }


        public ActionResult UserIDUpdate(TradeUserIds model)
        {

            MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteNonQuery("update " + dbuser + ".CUBRANCHFILE set branchcode='" + model.BranchCode + "',EXCHANGE='" + model.Exchange + "',proclient='" + model.Proclient + "',brokercode='" + model.BrokerCode + "',ctclid='" + model.CTCLID + "',prefix='" + model.Prefix + "' where clientid='" + model.ClientCodeFrom + "'", Session["SelectedConn"].ToString());
            ModelState.Clear();
            TempData["Message"] = "Data Updated Successfully";
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



        //------------------------Branch Maintenance Start---------------------------------

        [HttpGet]
        public ActionResult BranchMaintenance()
        {
            BranchMaintenance model = new BranchMaintenance();
            if (model.BranchCode == null)
            {
                DataSet ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet("SELECT CODE,NAME,ADD1,ADD2,ADD3,ADD4,PHNOS,EMAIL,RMCODE,REGIONCODE,BRGROUPADMIN,BRGROUPCD,INTCODE,FAX,ACOPENCHARGEYN,ACOPENCHARGE,MANAGER,INTPER FROM " + dbuser + ".BRANCHMST", Session["SelectedConn"].ToString());
                model.result = ds;
                return View(model);
            }


            return View(model);
        }

        [HttpPost]
        public ActionResult BranchMaintenance(BranchMaintenance model, string submit)
        {
            switch (submit)
            {
                case "Save":
                    return (SaveBranchMaintenance(model));
                case "Delete":
                    return (DeleteBranchMaintenance(model));

            }
            return View(model);

        }

        public ActionResult SaveBranchMaintenance(BranchMaintenance model)
        {

            try
            {
                DataSet ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet("SELECT ROWID ID,CODE,NAME FROM " + dbuser + ".BRANCHMST where rowID='" + model.Rwid + "'", Session["SelectedConn"].ToString());
                model.result = ds;
                if (ds.Tables[0].Rows.Count == 0)
                {

                    MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteNonQuery("INSERT INTO " + dbuser + ".BRANCHMST(CODE,NAME,ADD1,ADD2,ADD3,ADD4,PHNOS,EMAIL,MANAGER,ACOPENCHARGEYN,ACOPENCHARGE,INTCODE,INTPER,RMCODE,REGIONCODE,BRGROUPCD,BRGROUPADMIN,FAX,BRGSTREGNO ) VALUES('" + model.BranchCode + "', '" + model.Name + "','" + model.Address1 + "','" + model.Address2 + "','" + model.Address3 + "','" + model.Address4 + "','" + model.Phone + "','" + model.EmailId + "','" + model.Manager + "','" + model.AccOpenCharges + "','" + model.AccOpenCharg + "','" + model.IntroCode + "','" + model.Introducer + "','" + model.RMCode + "','" + model.RegionCode + "','" + model.GroupCode + "','" + model.GroupAdmin + "','" + model.FaxNo + "','" + model.GstRegNo + "')", Session["SelectedConn"].ToString());
                    TempData["Message"] = "Data Saved Successfully";
                    return RedirectToAction("BranchMaintenance", "Master", model);

                }
                else
                {

                    MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteNonQuery("update " + dbuser + ".BRANCHMST set CODE='" + model.BranchCode + "',NAME = '" + model.Name + "',ADD1 ='" + model.Address1 + "',ADD2 = '" + model.Address2 + "',ADD3='" + model.Address3 + "',ADD4 ='" + model.Address4 + "',PHNOS = '" + model.Phone + "',EMAIL='" + model.EmailId + "',MANAGER='" + model.Manager + "',ACOPENCHARGEYN='" + model.AccOpenCharges + "',ACOPENCHARGE='" + model.AccOpenCharg + "',INTCODE='" + model.IntroCode + "',INTPER='" + model.Introducer + "',RMCODE='" + model.RMCode + "',REGIONCODE='" + model.RegionCode + "',BRGROUPCD='" + model.GroupCode + "',BRGROUPADMIN='" + model.GroupAdmin + "',FAX='" + model.FaxNo + "',BRGSTREGNO='" + model.GstRegNo + "'  where rowID='" + model.Rwid + "'", Session["SelectedConn"].ToString());
                    TempData["Message"] = "Data Updated Successfully";
                    return RedirectToAction("BranchMaintenance", "Master", model);
                }


            }

            catch (Exception ex)
            {
                TempData["AlertMessage"] = "" + ex + " Your operation was failed";
                return RedirectToAction("BranchMaintenance", "Master", model);
            }


        }

        public ActionResult DeleteBranchMaintenance(BranchMaintenance model)
        {
            DataSet ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet("SELECT ROWID ID, CODE,NAME FROM " + dbuser + ".BRANCHMST where rowID='" + model.Rwid + "'", Session["SelectedConn"].ToString());
            model.result = ds;
            if (ds.Tables[0].Rows.Count != 0)
            {
                MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteNonQuery("delete from  " + dbuser + ".BRANCHMST where rowID='" + model.Rwid + "'", Session["SelectedConn"].ToString());
                TempData["DeleteMessage"] = "Data Deleted Sucessfully";
                return RedirectToAction("BranchMaintenance", "Master", model);
            }

            return View(model);
        }


        public ActionResult GetBranchDetails(string rid)
        {

            try
            {
                BranchMaintenance branch = new BranchMaintenance();
                DataSet ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet("SELECT ROWID ID, CODE,NAME,ADD1,ADD2,ADD3,ADD4,PHNOS,EMAIL,MANAGER,ACOPENCHARGEYN,ACOPENCHARGE,INTCODE,INTPER,RMCODE,REGIONCODE,BRGROUPCD,BRGROUPADMIN,FAX,BRGSTREGNO FROM " + dbuser + ".BRANCHMST where rowID='" + rid + "'", Session["SelectedConn"].ToString());
                if (ds.Tables[0].Rows.Count != 0)
                {

                    branch.BranchCode = ds.Tables[0].Rows[0]["CODE"].ToString();
                    branch.Name = ds.Tables[0].Rows[0]["NAME"].ToString();
                    branch.Manager = ds.Tables[0].Rows[0]["MANAGER"].ToString();
                    branch.Address1 = ds.Tables[0].Rows[0]["ADD1"].ToString();
                    branch.Address2 = ds.Tables[0].Rows[0]["ADD2"].ToString();
                    branch.Address3 = ds.Tables[0].Rows[0]["ADD3"].ToString();
                    branch.Address4 = ds.Tables[0].Rows[0]["ADD4"].ToString();
                    branch.Phone = ds.Tables[0].Rows[0]["PHNOS"].ToString();
                    branch.EmailId = ds.Tables[0].Rows[0]["EMAIL"].ToString();
                    branch.AccOpenCharges = ds.Tables[0].Rows[0]["ACOPENCHARGEYN"].ToString();
                    branch.AccOpenCharg = ds.Tables[0].Rows[0]["ACOPENCHARGE"].ToString();
                    branch.IntroCode = ds.Tables[0].Rows[0]["INTCODE"].ToString();
                    branch.Introducer = ds.Tables[0].Rows[0]["INTPER"].ToString();
                    branch.GstRegNo = ds.Tables[0].Rows[0]["BRGSTREGNO"].ToString();
                    branch.GroupCode = ds.Tables[0].Rows[0]["BRGROUPCD"].ToString();
                    branch.GroupAdmin = ds.Tables[0].Rows[0]["BRGROUPADMIN"].ToString();
                    branch.RegionCode = ds.Tables[0].Rows[0]["REGIONCODE"].ToString();
                    branch.RMCode = ds.Tables[0].Rows[0]["RMCODE"].ToString();
                    branch.FaxNo = ds.Tables[0].Rows[0]["FAX"].ToString();
                    //branch.Rwid = ds.Tables[0].Rows[0]["ID"].ToString();
                    BranchDetails.Add(branch);
                }

                return Json(branch, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                TempData["AlertMessage"] = "" + ex + " Your operation was failed";
                RedirectToAction("BranchMaintenance", "Master");
            }

            return View();
        }

        //------------------------Branch Maintenance End------------------------------------


        //---------------------Sub Branch Maintenance ---------------------------------------

        [HttpGet]
        public ActionResult SubBranchMaintenance()
        {
            BranchMaintenance model = new BranchMaintenance();
            if (model.BranchCode == null)
            {
                DataSet ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet("SELECT ROWID ID, MAINCODE,SUBCODE FROM " + dbuser + ".BRANCHMST_SUB", Session["SelectedConn"].ToString());
                model.result = ds;
                return View(model);
            }


            return View(model);
        }

        [HttpPost]
        public ActionResult SubBranchMaintenance(BranchMaintenance model, string submit)
        {
            switch (submit)
            {
                case "Save":
                    return (SaveSubBranchMaintenance(model));
                case "Delete":
                    return (DeleteSubBranchMaintenance(model));

            }
            return View(model);

        }

        public ActionResult SaveSubBranchMaintenance(BranchMaintenance model)
        {

            try
            {
                DataSet ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet("SELECT ROWID ID,MAINCODE,SUBCODE FROM " + dbuser + ".BRANCHMST_SUB where rowID='" + model.Rwid + "'", Session["SelectedConn"].ToString());
                model.result = ds;
                if (ds.Tables[0].Rows.Count == 0)
                {

                    MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteNonQuery("INSERT INTO " + dbuser + ".BRANCHMST_SUB(MAINCODE,SUBCODE,SUBNAME,MANAGER,ADD1,ADD2,ADD3,ADD4,PHNOS,MAIL,RMCODE ) VALUES('" + model.SubBranchCode + "', '" + model.SubBranchCode + "', '" + model.SubBranchName + "','" + model.Manager + "','" + model.Address1 + "','" + model.Address2 + "','" + model.Address3 + "','" + model.Address4 + "','" + model.Phone + "','" + model.EmailId + "','" + model.RMCode + "')", Session["SelectedConn"].ToString());
                    TempData["Message"] = "Data Saved Successfully";
                    return RedirectToAction("BranchMaintenance", "Master", model);

                }
                else
                {

                    MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteNonQuery("update " + dbuser + ".BRANCHMST_SUB set MAINCODE='" + model.SubBranchCode + "',SUBCODE='" + model.SubBranchCode + "',SUBNAME='" + model.SubBranchName + "',MANAGER='" + model.Manager + "',ADD1='" + model.Address1 + "',ADD2='" + model.Address2 + "', ADD3='" + model.Address3 + "',ADD4='" + model.Address4 + "',PHNOS='" + model.Phone + "',MAIL='" + model.EmailId + "',RMCODE='" + model.RMCode + "' where rowID='" + model.Rwid + "'", Session["SelectedConn"].ToString());
                    TempData["Message"] = "Data Updated Successfully";
                    return RedirectToAction("BranchMaintenance", "Master", model);
                }


            }

            catch (Exception ex)
            {
                TempData["AlertMessage"] = "" + ex + " Your operation was failed";
                return RedirectToAction("SubBranchMaintenance", "Master", model);
            }


        }

        public ActionResult DeleteSubBranchMaintenance(BranchMaintenance model)
        {
            DataSet ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet("SELECT ROWID ID, MAINCODE,SUBCODE FROM " + dbuser + ".BRANCHMST_SUB where rowID='" + model.Rwid + "'", Session["SelectedConn"].ToString());
            model.result = ds;
            if (ds.Tables[0].Rows.Count != 0)
            {
                MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteNonQuery("delete from  " + dbuser + ".BRANCHMST_SUB where rowID='" + model.Rwid + "'", Session["SelectedConn"].ToString());
                TempData["DeleteMessage"] = "Data Deleted Sucessfully";
                return RedirectToAction("SubBranchMaintenance", "Master", model);
            }

            return View(model);
        }


        public ActionResult GetSubBranchDetails(string rid)
        {
            try
            {
                BranchMaintenance subbranch = new BranchMaintenance();
                DataSet ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet("SELECT ROWID ID, MAINCODE,SUBCODE,SUBNAME,MANAGER,ADD1,ADD2,ADD3,ADD4,PHNOS,MAIL,RMCODE FROM " + dbuser + ".BRANCHMST_SUB where rowID='" + rid + "'", Session["SelectedConn"].ToString());
                if (ds.Tables[0].Rows.Count != 0)
                {
                    subbranch.BranchCode = ds.Tables[0].Rows[0]["MAINCODE"].ToString();
                    subbranch.SubBranchCode = ds.Tables[0].Rows[0]["SUBCODE"].ToString();
                    subbranch.SubBranchName = ds.Tables[0].Rows[0]["SUBNAME"].ToString();
                    subbranch.Manager = ds.Tables[0].Rows[0]["MANAGER"].ToString();
                    subbranch.Address1 = ds.Tables[0].Rows[0]["ADD1"].ToString();
                    subbranch.Address2 = ds.Tables[0].Rows[0]["ADD2"].ToString();
                    subbranch.Address3 = ds.Tables[0].Rows[0]["ADD3"].ToString();
                    subbranch.Address4 = ds.Tables[0].Rows[0]["ADD4"].ToString();
                    subbranch.Phone = ds.Tables[0].Rows[0]["PHNOS"].ToString();
                    subbranch.EmailId = ds.Tables[0].Rows[0]["MAIL"].ToString();
                    subbranch.RMCode = ds.Tables[0].Rows[0]["RMCODE"].ToString();
                    subbranch.Rwid = ds.Tables[0].Rows[0]["ID"].ToString();
                    SubBranchDetails.Add(subbranch);
                }
                return Json(subbranch, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                TempData["AlertMessage"] = "" + ex + " Your operation was failed";
                RedirectToAction("SubBranchMaintenance", "Master");
            }

            return View();
        }


        //----------------------Region Master---------------------------------------------

        [HttpGet]
        public ActionResult RegionMaster()
        {
            BranchMaintenance model = new BranchMaintenance();
            if (model.RegionCode == null)
            {
                DataSet ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet("SELECT ROWID ID, REGIONCODE,REGIONDESC,ZONE FROM " + dbuser + ".REGIONMASTER", Session["SelectedConn"].ToString());
                model.result = ds;
                return View(model);
            }


            return View(model);
        }

        [HttpPost]
        public ActionResult RegionMaster(BranchMaintenance model, string submit)
        {
            switch (submit)
            {
                case "Save":
                    return (SaveRegionMaster(model));
                case "Delete":
                    return (DeleteRegionMaster(model));

            }
            return View(model);
        }

        public ActionResult SaveRegionMaster(BranchMaintenance model)
        {
            try
            {
                DataSet ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet("SELECT ROWID ID, REGIONCODE,REGIONDESC,ZONE FROM " + dbuser + ".REGIONMASTER where rowID='" + model.Rwid + "'", Session["SelectedConn"].ToString());
                model.result = ds;
                if (ds.Tables[0].Rows.Count == 0)
                {

                    MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteNonQuery("INSERT INTO " + dbuser + ".REGIONMASTER(REGIONCODE,REGIONDESC,ZONE ) VALUES('" + model.RegionCode + "', '" + model.RegionDesc + "', '" + model.Zone + "')", Session["SelectedConn"].ToString());
                    TempData["Message"] = "Data Saved Successfully";
                    return RedirectToAction("RegionMaster", "Master", model);

                }
                else
                {

                    MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteNonQuery("update " + dbuser + ".REGIONMASTER set REGIONCODE='" + model.RegionCode + "',REGIONDESC='" + model.RegionDesc + "', ZONE='" + model.Zone + "' where rowID='" + model.Rwid + "'", Session["SelectedConn"].ToString());
                    TempData["Message"] = "Data Updated Successfully";
                    return RedirectToAction("RegionMaster", "Master", model);
                }


            }

            catch (Exception ex)
            {
                TempData["AlertMessage"] = "" + ex + " Your operation was failed";
                return RedirectToAction("RegionMaster", "Master", model);
            }
        }

        public ActionResult DeleteRegionMaster(BranchMaintenance model)
        {
            DataSet ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet("SELECT ROWID ID, REGIONCODE,REGIONDESC,ZONE FROM " + dbuser + ".REGIONMASTER where rowID='" + model.Rwid + "'", Session["SelectedConn"].ToString());
            model.result = ds;
            if (ds.Tables[0].Rows.Count != 0)
            {
                MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteNonQuery("delete from  " + dbuser + ".REGIONMASTER where rowID='" + model.Rwid + "'", Session["SelectedConn"].ToString());
                TempData["DeleteMessage"] = "Data Deleted Sucessfully";
                return RedirectToAction("RegionMaster", "Master", model);
            }

            return View(model);
        }

        public ActionResult GetRegionDetails(string rid)
        {
            try
            {
                BranchMaintenance region = new BranchMaintenance();
                DataSet ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet("SELECT ROWID ID, REGIONCODE,REGIONDESC,ZONE FROM " + dbuser + ".REGIONMASTER where rowID='" + rid + "'", Session["SelectedConn"].ToString());
                if (ds.Tables[0].Rows.Count != 0)
                {
                    region.RegionCode = ds.Tables[0].Rows[0]["REGIONCODE"].ToString();
                    region.RegionDesc = ds.Tables[0].Rows[0]["REGIONDESC"].ToString();
                    region.Zone = ds.Tables[0].Rows[0]["ZONE"].ToString();
                    region.Rwid = ds.Tables[0].Rows[0]["ID"].ToString();
                    SubBranchDetails.Add(region);
                }
                return Json(region, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                TempData["AlertMessage"] = "" + ex + " Your operation was failed";
                RedirectToAction("RegionMaster", "Master");
            }

            return View();
        }



        //----------------------------RM Master---------------------------------------------

        [HttpGet]
        public ActionResult RMMaster()
        {
            BranchMaintenance model = new BranchMaintenance();
            model.JoiningDate = DateTime.Parse(Session["FinYearTo"].ToString());
            model.ClosingDate = DateTime.Parse(Session["FinYearTo"].ToString());
            if (model.RMCode == null)
            {
                DataSet ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet("SELECT ROWID ID, RMCODE,RMDESC,BRANCH FROM " + dbuser + ".RMMASTER", Session["SelectedConn"].ToString());
                model.result = ds;
                return View(model);
            }


            return View(model);
        }

        [HttpPost]
        public ActionResult RMMaster(BranchMaintenance model, string submit)
        {
            switch (submit)
            {
                case "Save":
                    return (SaveRMMaster(model));
                case "Delete":
                    return (DeleteRMMaster(model));

            }
            return View(model);
        }


        public ActionResult SaveRMMaster(BranchMaintenance model)
        {
            try
            {
                DataSet ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet("SELECT ROWID ID, RMCODE,RMDESC,BRANCH FROM " + dbuser + ".RMMASTER where rowID='" + model.Rwid + "'", Session["SelectedConn"].ToString());
                model.result = ds;
                if (ds.Tables[0].Rows.Count == 0)
                {

                    MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteNonQuery("INSERT INTO " + dbuser + ".RMMASTER(RMCODE,RMDESC,MOBILENO,EMAILNO,JOINDATE,SALARY,PERTARGET,CLOSEDATE,ADD1,ADD2,RMCITY,RMSTATE,RMPIN ,ZONE,BRANCH,RMDESIG,RMDEP ) VALUES('" + model.RMCode + "', '" + model.RMDesc + "', '" + model.MobileNo + "','" + model.EmailId + "','" + model.JoiningDate.ToString("ddMMMyyyy") + "','" + model.Salary + "','" + model.PerMonthTarget + "','" + model.ClosingDate.ToString("ddMMMyyyy") + "','" + model.Address1 + "','" + model.Address2 + "','" + model.City + "','" + model.State + "','" + model.PinCode + "','" + model.Zone + "','" + model.BranchCode + "','" + model.Designation + "','" + model.Department + "')", Session["SelectedConn"].ToString());
                    TempData["Message"] = "Data Saved Successfully";
                    return RedirectToAction("RMMaster", "Master", model);

                }
                else
                {

                    MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteNonQuery("update " + dbuser + ".RMMASTER set RMCODE='" + model.RMCode + "',RMDESC='" + model.RMDesc + "', MOBILENO='" + model.MobileNo + "',EMAILNO='" + model.EmailId + "',JOINDATE=to_date('" + model.JoiningDate.ToString("ddMMMyyyy") + "'),SALARY='" + model.Salary + "',PERTARGET='" + model.PerMonthTarget + "',CLOSEDATE=to_date('" + model.ClosingDate.ToString("ddMMMyyyy") + "'),ADD1='" + model.Address1 + "',ADD2='" + model.Address2 + "',RMCITY='" + model.City + "',RMSTATE='" + model.State + "',RMPIN ='" + model.PinCode + "',ZONE='" + model.Zone + "',BRANCH='" + model.BranchCode + "',RMDESIG='" + model.Designation + "',RMDEP='" + model.Department + "'  where rowID='" + model.Rwid + "'", Session["SelectedConn"].ToString());
                    TempData["Message"] = "Data Updated Successfully";
                    return RedirectToAction("RMMaster", "Master", model);
                }


            }

            catch (Exception ex)
            {
                TempData["AlertMessage"] = "" + ex + " Your operation was failed";
                return RedirectToAction("RMMaster", "Master", model);
            }
        }

        public ActionResult DeleteRMMaster(BranchMaintenance model)
        {
            DataSet ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet("SELECT ROWID ID, RMCODE,RMDESC,BRANCH FROM " + dbuser + ".RMMASTER where rowID='" + model.Rwid + "'", Session["SelectedConn"].ToString());
            model.result = ds;
            if (ds.Tables[0].Rows.Count != 0)
            {
                MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteNonQuery("delete from  " + dbuser + ".REGIONMASTER where rowID='" + model.Rwid + "'", Session["SelectedConn"].ToString());
                TempData["DeleteMessage"] = "Data Deleted Sucessfully";
                return RedirectToAction("RMMaster", "Master", model);
            }

            return View(model);
        }

        public ActionResult GetRMDetails(string rid)
        {
            try
            {
                BranchMaintenance rm = new BranchMaintenance();
                DataSet ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet("SELECT ROWID ID, RMCODE,RMDESC,MOBILENO,EMAILNO,JOINDATE,SALARY,PERTARGET,CLOSEDATE,ADD1,ADD2,RMCITY,RMSTATE,RMPIN ,ZONE,BRANCH,RMDESIG,RMDEP FROM " + dbuser + ".RMMASTER where rowID='" + rid + "'", Session["SelectedConn"].ToString());
                if (ds.Tables[0].Rows.Count != 0)
                {
                    rm.RegionCode = ds.Tables[0].Rows[0]["RMCODE"].ToString();
                    rm.RegionDesc = ds.Tables[0].Rows[0]["RMDESC"].ToString();
                    rm.MobileNo = ds.Tables[0].Rows[0]["MOBILENO"].ToString();
                    rm.EmailId = ds.Tables[0].Rows[0]["EMAILNO"].ToString();
                    rm.Salary = ds.Tables[0].Rows[0]["SALARY"].ToString();
                    rm.PerMonthTarget = ds.Tables[0].Rows[0]["PERTARGET"].ToString();
                    rm.Address1 = ds.Tables[0].Rows[0]["ADD1"].ToString();
                    rm.Address2 = ds.Tables[0].Rows[0]["ADD2"].ToString();
                    rm.City = ds.Tables[0].Rows[0]["RMCITY"].ToString();
                    rm.State = ds.Tables[0].Rows[0]["RMSTATE"].ToString();
                    rm.PinCode = ds.Tables[0].Rows[0]["RMPIN"].ToString();
                    rm.Zone = ds.Tables[0].Rows[0]["ZONE"].ToString();
                    rm.BranchCode = ds.Tables[0].Rows[0]["BRANCH"].ToString();
                    rm.Designation = ds.Tables[0].Rows[0]["RMDESIG"].ToString();
                    rm.Department = ds.Tables[0].Rows[0]["RMDEP"].ToString();
                    rm.Rwid = ds.Tables[0].Rows[0]["ID"].ToString();
                    rm.ClosingDate = DateTime.Parse(ds.Tables[0].Rows[0]["CLOSEDATE"].ToString());
                    rm.JoiningDate = DateTime.Parse(ds.Tables[0].Rows[0]["JOINDATE"].ToString());
                    SubBranchDetails.Add(rm);
                }
                return Json(rm, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                TempData["AlertMessage"] = "" + ex + " Your operation was failed";
                RedirectToAction("RMMaster", "Master");
            }

            return View();
        }


        //------------------------Contract Specification Start---------------------------------

        [HttpGet]
        public ActionResult ContractSpecification()
        {


            ContractSpecification model = new ContractSpecification();

            if (model.ContName == null)
            {
                DataSet ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet("SELECT  INSTRUMENT_TYPE,SYMBOL,to_char( EXPIRYDATE,'dd-mm-yyyy')EXPIRYDATE,LOTSIZE,CONTNAME FROM " + dbuser + ".CUCONTRACTS", Session["SelectedConn"].ToString());
                model.result = ds;
                return View(model);
            }


            return View(model);



        }

        [HttpPost]
        public ActionResult ContractSpecification(ContractSpecification model, string submit)
        {

            switch (submit)
            {
                case "Save":
                    return (SaveContractSpecification(model));
                    //case "Delete":
                    //    return (DeleteContractSpecification(model));

            }
            return View(model);
        }



        public ActionResult SaveContractSpecification(ContractSpecification model)
        {
            try
            {
                DataSet ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet("SELECT ROWID ID, INSTRUMENT_TYPE,SYMBOL,to_char( EXPIRYDATE,'dd-mm-yyyy')EXPIRYDATE,LOTSIZE,CONTNAME FROM " + dbuser + ".CUCONTRACTS where rowID='" + model.Rwid + "'", Session["SelectedConn"].ToString());
                model.result = ds;
                if (ds.Tables[0].Rows.Count == 0)
                {

                    MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteNonQuery("INSERT INTO " + dbuser + ".CUCONTRACTS(INSTRUMENT_TYPE, SYMBOL,to_char( EXPIRYDATE,'dd-mm-yyyy') EXPIRYDATE, CONTNAME, STRIKEPRICE, MKTLOT, EXCHANGE, COMULTIPLIER) VALUES('" + model.InstrumentType + "', '" + model.Symbol + "', to_DATE('" + model.ExpiryDate + "','dd-mm-yyyy' ),,'" + model.LotSize + "', '" + model.Exchange + "')", Session["SelectedConn"].ToString());
                    TempData["Message"] = "Data Saved Successfully";
                    //ViewBag.Message = "Data Saved Sucessfully";
                    return View(model);

                }
                else
                {

                    //MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteNonQuery("update " + dbuser + ".CUCONTRACTS set INSTRUMENT_TYPE='" + model.InstrumentType + "',SYMBOL='" + model.Symbol + "',EXPIRYDATE=to_date('" + model.ExpiryDate + "','dd-mm-yyyy'),MKTLOT='" + model.LotSize + "' where rowID='" + model.Rwid + "'", Session["SelectedConn"].ToString());
                    //TempData["Message"] = "Data Updated Successfully";
                    //ViewBag.Message = "Data Updated Sucessfully";
                }


            }

            catch (Exception ex)
            {
                TempData["AlertMessage"] = "" + ex + " Your operation was failed";
                RedirectToAction("ContractSpecification", "Master");
            }
            return View(model);

        }


        //public ActionResult DeleteContractSpecification(ContractSpecification model)
        //{
        //    DataSet ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet("SELECT ROWID ID, INSTRUMENT_TYPE,SYMBOL,EXPIRYDATE,LOTSIZE,CONTNAME FROM " + dbuser + ".CUCONTRACTS where rowID='" + model.Rwid + "'", Session["SelectedConn"].ToString());
        //    model.result = ds;
        //    if (ds.Tables[0].Rows.Count != 0)
        //    {
        //        MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteNonQuery("delete from  " + dbuser + ".CUCONTRACTS where rowID='" + model.Rwid + "'", Session["SelectedConn"].ToString());
        //        TempData["DeleteMessage"] = "Data Deleted Sucessfully";
        //        return RedirectToAction("ContractSpecification", "Master", model);
        //    }

        //    return View(model);
        //}


        public ActionResult GetContractDetails(string rid)
        {


            try
            {
                ContractSpecification csn = new ContractSpecification();
                DataSet ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet("SELECT rowID ID,INSTRUMENT_TYPE,SYMBOL,to_char( EXPIRYDATE,'dd-mm-yyyy')EXPIRYDATE,LOTSIZE,CONTNAME FROM " + dbuser + ".CUCONTRACTS where rowID='" + rid + "'", Session["SelectedConn"].ToString());
                if (ds.Tables[0].Rows.Count != 0)
                {

                    csn.InstrumentType = ds.Tables[0].Rows[0]["INSTRUMENT_TYPE"].ToString();
                    csn.Symbol = ds.Tables[0].Rows[0]["SYMBOL"].ToString();
                    csn.ExpiryDate = ds.Tables[0].Rows[0]["EXPIRYDATE"].ToString();
                    csn.LotSize = ds.Tables[0].Rows[0]["LOTSIZE"].ToString();
                    csn.ContName = ds.Tables[0].Rows[0]["CONTNAME"].ToString();
                    csn.Rwid = ds.Tables[0].Rows[0]["ID"].ToString();
                    Contract.Add(csn);
                }
                return Json(csn, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                TempData["AlertMessage"] = "" + ex + " Your operation was failed";
                RedirectToAction("ContractSpecification", "Master");
            }

            return View();
        }


        //------------------------Contract Specification End-----------------------------------


        //------------------------Client Master Start---------------------------------
        [HttpGet]
        public ActionResult ClientMaster()
        {
            ClientMaster model = new ClientMaster();
            if (model.ClientCode == null)
            {
                DataSet ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet("SELECT ROWID ID, PAR_CODE,PAR_NAME,GROUPCODE FROM " + dbuser + ".CUPARTYMST", Session["SelectedConn"].ToString());
                model.result = ds;
                return View(model);
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult ClientMaster(ClientMaster model, string submit)
        {

            switch (submit)
            {
                case "Save":
                    return (SaveClientMaster(model));
                case "Delete":
                    return (DeleteClientMaster(model));

            }
            return View(model);
        }


        public ActionResult SaveClientMaster(ClientMaster model)
        {
            try
            {
                DataSet ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet("SELECT ROWID ID, PAR_CODE,PAR_NAME,GROUPCODE FROM " + dbuser + ".CUPARTYMST where rowID='" + model.Rwid + "'", Session["SelectedConn"].ToString());
                model.result = ds;
                //if (ds.Tables[0].Rows.Count == 0)
                //{
                //    string contname = model.InstrumentType + model.Symbol + model.ExpiryDate.ToString("ddMMMyyyy").ToUpper();
                //    MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteNonQuery("INSERT INTO " + dbuser + ".CUPARTYMST(INSTRUMENT_TYPE, SYMBOL, EXPIRYDATE, CONTNAME, STRIKEPRICE, MKTLOT, EXCHANGE, COMULTIPLIER) VALUES('" + model.InstrumentType + "', '" + model.Symbol + "', '" + model.ExpiryDate.ToString("ddMMMyyyy") + "', '" + contname + "', .00, '" + model.LotSize + "', '" + model.Exchange + "', 1.000000000)", Session["SelectedConn"].ToString());
                //    TempData["Message"] = "Data Saved Successfully";
                //    //ViewBag.Message = "Data Saved Sucessfully";
                //    return View(model);

                //}
                //else
                //{
                //    string contname = model.InstrumentType + model.Symbol + model.ExpiryDate.ToString("ddMMMyyyy").ToUpper();
                //    MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteNonQuery("update " + dbuser + ".CUPARTYMST set INSTRUMENT_TYPE='" + model.InstrumentType + "',SYMBOL='" + model.Symbol + "',EXPIRYDATE=to_date('" + model.ExpiryDate.ToString("ddMMMyyyy") + "'),CONTNAME='" + contname + "',MKTLOT='" + model.LotSize + "' where rowID='" + model.Rwid + "'", Session["SelectedConn"].ToString());
                //    TempData["Message"] = "Data Updated Successfully";
                //    //ViewBag.Message = "Data Updated Sucessfully";
                //}


            }

            catch (Exception ex)
            {
                TempData["AlertMessage"] = "" + ex + " Your operation was failed";
                RedirectToAction("ContractSpecification", "Master");
            }
            return View(model);

        }


        public ActionResult DeleteClientMaster(ClientMaster model)
        {
            DataSet ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet("SELECT ROWID ID, PAR_CODE,PAR_NAME,GROUPCODE FROM " + dbuser + ".CUPARTYMST where rowID='" + model.Rwid + "'", Session["SelectedConn"].ToString());
            model.result = ds;
            if (ds.Tables[0].Rows.Count != 0)
            {
                MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteNonQuery("delete from  " + dbuser + ".CUPARTYMST where rowID='" + model.Rwid + "'", Session["SelectedConn"].ToString());
                TempData["DeleteMessage"] = "Data Deleted Sucessfully";
                return RedirectToAction("ContractSpecification", "Master", model);
            }

            return View(model);
        }


        public ActionResult GetClientsDetails(string rid)
        {


            try
            {
                ClientMaster cm = new ClientMaster();
                DataSet ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet("SELECT rowID ID,INSTRUMENT_TYPE,SYMBOL,EXPIRYDATE,LOTSIZE,CONTNAME FROM " + dbuser + ".CUPARTYMST where rowID='" + rid + "'", Session["SelectedConn"].ToString());
                if (ds.Tables[0].Rows.Count != 0)
                {

                    cm.ClientCode = ds.Tables[0].Rows[0]["PAR_CODE"].ToString();
                    cm.Name = ds.Tables[0].Rows[0]["PAR_NAME"].ToString();
                    cm.FatherName = ds.Tables[0].Rows[0]["FNAME"].ToString();
                    cm.Address1 = ds.Tables[0].Rows[0]["PAR_ADD1"].ToString();
                    cm.Address2 = ds.Tables[0].Rows[0]["PAR_ADD2"].ToString();
                    cm.Address3 = ds.Tables[0].Rows[0]["PAR_ADD3"].ToString();
                    cm.PAddress1 = ds.Tables[0].Rows[0]["PAR_ADD11"].ToString();
                    cm.PAddress2 = ds.Tables[0].Rows[0]["PAR_ADD21"].ToString();
                    cm.PAddress3 = ds.Tables[0].Rows[0]["PAR_ADD31"].ToString();
                    cm.PinCode = ds.Tables[0].Rows[0]["PINCODE"].ToString();
                    cm.Dob = ds.Tables[0].Rows[0]["DOB"].ToString();
                    cm.Phone = ds.Tables[0].Rows[0]["PHNOS"].ToString();
                    cm.Mobile = ds.Tables[0].Rows[0]["MOBILENOS"].ToString();
                    cm.EmailId = ds.Tables[0].Rows[0]["EMAILNO"].ToString();
                    cm.City = ds.Tables[0].Rows[0]["CITY"].ToString();
                    cm.State = ds.Tables[0].Rows[0]["STATE"].ToString();
                    cm.StateCode = ds.Tables[0].Rows[0]["STATECODE"].ToString();
                    cm.Country = ds.Tables[0].Rows[0]["COUNTRY"].ToString();
                    cm.Group = ds.Tables[0].Rows[0]["GROUPCLIENT"].ToString();
                    cm.Gender = ds.Tables[0].Rows[0]["GENDER"].ToString();
                    cm.Martial = ds.Tables[0].Rows[0]["MARITAL_STATUS"].ToString();
                    cm.AccountGroup = ds.Tables[0].Rows[0]["CONTNAME"].ToString();
                    cm.BranchCode = ds.Tables[0].Rows[0]["BRANCHIND"].ToString();
                    cm.SubBranch = ds.Tables[0].Rows[0]["SUBBRANCHIND"].ToString();
                    cm.PanNo = ds.Tables[0].Rows[0]["ITAXNO"].ToString();
                    cm.ContractType = ds.Tables[0].Rows[0]["CONTRACT_TYPE"].ToString();
                    cm.RMCode = ds.Tables[0].Rows[0]["RMCODE"].ToString();
                    cm.CIN = ds.Tables[0].Rows[0]["CIN"].ToString();
                    cm.Category = ds.Tables[0].Rows[0]["CATG"].ToString();
                    cm.ClientEnable = ds.Tables[0].Rows[0]["CONTNAME"].ToString();
                    cm.Reason = ds.Tables[0].Rows[0]["CONTNAME"].ToString();
                    cm.Remark = ds.Tables[0].Rows[0]["REMARKS"].ToString();
                    cm.Exchange = ds.Tables[0].Rows[0]["CONTNAME"].ToString();
                    cm.ShortCode = ds.Tables[0].Rows[0]["CONTNAME"].ToString();
                    cm.BrokerageMethod = ds.Tables[0].Rows[0]["CONTNAME"].ToString();
                    cm.ContractG = ds.Tables[0].Rows[0]["CONTNAME"].ToString();
                    cm.Tax1 = ds.Tables[0].Rows[0]["CONTNAME"].ToString();
                    cm.Tax2 = ds.Tables[0].Rows[0]["CONTNAME"].ToString();
                    cm.Tax3 = ds.Tables[0].Rows[0]["CONTNAME"].ToString();
                    cm.Tax4 = ds.Tables[0].Rows[0]["CONTNAME"].ToString();
                    cm.CashAcc = ds.Tables[0].Rows[0]["CONTNAME"].ToString();
                    cm.DailyMTMAc = ds.Tables[0].Rows[0]["CONTNAME"].ToString();
                    cm.DailyMarginAcc = ds.Tables[0].Rows[0]["CONTNAME"].ToString();
                    cm.InterestApp = ds.Tables[0].Rows[0]["CONTNAME"].ToString();
                    cm.Interest = ds.Tables[0].Rows[0]["CONTNAME"].ToString();
                    cm.Interestamt = ds.Tables[0].Rows[0]["CONTNAME"].ToString();
                    cm.Securityacc = ds.Tables[0].Rows[0]["CONTNAME"].ToString();
                    cm.OpeningBal = ds.Tables[0].Rows[0]["CONTNAME"].ToString();
                    cm.Dealercd = ds.Tables[0].Rows[0]["CONTNAME"].ToString();
                    cm.brokerageutd = ds.Tables[0].Rows[0]["CONTNAME"].ToString();
                    cm.Transactioin = ds.Tables[0].Rows[0]["CONTNAME"].ToString();
                    cm.stampda = ds.Tables[0].Rows[0]["CONTNAME"].ToString();
                    cm.marginposting = ds.Tables[0].Rows[0]["CONTNAME"].ToString();
                    cm.Custodian = ds.Tables[0].Rows[0]["CONTNAME"].ToString();
                    cm.Introducercd = ds.Tables[0].Rows[0]["CONTNAME"].ToString();
                    cm.UCCUploded = ds.Tables[0].Rows[0]["CONTNAME"].ToString();
                    cm.brokapp = ds.Tables[0].Rows[0]["CONTNAME"].ToString();
                    cm.bankacc = ds.Tables[0].Rows[0]["BANKACNO1"].ToString();
                    cm.actype = ds.Tables[0].Rows[0]["AC_TYPE"].ToString();
                    cm.bankname = ds.Tables[0].Rows[0]["BANKNAME1"].ToString();
                    cm.BAddress1 = ds.Tables[0].Rows[0]["BANKADD11"].ToString();
                    cm.BAddress2 = ds.Tables[0].Rows[0]["BANKADD12"].ToString();
                    cm.MICR = ds.Tables[0].Rows[0]["MICRNO"].ToString();
                    cm.IFSC = ds.Tables[0].Rows[0]["IFSC"].ToString();
                    cm.InCorporationDate = ds.Tables[0].Rows[0]["INCORPDATE"].ToString();
                    cm.regNo = ds.Tables[0].Rows[0]["ROC_REGNO"].ToString();
                    cm.regAuth = ds.Tables[0].Rows[0]["ROC_AUTH"].ToString();
                    cm.regplace = ds.Tables[0].Rows[0]["ROC_PLACE"].ToString();
                    cm.contactP1 = ds.Tables[0].Rows[0]["ROC_CONTPERSON"].ToString();
                    cm.contactP2 = ds.Tables[0].Rows[0]["ROC_CONTPERSON2"].ToString();
                    cm.contactP1Deg = ds.Tables[0].Rows[0]["ROC_CONTDESG"].ToString();
                    cm.contactP2Deg = ds.Tables[0].Rows[0]["ROC_CONTDESG2"].ToString();
                    cm.contactP1Add = ds.Tables[0].Rows[0]["ROC_CONTADD"].ToString();
                    cm.contactP2Add = ds.Tables[0].Rows[0]["ROC_CONTADD2"].ToString();
                    cm.ContactP2PhnNo = ds.Tables[0].Rows[0]["ROC_CONTPHONE2"].ToString();
                    cm.ContactP2Emailo = ds.Tables[0].Rows[0]["ROC_CONTEMAIL2"].ToString();
                    cm.ContactP2Pan = ds.Tables[0].Rows[0]["ROC_CONTITAXNO2"].ToString();
                    cm.DirecNm = ds.Tables[0].Rows[0]["ROC_DR_NAME"].ToString();
                    cm.DirecAdd = ds.Tables[0].Rows[0]["ROC_DR_ADD"].ToString();
                    cm.Direcphn = ds.Tables[0].Rows[0]["ROC_DR_PHONE"].ToString();
                    cm.DirPanNo = ds.Tables[0].Rows[0]["ROC_DR_ITAXNO"].ToString();
                    cm.DirEmail = ds.Tables[0].Rows[0]["ROC_DR_EMAIL"].ToString();
                    cm.Din1 = ds.Tables[0].Rows[0]["DIN1"].ToString();
                    cm.Din2 = ds.Tables[0].Rows[0]["DIN2"].ToString();
                    cm.Rwid = ds.Tables[0].Rows[0]["ID"].ToString();
                    ClientDetails.Add(cm);
                }
                return Json(cm, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                TempData["AlertMessage"] = "" + ex + " Your operation was failed";
                RedirectToAction("ContractSpecification", "Master");
            }

            return View();
        }


        //------------------------Client Master End-----------------------------------

        //------------------------Exchange Master Start---------------------------------

        public ActionResult ExchangeMaster()
        {

            return View();
        }

        //------------------------Exchange Master End---------------------------------



        //------------------------Holiday Master---------------------------------
        [HttpGet]
        public ActionResult HolidayMaster()
        {
            HolidayMaster model = new HolidayMaster();
            if (model.Holiday == null)
            {
                DataSet ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet("SELECT  to_char( hdate,'dd-mm-yyyy') hdate, reason, exchange FROM " + dbuser + ".Holiday ", Session["SelectedConn"].ToString()); model.result = ds;
                return View(model);
            }


            return View(model);
        }
        [HttpPost]
        public ActionResult HolidayMaster(HolidayMaster model)
        {

            DataSet ds1 = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet("SELECT  to_char( hdate,'dd-mm-yyyy') hdate, reason, exchange FROM " + dbuser + ".Holiday  where exchange='" + model.Exchange + "'  And hdate= to_DATE('" + model.Sdate + "','dd-mm-yyyy' ) ", Session["SelectedConn"].ToString());
            model.result = ds1;
            if (ds1.Tables[0].Rows.Count == 0)
            {

                MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteNonQuery("insert into " + dbuser + ".Holiday(hdate,reason,exchange)   SELECT to_DATE('" + model.Sdate + "','dd-mm-yyyy' ), '" + model.Holiday.ToUpper() + "' ,'" + model.Exchange + "'  FROM DUAL", Session["SelectedConn"].ToString());
                DataSet ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet("SELECT  to_char( hdate,'dd-mm-yyyy') hdate, reason, exchange FROM " + dbuser + ".Holiday", Session["SelectedConn"].ToString()); model.result = ds;
                TempData["Message"] = "Data Inserted Successfully";
            }

            else
            {

                TempData["Message"] = "Data Already Found";
                DataSet ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet("SELECT  to_char( hdate,'dd-mm-yyyy') hdate, reason, exchange FROM " + dbuser + ".Holiday", Session["SelectedConn"].ToString()); model.result = ds;
                RedirectToAction("HolidayMaster", "Master", model);
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

            MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteNonQuery("delete from  " + dbuser + ".Holiday where REASON='" + model.Holiday + "'  and exchange='" + model.Exchange + "'", Session["SelectedConn"].ToString());
            TempData["DeleteMessage"] = "Data Deleted Successfully";
            return RedirectToAction("HolidayMaster", "Master", model);
        }


        public ActionResult HolidayUpdate(HolidayMaster model)
        {

            MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteNonQuery("update  " + dbuser + ".Holiday set REASON='" + model.Holiday.ToUpper() + "' where  hdate=to_date('" + model.Sdate + "','dd-mm-yyyy') and exchange='" + model.Exchange + "'", Session["SelectedConn"].ToString());
            TempData["Message"] = "Data Updated Successfully";
            return RedirectToAction("HolidayMaster", "Master", model);
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


        //------------------------Bank Account Details Start---------------------------------


     

        [HttpGet]
        public ActionResult BankAccountDetails()
        {
            BankAccountDetails model = new BankAccountDetails();
            if (model.ClientCodeFrom == null)
            {
                DataSet ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet("select  b.CLIENTCODE ,c.PAR_NAME,b.BANKACNO,b.BANKNAME,b.BANKADD1,b.BANKADD2, b.BANKADD3, b.MICRNO, b.IFSC  from " + dbuser + ".BANKDETAILS b, IFSC.CUPARTYMST c where  b.CLIENTCODE = c.PAR_CODE", Session["SelectedConn"].ToString());
                model.result = ds;
                return View(model);
            }


            return View(model);
        }

        [HttpPost]
        public ActionResult BankAccountDetails(BankAccountDetails model)
        {
            try
            {
                DataSet ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet("SELECT BANKACNO,BANKNAME,BANKADD1,BANKADD2,BANKADD3,MICRNO,IFSC FROM " + dbuser + ".BANKDETAILS where CLIENTCODE='" + model.ClientCodeFrom + "'and BANKACNO ='" + model.BankAc + "' ", Session["SelectedConn"].ToString());
                model.result = ds;
                string BankAc = model.BankAc != null ? model.BankAc.ToUpper() : "";
                string BankName = model.BankName != null ? model.BankName.ToUpper() : "";
                string Address1 = model.Address1 != null ? model.Address1.ToUpper() : "";
                string Address2 = model.Address2 != null ? model.Address2.ToUpper() : "";
                string Address3 = model.Address3 != null ? model.Address3.ToUpper() : "";
                string Micr = model.Micr != null ? model.Micr.ToUpper() : "";
                string IFSC = model.IFSC != null ? model.IFSC.ToUpper() : "";

                if (ds.Tables[0].Rows.Count == 0)
                {
                    string query = "INSERT INTO " + dbuser + ".BANKDETAILS(CLIENTCODE,BANKACNO,BANKNAME,BANKADD1,BANKADD2,BANKADD3,MICRNO,IFSC) VALUES('" + model.ClientCodeFrom + "','" + BankAc + "', '" + BankName + "', '" + Address1 + "','" + Address2 + "','" + Address3 + "','" + Micr + "','" + IFSC + "')";
                    MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteNonQuery(query, Session["SelectedConn"].ToString());
                    TempData["Message"] = "Data Saved Successfully";
                    return RedirectToAction("BankAccountDetails", "Master", model);

                }
                else
                {

                    TempData["Message"] = "Data Already Found";
                    
                    return RedirectToAction("BankAccountDetails", "Master", model);
                }
            }

            catch (Exception ex)
            {
                TempData["AlertMessage"] = "" + ex + " Your operation was failed";
                return RedirectToAction("BankAccountDetails", "Master", model);
            }
        }

        public ActionResult EditBankAccountDetails(BankAccountDetails model, string submit)
        {
            switch (submit)
            {
                case "Update":
                    return (UpdateBankAccountDetails(model));
                case "Delete":
                    return (DeleteBankAccountDetails(model));

            }
            return View(model);
        }

        public ActionResult UpdateBankAccountDetails(BankAccountDetails model)
        {

            MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteNonQuery("update " + dbuser + ".BANKDETAILS set BANKACNO='" + model.BankAc + "',BANKNAME='" + model.BankName.ToUpper() + "',BANKADD1='" + model.Address1.ToUpper() + "',BANKADD2='" + model.Address2.ToUpper() + "', BANKADD3='" + model.Address3.ToUpper() + "', MICRNO='" + model.Micr + "',IFSC='" + model.IFSC.ToUpper() + "' where CLIENTCODE='" + model.ClientCodeFrom + "'", Session["SelectedConn"].ToString());
            TempData["Message"] = "Data Updated Successfully";
            return RedirectToAction("BankAccountDetails", "Master", model);
        }

        public ActionResult DeleteBankAccountDetails(BankAccountDetails model)
        {

            MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteNonQuery("delete from  " + dbuser + ".BANKDETAILS where CLIENTCODE='" + model.ClientCodeFrom + "' and BANKACNO='" + model.BankAc + "'", Session["SelectedConn"].ToString());
            TempData["DeleteMessage"] = "Data Deleted Sucessfully";
            return RedirectToAction("BankAccountDetails", "Master", model);
        }

       

        public ActionResult GetBankAccountDetails(string id)
        {

            try
            {
                BankAccountDetails bdetails = new BankAccountDetails();
                DataSet ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet("SELECT CLIENTCODE,BANKACNO,BANKNAME,BANKADD1,BANKADD2,BANKADD3,MICRNO,IFSC FROM " + dbuser + ".BANKDETAILS where CLIENTCODE='" + id + "'", Session["SelectedConn"].ToString());
                if (ds.Tables[0].Rows.Count != 0)
                {
                    bdetails.ClientCodeFrom = ds.Tables[0].Rows[0]["CLIENTCODE"].ToString();
                    bdetails.BankAc = ds.Tables[0].Rows[0]["BANKACNO"].ToString();
                    bdetails.BankName = ds.Tables[0].Rows[0]["BANKNAME"].ToString();
                    bdetails.Micr = ds.Tables[0].Rows[0]["MICRNO"].ToString();
                    bdetails.IFSC = ds.Tables[0].Rows[0]["IFSC"].ToString();
                    bdetails.Address1 = ds.Tables[0].Rows[0]["BANKADD1"].ToString();
                    bdetails.Address2 = ds.Tables[0].Rows[0]["BANKADD2"].ToString();
                    bdetails.Address3 = ds.Tables[0].Rows[0]["BANKADD3"].ToString();
                    //bdetails.Rwid = ds.Tables[0].Rows[0]["ID"].ToString();
                    //BankDetail.Add(bdetails);
                }
                return Json(bdetails, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                TempData["AlertMessage"] = "" + ex + " Your operation was failed";
                RedirectToAction("BankAccountDetails", "Master");
            }

            return View();
        }

        //------------------------Bank Account Details End---------------------------------



        //------------------------------Account Head Detail START-----------------------------------

        [HttpGet]
        public ActionResult AccountHeadDetail()
        {
            string str3 = "";
            string str4 = "";
            string str5 = "";

            str3 = "SELECT PAR_CODE,PAR_NAME,GROUPCODE,GROUPLEVEL1 FROM " + dbuser + ".CUPARTYMST";
            str4 = "SELECT GROUPCD,GROUPDES FROM " + dbuser + ".GROUPMAST";
            str5 = "SELECT BRANCHIND,SUBBRANCHIND FROM " + dbuser + ".CUPARTYMST where branchind IS NOT NULL";
            List<AccountHeadDetail> bdetail = new List<AccountHeadDetail>();
            DataSet ds5 = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet(str4, Session["SelectedConn"].ToString());
            if (ds5.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds5.Tables[0].Rows)
                {
                    AccountHeadDetail sm = new AccountHeadDetail();
                    sm.Group = row["GROUPCD"].ToString();
                    sm.GroupDesc = row["GROUPDES"].ToString();
                    bdetail.Add(sm);
                }
            }
            Session["bdetail"] = bdetail;


            //Account head details data configured in session
            List<AccountHeadDetail> bdetails = new List<AccountHeadDetail>();
            DataSet ds4 = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet(str3, Session["SelectedConn"].ToString());
            if (ds4.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds4.Tables[0].Rows)
                {
                    AccountHeadDetail sm = new AccountHeadDetail();
                    sm.AccountCode = row["PAR_CODE"].ToString();
                    sm.AccountDesc = row["PAR_NAME"].ToString();
                    sm.Group = row["GROUPCODE"].ToString();
                    sm.GroupDesc = row["GROUPLEVEL1"].ToString();
                    bdetails.Add(sm);
                }
            }
            Session["bdetails"] = bdetails;

            List<AccountHeadDetail> bdetai = new List<AccountHeadDetail>();
            DataSet ds3 = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet(str5, Session["SelectedConn"].ToString());
            if (ds3.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds3.Tables[0].Rows)
                {
                    AccountHeadDetail sm = new AccountHeadDetail();
                    sm.Branch = row["BRANCHIND"].ToString();
                    sm.SubBranch = row["SUBBRANCHIND"].ToString();
                    bdetai.Add(sm);
                }
            }
            Session["bdetai"] = bdetai;
            AccountHeadDetail model = new AccountHeadDetail();
            if (model.AccountCode == null)
            {
                DataSet ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet("select PAR_CODE,PAR_NAME,GROUPLEVEL1,GROUPCODE,BRANCHIND,SUBBRANCHIND,GROUPLEVEL2,GROUPLEVEL3 from IFSC.CUPARTYMST ", Session["SelectedConn"].ToString());
                model.result = ds;
                return View(model);
            }


            return View(model);
        }
        [HttpPost]
        public ActionResult AccountHeadDetail(AccountHeadDetail model)
        {
            try
            {
                DataSet ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet("select PAR_CODE,PAR_NAME,GROUPLEVEL1,GROUPCODE,BRANCHIND,SUBBRANCHIND,GROUPLEVEL2,GROUPLEVEL3,CONTRACT FROM " + dbuser + ".CUPARTYMST where PAR_CODE='" + model.AccountCode + "'", Session["SelectedConn"].ToString());
                model.result = ds;

                string AccCode = model.AccountCode != null ? model.AccountCode.ToUpper() : "";
                string AccDesc = model.AccountDesc != null ? model.AccountDesc.ToUpper() : "";
                string group = model.Group != null ? model.Group.ToUpper() : "";
                string GroupDesc = model.GroupDesc != null ? model.GroupDesc.ToUpper() : "";
                string grouplvl2 = model.Grouplvl2 != null ? model.Grouplvl2.ToUpper() : "";
                string grouplvl3 = model.Grouplvl3 != null ? model.Grouplvl3.ToUpper() : "";
                string branch = model.Branch != null ? model.Branch.ToUpper() : "";
                string subbranch = model.SubBranch != null ? model.SubBranch.ToUpper() : "";
                string remark = model.Remarks != null ? model.Remarks.ToUpper() : "";

                if (ds.Tables[0].Rows.Count == 0)
                {


                    MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteNonQuery("INSERT INTO " + dbuser + " .CUPARTYMST(SUBBRANCHIND,PAR_CODE,PAR_NAME,GROUPLEVEL1,GROUPCODE,BRANCHIND,remarks,grouplevel2,grouplevel3,CONTRACT) VALUES ('" + subbranch + "', '" + AccCode + "', '" + AccDesc + "', '" + GroupDesc + "','" + group + "','" + branch + "','" + remark + "','" + grouplvl2 + "','" + grouplvl3 + "' ,'" + model.CONTRACT + "')", Session["SelectedConn"].ToString());
                    MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteNonQuery("INSERT INTO " + dbuser + " .CUPARTYMST_FIXES(exchange,party_cd,opbal) VALUES ('" + model.Exchange + "', '" + AccCode + "', '" + model.OpeningBal + "')", Session["SelectedConn"].ToString());
                    TempData["Message"] = "Data Saved Successfully";
                    return RedirectToAction("AccountHeadDetail", "Master", model);

                }
                else
                {
                    TempData["Message"] = "Data Already Exist";
                   
                    return RedirectToAction("AccountHeadDetail", "Master", model);
                }


            }

            catch (Exception ex)
            {
                TempData["AlertMessage"] = "" + ex + " Your operation was failed";
                return RedirectToAction("AccountHeadDetail", "Master", model);
            }

        }

        public ActionResult EditBankHeadDetails(AccountHeadDetail model, string submit)
        {
            switch (submit)
            {
                case "Update":
                    return (UpdateBankHeadDetails(model));
                case "Delete":
                    return (DeleteBankHeadDetails(model));

            }
            return View(model);
        }

        public ActionResult UpdateBankHeadDetails(AccountHeadDetail model)
        {

            MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteNonQuery("update " + dbuser + ".CUPARTYMST set SUBBRANCHIND='" + model.SubBranch.ToUpper() + "',par_name='" + model.AccountDesc.ToUpper() + "',grouplevel1='" + model.GroupDesc.ToUpper() + "', BRANCHIND='" + model.Branch.ToUpper() + "', remarks='" + model.Remarks.ToUpper() + "',grouplevel2='" + model.Grouplvl2.ToUpper() + "',grouplevel3='" + model.Grouplvl3.ToUpper() + "' where PAR_CODE='" + model.AccountCode + "' and GROUPCODE='" + model.Group + "'", Session["SelectedConn"].ToString());
            TempData["Message"] = "Data Updated Successfully";
            return RedirectToAction("AccountHeadDetail", "Master", model);
        }

        
        public ActionResult DeleteBankHeadDetails(AccountHeadDetail model)
        {

            MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteNonQuery("delete from  " + dbuser + ".CUPARTYMST where PAR_CODE='" + model.AccountCode + "' and GROUPCODE='" + model.Group + "'", Session["SelectedConn"].ToString());
            TempData["DeleteMessage"] = "Data Deleted Sucessfully";
            return RedirectToAction("AccountHeadDetail", "Master", model);
        }

        public ActionResult GetAccountHeadDetails(string id)
        {

            try
            {
                AccountHeadDetail bdetails = new AccountHeadDetail();
                DataSet ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet("SELECT SUBBRANCHIND,PAR_CODE,PAR_NAME,GROUPLEVEL1,GROUPCODE,BRANCHIND,GROUPLEVEL2,GROUPLEVEL3 FROM " + dbuser + ".CUPARTYMST where PAR_CODE='" + id + "'", Session["SelectedConn"].ToString());
                if (ds.Tables[0].Rows.Count != 0)
                {
                    bdetails.SubBranch = ds.Tables[0].Rows[0]["SUBBRANCHIND"].ToString();
                    bdetails.AccountCode = ds.Tables[0].Rows[0]["PAR_CODE"].ToString();
                    bdetails.AccountDesc = ds.Tables[0].Rows[0]["PAR_NAME"].ToString();
                    bdetails.Group = ds.Tables[0].Rows[0]["GROUPCODE"].ToString();
                    bdetails.GroupDesc = ds.Tables[0].Rows[0]["GROUPLEVEL1"].ToString();
                    bdetails.Grouplvl2 = ds.Tables[0].Rows[0]["GROUPLEVEL2"].ToString();
                    bdetails.Grouplvl3 = ds.Tables[0].Rows[0]["GROUPLEVEL3"].ToString();
                    bdetails.Branch = ds.Tables[0].Rows[0]["BRANCHIND"].ToString();
                    //bdetails.Remarks = ds.Tables[0].Rows[0]["remarks"].ToString();

                    //bdetails.Rwid = ds.Tables[0].Rows[0]["ID"].ToString();
                    //BankDetail.Add(bdetails);
                }
                return Json(bdetails, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                TempData["AlertMessage"] = "" + ex + " Your operation was failed";
                RedirectToAction("AccountHeadDetail", "Master");
            }

            return View();
        }
        //------------------------------Account Head Detail END-----------------------------------

        //----------------------TM Master Start---------------------------------------
        [HttpGet]
        public ActionResult TMMaster()
        {

            TMMaster model = new TMMaster();
            if (model.TMId == null)
            {
                DataSet ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet("SELECT TMID,CLIENTID,UCCODE,EXCHANGE FROM " + dbuser + ".TMMASTER", Session["SelectedConn"].ToString());
                model.result = ds;
                return View(model);
            }

            return View(model);
        }

        [HttpPost]
        public ActionResult EditTMMaster(TMMaster model, string submit)
        {
            switch (submit)
            {
                case "Delete":
                    return (DeleteTMMaster(model));

            }
            return View(model);
        }

        public ActionResult TMMaster(TMMaster model)
        {
            try
            {
                DataSet ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet("SELECT TMID,CLIENTID,UCCODE,EXCHANGE FROM " + dbuser + ".TMMASTER where TMID='" + model.TMId + "'", Session["SelectedConn"].ToString());
                model.result = ds;
                if (ds.Tables[0].Rows.Count == 0)
                {

                    MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteNonQuery("INSERT INTO " + dbuser + ".TMMASTER(TMID,CLIENTID,UCCODE,EXCHANGE) VALUES('" + model.TMId.ToUpper() + "', '" + model.ClientCodeFrom.ToUpper() + "', '" + model.UccCode.ToUpper() + "','" + model.Exchange.ToUpper() + "')", Session["SelectedConn"].ToString());
                    TempData["Message"] = "Data Saved Successfully";
                    return RedirectToAction("TMMaster", "Master", model);

                }
                else
                {
                    TempData["Message"] = "Data Already Found";
                   
                    return RedirectToAction("TMMaster", "Master", model);
                }
            }
            catch (Exception ex)
            {
                TempData["Message"] = "" + ex + " Your operation was failed";
                return RedirectToAction("TMMaster", "Master", model);
            }
        }

        public ActionResult UpdateTMMaster(TMMaster model)
        {

            MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteNonQuery("update " + dbuser + ".TMMASTER set  EXCHANGE='" + model.Exchange + "' ,UCCODE='" + model.UccCode.ToUpper() + "' where CLIENTID='" + model.ClientCodeFrom + "' and TMID='" + model.TMId.ToUpper() + "'", Session["SelectedConn"].ToString());
            TempData["Message"] = "Data Updated Successfully";
            return RedirectToAction("TMMaster", "Master", model);

        }
        public ActionResult DeleteTMMaster(TMMaster model)
        {

            MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteNonQuery("delete from  " + dbuser + ".TMMASTER where TMID='" + model.TMId + "'and CLIENTID='" + model.ClientCodeFrom + "'", Session["SelectedConn"].ToString());
            TempData["DeleteMessage"] = "Data Deleted Sucessfully";
            return RedirectToAction("TMMaster", "Master", model);

        }

        public ActionResult GetTMDetails(string id)
        {
            try
            {
                TMMaster tmd = new TMMaster();
                DataSet ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet("SELECT TMID,CLIENTID,UCCODE,EXCHANGE FROM " + dbuser + ".TMMASTER where TMID='" + id + "'", Session["SelectedConn"].ToString());
                if (ds.Tables[0].Rows.Count != 0)
                {

                    tmd.TMId = ds.Tables[0].Rows[0]["TMID"].ToString();
                    tmd.ClientCodeFrom = ds.Tables[0].Rows[0]["CLIENTID"].ToString();
                    tmd.UccCode = ds.Tables[0].Rows[0]["UCCODE"].ToString();
                    tmd.Exchange = ds.Tables[0].Rows[0]["EXCHANGE"].ToString();
                    //tmd.Rwid = ds.Tables[0].Rows[0]["ID"].ToString();
                    TmDetails.Add(tmd);
                }
                return Json(tmd, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                TempData["Message"] = "" + ex + " Your operation was failed";
                RedirectToAction("TMMaster", "Master");
            }

            return View();
        }

        //----------------------TM Master End---------------------------------------


        //------------------------State Master Start---------------------------------

        [HttpGet]
        public ActionResult StateMaster()
        {
            string str2 = "";
            str2 = "SELECT * FROM  " + dbuser + ".statemst where CODE IS NOT NULL";
            List<StateMaster> StateDetails = new List<StateMaster>();
            DataSet ds2 = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet(str2, Session["SelectedConn"].ToString());
            if (ds2.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds2.Tables[0].Rows)
                {
                    StateMaster sm = new StateMaster();
                    sm.StateCode = row["CODE"].ToString();
                    sm.StateName = row["NAME"].ToString();
                    sm.UnionTerritory = row["UT"].ToString();
                    sm.StateId = row["STATEID"].ToString();
                    StateDetails.Add(sm);
                }
            }
            Session["StateDetails"] = StateDetails;
            StateMaster model = new StateMaster();
            if (model.StateName == null)
            {
                DataSet ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet("SELECT CODE,NAME,UT,StateId FROM " + dbuser + ".STATEMST where code is not null", Session["SelectedConn"].ToString());
                model.result = ds;
                return View(model);
            }


            return View(model);
        }

        [HttpPost]
        public ActionResult StateMaster(StateMaster model)
        {

            try
            {
                DataSet ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet("SELECT STATEID,NAME FROM " + dbuser + ".STATEMST where STATEID='" + model.StateId + "'", Session["SelectedConn"].ToString());
                model.result = ds;


                string stateCode = model.StateCode != null ? model.StateCode.ToUpper() : "";
                string stateName = model.StateName != null ? model.StateName.ToUpper() : "";
                string unionTerritory = model.UnionTerritory != null ? model.UnionTerritory : "";

                if (ds.Tables[0].Rows.Count == 0)
                {

                    MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteNonQuery("INSERT INTO " + dbuser + ".STATEMST( CODE,NAME,UT,STATEID ) VALUES('" + stateCode + "', '" + stateName + "', '" + unionTerritory + "', '" + model.StateId + "')", Session["SelectedConn"].ToString());
                    TempData["Message"] = "Data Saved Successfully";
                    return RedirectToAction("StateMaster", "Master", model);

                }
                else
                {

                    TempData["Message"] = "Data Already Exist";
                   
                    return RedirectToAction("StateMaster", "Master", model);
                }


            }

            catch (Exception ex)
            {
                TempData["AlertMessage"] = "" + ex + " Your operation was failed";
                return RedirectToAction("StateMaster", "Master", model);
            }
            return View(model);
        }


        public ActionResult StateMasteredit(StateMaster model, string submit)
        {
            switch (submit)
            {
                case "Update":
                    return (EditStateMaster(model));
                case "Delete":
                    return (DeleteStateMaster(model));

            }

            return View(model);
        }
        public ActionResult EditStateMaster(StateMaster model)
        {
            DataSet ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet("SELECT  STATEID,NAME FROM " + dbuser + ".STATEMST where STATEID='" + model.StateId + "'", Session["SelectedConn"].ToString());
            model.result = ds;
            if (ds.Tables[0].Rows.Count != 0)
            {
                MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteNonQuery("update " + dbuser + ".STATEMST set NAME='" + model.StateName.ToUpper() + "',UT='" + model.UnionTerritory + "',Code='" + model.StateCode + "' where STATEID='" + model.StateId + "'", Session["SelectedConn"].ToString());
                TempData["Message"] = "Data Updated Successfully";
                return RedirectToAction("StateMaster", "Master", model);
            }

            return View(model);
        }
        public ActionResult DeleteStateMaster(StateMaster model)
        {
            DataSet ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet("SELECT  STATEID,NAME FROM " + dbuser + ".STATEMST where STATEID='" + model.StateId + "' AND NAME='" + model.StateName + "'", Session["SelectedConn"].ToString());
            model.result = ds;
            if (ds.Tables[0].Rows.Count != 0)
            {
                MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteNonQuery("delete from  " + dbuser + ".STATEMST where STATEID='" + model.StateId + "' AND NAME='" + model.StateName + "' ", Session["SelectedConn"].ToString());
                TempData["DeleteMessage"] = "Data Deleted Sucessfully";
                return RedirectToAction("StateMaster", "Master", model);
            }

            return View(model);
        }

        public ActionResult GetStateDetails(string rid)
        {


            try
            {
                StateMaster state = new StateMaster();
                DataSet ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet("SELECT STATEID,NAME,CODE,UT FROM " + dbuser + ".STATEMST where STATEID='" + rid + "'", Session["SelectedConn"].ToString());
                if (ds.Tables[0].Rows.Count != 0)
                {

                    state.StateCode = ds.Tables[0].Rows[0]["CODE"].ToString();
                    state.StateId = ds.Tables[0].Rows[0]["STATEID"].ToString();
                    state.UnionTerritory = ds.Tables[0].Rows[0]["UT"].ToString();
                    state.StateName = ds.Tables[0].Rows[0]["NAME"].ToString();
                    StateDetails.Add(state);
                }
                return Json(state, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                TempData["AlertMessage"] = "" + ex + " Your operation was failed";
                RedirectToAction("StateMaster", "Master");
            }

            return View();
        }
        //------------------------State Master End-----------------------------------------

        //------------------------Bank Master Start---------------------------------

        [HttpGet]
        public ActionResult BankMaster()
        {

            string str1 = "";
            str1 = "SELECT BANKNAME,IFSCCODE,MICR,PINCODE,STATECODE,CITY,ADD1,ADD2,ADD3 FROM (SELECT BANKNAME,IFSCCODE,MICR,PINCODE,STATECODE,CITY,ADD1,ADD2,ADD3 FROM  " + dbuser + ".BANKMASTER) WHERE ROWNUM <= 10000";
            List<BankMaster> BankDetails = new List<BankMaster>();
            DataSet ds1 = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet(str1, Session["SelectedConn"].ToString());
            if (ds1.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds1.Tables[0].Rows)
                {
                    BankMaster bm = new BankMaster();
                    bm.BankName = row["BANKNAME"].ToString();
                    bm.IFSC = row["IFSCCODE"].ToString();
                    bm.Micr = row["MICR"].ToString();
                    bm.pincode = row["PINCODE"].ToString();
                    bm.StateCode = row["STATECODE"].ToString();
                    bm.city = row["CITY"].ToString();
                    bm.Address1 = row["ADD1"].ToString();
                    bm.Address2 = row["ADD2"].ToString();
                    bm.Address3 = row["ADD3"].ToString();
                    BankDetails.Add(bm);
                }
            }
            Session["BankDetails"] = BankDetails;
            BankMaster model = new BankMaster();
            if (model.BankName == null)
            {
                
                DataSet ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet("SELECT BANKNAME,MICR,CITY,PINCODE,IFSCCODE,STATECODE,ADD1,ADD2,ADD3 FROM " + dbuser + ".BANKMASTER  WHERE ROWNUM <= 10000", Session["SelectedConn"].ToString());
                model.result = ds;
                return View(model);
            }
            return View(model);

        }

        [HttpPost]
        public ActionResult BankMaster(BankMaster model)
        {
            try
            {
                DataSet ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet("SELECT BANKNAME,MICR,CITY,PINCODE,IFSCCODE,STATECODE,ADD1,ADD2,ADD3 FROM " + dbuser + ".BANKMASTER where IFSCCODE='" + model.IFSC + "'", Session["SelectedConn"].ToString());
                model.result = ds;
                string BankName = model.BankName != null ? model.BankName.ToUpper() : "";
                string Address1 = model.Address1 != null ? model.Address1.ToUpper() : "";
                string Address2 = model.Address2 != null ? model.Address2.ToUpper() : "";
                string Address3 = model.Address3 != null ? model.Address3.ToUpper() : "";
                string Micr = model.Micr != null ? model.Micr.ToUpper() : "";
                string IFSC = model.IFSC != null ? model.IFSC.ToUpper() : "";
                string city = model.city != null ? model.city.ToUpper() : "";
                string pincode = model.pincode != null ? model.pincode.ToUpper() : "";
                string StateCode = model.StateCode != null ? model.StateCode.ToUpper() : "";

                if (ds.Tables[0].Rows.Count == 0)
                {

                    MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteNonQuery("INSERT INTO " + dbuser + ".BANKMASTER(BANKNAME,MICR,CITY,PINCODE,IFSCCODE,STATECODE,ADD1,ADD2,ADD3) VALUES( '" + BankName + "', '" + Micr + "','" + city + "', '" + pincode + "', '" + IFSC + "','" + StateCode + "', '" + Address1 + "', '" + Address2 + "', '" + Address3 + "')", Session["SelectedConn"].ToString());
                    TempData["Message"] = "Data Saved Successfully";
                    return RedirectToAction("BankMaster", "Master", model);

                }
                else
                {

                    TempData["Message"] = "Data Already Exist";
                    
                    return RedirectToAction("BankMaster", "Master", model);
                }
            }

            catch (Exception ex)
            {
                TempData["AlertMessage"] = "" + ex + " Your operation was failed";
                return RedirectToAction("BankMaster", "Master", model);
            }
            return View(model);
        }



        public ActionResult EditBankMaster(BankMaster model, string submit)
        {
            switch (submit)
            {
                case "Update":
                    return (UpdateBankMaster(model));
                case "Delete":
                    return (DeleteBankMaster(model));

            }
            return View(model);
        }


        public ActionResult UpdateBankMaster(BankMaster model)
        {

            DataSet ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet("SELECT BANKNAME,MICR,CITY,PINCODE,IFSCCODE,STATECODE,ADD1,ADD2,ADD3 FROM " + dbuser + ".BANKMASTER where IFSCCODE ='" + model.IFSC + "'", Session["SelectedConn"].ToString());
            model.result = ds;
            if (ds.Tables[0].Rows.Count != 0)
            {
                MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteNonQuery("update " + dbuser + ".BANKMASTER set " + "BANKNAME='" + (model.BankName != null ? model.BankName.ToUpper() : "") + "'," + "MICR='" + model.Micr + "'," + "PINCODE='" + model.pincode + "'," + "CITY='" + (model.city != null ? model.city.ToUpper() : "") + "'," + "STATECODE='" + model.StateCode + "'," + "ADD1='" + (model.Address1 != null ? model.Address1.ToUpper() : "") + "'," + "ADD2='" + (model.Address2 != null ? model.Address2.ToUpper() : "") + "'," + "ADD3='" + (model.Address3 != null ? model.Address3.ToUpper() : "") + "' " + "where IFSCCODE='" + model.IFSC + "'", Session["SelectedConn"].ToString());
                TempData["Message"] = "Data Updated Successfully";
                return RedirectToAction("BankMaster", "Master", model);
            }
            return View(model);
        }
        

        public ActionResult DeleteBankMaster(BankMaster model)
        {
            DataSet ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet("SELECT BANKNAME,MICR,CITY,PINCODE,IFSCCODE,STATECODE,ADD1,ADD2,ADD3 FROM " + dbuser + ".BANKMASTER", Session["SelectedConn"].ToString());
            model.result = ds;
            if (ds.Tables[0].Rows.Count != 0)
            {

                MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteNonQuery("delete from  " + dbuser + ".BANKMASTER where IFSCCODE='" + model.IFSC + "'", Session["SelectedConn"].ToString());
                TempData["DeleteMessage"] = "Data Deleted Sucessfully";
                return RedirectToAction("BankMaster", "Master", model);
            }
            return View(model);
        }

        public ActionResult GetBankDetails(BankMaster model, string rid)
        {
            try
            {
                BankMaster bank = new BankMaster();
                DataSet ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet("SELECT BANKNAME,MICR,CITY,PINCODE,IFSCCODE,STATECODE,ADD1,ADD2,ADD3 FROM " + dbuser + ".BANKMASTER where BANKNAME ='" + rid + "'", Session["SelectedConn"].ToString());
                if (ds.Tables[0].Rows.Count != 0)
                {
                    bank.IFSC = ds.Tables[0].Rows[0]["IFSCCODE"].ToString();
                    bank.BankName = ds.Tables[0].Rows[0]["BANKNAME"].ToString();
                    bank.Micr = ds.Tables[0].Rows[0]["MICR"].ToString();
                    bank.city = ds.Tables[0].Rows[0]["CITY"].ToString();
                    bank.pincode = ds.Tables[0].Rows[0]["PINCODE"].ToString();
                    bank.Address1 = ds.Tables[0].Rows[0]["ADD1"].ToString();
                    bank.Address2 = ds.Tables[0].Rows[0]["ADD2"].ToString();
                    bank.Address3 = ds.Tables[0].Rows[0]["ADD3"].ToString();
                    bank.StateCode = ds.Tables[0].Rows[0]["STATECODE"].ToString();

                    //bank.Rwid = ds.Tables[0].Rows[0]["ID"].ToString();
                    BankDetails.Add(bank);
                }
                return Json(bank, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                TempData["AlertMessage"] = "" + ex + " Your operation was failed";
                RedirectToAction("ContractSpecification", "Master");
            }

            return View();
        }
        //------------------------Bank Master end-----------------------------------------


        //-------------------------Brokrage master ---------------------------

        [HttpGet]
        public ActionResult BrokrageMaster()
        {
            return View();
        }


        //------------------Settlement Master--------------------------------------



        [HttpGet]
        public ActionResult SettlementMaster()
        {
            SettlementSchedule objuser = new SettlementSchedule();
            DataSet ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet("SELECT STN_NAME,STN_code FROM " + dbuser + ".STNMAST ", Session["SelectedConn"].ToString());


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
            DataSet ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet("SELECT STN_NAME,STN_code FROM " + dbuser + ".STNMAST ", Session["SelectedConn"].ToString());


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
            DataSet ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet("SELECT STATION,SETTTYPE,SETTDES,TOGAP,PAYINGAP,PAYOUTGAP,rowid ID FROM " + dbuser + ".settletype", Session["SelectedConn"].ToString());
            model.result = ds;
            return View(model);

        }

        [HttpPost]
        public ActionResult SettlementTypeEntry(SettlementSchedule model)
        {
            DataSet ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet("SELECT STATION,SETTTYPE,SETTDES,TOGAP,PAYINGAP,PAYOUTGAP,delvbrokapplied,rowid ID FROM " + dbuser + ".settletype  where setttype= '" + model.SettType + "' and station='" + model.StationName + "'", Session["SelectedConn"].ToString());
            model.result = ds;

            if (ds.Tables[0].Rows.Count != 0)
            {
                MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteNonQuery("Update " + dbuser + ".settletype set SETTTYPE= '" + model.SettType + "', SETTDES = '" + model.DescOfSettlement + "',TOGAP = '" + model.SettlementPeriodGap + "',PAYINGAP = '" + model.PayinGap + "',PAYOUTGAP = '" + model.PayoutGap + "' ,delvbrokapplied= '" + model.BrokrageDebitNote + "' where setttype= '" + model.SettType + "' and station='" + model.StationName + "'", Session["SelectedConn"].ToString());

            }
            else
            {
                MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteNonQuery("insert into  " + dbuser + ".Settletype (SETTTYPE,SETTDES,TOGAP,PAYINGAP,PAYOUTGAP,delvbrokapplied) select '" + model.SettType + "', '" + model.DescOfSettlement + "' ,'" + model.SettlementPeriodGap + "','" + model.PayinGap + "','" + model.PayoutGap + "','" + model.BrokrageDebitNote + "' FROM DUAL", Session["SelectedConn"].ToString());
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
                DataSet ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet("SELECT STATION,SETTTYPE,SETTDES,TOGAP,PAYINGAP,PAYOUTGAP,delvbrokapplied FROM " + dbuser + ".settletype where rowID='" + rid + "'", Session["SelectedConn"].ToString());
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
                MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteNonQuery("Truncate table  IFSC.settfile_temp", Session["SelectedConn"].ToString());
                bool status = MvcApplication.OracleDBHelperCore().LoaderHelper.runLoader("NSE2005", "sysadm.settfile_temp", newfileName, " Settlement_Type,Settlement_No,Trade_Start_Date,Trade_End_Date,Funds_Payin_Date,Funds_Payout_Date,Security_Payin_Date,Security_Payout_Date,Final_Obligation_Date,Settlement_Merge_Number,Settlement_active,Settlement_Special", "E");
                MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteNonQuery("insert into  IFSC.settfile (SETTNO,NSESETTNO,PERIODFROM,PERIODTO,payindate,payoutdate) select  'N'||SETTLEMENT_TYPE||SUBSTR(SettlemenT_NO,4,4),Settlement_No,Trade_Start_Date,Trade_End_Date,Funds_Payin_Date ,Funds_Payout_Date from sysadm.settfile_temp", Session["SelectedConn"].ToString());
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
            DataSet ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet("SELECT STN_NAME,STN_code FROM IFSC.STNMAST ", Session["SelectedConn"].ToString());


            Dictionary<string, string> stat = new Dictionary<string, string>();
            for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
            {
                stat[ds.Tables[0].Rows[i]["STN_code"].ToString()] = ds.Tables[0].Rows[i]["STN_NAME"].ToString();
            }
            ViewBag.States = stat;

            string set = model.StationName.Substring(0, 1) + model.SettType.Substring(0, 1) + model.SettNo;
            MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteNonQuery("insert into IFSC.SETTFILE (SETTNO,NSESETTNO,PERIODFROM,PERIODTO,PAYINDATE,PAYOUTDATE,DELVINDATE,DELVOUTDATE) SELECT '" + set + "','" + model.ExchSett + "', '" + model.PeriodFrom.ToString() + "', '" + model.PeriodTo.ToString() + "' ,'" + model.PayinDate.ToString() + "','" + model.PayoutDate.ToString() + "','" + model.PayinDate.ToString() + "','" + model.PayoutDate.ToString() + "','" + model.DelinDate.ToString() + "','" + model.DeloutDate.ToString() + "' FROM DUAL", Session["SelectedConn"].ToString());
            ModelState.Clear();
            return View();
        }

        //------for 2nd dropdown binding
        public ActionResult SettlementSechdulelist(string stncode)
        {
            DataSet ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet("SELECT SETTTYPE,SETTDES FROM IFSC.SETTLETYPE where STATION='" + stncode + "'", Session["SelectedConn"].ToString());
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
                DataSet ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet("select Settno, to_char(Periodfrom,'dd-mm-yyyy') Periodfrom,to_char(periodto,'dd-mm-yyyy')periodto,to_char(payindate,'dd-mm-yyyy')payindate,to_char(payoutdate,'dd-mm-yyyy' )payoutdate,to_char(DELVINDATE,'dd-mm-yyyy')DELVINDATE,to_char(DELVOUTDATE,'dd-mm-yyyy')DELVOUTDATE,Nsesettno from " + dbuser + ".settfile   where SETTNO='" + Exchcode + "'", Session["SelectedConn"].ToString());
                if (ds.Tables[0].Rows.Count != 0)
                {

                    bnk.SettNo = ds.Tables[0].Rows[0]["SettNo"].ToString();
                    bnk.PeriodFrom = DateTime.Parse(ds.Tables[0].Rows[0]["Periodfrom"].ToString());
                    bnk.PeriodTo = DateTime.Parse(ds.Tables[0].Rows[0]["periodto"].ToString());
                    bnk.PayinDate = DateTime.Parse(ds.Tables[0].Rows[0]["payindate"].ToString());
                    bnk.PayoutDate = DateTime.Parse(ds.Tables[0].Rows[0]["PayoutDate"].ToString());
                    bnk.DelinDate = DateTime.Parse(ds.Tables[0].Rows[0]["DELVINDATE"].ToString());
                    bnk.DeloutDate = DateTime.Parse(ds.Tables[0].Rows[0]["DELVOUTDATE"].ToString());
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


        //-----------Scrip Master---------------


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



        //-------------- Parameter------------------------------

        [HttpGet]
        public ActionResult Parameter()
        {
            return View();
        }


        //-------------- Tax Master------------------------------
        [HttpGet]
        public ActionResult TaxMaster()
        {
            TaxMaster model = new TaxMaster();
            model.DateFrom = DateTime.Parse(Session["FinYearFrom"].ToString());
            model.DateTo = DateTime.Parse(Session["FinYearTo"].ToString());
            return View(model);
        }


        //-------------- Account Group Transfer------------------------------

        [HttpGet]
        public ActionResult AccountTransfer()
        {
            AccountMaster model = new AccountMaster();
            if (model.AccountCode == null)
            {
                DataSet ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet("SELECT ROWID ID,par_name ,par_code,groupclient,groupcode,bankacno1,Bankname1 BankName,ITAXNO from " + dbuser + ".CUPARTYMST", Session["SelectedConn"].ToString());
                model.result = ds;

                DataSet ds1 = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet("SELECT DISTINCT ROWID ID,GROUPDES,GROUPCD,subgroup FROM IFSC.GROUPMAST WHERE SUBGROUP IS NULL ORDER BY GROUPCD,subgroup", Session["SelectedConn"].ToString());
                model.result1 = ds1;

                List<DBList> selectedDBLists = Session["SelectedDBLists"] as List<DBList>;
                List<Oracle.ManagedDataAccess.Client.OracleParameter> lst = new List<Oracle.ManagedDataAccess.Client.OracleParameter>();
                DataSet ds2 = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet("SELECT DISTINCT GROUPDES,GROUPCD FROM IFSC.GROUPMAST WHERE SUBGROUP IS NULL", Session["SelectedConn"].ToString());
                model.NewGroup = new List<SelectListItem>();
                model.NewGroup1 = new List<SelectListItem>();
                model.NewGroup1.Add(new SelectListItem { Text = "--Select Item--", Value = "0" });
                for (int i = 0; i < ds2.Tables[0].Rows.Count; i++)
                {
                    model.NewGroup.Add(new SelectListItem { Text = ds2.Tables[0].Rows[i][0].ToString(), Value = ds2.Tables[0].Rows[i][1].ToString() });
                }


             

            }

            return View(model);

        }

        public ActionResult GetAccountTransferDetails(string rid)
        {


            try
            {
                AccountMaster am = new AccountMaster();
                DataSet ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet("SELECT ROWID ID,par_name ,par_code,groupclient,groupcode,GROUPLEVEL1  from " + dbuser + ".CUPARTYMST where rowID='" + rid + "'", Session["SelectedConn"].ToString());
                if (ds.Tables[0].Rows.Count != 0)
                {
                    am.AccountCode = ds.Tables[0].Rows[0]["par_code"].ToString();
                    am.CodeName = ds.Tables[0].Rows[0]["par_name"].ToString();
                    am.ExistingGroup = ds.Tables[0].Rows[0]["GROUPCODE"].ToString();
                    am.ExistingGroup1 = ds.Tables[0].Rows[0]["GROUPLEVEL1"].ToString();
                    am.Rwid = ds.Tables[0].Rows[0]["ID"].ToString();
                    AccountDetails.Add(am);
                }
                return Json(am, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                TempData["AlertMessage"] = "" + ex + " Your operation was failed";
                RedirectToAction("AccountTransfer", "Master");
            }

            return View();
        }

        public ActionResult GetNewAccountDetails(string categoryId)
        {

            try
            {
                AccountMaster model = new AccountMaster();                       
                DataSet ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet("SELECT DISTINCT GROUPDES,SUBGROUP FROM IFSC.GROUPMAST WHERE  GROUPCD=" + categoryId + " AND  SUBGROUP IS NOT NULL", Session["SelectedConn"].ToString());
                model.NewGroup1 = new List<SelectListItem>();          
                model.NewGroup1.Add(new SelectListItem { Text = "--Select Item--", Value = "0" });
                for (int i = 0; i < ds.Tables[0].Rows.Count; i++)
                {
                   
                    model.NewGroup1.Add(new SelectListItem { Text = ds.Tables[0].Rows[i][0].ToString(), Value = ds.Tables[0].Rows[i][1].ToString() });
                }
               
                return Json(model, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                TempData["AlertMessage"] = "" + ex + " Your operation was failed";
                RedirectToAction("AccountTransfer", "Master");
            }

            return View();

        }



    }
}