using Rising.WebRise.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;

//namespace Rising.WebRise.Controllers
//{
//    public class ImportFileController : Controller
//    {
       
//        // GET: ImportFile
//        //public ActionResult RateFileImport(ImportFileInput model, HttpPostedFileBase TradeFile)
//        //{
//        //    try
//        //    {
//        //        string qry;
//        //        DataSet ds = new DataSet();
                
                
                
//        //        //For NSE Exchange
//        //        if(model.Exchange == "NSE")
//        //        {
//        //            string dbname = MvcApplication.OracleDBHelperCore().OracleDBManager.getDBName("MainConn");
//        //            qry = "Select * From dba_tables where table_name ='NSE_RATE_TEMP_TABLE'";
//        //            ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet(qry, Session["SelectedConn"].ToString());
//        //            if (ds.Tables[0].Rows.Count == 0)
//        //            {
//        //                ds = null;
//        //                qry = "CREATE TABLE " + dbname + ".NSE_RATE_TEMP_TABLE(RDATE date, INSTRUMENT_TYPE varchar2(50), SYMBOL varchar2(50),EXPIRY_DATE date,STRIKE varchar2(50),OPTION_TYPE varchar2(50), SETTLEMENT_PRICE varchar2(50))";
//        //                ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet(qry, Session["SelectedConn"].ToString());
//        //            }
//        //            else
//        //            {
//        //                ds = null;
//        //                qry = "TRUNCATE TABLE " + dbname + ".NSE_RATE_TEMP_TABLE";
//        //                ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet(qry, Session["SelectedConn"].ToString());
//        //            }

//        //            string filename = TradeFile.FileName;
//        //            var fileName = System.IO.Path.GetFileName(filename);
//        //            var extn = System.IO.Path.GetExtension(filename);


//        //            // Create the directory if it doesn't exist
//        //            string uploadedFiles = Server.MapPath("~/UploadedFiles");
//        //            if (!System.IO.Directory.Exists(uploadedFiles))
//        //            {
//        //                System.IO.Directory.CreateDirectory(uploadedFiles);
//        //            }

//        //            string relativePath = "~/UploadedFiles/";
//        //            string newfileName = System.IO.Path.Combine(Server.MapPath(relativePath), Guid.NewGuid().ToString() + "." + extn);
//        //            TradeFile.SaveAs(newfileName);

//        //            // Create the "Logs" directory if it doesn't exist
//        //            string logsDirectoryPath = Server.MapPath("~/Logs");
//        //            if (!System.IO.Directory.Exists(logsDirectoryPath))
//        //            {
//        //                System.IO.Directory.CreateDirectory(logsDirectoryPath);
//        //            }

//        //            // Log a message before running the loader
//        //            string logFilePath = System.IO.Path.Combine(logsDirectoryPath, "LogFile.txt"); // Specifies the path for the log file
//        //            string logMessage = $"Starting file processing for {newfileName} at {DateTime.Now}";
//        //            System.IO.File.AppendAllText(logFilePath, logMessage + Environment.NewLine);

//        //            bool status = MvcApplication.OracleDBHelperCore().LoaderHelper.runLoader(dbname, "NSE_RATE_TEMP_TABLE", newfileName, "RDATE, INSTRUMENT_TYPE, SYMBOL,EXPIRY_DATE,STRIKE,OPTION_TYPE, SETTLEMENT_PRICE","E");

//        //            // Log the status of the loader
//        //            logMessage = $"Loader status for {newfileName}: {(status ? "Success" : "Failure")} at {DateTime.Now}";
//        //            System.IO.File.AppendAllText(logFilePath, logMessage + Environment.NewLine);

//        //            // Delete the file
//        //            System.IO.File.Delete(newfileName);


//        //            // Check if the log file size exceeds 1MB and delete it if necessary
//        //            long logFileSizeBytes = new System.IO.FileInfo(logFilePath).Length;
//        //            const long maxLogFileSizeBytes = 1024 * 1024; // 1MB

