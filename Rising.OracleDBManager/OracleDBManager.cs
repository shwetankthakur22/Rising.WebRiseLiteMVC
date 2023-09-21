using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Configuration;
using System.Data;

namespace Rising.OracleDBManager
{
    using Oracle.ManagedDataAccess.Client;   
  
    public class OracleDBManager
    {
        public OracleConnection MainConn { get; set; }

        public OracleConnection MainConn2 { get; set; }

        public OracleConnection MainConn3 { get; set; }

        public Dictionary<string, OracleConnection> ConnLists { get; set; }


        public List<OracleParameter> Parameters { get; set; }

        public OracleDataReader OracleDataReader { get; set; }

        public OracleDBManager()
        {
           
        }

        public void loadDB(string connName, string connName2, string connName3)
        {
            try
            {
                ConnLists = new Dictionary<string, OracleConnection>();
                MainConn = new OracleConnection(ConfigurationManager.ConnectionStrings[connName].ConnectionString.Replace("Password=#########", "Password=P5543453$3"));

                MainConn2 = new OracleConnection(ConfigurationManager.ConnectionStrings[connName2].ConnectionString.Replace("Password=#########", "Password=P5543453$3"));

                MainConn3 = new OracleConnection(ConfigurationManager.ConnectionStrings[connName3].ConnectionString.Replace("Password=#########", "Password=P5543453$3"));

                ConnLists.Add("MainConn", MainConn);
                ConnLists.Add("MainConn2", MainConn2);
                ConnLists.Add("MainConn3", MainConn3);
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
            }
           
        }

        public void initialiseParameters()
        {
            Parameters = new List<Oracle.ManagedDataAccess.Client.OracleParameter>();
        }

        public OracleParameter OracleParameter(string name, object value, OracleDbType type, ParameterDirection direction)
        {
            OracleParameter para = new Oracle.ManagedDataAccess.Client.OracleParameter();
            para.ParameterName = name;
            para.Value = value;
            para.Direction = direction;
            para.OracleDbType = type;
            return para;
        }

        public OracleParameter OracleParameter(string name, object value, string type, ParameterDirection direction = ParameterDirection.Input)
        {
            OracleParameter para = new Oracle.ManagedDataAccess.Client.OracleParameter();
            para.ParameterName = name;
            para.Value = value;
            para.Direction = direction;
            para.OracleDbType = (OracleDbType)Enum.Parse(typeof(OracleDbType), type);
            return para;
        }



        public string getDBName(string connName)
        {
            OracleConnection conn = this.ConnLists[connName];           
            return conn.DataSource.Split('/')[1];
        }

        public OracleDataReader ExecuteReader(string ProcedureName, List<OracleParameter> parameters, string connName)
        {
            //try
            //{
            OracleConnection conn = this.ConnLists[connName];
            OracleCommand command = new OracleCommand(ProcedureName, conn);
            command.CommandType = CommandType.StoredProcedure;
            foreach (OracleParameter parameter in parameters)
            {
                command.Parameters.Add(parameter);
            }
            conn.Close();
            conn.Open();
            return command.ExecuteReader(CommandBehavior.CloseConnection);
            //}
            //catch (Exception ex)
            //{
            //    System.Windows.Forms.MessageBox.Show(ex.Message);
            //    return null;
            //}           
        }

        public void ExecuteNonQuery(string ProcedureName, List<OracleParameter> parameters, string connName)
        {
            //try
            //{
            OracleConnection conn = this.ConnLists[connName];
            OracleCommand command = new OracleCommand(ProcedureName, conn);
            command.CommandType = CommandType.StoredProcedure;
            foreach (OracleParameter parameter in parameters)
            {
                command.Parameters.Add(parameter);
            }
            conn.Close();
            conn.Open();
            command.ExecuteNonQuery();
            conn.Close();
            //}
            //catch (Exception ex)
            //{
            //    System.Windows.Forms.MessageBox.Show(ex.Message);
            //}           
        }

        public void ExecuteNonQuery(string Query, string connName)
        {
            //try
            //{
            OracleConnection conn = this.ConnLists[connName];
            OracleCommand command = new OracleCommand(Query, conn);
            command.CommandType = CommandType.Text;
            conn.Close();
            conn.Open();
            command.ExecuteNonQuery();
            conn.Close();
            //}
            //catch (Exception ex)
            //{
            //    System.Windows.Forms.MessageBox.Show(ex.Message);
            //}
        }

        public DataSet ExecuteDataSet(string ProcedureName, List<OracleParameter> parameters, string connName)
        {
            //try
            //{
            OracleConnection conn = this.ConnLists[connName];
            DataSet result = new DataSet();
            OracleCommand command = new OracleCommand(ProcedureName, conn);
            command.CommandType = CommandType.StoredProcedure;
            if (parameters != null)
            {
                foreach (OracleParameter parameter in parameters)
                {
                    command.Parameters.Add(parameter);
                }
            }
            conn.Close();
            conn.Open();
            OracleDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable();
            for (int i = 0; i < reader.FieldCount; i++)
            {
                dt.Columns.Add(reader.GetName(i));
            }
            result.Tables.Add(dt);
            while (reader.Read())
            {
                object[] array = new object[reader.FieldCount];
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    array[i] = reader[i].ToString();
                }
                result.Tables[0].Rows.Add(array);
            }
            return result;
            //}
            //catch (Exception ex)
            //{
            //    System.Windows.Forms.MessageBox.Show(ex.Message);
            //    return null;
            //}

        }

        public DataSet ExecuteDataSet(string Query, string connName)
        {
            //try
            //{
            OracleConnection conn = this.ConnLists[connName];
            DataSet result = new DataSet();
            OracleCommand command = new OracleCommand(Query, conn);
            command.CommandType = CommandType.Text;
            conn.Close();
            conn.Open();
            OracleDataReader reader = command.ExecuteReader(CommandBehavior.CloseConnection);

            DataTable dt = new DataTable();
            for (int i = 0; i < reader.FieldCount; i++)
            {
                dt.Columns.Add(reader.GetName(i), reader.GetFieldType(i));
            }
            result.Tables.Add(dt);
            while (reader.Read())
            {
                object[] array = new object[reader.FieldCount];
                for (int i = 0; i < reader.FieldCount; i++)
                {
                    array[i] = reader[i];
                }
                result.Tables[0].Rows.Add(array);
            }

            return result;
            //}
            //catch (Exception ex)
            //{
            //    System.Windows.Forms.MessageBox.Show(ex.Message);
            //    return null;
            //}
        }

        public DataSet ExecuteDataSetList(string ProcedureName, List<OracleParameter> parameters, string connName)
        {
            //try
            //{
            OracleConnection conn = this.ConnLists[connName];
            DataSet result = new DataSet();
            OracleCommand command = new OracleCommand(ProcedureName, conn);
            command.CommandType = CommandType.StoredProcedure;
            if (parameters != null)
            {
                foreach (OracleParameter parameter in parameters)
                {
                    command.Parameters.Add(parameter);
                }
            }
            conn.Close();
            conn.Open();
            OracleDataAdapter da = new OracleDataAdapter();
            da.SelectCommand = command;
            da.Fill(result);
            return result;
            //}
            //catch (Exception ex)
            //{
            //    System.Windows.Forms.MessageBox.Show(ex.Message);
            //    return null;
            //}
        }
    }
}
