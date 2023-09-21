﻿using System;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web;
using System.Web.Mvc;
using System.Data;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.Owin;
using Microsoft.Owin.Security;
using System.Collections.Generic;
using System.Xml;

namespace Rising.WebRise.Controllers
{
    using Rising.WebRise.Models;
    using Rising.OracleDBHelper;
    using System.Configuration;

    [Authorize]
    public class LoginController : Controller
    {
        string dbuser = ConfigurationManager.AppSettings["DBUSER"];
        [AllowAnonymous]
        public ActionResult Index()
        {
            Session["SelectedConn"] = "MainConn";
            List<Oracle.ManagedDataAccess.Client.OracleParameter> lst = new List<Oracle.ManagedDataAccess.Client.OracleParameter>();
            lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("USER_", "IFSC", Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));

            DataSet ds = MvcApplication.OracleDBHelperCore().CustomHelper.CustomDataSet("" + dbuser + ".N$GET_CompanyDetails", lst, Session["SelectedConn"].ToString());
            Session["CompanyName"] = ds.Tables[0].Rows[0]["COMPANY"].ToString();
            Session["FinYearFrom"] = ds.Tables[0].Rows[0]["FINYRFROM"].ToString();
            Session["FinYearTo"] = ds.Tables[0].Rows[0]["FINYRTO"].ToString();
            Session["CompanyAddress1"] = ds.Tables[0].Rows[0]["ADD1"].ToString();
            Session["CompanyAddress2"] = ds.Tables[0].Rows[0]["ADD2"].ToString();
            Session["CompanyAddress3"] = ds.Tables[0].Rows[0]["ADD3"].ToString();
            Session["CompanyAddress4"] = ds.Tables[0].Rows[0]["ADD4"].ToString();
            
            Session["CompanyPhoneNo"] = ds.Tables[0].Rows[0]["PHNOS"].ToString();
            Session["SebiRegNo"] = ds.Tables[0].Rows[0]["FOSEBIREGNNO"].ToString();
            Session["ComplianceName"] = ds.Tables[0].Rows[0]["AUTHSIGN1"].ToString();

            return View();
        }


        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Login(LoginViewModel model)
        {
            if (ModelState.IsValid)
            {
                WebUser webUser;
                if (Session["WebUser"] == null) webUser = new WebUser(MvcApplication.__OracleDBHelperCore.OracleDBManager);
                else webUser = Session["WebUser"] as WebUser;
                //-----------inilialise webuser class
                //WebUser ws = MvcApplication.OracleDBHelperCore().LoginHelper.LoginHO(model.UserID, model.Password, Session["SelectedConn"].ToString());
                WebUser ws = MvcApplication.OracleDBHelperCore().LoginHelper.Login(model.UserID, model.Password, Session["SelectedConn"].ToString());

                if (ws != null)
                {
                    ws.OracleDBManager = MvcApplication.OracleDBHelperCore().OracleDBManager;
                    webUser = ws;
                    Session["WebUser"] = ws;
                    Session["ClientCode"] = ws.UserID;
                    Session["ClientName"] = ws.UserName;
                    
                    //------if userid, pasword is valid
                    if (ws.DisableStatus == true)
                    {
                        //------if user is disable
                        TempData["AlertMessage"] = "User Disable. Please Contact Admin...";
                        return RedirectToAction("Index", "Login");
                    }
                    else
                    {
                        //------if user is enable
                        if ((ws.MachineName == "" ? Environment.MachineName : ws.MachineName) == Environment.MachineName)
                        {
                            //-------if system name validated
                            if (ws.ResetStatus == true)
                            {
                                //---------if user reset by admin or user

                                MvcApplication.ViewDic["ResetReason"] = "AdminReset";
                                ws.LoginValidationStatus = false;
                                return RedirectToAction("Index", "ResetPassword");
                            }
                            else
                            {
                                //-----------check password laste change date
                                if (ws.Password1Date.AddDays(100) <= System.DateTime.Now)
                                {
                                    //required reset password
                                    MvcApplication.ViewDic["ResetReason"] = "PasswordExpired";
                                    ws.LoginValidationStatus = false;
                                    return RedirectToAction("Index", "ResetPassword");
                                }
                                else
                                {
                                    if (ws.AllowMultiLogin == false && ws.LoginStatus == true)
                                    {
                                        //-----------check multi login
                                        TempData["AlertMessage"] = "User Already Login. Multi Login not Allowed...";
                                        return RedirectToAction("Index", "Login");
                                    }
                                    else
                                    {
                                        ws.LoginValidationStatus = true;
                                        ws.LoginStatus = true;
                                        ws.SaveWebUser(Session["SelectedConn"].ToString());
                                        //-----loading menuitems on user login
                                        createExchanges();
                                        CreateMenuItems();
                                        //------create dblinks in dropdown
                                        MvcApplication.SelectedDBLists = new List<DBList>();

                                        CreateDBLinks();

                                        //NOT IN USE FOR HO
                                        CreateCodeSearchFilterType();
                                       
                                    }
                                }
                                return RedirectToAction("Index", "ClientHome");
                            }
                        }
                        else
                        {
                            TempData["AlertMessage"] = "Wrong System. Please Contact Admin...";
                            return RedirectToAction("Index", "Login");
                        }
                    }

                    //if (ws.LoginValidationStatus == true)
                    //{

                    //}
                    //else return RedirectToAction("Index", "Login");
                }
                else
                {
                    //------if userid, pasword is not valid
                    TempData["AlertMessage"] = "Wrong UserID or Password...";
                    return RedirectToAction("Index", "Login");
                }
            }
            return null;
        }

