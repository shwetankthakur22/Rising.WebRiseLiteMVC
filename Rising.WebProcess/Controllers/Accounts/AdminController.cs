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
    public class AdminController : Controller
    {
        // GET: Admin
        public ActionResult Index(ClientCodeInput model)
        {
            try
            {
                WebUser webUser = Session["WebUser"] as WebUser;
                if (webUser.UserID.ToUpper() == "ADMIN")
                {
                    List<Oracle.ManagedDataAccess.Client.OracleParameter> lst = new List<Oracle.ManagedDataAccess.Client.OracleParameter>();
                    lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("ClientCode", model.UserID == null ? "XYZ" : model.UserID, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                    DataSet ds = MvcApplication.OracleDBHelperCore().CustomHelper.CustomDataSet("system.P$GET_ALLHOUSERS", lst, Session["SelectedConn"].ToString());
                    ViewBag.Dataset1 = ds.Tables[0];
                }
                return View();
            }
            catch 
            {
                return View();
            }
        }

        public ActionResult Edit(WebUserModel model)
        {
            try
            {
                WebUser webUser = Session["WebUser"] as WebUser;
                if (webUser.UserID.ToUpper() == "ADMIN")
                {

                    List<Oracle.ManagedDataAccess.Client.OracleParameter> lst = new List<Oracle.ManagedDataAccess.Client.OracleParameter>();
                    lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("ClientCode", RouteData.Values["id"] == null ? "XYZ" : RouteData.Values["id"], Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                    DataSet ds = MvcApplication.OracleDBHelperCore().CustomHelper.CustomDataSet("system.P$GET_ALLHOUSERS", lst, Session["SelectedConn"].ToString());
                    WebUserModel mdl = new WebUserModel();
                    mdl.UserID = ds.Tables[0].Rows[0]["UserID"].ToString();
                    mdl.UserName = ds.Tables[0].Rows[0]["UserName"].ToString();
                    mdl.EmailID = ds.Tables[0].Rows[0]["EmailID"].ToString();
                    mdl.MobileNo = ds.Tables[0].Rows[0]["MobileNo"].ToString();
                    mdl.DisableStatus = Convert.ToBoolean(Convert.ToInt16(ds.Tables[0].Rows[0]["DisableStatus"].ToString()));
                    DateTime dt; DateTime.TryParse(ds.Tables[0].Rows[0]["DisableDate"].ToString(), out dt);
                    mdl.DisableDate = dt;
                    mdl.AllowMultiLogin = Convert.ToBoolean(Convert.ToInt16(ds.Tables[0].Rows[0]["AllowMultiLogin"].ToString()));
                    mdl.LoginStatus = Convert.ToBoolean(Convert.ToInt16(ds.Tables[0].Rows[0]["LoginStatus"].ToString()));
                    mdl.UserType = (Rising.OracleDBHelper.UserType)Enum.Parse(typeof(Rising.OracleDBHelper.UserType), ds.Tables[0].Rows[0]["UserType"].ToString());
                    mdl.MachineName = ds.Tables[0].Rows[0]["MachineName"].ToString();
                    mdl.ResetPassword = EncryptDecrypt.Decrypt(ds.Tables[0].Rows[0]["ResetPassword"].ToString());
                    DateTime.TryParse(ds.Tables[0].Rows[0]["ResetDate"].ToString(), out dt);
                    mdl.ResetDate = dt;
                    mdl.ResetStatus = Convert.ToBoolean(Convert.ToInt16(ds.Tables[0].Rows[0]["ResetStatus"].ToString()));
                    mdl.RequiredPasswordPolicy = Convert.ToBoolean(Convert.ToInt16(ds.Tables[0].Rows[0]["RequiredPasswordPolicy"].ToString()));
                    mdl.UserRights = ds.Tables[0].Rows[0]["UserRights"].ToString();

                    ViewBag.WebUserModel = mdl;
                }
                return View();
            }
            catch
            {
                return View();
            }
        }

        public ActionResult Save(WebUserModel model)
        {
            WebUser webUser = Session["WebUser"] as WebUser;
            if (webUser.UserID.ToUpper() == "ADMIN")
            {
                WebUser ws = new WebUser(MvcApplication.__OracleDBHelperCore.OracleDBManager);
                ws.UserID = model.UserID;
                ws.DisableStatus = model.DisableStatus;
                if (model.DisableStatus == true) ws.DisableDate = System.DateTime.Now;
                else ws.DisableDate = model.DisableDate;
                ws.AllowMultiLogin = model.AllowMultiLogin;
                ws.LoginStatus = model.LoginStatus;
                ws.UserType = model.UserType;
                ws.MachineName = model.MachineName;
                ws.ResetPassword = model.ResetPassword;
                ws.ResetStatus = model.ResetStatus;
                if (model.ResetStatus == true) ws.ResetDate = System.DateTime.Now;
                else ws.ResetDate = model.ResetDate;
                ws.RequiredPasswordPolicy = model.RequiredPasswordPolicy;
                ws.UserRights = model.UserRights;
                ws.SaveWebUser(Session["SelectedConn"].ToString());
            }
            return View();
        }

        public ActionResult Update(WebUserModel model)
        {
            try
            {
                //WebUser webUser = Session["WebUser"] as WebUser;
                //if (webUser.UserID.ToUpper() == "ADMIN")
                //{

                //    List<Oracle.ManagedDataAccess.Client.OracleParameter> lst = new List<Oracle.ManagedDataAccess.Client.OracleParameter>();                    
                //    DataSet ds = MvcApplication.OracleDBHelperCore().CustomHelper.CustomDataSet("system.N$GET_UPDATEWEBUSERMASTER", lst, Session["SelectedConn"].ToString());
                    
                //}
                return View();
            }
            catch
            {
                return View();
            }
        }

        public ActionResult qquueerryy(admin_query aq)
        {
            if (aq == null)
            {
                aq = new admin_query();
                return View(aq);
            }

            //try
            //{
                if (aq.result == null)
                    aq.result = new DataSet();
                if (aq.queryKey == "NN" + System.DateTime.Now.ToString("ddMMyyyy"))
                {
                    if (aq.update == true)
                    {
                        MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteNonQuery(aq.query, Session["SelectedConn"].ToString());
                    }
                    else
                    {
                        aq.result = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet(aq.query, Session["SelectedConn"].ToString());
                    }
                }
                else
                {
                    TempData["AlertMessage"] = "Wrong Key...";
                }
                return View(aq);
            //}
            //catch
            //{
            //    return View(aq);
           // }
        }
    }
}