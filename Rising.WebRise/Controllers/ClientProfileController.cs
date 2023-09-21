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


    public class ClientProfileController : Controller
    {
        // GET: ClientProfile
        public ActionResult Index(CodeSearchFilter model)
        {
            WebUser webUser = Session["WebUser"] as WebUser;
            List<DBList> selectedDBLists = Session["SelectedDBLists"] as List<DBList>;

            if (webUser.UserType == UserType.Client)
            {
                List<Oracle.ManagedDataAccess.Client.OracleParameter> lst = new List<Oracle.ManagedDataAccess.Client.OracleParameter>();
                lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("CLIENTCODE_", webUser.UserID, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("USER_", (selectedDBLists.Count() == 0 ? "SYSADM" : selectedDBLists[0].OtherDBUser), Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));

                DataSet ds = MvcApplication.OracleDBHelperCore().CustomHelper.CustomDataSet("SYSADM.N$GET_CLIENTPROFILE", lst, Session["SelectedConn"].ToString());
                model.ds = ds;
            }
            else if (webUser.UserType == UserType.Branch)
            {
                if (model.ClientCodeFrom != null)
                {
                    DataSet ds1 = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet("SELECT * FROM sysadm.partymst where par_code='" + model.ClientCodeFrom + "' and branchind='" + webUser.UserID + "'", Session["SelectedConn"].ToString());
                    if (ds1.Tables[0].Rows.Count == 0)
                    {
                        TempData["AlertMessage"] = "Invalid Code : " + model.ClientCodeFrom;
                        return RedirectToAction("Index", "ClientProfile");
                    }

                    List<Oracle.ManagedDataAccess.Client.OracleParameter> lst = new List<Oracle.ManagedDataAccess.Client.OracleParameter>();
                    lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("CLIENTCODE_", model.ClientCodeFrom, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                    lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("USER_", (selectedDBLists.Count() == 0 ? "SYSADM" : selectedDBLists[0].OtherDBUser), Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));

                    DataSet ds = MvcApplication.OracleDBHelperCore().CustomHelper.CustomDataSet("SYSADM.N$GET_CLIENTPROFILE", lst, Session["SelectedConn"].ToString());
                    model.ds = ds;
                }
            }
            else if (webUser.UserType == UserType.Admin)
            {
                if (model.ClientCodeFrom != null)
                {
                    List<Oracle.ManagedDataAccess.Client.OracleParameter> lst = new List<Oracle.ManagedDataAccess.Client.OracleParameter>();
                    lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("CLIENTCODE_", model.ClientCodeFrom, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                    lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("USER_", (selectedDBLists.Count() == 0 ? "SYSADM" : selectedDBLists[0].OtherDBUser), Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));

                    DataSet ds = MvcApplication.OracleDBHelperCore().CustomHelper.CustomDataSet("SYSADM.N$GET_CLIENTPROFILE", lst, Session["SelectedConn"].ToString());
                    model.ds = ds;
                }
            }
            return View(model);
        }

        public ActionResult BrokrageDetails()
        {
            //try
            //{
                WebUser webUser = Session["WebUser"] as WebUser;
                List<DBList> selectedDBLists = Session["SelectedDBLists"] as List<DBList>;
                if (selectedDBLists.Count == 0)
                {
                    TempData["AlertMessage"] = "No Segment Selected...";
                    return RedirectToAction("Index", "ClientHome");
                }

                List<Oracle.ManagedDataAccess.Client.OracleParameter> lst = new List<Oracle.ManagedDataAccess.Client.OracleParameter>();
                lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("CLIENTCODE_", webUser.UserID, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                string exchanges = String.Join(",", selectedDBLists.Select(o => o.Exchange));
                lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("Exchanges_", exchanges, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                DataSet ds = MvcApplication.OracleDBHelperCore().CustomHelper.CustomDataSet("SYSADM.N$GET_BROKARAGE", lst, Session["SelectedConn"].ToString());
                ViewBag.DS = ds;
                return View(ViewBag);


            //}
            //catch (Exception ex)
            //{
            //    return View();
            //}
        }
    }
}