        public void createExchanges()
        {
            MvcApplication._Exchanges = new List<_Exchange>();
            MvcApplication._Exchanges.Add(new _Exchange("NSE", "NSECM", "CM"));
            MvcApplication._Exchanges.Add(new _Exchange("BSE", "BSECM", "CM"));
            MvcApplication._Exchanges.Add(new _Exchange("MCX", "MCXCM", "CM"));

            MvcApplication._Exchanges.Add(new _Exchange("NSE", "NSEFO", "FO"));
            MvcApplication._Exchanges.Add(new _Exchange("BSE", "BSEFO", "FO"));
            MvcApplication._Exchanges.Add(new _Exchange("MCX", "MCXFO", "FO"));

            MvcApplication._Exchanges.Add(new _Exchange("NSE", "NSECD", "CD"));
            MvcApplication._Exchanges.Add(new _Exchange("BSE", "BSECD", "CD"));
            MvcApplication._Exchanges.Add(new _Exchange("MCX", "MCXCD", "CD"));

            MvcApplication._Exchanges.Add(new _Exchange("NSE", "NSECO", "CO"));
            MvcApplication._Exchanges.Add(new _Exchange("BSE", "BSECO", "CO"));
            MvcApplication._Exchanges.Add(new _Exchange("MCX", "MCXCO", "CO"));
            MvcApplication._Exchanges.Add(new _Exchange("NCDEX", "NCDEXCO", "CO"));
            MvcApplication._Exchanges.Add(new _Exchange("ICEX", "ICEXCO", "CO"));
        }





