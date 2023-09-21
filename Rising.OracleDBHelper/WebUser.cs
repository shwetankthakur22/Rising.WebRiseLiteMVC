using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Rising.OracleDBHelper
{
    using OracleDBManager;
    public class WebUser
    {
        public OracleDBManager OracleDBManager { get; set; }

        public WebUser(OracleDBManager OracleDBManager)
        {
            this.OracleDBManager = OracleDBManager;
        }

        public string UserID { get; set; }
        public string UserName { get; set; }
        public string EmailID { get; set; }
        public string MobileNo { get; set; }
        public bool DisableStatus { get; set; }
        public DateTime DisableDate { get; set; }
        public bool AllowMultiLogin { get; set; }
        public bool LoginStatus { get; set; }
        public UserType UserType { get; set; }
        public string MachineName { get; set; }
        public string Password1 { get; set; }
        public DateTime Password1Date { get; set; }
        public string Password2 { get; set; }
        public string Password3 { get; set; }
        public string ResetPassword { get; set; }
        public bool ResetStatus { get; set; }
        public DateTime ResetDate { get; set; }
        public bool RequiredPasswordPolicy { get; set; }
        public bool LoginValidationStatus { get; set; }
        public string UserRights { get; set; }  
        public string tempOTP { get; set; }
        public string DPType { get; set; }
        public string DPCode { get; set; }
        public string DPACNO { get; set; }


        public override string ToString()
        {
            return UserID;
        }


        public void SaveWebUser(string connName)
        {
            OracleDBManager odm = new OracleDBManager();
            odm.Parameters = new List<Oracle.ManagedDataAccess.Client.OracleParameter>();
            odm.Parameters.Add(odm.OracleParameter("@UserID_", this.UserID, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, System.Data.ParameterDirection.Input));
            odm.Parameters.Add(odm.OracleParameter("@UserName_", this.UserName, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, System.Data.ParameterDirection.Input));
            odm.Parameters.Add(odm.OracleParameter("@EmailID_", this.EmailID, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, System.Data.ParameterDirection.Input));
            odm.Parameters.Add(odm.OracleParameter("@MobileNo_", this.MobileNo, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, System.Data.ParameterDirection.Input));
            odm.Parameters.Add(odm.OracleParameter("@DisableStatus_", this.DisableStatus==true?"1":"0", Oracle.ManagedDataAccess.Client.OracleDbType.Char, System.Data.ParameterDirection.Input));
            odm.Parameters.Add(odm.OracleParameter("@DisableDate_", this.DisableDate, Oracle.ManagedDataAccess.Client.OracleDbType.Date, System.Data.ParameterDirection.Input));
            odm.Parameters.Add(odm.OracleParameter("@AllowMultiLogin_", this.AllowMultiLogin == true ? "1" : "0", Oracle.ManagedDataAccess.Client.OracleDbType.Char, System.Data.ParameterDirection.Input));
            odm.Parameters.Add(odm.OracleParameter("@LoginStatus_", this.LoginStatus == true ? "1" : "0", Oracle.ManagedDataAccess.Client.OracleDbType.Char, System.Data.ParameterDirection.Input));
            odm.Parameters.Add(odm.OracleParameter("@UserType_", this.UserType.ToString(), Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, System.Data.ParameterDirection.Input));
            odm.Parameters.Add(odm.OracleParameter("@MachineName_", this.MachineName, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, System.Data.ParameterDirection.Input));
            odm.Parameters.Add(odm.OracleParameter("@Password1_", EncryptDecrypt.Encrypt(this.Password1), Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, System.Data.ParameterDirection.Input));
            odm.Parameters.Add(odm.OracleParameter("@Password1Date_", this.Password1Date, Oracle.ManagedDataAccess.Client.OracleDbType.Date, System.Data.ParameterDirection.Input));
            odm.Parameters.Add(odm.OracleParameter("@Password2_", EncryptDecrypt.Encrypt(this.Password2), Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, System.Data.ParameterDirection.Input));
            odm.Parameters.Add(odm.OracleParameter("@Password3_", EncryptDecrypt.Encrypt(this.Password3), Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, System.Data.ParameterDirection.Input));
            odm.Parameters.Add(odm.OracleParameter("@ResetPassword_", EncryptDecrypt.Encrypt(this.ResetPassword), Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, System.Data.ParameterDirection.Input));
            odm.Parameters.Add(odm.OracleParameter("@ResetStatus_", this.ResetStatus == true ? "1" : "0", Oracle.ManagedDataAccess.Client.OracleDbType.Char, System.Data.ParameterDirection.Input));
            odm.Parameters.Add(odm.OracleParameter("@ResetDate_", this.ResetDate, Oracle.ManagedDataAccess.Client.OracleDbType.Date, System.Data.ParameterDirection.Input));
            odm.Parameters.Add(odm.OracleParameter("@RequiredPasswordPolicy_", this.RequiredPasswordPolicy == true ? "1" : "0", Oracle.ManagedDataAccess.Client.OracleDbType.Char, System.Data.ParameterDirection.Input));
            odm.Parameters.Add(odm.OracleParameter("@UserRights_", this.UserRights, Oracle.ManagedDataAccess.Client.OracleDbType.Varchar2, System.Data.ParameterDirection.Input));
            if(this.UserType==UserType.HO) this.OracleDBManager.ExecuteNonQuery("P$UPDATE_HOUSER", odm.Parameters, connName);
             else this.OracleDBManager.ExecuteNonQuery("IFSC.N$UPDATE_WEBUSER", odm.Parameters, connName);
        }
    }

    public enum UserType
    {
        Client,
        Region,
        HeadBranch,
        Branch,
        SubBranch,
        Group,
        RM,
        HO,
        Admin
    }
}
