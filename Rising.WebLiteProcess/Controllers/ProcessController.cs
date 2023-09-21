using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.IO;
using System.Web.Mvc;
using System.Diagnostics;
using System.Management;
using Rising.WebRise.Models;
using System.Data;
using System.Configuration;
using Rising.OracleDBHelper;
using System.Web.Script.Serialization;
using Rising.WebRise.Repositories.Implementation;
using Rising.WebRise.Models.Process;

namespace Rising.WebRise.Controllers
{
    public class ProcessController : Controller
    {
        string dbuser = ConfigurationManager.AppSettings["DBUSER"];
        // GET: Process
        //------------------------Trade Import Start---------------------------------

        [HttpGet]
        public ActionResult TradeImport(ImportFileInput model)
        {
            model.TradeDate = DateTime.Now;
            return View(model);
        }


        [HttpPost]
        public ActionResult TradeImport(ImportFileInput model, HttpPostedFileBase TradeFile)
        {
            try
            {
                string filename = TradeFile.FileName;
                var fileName = System.IO.Path.GetFileName(filename);
                var extn = System.IO.Path.GetExtension(filename);
                string newfileName = "E:\\temp\\" + Guid.NewGuid().ToString() + extn;
                TradeFile.SaveAs(newfileName);

                string dbname = MvcApplication.OracleDBHelperCore().OracleDBManager.getDBName("MainConn");
                if (model.Exchange == "INX")
                {
                    string qry = "Truncate table " + dbuser + ".TEMP_TRD_IMPORT";
                    DataSet ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet(qry, Session["SelectedConn"].ToString());
                    bool status = MvcApplication.OracleDBHelperCore().LoaderHelper.runLoader(dbname, "TEMP_TRD_IMPORT", newfileName, "TRADE_NO,TRADE_STATUS,TEMP3,INSTRUMENT_TYPE,SYMBOL,EXPIRY_DATE,TEMP7,STRIKE_PRICE,OPTION_TYPE,TEMP10,TEMP11,TEMP12,TEMP13,USER_ID,TEMP15,BUY_SELLIND,TEMP17,TRD_NETPRICE,TEMP19,ORG_CLIENTID,TEMP21,TEMP22,TMID,TEMP24,TRADE_TIME,TEMP26,ORDER_NO,TEMP28,TEMP29,ORDER_TIME,TEMP31,TEMP32,TEMP33,TEMP34,CTCLID,TEMP36,TEMP37", "E");
                    if (status == true)
                    {
                        string qry1 = "select NVL(count(*),'0') NO from TEMP_TRD_IMPORT";
                        DataSet dsqry = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet(qry1, Session["SelectedConn"].ToString());
                        model.Records = dsqry.Tables[0].Rows[0]["NO"].ToString();

                        DataSet ds1 = new DataSet();
                        string qry2 = "";
                        ds1 = null;
                        string tableName = "IN" + model.TradeDate.ToString("dd-MM-yyyy").Replace("-", "");
                        string query = "select table_name from dba_tables where table_name = '" + tableName + "'";
                        ds1 = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet(query, Session["SelectedConn"].ToString());
                        if (ds1.Tables[0].Rows.Count == 0)
                        {
                            string NTable = "create table " + tableName + " as select * from " + dbuser + ".CUTRNMAST where rownum<1";
                            ds1 = null;
                            ds1 = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet(NTable, Session["SelectedConn"].ToString());


                            NTable = null;
                            NTable = "create index " + tableName + "_idx on " + dbuser + "." + tableName + "(tradeno,orderno,trade_time,trn_qty)";
                            ds1 = null;
                            ds1 = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet(NTable, Session["SelectedConn"].ToString());


                        }
                        qry2 = "";
                        ds1 = null;
                        qry2 = "SELECT CONSTRAINT_NAME FROM DBA_CONSTRAINTS WHERE OWNER='" + tableName + "'";
                        ds1 = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet(qry2, Session["SelectedConn"].ToString());
                        if (ds1.Tables[0].Rows.Count != 0)
                        {
                            foreach (DataRow rdr in ds1.Tables[0].Rows)
                            {
                                ds1 = null;
                                qry2 = "ALTER TABLE " + dbuser + "." + tableName + " DROP CONSTRAINT " + rdr["CONSTRAINT_NAME"];
                                ds1 = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet(qry2, Session["SelectedConn"].ToString());
                            }
                        }
                        bool Val_Id = false;
                        qry2 = "";
                        ds1 = null;
                        qry2 = "select distinct branchcode from " + dbuser + ".CUBRANCHFILE where recordtype='A' and exchange='" + model.Exchange + "' ";
                        ds1 = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet(qry2, Session["SelectedConn"].ToString());
                        if (ds1.Tables[0].Rows.Count != 0) Val_Id = true;

                        bool Val_Pref = false;
                        qry2 = "";
                        ds1 = null;
                        qry2 = "select distinct branchcode from " + dbuser + ".CUBRANCHFILE where recordtype='V' and exchange='" + model.Exchange + "' ";
                        ds1 = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet(qry2, Session["SelectedConn"].ToString());
                        if (ds1.Tables[0].Rows.Count != 0) Val_Pref = true;

                        //-----------------------Checking Duplicated Trades--------------------------------

                        qry2 = "";
                        ds1 = null;
                        qry2 = "delete from " + dbuser + ".TEMP_TRD_IMPORT where (TO_CHAR(TRADE_NO),TO_CHAR(ORDER_NO),decode(BUY_SELLIND,'2',0-TEMP17,TEMP17),TO_CHAR(INSTRUMENT_TYPE))  in(select TO_CHAR(TRADE_NO),TO_CHAR(ORDER_NO),unit,instrument_type from " + dbuser + "." + tableName + " WHERE sessionid='" + model.Session + "')  ";
                        ds1 = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet(qry2, Session["SelectedConn"].ToString());

                        //------------------------Importing From File-----------------------------------------
                        qry2 = "";
                        ds1 = null;
                        qry2 = "insert into " + dbuser + "." + tableName + "(exchange,tradeno,trade_date,instrument_type,symbol,expirydate,strikeprice,optiontype,tradeprice,trade_time,trn_qty,branchid,userid,orderno,orgclientid, clientcode,securityname,booktype,booktypename,mkt_type,openclosflag,netprice, trn_slno,imp_flag,tradestatus,BROKFLAG,TMID,buysellind,trn_brok,ORDER_TIME,unit,ctclid,REVERSECODE,sessionid,ORDERDATE)  select '" + model.Exchange + "',TRADE_NO, to_date('" + model.TradeDate.ToString("ddMMMyyyy") + "')Trade_date,trim(INSTRUMENT_TYPE) instrument_type,trim(SYMBOL) symbol,  EXPIRY_DATE expirydate,nvl(STRIKE_PRICE,0),decode(trim(OPTION_TYPE),'XX','FX',trim(NVL(OPTION_TYPE,'FX'))) optiontype,TRD_NETPRICE,to_char( to_date(TRADE_TIME, 'dd-mon-yyyy hh24:mi:ss') ,'HH24:MI:SS') trade_time, decode(BUY_SELLIND,2,0-temp17,temp17),null,trim(USER_ID)userid,to_char(ORDER_NO) ordno,trim(ORG_CLIENTID) clientcode,null,trim(INSTRUMENT_TYPE)||trim(SYMBOL)||(EXPIRY_DATE) Securityname,  'RL','RL','01','OPEN',TRD_NETPRICE,null,null,to_char(TRADE_STATUS) trstatus,null,trim(TMID) tmid,to_char(BUY_SELLIND) buysellind,0,to_char( to_date(ORDER_TIME, 'dd-mon-yyyy hh24:mi:ss') ,'HH24:MI:SS') order_time,decode(BUY_SELLIND,2,0-temp17,temp17),to_char(CTCLID),'" + model.Imported + "','" + model.Session + "',to_char( to_date(ORDER_TIME, 'dd-mon-yyyy hh24:mi:ss') ,'DD-MON-YYYY') ORDER_Date  From " + dbuser + ".TEMP_TRD_IMPORT where trim(USER_ID) is not null ";


                        if (Val_Id == true)
                        {
                            qry2 = qry2 + "and trim(USER_ID) IN(select distinct ltrim(branchcode,'0') from " + dbuser + ".CUBRANCHFILE   where recordtype='A' and exchange='" + model.Exchange + "')";
                        }

                        if (Val_Pref == true)
                        {
                            qry2 = qry2 + "and trim(ORG_CLIENTID) IN(select distinct branchcode from " + dbuser + ".CUBRANCHFILE  where recordtype='V' and exchange='" + model.Exchange + "')";
                        }

                        ds1 = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet(qry2, Session["SelectedConn"].ToString());
                        TempData["Message"] = "" + model.Records + " Records Inserted Successfully";

                        qry2 = "";
                        ds1 = null;
                        qry2 = "UPDATE " + dbuser + "." + tableName + " I SET CLIENTCODE=(SELECT CLIENTID FROM " + dbuser + ".TMMASTER T  WHERE I.TMID=T.TMID AND EXCHANGE='" + model.Exchange + "' AND UCCODE IS NULL ) WHERE TMID!='" + model.memberid + "' AND I.sessionid='" + model.Session + "'";
                        ds1 = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet(qry2, Session["SelectedConn"].ToString());


                        qry2 = "";
                        ds1 = null;
                        qry2 = "select index_name from DBA_ind_columns where table_name ='" + tableName + "' and column_name='IMP_FLAG'";
                        ds1 = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet(qry2, Session["SelectedConn"].ToString());
                        if (ds1.Tables[0].Rows.Count == 0)
                        {
                            string dtable = null;
                            dtable = "create index " + dbuser + "." + tableName + "_idx1 on " + dbuser + "." + tableName + "(imp_flag)";
                            ds1 = null;
                            ds1 = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet(dtable, Session["SelectedConn"].ToString());
                        }

                        //---------------------------CHECKING IF CONTRACT IS MISSING-------------------------

                        qry2 = "";
                        ds1 = null;
                        qry2 = "select * from " + dbuser + "." + tableName + " where SYMBOL||EXPIRYDATE not in(select SYMBOL||EXPIRYDATE from " + dbuser + ".cucontracts where exchange='INX') ";
                        ds1 = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet(qry2, Session["SelectedConn"].ToString());
                        if (ds1.Tables[0].Rows.Count != 0)
                        {
                            //TempData["Message"] = "Trade Import is Cancelled,Some Contracts are Missing.Import Contract Master then Import Trade File Again";
                            //TempData["MessageType"] = "FAILED";

                            qry2 = "";
                            ds1 = null;
                            qry2 = "drop table " + dbuser + "." + tableName + " ";
                            ds1 = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet(qry2, Session["SelectedConn"].ToString());
                            TempData["AlertMessage"] = "Trade Import is Cancelled,Some Contracts are Missing.Import Contract Master then Import Trade File Again";
                            // RedirectToAction("TradeImport", "Process", model);
                        }


                        DoValidate(model, tableName, true);

                    }
                }

                else
                {

                    NSETradeImport();

                }
                return View(model);
            }
            catch (Exception ex)
            {
            }
            return View();
        }

        public void NSETradeImport()
        {


        }


