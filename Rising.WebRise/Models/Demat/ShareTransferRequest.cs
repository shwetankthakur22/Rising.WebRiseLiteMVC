using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;
using System.Web.Mvc;

namespace Rising.WebRise.Models
{
    public class ShareTransferRequest
    {
        [Required]
        [Display(Name = "Search ")]
        public List<string> Search { get; set; }

        [Required]
        [Display(Name = "AsOnDate")]
        public DateTime AsOnDate { get; set; }

        [Display(Name = "TransferDate")]
        public DateTime TransferDate { get; set; }

        [Required]
        [Display(Name = "Code")]
        public string ClientCodeFrom { get; set; }

        [Required]
        [Display(Name = "BenCode")]
        public Dictionary<string, string> BenCodes { get; set; }

        public List<ShareTransferOutputRow> listShareTransferOutputRow { get; set; }
    }


    public class ShareTransferOutputRow
    {
        public string ClientCode { get; set; }
        public string ClientName { get; set; }

        public string DPCode { get; set; }
        public string DPAcNo { get; set; }

        public string ScripName { get; set; }
        public string ScripCode { get; set; }
        public string ScripISIN { get; set; }

        public decimal Holding { get; set; }
        public decimal TransferQty { get; set; }
        public int SlipNo { get; set; }
        public string BenCode { get; set; }

        public string BenDPCode { get; set; }
        public string BenDPAcNo { get; set; }
    }
}