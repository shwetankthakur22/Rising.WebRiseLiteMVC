using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rising.OracleDBHelper
{
    using OracleDBManager;

    public class CustomHelper
    {
        public OracleDBManager OracleDBManager { get; set; }

        public CustomHelper(OracleDBManager OracleDBManager)
        {
            this.OracleDBManager = OracleDBManager;
        }

        public DataSet CustomDataSet(string procedureName, List<Oracle.ManagedDataAccess.Client.OracleParameter> parameters, string connName)
        {
            //try
            //{
                if (parameters == null) parameters = new List<Oracle.ManagedDataAccess.Client.OracleParameter>();

                parameters.Add(this.OracleDBManager.OracleParameter("@p_recordset", "", Oracle.ManagedDataAccess.Client.OracleDbType.RefCursor, System.Data.ParameterDirection.Output));
                return this.OracleDBManager.ExecuteDataSet(procedureName, parameters, connName);
            //}
            //catch (Exception ex)
            //{
            //    System.Windows.Forms.MessageBox.Show(ex.Message);
            //    return null;
            //}
        }

        public DataSet CustomDataSetList(string procedureName, List<Oracle.ManagedDataAccess.Client.OracleParameter> parameters, string connName)
        {
            //try
            //{
                if (parameters == null) parameters = new List<Oracle.ManagedDataAccess.Client.OracleParameter>();                
                return this.OracleDBManager.ExecuteDataSetList(procedureName, parameters, connName);
            //}
            //catch (Exception ex)
            //{
            //    System.Windows.Forms.MessageBox.Show(ex.Message);
            //    return null;
            //}
        }

        public void ExecuteNonQuery(string procedureName, List<Oracle.ManagedDataAccess.Client.OracleParameter> parameters, string connName)
        {
            //try
            //{
                if (parameters == null) parameters = new List<Oracle.ManagedDataAccess.Client.OracleParameter>();
                this.OracleDBManager.ExecuteNonQuery(procedureName, parameters, connName);
            //}
            //catch (Exception ex)
            //{
            //    System.Windows.Forms.MessageBox.Show(ex.Message);
            //}
        }

        public DataSet CustomDataSet(string query, string connName)
        {
            //try
            //{
                return this.OracleDBManager.ExecuteDataSet(query,connName);
            //}
            //catch (Exception ex)
            //{
            //    System.Windows.Forms.MessageBox.Show(ex.Message);
            //    return null;
            //}
        }

        public void ExecuteNonQuery(string Query, string connName)
        {
            //try
            //{                
                this.OracleDBManager.ExecuteNonQuery(Query, connName);
            //}
            //catch (Exception ex)
            //{
            //    System.Windows.Forms.MessageBox.Show(ex.Message);
            //}
        }

        public DataSet ExecuteDataSet(string Query, string connName)
        {
            //try
            //{               
                return this.OracleDBManager.ExecuteDataSet(Query, connName);
            //}
            //catch (Exception ex)
            //{               
            //    System.Windows.Forms.MessageBox.Show(ex.Message);
            //    return null;
            //}
        }
    }
}
