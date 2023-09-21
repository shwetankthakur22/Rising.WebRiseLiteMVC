using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;
using Rising.WebRise.Models.Process;
using System.Web;

namespace Rising.WebRise.Models
{  
    public class ImportFileInput
    {

        //public HttpPostedFileBase TradeFile { get; set; }
        public string FilePath { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime TradeDate { get; set; }

        [Display(Name = "Import Basis")]
        public string ImportBasis { get; set; }

        public string FileType { get; set; }

        public string Exchange { get; set; }

        public DateTime ExpiryDate { get; set; }
        public string FileName { get; set; }

        public string Session { get; set; }

        public string Records { get; set; }

        [Display(Name = "Pro Code")]
        public string procode { get; set; }
        [Display(Name = "Broker Code")]
        public string brokercode { get; set; }

        [Display(Name = "Calculate Brokerage")]
        public bool calbrok { get; set; }

        [Display(Name = "User Id Wise Transfer")]
        public string useridwt { get; set; }

        public string Imported { get; set; }

        public string Rejected { get; set; }
        [Display(Name = "Member ID")]
        public string memberid { get; set; }

        public string File { get; set; }

        [Display(Name = "Session Id")]
        public string SessionId { get; set; }

        [DisplayFormat(DataFormatString = "{0:MM/dd/yyyy}")]
        public DateTime Date { get; set; }

        [Display(Name = "Delete Only")]
        public bool DeleteOnly { get; set; }

        public bool IsOverrideConfirmed { get; set; }

        public bool IsDeleteConfirmed { get; set; }

        public bool IsImportedSuccess { get; set; }

        public ImportFileOutput importFileOutput { get; set; }

        public ImportMatchingRecord importMatchingRecord { get; set; }



    }
}
