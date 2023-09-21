using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Data;


namespace Rising.WebRise.Controllers
{
    using Rising.WebRise.Models;
    using OracleDBHelper;

    public class TradeEditController : Controller
    {
        // GET: TradeEdit
        public ActionResult Index(TradeEdit model)
        {
            try
            {
                WebUser webUser = Session["WebUser"] as WebUser;
                if (webUser == null) return null;

                string str = "select  TRN_CLIENTCD, PAR_NAME, TRN_SCRIP,TRN_SYMBOL,TRN_QTY,TRN_MKTRATE, TRN_NETRATE,TRN_TIME,TRN_ORDNO,TRN_TRDNO,t.ROWID,sh_name,trn_delvsettno,trn_orgclientid,trn_branch,trn_ctclid,trn_ordtime,BRCODE,RM_CODE,type,INTRO_BY,client_statecd,DEALING_STATECD,trn_shortdlvflag from SYSADM.trnmast t, SYSADM.sharemst S, SYSADM.PARTYMST P WHERE P.PAR_CODE = T.TRN_CLIENTCD AND sh_code(+) = trn_scrip  and trn_settno = '"+model.SettNo+"' and post100 is null  and trn_balqty <> 0 and trn_trntype not in('X', 'M')  and trn_clientcd<> 'CNSE' and trn_clientcd='"+model.ClientCodeFrom+"' ";

                model.TradeEditRows = new List<TradeEditRow>();
                DataSet ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet(str, Session["SelectedConn"].ToString());
                int cnt = 0;
                foreach (System.Data.DataRow rw in ds.Tables[0].Rows)
                {
                    TradeEditRow ter = new TradeEditRow();
                    ter.ClientCode = ds.Tables[0].Rows[cnt]["TRN_CLIENTCD"].ToString();
                    ter.ClientName = ds.Tables[0].Rows[cnt]["PAR_NAME"].ToString();
                    ter.ScripCode = ds.Tables[0].Rows[cnt]["TRN_SCRIP"].ToString();
                    ter.ScripName = ds.Tables[0].Rows[cnt]["TRN_SYMBOL"].ToString();
                    ter.Qty = float.Parse(ds.Tables[0].Rows[cnt]["TRN_QTY"].ToString());
                    ter.MktRate = float.Parse(ds.Tables[0].Rows[cnt]["TRN_MKTRATE"].ToString());
                    ter.NetRate = float.Parse(ds.Tables[0].Rows[cnt]["TRN_NETRATE"].ToString());
                   // ter.se = ds.Tables[0].Rows[cnt]["trn_delvsettno"].ToString();
                    ter.OrderNo = float.Parse(ds.Tables[0].Rows[cnt]["TRN_ORDNO"].ToString());
                    ter.OrderTime = DateTime.Parse(ds.Tables[0].Rows[cnt]["trn_ordtime"].ToString());
                    ter.TradeNo = float.Parse(ds.Tables[0].Rows[cnt]["TRN_TRDNO"].ToString());
                    ter.TradeTime = DateTime.Parse(ds.Tables[0].Rows[cnt]["TRN_TIME"].ToString());
                    ter.OrgClient = ds.Tables[0].Rows[cnt]["trn_orgclientid"].ToString();
                    ter.UserID = ds.Tables[0].Rows[cnt]["trn_branch"].ToString();
                    ter.CtclID = ds.Tables[0].Rows[cnt]["trn_ctclid"].ToString();
                    ter.RowID = ds.Tables[0].Rows[cnt]["rowid"].ToString();
                    model.TradeEditRows.Add(ter);
                }

                return View(model);
            }
            catch (Exception ex)
            {
                return View();
            }
        }


        public ActionResult SaveTradeEdit(TradeEdit model)
        {
            try
            {
                WebUser webUser = Session["WebUser"] as WebUser;
                if (webUser == null) return null;
                      
                if(model.TradeEditRows!=null)
                {

                     
                foreach(TradeEditRow ter in model.TradeEditRows)
                {
                    MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteNonQuery("update SYSADM.trnmast set TRN_CLIENTCD='"+ter.ClientCode+"' where rowid='"+ter.RowID+"'", Session["SelectedConn"].ToString());
                }
                }
                return RedirectToAction("Index", model);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Index", model);
            }
        }
    }
}