using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Rising.OracleDBHelper
{
    using OracleDBManager;

    public class OracleDBHelperCore
    {
        public OracleDBManager OracleDBManager { get; set; }

        public LoginHelper LoginHelper { get; set; }

        public CustomHelper CustomHelper { get; set; }

        public LoaderHelper LoaderHelper { get; set; }

        public OracleDBHelperCore(string connName, string connName2, string connName3)
        {
            this.OracleDBManager = new OracleDBManager();
            this.OracleDBManager.loadDB(connName, connName2, connName3);
            this.LoginHelper = new LoginHelper(this.OracleDBManager);
            this.CustomHelper = new CustomHelper(this.OracleDBManager);
            this.LoaderHelper = new LoaderHelper(this.OracleDBManager);
        }       
    }
}
