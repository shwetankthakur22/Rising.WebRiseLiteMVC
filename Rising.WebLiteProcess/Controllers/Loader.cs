using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Web;
using iRMSdll;

namespace Rising.WebRise.Controllers
{
    public class Loader
    {
        [DllImport("kernel32")]
        private static extern long OpenProcess(long dwDesiredAccess, long bInheritHandle, long dwProcessId);
        [DllImport("kernel32")]
        private static extern long WaitForSingleObject(long hHandle, long dwMilliseconds);
        [DllImport("kernel32.dll")]
        private static extern long GetExitCodeProcess(long hProcess, long lpExitCode);
        [DllImport("kernel32")]
        private static extern long CloseHandle(long hObject);
        private static bool inProcess = false;
        private static object obj = new object();
        private static object brdobj = new object();

        public static void FileLoading(string pathname)
        {

         

            string xxx;
            string DefPathTemp = (@"E:\temp");
            String sb = string.Empty;
            sb = "OPTIONS  \r\n";
            sb = sb + "(ROWS=60000,BINDSIZE=5442880,READSIZE=5442880,,silent=feedback)  \r\n";
            sb = sb + "LOAD DATA  \r\n";
            sb = sb + "infile '" + pathname + "'  \r\n";
            sb = sb + "into table IFSC.RATE_TEMP_TABLE TRUNCATE  \r\n";
            sb = sb + "fields terminated by ',' optionally enclosed by ','  \r\n";
            sb = sb + "trailing nullcols  \r\n";
            sb = sb + "(RDATE, INSTRUMENT_TYPE, SYMBOL,EXPIRY_DATE,STRIKE,OPTION_TYPE, SETTLEMENT_PRICE)  \r\n";

            using (StreamWriter outputFile = new StreamWriter(Path.Combine(DefPathTemp, "UPLD.CTL")))
            {
                outputFile.WriteAsync(sb.ToString());
            }

            lock (obj)
            {

                string dbuser = "IFSC";
                string dbpass = "IFSC1";
                string dbname = "IFSC";
                DllLoader loader = new DllLoader(dbuser, dbpass, dbname, "E:\temp");
                loader.execsqlldr("UPLD", 0, true);





            }
        }




    }
}