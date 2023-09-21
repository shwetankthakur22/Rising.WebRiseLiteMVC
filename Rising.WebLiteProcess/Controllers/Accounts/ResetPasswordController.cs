using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Microsoft.AspNet.Identity;
using Microsoft.AspNet.Identity.EntityFramework;

namespace Rising.WebRise.Controllers
{
    using Rising.WebRise.Models;
    using OracleDBHelper;
    public class ResetPasswordController : Controller
    {
        // GET: ResetPassword
        public ActionResult Index()
        {
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        [ValidateAntiForgeryToken]
        public ActionResult Index(ResetPasswordModel model)
        {
            WebUser webUser = Session["WebUser"] as WebUser;
            if (ModelState.IsValid)
            {
                if (MvcApplication.ViewDic["ResetReason"].ToString() == "AdminReset")
                {
                    if (webUser.ResetPassword != model.OldPassword)
                    {
                        TempData["AlertMessage"] = "Wrong old Password...";
                        return View();
                    }
                    else if (webUser.Password1 == model.NewPassword || webUser.Password2 == model.NewPassword || webUser.Password3 == model.NewPassword)
                    {
                        TempData["AlertMessage"] = "Password must not be same as last three password...";
                        return View();
                    }
                    else
                    {
                        webUser.Password3 = webUser.Password2;
                        webUser.Password2 = webUser.Password1;
                        webUser.Password1 = model.NewPassword;
                        webUser.ResetPassword = "";
                        webUser.ResetStatus = false;
                        webUser.Password1Date = System.DateTime.Now;
                        webUser.SaveWebUser(Session["SelectedConn"].ToString());
                        return RedirectToAction("Index", "Login");
                    }                    
                }
                else if (MvcApplication.ViewDic["ResetReason"].ToString() == "PasswordExpired")
                {
                    if (webUser.Password1 != model.OldPassword)
                    {
                        TempData["AlertMessage"] = "Wrong old Password...";
                        return View();
                    }
                    else if (webUser.Password1 == model.NewPassword || webUser.Password2 == model.NewPassword || webUser.Password3 == model.NewPassword)
                    {
                        TempData["AlertMessage"] = "Password must not be same as last three password...";
                        return View();
                    }
                    else
                    {
                        webUser.Password3 = webUser.Password2;
                        webUser.Password2 = webUser.Password1;
                        webUser.Password1 = model.NewPassword;
                        webUser.ResetPassword = "";
                        webUser.ResetStatus = false;
                        webUser.Password1Date = System.DateTime.Now;
                        webUser.SaveWebUser(Session["SelectedConn"].ToString());
                        return RedirectToAction("Index", "Login");
                    }
                }
                else
                {
                    return View();
                }
            }
            return View();
        }
    }
}