        public void CreateMenuItems()
        {
            WebUser ws = Session["WebUser"] as WebUser;

            List<MenuItem> MenuItems = new List<MenuItem>();
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(Server.MapPath("../MenuItems") + "//MenuItems.xml");
            foreach (XmlNode xnode in xDoc.SelectNodes("Menus/MenuItem"))
            {
                MenuItem mnu = new MenuItem();
                mnu.Key = xnode.Attributes["Key"].Value; ;
                mnu.MenuName = xnode.Attributes["MenuName"].Value;
                mnu.ControllerName = xnode.Attributes["ControllerName"].Value;
                mnu.ActionName = xnode.Attributes["ActionName"].Value;
                mnu.Group = xnode.Attributes["Group"].Value;
                mnu.ParentMenuName = xnode.Attributes["ParentMenuName"].Value;
                mnu.Logo = xnode.Attributes["Logo"].Value;
                mnu.isView = bool.Parse(xnode.Attributes["isView"].Value);
                mnu.isEdit = bool.Parse(xnode.Attributes["isEdit"].Value);
                mnu.isDelete = bool.Parse(xnode.Attributes["isDelete"].Value);
                mnu.isExport = bool.Parse(xnode.Attributes["isExport"].Value);
                mnu.SubMenuItems = new List<MenuItem>();
                if (mnu.MenuName == "Admin" && ws.UserType != UserType.Admin)
                {
                }
                else
                {

                    MenuItems.Add(mnu);

                    foreach (XmlNode xnode1 in xnode.ChildNodes)
                    {
                        MenuItem mnu1 = new MenuItem();
                        mnu1.Key = xnode1.Attributes["Key"].Value; ;
                        mnu1.MenuName = xnode1.Attributes["MenuName"].Value;
                        mnu1.ControllerName = xnode1.Attributes["ControllerName"].Value;
                        mnu1.ActionName = xnode1.Attributes["ActionName"].Value;
                        mnu1.Logo = xnode.Attributes["Logo"].Value;
                        mnu1.isView = bool.Parse(xnode1.Attributes["isView"].Value);
                        mnu1.isEdit = bool.Parse(xnode1.Attributes["isEdit"].Value);
                        mnu1.isDelete = bool.Parse(xnode1.Attributes["isDelete"].Value);
                        mnu1.isExport = bool.Parse(xnode1.Attributes["isExport"].Value);
                        mnu1.SubMenuItems = new List<MenuItem>();
                        if (mnu1.MenuName == "Share Transfer Request" && ws.UserType != UserType.Branch)
                        {
                        }
                        else
                        {
                            mnu.SubMenuItems.Add(mnu1);

                            foreach (XmlNode xnode2 in xnode1.ChildNodes)
                            {
                                MenuItem mnu2 = new MenuItem();
                                mnu2.Key = xnode2.Attributes["Key"].Value; ;
                                mnu2.MenuName = xnode2.Attributes["MenuName"].Value;
                                mnu2.ControllerName = xnode2.Attributes["ControllerName"].Value;
                                mnu2.ActionName = xnode2.Attributes["ActionName"].Value;
                                mnu2.Logo = xnode.Attributes["Logo"].Value;
                                mnu2.isView = bool.Parse(xnode2.Attributes["isView"].Value);
                                mnu2.isEdit = bool.Parse(xnode2.Attributes["isEdit"].Value);
                                mnu2.isDelete = bool.Parse(xnode2.Attributes["isDelete"].Value);
                                mnu2.isExport = bool.Parse(xnode2.Attributes["isExport"].Value);
                                mnu2.SubMenuItems = new List<MenuItem>();
                                mnu1.SubMenuItems.Add(mnu2);
                            }
                        }
                    }
                }
            }
            MvcApplication.MenuItems = MenuItems;

        }

        public void CreateDBLinks()
        {
            List<MenuItem> MenuItems = new List<MenuItem>();
            XmlDocument xDoc = new XmlDocument();
            xDoc.Load(Server.MapPath("../MenuItems") + "//DBList.xml");
            MvcApplication.DBLists = new List<DBList>();

            List<DBList> selectedDBLists = new List<DBList>();
            foreach (XmlNode xnode in xDoc.SelectNodes("DB/DBItem"))
            {
                DBList DB = new DBList();
                DB.DBName = xnode.Attributes["DBName"].Value;
                DB.FinDBUser = xnode.Attributes["FinDBUser"].Value;
                DB.OtherDBUser = xnode.Attributes["OtherDBUser"].Value;
                DB.Exchange = xnode.Attributes["Exchange"].Value;
                DB.Group = xnode.Attributes["Group"].Value;
                DB.Visible = xnode.Attributes["Visible"].Value == "1" ? true : false;
                DB.CompGroup = xnode.Attributes["CompGroup"].Value;
                MvcApplication.DBLists.Add(DB);
            }

            Session["SelectedDBGroup"] = MvcApplication.DBLists[0].Group;

            if (selectedDBLists.Count() == 0)
            {
                foreach (DBList dbl in MvcApplication.DBLists)
                {
                    if (dbl.Group == MvcApplication.DBLists[0].Group) selectedDBLists.Add(dbl);
                }
                Session["SelectedDBLists"] = selectedDBLists; Session["CurrentDBUser"] = MvcApplication.DBLists[0].FinDBUser;
            }
        }

