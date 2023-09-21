using System.Collections.Generic;
using Rising.WebRise.Models;
using Rising.WebRise.Repositories.Abstract;
using System.Data;
using System.Configuration;
using System.Web;
using Rising.WebRiseProecss.Models;

namespace Rising.WebRise.Repositories.Implementation
{
    public class CommonRef : ICommonRef
    {
        public List<Exchange> GetExchange()
        {
            string dbuser = ConfigurationManager.AppSettings["DBUSER"];
            List<Exchange> exgList = new List<Exchange>();

            // Checking if Session is not null and has the key "SelectedConn"
            if (HttpContext.Current.Session != null && HttpContext.Current.Session["SelectedConn"].ToString() != null)
            {
                //string selectedConn = HttpContext.Current.Session["SelectedConn"].ToString();

              

                string qry = "Select distinct segment from " + dbuser + ".Partytrn where segment is not null";
                DataSet ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet(qry, HttpContext.Current.Session["SelectedConn"].ToString());

                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        // GetExchange has a constructor that accepts a string
                        Exchange exchangeItem = new Exchange();
                        exchangeItem.ExchangeName = (row["segment"].ToString());
                        exgList.Add(exchangeItem);
                    }
                }
            }
            else
            {
                
            }

            return exgList;
        }
    }
   
}
