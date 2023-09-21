using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rising.OracleDBHelper
{
    using OracleDBManager;
    public class LoginHelper
    {
      
        public OracleDBManager OracleDBManager { get; set; }
        public LoginHelper(OracleDBManager OracleDBManager)
        {
            this.OracleDBManager = OracleDBManager;
        }

        public WebUser Login(string UserName, string Password, string connName)
        {
            //try
            //{
                this.OracleDBManager.Parameters = new List<Oracle.ManagedDataAccess.Client.OracleParameter>();
                this.OracleDBManager.Parameters.Add(this.OracleDBManager.OracleParameter("@USRID", UserName, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, System.Data.ParameterDirection.Input));
                this.OracleDBManager.Parameters.Add(this.OracleDBManager.OracleParameter("@PWD", EncryptDecrypt.Encrypt(Password), Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, System.Data.ParameterDirection.Input));
                this.OracleDBManager.Parameters.Add(this.OracleDBManager.OracleParameter("@p_recordset", "", Oracle.ManagedDataAccess.Client.OracleDbType.RefCursor, System.Data.ParameterDirection.Output));
                this.OracleDBManager.OracleDataReader = this.OracleDBManager.ExecuteReader("IFSC.N$GET_WebUser", this.OracleDBManager.Parameters, connName);
                while (this.OracleDBManager.OracleDataReader.Read())
                {
                    //try
                    //{
                        WebUser rld = new WebUser(this.OracleDBManager);
                        rld.UserID = this.OracleDBManager.OracleDataReader["UserID"].ToString().ToUpper();
                        rld.UserName = this.OracleDBManager.OracleDataReader["UserName"].ToString();
                        rld.EmailID = this.OracleDBManager.OracleDataReader["EmailID"].ToString();
                        rld.MobileNo = this.OracleDBManager.OracleDataReader["MobileNo"].ToString();
                        rld.DisableStatus = Convert.ToBoolean(Convert.ToInt16(this.OracleDBManager.OracleDataReader["DisableStatus"].ToString()));
                        DateTime dt; DateTime.TryParse(this.OracleDBManager.OracleDataReader["DisableDate"].ToString(),out dt);
                        rld.DisableDate = dt;
                        rld.AllowMultiLogin = Convert.ToBoolean(Convert.ToInt16(this.OracleDBManager.OracleDataReader["AllowMultiLogin"].ToString()));
                        rld.LoginStatus = Convert.ToBoolean(Convert.ToInt16(this.OracleDBManager.OracleDataReader["LoginStatus"].ToString()));
                        rld.UserType = (UserType)Enum.Parse(typeof(UserType), this.OracleDBManager.OracleDataReader["UserType"].ToString());
                        rld.MachineName = this.OracleDBManager.OracleDataReader["MachineName"].ToString();
                        rld.Password1 = EncryptDecrypt.Decrypt(this.OracleDBManager.OracleDataReader["Password1"].ToString());
                        DateTime.TryParse(this.OracleDBManager.OracleDataReader["Password1Date"].ToString(), out dt);
                        rld.Password1Date = dt;
                        rld.Password2 = EncryptDecrypt.Decrypt(this.OracleDBManager.OracleDataReader["Password2"].ToString());
                        rld.Password3 = EncryptDecrypt.Decrypt(this.OracleDBManager.OracleDataReader["Password3"].ToString());
                        rld.ResetPassword = EncryptDecrypt.Decrypt(this.OracleDBManager.OracleDataReader["ResetPassword"].ToString());
                        rld.ResetStatus = Convert.ToBoolean(Convert.ToInt16(this.OracleDBManager.OracleDataReader["ResetStatus"].ToString()));
                        DateTime.TryParse(this.OracleDBManager.OracleDataReader["ResetDate"].ToString(), out dt);
                        rld.ResetDate = dt;
                        rld.RequiredPasswordPolicy = Convert.ToBoolean(Convert.ToInt16(this.OracleDBManager.OracleDataReader["requiredPasswordPolicy"].ToString()));
                        //rld.DPType = this.OracleDBManager.OracleDataReader["DPType"].ToString();
                        //rld.DPCode = this.OracleDBManager.OracleDataReader["DPCode"].ToString();
                        //rld.DPACNO = this.OracleDBManager.OracleDataReader["DPACNO"].ToString();
                        //-------assign after validation
                        rld.LoginValidationStatus =false;
                        rld.UserRights = this.OracleDBManager.OracleDataReader["UserRights"].ToString();

                        rld.LoginValidationStatus = true;
                        return rld;
                    //}
                    //catch (Exception ex) { return null; }
                }
                return null;
            //}
            //catch (Exception ex)
            //{
            //    System.Windows.Forms.MessageBox.Show(ex.Message);
            //    return null;
            //}

        }

        public WebUser GetWebUser(string UserName, string connName)
        {
            try
            {
                this.OracleDBManager.Parameters = new List<Oracle.ManagedDataAccess.Client.OracleParameter>();
                this.OracleDBManager.Parameters.Add(this.OracleDBManager.OracleParameter("@USRID", UserName, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, System.Data.ParameterDirection.Input));
                this.OracleDBManager.Parameters.Add(this.OracleDBManager.OracleParameter("@p_recordset", "", Oracle.ManagedDataAccess.Client.OracleDbType.RefCursor, System.Data.ParameterDirection.Output));
                this.OracleDBManager.OracleDataReader = this.OracleDBManager.ExecuteReader("IFSC.N$GET_WebUser_withoutPassword", this.OracleDBManager.Parameters, connName);
                while (this.OracleDBManager.OracleDataReader.Read())
                {
                    try
                    {
                        WebUser rld = new WebUser(this.OracleDBManager);
                        rld.UserID = this.OracleDBManager.OracleDataReader["UserID"].ToString().ToUpper();
                        rld.UserName = this.OracleDBManager.OracleDataReader["UserName"].ToString();
                        rld.EmailID = this.OracleDBManager.OracleDataReader["EmailID"].ToString();
                        rld.MobileNo = this.OracleDBManager.OracleDataReader["MobileNo"].ToString();
                        rld.DisableStatus = Convert.ToBoolean(Convert.ToInt16(this.OracleDBManager.OracleDataReader["DisableStatus"].ToString()));
                        DateTime dt; DateTime.TryParse(this.OracleDBManager.OracleDataReader["DisableDate"].ToString(), out dt);
                        rld.DisableDate = dt;
                        rld.AllowMultiLogin = Convert.ToBoolean(Convert.ToInt16(this.OracleDBManager.OracleDataReader["AllowMultiLogin"].ToString()));
                        rld.LoginStatus = Convert.ToBoolean(Convert.ToInt16(this.OracleDBManager.OracleDataReader["LoginStatus"].ToString()));
                        rld.UserType = (UserType)Enum.Parse(typeof(UserType), this.OracleDBManager.OracleDataReader["UserType"].ToString());
                        rld.MachineName = this.OracleDBManager.OracleDataReader["MachineName"].ToString();
                        rld.Password1 = EncryptDecrypt.Decrypt(this.OracleDBManager.OracleDataReader["Password1"].ToString());
                        DateTime.TryParse(this.OracleDBManager.OracleDataReader["Password1Date"].ToString(), out dt);
                        rld.Password1Date = dt;
                        rld.Password2 = EncryptDecrypt.Decrypt(this.OracleDBManager.OracleDataReader["Password2"].ToString());
                        rld.Password3 = EncryptDecrypt.Decrypt(this.OracleDBManager.OracleDataReader["Password3"].ToString());
                        rld.ResetPassword = EncryptDecrypt.Decrypt(this.OracleDBManager.OracleDataReader["ResetPassword"].ToString());
                        rld.ResetStatus = Convert.ToBoolean(Convert.ToInt16(this.OracleDBManager.OracleDataReader["ResetStatus"].ToString()));
                        DateTime.TryParse(this.OracleDBManager.OracleDataReader["ResetDate"].ToString(), out dt);
                        rld.ResetDate = dt;
                        rld.RequiredPasswordPolicy = Convert.ToBoolean(Convert.ToInt16(this.OracleDBManager.OracleDataReader["requiredPasswordPolicy"].ToString()));
                        //-------assign after validation
                        rld.LoginValidationStatus = false;
                        rld.UserRights = this.OracleDBManager.OracleDataReader["UserRights"].ToString();

                        rld.LoginValidationStatus = true;
                        return rld;
                    }
                    catch (Exception ex) { return null; }
                }
                return null;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
                return null;
            }

        }




        public WebUser LoginHO(string UserName, string Password, string connName)
        {
            //try
            //{
            this.OracleDBManager.Parameters = new List<Oracle.ManagedDataAccess.Client.OracleParameter>();
            this.OracleDBManager.Parameters.Add(this.OracleDBManager.OracleParameter("@USRID", UserName, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, System.Data.ParameterDirection.Input));
            this.OracleDBManager.Parameters.Add(this.OracleDBManager.OracleParameter("@PWD", EncryptDecrypt.Encrypt(Password), Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, System.Data.ParameterDirection.Input));
            this.OracleDBManager.Parameters.Add(this.OracleDBManager.OracleParameter("@p_recordset", "", Oracle.ManagedDataAccess.Client.OracleDbType.RefCursor, System.Data.ParameterDirection.Output));
            this.OracleDBManager.OracleDataReader = this.OracleDBManager.ExecuteReader("P$GET_HOUSER", this.OracleDBManager.Parameters, connName);
            while (this.OracleDBManager.OracleDataReader.Read())
            {
                //try
                //{
                WebUser rld = new WebUser(this.OracleDBManager);
                rld.UserID = this.OracleDBManager.OracleDataReader["UserID"].ToString().ToUpper();
                rld.UserName = this.OracleDBManager.OracleDataReader["UserName"].ToString();
                rld.EmailID = this.OracleDBManager.OracleDataReader["EmailID"].ToString();
                rld.MobileNo = this.OracleDBManager.OracleDataReader["MobileNo"].ToString();
                rld.DisableStatus = Convert.ToBoolean(Convert.ToInt16(this.OracleDBManager.OracleDataReader["DisableStatus"].ToString()));
                DateTime dt; DateTime.TryParse(this.OracleDBManager.OracleDataReader["DisableDate"].ToString(), out dt);
                rld.DisableDate = dt;
                rld.AllowMultiLogin = Convert.ToBoolean(Convert.ToInt16(this.OracleDBManager.OracleDataReader["AllowMultiLogin"].ToString()));
                rld.LoginStatus = Convert.ToBoolean(Convert.ToInt16(this.OracleDBManager.OracleDataReader["LoginStatus"].ToString()));
                rld.UserType = (UserType)Enum.Parse(typeof(UserType), this.OracleDBManager.OracleDataReader["UserType"].ToString());
                rld.MachineName = this.OracleDBManager.OracleDataReader["MachineName"].ToString();
                rld.Password1 = EncryptDecrypt.Decrypt(this.OracleDBManager.OracleDataReader["Password1"].ToString());
                DateTime.TryParse(this.OracleDBManager.OracleDataReader["Password1Date"].ToString(), out dt);
                rld.Password1Date = dt;
                rld.Password2 = EncryptDecrypt.Decrypt(this.OracleDBManager.OracleDataReader["Password2"].ToString());
                rld.Password3 = EncryptDecrypt.Decrypt(this.OracleDBManager.OracleDataReader["Password3"].ToString());
                rld.ResetPassword = EncryptDecrypt.Decrypt(this.OracleDBManager.OracleDataReader["ResetPassword"].ToString());
                rld.ResetStatus = Convert.ToBoolean(Convert.ToInt16(this.OracleDBManager.OracleDataReader["ResetStatus"].ToString()));
                DateTime.TryParse(this.OracleDBManager.OracleDataReader["ResetDate"].ToString(), out dt);
                rld.ResetDate = dt;
                rld.RequiredPasswordPolicy = Convert.ToBoolean(Convert.ToInt16(this.OracleDBManager.OracleDataReader["requiredPasswordPolicy"].ToString()));
                //rld.DPType = this.OracleDBManager.OracleDataReader["DPType"].ToString();
                //rld.DPCode = this.OracleDBManager.OracleDataReader["DPCode"].ToString();
                //rld.DPACNO = this.OracleDBManager.OracleDataReader["DPACNO"].ToString();
                //-------assign after validation
                rld.LoginValidationStatus = false;
                rld.UserRights = this.OracleDBManager.OracleDataReader["UserRights"].ToString();

                rld.LoginValidationStatus = true;
                return rld;
                //}
                //catch (Exception ex) { return null; }
            }
            return null;
            //}
            //catch (Exception ex)
            //{
            //    System.Windows.Forms.MessageBox.Show(ex.Message);
            //    return null;
            //}

        }

        public WebUser GetHOUser(string UserName, string connName)
        {
            try
            {
                this.OracleDBManager.Parameters = new List<Oracle.ManagedDataAccess.Client.OracleParameter>();
                this.OracleDBManager.Parameters.Add(this.OracleDBManager.OracleParameter("@USRID", UserName, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, System.Data.ParameterDirection.Input));
                this.OracleDBManager.Parameters.Add(this.OracleDBManager.OracleParameter("@p_recordset", "", Oracle.ManagedDataAccess.Client.OracleDbType.RefCursor, System.Data.ParameterDirection.Output));
                this.OracleDBManager.OracleDataReader = this.OracleDBManager.ExecuteReader("P$GET_HOUser_withoutPassword", this.OracleDBManager.Parameters, connName);
                while (this.OracleDBManager.OracleDataReader.Read())
                {
                    try
                    {
                        WebUser rld = new WebUser(this.OracleDBManager);
                        rld.UserID = this.OracleDBManager.OracleDataReader["UserID"].ToString().ToUpper();
                        rld.UserName = this.OracleDBManager.OracleDataReader["UserName"].ToString();
                        rld.EmailID = this.OracleDBManager.OracleDataReader["EmailID"].ToString();
                        rld.MobileNo = this.OracleDBManager.OracleDataReader["MobileNo"].ToString();
                        rld.DisableStatus = Convert.ToBoolean(Convert.ToInt16(this.OracleDBManager.OracleDataReader["DisableStatus"].ToString()));
                        DateTime dt; DateTime.TryParse(this.OracleDBManager.OracleDataReader["DisableDate"].ToString(), out dt);
                        rld.DisableDate = dt;
                        rld.AllowMultiLogin = Convert.ToBoolean(Convert.ToInt16(this.OracleDBManager.OracleDataReader["AllowMultiLogin"].ToString()));
                        rld.LoginStatus = Convert.ToBoolean(Convert.ToInt16(this.OracleDBManager.OracleDataReader["LoginStatus"].ToString()));
                        rld.UserType = (UserType)Enum.Parse(typeof(UserType), this.OracleDBManager.OracleDataReader["UserType"].ToString());
                        rld.MachineName = this.OracleDBManager.OracleDataReader["MachineName"].ToString();
                        rld.Password1 = EncryptDecrypt.Decrypt(this.OracleDBManager.OracleDataReader["Password1"].ToString());
                        DateTime.TryParse(this.OracleDBManager.OracleDataReader["Password1Date"].ToString(), out dt);
                        rld.Password1Date = dt;
                        rld.Password2 = EncryptDecrypt.Decrypt(this.OracleDBManager.OracleDataReader["Password2"].ToString());
                        rld.Password3 = EncryptDecrypt.Decrypt(this.OracleDBManager.OracleDataReader["Password3"].ToString());
                        rld.ResetPassword = EncryptDecrypt.Decrypt(this.OracleDBManager.OracleDataReader["ResetPassword"].ToString());
                        rld.ResetStatus = Convert.ToBoolean(Convert.ToInt16(this.OracleDBManager.OracleDataReader["ResetStatus"].ToString()));
                        DateTime.TryParse(this.OracleDBManager.OracleDataReader["ResetDate"].ToString(), out dt);
                        rld.ResetDate = dt;
                        rld.RequiredPasswordPolicy = Convert.ToBoolean(Convert.ToInt16(this.OracleDBManager.OracleDataReader["requiredPasswordPolicy"].ToString()));
                        //-------assign after validation
                        rld.LoginValidationStatus = false;
                        rld.UserRights = this.OracleDBManager.OracleDataReader["UserRights"].ToString();

                        rld.LoginValidationStatus = true;
                        return rld;
                    }
                    catch (Exception ex) { return null; }
                }
                return null;
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(ex.Message);
                return null;
            }

        }

    }
}
