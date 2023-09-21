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
    public class ClientHomeController : Controller
    {
        // GET: ClientHome
        public ActionResult Index()
        {
            WebUser webUser = Session["WebUser"] as WebUser;
            if (webUser != null)
            {
                if (webUser.LoginValidationStatus == true)
                {
                    List<DBList> selectedDBLists = Session["SelectedDBLists"] as List<DBList>;
                    
                    return View();
                }
                else return RedirectToAction("Index", "Login");
            }
            else { return RedirectToAction("Index", "Login"); }
        }
       

        public ActionResult LogOut()
        {
            WebUser webUser = Session["WebUser"] as WebUser;
            if (webUser.LoginValidationStatus == true)
            {
                webUser.LoginValidationStatus = false;
                webUser.LoginStatus = false;
                webUser.SaveWebUser(Session["SelectedConn"].ToString());                
                return RedirectToAction("Index", "Login");
            }
            return RedirectToAction("Index", "Login");
        }

    

        [HttpPost]
        public ActionResult changeSegment(FormCollection formcollection)
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
                    var str = formcollection["DBUser"];
                    var str1 = str.Split(',');
                    List<DBList> tempDBLists = new List<DBList>();
                    List<DBList> selectedDBLists = Session["SelectedDBLists"] as List<DBList>;
                    foreach (DBList db in selectedDBLists) { tempDBLists.Add(db); }
                    selectedDBLists = new List<DBList>();

                    var distinct = tempDBLists[0].Group;

                    foreach (var str2 in str1)
                    {
                        if (str2 != "")
                        {
                            if (str2 == distinct)
                            { }
                            else
                            {
                                foreach (DBList dbl9 in MvcApplication.DBLists)
                                {
                                    if (dbl9.Group == str2)
                                    {
                                        selectedDBLists.Add(dbl9);
                                    }
                                }
                            }
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
                    List<Oracle.ManagedDataAccess.Client.OracleParameter> lst = new List<Oracle.ManagedDataAccess.Client.OracleParameter>();
                    lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("USER_", (selectedDBLists.Count() == 0 ? "SYSADM" : selectedDBLists[0].OtherDBUser), Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                    DataSet ds = MvcApplication.OracleDBHelperCore().CustomHelper.CustomDataSet("SYSADM.N$GET_CompanyDetails", lst, Session["SelectedConn"].ToString());

                    Session["CompanyName"] = ds.Tables[0].Rows[0][0].ToString();
                    Session["FinYearFrom"] = ds.Tables[0].Rows[0][1].ToString();
                    Session["FinYearTo"] = ds.Tables[0].Rows[0][2].ToString();


                    return Redirect(Request.UrlReferrer.ToString());
                }
                catch (Exception ex)
                {
                    TempData["AlertMessage"] = ex;
                    return Redirect(Request.UrlReferrer.ToString());
                }
            }
        }

        public ActionResult changeSegment1(string id)
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
                    string str2 = id.Replace("---", "&");

                    List<DBList> tempDBLists = new List<DBList>();
                    List<DBList> selectedDBLists = Session["SelectedDBLists"] as List<DBList>;
                    foreach (DBList db in selectedDBLists) { tempDBLists.Add(db); }
                    selectedDBLists = new List<DBList>();

                    var distinct = tempDBLists[0].Group;

                    foreach (DBList dbl9 in MvcApplication.DBLists)
                    {
                        if (dbl9.Group == str2)
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
                    List<Oracle.ManagedDataAccess.Client.OracleParameter> lst = new List<Oracle.ManagedDataAccess.Client.OracleParameter>();
                    lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("USER_", (selectedDBLists.Count() == 0 ? "SYSADM" : selectedDBLists[0].OtherDBUser), Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                    DataSet ds = MvcApplication.OracleDBHelperCore().CustomHelper.CustomDataSet("SYSADM.N$GET_CompanyDetails", lst, Session["SelectedConn"].ToString());

                    Session["CompanyName"] = ds.Tables[0].Rows[0][0].ToString();
                    Session["FinYearFrom"] = ds.Tables[0].Rows[0][1].ToString();
                    Session["FinYearTo"] = ds.Tables[0].Rows[0][2].ToString();

                    //return RedirectToAction("Index", "ClientHome");
                    return Redirect(Request.UrlReferrer.ToString());
                }
                catch (Exception ex)
                {
                    TempData["AlertMessage"] = ex;
                    return Redirect(Request.UrlReferrer.ToString());
                }


            }
        }
    }
}