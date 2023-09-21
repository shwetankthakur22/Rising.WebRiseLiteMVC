using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Rising.WebRise.Models
{  
    public class ContractSpecification
    {
      
    
        [Display(Name = "Instrument Type")]
        public string InstrumentType { get; set; }
       
        [Display(Name = "Underlying Symbol")]
        public string Symbol { get; set; }
            
        public string Exchange { get; set; }

        [Display(Name = "Expiry Date")]
        public string ExpiryDate { get; set; }

        [Display(Name = "Lot Size")]
        public string LotSize { get; set; }

        public string ContName { get; set; }

        public System.Data.DataSet result { get; set; }

        public string Rwid { get; set; }


    }
}