//        //            if (logFileSizeBytes > maxLogFileSizeBytes)
//        //            {
//        //                // Create a new log file
//        //                logFilePath = System.IO.Path.Combine(logsDirectoryPath, $"LogFile_{DateTime.Now:yyyyMMddHHmmss}.txt");

//        //                // Delete the log file
//        //                //System.IO.File.Delete(logFilePath);
//        //            }

//        //            Session["UploadedFilesPath"] = uploadedFiles;
//        //            Session["LogPath"] = logsDirectoryPath;


//        //            if (status == true)
//        //            {

//        //                qry = "Select * from " + dbname + ".CUDAILYPRICE where WDATE =to_date('" + model.TradeDate.ToString("dd-MM-yyyy")+"','dd-MM-yyyy') and Exchange = '"+model.Exchange+"' and SessionId = '"+model.Session+"'";
//        //                ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet(qry, Session["SelectedConn"].ToString());
//        //                if (ds.Tables[0].Rows.Count != 0)
//        //                {
                            
//        //                    ds = null;
//        //                    qry = "DELETE FROM " + dbname + ".CUDAILYPRICE where WDATE =to_date('" + model.TradeDate.ToString("dd-MM-yyyy") + "','dd-MM-yyyy') and Exchange = '" + model.Exchange + "' and SessionId = '" + model.Session + "'";
//        //                    ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet(qry, Session["SelectedConn"].ToString());
//        //                    qry = "INSERT INTO " + dbname + ".cudailyprice Select  RDATE WDATE,INSTRUMENT_TYPE || SYMBOL || to_char(EXPIRY_DATE,'DDMONYYYY') CONTNAME, STRIKE STRIKEPRICE, SETTLEMENT_PRICE CLOSPRICE,EXPIRY_DATE EXPIRYDATE,decode(OPTION_TYPE,'FF','FX',OPTION_TYPE) OPTIONTYPE,'" + model.Exchange + "'EXCHANGE,'' PRICEUNIT,'' UM,'" + model.Session + "' SESSIONID,'' SETTPRICE from " + dbname + ". NSE_RATE_TEMP_TABLE";
//        //                    ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet(qry, Session["SelectedConn"].ToString());
//        //                    var successMessage = new
//        //                    {
//        //                        title = "Override",
//        //                        text = "File overrided successfully!",
//        //                        icon = "success"
//        //                    };
//        //                    TempData["sweetAlertOptions"] = new JavaScriptSerializer().Serialize(successMessage);
//        //                }
//        //                else
//        //                {
//        //                    ds = null;
//        //                    qry = "INSERT INTO " + dbname + ".cudailyprice Select  RDATE WDATE,INSTRUMENT_TYPE || SYMBOL || to_char(EXPIRY_DATE,'DDMONYYYY') CONTNAME, STRIKE STRIKEPRICE, SETTLEMENT_PRICE CLOSPRICE,EXPIRY_DATE EXPIRYDATE,decode(OPTION_TYPE,'FF','FX',OPTION_TYPE) OPTIONTYPE,'" + model.Exchange + "'EXCHANGE,'' PRICEUNIT,'' UM,'" + model.Session + "' SESSIONID,'' SETTPRICE from " + dbname + ". NSE_RATE_TEMP_TABLE";
//        //                    ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet(qry, Session["SelectedConn"].ToString());
//        //                    var successMessage = new
//        //                    {
//        //                        title = "Success",
//        //                        text = "File imported successfully!",
//        //                        icon = "success"
//        //                    };
//        //                    TempData["sweetAlertOptions"] = new JavaScriptSerializer().Serialize(successMessage);
                            
//        //                }


                       
//        //            }
//        //            else
//        //            {
//        //                var errorMessage = new
//        //                {
//        //                    title = "Error",
//        //                    text = "File import failed!",
//        //                    icon = "error"
//        //                };
//        //                TempData["sweetAlertOptions"] = new JavaScriptSerializer().Serialize(errorMessage);

