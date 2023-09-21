using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Rising.OracleDBHelper;
using Oracle.ManagedDataAccess;

namespace Utility
{
    using Rising.WebRise.Models;
    using OracleDBHelper;

    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            List<Oracle.ManagedDataAccess.Client.OracleParameter> lst = new List<Oracle.ManagedDataAccess.Client.OracleParameter>();

           

                lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("clientcode_", model.ClientCodeFrom, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, ParameterDirection.Input));
           
            lst.Add(MvcApplication.OracleDBHelperCore().OracleDBManager.OracleParameter("AsOnDate_", model.AsOnDate, Oracle.ManagedDataAccess.Client.OracleDbType.Date, ParameterDirection.Input));
            DataSet ds = MvcApplication.OracleDBHelperCore().CustomHelper.CustomDataSet("SYSADM.N$GET_DematHolding", lst, Session["SelectedConn"].ToString());

            lstOut = new DematHoldingOutput();
            lstOut.listDematHoldingOutputRow = new List<DematHoldingOutputRow>();
            lstOut.ClientCode = webUser.UserID;
            lstOut.ClientName = webUser.UserName;
            lstOut.AsOnDate = model.AsOnDate;

            int c = 0;
            foreach (DataRow row in ds.Tables[0].Rows)
            {
                if (c == 0)
                {
                    lstOut.ClientCode = row["Code"].ToString();
                    lstOut.ClientName = row["Name"].ToString();
                }
                c++;
                DematHoldingOutputRow bdo = new DematHoldingOutputRow();


                bdo.ScripCode = row["SCRIPCD"].ToString();
                bdo.ScripName = row["SH_NAME"].ToString();
                bdo.ScripIsin = row["ISINCODE"].ToString();
                bdo.Qty = decimal.Parse(row["Qty"].ToString());
                bdo.Stock = decimal.Parse(row["Stock"].ToString());
                bdo.LockQty = decimal.Parse(row["Qty_lock"].ToString());
                bdo.CDSLQty = decimal.Parse(row["cdsl_qty"].ToString());
                bdo.NSDLQty = decimal.Parse(row["nsdl_qty"].ToString());
                bdo.TotalQty = decimal.Parse(row["tot_qty"].ToString());
                bdo.Rate = decimal.Parse(row["rate"].ToString());
                bdo.Value = decimal.Parse(row["stk_val"].ToString());
                bdo.VarPer = decimal.Parse(row["rate_var"].ToString());
                bdo.VarValue = decimal.Parse(row["varvalue"].ToString());
                lstOut.listDematHoldingOutputRow.Add(bdo);
            }
            Session["ReportHeader1"] = "Demat Holding";
            Session["ReportHeader2"] = "As On Date : " + model.AsOnDate.ToString("dd/MM/yyyy");

            return View(lstOut);

            //}
            //catch (Exception ex)
            //{
            //    return View();
            //}
        }
    }
}