        public void CreateCodeSearchFilterType()
        {
            WebUser webUser = Session["WebUser"] as WebUser;
            List<string> lst = new List<string>();
            if (webUser.UserType == UserType.Admin)
            {
                lst.Add(UserType.Client.ToString());
                lst.Add(UserType.RM.ToString());
                lst.Add(UserType.Group.ToString());
                lst.Add(UserType.SubBranch.ToString());
                lst.Add(UserType.Branch.ToString());
                Session["ClientType"] = "Region";
            }
            if (webUser.UserType == UserType.Region)
            {
                lst.Add(UserType.Client.ToString());
                lst.Add(UserType.RM.ToString());
                lst.Add(UserType.Group.ToString());
                lst.Add(UserType.SubBranch.ToString());
                lst.Add(UserType.Branch.ToString());
                Session["ClientType"] = "Region";
            }
            if (webUser.UserType == UserType.HeadBranch)
            {
                lst.Add(UserType.Client.ToString());
                lst.Add(UserType.RM.ToString());
                lst.Add(UserType.Group.ToString());
                lst.Add(UserType.SubBranch.ToString());
                lst.Add(UserType.Branch.ToString());
                Session["ClientType"] = "HeadBranch";
            }

            if (webUser.UserType == UserType.SubBranch)
            {
                lst.Add(UserType.Client.ToString());
                lst.Add(UserType.RM.ToString());
                lst.Add(UserType.Group.ToString());
                Session["ClientType"] = "SubBranch";
            }
            if (webUser.UserType == UserType.Group)
            {
                lst.Add(UserType.Client.ToString());
                Session["ClientType"] = "Group";
            }
            if (webUser.UserType == UserType.RM)
            {
                lst.Add(UserType.Client.ToString());
                Session["ClientType"] = "RM";
            }
            if (webUser.UserType == UserType.Client)
            {
                Session["ClientType"] = "Client";
            }
            Session["CodeSearchFilterType"] = lst;


            string str = "";
            if (webUser.UserType == UserType.Region)
            {
                str = "SELECT  userid, username, mobileno FROM WEBUSER WHERE REGION='" + webUser.UserID + "' AND USERTYPE='Client' ";
            }
            if (webUser.UserType == UserType.HeadBranch)
            {
                str = "SELECT  userid, username, mobileno FROM WEBUSER WHERE BRANCH='" + webUser.UserID + "' AND USERTYPE='Client' ";
            }

            if (webUser.UserType == UserType.SubBranch)
            {
                str = "SELECT  userid, username, mobileno FROM WEBUSER WHERE SUBBRANCH='" + webUser.UserID + "' AND USERTYPE='Client' ";
            }
            if (webUser.UserType == UserType.Group)
            {
                str = "SELECT  userid, username, mobileno FROM WEBUSER WHERE GROUPCODE='" + webUser.UserID + "' AND USERTYPE='Client' ";
            }
            if (webUser.UserType == UserType.RM)
            {
                str = "SELECT  userid, username, mobileno FROM WEBUSER WHERE RMCODE='" + webUser.UserID + "' AND USERTYPE='Client' ";
            }
            if (webUser.UserType == UserType.Client)
            {
                str = "SELECT  userid, username, mobileno FROM WEBUSER WHERE USERID='" + webUser.UserID + "' AND USERTYPE='Client' ";
            }
            if (webUser.UserType == UserType.Admin || webUser.UserType == UserType.HO)
            {
                //str = "SELECT  userid, username, mobileno FROM WEBUSER ";
                str = "SELECT par_name,par_code,groupcode FROM " + dbuser + ".CUPARTYMST where PAR_CODE IS NOT NULL";
 
            }

            List<ClientDetail> ClientDetails = new List<ClientDetail>();
            DataSet ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet(str, Session["SelectedConn"].ToString());
            if (ds.Tables[0].Rows.Count > 0)
            {
                foreach (DataRow row in ds.Tables[0].Rows)
                {
                    ClientDetail cd = new ClientDetail();
                    cd.ClientCode = row["par_code"].ToString();
                    cd.ClientName = row["par_name"].ToString();
                    cd.ClientMobileNo = row["groupcode"].ToString();
                    ClientDetails.Add(cd);
                }
            }
            Session["ClientDetails"] = ClientDetails;
        }
    }
}