        public void DoValidate(ImportFileInput model, string tableName, bool Trans)
        {


            //-------------------------Updating Multiplier

            DataSet ds1 = new DataSet();
            string qry2 = "";
            ds1 = null;

            qry2 = "update " + dbuser + "." + tableName + " c set MULTIPLIER=(SELECT NVL(COMULTIPLIER,1) from " + dbuser + ".cucontracts cu  where C.SYMBOL=CU.SYMBOL and c.expirydate=cu.expirydate AND CU.INSTRUMENT_TYPE LIKE'FUT%'  and exchange='" + model.Exchange + "')  where trade_date=to_date('" + model.TradeDate.ToString("ddMMMyyyy") + "') and imp_flag is null and exchange='" + model.Exchange + "'";
            ds1 = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet(qry2, Session["SelectedConn"].ToString());

            ////////////--------------------Updating Mktlot

            qry2 = "";
            ds1 = null;
            qry2 = " update " + dbuser + "." + tableName + " c set trn_qty=unit*1 where trade_date=to_date('" + model.TradeDate.ToString("ddMMMyyyy") + "') and (imp_flag<>'Y' or imp_flag is null)";
            ds1 = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet(qry2, Session["SelectedConn"].ToString());

            if (model.Exchange == "NSE")
            {
                qry2 = "";
                ds1 = null;
                qry2 = " update " + dbuser + "." + tableName + " c set trn_qty=trn_qty*200  where trade_date=to_date('" + model.TradeDate.ToString("ddMMMyyyy") + "') AND SYMBOL='INRUSD' and (imp_flag<>'Y' or imp_flag is null)";
                ds1 = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet(qry2, Session["SelectedConn"].ToString());
            }
            else
            {
                qry2 = "";
                ds1 = null;
                qry2 = " update " + dbuser + "." + tableName + " c set trn_qty=trn_qty*100 where trade_date=to_date('" + model.TradeDate.ToString("ddMMMyyyy") + "') AND SYMBOL='INRUSD' and (imp_flag<>'Y' or imp_flag is null)";
                ds1 = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet(qry2, Session["SelectedConn"].ToString());
            }

            //----------------Updating Pro Client(1)

            qry2 = "";
            ds1 = null;
            qry2 = " UPDATE " + dbuser + "." + tableName + " C SET C.CLIENTCODE=(SELECT B.PROCLIENT FROM " + dbuser + ".CUBRANCHFILE  B WHERE B.RECORDTYPE IS NULL  AND SUBSTR(C.CTCLID,1,13)=SUBSTR(B.CTCLID,1,13) and B.branchcode IS NULL  AND b.CTCLID IS NOT NULL AND EXCHANGE IN ('ICEX') and B.exchange='" + model.Exchange + "') WHERE C.TRADE_DATE=to_date('" + model.TradeDate.ToString("ddMMMyyyy") + "')  and C.exchange='" + model.Exchange + "' AND  C.imp_flag is null and (C.ORGCLIENTID=C.TMID  OR C.ORGCLIENTID = 'OWN')AND  EXISTS(SELECT B.PROCLIENT FROM " + dbuser + ".CUBRANCHFILE B  WHERE B.RECORDTYPE IS NULL AND SUBSTR(C.CTCLID,1,13)=SUBSTR(B.CTCLID,1,13) and B.exchange='" + model.Exchange + "') and C.clientcode is null  AND CTCLID IS NOT NULL ";
            ds1 = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet(qry2, Session["SelectedConn"].ToString());


            qry2 = "";
            ds1 = null;
            qry2 = " UPDATE " + dbuser + "." + tableName + " C SET C.CLIENTCODE=(SELECT B.PROCLIENT FROM " + dbuser + ".CUBRANCHFILE  B  WHERE B.RECORDTYPE IS NULL  AND ltrim(B.branchcode,'0')=C.USERID AND SUBSTR(C.CTCLID,1,13)=SUBSTR(B.CTCLID,1,13)AND b.CTCLID IS NOT NULL and B.branchcode IS NOT NULL AND B.exchange='" + model.Exchange + "') WHERE C.TRADE_DATE=to_date('" + model.TradeDate.ToString("ddMMMyyyy") + "')  and C.exchange='" + model.Exchange + "' AND  C.imp_flag is null and (C.ORGCLIENTID=C.TMID  OR C.ORGCLIENTID = 'OWN')AND  EXISTS(SELECT B.PROCLIENT FROM " + dbuser + ".CUBRANCHFILE B WHERE B.RECORDTYPE IS NULL AND SUBSTR(C.CTCLID,1,13)=SUBSTR(B.CTCLID,1,13) and B.exchange='" + model.Exchange + "')  and C.clientcode is null  AND CTCLID IS NOT NULL ";
            ds1 = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet(qry2, Session["SelectedConn"].ToString());


            qry2 = "";
            ds1 = null;
            qry2 = " UPDATE " + dbuser + "." + tableName + " C SET C.CLIENTCODE=(SELECT B.PROCLIENT FROM " + dbuser + ".CUBRANCHFILE  B WHERE B.RECORDTYPE IS NULL AND ltrim(B.branchcode,'0')=C.USERID AND C.ORGCLIENTID=B.PREFIX   and B.exchange='" + model.Exchange + "' AND PREFIX IS NOT NULL and b.ctclid is  null) WHERE C.TRADE_DATE=to_date('" + model.TradeDate.ToString("ddMMMyyyy") + "')  and C.exchange='" + model.Exchange + "' AND  C.imp_flag is null and (C.ORGCLIENTID=C.TMID  OR C.ORGCLIENTID = 'OWN')AND EXISTS(SELECT B.PROCLIENT FROM " + dbuser + ".CUBRANCHFILE B  WHERE B.RECORDTYPE IS NULL AND ltrim(B.branchcode,'0')=C.USERID  AND SUBSTR(C.CTCLID,1,13)=SUBSTR(B.CTCLID,1,13) and C.ORGCLIENTID=B.PREFIX  and B.exchange='" + model.Exchange + "' and prefix is not null and b.ctclid is null)  and C.clientcode is null   ";
            ds1 = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet(qry2, Session["SelectedConn"].ToString());



            //----------Updating Pro Client(3)----------------
            qry2 = "";
            ds1 = null;
            qry2 = " UPDATE " + dbuser + "." + tableName + " C SET CLIENTCODE=(SELECT PROCLIENT FROM " + dbuser + ".CUBRANCHFILE WHERE RECORDTYPE IS NULL AND ltrim(branchcode,'0')=C.USERID  and exchange='" + model.Exchange + "'  AND CTCLID IS NULL and prefix is null) WHERE TRADE_DATE=to_date('" + model.TradeDate.ToString("ddMMMyyyy") + "')  and exchange='" + model.Exchange + "' AND  imp_flag is null and (ORGCLIENTID=TMID  OR ORGCLIENTID = 'OWN')AND  EXISTS(SELECT PROCLIENT FROM " + dbuser + ".CUBRANCHFILE  WHERE RECORDTYPE IS NULL AND ltrim(branchcode,'0')=C.USERID  and exchange='" + model.Exchange + "' and prefix is null and ctclid is null) and clientcode is null AND CTCLID IS NULL  ";
            ds1 = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet(qry2, Session["SelectedConn"].ToString());




            //-----------------Updating Pro Client(4)----------------------

            qry2 = "";
            ds1 = null;
            qry2 = " UPDATE " + dbuser + "." + tableName + " C SET CLIENTCODE='" + model.procode + "' WHERE TRADE_DATE=to_date('" + model.TradeDate.ToString("ddMMMyyyy") + "')  and exchange='" + model.Exchange + "' AND  imp_flag is null and (ORGCLIENTID=TMID OR ORGCLIENTID = 'OWN') AND NOT EXISTS(SELECT PROCLIENT FROM " + dbuser + ".CUBRANCHFILE B  WHERE RECORDTYPE IS NULL AND ltrim(branchcode,'0')=C.USERID and B.CTCLID IS NULL and prefix is null AND proclient is NOT null  and exchange='" + model.Exchange + "') and clientcode is null";
            ds1 = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet(qry2, Session["SelectedConn"].ToString());


            //-----------------Updating Pro Client(5)------------------------

            qry2 = "";
            ds1 = null;
            qry2 = " UPDATE " + dbuser + "." + tableName + " C SET CLIENTCODE=(SELECT PROCLIENT FROM " + dbuser + ".CUBRANCHFILE WHERE RECORDTYPE IS NULL AND ltrim(branchcode,'0')=C.USERID  and exchange='" + model.Exchange + "') WHERE TRADE_DATE=to_date('" + model.TradeDate.ToString("ddMMMyyyy") + "')  and exchange='" + model.Exchange + "' AND  imp_flag is null and (ORGCLIENTID=TMID  OR ORGCLIENTID = 'OWN')AND   EXISTS(SELECT PROCLIENT FROM " + dbuser + ".CUBRANCHFILE  WHERE RECORDTYPE IS NULL AND ltrim(branchcode,'0')=C.USERID  and exchange='" + model.Exchange + "')  and clientcode is null";
            ds1 = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet(qry2, Session["SelectedConn"].ToString());

            //----------------------- "Updating Clients (1)----------------------------


            qry2 = "";
            ds1 = null;
            qry2 = " UPDATE " + dbuser + "." + tableName + " C SET CLIENTCODE=(SELECT CLIENTID FROM " + dbuser + ".CUBRANCHFILE B WHERE RECORDTYPE IS NULL AND BRANCHCODE=C.USERID AND substr(B.CTCLID,1,13)=substr(C.CTCLID,1,13) and orgclientid=prefix AND prefix is not null and B.CTCLID IS NOT NULL AND CLIENTID IS NOT NULL  and exchange='" + model.Exchange + "') WHERE TRADE_DATE=to_date('" + model.TradeDate.ToString("ddMMMyyyy") + "') AND   imp_flag is null and exchange='" + model.Exchange + "' and EXISTS(SELECT CLIENTID FROM " + dbuser + ".CUBRANCHFILE  WHERE RECORDTYPE IS NULL AND BRANCHCODE=C.USERID AND CLIENTID IS NOT NULL  and exchange='" + model.Exchange + "' and prefix is not null and ctclid is not null) and clientcode is null AND CTCLID IS NOT NULL";
            ds1 = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet(qry2, Session["SelectedConn"].ToString());


            qry2 = "";
            ds1 = null;
            qry2 = " UPDATE " + dbuser + "." + tableName + " C SET CLIENTCODE=(SELECT CLIENTID FROM " + dbuser + ".CUBRANCHFILE B  WHERE RECORDTYPE IS NULL AND BRANCHCODE=C.USERID AND  orgclientid=prefix AND prefix is not  null and B.CTCLID IS NULL AND CLIENTID IS NOT NULL  and exchange='" + model.Exchange + "')WHERE TRADE_DATE=to_date('" + model.TradeDate.ToString("ddMMMyyyy") + "') AND   imp_flag is null and exchange='" + model.Exchange + "' and EXISTS(SELECT CLIENTID  FROM " + dbuser + ".CUBRANCHFILE  WHERE RECORDTYPE IS NULL AND BRANCHCODE=C.USERID AND  orgclientid=prefix AND CLIENTID IS NOT NULL  and exchange='" + model.Exchange + "' and prefix is not null and ctclid is null)  and clientcode is null ";
            ds1 = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet(qry2, Session["SelectedConn"].ToString());


            //-------------Updating Clients (2)------------

            qry2 = "";
            ds1 = null;
            qry2 = " UPDATE " + dbuser + "." + tableName + " C SET CLIENTCODE=(SELECT CLIENTID FROM " + dbuser + ".CUBRANCHFILE B WHERE RECORDTYPE IS NULL AND BRANCHCODE=C.USERID AND substr(B.CTCLID,1,13)=substr(C.CTCLID,1,13) AND B.CTCLID IS NOT NULL AND prefix is null and CLIENTID IS NOT NULL  and exchange='" + model.Exchange + "') WHERE TRADE_DATE=to_date('" + model.TradeDate.ToString("ddMMMyyyy") + "') AND   imp_flag is null and exchange='" + model.Exchange + "' and EXISTS(SELECT CLIENTID  FROM " + dbuser + ".CUBRANCHFILE WHERE RECORDTYPE IS NULL AND BRANCHCODE=C.USERID AND CLIENTID IS NOT NULL  and exchange='" + model.Exchange + "' and prefix is null and ctclid is not null) and clientcode is null AND CTCLID IS NOT NULL";
            ds1 = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet(qry2, Session["SelectedConn"].ToString());

            //-------------------Updating Clients (3)--------------------

            qry2 = "";
            ds1 = null;
            qry2 = " UPDATE " + dbuser + "." + tableName + " C SET CLIENTCODE=(SELECT CLIENTID FROM " + dbuser + ".CUBRANCHFILE B WHERE RECORDTYPE IS NULL AND BRANCHCODE=C.USERID AND B.CTCLID IS NULL AND CLIENTID IS NOT NULL  and exchange='" + model.Exchange + "' and prefix is null) WHERE TRADE_DATE=to_date('" + model.TradeDate.ToString("ddMMMyyyy") + "') AND   imp_flag is null and exchange='" + model.Exchange + "' and EXISTS(SELECT CLIENTID FROM " + dbuser + ".CUBRANCHFILE  WHERE RECORDTYPE IS NULL AND BRANCHCODE=C.USERID AND prefix is null and CLIENTID IS NOT NULL  and exchange='" + model.Exchange + "' and ctclid is null) and clientcode is null AND CTCLID is null";
            ds1 = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet(qry2, Session["SelectedConn"].ToString());


            //---------------------Updating Clients (4)------------------------

            qry2 = "";
            ds1 = null;
            qry2 = " UPDATE " + dbuser + "." + tableName + " C SET CLIENTCODE=(SELECT CLIENTID FROM " + dbuser + ".CUBRANCHFILE B WHERE RECORDTYPE IS NULL AND BRANCHCODE=C.USERID AND B.CTCLID IS NULL and prefix is null AND CLIENTID IS NOT NULL  and exchange='" + model.Exchange + "') WHERE TRADE_DATE=to_date('" + model.TradeDate.ToString("ddMMMyyyy") + "') AND   imp_flag is null and exchange='" + model.Exchange + "' and EXISTS(SELECT CLIENTID  FROM " + dbuser + ".CUBRANCHFILE  WHERE RECORDTYPE IS NULL AND BRANCHCODE=C.USERID AND CLIENTID IS NOT NULL  and prefix is null and exchange='" + model.Exchange + "') and clientcode is null AND nvl(CTCLID,0)=0";
            ds1 = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet(qry2, Session["SelectedConn"].ToString());

            //------------------------Updating Clients (5)-------------------

            qry2 = "";
            ds1 = null;
            qry2 = " UPDATE " + dbuser + "." + tableName + " C SET CLIENTCODE=(SELECT CLIENTID FROM " + dbuser + ".CUBRANCHFILE B  WHERE RECORDTYPE IS NULL AND BRANCHCODE=C.USERID AND B.CTCLID IS NULL AND CLIENTID IS NOT NULL  and prefix is null and exchange='" + model.Exchange + "') WHERE TRADE_DATE=to_date('" + model.TradeDate.ToString("ddMMMyyyy") + "') AND   imp_flag is null and exchange='" + model.Exchange + "' and EXISTS(SELECT CLIENTID FROM " + dbuser + ".CUBRANCHFILE WHERE RECORDTYPE IS NULL AND BRANCHCODE=C.USERID AND CLIENTID IS NOT NULL  and prefix is null and exchange='" + model.Exchange + "') and clientcode is null ";
            ds1 = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet(qry2, Session["SelectedConn"].ToString());


            //---------------------Updating Clients(Prefix)---------------------

            qry2 = "";
            ds1 = null;
            qry2 = " UPDATE " + dbuser + "." + tableName + " C SET CLIENTCODE=(SELECT CLIENTID FROM " + dbuser + ".CUBRANCHFILE WHERE RECORDTYPE='P' AND orgclientid like branchcode||'%' AND CLIENTID IS NOT NULL  and exchange='" + model.Exchange + "')WHERE TRADE_DATE=to_date('" + model.TradeDate.ToString("ddMMMyyyy") + "') and exchange='" + model.Exchange + "' AND  imp_flag is null and EXISTS(SELECT CLIENTID FROM " + dbuser + ".CUBRANCHFILE  WHERE RECORDTYPE='P' AND orgclientid like branchcode||'%' AND CLIENTID IS NOT NULL  and exchange='" + model.Exchange + "')  and clientcode is null";
            ds1 = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet(qry2, Session["SelectedConn"].ToString());

            if (model.ImportBasis == "Actual Code")
            {
                qry2 = "";
                ds1 = null;
                qry2 = "UPDATE " + dbuser + "." + tableName + " C SET CLIENTCODE=(SELECT PAR_CODE FROM " + dbuser + ".CUPARTYMST WHERE PAR_CODE=C.ORGCLIENTID)WHERE TRADE_DATE=to_date('" + model.TradeDate.ToString("ddMMMyyyy") + "')  and exchange='" + model.Exchange + "' AND  imp_flag is null and NOT EXISTS(SELECT CLIENTID FROM " + dbuser + ".CUBRANCHFILE WHERE RECORDTYPE IS NULL AND BRANCHCODE=C.USERID AND CLIENTID IS NOT NULL  and exchange='" + model.Exchange + "') and clientcode is null";
                ds1 = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet(qry2, Session["SelectedConn"].ToString());
            }
            else
            {
                qry2 = "";
                ds1 = null;
                qry2 = "UPDATE " + dbuser + "." + tableName + " C SET CLIENTCODE=(SELECT party_cd FROM " + dbuser + ".CUPARTYMST_FIXES  WHERE SHORTCODE=C.ORGCLIENTID AND EXCHANGE='" + model.Exchange + "') WHERE TRADE_DATE=to_date('" + model.TradeDate.ToString("ddMMMyyyy") + "')  and exchange='" + model.Exchange + "' AND  imp_flag is null AND  NOT EXISTS(SELECT CLIENTID FROM " + dbuser + ".CUBRANCHFILE WHERE RECORDTYPE IS NULL AND BRANCHCODE=C.USERID AND CLIENTID IS NOT NULL  and exchange='" + model.Exchange + "' ) and clientcode is null ";
                ds1 = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet(qry2, Session["SelectedConn"].ToString());
            }

            //-----------------OWN Pro Client----------------------

            qry2 = "";
            ds1 = null;
            qry2 = " UPDATE " + dbuser + "." + tableName + " C SET CLIENTCODE='" + model.procode + "' WHERE TRADE_DATE=to_date('" + model.TradeDate.ToString("ddMMMyyyy") + "')  and exchange='" + model.Exchange + "' AND  imp_flag is null and (TMID='" + model.memberid + "' AND ORGCLIENTID = 'OWN') AND NOT EXISTS(SELECT PROCLIENT FROM " + dbuser + ".CUBRANCHFILE B WHERE RECORDTYPE IS NULL AND ltrim(branchcode,'0')=C.USERID and B.CTCLID IS NULL and prefix is null AND proclient is NOT null  and exchange='" + model.Exchange + "') and clientcode is null";
            ds1 = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet(qry2, Session["SelectedConn"].ToString());


            qry2 = "";
            ds1 = null;
            qry2 = "select trfmain from " + dbuser + ".CUPARA where exchange = '" + model.Exchange + "'";
            ds1 = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet(qry2, Session["SelectedConn"].ToString());
            if (ds1.Tables[0].Rows[0]["trfmain"].ToString() == "Y")
            {
                TrToMain(tableName, model);
                RedirectToAction("TradeImport", "Process");
            }
        }


