using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Rising.WebRise.Models
{
    public class ClientMaster
    {
   
        [Display(Name = "Client Code")]
        public string ClientCode { get; set; }

        public string Name { get; set; }
        [Display(Name = "State Code")]
        public string StateCode { get; set; }
        [Required]
        [Display(Name = "Address")]
        public string Address1 { get; set; }
        public string Address2 { get; set; }
        public string Address3 { get; set; }
        [Required]
        [Display(Name = "Phone Nos.")]
        public string Phone { get; set; }

        [Display(Name = "Mobile Nos.")]
        public string Mobile { get; set; }

        [Display(Name = "Email")]
        public string EmailId { get; set; }
        public string Dob { get; set; }
        public string PinCode { get; set; }
        public string City { get; set; }
        [Required]
        public string State { get; set; }
        public string Country { get; set; }
        public string Group { get; set; }
        public string Gender { get; set; }
        public string Martial { get; set; }
        [Display(Name = "Account Code")]
        public string AccountGroup { get; set; }
        public string FatherName { get; set; }
        public string PAddress1 { get; set; }
        public string PAddress2 { get; set; }
        public string PAddress3 { get; set; }
        public string BranchCode { get; set; }
        public string SubBranch { get; set; }
        public string PanNo { get; set; }
        public string ContractType { get; set; }
        public string Category { get; set; }
        public string ClientEnable { get; set; }
        public string RMCode { get; set; }
        public string CIN { get; set; }
        public string Reason { get; set; }
        public string Remark { get; set; }
        public string RiskCategory { get; set; }
        public string IsPermanentAddress { get; set; }
        public string Exchange { get; set; }

        [Display(Name = "Short Code")]
        public string ShortCode { get; set; }
        [Display(Name = "Brokerage Method")]
        public string BrokerageMethod { get; set; }
        [Display(Name = "Contract Generator")]
        public string ContractG { get; set; }
        [Display(Name = "Tax1 Applied")]
        public string Tax1 { get; set; }
        [Display(Name = "Tax2 Applied")]
        public string Tax2 { get; set; }
        [Display(Name = "Tax3 Applied")]
        public string Tax3 { get; set; }
        [Display(Name = "Tax4 Applied")]
        public string Tax4 { get; set; }
        [Display(Name = "Cash Margin Acc")]
        public string CashAcc { get; set; }
        [Display(Name = "Daily MTM Acc")]
        public string DailyMTMAc { get; set; }
        [Display(Name = "Daily Margin Acc")]
        public string DailyMarginAcc { get; set; }
        [Display(Name = "Interest Applied")]
        public string InterestApp { get; set; }
        [Display(Name = "Interest %")]
        public string Interest { get; set; }
        [Display(Name = "Interest Amount")]
        public string Interestamt { get; set; }
        [Display(Name = "Security Acc")]
        public string Securityacc { get; set; }
        [Display(Name = "Opening Balance")]
        public string OpeningBal { get; set; }
        [Display(Name = "Dealer Code")]
        public string Dealercd { get; set; }
        [Display(Name = "Brokerage Up To Deci")]
        public string brokerageutd{ get; set; }
        [Display(Name = "Trancation Charges")]
        public string Transactioin { get; set; }
        [Display(Name = "StampDuty Applied")]
        public string stampda { get; set; }
        [Display(Name = "Margin Posting")]
        public string marginposting { get; set; }
      
        public string Custodian { get; set; }
        [Display(Name = "Introducer Code")]
        public string Introducercd { get; set; }
        [Display(Name = "Introducer %")]
        public string Introducercdpr { get; set; }
        [Display(Name = "UCC Uploded")]
        public string UCCUploded { get; set; }
        [Display(Name = "Brok Applied (S+P)")]
        public string brokapp { get; set; }
        [Display(Name = "Bank A/c No.")]
        public string bankacc { get; set; }
        [Display(Name = "A/c Type.")]
        public string actype { get; set; }
        [Display(Name = "Bank Name")]
        public string bankname { get; set; }
        public string BAddress1 { get; set; }
        public string BAddress2 { get; set; }
        public string IFSC { get; set; }
        public string MICR { get; set; }

        [Display(Name = "InCorporation Date ")]
        public string InCorporationDate { get; set; }
        [Display(Name = "Registration No")]
        public string regNo { get; set; }
        [Display(Name = "Registration Authority")]
        public string regAuth { get; set; }

        [Display(Name = "Registration Place")]
        public string regplace { get; set; }

        [Display(Name = "Contact Person-1")]
        public string contactP1 { get; set; }
        [Display(Name = "Contact Person-2")]
        public string contactP2 { get; set; }
        [Display(Name = "Contact Person-1 Desig")]
        public string contactP1Deg { get; set; }

        [Display(Name = "Contact Person-2 Desig")]
        public string contactP2Deg { get; set; }
        [Display(Name = "Contact Person-1 Add")]
        public string contactP1Add { get; set; }
        [Display(Name = "Contact Person-2 Add")]
        public string contactP2Add { get; set; }
        [Display(Name = "Contact Person-2 Phn No.")]
        public string ContactP2PhnNo { get; set; }

        [Display(Name = "Contact Person-2 EmailId")]
        public string ContactP2Emailo { get; set; }
        [Display(Name = "Contact Person-2 Pan No.")]
        public string ContactP2Pan { get; set; }
        [Display(Name = "Director Name")]
        public string DirecNm { get; set; }
        [Display(Name = "Director Add")]
        public string DirecAdd { get; set; }
        [Display(Name = "Director PhnNo")]
        public string Direcphn{ get; set; }
        [Display(Name = "PanNo.")]
        public string DirPanNo { get; set; }
        [Display(Name = "Email Id.")]
        public string DirEmail{ get; set; }
        [Display(Name = "Din-1")]
        public string Din1 { get; set; }
        [Display(Name = "Din-2")]
        public string Din2 { get; set; }

        public System.Data.DataSet result { get; set; }

        public string Rwid { get; set; }

    }

    
}
