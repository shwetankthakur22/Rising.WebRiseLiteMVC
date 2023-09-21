using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Diagnostics;


namespace Rising.OracleDBHelper
{
    using OracleDBManager;
   
    public class LoaderHelper
    {
        public bool isFinished { get; set; }

        public OracleDBManager OracleDBManager { get; set; }

        public LoaderHelper(OracleDBManager OracleDBManager)
        {
            this.OracleDBManager = OracleDBManager;
        }

        public bool runLoader(string dbname, string tablename, string filename, string columnList)
        {
            try
            {
                String sb = string.Empty;
                sb = "OPTIONS  \r\n";
                sb = sb + "(ROWS=60000,BINDSIZE=5442880,READSIZE=5442880,,silent=feedback)  \r\n";
                sb = sb + "LOAD DATA  \r\n";
                sb = sb + "infile '"+filename+"'  \r\n";
                sb = sb + "into table "+tablename+" TRUNCATE  \r\n";            
                sb = sb + "fields terminated by ',' optionally enclosed by '\"'  \r\n";
                sb = sb + "trailing nullcols  \r\n";
                string clm = "";
                foreach (string str in columnList.Split(','))
                clm = clm + ""+ str.Trim() + " \" (:" + str.Trim() + ")\"" + ",";
                clm = clm + "~~~~~~";
                clm = clm.Replace(",~~~~~~", "");
                sb = sb + "("+clm+")  \r\n";

                StreamWriter sw = new StreamWriter("E:\\loader.ctl");
                sw.WriteLine(sb);
                sw.Close();

                string str1 = "sqlldr IFSC@"+ dbname + "/IFSC control=E:\\loader.ctl, log=E:\\log.txt";
                return RunCommands1(str1);
                 
            }
            catch (Exception ex)
            {
                throw;
            }           
        }

        public Process process { get; set; }

        public bool RunCommands1(string cmd, string workingDirectory = "")
        {
            isFinished = false;
            process = new Process();
            var psi = new ProcessStartInfo();
            psi.FileName = "cmd.exe";
            psi.RedirectStandardInput = true;
            psi.CreateNoWindow = true;
            psi.WindowStyle = System.Diagnostics.ProcessWindowStyle.Normal;
            psi.RedirectStandardOutput = true;
            psi.RedirectStandardError = true;
            psi.UseShellExecute = false;
            psi.WorkingDirectory = workingDirectory;
            process.StartInfo = psi;

            process.OutputDataReceived += OutputDataReceived;
            process.ErrorDataReceived += ErrorDataReceived;

            process.Start();

            process.BeginOutputReadLine();
            process.BeginErrorReadLine();


            using (StreamWriter sw = process.StandardInput)
            {
                string cmmmd = cmd.Replace('#', '"');
                sw.WriteLine(cmmmd);
            }
            while (isFinished == false) { }
            return true;
        }

        void OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
           if(e.Data==null) isFinished = true;
        }

        void ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            
        }

        bool IsProcessRunning(string sProcessName)
        {
            bool isProcessing = false;
            try
            {
                Process[] proc = Process.GetProcessesByName(sProcessName);
                if (proc.Length > 0)
                    isProcessing = true;
                else
                    isProcessing = false;
            }
            catch (Exception ex)
            {
                ex.ToString();
            }

            return isProcessing;
        }

        bool KillProcess(string sProcessName)
        {
            bool isProcessing = false;
            try
            {
                Process[] proc = Process.GetProcessesByName(sProcessName);
                if (proc.Length > 0)
                {
                    foreach (var process in proc)
                    {
                        process.Kill();
                    }
                    isProcessing = true;
                }
            }
            catch (Exception ex)
            {
                ex.ToString();
            }

            return isProcessing;
        }


    }
}