        public void TrToMain(String tableName, ImportFileInput model)
        {

            //----------------------- for trades other than custodian---------

            DataSet ds1 = new DataSet();
            string qry2 = "";
            ds1 = null;
            qry2 = "insert into " + dbuser + ".CUTRNMAST(tradeno,trade_date,instrument_type,symbol,expirydate,strikeprice,optiontype,tradeprice,trade_time,trn_qty,branchid,userid,orderno,orgclientid,clientcode,securityname,booktype,booktypename,mkt_type,openclosflag,netprice,trn_slno,imp_flag,tradestatus,trn_brok,brokround,tmid,BUYSELLIND,ORDER_TIME,multiplier,Exchange,UNIT,ctclid,REVERSECODE,APFLAG,BRCODE,RM_CODE,sessionid,ORDERDATE) select tradeno,trade_date,instrument_type,symbol,expirydate,strikeprice,optiontype,tradeprice,trade_time,trn_qty,branchid,userid,orderno,orgclientid,clientcode,securityname,booktype,booktypename,mkt_type,openclosflag,netprice," + dbuser + ".sysdbsequence.nextval,imp_flag,tradestatus,trn_brok,brokround,tmid,BUYSELLIND,ORDER_TIME,multiplier,exchange, UNIT,ctclid,REVERSECODE,APFLAG,BRANCHIND,RMCODE,SESSIONID,ORDERDATE from " + dbuser + "." + tableName + "," + dbuser + ".CUPARTYMST where CLIENTCODE=PAR_CODE(+) AND imp_flag is null";

            qry2 = qry2 + " AND CLIENTCODE IN (SELECT PARTY_CD FROM " + dbuser + ".CUPARTYMST_FIXES WHERE NVL(CUSTODIAN,'N')='N' AND EXCHANGE='" + model.Exchange + "') and (tmid='" + model.memberid + "' or tmid is null) AND TRADESTATUS not in ('19','17') ";
            ds1 = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet(qry2, Session["SelectedConn"].ToString());


            //---------------------for reverse trades other than custodian trades---------------------
            //-----------------------Importing Exchange Trades--------

            qry2 = "";
            ds1 = null;
            qry2 = "insert into " + dbuser + ".CUTRNMAST(tradeno,trade_date,instrument_type,symbol,expirydate,strikeprice,optiontype,tradeprice,trade_time,trn_qty,branchid,userid,orderno,orgclientid, clientcode,securityname,booktype,booktypename,mkt_type,openclosflag,netprice,trn_slno,imp_flag,tradestatus,trn_brok,brokround,tmid,buysellind,ORDER_TIME,multiplier,Exchange,UNIT,ctclid,APFLAG,SESSIONID,ORDERDATE) select tradeno,trade_date,instrument_type,symbol,expirydate,  strikeprice,optiontype,tradeprice,trade_time,0-trn_qty,branchid,userid,orderno,orgclientid,'" + model.brokercode + "',securityname,booktype,booktypename,mkt_type,openclosflag,netprice," + dbuser + ".sysdbsequence.nextval,imp_flag,tradestatus,0,0,tmid,decode(buysellind,'1','2','2','1'),ORDER_TIME,multiplier,Exchange,0-UNIT,ctclid,APFLAG,SESSIONID,ORDERDATE from " + dbuser + "." + tableName + " where imp_flag is null";
            qry2 = qry2 + " AND CLIENTCODE IN (SELECT PARTY_CD FROM " + dbuser + ".CUPARTYMST_FIXES WHERE NVL(CUSTODIAN,'N')='N' AND EXCHANGE='" + model.Exchange + "') and (tmid='" + model.memberid + "' or tmid is null) AND TRADESTATUS not in ('19','17') ";
            ds1 = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet(qry2, Session["SelectedConn"].ToString());


            //------------------------ for custodian trades-------------------------------
            //---------------- Importing Custodian Trades ------------------

            qry2 = "";
            ds1 = null;
            qry2 = "insert into " + dbuser + ".CUCTRNMAST (tradeno,trade_date,instrument_type,symbol,expirydate,strikeprice,optiontype,tradeprice,trade_time,trn_qty,branchid,userid,orderno,orgclientid, clientcode,securityname,booktype,booktypename,mkt_type,openclosflag,netprice, trn_slno,imp_flag,tradestatus,trn_brok,brokround,tmid,BUYSELLIND,ORDER_TIME,multiplier,Exchange,UNIT,APFLAG,BRCODE,RM_CODE,SESSIONID) select tradeno,trade_date,instrument_type,symbol,expirydate,strikeprice,optiontype,tradeprice,trade_time,trn_qty,branchid,userid,orderno,orgclientid,clientcode,securityname,booktype,booktypename,mkt_type,openclosflag,netprice, " + dbuser + ".sysdbsequence.nextval,imp_flag,'11',trn_brok,brokround,tmid,BUYSELLIND,ORDER_TIME,multiplier,Exchange,UNIT,APFLAG,BRANCHIND,RMCODE,SESSIONID from " + dbuser + "." + tableName + " t ," + dbuser + ".CUPARTYMST P where CLIENTCODE=PAR_CODE(+) AND imp_flag is null";
            qry2 = qry2 + " AND CLIENTCODE IN (SELECT PARTY_CD FROM " + dbuser + ".CUPARTYMST_FIXES WHERE NVL(CUSTODIAN,'N')='Y' AND EXCHANGE='" + model.Exchange + "')";

            ds1 = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet(qry2, Session["SelectedConn"].ToString());


            //-------------------- for reverse custodian trade------------------------ 

            qry2 = "";
            ds1 = null;
            qry2 = "insert into " + dbuser + ".CUCTRNMAST(tradeno,trade_date,instrument_type,symbol,expirydate, strikeprice,optiontype,tradeprice,trade_time,trn_qty,branchid,userid,orderno,orgclientid,clientcode,securityname,booktype,booktypename,mkt_type,openclosflag,netprice,trn_slno,imp_flag,tradestatus,trn_brok,brokround,tmid,buysellind,ORDER_TIME,multiplier,Exchange,UNIT,APFLAG) select tradeno,trade_date,instrument_type,symbol,expirydate,strikeprice,optiontype,tradeprice,trade_time,0-trn_qty,branchid,userid,orderno,orgclientid,'" + model.brokercode + "',securityname,booktype,booktypename,mkt_type,openclosflag,netprice," + dbuser + ".sysdbsequence.nextval,imp_flag,'11',0,0,tmid,decode(buysellind,'1','2','2','1'),ORDER_TIME,multiplier,Exchange,0-UNIT,APFLAG from " + dbuser + "." + tableName + " T where imp_flag is null";
            qry2 = qry2 + " AND CLIENTCODE IN (SELECT PARTY_CD FROM " + dbuser + ".CUPARTYMST_FIXES WHERE NVL(CUSTODIAN,'N')='Y' AND EXCHANGE='" + model.Exchange + "') ";
            ds1 = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet(qry2, Session["SelectedConn"].ToString());


            qry2 = "";
            ds1 = null;
            qry2 = "update " + dbuser + "." + tableName + " set imp_flag='Y' where imp_flag is null AND (tmid='" + model.memberid + "' or tmid is null)";
            ds1 = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet(qry2, Session["SelectedConn"].ToString());


            qry2 = "";
            ds1 = null;
            qry2 = "UPDATE " + dbuser + ".CUTRNMAST C SET CLIENT_STATECD=(SELECT NVL(GSTSTATECODE,DEF_STCODE) FROM " + dbuser + ".CUPARTYMST C," + dbuser + ".CUPARA P WHERE PAR_CODE=CLIENTCODE AND C.EXCHANGE=P.EXCHANGE), DEALING_STATECD=(SELECT NVL(STCODE,DEF_STCODE) FROM " + dbuser + ".BRANCHMST," + dbuser + ".CUPARA P WHERE BRCODE=CODE AND P.EXCHANGE=C.EXCHANGE) WHERE  TrADE_date=to_date('" + model.TradeDate.ToString("ddMMMyyyy") + "') ";
            ds1 = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet(qry2, Session["SelectedConn"].ToString());

            TempData["AlertMessage"] = "" + model.Records + " Records Inserted Successfully";
            TempData["MessageType"] = "success";
            RedirectToAction("TradeImport", "Process");



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
                bool status = MvcApplication.OracleDBHelperCore().LoaderHelper.runLoader(dbname, "sysadm.trfo", newfileName, "TRADENO, TRSTATUS, SYMBOL, INSTRUMENT_TYPE, EXPIRYDATE, STRIKEPRICE, OPTIONTYPE, TEMP1 , TEMP2 , TEMP3 , USERID, TEMP4 , BUYSELLIND  , QTY, TRADEPRICE  , TEMP5 , CLIENTCODE , TMID  , TEMP6 , TEMP7 , TRADEDATE, ORDERNO  , TEMP8 , ORDERDATE, CTCLID", "E");
            }
            catch (Exception ex)
            {

            }
            return View();
        }


