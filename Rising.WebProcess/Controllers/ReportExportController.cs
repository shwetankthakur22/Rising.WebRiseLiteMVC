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
            str = str.Replace("~~ReportHeader1~~", Session["ReportHeader1"].ToString());
            str = str.Replace("~~ReportHeader2~~", Session["ReportHeader2"].ToString());
            str = str.Replace("~~ClientCode~~", Session["ClientCode"].ToString());
            str = str.Replace("~~ClientType~~", Session["ClientType"].ToString());
            str = str.Replace("~~ClientName~~", Session["ClientName"].ToString());
            str = str.Replace("~~CompanyPhoneNo~~", Session["CompanyPhoneNo"].ToString());
            str = str.Replace("~~SebiRegNo~~", Session["SebiRegNo"].ToString());
            str = str.Replace("~~ComplianceName~~", Session["ComplianceName"].ToString());
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
            str = str.Replace("~~ReportHeader1~~", Session["ReportHeader1"].ToString());
            str = str.Replace("~~ReportHeader2~~", Session["ReportHeader2"].ToString());
            str = str.Replace("~~ClientCode~~", Session["ClientCode"].ToString());
            str = str.Replace("~~ClientType~~", Session["ClientType"].ToString());
            str = str.Replace("~~ClientName~~", Session["ClientName"].ToString());
            str = str.Replace("~~CompanyPhoneNo~~", Session["CompanyPhoneNo"].ToString());
            str = str.Replace("~~SebiRegNo~~", Session["SebiRegNo"].ToString());
            str = str.Replace("~~ComplianceName~~", Session["ComplianceName"].ToString());
            str = str.Replace("~~PrintDate~~", System.DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));

            return File(Encoding.ASCII.GetBytes(str + GridHtml2 + str1), "application/vnd.ms-word", Session["ClientCode"].ToString() + "_" + Session["ReportHeader1"].ToString() + "_" + Session["ReportHeader2"].ToString() + ".doc");
        }

        [HttpPost]
        [ValidateInput(false)]
        public FileResult ExportPdf(string GridHtml3)
        {
            using (MemoryStream stream = new System.IO.MemoryStream())
            {
                string str = reportHeader();
                string str1 = reportFooter();
                str = str.Replace("~~CompanyName~~", Session["CompanyName"].ToString());
                str = str.Replace("~~CompanyAddress1~~", Session["CompanyAddress1"].ToString());
                str = str.Replace("~~CompanyAddress2~~", Session["CompanyAddress2"].ToString());
                str = str.Replace("~~ReportHeader1~~", Session["ReportHeader1"].ToString());
                str = str.Replace("~~ReportHeader2~~", Session["ReportHeader2"].ToString());
                str = str.Replace("~~ClientCode~~", Session["ClientCode"].ToString());
                str = str.Replace("~~ClientType~~", Session["ClientType"].ToString());
                str = str.Replace("~~ClientName~~", Session["ClientName"].ToString());
                str = str.Replace("~~CompanyPhoneNo~~", Session["CompanyPhoneNo"].ToString());
                str = str.Replace("~~SebiRegNo~~", Session["SebiRegNo"].ToString());
                str = str.Replace("~~ComplianceName~~", Session["ComplianceName"].ToString());
                str = str.Replace("~~PrintDate~~", System.DateTime.Now.ToString("dd/MM/yyyy HH:mm:ss"));


                StringReader sr = new StringReader(str + GridHtml3 + str1);
                Document pdfDoc = new Document(PageSize.A4, 10f, 10f, 20f, 10f);
                PdfWriter writer = PdfWriter.GetInstance(pdfDoc, stream);
                pdfDoc.Open();
                XMLWorkerHelper.GetInstance().ParseXHtml(writer, pdfDoc, sr);
                pdfDoc.Close();
                return File(stream.ToArray(), "application/pdf", Session["ClientCode"].ToString() + "_" + Session["ReportHeader1"].ToString() + "_"+ Session["ReportHeader2"].ToString() + ".pdf");
            }
        }
        
    }
}