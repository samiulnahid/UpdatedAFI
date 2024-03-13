using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AFI.Feature.QuoteForm.Areas.AFIWEB.Models
{
    public class HomePageQuoteViewModel
    {
        public List<InsuranceType> InsuranceTypes { get; set; } 
     
        public string ImageUrl { get; set; }
    }
}