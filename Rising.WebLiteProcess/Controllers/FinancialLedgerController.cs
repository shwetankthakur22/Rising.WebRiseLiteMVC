using Rising.WebRise.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Rising.WebRise.Controllers
{
    public class FinancialLedgerController : Controller
    {
        private List<BillDetails> bd = new List<Models.BillDetails>();

        // GET: FinancialLedger
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult BillDetails(string BillNo)
        {
            DataSet ds = null;
            
            try
            {
                if (BillNo != null)
                {

                    ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet("Select f.exchange,to_char(bdate,'yyyymmdd')||par_code gr,b.billdate,pa.company,pa.add1,pa.add2,pa.add3,pa.add4,par_code,p.par_name,par_add1,par_add2,par_add3, par_add4, b.billno, b.bdate, f.instrument_type || f.symbol || to_char(f.expirydate, 'ddMONyyyy') as Contract, trade_date, F.OPTIONTYPE, MULTIPLIER,trn_qty, tradeprice, strikeprice, netprice, tradestatus, abs(NVL(TRN_BROK, 0)) as diff, ABS(decode(sign(trn_qty), 1, decode(p1.BROKERAGEMETHOD, 'Pay',(tradeprice * trn_qty * multiplier) + NVL(TRN_BROK, 0), 'Charge', (tradeprice * trn_qty * multiplier) + NVL(TRN_BROK, 0), (tradeprice * trn_qty * multiplier)), 0)) AS PVALUE,ABS(decode(sign(trn_qty), -1, decode(p1.brokeragemethod, 'Pay', (tradeprice * trn_qty * multiplier) + NVL(TRN_BROK, 0), 'Charge',(tradeprice * trn_qty * multiplier) + NVL(TRN_BROK, 0), (tradeprice * trn_qty * multiplier)), 0)) AS SVALUE, NVL(B.STAX, 0) AS stax, nvl(b.nsetax, 0) nsetax,nvl(b.stamp, 0) Stamp, nvl(b.stax_turn, 0) stax_turn, nvl(b.stax_stamp, 0) stax_stamp, NVL(b.TAX1, 0) TAX1, NVL(b.TAX2, 0) TAX2, NVL(b.STAX_TAX1, 0) STAX_TAX1,NVL(b.STAX_TAX2, 0) STAX_TAX2, nvl(pa.tax1narr, 'Tax1 Charges') tax1narr, pa.tax1control tax1control, nvl(pa.tax2narr, 'Tax2 Charges') tax2narr,pa.tax2control tax2control, pa.stampdutynarr, NVL(b.TAX3, 0) TAX3, NVL(b.STAX_TAX3, 0) STAX_TAX3, nvl(pa.tax3narr, 'Tax3 Charges') tax3narr,pa.tax3control tax3control, NVL(b.TAX4, 0) TAX4, nvl(pa.tax4narr, 'Tax4 Charges') tax4narr, pa.tax4control tax4control, nvl(sbc_stax, 0) sbc_stax,nvl(kkc_stax, 0) kkc_stax from IFSC.cutrnmast f, IFSC.CUPARTYMST p, IFSC.CUBILL b, IFSC.CUPARA pa, IFSC.CUPARTYMST_fixes p1 Where f.clientcode = P.PAR_CODE  and f.clientcode = b.clientid and f.trade_date = b.bdate and P1.PARTY_CD = F.CLIENTCODE and trade_date >= TO_DATE('08-08-2023', 'DD-MM-YYYY') and trade_date <= TO_DATE('08-08-2023', 'DD-MM-YYYY') and f.exchange = 'NSE' and f.exchange = b.exchange AND P1.EXCHANGE = F.EXCHANGE AND PA.EXCHANGE = F.EXCHANGE  AND TRADE_DATE <> F.EXPIRYDATE AND BILLNO LIKE 'DLYMTM%' AND INSTRUMENT_TYPE LIKE 'FUT%' AND BillNo = '" + BillNo + "'", Session["SelectedConn"].ToString());



                    //foreach(DataRow dr)
                    //bd = ds.Tables[0].AsEnumerable().Select(datarow => new BillDetails 
                    //{ 
                          //EXCHANGE = datarow.Field<string>("EXCHANGE"),
                    //    GR = datarow.Field<string>("GR"),
                    //    BILLDATE = datarow.Field<string>("BILLDATE"),
                    //    COMPANY = datarow.Field<string>("COMPANY"),
                    //    ADD1 = datarow.Field<string>("ADD1"),
                    //    ADD2 = datarow.Field<string>("ADD2"),
                    //    ADD3 = datarow.Field<string>("ADD3"),
                    //    ADD4 = datarow.Field<string>("ADD4"),
                    //    PAR_CODE = datarow.Field<string>("PAR_CODE"),
                    //    PAR_NAME = datarow.Field<string>("PAR_NAME"),
                    //    PAR_ADD1 = datarow.Field<string>("PAR_ADD1"),
                    //    PAR_ADD2 = datarow.Field<string>("PAR_ADD2"),
                    //    PAR_ADD3 = datarow.Field<string>("PAR_ADD3"),
                    //    PAR_ADD4 = datarow.Field<string>("PAR_ADD4"),
                    //    BILLNO = datarow.Field<string>("BILLNO"),
                    //    BDATE = datarow.Field<string>("BDATE"),
                    //    CONTRACT = datarow.Field<string>("CONTRACT"),
                    //    TRADE_DATE = datarow.Field<string>("TRADE_DATE"),
                    //    OPTIONTYPE = datarow.Field<string>("OPTIONTYPE"),
                    //    MULTIPLIER = datarow.Field<string>("MULTIPLIER"),
                    //    TRN_QTY = datarow.Field<string>("TRN_QTY"),
                    //    TRADEPRICE = datarow.Field<string>("TRADEPRICE"),
                    //    STRIKEPRICE = datarow.Field<string>("STRIKEPRICE"),
                    //    NETPRICE = datarow.Field<string>("NETPRICE"),
                    //    TRADESTATUS = datarow.Field<string>("TRADESTATUS"),
                    //    DIFF = datarow.Field<string>("DIFF"),
                    //    PVALUE = datarow.Field<string>("PVALUE"),
                    //    SVALUE = datarow.Field<string>("SVALUE"),
                    //    STAX = datarow.Field<string>("STAX"),
                    //    NSETAX = datarow.Field<string>("NSETAX"),
                    //    STAMP = datarow.Field<string>("STAMP"),
                    //    STAX_TURN = datarow.Field<string>("STAX_TURN"),
                    //    STAX_STAMP = datarow.Field<string>("STAX_STAMP"),
                    //    TAX1 = datarow.Field<string>("TAX1"),
                    //    TAX2 = datarow.Field<string>("TAX2"),
                    //    TAX3 = datarow.Field<string>("TAX3"),
                    //    TAX4 = datarow.Field<string>("TAX4"),
                    //    TAX1NARR = datarow.Field<string>("TAX1NARR"),
                    //    TAX2NARR = datarow.Field<string>("TAX2NARR"),
                    //    TAX3NARR = datarow.Field<string>("TAX3NARR"),
                    //    TAX4NARR = datarow.Field<string>("TAX4NARR"),
                    //    TAX1CONTROL = datarow.Field<string>("TAX1CONTROL"),
                    //    TAX2CONTROL = datarow.Field<string>("TAX2CONTROL"),
                    //    TAX3CONTROL = datarow.Field<string>("TAX3CONTROL"),
                    //    TAX4CONTROL = datarow.Field<string>("TAX4CONTROL"),
                    //    STAMPDUTYNARR = datarow.Field<string>("STAMPDUTYNARR"),
                    //    SBC_STAX = datarow.Field<string>("SBC_STAX"),
                    //    KKC_STAX = datarow.Field<string>("KKC_STAX")
                    //}).ToList();
                    

                }
                else
                {

                }

                
            }
            catch (Exception ex)
            {

            }
            return View(bd);
        }
                
        
    }
}