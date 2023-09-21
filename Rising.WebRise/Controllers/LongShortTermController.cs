using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;

namespace Rising.WebRise.Controllers.Reports
{
    using Rising.WebRise.Models;
    using OracleDBHelper;


    public class LongShortTermController : Controller
    {
        // GET: LongShortTerm
        public ActionResult Index()
        {
            //for date range
            LongShortTermInput model = new LongShortTermInput();
            model.DateFrom = DateTime.Parse(Session["FinYearFrom"].ToString());
            model.DateTo = System.DateTime.Now;

            return View(model);
        }


        public static LongShortTermOutput lstOut;

        public ActionResult Report(LongShortTermInput model)
        {
            //try
            //{
            WebUser webUser = Session["WebUser"] as WebUser;
            List<DBList> selectedDBLists = Session["SelectedDBLists"] as List<DBList>;
            if (selectedDBLists.Count == 0)
            {
                TempData["AlertMessage"] = "No Segment Selected...";
                return RedirectToAction("Index", "ClientHome");
            }
            if (model.DateFrom > DateTime.Parse("01jan1900") || model.DateTo > DateTime.Parse("01jan1900"))
            {
                List<Oracle.ManagedDataAccess.Client.OracleParameter> lst = new List<Oracle.ManagedDataAccess.Client.OracleParameter>();
                if (webUser.UserType == UserType.Client)
                {

                    lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("CODE", webUser.UserID, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                }
                else if (webUser.UserType == UserType.Branch)
                {
                    DataSet ds1 = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet("SELECT * FROM sysadm.partymst where par_code='" + model.ClientCodeFrom + "' and branchind='" + webUser.UserID + "'", Session["SelectedConn"].ToString());
                    if (ds1.Tables[0].Rows.Count == 0)
                    {
                        TempData["AlertMessage"] = "Invalid Code : " + model.ClientCodeFrom;
                        return RedirectToAction("Index", "Demat");

                    }

                    lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("CODE", model.ClientCodeFrom, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                }
                else if (webUser.UserType == UserType.RM)
                {
                    DataSet ds1 = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet("SELECT * FROM sysadm.partymst where par_code='" + model.ClientCodeFrom + "' and rmcode='" + webUser.UserID + "'", Session["SelectedConn"].ToString());
                    if (ds1.Tables[0].Rows.Count == 0)
                    {
                        TempData["AlertMessage"] = "Invalid Code : " + model.ClientCodeFrom;
                        return RedirectToAction("Index", "Demat");

                    }

                    lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("CODE", model.ClientCodeFrom, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                }
                else if (webUser.UserType == UserType.Admin)
                {
                    lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("CODE", model.ClientCodeFrom, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                }

                lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("DateFrom", model.DateFrom, Oracle.ManagedDataAccess.Client.OracleDbType.Date, ParameterDirection.Input));
                lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("DateTo", model.DateTo, Oracle.ManagedDataAccess.Client.OracleDbType.Date, ParameterDirection.Input));
                lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("ONMKTRATE", "N", Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
                DataSet ds = MvcApplication.OracleDBHelperCore().CustomHelper.CustomDataSetList("SYSADM.CAP_LONG_SHORT", lst, Session["SelectedConn"].ToString());

                lstOut = new LongShortTermOutput();


                lstOut.DateFrom = model.DateFrom;
                lstOut.DateTo = model.DateTo;
                lstOut.ClientCode = webUser.UserID;

                /* 3. turn over   current year */
                lstOut.DsOut1 = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet(@"SELECT round(SUM(NN_TURN_BUY), 2) Jobbing_Buy,round(SUM(NN_TURN_SALE),2)Jobbing_Sale ,round(SUM(DL_TURN_BUY),2)Delivery_Buy ,round(SUM(DL_TURN_SALE),2) Delivery_Sale FROM 
(
SELECT SUM(CASE WHEN TRN_BROKTYPE = 'NN' THEN  ABS(TRN_QTY) * TRN_netRATE  ELSE 0 END) NN_TURN,
SUM((CASE WHEN TRN_BROKTYPE = 'NN' THEN(case when trn_qty > 0 then ABS(TRN_QTY) * TRN_netRATE else 0 end) ELSE 0 END)) NN_TURN_BUY,
SUM((CASE WHEN TRN_BROKTYPE = 'NN' THEN(case when trn_qty > 0 then 0 else ABS(TRN_QTY) * TRN_netRATE end) ELSE 0 END)) NN_TURN_SALE,
SUM((CASE WHEN TRN_BROKTYPE = 'NN' THEN  0 ELSE(CASE WHEN  trn_qty > 0 THEN ABS(TRN_QTY) * TRN_netRATE ELSE 0 END) END)) DL_TURN_BUY ,
SUM((CASE WHEN TRN_BROKTYPE = 'NN' THEN  0 ELSE(CASE WHEN  trn_qty > 0 THEN 0 ELSE ABS(TRN_QTY) * TRN_netRATE END) END)) DL_TURN_SALE ,
SUM((CASE WHEN TRN_BROKTYPE = 'NN' THEN  0 ELSE ABS(TRN_QTY) * TRN_netRATE END)) DL_TURN FROM SYSADM.TRNMAST WHERE  TRN_CLIENTCD = '" + webUser.UserID + "' and trn_date>= to_date('01-04-2020', 'dd-mm-yyyy') and trn_date<= to_date('31-03-2022', 'dd-mm-yyyy')  union all SELECT 0,0,0,0,0,0 from dual)", Session["SelectedConn"].ToString());



                /*  2.TAX   DETAIL  */
                lstOut.DsOut2 = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet(@"select round(SUM(b.NSETAX),2) NSETAX,round(SUM(b.STAX),2) STAX, round(sum(b.stt),2) stt, round(SUM(b.STAMP),2) STAMP, round(SUM(b.DEMCHRG),2) DEMCHRG, round(sum(b.sebi),2) sebi, round(SUM(b.dbrok),2) totminbrok, round(SUM(b.tax1),2) tax1, round(SUM(b.tax2),2) tax2, round(SUM(b.tax3),2) tax3, round(SUM(b.sbc_stax),2) sbc_stax,round(SUM(b.kkc_stax),2) kkc_stax,round(sum(b.taxes),2) taxes, round(SUM(b.CGST),2) + round(SUM(b.SGST),2) + round(SUM(b.IGST),2) + round(SUM(b.UTT),2) TOTALGST,  round(SUM(b.CGST),2) CGST, round(SUM(b.SGST),2) SGST, round(SUM(b.IGST),2) IGST, round(SUM(b.UTT),2) UTT,PAR_NAME,itaxno from ( SELECT SUM(nvl(D.NSETAX,0)) NSETAX,SUM(nvl(D.STAX,0)) STAX, 0 stt,SUM(nvl(D.STAMPDUTY,0)) STAMP,SUM(nvl(D.DEMATCHRG,0)) DEMCHRG, NVL(sum(SEBITAX),0) sebi,SUM(nvl(D.dbrok,0)) dbrok,SUM(nvl(D.tax1,0)) tax1,SUM(nvl(D.tax2,0)) tax2,SUM(nvl(D.tax3,0)) tax3,SUM(nvl(D.sbc_stax,0)) sbc_stax,SUM(nvl(D.kkc_stax,0)) kkc_stax,SUM(nvl(D.CGST,0)) CGST,SUM(nvl(D.SGST,0)) SGST,SUM(nvl(D.IGST,0)) IGST,SUM(nvl(D.UTT,0)) UTT,  sum(nvl(D.CGST,0)+nvl(D.SGST,0)+nvl(D.IGST,0)+nvl(D.UTT,0)+nvl(nsetax,0)+nvl(stax,0)/*+nvl(sttamt,0)*/+nvl(stampduty,0)+nvl(DEMATCHRG,0)+nvl(tax1,0)+nvl(tax2,0)+nvl(tax3,0)+nvl((SEBITAX),0)+nvl((sbc_stax),0)+nvl((kkc_stax),0)) taxes,CLIENTCD FROM   SYSADM.DELV100 D,sysadm.sttfilecap s  WHERE   D.CLIENTCD='" + webUser.UserID + "'  AND D.SETTNO IN (SELECT SETTNO FROM SYSADM.SETTFILE WHERE    PERIODFROM>=TO_DATE('30-03-2017','DD-MM-YYYY')  and PERIODFROM<=TO_DATE('31-03-2022','DD-MM-YYYY')) AND CLIENTCD=clientcode(+) and  d.settno=s.settno(+) AND recordtype(+)='20' and  type(+)='E' GROUP BY clientcd  union all  SELECT  0 NSETAX,0 STAX,  sum(NVL(totalstt,0)) stt,0 STAMP,0 DEMCHRG,0 sebi,0 totminbrok,0 tax1,0 tax2,0 tax3,0 sbc_stax,0 kkc_stax,0 CGST,0 SGST,0 IGST,0 UTT, sum(NVL(totalstt,0)) taxes,clientcode CLIENTCD FROM   sysadm.sttfilebse s  WHERE   S.clientcode='" + webUser.UserID + "'  AND S.SETTNO IN (SELECT SETTNO FROM SYSADM.SETTFILE WHERE    PERIODFROM>=TO_DATE('30-03-2017','DD-MM-YYYY')  and PERIODFROM<=TO_DATE('31-03-2022','DD-MM-YYYY')) AND recordtype='20' and  type='E' GROUP BY clientcode  union all  SELECT  0 NSETAX,0 STAX,  sum(NVL(totalstt,0)) stt,0 STAMP,0 DEMCHRG,0 sebi,0 totminbrok,0 tax1,0 tax2,0 tax3,0 sbc_stax,0 kkc_stax,0 CGST,0 SGST,0 IGST,0 UTT, sum(NVL(totalstt,0)) taxes,clientcode CLIENTCD FROM   sysadm.sttfileCAP s  WHERE   S.clientcode='" + webUser.UserID + "'  AND S.SETTNO IN (SELECT SETTNO FROM SYSADM.SETTFILE WHERE    PERIODFROM>=TO_DATE('30-03-2017','DD-MM-YYYY')  and PERIODFROM<=TO_DATE('31-03-2022','DD-MM-YYYY')) AND recordtype='20' and  type='E' GROUP BY clientcode  )b,sysadm.partymst p where p.par_code= b.CLIENTCD group by PAR_NAME,itaxno", Session["SelectedConn"].ToString());





                /* 4.  Stock Detail  ye nahi dena hai */
                lstOut.DsOut3 = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet(@" SELECT A.TRN_SCRIP,to_char(A.TRN_SDATE,'dd-mm-yyyy') sdate,to_char(A.TRN_DATE,'dd-mm-yyyy') trndate,A.TRN_QTY,A.TRN_QTY1,
(case when A.TRN_MKTRATE>0 then A.TRN_MKTRATE else A.TRN_MKTRATE1 end) rate,A.DAYS,NVL(f.CLOS,nvl(CLG_RATE,0)) CLOS,
isincode,nvl(r.clos,0) clos1,   (TRN_QTY-TRN_QTY1)*(case when A.TRN_MKTRATE>0 then A.TRN_MKTRATE else A.TRN_MKTRATE1 end)*.0000001 stt,
 CASE WHEN A.DAYS<=365 THEN  (TRN_QTY-TRN_QTY1)*(case when A.TRN_MKTRATE>0 then A.TRN_MKTRATE else A.TRN_MKTRATE1 end) -
 (TRN_QTY-TRN_QTY1)*NVL(f.CLOS,nvl(CLG_RATE,0)) ELSE 0 END  unrelstermpl, 
 CASE WHEN A.DAYS>365 THEN  (TRN_QTY-TRN_QTY1)*(case when A.TRN_MKTRATE>0 then A.TRN_MKTRATE else A.TRN_MKTRATE1 end) -
 (TRN_QTY-TRN_QTY1)*NVL(f.CLOS,nvl(CLG_RATE,0)) ELSE 0 END  unreallongtermpl,
 CASE WHEN A.DAYS<=365 THEN  (TRN_QTY-TRN_QTY1)*(case when A.TRN_MKTRATE>0 then A.TRN_MKTRATE else A.TRN_MKTRATE1 end) -
 (TRN_QTY-TRN_QTY1)*NVL(f.CLOS,nvl(CLG_RATE,0)) ELSE 0 END -
 CASE WHEN A.DAYS>365 THEN  (TRN_QTY-TRN_QTY1)*(case when A.TRN_MKTRATE>0 then A.TRN_MKTRATE else A.TRN_MKTRATE1 end) -
 (TRN_QTY-TRN_QTY1)*NVL(f.CLOS,nvl(CLG_RATE,0)) ELSE 0 END  unrealtotalpl,
 (TRN_QTY-TRN_QTY1)*(case when A.TRN_MKTRATE>0 then A.TRN_MKTRATE else A.TRN_MKTRATE1 end) NETVALUE, 
 (TRN_QTY-TRN_QTY1)*NVL(f.CLOS,nvl(CLG_RATE,0)) CLOSEVALUE FROM ( 
 SELECT SH_NAME TRN_SCRIP,TRN_SDATE, TRN_DATE,
SUM(TRN_BALQTY) TRN_QTY,0 TRN_QTY1,ROUND(SUM(TRN_BALQTY*BADLA_RATE)/SUM(TRN_BALQTY),2) 
TRN_MKTRATE,0 TRN_MKTRATE1, to_date('30-03-2021','dd-mm-yyyy')-TRN_SDATE DAYS,SH_CODE,isincode FROM 
SYSADM.longtermweb,SYSADM.SHAREMST S  WHERE BROKTYPE='DL'  AND TRANS_FLAG='D' 
AND TRN_CLIENTCD='" + webUser.UserID + "' AND TRN_SCRIP=S.SH_CODE  GROUP BY isincode,SH_NAME,TRN_SDATE,SH_CODE,trn_date   union all  SELECT SH_NAME TRN_SCRIP,TRN_SDATE ,TRN_DATE, 0 TRN_QTY,SUM(TRN_QTY) TRN_QTY1,0 TRN_MKTRATE,ROUND(SUM(TRN_QTY*TRN_MKTRATE)/SUM(TRN_QTY),2) TRN_MKTRATE1, 0 DAYS,SH_CODE,isincode FROM SYSADM.longtermweb,SYSADM.SHAREMST  WHERE BROKTYPE='DL'  AND TRANS_FLAG is null AND TRN_CLIENTCD='" + webUser.UserID + "' AND TRN_SCRIP=SH_CODE  GROUP BY isincode,SH_NAME,TRN_SDATE,SH_CODE,trn_date  ) A,SYSADM.RATEFILE f,SYSADM.CLGRATE c,sysadm.rate31jan2018 r WHERE A.SH_CODE=f.RATE_CODE(+) AND A.SH_CODE=c.CLG_SCRIPCD(+) AND f.RATE_SETTNO(+)='NN1045' AND c.CLG_SETTNO(+)='BR0145' and  A.SH_CODE=r.rate_code(+) order by A.TRN_SCRIP ,A.TRN_DATE,A.TRN_SDATE", Session["SelectedConn"].ToString());





                /*   1. ltst report  summery   */
                lstOut.DsOut4 = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet(@"SELECT CODE,nvl(SH_NAME,TRN_SCRIP) SH_NAME, ' '  BDATE ,' '  SDATE, round(SUM(QTY_J),2) QTY_JOBB,round(SUM(BUY_J),2) BUY_JOBB,ABS(round(SUM(SALE_J),2)) SALE_JOBB, round(SUM(PL_J),2) PL_JOBB,round(SUM(QTY_SRT),2) QTY_SHORT,round(SUM(BUY_SRT),2) BUY_SHORT, ABS(round(SUM(SALE_SRT),2)) SALE_SHORT,round(SUM(PL_SRT),2) PL_SHORT,round(SUM(QTY_LNG),2) QTY_LONG,round(SUM(BUY_LNG) ,2)BUY_LONG,ABS(round(SUM(SALE_LNG),2)) SALE_LONG,round(SUM(PL_LNG),2) PL_LONG,round(SUM(QTY),2) QTY,round(SUM(BY_AVG),2) BUY_AVG,round(SUM(VALUE) ,2)VALUE,TRN_SCRIP,isincode,nvl(clos,0) clos  , round(SUM(QTY_SRT)* SUM(BUY_SRT),2) QTY_SHORT_BUY_VALUE, round(SUM(QTY_SRT)* SUM(SALE_SRT),2) QTY_SHORT_SALE_VALUE,  round(SUM(QTY_LNG)* SUM(BUY_LNG),2) QTY_LONG_BUY_VALUE, round(SUM(QTY_LNG)* SUM(SALE_LNG),2) QTY_LONG_SALE_VALUE
FROM ( 
SELECT TRN_CLIENTCD CODE,TRN_SCRIP,null BDATE,null SDATE,SUM(TRN_BALQTY) QTY_J,SUM(ABS(TRN_BALQTY)*BADLA_RATE)/(case when SUM(ABS(TRN_BALQTY))=0 then 1 else SUM(ABS(TRN_BALQTY)) end) BUY_J,SUM(TRN_QTY*TRN_MKTRATE)/(case when SUM(TRN_QTY)=0 then 1 else SUM(TRN_QTY) end) SALE_J,SUM(ABS(TRN_QTY)*TRN_MKTRATE)-SUM(TRN_BALQTY*BADLA_RATE) PL_J,0 QTY_SRT,0BUY_SRT,0 SALE_SRT,0 PL_SRT,0 QTY_LNG,  0 BUY_LNG,0 SALE_LNG,0 PL_LNG,0 QTY,0BY_AVG,0 VALUE    
FROM SYSADM.longtermweb
 WHERE  BROKTYPE='NN'  AND TRN_CLIENTCD='" + webUser.UserID + "' GROUP BY TRN_CLIENTCD,TRN_SCRIP,null,null    UNION All  SELECT TRN_CLIENTCD CODE,TRN_SCRIP,null BDATE,null SDATE,0 QTY_J,0 BUY_J,0 SALE_J,0 PL_J,SUM(TRN_BALQTY) QTY_SRT,SUM(TRN_BALQTY*BADLA_RATE)/SUM(TRN_BALQTY) BUY_SRT,SUM(ABS(TRN_QTY)*TRN_MKTRATE)/SUM(ABS(TRN_QTY)) SALE_SRT, SUM(ABS(TRN_QTY)*TRN_MKTRATE)-SUM(TRN_BALQTY*BADLA_RATE) PL_SRT,0 QTY_LNG,0 BUY_LNG,0 SALE_LNG,0 PL_LNG,0 QTY,0 BY_AVG,0 VALUE FROM SYSADM.longtermweb WHERE BROKTYPE='DL'  AND TRANS_FLAG='M' AND DAYS<365  AND TRN_CLIENTCD='" + webUser.UserID + "' GROUP BY TRN_CLIENTCD,TRN_SCRIP,null,null   UNION All SELECT TRN_CLIENTCD CODE,TRN_SCRIP,null BDATE,null SDATE,0 QTY_J,0 BUY_J,0 SALE_J,0 PL_J,0 QTY_SRT,0 BUY_SRT,0 SALE_SRT,0 PL_SRT,SUM(TRN_BALQTY) QTY_LNG,SUM(TRN_BALQTY*BADLA_RATE)/SUM(TRN_BALQTY) BUY_LNG,SUM(ABS(TRN_QTY)*TRN_MKTRATE)/SUM(ABS(TRN_QTY)) SALE_LNG,SUM(ABS(TRN_QTY)*TRN_MKTRATE)-SUM(TRN_BALQTY*BADLA_RATE) PL_LNG,0 QTY,0 BY_AVG,0 VALUE  FROM SYSADM.longtermweb WHERE BROKTYPE='DL'  AND TRANS_FLAG='M' AND DAYS>=365  AND TRN_CLIENTCD='" + webUser.UserID + "'  GROUP BY TRN_CLIENTCD,TRN_SCRIP,null,null   UNION All SELECT TRN_CLIENTCD CODE,TRN_SCRIP,null BDATE,null SDATE,0 QTY_J,0BUY_J,0 SALE_J,0 PL_J,0 QTY_SRT,0 BUY_SRT,0 SALE_SRT,0 PL_SRT,0 QTY_LNG,0 BUY_LNG,0 SALE_LNG,0 PL_LNG,SUM(TRN_BALQTY) QTY,ROUND(SUM(TRN_BALQTY*BADLA_RATE)/DECODE(SUM(TRN_BALQTY),'0',1,SUM(TRN_BALQTY)),4) BY_AVG,sum(BADLA_RATE*TRN_BALQTY) value   FROM SYSADM.longtermweb WHERE BROKTYPE='DL'  AND TRANS_FLAG='D'  AND TRN_CLIENTCD='" + webUser.UserID + "' GROUP BY TRN_CLIENTCD,TRN_SCRIP,null,null    ),SYSADM.SHAREMST SH,sysadm.rate31jan2018 r   WHERE TRN_SCRIP=SH_CODE and trn_scrip=rate_code(+)   GROUP BY isincode,CODE,TRN_SCRIP, SH_NAME,clos      ORDER BY nvl(SH_NAME,TRN_SCRIP)   ", Session["SelectedConn"].ToString());





                /* 5. ltst date range  */
                lstOut.DsOut5 = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet(@"SELECT CODE,nvl(SH_NAME,TRN_SCRIP) SH_NAME,  BDATE ,  SDATE,round(SUM(QTY_J),2) QTY_JOBB,round(SUM(BUY_J),2) BUY_JOBB,ABS(round(SUM(SALE_J),2)) SALE_JOBB, round(SUM(PL_J),2) PL_JOBB,round(SUM(QTY_SRT),2) QTY_SHORT,round(SUM(BUY_SRT),2) BUY_SHORT, ABS(round(SUM(SALE_SRT),2)) SALE_SHORT,round(SUM(PL_SRT),2) PL_SHORT,round(SUM(QTY_LNG),2) QTY_LONG,round(SUM(BUY_LNG),2) BUY_LONG,ABS(round(SUM(SALE_LNG),2)) SALE_LONG,round(SUM(PL_LNG),2) PL_LONG,round(SUM(QTY),2) QTY,round(SUM(BY_AVG),2) BUY_AVG,round(SUM(VALUE),2) VALUE,TRN_SCRIP,isincode,nvl(clos,0) clos,trn_date ,trn_sdate 
FROM ( 
SELECT TRN_CLIENTCD CODE,TRN_SCRIP,to_char(trn_date,'dd-mm-yyyy') BDATE,to_char(trn_sdate,'dd-mm-yyyy') SDATE,SUM(TRN_BALQTY) QTY_J,SUM(ABS(TRN_BALQTY)*BADLA_RATE)/(case when SUM(ABS(TRN_BALQTY))=0 then 1 else SUM(ABS(TRN_BALQTY)) end) BUY_J,SUM(TRN_QTY*TRN_MKTRATE)/(case when SUM(TRN_QTY)=0 then 1 else SUM(TRN_QTY) end) SALE_J,SUM(ABS(TRN_QTY)*TRN_MKTRATE)-SUM(TRN_BALQTY*BADLA_RATE) PL_J,0 QTY_SRT,0BUY_SRT,0 SALE_SRT,0 PL_SRT,0 QTY_LNG,  0 BUY_LNG,0 SALE_LNG,0 PL_LNG,0 QTY,0BY_AVG,0 VALUE ,trn_date ,trn_sdate  
FROM SYSADM.longtermweb
 WHERE  BROKTYPE='NN'  AND TRN_CLIENTCD='" + webUser.UserID + "' GROUP BY TRN_CLIENTCD,TRN_SCRIP,to_char(trn_date,'dd-mm-yyyy'),to_char(trn_sdate,'dd-mm-yyyy') ,trn_date ,trn_sdate   UNION All  SELECT TRN_CLIENTCD CODE,TRN_SCRIP,to_char(trn_date,'dd-mm-yyyy') BDATE,to_char(trn_sdate,'dd-mm-yyyy') SDATE,0 QTY_J,0 BUY_J,0 SALE_J,0 PL_J,SUM(TRN_BALQTY) QTY_SRT,SUM(TRN_BALQTY*BADLA_RATE)/SUM(TRN_BALQTY) BUY_SRT,SUM(ABS(TRN_QTY)*TRN_MKTRATE)/SUM(ABS(TRN_QTY)) SALE_SRT, SUM(ABS(TRN_QTY)*TRN_MKTRATE)-SUM(TRN_BALQTY*BADLA_RATE) PL_SRT,0 QTY_LNG,0 BUY_LNG,0 SALE_LNG,0 PL_LNG,0 QTY,0 BY_AVG,0 VALUE ,trn_date ,trn_sdate  FROM SYSADM.longtermweb WHERE BROKTYPE='DL'  AND TRANS_FLAG='M' AND DAYS<365  AND TRN_CLIENTCD='" + webUser.UserID + "' GROUP BY TRN_CLIENTCD,TRN_SCRIP,to_char(trn_date,'dd-mm-yyyy'),to_char(trn_sdate,'dd-mm-yyyy') ,trn_date ,trn_sdate UNION All SELECT TRN_CLIENTCD CODE,TRN_SCRIP,to_char(trn_date,'dd-mm-yyyy') BDATE,to_char(trn_sdate,'dd-mm-yyyy') SDATE,0 QTY_J,0 BUY_J,0 SALE_J,0 PL_J,0 QTY_SRT,0 BUY_SRT,0 SALE_SRT,0 PL_SRT,SUM(TRN_BALQTY) QTY_LNG,SUM(TRN_BALQTY*BADLA_RATE)/SUM(TRN_BALQTY) BUY_LNG,SUM(ABS(TRN_QTY)*TRN_MKTRATE)/SUM(ABS(TRN_QTY)) SALE_LNG,SUM(ABS(TRN_QTY)*TRN_MKTRATE)-SUM(TRN_BALQTY*BADLA_RATE) PL_LNG,0 QTY,0 BY_AVG,0 VALUE ,trn_date ,trn_sdate  FROM SYSADM.longtermweb WHERE BROKTYPE='DL'  AND TRANS_FLAG='M' AND DAYS>=365  AND TRN_CLIENTCD='" + webUser.UserID + "'  GROUP BY TRN_CLIENTCD,TRN_SCRIP,to_char(trn_date,'dd-mm-yyyy'),to_char(trn_sdate,'dd-mm-yyyy')  ,trn_date ,trn_sdate UNION All SELECT TRN_CLIENTCD CODE,TRN_SCRIP,to_char(trn_date,'dd-mm-yyyy') BDATE,to_char(trn_sdate,'dd-mm-yyyy') SDATE,0 QTY_J,0BUY_J,0 SALE_J,0 PL_J,0 QTY_SRT,0 BUY_SRT,0 SALE_SRT,0 PL_SRT,0 QTY_LNG,0 BUY_LNG,0 SALE_LNG,0 PL_LNG,SUM(TRN_BALQTY) QTY,ROUND(SUM(TRN_BALQTY*BADLA_RATE)/DECODE(SUM(TRN_BALQTY),'0',1,SUM(TRN_BALQTY)),4) BY_AVG,sum(BADLA_RATE*TRN_BALQTY) value  ,trn_date ,trn_sdate  FROM SYSADM.longtermweb WHERE BROKTYPE='DL'  AND TRANS_FLAG='D'  AND TRN_CLIENTCD='" + webUser.UserID + "' GROUP BY TRN_CLIENTCD,TRN_SCRIP,to_char(trn_date,'dd-mm-yyyy'),to_char(trn_sdate,'dd-mm-yyyy') ,trn_date ,trn_sdate  ),SYSADM.SHAREMST SH,sysadm.rate31jan2018 r   WHERE TRN_SCRIP=SH_CODE and trn_scrip=rate_code(+)   GROUP BY isincode,CODE,TRN_SCRIP, SH_NAME,clos ,bdate,sdate ,trn_date ,trn_sdate   ORDER BY nvl(SH_NAME,TRN_SCRIP) ,trn_sdate ,trn_date ", Session["SelectedConn"].ToString());
                Session["ReportHeader1"] = "Long short report";
                Session["ReportHeader2"] = "Date Range : " + model.DateFrom.ToString("dd/MM/yyyy") + " - " + model.DateTo.ToString("dd/MM/yyyy");
                return View(lstOut);
            }
            return RedirectToAction("Index", "LongShortTerm");
            //}
            //catch (Exception ex)
            //{
            //    return View();
            //}
        }


        public ActionResult OpeningStock(OpeningStock model)
        {
            WebUser webUser = Session["WebUser"] as WebUser;
            List<DBList> selectedDBLists = Session["SelectedDBLists"] as List<DBList>;

            if (selectedDBLists.Count == 0)
            {
                TempData["AlertMessage"] = "No Segment Selected...";
                return RedirectToAction("Index", "ClientHome");
            }

            if (model.OpeningDate.ToString("ddMMyyyy") == "01010001") model.OpeningDate = System.DateTime.Now;
            if (model.ScripCode != null)
            {
                string str = "select SYSADM.sysdbsequence.nextval from dual";
                DataSet ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet(str, Session["SelectedConn"].ToString());
                string ctlno = ds.Tables[0].Rows[0][0].ToString();

                str = "insert into SYSADM.TRNMAST_OPN(TRN_SLNO,  TRN_DATE, TRN_SCRIP, TRN_CLIENTCD, TRN_SETTNO, TRN_QTY, TRN_MKTRATE, TRN_NETRATE,TRN_BALQTY,TRN_DELVSETTNO) values('" + ctlno.ToString() + "','" + model.OpeningDate.ToString("ddMMMyyyy") + "','" + model.ScripCode + "', '" + webUser.UserID + "','OPEN', '" + model.Quantity + "','" + model.Rate + "','" + model.Rate + "','0','0')";
                MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteNonQuery(str, Session["SelectedConn"].ToString());
            }


            string qry = "select TRN_SCRIP, SH_NAME, to_char(TRN_DATE,'dd-mm-yyyy') trndate, TRN_QTY, TRN_MKTRATE from SYSADM.TRNMAST_OPN T, SYSADM.SHAREMST S WHERE TRN_CLIENTCD='" + webUser.UserID + "' AND T.TRN_SCRIP=S.SH_CODE";

            List<Oracle.ManagedDataAccess.Client.OracleParameter> lst = new List<Oracle.ManagedDataAccess.Client.OracleParameter>();

            DataSet ds1 = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet(qry, Session["SelectedConn"].ToString());
            model.DsOut1 = ds1;


            return View(model);
        }
    }
}