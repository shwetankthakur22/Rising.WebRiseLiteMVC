using Rising.WebRise.Repositories.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Rising.WebRise.Models;
using System.Configuration;
using System.Data;
using Rising.WebRiseProcess.Models;

namespace Rising.WebRise.Repositories.Implementation
{
    public class SymbolImp : ISymbol
    {
        public List<Symbol> GetSymbol()
        {
            string dbuser = ConfigurationManager.AppSettings["DBUSER"];
             List<Symbol> symbolList = new List<Symbol>();

            if (HttpContext.Current.Session != null && HttpContext.Current.Session["SelectedConn"].ToString() != null)
            {
                string qry = "Select distinct INSTRUMENT_TYPE from " + dbuser + ".cucontracts where INSTRUMENT_TYPE is not null";
                DataSet ds = MvcApplication.OracleDBHelperCore().CustomHelper.ExecuteDataSet(qry, HttpContext.Current.Session["SelectedConn"].ToString());
                if (ds.Tables.Count > 0 && ds.Tables[0].Rows.Count > 0)
                {
                    foreach (DataRow row in ds.Tables[0].Rows)
                    {
                        // GetExchange has a constructor that accepts a string
                        Symbol symbolItem = new Symbol();
                        symbolItem.SymbolName = (row["INSTRUMENT_TYPE"].ToString());
                        symbolList.Add(symbolItem);
                    }
                }
            }
            else
            {

            }


                return symbolList;
        }
        
    }
}