//        //            }

//        //        }



//        //        //For INX Exchange
//        //        else
//        //        {
//        //            string dbname = MvcApplication.OracleDBHelperCore().OracleDBManager.getDBName("MainConn");
//        //            qry = "Select * From dba_tables where table_name ='INX_RATE_TEMP_TABLE'";
//        //            ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet(qry, Session["SelectedConn"].ToString());
//        //            if (ds.Tables[0].Rows.Count == 0)
//        //            {
//        //                ds = null;
//        //                qry = "CREATE TABLE " + dbname + ".INX_RATE_TEMP_TABLE(RDATE date, INSTRUMENT_TYPE varchar2(50), SYMBOL varchar2(50),EXPIRY_DATE date,STRIKE varchar2(50),OPTION_TYPE varchar2(50), SETTLEMENT_PRICE varchar2(50), SessionId varchar2(50))";
//        //                ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet(qry, Session["SelectedConn"].ToString());
//        //            }
//        //            else
//        //            {
//        //                ds = null;
//        //                qry = "TRUNCATE TABLE " + dbname + ".INX_RATE_TEMP_TABLE";
//        //                ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet(qry, Session["SelectedConn"].ToString());
//        //            }
//        //            string filename = TradeFile.FileName;
//        //            var fileName = System.IO.Path.GetFileName(filename);
//        //            var extn = System.IO.Path.GetExtension(filename);


//        //            // Create the directory if it doesn't exist
//        //            string uploadedFiles = Server.MapPath("~/UploadedFiles");
//        //            if (!System.IO.Directory.Exists(uploadedFiles))
//        //            {
//        //                System.IO.Directory.CreateDirectory(uploadedFiles);
//        //            }

//        //            string relativePath = "~/UploadedFiles/";
//        //            string newfileName = System.IO.Path.Combine(Server.MapPath(relativePath), Guid.NewGuid().ToString() + "." + extn);
//        //            TradeFile.SaveAs(newfileName);

//        //            // Create the "Logs" directory if it doesn't exist
//        //            string logsDirectoryPath = Server.MapPath("~/Logs");
//        //            if (!System.IO.Directory.Exists(logsDirectoryPath))
//        //            {
//        //                System.IO.Directory.CreateDirectory(logsDirectoryPath);
//        //            }

//        //            // Log a message before running the loader
//        //            string logFilePath = System.IO.Path.Combine(logsDirectoryPath, "LogFile.txt"); // Specifies the path for the log file
//        //            string logMessage = $"Starting file processing for {newfileName} at {DateTime.Now}";
//        //            System.IO.File.AppendAllText(logFilePath, logMessage + Environment.NewLine);

//        //            bool status = MvcApplication.OracleDBHelperCore().LoaderHelper.runLoader(dbname, "NSE_RATE_TEMP_TABLE", newfileName, "RDATE, INSTRUMENT_TYPE, SYMBOL,EXPIRY_DATE,STRIKE,OPTION_TYPE, SETTLEMENT_PRICE", "E");

//        //            // Log the status of the loader
//        //            logMessage = $"Loader status for {newfileName}: {(status ? "Success" : "Failure")} at {DateTime.Now}";
//        //            System.IO.File.AppendAllText(logFilePath, logMessage + Environment.NewLine);

//        //            // Delete the file
//        //            System.IO.File.Delete(newfileName);


//        //            // Check if the log file size exceeds 1MB and delete it if necessary
//        //            long logFileSizeBytes = new System.IO.FileInfo(logFilePath).Length;
//        //            const long maxLogFileSizeBytes = 1024 * 1024; // 1MB

