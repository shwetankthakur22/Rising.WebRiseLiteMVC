using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;
using System.IO;
using Rising.OracleDBHelper;
using Rising.WebRise.Models;


namespace Rising.WebRise.Controllers
{
    public class ReportExportController : Controller
    {
        // GET: ReportExport
        public ActionResult Index()
        {
            return View();
        }

        public string reportHeader()
        {
            if (Session["WebUser"] != null)
            {
                WebUser webUser = Session["WebUser"] as WebUser;
                if (webUser.UserType == UserType.Client)
                {
                    StringBuilder sb1 = new StringBuilder();
                    sb1.Append(System.IO.File.ReadAllText(Server.MapPath("reportheaderformatClient.html")));
                    return sb1.ToString();
                }
                if (Session["ReportClientCode"].ToString() != webUser.UserID)
                {
                    StringBuilder sb1 = new StringBuilder();
                    sb1.Append(System.IO.File.ReadAllText(Server.MapPath("reportheaderformatClient.html")));
                    return sb1.ToString();
                }
            }

            StringBuilder sb = new StringBuilder();
            sb.Append(System.IO.File.ReadAllText(Server.MapPath("reportheaderformat.html")));
            return sb.ToString();
        }



        public string reportFooter()
        {
            StringBuilder sb = new StringBuilder();
            sb.Append(System.IO.File.ReadAllText(Server.MapPath("reportfooterformat.html")));
            return sb.ToString();


        }

        [HttpPost]
        [ValidateInput(false)]
        public FileResult ExportExcel(string GridHtml1)
        {

            string str = reportHeader();
            string str1 = reportFooter();
            str = str.Replace("~~CompanyName~~", Session["CompanyName"].ToString());
            str = str.Replace("~~CompanyAddress1~~", Session["CompanyAddress1"].ToString());
            str = str.Replace("~~CompanyAddress2~~", Session["CompanyAddress2"].ToString());
            str = str.Replace("~~CompanyAddress3~~", Session["CompanyAddress3"].ToString());
            str = str.Replace("~~CompanyAddress4~~", Session["CompanyAddress4"].ToString());
            str = str.Replace("~~ReportHeader1~~", Session["ReportHeader1"].ToString());
            str = str.Replace("~~ReportHeader2~~", Session["ReportHeader2"].ToString());
            str = str.Replace("~~ClientCode~~", Session["ReportClientCode"].ToString());
            str = str.Replace("~~ClientType~~", Session["ClientType"].ToString());
            str = str.Replace("~~ClientName~~", Session["ReportClientName"].ToString());
            str = str.Replace("~~CompanyPhoneNo~~", Session["CompanyPhoneNo"].ToString());
            str = str.Replace("~~SebiRegNo~~", Session["SebiRegNo"].ToString());
            str = str.Replace("~~ComplianceName~~", Session["ComplianceName"].ToString());
            str = str.Replace("~~ClientPanNo~~", Session["ReportClientPanNo"].ToString());
            str = str.Replace("~~PrintDate~~", System.DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));

            return File(Encoding.ASCII.GetBytes(str + GridHtml1 + str1), "application/vnd.ms-excel", Session["ClientCode"].ToString() + "_" + Session["ReportHeader1"].ToString() + "_" + Session["ReportHeader2"].ToString() + ".xls");
        }

        [HttpPost]
        [ValidateInput(false)]
        public FileResult ExportWord(string GridHtml2)
        {
            string str = reportHeader();
            string str1 = reportFooter();
            str = str.Replace("~~CompanyName~~", Session["CompanyName"].ToString());
            str = str.Replace("~~CompanyAddress1~~", Session["CompanyAddress1"].ToString());
            str = str.Replace("~~CompanyAddress2~~", Session["CompanyAddress2"].ToString());
            str = str.Replace("~~CompanyAddress3~~", Session["CompanyAddress3"].ToString());
            str = str.Replace("~~CompanyAddress4~~", Session["CompanyAddress4"].ToString());
            str = str.Replace("~~ReportHeader1~~", Session["ReportHeader1"].ToString());
            str = str.Replace("~~ReportHeader2~~", Session["ReportHeader2"].ToString());
            str = str.Replace("~~ClientCode~~", Session["ReportClientCode"].ToString());
            str = str.Replace("~~ClientType~~", Session["ClientType"].ToString());
            str = str.Replace("~~ClientName~~", Session["ReportClientName"].ToString());
            str = str.Replace("~~CompanyPhoneNo~~", Session["CompanyPhoneNo"].ToString());
            str = str.Replace("~~SebiRegNo~~", Session["SebiRegNo"].ToString());
            str = str.Replace("~~ComplianceName~~", Session["ComplianceName"].ToString());
            str = str.Replace("~~ClientPanNo~~", Session["ReportClientPanNo"].ToString());
            str = str.Replace("~~PrintDate~~", System.DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));

