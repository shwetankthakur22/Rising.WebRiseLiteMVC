using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Net.Mail;
using System.Net;
using System.Data;
using System.Net.Sockets;
using Rising.WebRise.Models;
using Rising.OracleDBHelper;
using System.Configuration;
using System.IO;
using System.Net.Mime;

namespace Rising.WebRise.Models
{
    public class Common
    {
        public static string SMSurl { get; set; }
        public static string E_PORT { get; set; }
        public static string E_HOST { get; set; }
        public static string E_PWD { get; set; }
        public static string E_USER { get; set; }
        public static string E_SSL { get; set; }
        public static string E_EMAIL { get; set; }
        public static string ConnectionString { get; set; }
        public static DateTime ExpireDate { get; set; }
        public static Boolean InitialFound { get; set; }

        public static Boolean email(string MailTo, string subject, string msg)
        {
            //try
            //{
            MailMessage mail = new MailMessage();
            SmtpClient smtpclient = new SmtpClient(E_HOST);
            NetworkCredential Credential = new NetworkCredential(E_USER, E_PWD);
            mail.To.Add(MailTo);
            mail.From = new MailAddress(E_EMAIL);
            mail.Subject = subject;
            mail.Body = msg;
            mail.IsBodyHtml = true;
            smtpclient.Credentials = Credential;
            if (E_SSL == "1") smtpclient.EnableSsl = true; else smtpclient.EnableSsl = false;
            if (E_PORT != "") smtpclient.Port = Convert.ToInt32(E_PORT);
            smtpclient.Send(mail);
           
            return true;
            //}
            //catch (Exception ex)
            //{
            //    return false;
            //}
        }

        public static Boolean SendEmailReport(string MailTo, string subject, string msg, MemoryStream file, string fname)
        {
            //try
            //{
            MailMessage mail = new MailMessage();
            SmtpClient smtpclient = new SmtpClient(E_HOST);
            NetworkCredential Credential = new NetworkCredential(E_USER, E_PWD);



            file.Seek(0, SeekOrigin.Begin);
            Attachment data = new Attachment(file, fname, "application/pdf");
            ContentDisposition disposition = data.ContentDisposition;
            disposition.CreationDate = System.DateTime.Now;
            disposition.ModificationDate = System.DateTime.Now;
            disposition.DispositionType = DispositionTypeNames.Attachment;
            //Attachment atc = new Attachment(filepath);
            mail.To.Add(MailTo);
            mail.From = new MailAddress(E_EMAIL);
            mail.Subject = subject;
            mail.Body = msg;
            mail.IsBodyHtml = true;
            mail.Attachments.Add(data);
            smtpclient.Credentials = Credential;
            if (E_SSL == "1") smtpclient.EnableSsl = true; else smtpclient.EnableSsl = false;
            if (E_PORT != "") smtpclient.Port = Convert.ToInt32(E_PORT);
            smtpclient.Send(mail);

            return true;
            
           
            //}
            //catch (Exception ex)
            //{
            //    return false;
            //}
        }
        public static string getEmail(string code)
        {
            string email = "";
            DataSet ds = MvcApplication.OracleDBHelperCore().CustomHelper.CustomDataSet("SELECT * FROM SYSADM.WEBUSER WHERE USERID='" + code + "'", "MainConn");
            email = ds.Tables[0].Rows[0]["EMAILID"].ToString();

            return email;
        }

        public static Boolean SMS(string Msg)
        {
            HttpWebRequest request;
            HttpWebResponse response;
            string responseString = "";
            //try
            //{
            request = (HttpWebRequest)WebRequest.Create(Msg);
            response = (HttpWebResponse)request.GetResponse();
            System.IO.StreamReader respStreamReader = new System.IO.StreamReader(response.GetResponseStream());
            responseString = respStreamReader.ReadToEnd();
            respStreamReader.Close();
            response.Close();            
            return true;
            //}
            //catch (Exception ex)
            //{
            //    return false;
            //}
        }

        public static void getSMS_Email_Paramater()
        {
            try
            {
                Common.E_PORT = ConfigurationManager.AppSettings["E_PORT"];
                Common.E_HOST = ConfigurationManager.AppSettings["E_HOST"];
                Common.E_PWD = ConfigurationManager.AppSettings["E_PWD"];
                Common.E_USER = ConfigurationManager.AppSettings["E_USER"];
                Common.E_SSL = ConfigurationManager.AppSettings["E_SSL"];
                Common.E_EMAIL = ConfigurationManager.AppSettings["E_EMAIL"];

                DataSet ds = MvcApplication.OracleDBHelperCore().CustomHelper.CustomDataSet("SELECT * FROM SYSADM.NN_API WHERE APINAME='"+ ConfigurationManager.AppSettings["SMSAPINAME"] + "'", "MainConn");      
                Common.SMSurl = ds.Tables[0].Rows[0]["COL1"].ToString();
            }
            catch (Exception ex)
            {
            }

           
        }
    }
}