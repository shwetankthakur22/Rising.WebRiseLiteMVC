using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Text.RegularExpressions;
using System.Net;
using System.Net.Mail;

namespace Rising.WebRise.Controllers
{
    using Rising.WebRise.Models;
    using Rising.OracleDBHelper;
    public class ForgetPasswordController : Controller
    {
        // GET: ForgetPassword
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult GetWebUser(ForgetPasswordModel model)
        {
            //-----------inilialise webuser class
            WebUser ws = MvcApplication.OracleDBHelperCore().LoginHelper.GetHOUser(model.UserID, Session["SelectedConn"].ToString());

            if (ws != null)
            {
                ws.OracleDBManager = MvcApplication.OracleDBHelperCore().OracleDBManager;
                Session["WebUser"]  = ws;

                //------if userid, pasword is valid
                if (ws.DisableStatus == true)
                {
                    //------if user is disable
                    TempData["AlertMessage"] = "User Disable. Please Contact Admin...";
                    return RedirectToAction("Index", "ForgetPassword");
                }
                else
                {
                    string input = ws.EmailID;
                    string pattern = @"(?<=[\w]{1})[\w-\._\+%]*(?=[\w]{1}@)";
                    string result = Regex.Replace(input, pattern, m => new string('*', m.Length));

                    model.EmailID = result;
                    model.MobileNo = "*******" + ws.MobileNo.Substring(ws.MobileNo.Length - 3, 3);

                    ViewBag.Data = model;
                    return View();
                }
            }
            else
            {
                //------if user is disable
                TempData["AlertMessage"] = "Wrong UserID or Password...";
                return RedirectToAction("Index", "ForgetPassword");
            }
        }

        public ActionResult GetOTP(ForgetPasswordModel model)
        {
            WebUser webUser = Session["WebUser"] as WebUser;
            if (webUser.tempOTP == null)
            {
                Random rnd = new Random();
                int otp = rnd.Next(1000, 9999);
                otp = 1000;
                //------code for send sms

                ////-----code for send email
                //System.Net.Mail.MailMessage message = new System.Net.Mail.MailMessage();
                //message.To.Add(webUser.EmailID);
                //message.Subject = "CP1 Lab Password";
                //message.From = new System.Net.Mail.MailAddress("nandan.rising@gmail.com");
                //message.Body = "123";
                //System.Net.Mail.SmtpClient smtp = new System.Net.Mail.SmtpClient("smtp.gmail.com");

                //smtp.Host = "smtp.gmail.com";
                //smtp.Port = 587; //(465 for SSL)587 
                //smtp.EnableSsl = true;
                //smtp.Credentials = new System.Net.NetworkCredential("nandan.rising@gmail.com", "RsgNa@123");
                //smtp.Send(message);


                model.OTP = otp.ToString();
                model.UserID = webUser.UserID;
                ViewBag.Data = model;
               
                webUser.tempOTP = otp.ToString();
                return View();
            }
            model.UserID = webUser.UserID;
            ViewBag.Data = model;
            return View();
        }

        public ActionResult ValidateOTP(ForgetPasswordModel model)
        {
            if (model.OTP != null)
            {
                WebUser webUser = Session["WebUser"] as WebUser;
                if (webUser.tempOTP == model.OTP)
                {
                    model.UserID = webUser.UserID;
                    ViewBag.Data = model;

                    return View();
                }
                else
                {
                    TempData["AlertMessage"] = "Wrong OTP...";
                    model.UserID = webUser.UserID;
                    ViewBag.Data = model;
                    return RedirectToAction("GetOTP", "ForgetPassword", model);
                }
            }
            return null;            
        }

        public ActionResult Success(ForgetPasswordModel model)
        {
            WebUser webUser = Session["WebUser"] as WebUser;
            if (ModelState.IsValid)
            {
               
                if (webUser.Password1 == model.NewPassword || webUser.Password2 == model.NewPassword || webUser.Password3 == model.NewPassword)
                {
                    TempData["AlertMessage"] = "Password must not be same as last three password...";
                    model.OTP = model.UserID = webUser.tempOTP;
                    model.UserID = model.UserID = webUser.UserID;
                    return RedirectToAction("ValidateOTP", "ForgetPassword", model);
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
                    return View();
                }
            }
            model.OTP = model.UserID = webUser.tempOTP;
            model.UserID = model.UserID = webUser.UserID;
            return RedirectToAction("ValidateOTP", "ForgetPassword", model);
        }
    }
}