            return File(Encoding.ASCII.GetBytes(str + GridHtml2 + str1), "application/vnd.ms-word", Session["ClientCode"].ToString() + "_" + Session["ReportHeader1"].ToString() + "_" + Session["ReportHeader2"].ToString() + ".doc");
        }


        [HttpPost]
        [ValidateInput(false)]
        public FileResult ExportPdf(string GridHtml3)
        {
            using (MemoryStream stream = new System.IO.MemoryStream())
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(System.IO.File.ReadAllText(Server.MapPath("reportheaderformat_clientmaster.html")));
                string str = sb.ToString();

                StringBuilder sb1 = new StringBuilder();
                sb1.Append(System.IO.File.ReadAllText(Server.MapPath("reportfooterformat_clientmaster.html")));
                string str1 = sb1.ToString();


                StringReader sr = new StringReader(str + GridHtml3 + str1);
                Document pdfDoc = new Document(PageSize.A4.Rotate(), 25f, 25f, 25f, 25f);

                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);
                pdfDoc.Open();
                XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
                pdfDoc.Close();
                return File(stream.ToArray(), "application/pdf", Session["ClientCode"].ToString() + "_" + Session["ReportHeader1"].ToString() + "_" + Session["ReportHeader2"].ToString() + ".pdf");
            }
        }
        
        [HttpPost]
        [ValidateInput(false)]
        public FileResult ExportPdf_NoHeader_LandScape(string GridHtml3)
        {
            using (MemoryStream stream = new System.IO.MemoryStream())
            {
                StringBuilder sb = new StringBuilder();
                sb.Append(System.IO.File.ReadAllText(Server.MapPath("reportheaderformat_clientmaster.html")));
                string str = sb.ToString();

                StringBuilder sb1 = new StringBuilder();
                sb1.Append(System.IO.File.ReadAllText(Server.MapPath("reportfooterformat_clientmaster.html")));
                string str1 = sb1.ToString();


                StringReader sr = new StringReader(str + GridHtml3 + str1);
                Document pdfDoc = new Document(PageSize.A4.Rotate(), 10f, 10f, 10f, 10f);

                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);
                pdfDoc.Open();
                XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
                pdfDoc.Close();
                return File(stream.ToArray(), "application/pdf", Session["ClientCode"].ToString() + "_" + Session["ReportHeader1"].ToString() + "_" + Session["ReportHeader2"].ToString() + ".pdf");
            }
        }

        [HttpPost]
        [ValidateInput(false)]
        public ActionResult SendEmail(string GridHtml4)
        {
            using (MemoryStream stream = new System.IO.MemoryStream())
            {
                string str = reportHeader();
                string str1 = reportFooter();
                str = str.Replace("~~CompanyName~~", Session["CompanyName"].ToString());
                str = str.Replace("~~CompanyAddress1~~", Session["CompanyAddress1"].ToString());
                str = str.Replace("~~CompanyAddress2~~", Session["CompanyAddress2"].ToString());
                str = str.Replace("~~CompanyAddress3~~", Session["CompanyAddress3"].ToString());
                str = str.Replace("~~CompanyAddress4~~", Session["CompanyAddress4"].ToString());
                str = str.Replace("~~ReportHeader1~~", Session["ReportHeader1"].ToString());
                str = str.Replace("~~ReportHeader2~~", Session["ReportHeader2"].ToString());
                str = str.Replace("~~ClientCode~~", Session["ReportClientCode"].ToString());
                str = str.Replace("~~ClientType~~", Session["ClientType"].ToString());
                str = str.Replace("~~ClientName~~", Session["ReportClientName"].ToString());
                str = str.Replace("~~CompanyPhoneNo~~", Session["CompanyPhoneNo"].ToString());
                str = str.Replace("~~SebiRegNo~~", Session["SebiRegNo"].ToString());
                str = str.Replace("~~ComplianceName~~", Session["ComplianceName"].ToString());
                str = str.Replace("~~ClientPanNo~~", Session["ReportClientPanNo"].ToString());
                str = str.Replace("~~PrintDate~~", System.DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));

                //for sending Emails


                String Email = Common.getEmail(Session["ClientCode"].ToString());
                Common.getSMS_Email_Paramater();
                StringReader sr = new StringReader(str + GridHtml4 + str1);
                Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 20f, 10f);
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);
                pdfDoc.Open();
                XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
                pdfDoc.Close();
                string fname = Session["ClientCode"].ToString() + "_" + Session["ReportHeader1"].ToString() + "_" + Session["ReportHeader2"].ToString() + ".pdf";


                MemoryStream file = new MemoryStream(stream.ToArray());







                string pdf = Session["ClientCode"].ToString() + "_" + Session["ReportHeader1"].ToString() + "_" + Session["ReportHeader2"].ToString() + ".pdf";
                //string MailTo, string subject, string msg,string filepath
                Common.SendEmailReport(Email, "Report", "Please Find Attachment", file, fname);
                TempData["AlertMessage"] = "Report Send to your registered Email ID...";
                return RedirectToAction("Index", "ClientHome");
                //return Redirect(Request.UrlReferrer.PathAndQuery);
            }
        }


        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ExportDashReport(string GridHtml, string rtype)
        {
            using (MemoryStream stream = new System.IO.MemoryStream())
            {
                string str = reportHeader();
                string str1 = reportFooter();
                str = str.Replace("~~CompanyName~~", Session["CompanyName"].ToString());
                str = str.Replace("~~CompanyAddress1~~", Session["CompanyAddress1"].ToString());
                str = str.Replace("~~CompanyAddress2~~", Session["CompanyAddress2"].ToString());
                str = str.Replace("~~CompanyAddress3~~", Session["CompanyAddress3"].ToString());
                str = str.Replace("~~CompanyAddress4~~", Session["CompanyAddress4"].ToString());

                if (rtype.Contains("Ledger") == true)
                {
                    Session["ReportHeader1"] = "Client Financial Ledger";
                    Session["ReportHeader2"] = "Source - DashBoard";
                }
                if (rtype.Contains("NetPos") == true)
                {
                    Session["ReportHeader1"] = "Net Position";
                    Session["ReportHeader2"] = "Source - DashBoard";
                }
                if (rtype.Contains("Unsettled") == true)
                {
                    Session["ReportHeader1"] = "Unsettled Trade";
                    Session["ReportHeader2"] = "Source - DashBoard";
                }
                if (rtype.Contains("Holding") == true)
                {
                    Session["ReportHeader1"] = "Client Holding";
                    Session["ReportHeader2"] = "Source - DashBoard";
                }

                str = str.Replace("~~ReportHeader1~~", Session["ReportHeader1"].ToString());
                str = str.Replace("~~ReportHeader2~~", Session["ReportHeader2"].ToString());
                str = str.Replace("~~ClientCode~~", Session["ReportClientCode"].ToString());
                str = str.Replace("~~ClientType~~", Session["ClientType"].ToString());
                str = str.Replace("~~ClientName~~", Session["ReportClientName"].ToString());
                str = str.Replace("~~CompanyPhoneNo~~", Session["CompanyPhoneNo"].ToString());
                str = str.Replace("~~SebiRegNo~~", Session["SebiRegNo"].ToString());
                str = str.Replace("~~ComplianceName~~", Session["ComplianceName"].ToString());
                str = str.Replace("~~ClientPanNo~~", Session["ReportClientPanNo"].ToString());
                str = str.Replace("~~PrintDate~~", System.DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));


                if (rtype.Contains("Pdf") == true)
                {
                    StringReader sr = new StringReader(str + GridHtml + str1);
                    Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 20f, 10f);
                    PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);
                    pdfDoc.Open();
                    XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
                    pdfDoc.Close();
                    return File(stream.ToArray(), "application/pdf", Session["ClientCode"].ToString() + "_" + Session["ReportHeader1"].ToString() + "_" + Session["ReportHeader2"].ToString() + ".pdf");
                }
                if (rtype.Contains("Excel") == true)
                {
                    return File(Encoding.ASCII.GetBytes(str + GridHtml + str1), "application/vnd.ms-excel", Session["ClientCode"].ToString() + "_" + Session["ReportHeader1"].ToString() + "_" + Session["ReportHeader2"].ToString() + ".xls");
                }
                if (rtype.Contains("Word") == true)
                {
                    return File(Encoding.ASCII.GetBytes(str + GridHtml + str1), "application/vnd.ms-word", Session["ClientCode"].ToString() + "_" + Session["ReportHeader1"].ToString() + "_" + Session["ReportHeader2"].ToString() + ".doc");
                }


                //for sending Emails
                if (rtype.Contains("Email") == true)
                {
                    String Email = Common.getEmail(Session["ClientCode"].ToString());
                    Common.getSMS_Email_Paramater();
                    StringReader sr = new StringReader(str + GridHtml + str1);
                    Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 20f, 10f);
                    PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);
                    pdfDoc.Open();
                    XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
                    pdfDoc.Close();
                    string fname = Session["ClientCode"].ToString() + "_" + Session["ReportHeader1"].ToString() + "_" + Session["ReportHeader2"].ToString() + ".pdf";


                    MemoryStream file = new MemoryStream(stream.ToArray());







                    string pdf = Session["ClientCode"].ToString() + "_" + Session["ReportHeader1"].ToString() + "_" + Session["ReportHeader2"].ToString() + ".pdf";
                    //string MailTo, string subject, string msg,string filepath
                    Common.SendEmailReport(Email, "Report", "Please Find Attachment", file, fname);
                    TempData["AlertMessage"] = "Report Send to your registered Email ID...";
                    return Redirect(Request.UrlReferrer.PathAndQuery);

                    //return null;
                }


                return null;
            }
        }


        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ExportPrint(string GridHtml5)
        {

            string str = reportHeader();
            string str1 = reportFooter();
            str = str.Replace("~~CompanyName~~", Session["CompanyName"].ToString());
            str = str.Replace("~~CompanyAddress1~~", Session["CompanyAddress1"].ToString());
            str = str.Replace("~~CompanyAddress2~~", Session["CompanyAddress2"].ToString());
            str = str.Replace("~~CompanyAddress3~~", Session["CompanyAddress3"].ToString());
            str = str.Replace("~~CompanyAddress4~~", Session["CompanyAddress4"].ToString());
            str = str.Replace("~~ReportHeader1~~", Session["ReportHeader1"].ToString());
            str = str.Replace("~~ReportHeader2~~", Session["ReportHeader2"].ToString());
            str = str.Replace("~~ClientCode~~", Session["ReportClientCode"].ToString());
            str = str.Replace("~~ClientType~~", Session["ClientType"].ToString());
            str = str.Replace("~~ClientName~~", Session["ReportClientName"].ToString());
            str = str.Replace("~~CompanyPhoneNo~~", Session["CompanyPhoneNo"].ToString());
            str = str.Replace("~~SebiRegNo~~", Session["SebiRegNo"].ToString());
            str = str.Replace("~~ComplianceName~~", Session["ComplianceName"].ToString());
            str = str.Replace("~~ClientPanNo~~", Session["ReportClientPanNo"].ToString());
            str = str.Replace("~~PrintDate~~", System.DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));

            Document pdfDoc = new Document(PageSize.A4.Rotate(), 10f, 10f, 20f, 10f);
            // return RedirectToAction("ExportPrint", "ReportExport");


            return View(new HtmlString(str + GridHtml5 + str1));
        }


        [HttpPost]
        [ValidateInput(false)]
        public ActionResult ExportPrint2(string GridHtml5)
        {
            string str = reportHeader();
            string str1 = reportFooter();
            str = str.Replace("~~CompanyName~~", Session["CompanyName"].ToString());
            str = str.Replace("~~CompanyAddress1~~", Session["CompanyAddress1"].ToString());
            str = str.Replace("~~CompanyAddress2~~", Session["CompanyAddress2"].ToString());
            str = str.Replace("~~CompanyAddress3~~", Session["CompanyAddress3"].ToString());
            str = str.Replace("~~CompanyAddress4~~", Session["CompanyAddress4"].ToString());
            str = str.Replace("~~ReportHeader1~~", Session["ReportHeader1"].ToString());
            str = str.Replace("~~ReportHeader2~~", Session["ReportHeader2"].ToString());
            str = str.Replace("~~ClientCode~~", Session["ReportClientCode"].ToString());
            str = str.Replace("~~ClientType~~", Session["ClientType"].ToString());
            str = str.Replace("~~ClientName~~", Session["ReportClientName"].ToString());
            str = str.Replace("~~CompanyPhoneNo~~", Session["CompanyPhoneNo"].ToString());
            str = str.Replace("~~SebiRegNo~~", Session["SebiRegNo"].ToString());
            str = str.Replace("~~ComplianceName~~", Session["ComplianceName"].ToString());
            str = str.Replace("~~ClientPanNo~~", Session["ReportClientPanNo"].ToString());
            str = str.Replace("~~PrintDate~~", System.DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));
            Document pdfDoc = new Document(PageSize.A4.Rotate(), 10f, 10f, 20f, 10f);
            return View(new HtmlString(str + GridHtml5 + str1));

        }

    }

}
