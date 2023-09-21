using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;
using System.Data;



namespace Rising.WebRise.Models
{
    public class DashBoardOutPut
    {       
        [Required]
        [Display(Name = "Search ")]
        public List<string> Search { get; set; }
                

        [Required]
        [Display(Name = "Code From")]
        public string ClientCodeFrom { get; set; }

        [Required]
        [Display(Name = "Code To")]
        public string ClientCodeTo { get; set; }

        [Required]
        [Display(Name = "Date From")]       
        public DateTime DateFrom { get; set; }

        [Required]
        [Display(Name = "Date To")]        
        public DateTime DateTo { get; set; }

        [Required]
        [Display(Name = "Date To")]
        public DateTime AsonDate { get; set; }


        [Required]
        [Display(Name = "Date To")]
        public DateTime DateClosing { get; set; }



        [Required]
        [Display(Name = "Segment")]
        public enumCashSegment Segment { get; set; }

        public bool OpenPosition { get; set; }

        public DataSet ds { get; set; }

        public DataSet dsOut1 { get; set; }

        public DataSet dsOut2 { get; set; }

        public DataSet dsOut3 { get; set; }

        public DataSet dsOut4 { get; set; }

        public DataSet dsOut5 { get; set; }

        public DataSet dsOut6 { get; set; }

        public DataSet dsOut7 { get; set; }

        public DataSet dsOut8 { get; set; }

        public DataSet dsOut9 { get; set; }

        public DataSet dsOut10 { get; set; }


        public string OpeningBalance { get; set; }
        public string ClosingBalance { get; set; }
        public List<FinancialLedgerOutputRow> listFinancialLedgerOutputRow { get; set; }

        public List<FinancialLedgerOutputRow> listSegmentWiseFinancialLedgerOutputRow { get; set; }

        public DataSet dsFinancialSummary { get; set; }

        public List<NetPositionOutputRow> listCashNetPositionOutputRow { get; set; }
        public List<NetPositionOutputRow> listFoNetPositionOutputRow { get; set; }
        public List<NetPositionOutputRow> listCommNetPositionOutputRow { get; set; }
        public List<NetPositionOutputRow> listCurrNetPositionOutputRow { get; set; }


        public List<DematHoldingOutputRow> listDematHoldingOutputRow { get; set; }
    }

    public enum enumCashSegment
    {
        Select,
               
        All,

        NSE,
        BSE,
        MCX       
    }

    public enum enumFoSegment
    {
        Select,

        All,

        NFO,
        BFO,
        MFO       
    }

    public enum enumCurrSegment
    {
        Select,

        All,
       
        NSECD,
        BSECD,
        MCXCD
    }

    public enum enumCommSegment
    {
        Select,

        All,

        MCOM,
        NCDEX,       
    }
}