//        //            if (logFileSizeBytes > maxLogFileSizeBytes)
//        //            {
//        //                // Create a new log file
//        //                logFilePath = System.IO.Path.Combine(logsDirectoryPath, $"LogFile_{DateTime.Now:yyyyMMddHHmmss}.txt");

//        //                // Delete the log file
//        //                //System.IO.File.Delete(logFilePath);
//        //            }

//        //            Session["UploadedFilesPath"] = uploadedFiles;
//        //            Session["LogPath"] = logsDirectoryPath;



//        //            if (status == true)
//        //            {
//        //                qry = "Select * from " + dbname + ".CUDAILYPRICE where WDATE =to_date('" + model.TradeDate.ToString("dd-MM-yyyy") + "','dd-MM-yyyy') and Exchange = '" + model.Exchange + "' and SessionId = '" + model.Session + "'";
//        //                ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet(qry, Session["SelectedConn"].ToString());
//        //                if (ds.Tables[0].Rows.Count != 0)
//        //                {
//        //                    ds = null;
//        //                    qry = "DELETE FROM " + dbname + ".CUDAILYPRICE where WDATE =to_date('" + model.TradeDate.ToString("dd-MM-yyyy") + "','dd-MM-yyyy') and Exchange = '" + model.Exchange + "' and SessionId = '" + model.Session + "'";
//        //                    ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet(qry, Session["SelectedConn"].ToString());
//        //                    qry = "INSERT INTO " + dbname + ".cudailyprice Select  RDATE WDATE,INSTRUMENT_TYPE || SYMBOL || to_char(EXPIRY_DATE,'DDMONYYYY') CONTNAME, STRIKE STRIKEPRICE, SETTLEMENT_PRICE CLOSPRICE,EXPIRY_DATE EXPIRYDATE,decode(OPTION_TYPE,'FF','FX',OPTION_TYPE) OPTIONTYPE,'" + model.Exchange + "'EXCHANGE,'' PRICEUNIT,'' UM,'" + model.Session + "' SESSIONID,'' SETTPRICE from " + dbname + ". NSE_RATE_TEMP_TABLE";
//        //                    ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet(qry, Session["SelectedConn"].ToString());
//        //                }

//        //                else
//        //                {
//        //                    ds = null;
//        //                    qry = qry = "INSERT INTO " + dbname + ".cudailyprice Select  RDATE WDATE,INSTRUMENT_TYPE || SYMBOL || to_char(EXPIRY_DATE,'DDMONYYYY') CONTNAME, STRIKE STRIKEPRICE, SETTLEMENT_PRICE CLOSPRICE,EXPIRY_DATE EXPIRYDATE,decode(OPTION_TYPE,'FF','FX',OPTION_TYPE) OPTIONTYPE, '" + model.Exchange + "'EXCHANGE,'' PRICEUNIT,'' UM,'" + model.Session + "' SESSIONID,'' SETTPRICE from " + dbname + ". INX_RATE_TEMP_TABLE";
//        //                    ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet(qry, Session["SelectedConn"].ToString());
//        //                    var successMessage = new
//        //                    {
//        //                        title = "Success",
//        //                        text = "File imported successfully!",
//        //                        icon = "success"
//        //                    };
//        //                    TempData["sweetAlertOptions"] = new JavaScriptSerializer().Serialize(successMessage);

//        //                }
                        

//        //            }
//        //            else
//        //            {
//        //                var errorMessage = new
//        //                {
//        //                    title = "Error",
//        //                    text = "File import failed!",
//        //                    icon = "error"
//        //                };
//        //                TempData["sweetAlertOptions"] = new JavaScriptSerializer().Serialize(errorMessage);

//        //            }

//        //        }

                

//        //    }
//        //    catch (Exception ex)
//        //    {
//        //        Console.WriteLine("Error: " + ex.Message);
//        //        Console.WriteLine("StackTrace: " + ex.StackTrace);
//        //    }
//        //    return RedirectToAction("ClosingPriceImport", "Process");
//        //}
//    }
//}