        //------------------------Close Rate Entry Start---------------------------------

        public ActionResult CloseRateEntry()
        {
            SymbolImp symImp = new SymbolImp();
            CloseRateEntryIn model = new CloseRateEntryIn();
            var dt = TempData["lstclOutData"];
            model.closeRateEntryOut = (CloseRateEntryOut)dt;
            if (Session["WebUser"] == null)
            {
                TempData["AlertMessage"] = "Session Time Out Please Login Again";
                return RedirectToAction("Index", "Login");

            }
            else
            {

                model.TrDate = DateTime.Parse(Session["FinYearFrom"].ToString());
                //return View(model);
            }
            var symbol = symImp.GetSymbol();
            ViewBag.SymbolList = new SelectList(symbol, "", "SymbolName");
            return View(model);
        }

        public static CloseRateEntryOut lstclOut;

        public ActionResult CloseRateEntryReport(CloseRateEntryOut model)
        {
            string date = model.TrDate.ToString("ddMMMyyyy");
            if (Session["WebUser"] == null)
            {
                TempData["AlertMessage"] = "Session Time Out Please Login Again";
                return RedirectToAction("Index", "Login");

            }
            else
            {
                try
                {
                    WebUser webUser = Session["WebUser"] as WebUser;
                    if (webUser == null) return null;
                    if (model.Exchange.ToString() == "NSE" || model.Exchange.ToString() == "INX")
                    {

                        string qry = "SELECT WDATE, CONTNAME, STRIKEPRICE,CLOSPRICE,EXPIRYDATE,OPTIONTYPE,EXCHANGE,SESSIONID,UM,PRICEUNIT,SETTPRICE from " + dbuser + ".CUDAILYPRICE WHERE WDATE ='"+ date + "'And ROWNUM <= 500";
                        DataSet ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet(qry, Session["SelectedConn"].ToString());
                        lstclOut = new CloseRateEntryOut();
                        lstclOut.listCloseRateEntryOutRow = new List<CloseRateEntryOutRow>();
                        lstclOut.TrDate = model.TrDate;

                        int c = 0;
                        foreach (DataRow row in ds.Tables[0].Rows)
                        {
                            if (c == 0)
                            {
                                lstclOut.TrDate = model.TrDate;

                                // lstDcOut.ClientName = row["PAR_NAME"].ToString();
                            }
                            c++;
                            CloseRateEntryOutRow Clro = new CloseRateEntryOutRow();
                            Clro.TrDate = DateTime.Parse(row["WDATE"].ToString());
                            Clro.ContName = row["CONTNAME"].ToString();
                            Clro.StrikePrice = row["STRIKEPRICE"].ToString();
                            Clro.OptionType = row["OPTIONTYPE"].ToString();
                            Clro.PriceUnit = row["PRICEUNIT"].ToString();
                            Clro.ExpiryDate = DateTime.Parse(row["EXPIRYDATE"].ToString());
                            Clro.UM = row["UM"].ToString();
                            Clro.SessionId = row["SESSIONID"].ToString();
                            Clro.SetTTPrice = row["SETTPRICE"].ToString();
                            Clro.Exchange = row["EXCHANGE"].ToString();
                            Clro.ClosePrice = row["CLOSPRICE"].ToString();
                            lstclOut.listCloseRateEntryOutRow.Add(Clro);
                        }
                    }
                    TempData["lstclOutData"] = lstclOut;
                    //return View(lstclOut);
                    return RedirectToAction("CloseRateEntry", "Process");
                }


                catch (Exception ex)
                {
                    TempData["AlertMessage"] = ex;
                    //return RedirectToAction("BillSummary", "Report");
                }
                return RedirectToAction("CloseRateEntry", "Process");
            }
        }


        public ActionResult CloseRateExpiry()
        {
            if (Session["WebUser"] == null)
            {
                TempData["AlertMessage"] = "Session Time Out Please Login Again";
                return RedirectToAction("Index", "Login");

            }
            else
            {
                CloseRateEntryIn model = new CloseRateEntryIn();
                model.TrDate = DateTime.Parse(Session["FinYearFrom"].ToString());
                return View(model);
            }


        }

        public ActionResult CloseRateGetdata(string id)
        {
            CloseRateEntryIn Closerate = new CloseRateEntryIn();

            DataSet ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet("SELECT  to_char( WDATE,'dd-mm-yyyy') WDATE, CONTNAME, STRIKEPRICE,CLOSPRICE,EXPIRYDATE,OPTIONTYPE,EXCHANGE,SESSIONID FROM " + dbuser + ".CUDAILYPRICE where EXCHANGE ='" + id + "' ", Session["SelectedConn"].ToString());
            if (ds.Tables[0].Rows.Count != 0)
            {
                Closerate.TrDate = DateTime.Parse(ds.Tables[0].Rows[0]["WDATE"].ToString());
                Closerate.Exchange = ds.Tables[0].Rows[0]["exchange"].ToString();
                Closerate.SessionId = ds.Tables[0].Rows[0]["SESSIONID"].ToString();
                //Closerate.SymbolList = ds.Tables[0].Rows[0]["OPTIONTYPE"].ToString();
            }
            return Json(Closerate, JsonRequestBehavior.AllowGet);
        }

        public ActionResult CloseRateExpiryReport(CloseRateEntryOut model)
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
                    WebUser webUser = Session["WebUser"] as WebUser;
                    if (webUser == null) return null;
                    string qry = "select DISTINCT SYMBOL,INSTRUMENT_TYPE from " + dbuser + ".CuCONTRACTS where exchange='" + model.Exchange + "' AND EXPIRYDATE= TO_DATE('" + model.ExpiryDate.ToString("ddMMMyyyy") + "')  AND LENGTH(SYMBOL)<=10" + model.SessionId + "' ";

                    DataSet ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet(qry, Session["SelectedConn"].ToString());
                    lstclOut = new CloseRateEntryOut();
                    lstclOut.listCloseRateEntryOutRow = new List<CloseRateEntryOutRow>();
                    lstclOut.TrDate = model.TrDate;

