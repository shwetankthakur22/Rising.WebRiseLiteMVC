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
                DataSet ds = MvcApplication.OracleDBHelperCore().CustomHelper.CustomDataSet("SELECT * FROM SYSADM.MSGSETUP", "MainConn");
                Common.E_PORT = "26";//ds.Tables[0].Rows[0]["SMTPPORT"].ToString();
                Common.E_HOST = "mail.rkfml.in";//ds.Tables[0].Rows[0]["SMTPHOST"].ToString();
                Common.E_PWD =  "W3lc0me@123456789";//ds.Tables[0].Rows[0]["SMTPPWD"].ToString();
                Common.E_USER = "info@rkfml.in";//ds.Tables[0].Rows[0]["SMTPUSER"].ToString();//
                Common.E_SSL =  "0"; //ds.Tables[0].Rows[0]["SSL"].ToString();
                Common.E_EMAIL = "info@rkfml.in";// ds.Tables[0].Rows[0]["SMTPUSER"].ToString();

                ds = MvcApplication.OracleDBHelperCore().CustomHelper.CustomDataSet("SELECT * FROM SYSADM.NN_API WHERE APINAME='R.K.'", "MainConn");      
                Common.SMSurl = ds.Tables[0].Rows[0]["COL1"].ToString();
            }
            catch (Exception ex)
            {
            }

           
        }
    }
}