using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace Rising.WebRise.Models.Reports
{
    public class GSTReport
    {

        [Required]
        [Display(Name = "Client Code")]
        public string ClientCode { get; set; }

        public string Exchange { get; set; }

        [Required]
        [Display(Name = "Report Type")]
        public string ReportType { get; set; }

        [Required]
        [Display(Name = "Date From")]
        public string DateFrom { get; set; }

        [Required]
        [Display(Name = "Dealing State ID")]
        public string DealingStateID { get; set; }
        public string To { get; set; }

        [Required]
        [Display(Name = "Bill No. Wise File Export")]
        public bool BillnowiseFileExport  { get; set; }

        [Required]
        [Display(Name = "Export HTML")]
        public bool ExportHTML { get; set; }
    }
}