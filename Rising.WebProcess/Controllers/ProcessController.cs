using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Web.Mvc;
using System.Diagnostics;
using System.Management;
using Rising.WebRise.Models;


namespace Rising.WebRise.Controllers
{
    public class ProcessController : Controller
    {
        // GET: Process
        public ActionResult TradeImport(ImportFileInput model)
        {
            return View(model);
        }


        public ActionResult TradeImportProcess(ImportFileInput model, HttpPostedFileBase TradeFile)
        {
            try
            {
                string filename = TradeFile.FileName;
                var fileName = System.IO.Path.GetFileName(filename);
                var extn = System.IO.Path.GetExtension(filename);
                string newfileName = "d:\\temp\\" + Guid.NewGuid().ToString() + "." + extn;
                TradeFile.SaveAs(newfileName);

                string dbname = MvcApplication.OracleDBHelperCore().OracleDBManager.getDBName("MainConn");
                bool status = MvcApplication.OracleDBHelperCore().LoaderHelper.runLoader(dbname, "sysadm.trnse", newfileName, "TRADENO, STATUS, SYMBOL, SERIES, SCRIPNAME, TEMP1 , TEMP2 , AUCT , USERID, TEMP4 , BUYSELLIND  , QTY, MKTRATE  , TEMP5 , CLIENTCD , TMID  , TYPE  , TEMP6 , TEMP7 , TRADETIME, TRADEDATE, ORDERNO  , TEMP8 , ORDERTIME, TRN_CTCLID");
            }
            catch (Exception ex)
            {
            }
            return View();
        }




        public ActionResult TradeImportFO(ImportFileInput model)
        {
            return View(model);
        }


        public ActionResult TradeImportProcessFO(ImportFileInput model, HttpPostedFileBase TradeFile)
        {
            try
            {
                string filename = TradeFile.FileName;
                var fileName = System.IO.Path.GetFileName(filename);
                var extn = System.IO.Path.GetExtension(filename);
                string newfileName = "d:\\temp\\" + Guid.NewGuid().ToString() + "." + extn;
                TradeFile.SaveAs(newfileName);

                string dbname = MvcApplication.OracleDBHelperCore().OracleDBManager.getDBName("MainConn");
                bool status = MvcApplication.OracleDBHelperCore().LoaderHelper.runLoader(dbname, "sysadm.trfo", newfileName, "TRADENO, TRSTATUS, SYMBOL, INSTRUMENT_TYPE, EXPIRYDATE, STRIKEPRICE, OPTIONTYPE, TEMP1 , TEMP2 , TEMP3 , USERID, TEMP4 , BUYSELLIND  , QTY, TRADEPRICE  , TEMP5 , CLIENTCODE , TMID  , TEMP6 , TEMP7 , TRADEDATE, ORDERNO  , TEMP8 , ORDERDATE, CTCLID");
            }
            catch (Exception ex)
            {
            }
            return View();
        }
    }
}