                    int c = 0;
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        if (c == 0)
                        {
                            lstclOut.ExpiryDate = model.ExpiryDate;
                        }
                        c++;
                        CloseRateEntryOutRow Clro = new CloseRateEntryOutRow();
                        Clro.Symbol = row["SYMBOL"].ToString();
                        Clro.InstrumentType = row["INSTRUMENT_TYPE"].ToString();
                        lstclOut.listCloseRateEntryOutRow.Add(Clro);
                    }

                    return View(lstclOut);
                }


                catch (Exception ex)
                {
                    TempData["AlertMessage"] = ex;
                    return RedirectToAction("CloseRateExpiry", "Report");
                }
            }
        }



        //------------------------Close Rate Entry End---------------------------------


        //------------------------Closing Price Import Start---------------------------------

        [HttpGet]
        public ActionResult ClosingPriceImport(ImportFileInput model)
        {
            //model.TradeDate = DateTime.Now;
            return View(model);
        }


        //[HttpPost]
        //public ActionResult RateFileImport(ImportFileInput model, HttpPostedFileBase TradeFile)
        //{
        //    try
        //    {
        //        string qry;
        //        DataSet ds = new DataSet();



        //        //For NSE Exchange
        //        if (model.Exchange == "NSE")
        //        {
        //            string dbname = MvcApplication.OracleDBHelperCore().OracleDBManager.getDBName("MainConn");
        //            qry = "Select * From dba_tables where table_name ='NSE_RATE_TEMP_TABLE'";
        //            ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet(qry, Session["SelectedConn"].ToString());
        //            if (ds.Tables[0].Rows.Count == 0)
        //            {
        //                ds = null;
        //                qry = "CREATE TABLE " + dbname + ".NSE_RATE_TEMP_TABLE(RDATE date, INSTRUMENT_TYPE varchar2(50), SYMBOL varchar2(50),EXPIRY_DATE date,STRIKE varchar2(50),OPTION_TYPE varchar2(50), SETTLEMENT_PRICE varchar2(50))";
        //                ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet(qry, Session["SelectedConn"].ToString());
        //            }
        //            else
        //            {
        //                ds = null;
        //                qry = "TRUNCATE TABLE " + dbname + ".NSE_RATE_TEMP_TABLE";
        //                ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet(qry, Session["SelectedConn"].ToString());
        //            }

        //            string filename = TradeFile.FileName;
        //            var fileName = System.IO.Path.GetFileName(filename);
        //            var extn = System.IO.Path.GetExtension(filename);


        //            // Create the directory if it doesn't exist
        //            string uploadedFiles = Server.MapPath("~/UploadedFiles");
        //            if (!System.IO.Directory.Exists(uploadedFiles))
        //            {
        //                System.IO.Directory.CreateDirectory(uploadedFiles);
        //            }

        //            string relativePath = "~/UploadedFiles/";
        //            string newfileName = System.IO.Path.Combine(Server.MapPath(relativePath), Guid.NewGuid().ToString() + "." + extn);
        //            TradeFile.SaveAs(newfileName);

        //            // Create the "Logs" directory if it doesn't exist
        //            string logsDirectoryPath = Server.MapPath("~/Logs");
        //            if (!System.IO.Directory.Exists(logsDirectoryPath))
        //            {
        //                System.IO.Directory.CreateDirectory(logsDirectoryPath);
        //            }

        //            // Log a message before running the loader
        //            string logFilePath = System.IO.Path.Combine(logsDirectoryPath, "LogFile.txt"); // Specifies the path for the log file
        //            string logMessage = $"Starting file processing for {newfileName} at {DateTime.Now}";
        //            System.IO.File.AppendAllText(logFilePath, logMessage + Environment.NewLine);

        //            bool status = MvcApplication.OracleDBHelperCore().LoaderHelper.runLoader(dbname, "NSE_RATE_TEMP_TABLE", newfileName, "RDATE, INSTRUMENT_TYPE, SYMBOL,EXPIRY_DATE,STRIKE,OPTION_TYPE, SETTLEMENT_PRICE", "E");

        //            // Log the status of the loader
        //            logMessage = $"Loader status for {newfileName}: {(status ? "Success" : "Failure")} at {DateTime.Now}";
        //            System.IO.File.AppendAllText(logFilePath, logMessage + Environment.NewLine);

        //            // Delete the file
        //            System.IO.File.Delete(newfileName);


        //            // Check if the log file size exceeds 1MB and delete it if necessary
        //            long logFileSizeBytes = new System.IO.FileInfo(logFilePath).Length;
        //            const long maxLogFileSizeBytes = 1024 * 1024; // 1MB

        //            if (logFileSizeBytes > maxLogFileSizeBytes)
        //            {
        //                // Create a new log file
        //                logFilePath = System.IO.Path.Combine(logsDirectoryPath, $"LogFile_{DateTime.Now:yyyyMMddHHmmss}.txt");

        //                // Delete the log file
        //                //System.IO.File.Delete(logFilePath);
        //            }

        //            Session["UploadedFilesPath"] = uploadedFiles;
        //            Session["LogPath"] = logsDirectoryPath;


        //            if (status == true)
        //            {

        //                qry = "Select * from " + dbname + ".CUDAILYPRICE where WDATE =to_date('" + model.TradeDate.ToString("dd-MM-yyyy") + "','dd-MM-yyyy') and Exchange = '" + model.Exchange + "' and SessionId = '" + model.Session + "'";
        //                ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet(qry, Session["SelectedConn"].ToString());
        //                if (ds.Tables[0].Rows.Count != 0)
        //                {

        //                    ds = null;
        //                    qry = "DELETE FROM " + dbname + ".CUDAILYPRICE where WDATE =to_date('" + model.TradeDate.ToString("dd-MM-yyyy") + "','dd-MM-yyyy') and Exchange = '" + model.Exchange + "' and SessionId = '" + model.Session + "'";
        //                    ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet(qry, Session["SelectedConn"].ToString());
        //                    qry = "INSERT INTO " + dbname + ".cudailyprice Select  RDATE WDATE,INSTRUMENT_TYPE || SYMBOL || to_char(EXPIRY_DATE,'DDMONYYYY') CONTNAME, STRIKE STRIKEPRICE, SETTLEMENT_PRICE CLOSPRICE,EXPIRY_DATE EXPIRYDATE,decode(OPTION_TYPE,'FF','FX',OPTION_TYPE) OPTIONTYPE,'" + model.Exchange + "'EXCHANGE,'' PRICEUNIT,'' UM,'" + model.Session + "' SESSIONID,'' SETTPRICE from " + dbname + ". NSE_RATE_TEMP_TABLE";
        //                    ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet(qry, Session["SelectedConn"].ToString());
        //                    var successMessage = new
        //                    {
        //                        title = "Override",
        //                        text = "File overrided successfully!",
        //                        icon = "success"
        //                    };
        //                    TempData["sweetAlertOptions"] = new JavaScriptSerializer().Serialize(successMessage);
        //                }
        //                else
        //                {
        //                    ds = null;
        //                    qry = "INSERT INTO " + dbname + ".cudailyprice Select  RDATE WDATE,INSTRUMENT_TYPE || SYMBOL || to_char(EXPIRY_DATE,'DDMONYYYY') CONTNAME, STRIKE STRIKEPRICE, SETTLEMENT_PRICE CLOSPRICE,EXPIRY_DATE EXPIRYDATE,decode(OPTION_TYPE,'FF','FX',OPTION_TYPE) OPTIONTYPE,'" + model.Exchange + "'EXCHANGE,'' PRICEUNIT,'' UM,'" + model.Session + "' SESSIONID,'' SETTPRICE from " + dbname + ". NSE_RATE_TEMP_TABLE";
        //                    ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet(qry, Session["SelectedConn"].ToString());
        //                    var successMessage = new
        //                    {
        //                        title = "Success",
        //                        text = "File imported successfully!",
        //                        icon = "success"
        //                    };
        //                    TempData["sweetAlertOptions"] = new JavaScriptSerializer().Serialize(successMessage);

        //                }



        //            }
        //            else
        //            {
        //                var errorMessage = new
        //                {
        //                    title = "Error",
        //                    text = "File import failed!",
        //                    icon = "error"
        //                };
        //                TempData["sweetAlertOptions"] = new JavaScriptSerializer().Serialize(errorMessage);

        //            }

        //        }



        //        //For INX Exchange
        //        else
        //        {
        //            string dbname = MvcApplication.OracleDBHelperCore().OracleDBManager.getDBName("MainConn");
        //            qry = "Select * From dba_tables where table_name ='INX_RATE_TEMP_TABLE'";
        //            ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet(qry, Session["SelectedConn"].ToString());
        //            if (ds.Tables[0].Rows.Count == 0)
        //            {
        //                ds = null;
        //                qry = "CREATE TABLE " + dbname + ".INX_RATE_TEMP_TABLE(RDATE date, INSTRUMENT_TYPE varchar2(50), SYMBOL varchar2(50),EXPIRY_DATE date,STRIKE varchar2(50),OPTION_TYPE varchar2(50), SETTLEMENT_PRICE varchar2(50), SessionId varchar2(50))";
        //                ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet(qry, Session["SelectedConn"].ToString());
        //            }
        //            else
        //            {
        //                ds = null;
        //                qry = "TRUNCATE TABLE " + dbname + ".INX_RATE_TEMP_TABLE";
        //                ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet(qry, Session["SelectedConn"].ToString());
        //            }
        //            string filename = TradeFile.FileName;
        //            var fileName = System.IO.Path.GetFileName(filename);
        //            var extn = System.IO.Path.GetExtension(filename);


        //            // Create the directory if it doesn't exist
        //            string uploadedFiles = Server.MapPath("~/UploadedFiles");
        //            if (!System.IO.Directory.Exists(uploadedFiles))
        //            {
        //                System.IO.Directory.CreateDirectory(uploadedFiles);
        //            }

        //            string relativePath = "~/UploadedFiles/";
        //            string newfileName = System.IO.Path.Combine(Server.MapPath(relativePath), Guid.NewGuid().ToString() + "." + extn);
        //            TradeFile.SaveAs(newfileName);

        //            // Create the "Logs" directory if it doesn't exist
        //            string logsDirectoryPath = Server.MapPath("~/Logs");
        //            if (!System.IO.Directory.Exists(logsDirectoryPath))
        //            {
        //                System.IO.Directory.CreateDirectory(logsDirectoryPath);
        //            }

        //            // Log a message before running the loader
        //            string logFilePath = System.IO.Path.Combine(logsDirectoryPath, "LogFile.txt"); // Specifies the path for the log file
        //            string logMessage = $"Starting file processing for {newfileName} at {DateTime.Now}";
        //            System.IO.File.AppendAllText(logFilePath, logMessage + Environment.NewLine);

        //            bool status = MvcApplication.OracleDBHelperCore().LoaderHelper.runLoader(dbname, "NSE_RATE_TEMP_TABLE", newfileName, "RDATE, INSTRUMENT_TYPE, SYMBOL,EXPIRY_DATE,STRIKE,OPTION_TYPE, SETTLEMENT_PRICE", "E");

        //            // Log the status of the loader
        //            logMessage = $"Loader status for {newfileName}: {(status ? "Success" : "Failure")} at {DateTime.Now}";
        //            System.IO.File.AppendAllText(logFilePath, logMessage + Environment.NewLine);

        //            // Delete the file
        //            System.IO.File.Delete(newfileName);


        //            // Check if the log file size exceeds 1MB and delete it if necessary
        //            long logFileSizeBytes = new System.IO.FileInfo(logFilePath).Length;
        //            const long maxLogFileSizeBytes = 1024 * 1024; // 1MB

        //            if (logFileSizeBytes > maxLogFileSizeBytes)
        //            {
        //                // Create a new log file
        //                logFilePath = System.IO.Path.Combine(logsDirectoryPath, $"LogFile_{DateTime.Now:yyyyMMddHHmmss}.txt");

        //                // Delete the log file
        //                //System.IO.File.Delete(logFilePath);
        //            }

        //            Session["UploadedFilesPath"] = uploadedFiles;
        //            Session["LogPath"] = logsDirectoryPath;



        //            if (status == true)
        //            {
        //                qry = "Select * from " + dbname + ".CUDAILYPRICE where WDATE =to_date('" + model.TradeDate.ToString("dd-MM-yyyy") + "','dd-MM-yyyy') and Exchange = '" + model.Exchange + "' and SessionId = '" + model.Session + "'";
        //                ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet(qry, Session["SelectedConn"].ToString());
        //                if (ds.Tables[0].Rows.Count != 0)
        //                {
        //                    ds = null;
        //                    qry = "DELETE FROM " + dbname + ".CUDAILYPRICE where WDATE =to_date('" + model.TradeDate.ToString("dd-MM-yyyy") + "','dd-MM-yyyy') and Exchange = '" + model.Exchange + "' and SessionId = '" + model.Session + "'";
        //                    ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet(qry, Session["SelectedConn"].ToString());
        //                    qry = "INSERT INTO " + dbname + ".cudailyprice Select  RDATE WDATE,INSTRUMENT_TYPE || SYMBOL || to_char(EXPIRY_DATE,'DDMONYYYY') CONTNAME, STRIKE STRIKEPRICE, SETTLEMENT_PRICE CLOSPRICE,EXPIRY_DATE EXPIRYDATE,decode(OPTION_TYPE,'FF','FX',OPTION_TYPE) OPTIONTYPE,'" + model.Exchange + "'EXCHANGE,'' PRICEUNIT,'' UM,'" + model.Session + "' SESSIONID,'' SETTPRICE from " + dbname + ". NSE_RATE_TEMP_TABLE";
        //                    ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet(qry, Session["SelectedConn"].ToString());
        //                }

        //                else
        //                {
        //                    ds = null;
        //                    qry = qry = "INSERT INTO " + dbname + ".cudailyprice Select  RDATE WDATE,INSTRUMENT_TYPE || SYMBOL || to_char(EXPIRY_DATE,'DDMONYYYY') CONTNAME, STRIKE STRIKEPRICE, SETTLEMENT_PRICE CLOSPRICE,EXPIRY_DATE EXPIRYDATE,decode(OPTION_TYPE,'FF','FX',OPTION_TYPE) OPTIONTYPE, '" + model.Exchange + "'EXCHANGE,'' PRICEUNIT,'' UM,'" + model.Session + "' SESSIONID,'' SETTPRICE from " + dbname + ". INX_RATE_TEMP_TABLE";
        //                    ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet(qry, Session["SelectedConn"].ToString());
        //                    var successMessage = new
        //                    {
        //                        title = "Success",
        //                        text = "File imported successfully!",
        //                        icon = "success"
        //                    };
        //                    TempData["sweetAlertOptions"] = new JavaScriptSerializer().Serialize(successMessage);

        //                }


        //            }
        //            else
        //            {
        //                var errorMessage = new
        //                {
        //                    title = "Error",
        //                    text = "File import failed!",
        //                    icon = "error"
        //                };
        //                TempData["sweetAlertOptions"] = new JavaScriptSerializer().Serialize(errorMessage);

        //            }

        //        }



        //    }
        //    catch (Exception ex)
        //    {
        //        Console.WriteLine("Error: " + ex.Message);
        //        Console.WriteLine("StackTrace: " + ex.StackTrace);
        //    }
        //    return RedirectToAction("ClosingPriceImport", "Process");
        //}



        public JsonResult ClosingPriceImportResult(ImportFileInput model, HttpPostedFileBase TradeFile)
        
        {
            try
            {
                string qry;
                DataSet ds = new DataSet();
                string dbname = MvcApplication.OracleDBHelperCore().OracleDBManager.getDBName("MainConn");
                qry = "Select * From dba_tables where table_name ='NSE_RATE_TEMP_TABLE'";
                ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet(qry, Session["SelectedConn"].ToString());
                if (ds.Tables[0].Rows.Count == 0)
                {
                    ds = null;
                    qry = "CREATE TABLE " + dbname + ".NSE_RATE_TEMP_TABLE(RDATE varchar2(20), INSTRUMENT_TYPE varchar2(50), SYMBOL varchar2(50),EXPIRY_DATE varchar2(20),STRIKE varchar2(50),OPTION_TYPE varchar2(50), SETTLEMENT_PRICE varchar2(50))";
                    ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet(qry, Session["SelectedConn"].ToString());
                }
                else
                {
                    ds = null;
                    qry = "TRUNCATE TABLE " + dbname + ".NSE_RATE_TEMP_TABLE";
                    ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet(qry, Session["SelectedConn"].ToString());
                }

                string filename = TradeFile.FileName;
                var fileName = System.IO.Path.GetFileName(filename);
                var extn = System.IO.Path.GetExtension(filename);
                string uploadedFiles = Server.MapPath("~/UploadedFiles");
                if (!System.IO.Directory.Exists(uploadedFiles))
                {
                    System.IO.Directory.CreateDirectory(uploadedFiles);
                }

                string relativePath = "~/UploadedFiles/";
                string newfileName = System.IO.Path.Combine(Server.MapPath(relativePath), Guid.NewGuid().ToString() + "." + extn);
                TradeFile.SaveAs(newfileName);
                // Loader.FileLoading(newfileName);


                bool status = MvcApplication.OracleDBHelperCore().LoaderHelper.runLoader(dbname, "NSE_RATE_TEMP_TABLE", newfileName, "RDATE, INSTRUMENT_TYPE, SYMBOL,EXPIRY_DATE,STRIKE,OPTION_TYPE, SETTLEMENT_PRICE","E");

                if (status == true)
                {

                    ds = null;
                    qry = "Select * from " + dbname + ".CUDAILYPRICE where WDATE =to_date('" + model.TradeDate.ToString("dd-MM-yyyy") + "','dd-MM-yyyy') and Exchange = '" + model.Exchange + "' and SessionId = '" + model.Session + "'";
                    ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet(qry, Session["SelectedConn"].ToString());
                    if (ds.Tables[0].Rows.Count != 0)
                    {
                       
                            ds = null;
                            qry = "DELETE FROM " + dbname + ".CUDAILYPRICE where WDATE =to_date('" + model.TradeDate.ToString("dd-MM-yyyy") + "','dd-MM-yyyy') and Exchange = '" + model.Exchange + "' and SessionId = '" + model.Session + "'";
                            ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet(qry, Session["SelectedConn"].ToString());
                            qry = "INSERT INTO " + dbname + ".cudailyprice Select  RDATE WDATE,INSTRUMENT_TYPE || SYMBOL || to_char(EXPIRY_DATE,'DDMONYYYY') CONTNAME, STRIKE STRIKEPRICE, SETTLEMENT_PRICE CLOSPRICE,EXPIRY_DATE EXPIRYDATE,decode(OPTION_TYPE,'FF','FX',OPTION_TYPE) OPTIONTYPE,'" + model.Exchange + "'EXCHANGE,'' PRICEUNIT,'' UM,'" + model.Session + "' SESSIONID,'' SETTPRICE from " + dbname + ". NSE_RATE_TEMP_TABLE";
                            ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet(qry, Session["SelectedConn"].ToString());
                            ViewBag.Message = "You clicked YES!";
                            
                        
                    }
                    else
                    {
                        ds = null;
                        qry = "INSERT INTO " + dbname + ".cudailyprice Select  RDATE WDATE,INSTRUMENT_TYPE || SYMBOL || to_char(EXPIRY_DATE,'DDMONYYYY') CONTNAME, STRIKE STRIKEPRICE, SETTLEMENT_PRICE CLOSPRICE,EXPIRY_DATE EXPIRYDATE,decode(OPTION_TYPE,'FF','FX',OPTION_TYPE) OPTIONTYPE, '" + model.Exchange + "'EXCHANGE,'' PRICEUNIT,'' UM,'" + model.Session + "' SESSIONID,'' SETTPRICE from " + dbname + ". NSE_RATE_TEMP_TABLE";
                        ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet(qry, Session["SelectedConn"].ToString());
                    }
                      
                }
                var response = new { success = true };
                return Json(response);
            }
            catch (Exception ex)
            {
                // If an error occurs, return an error response with a message
                var response = new { success = false, errorMessage = "An error occurred." };
                return Json(response);
            }
        }


        //-----------------------TRade edit Start---------------------------------

        public ActionResult TradeEditAftertransfer(TradeEditAftertransferIn model)
        {
            return View(model);
        }

        public ActionResult TradeEditBeforetransfer(TradeEditAftertransferIn model)
        {
            return View(model);
        }



        //-----------------------Position File Import---------------------------------

        [HttpGet]
        public ActionResult PositionFileImport()
         {
            ImportFileInput model = new ImportFileInput();
            if(TempData["impFl"] != null)
            {
                var dt = TempData["impFl"];
                model.importFileOutput = (ImportFileOutput)dt;
            }
            else if(TempData["impMth"] != null)
            {
                var dt = TempData["impMth"];
                model.importMatchingRecord = (ImportMatchingRecord)dt;
            }
            if(TempData["modelData"] !=null)
            {
                var modelData = TempData["modelData"];
                model = (ImportFileInput)modelData;
            }
            //model.importFileOutput = TempData["impFl"]; 
            return View(model);
        }

        //public static ImportFileInput impFlIn;
        public static ImportFileOutput impFlOt;
        public static ImportMatchingRecord impMthRr;

        public ActionResult PositionFileImportReport (ImportFileInput model)
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
                    WebUser webUser = Session["WebUser"] as WebUser;
                    if (webUser == null)
                        return null;
                    string qry;
                    DataSet ds;
                    if (model.Exchange.ToString() == "NSE" || model.Exchange.ToString() == "INX")
                    {
                        if (model.DeleteOnly == true)
                        {
                            qry = qry = "DELETE FROM " + dbuser + ".CUPOSITION where PDATE =to_date('" + model.Date.ToString("dd-MM-yyyy") + "','dd-MM-yyyy') and Exchange = '" + model.Exchange + "' and SessionId = '" + model.SessionId + "'";

                            ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet(qry, Session["SelectedConn"].ToString());
                        }
                        // string qry = "SELECT COUNT(*) from " + dbuser + ".CUPOSITION";
                      


                            qry = "SELECT * from " + dbuser + ".CUPOSITION";

                            ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet(qry, Session["SelectedConn"].ToString());


                            impFlOt = new ImportFileOutput();
                            impFlOt.lstImportFileOutputRow = new List<ImportFileOutputRow>();

                            int c = 0;
                            foreach (DataRow row in ds.Tables[0].Rows) {

                                if (c == 0)
                                {
                                    impFlOt.Date = model.Date;

                                    // lstDcOut.ClientName = row["PAR_NAME"].ToString();
                                }
                                c++;

                                ImportFileOutputRow impFlOtRw = new ImportFileOutputRow();



                                impFlOtRw.POSITIONDATE = DateTime.Parse(row["PDATE"].ToString());
                                impFlOtRw.SEGMENT_INDICATOR = row["SEG_IND"].ToString();
                                impFlOtRw.SETTLEMENT_TYPE = row["SETT_TYPE"].ToString();
                                impFlOtRw.CLEARING_MEMBER_CODE = row["CMCODE"].ToString();
                                impFlOtRw.MEMBER_TYPE = row["MEMTYPE"].ToString();
                                impFlOtRw.TRADING_MEMBER_CODE = row["TMCODE"].ToString();
                                impFlOtRw.ACCOUNT_TYPE = row["ACTYPE"].ToString();
                                impFlOtRw.CLIENT_ACCOUNTCODE = row["CLIENTCD"].ToString();
                                impFlOtRw.INSTRUMENT_TYPE = row["INSTRUMENT_TYPE"].ToString();
                                impFlOtRw.SYMBOL = row["SYMBOL"].ToString();
                                impFlOtRw.EXPIRY_DATE = DateTime.Parse(row["EXPIRY_DATE"].ToString());
                                impFlOtRw.STRIKE_PRICE = row["STRIKE_PRICE"].ToString();
                                impFlOtRw.OPTION_TYPE = row["OPTIONTYPE"].ToString();
                                impFlOtRw.CA_LEVEL = row["CALEVEL"].ToString();
                                impFlOtRw.BROUGHT_FORWARD_LONG_QUANTITY = row["BFLONGQTY"].ToString();
                                impFlOtRw.BROUGHT_FORWARD_LONG_VALUE = row["BFLONGVAL"].ToString();
                                impFlOtRw.BROUGHT_FORWARD_SHORT_QUANTITY = row["BFSHORTQTY"].ToString();
                                impFlOtRw.BROUGHT_FORWARD_SHORT_VALUE = row["BFSHORTVAL"].ToString();
                                impFlOtRw.DAY_BUY_OPEN_QUANTITY = row["BOPEN_QTY"].ToString();
                                impFlOtRw.DAY_BUY_OPEN_VALUE = row["BOPEN_VAL"].ToString();
                                impFlOtRw.DAY_SELL_OPEN_QUANTITY = row["SOPEN_QTY"].ToString();
                                impFlOtRw.DAY_SELL_OPEN_VALUE = row["SOPEN_VAL"].ToString();
                                impFlOtRw.PRE_EXASSGN_LONG_QUANTITY = row["PREEXAS_LONGQTY"].ToString();
                                impFlOtRw.PRE_EXASSGN_LONG_VALUE = row["PREEXAS_LONGVAL"].ToString();
                                impFlOtRw.PRE_EXASSGN_SHORT_QUANTITY = row["PREEXAS_SHORTQTY"].ToString();
                                impFlOtRw.PRE_EXASSGN_SHORT_VALUE = row["PREEXAS_SHORTVAL"].ToString();
                                impFlOtRw.EXERCISED_QUANTITY = row["EXQTY"].ToString();
                                impFlOtRw.ASSIGNED_QUANTITY = row["ASQTY"].ToString();
                                impFlOtRw.POST_EXASSGN_LONG_QUANTITY = row["POSTEXAS_LONGQTY"].ToString();
                                impFlOtRw.POST_EXASSGN_LONG_VALUE = row["POSTEXAS_LONGVAL"].ToString();
                                impFlOtRw.POST_EXASSGN_SHORT_QUANTITY = row["POSTEXAS_SHORTQTY"].ToString();
                                impFlOtRw.POST_EXASSGN_SHORT_VALUE = row["POSTEXAS_SHORTVAL"].ToString();
                                impFlOtRw.SETTLEMENT_PRICE = row["SETTPRICE"].ToString();
                                impFlOtRw.NET_PREMIUM = row["NETPREM"].ToString();
                                impFlOtRw.DAILY_MTM_SETTLEMENT_VALUE = row["MTMSETT_VAL"].ToString();
                                impFlOtRw.FUTURES_FINAL_SETTLEMENT_VALUE = row["FUTFINSETT_VAL"].ToString();
                                impFlOtRw.EXERCISEDASSIGNED_VALUE = row["EXASVAL"].ToString();
                                impFlOt.lstImportFileOutputRow.Add(impFlOtRw);

                            }

                        }


                        TempData["impFl"] = impFlOt;
                        //return View(lstclOut);
                        return RedirectToAction("PositionFileImport", "Process");
                    }
                

                catch (Exception ex)
                {

                }
            }

                    return RedirectToAction("PositionFileImport", "Process");
        }

        
        public ActionResult PositionMatching(ImportFileInput model)
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
                    WebUser webUser = Session["WebUser"] as WebUser;
                    if (webUser == null)
                        return null;
                    string date = model.Date.ToString("dd-MM-yyyy");
                    string qry = " SELECT SYMBOL,To_Char(EXPIRYDATE,'dd-MM-yyyy')EXPIRYDATE,SUM(OUR) OUR,SUM(EXCH) EXCH,SUM(OUR)-SUM(EXCH) DIFF FROM (  select symbol,expirydate,ABS(sum(UNIT)) OUR,0 EXCH from IFSC.cUtrnmast C1  where trade_date<=TO_DATE('" + date + "','DD-MM-YYYY') and exchange='" + model.Exchange + "' and expirydate>=TO_DATE('" + date + "','DD-MM-YYYY')  and clientcode='CNSE' and tradestatus not in ('BF','CF','CL','EX','AS')  group by symbol,expirydate HAVING SUM(TRN_QTY)!=0  Union All  SELECT SYMBOL,EXPIRY_DATE,0,ABS(SUM((BFLONGQTY+BOPEN_QTY)-(BFSHORTQTY+SOPEN_QTY))) QTY  FROM IFSC.CUPOSITION WHERE PDATE=TO_DATE('" +date+ "','DD-MM-YYYY')  AND EXCHANGE='" + model.Exchange + "' GROUP BY SYMBOL,EXPIRY_DATE HAVING  SUM((BFLONGQTY+BOPEN_QTY)-(BFSHORTQTY+SOPEN_QTY))!=0 )  GROUP BY SYMBOL,EXPIRYDATE ORDER BY SYMBOL,EXPIRYDATE";
                    DataSet ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet(qry, Session["SelectedConn"].ToString());

                    //impFlOt = new ImportFileOutput();
                    impMthRr = new ImportMatchingRecord();

                    impMthRr.lstImportMatchingrecordRow = new List<ImportMatchingRecordRow>();
                    
                    //impFlOt.lstImportFileOutputRow = new List<ImportFileOutputRow>();

                    int c = 0;
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {

                        if (c == 0)
                        {
                            //impFlOt.Date = model.Date;
                            impMthRr.expirydate = model.Date.ToString("dd-MM-yyyy");
                            // lstDcOut.ClientName = row["PAR_NAME"].ToString();
                        }
                        c++;

                        ImportMatchingRecordRow impMthRw = new ImportMatchingRecordRow();
                        //ImportFileOutputRow impFlOtRw = new ImportFileOutputRow();
                        impMthRw.EXCHANGE = row["EXCH"].ToString();
                        impMthRw.symbol = row["SYMBOL"].ToString();
                        impMthRw.expirydate = row["EXPIRYDATE"].ToString();
                        impMthRw.ourmtm = row["OUR"].ToString();
                        impMthRw.exchmtm = row["DIFF"].ToString();
                        //impFlOt.lstImportFileOutputRow.Add(impFlOtRw);
                        impMthRr.lstImportMatchingrecordRow.Add(impMthRw);

                    }
                    TempData["impMth"] = impMthRr;
                    //return View(lstclOut);
                    return RedirectToAction("PositionFileImport", "Process");
                }

                catch (Exception ex)
                {

                }
                return View();
            }
        }
        [HttpPost]
        public ActionResult PositionFileImportResult(ImportFileInput model, HttpPostedFileBase TradeFile)

        {
            //7/3/2023 12:00:00 AM
            

           
            try
            {
                string qry;
                DataSet ds = new DataSet();
                string dbname = MvcApplication.OracleDBHelperCore().OracleDBManager.getDBName("MainConn");
                qry = "Select * From dba_tables where table_name ='NSE_POSITION_TEMP_TABLE'";
                ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet(qry, Session["SelectedConn"].ToString());
                if (ds.Tables[0].Rows.Count == 0)
                {
                    ds = null;
                    qry = "CREATE TABLE " + dbname + ".NSE_POSITION_TEMP_TABLE(PositionDate DATE, Segment_Indicator varchar2(50), Settlement_Type varchar2(50), Clearing_Member_Code varchar2(20), Member_Type varchar2(50),Trading_Member_Code varchar2(50), Account_Type varchar2(50), Client_AccountCode varchar2(50), Instrument_Type varchar2(50), Symbol varchar2(50), Expiry_date Date, Strike_Price NUMBER(7,2), Option_Type varchar2(50), CA_Level NUMBER(7,2), Brought_Forward_Long_Quantity NUMBER(7), Brought_Forward_Long_Value NUMBER (14,2), Brought_Forward_Short_Quantity NUMBER(7), Brought_Forward_Short_Value NUMBER(14,2), Day_Buy_Open_Quantity NUMBER(7), Day_Buy_Open_Value NUMBER(14,2), Day_Sell_Open_Quantity NUMBER(7), Day_Sell_Open_Value NUMBER(14,2), Pre_ExAssgn_Long_Quantity NUMBER(7), Pre_ExAssgn_Long_Value NUMBER(14,2), Pre_ExAssgn_Short_Quantity NUMBER(7), Pre_ExAssgn_Short_Value NUMBER(14,2), Exercised_Quantity varchar2(50), Assigned_Quantity NUMBER(7), Post_ExAssgn_Long_Quantity NUMBER(7), Post_ExAssgn_Long_Value NUMBER(14,2), Post_ExAssgn_Short_Quantity NUMBER(7), Post_ExAssgn_Short_Value NUMBER(14,2), Settlement_Price NUMBER, Net_Premium varchar2(50), Daily_MTM_Settlement_Value varchar2(50), Futures_Final_Settlement_Value varchar2(50), ExercisedAssigned_Value NUMBER)";
                    ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet(qry, Session["SelectedConn"].ToString());
                }
                else
                {
                    ds = null;
                    qry = "TRUNCATE TABLE " + dbname + ".NSE_POSITION_TEMP_TABLE";
                    ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet(qry, Session["SelectedConn"].ToString());
                }

                string filename = TradeFile.FileName;
                var fileName = System.IO.Path.GetFileName(filename);
                var extn = System.IO.Path.GetExtension(filename);
                //var extn = System.IO.Path.GetExtension(filename);
                string uploadedFiles = Server.MapPath("~/UploadedFiles");
                if (!System.IO.Directory.Exists(uploadedFiles))
                {
                    System.IO.Directory.CreateDirectory(uploadedFiles);
                }

                string relativePath = "~/UploadedFiles/";
                string newfileName = System.IO.Path.Combine(Server.MapPath(relativePath), Guid.NewGuid().ToString() + "." + extn);
                TradeFile.SaveAs(newfileName);
                //TradeFile.SaveAs(newfileName);
                // Loader.FileLoading(newfileName);


                bool status = MvcApplication.OracleDBHelperCore().LoaderHelper.runLoader(dbname, "NSE_POSITION_TEMP_TABLE", newfileName, "PositionDate, Segment_Indicator, Settlement_Type, Clearing_Member_Code, Member_Type, Trading_Member_Code, Account_Type, Client_AccountCode, Instrument_Type, Symbol, Expiry_date, Strike_Price, Option_Type, CA_Level, Brought_Forward_Long_Quantity, Brought_Forward_Long_Value, Brought_Forward_Short_Quantity, Brought_Forward_Short_Value, Day_Buy_Open_Quantity, Day_Buy_Open_Value, Day_Sell_Open_Quantity, Day_Sell_Open_Value, Pre_ExAssgn_Long_Quantity, Pre_ExAssgn_Long_Value, Pre_ExAssgn_Short_Quantity, Pre_ExAssgn_Short_Value, Exercised_Quantity, Assigned_Quantity, Post_ExAssgn_Long_Quantity, Post_ExAssgn_Long_Value, Post_ExAssgn_Short_Quantity, Post_ExAssgn_Short_Value, Settlement_Price, Net_Premium, Daily_MTM_Settlement_Value, Futures_Final_Settlement_Value,  ExercisedAssigned_Value", "E");

                if (status == true)
                {

                    ds = null;
                    qry = "Select * from " + dbname + ".CUPOSITION where PDATE =to_date('" + model.Date.ToString("dd-MM-yyyy") + "','dd-MM-yyyy') and Exchange = '" + model.Exchange + "' and SessionId = '" + model.SessionId + "'";
                    ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet(qry, Session["SelectedConn"].ToString());
                    if (ds.Tables[0].Rows.Count != 0)
                    {
                        
                        if (model.DeleteOnly == true)
                        {
                            model.IsDeleteConfirmed = true;
                           
                            //int deletedRowCount = ds.Tables[0].Rows.Count;
                        }
                        else
                        {
                            model.IsOverrideConfirmed = true;
                            //ds = null;
                            //qry = "DELETE FROM " + dbname + ".CUPOSITION where PDATE =to_date('" + model.Date.ToString("dd-MM-yyyy") + "','dd-MM-yyyy') and Exchange = '" + model.Exchange + "' and SessionId = '" + model.SessionId + "'";
                            //ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet(qry, Session["SelectedConn"].ToString());
                            //qry = "INSERT INTO " + dbname + ".CUPOSITION Select  POSITIONDATE PDATE,SEGMENT_INDICATOR SEG_IND, SETTLEMENT_TYPE SETT_TYPE, CLEARING_MEMBER_CODE CMCODE, MEMBER_TYPE MEMTYPE, TRADING_MEMBER_CODE TMCODE, ACCOUNT_TYPE ACTYPE, CLIENT_ACCOUNTCODE CLIENTCD, INSTRUMENT_TYPE INSTRUMENT_TYPE, SYMBOL SYMBOL, EXPIRY_DATE EXPIRY_DATE, INSTRUMENT_TYPE || SYMBOL || to_char(EXPIRY_DATE,'DDMONYYYY') CONTNAME, STRIKE_PRICE STRIKE_PRICE, decode(OPTION_TYPE,'FF','FX',OPTION_TYPE) OPTIONTYPE, CA_LEVEL CALEVEL, BROUGHT_FORWARD_LONG_QUANTITY BFLONGQTY, BROUGHT_FORWARD_LONG_VALUE BFLONGVAL, BROUGHT_FORWARD_SHORT_QUANTITY BFSHORTQTY, BROUGHT_FORWARD_SHORT_VALUE BFSHORTVAL, DAY_BUY_OPEN_QUANTITY BOPEN_QTY, DAY_BUY_OPEN_VALUE BOPEN_VAL, DAY_SELL_OPEN_QUANTITY SOPEN_QTY, DAY_SELL_OPEN_VALUE SOPEN_VAL, PRE_EXASSGN_LONG_QUANTITY PREEXAS_LONGQTY, PRE_EXASSGN_LONG_VALUE PREEXAS_LONGVAL, PRE_EXASSGN_SHORT_QUANTITY PREEXAS_SHORTQTY, PRE_EXASSGN_SHORT_VALUE  PREEXAS_SHORTVAL, EXERCISED_QUANTITY EXQTY, ASSIGNED_QUANTITY ASQTY, POST_EXASSGN_LONG_QUANTITY POSTEXAS_LONGQTY, POST_EXASSGN_LONG_VALUE POSTEXAS_LONGVAL, POST_EXASSGN_SHORT_QUANTITY POSTEXAS_SHORTQTY, POST_EXASSGN_SHORT_VALUE POSTEXAS_SHORTVAL, SETTLEMENT_PRICE SETTPRICE, NET_PREMIUM NETPREM, DAILY_MTM_SETTLEMENT_VALUE MTMSETT_VAL, FUTURES_FINAL_SETTLEMENT_VALUE FUTFINSETT_VAL, EXERCISEDASSIGNED_VALUE EXASVAL,'' CLOSPRICE,'" + model.Exchange + "'EXCHANGE,'" + model.SessionId + "' SESSIONID,'' SETTNO, '' ORGCODE from " + dbname + ". NSE_POSITION_TEMP_TABLE";
                            //ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet(qry, Session["SelectedConn"].ToString());
                            //    qry = "SELECT COUNT(*) FROM " + dbname + ".CUPOSITION WHERE PDATE = to_date('" + model.Date.ToString("dd-MM-yyyy") + "','dd-MM-yyyy') and Exchange = '" + model.Exchange + "' and SessionId = '" + model.SessionId + "'";
                            //    int insertedRowCount = Convert.ToInt32(MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet(qry, Session["SelectedConn"].ToString()));
                        }

                    }
                    else
                    {

                        ds = null;
                        qry = "INSERT INTO " + dbname + ".CUPOSITION Select  POSITIONDATE PDATE,SEGMENT_INDICATOR SEG_IND, SETTLEMENT_TYPE SETT_TYPE, CLEARING_MEMBER_CODE CMCODE, MEMBER_TYPE MEMTYPE, TRADING_MEMBER_CODE TMCODE, ACCOUNT_TYPE ACTYPE, CLIENT_ACCOUNTCODE CLIENTCD, INSTRUMENT_TYPE INSTRUMENT_TYPE, SYMBOL SYMBOL, EXPIRY_DATE EXPIRY_DATE, INSTRUMENT_TYPE || SYMBOL || to_char(EXPIRY_DATE,'DDMONYYYY') CONTNAME, STRIKE_PRICE STRIKE_PRICE, decode(OPTION_TYPE,'FF','FX',OPTION_TYPE) OPTIONTYPE, CA_LEVEL CALEVEL, BROUGHT_FORWARD_LONG_QUANTITY BFLONGQTY, BROUGHT_FORWARD_LONG_VALUE BFLONGVAL, BROUGHT_FORWARD_SHORT_QUANTITY BFSHORTQTY, BROUGHT_FORWARD_SHORT_VALUE BFSHORTVAL, DAY_BUY_OPEN_QUANTITY BOPEN_QTY, DAY_BUY_OPEN_VALUE BOPEN_VAL, DAY_SELL_OPEN_QUANTITY SOPEN_QTY, DAY_SELL_OPEN_VALUE SOPEN_VAL, PRE_EXASSGN_LONG_QUANTITY PREEXAS_LONGQTY, PRE_EXASSGN_LONG_VALUE PREEXAS_LONGVAL, PRE_EXASSGN_SHORT_QUANTITY PREEXAS_SHORTQTY, PRE_EXASSGN_SHORT_VALUE  PREEXAS_SHORTVAL, EXERCISED_QUANTITY EXQTY, ASSIGNED_QUANTITY ASQTY, POST_EXASSGN_LONG_QUANTITY POSTEXAS_LONGQTY, POST_EXASSGN_LONG_VALUE POSTEXAS_LONGVAL, POST_EXASSGN_SHORT_QUANTITY POSTEXAS_SHORTQTY, POST_EXASSGN_SHORT_VALUE POSTEXAS_SHORTVAL, SETTLEMENT_PRICE SETTPRICE, NET_PREMIUM NETPREM, DAILY_MTM_SETTLEMENT_VALUE MTMSETT_VAL, FUTURES_FINAL_SETTLEMENT_VALUE FUTFINSETT_VAL, EXERCISEDASSIGNED_VALUE EXASVAL,'' CLOSPRICE,'" + model.Exchange + "'EXCHANGE,'" + model.SessionId + "' SESSIONID,'' SETTNO, '' ORGCODE from " + dbname + ". NSE_POSITION_TEMP_TABLE";
                        ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet(qry, Session["SelectedConn"].ToString());
                        qry = "SELECT COUNT(*) FROM " + dbname + ".CUPOSITION WHERE PDATE = to_date('" + model.Date.ToString("dd-MM-yyyy") + "','dd-MM-yyyy') and Exchange = '" + model.Exchange + "' and SessionId = '" + model.SessionId + "'";
                        // int insertedRowCount = Convert.ToInt32(MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteScalar(qry, Session["SelectedConn"].ToString()));
                        model.IsImportedSuccess = true;
                    }
                    
                }
                TempData["modelData"] = model;
                var response = new { success = true};
                 return RedirectToAction("PositionFileImport", "Process");
                //return Json(response, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                // If an error occurs, return an error response with a message
                var response = new { success = false, errorMessage = "An error occurred." };
                return Json(response,JsonRequestBehavior.AllowGet);
            }
        }
        
        public ActionResult PositionFileOverrideConfirmation(DateTime TradeDate, string Exchange, string SessionId, bool IsOverrideConfirmed,bool IsDeleteConfirmed)
        {
            
            DataSet ds = new DataSet();
            string dbname = MvcApplication.OracleDBHelperCore().OracleDBManager.getDBName("MainConn");
            string qry = null;
            if (IsOverrideConfirmed)
            {
                 qry = "DELETE FROM " + dbname + ".CUPOSITION where PDATE =to_date('" + TradeDate.ToString("dd-MM-yyyy") + "','dd-MM-yyyy') and Exchange = '" + Exchange + "' and SessionId = '" + SessionId + "'";
                ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet(qry, Session["SelectedConn"].ToString());
                qry = "INSERT INTO " + dbname + ".CUPOSITION Select  POSITIONDATE PDATE,SEGMENT_INDICATOR SEG_IND, SETTLEMENT_TYPE SETT_TYPE, CLEARING_MEMBER_CODE CMCODE, MEMBER_TYPE MEMTYPE, TRADING_MEMBER_CODE TMCODE, ACCOUNT_TYPE ACTYPE, CLIENT_ACCOUNTCODE CLIENTCD, INSTRUMENT_TYPE INSTRUMENT_TYPE, SYMBOL SYMBOL, EXPIRY_DATE EXPIRY_DATE, INSTRUMENT_TYPE || SYMBOL || to_char(EXPIRY_DATE,'DDMONYYYY') CONTNAME, STRIKE_PRICE STRIKE_PRICE, decode(OPTION_TYPE,'FF','FX',OPTION_TYPE) OPTIONTYPE, CA_LEVEL CALEVEL, BROUGHT_FORWARD_LONG_QUANTITY BFLONGQTY, BROUGHT_FORWARD_LONG_VALUE BFLONGVAL, BROUGHT_FORWARD_SHORT_QUANTITY BFSHORTQTY, BROUGHT_FORWARD_SHORT_VALUE BFSHORTVAL, DAY_BUY_OPEN_QUANTITY BOPEN_QTY, DAY_BUY_OPEN_VALUE BOPEN_VAL, DAY_SELL_OPEN_QUANTITY SOPEN_QTY, DAY_SELL_OPEN_VALUE SOPEN_VAL, PRE_EXASSGN_LONG_QUANTITY PREEXAS_LONGQTY, PRE_EXASSGN_LONG_VALUE PREEXAS_LONGVAL, PRE_EXASSGN_SHORT_QUANTITY PREEXAS_SHORTQTY, PRE_EXASSGN_SHORT_VALUE  PREEXAS_SHORTVAL, EXERCISED_QUANTITY EXQTY, ASSIGNED_QUANTITY ASQTY, POST_EXASSGN_LONG_QUANTITY POSTEXAS_LONGQTY, POST_EXASSGN_LONG_VALUE POSTEXAS_LONGVAL, POST_EXASSGN_SHORT_QUANTITY POSTEXAS_SHORTQTY, POST_EXASSGN_SHORT_VALUE POSTEXAS_SHORTVAL, SETTLEMENT_PRICE SETTPRICE, NET_PREMIUM NETPREM, DAILY_MTM_SETTLEMENT_VALUE MTMSETT_VAL, FUTURES_FINAL_SETTLEMENT_VALUE FUTFINSETT_VAL, EXERCISEDASSIGNED_VALUE EXASVAL,'' CLOSPRICE,'" + Exchange + "'EXCHANGE,'" + SessionId + "' SESSIONID,'' SETTNO, '' ORGCODE from " + dbname + ". NSE_POSITION_TEMP_TABLE";
                ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet(qry, Session["SelectedConn"].ToString());
                //qry = "SELECT COUNT(*) FROM " + dbname + ".CUPOSITION WHERE PDATE = to_date('" + TradeDate.ToString("dd-MM-yyyy") + "','dd-MM-yyyy') and Exchange = '" + Exchange + "' and SessionId = '" + SessionId + "'";
                //int insertedRowCount = Convert.ToInt32(MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet(qry, Session["SelectedConn"].ToString()));
            }
           else if (IsDeleteConfirmed)
            {
                qry = "DELETE FROM " + dbname + ".CUPOSITION where PDATE =to_date('" + TradeDate.ToString("dd-MM-yyyy") + "','dd-MM-yyyy') and Exchange = '" + Exchange + "' and SessionId = '" + SessionId + "'";
                ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet(qry, Session["SelectedConn"].ToString());
            }

            

            var response = new { success = true };
            return Json(response, JsonRequestBehavior.AllowGet);
        }

        //public ActionResult PostionFileOverride()

        //-------------Contract Master Import ----------------------------

        [HttpGet]
        public ActionResult ContractMasterImport()
        {
            return View();
        }

        //-------------Option Bill Posting----------------------------
        [HttpGet]
        public ActionResult OptionBillPosting()
        {
            return View();
        }

        //-------------Expiry Bill Posting----------------------------
        [HttpGet]
        public ActionResult ExpiryBillPosting()
        {
            return View();
        }

        //-------------Position Matching----------------------------
        
    }
}