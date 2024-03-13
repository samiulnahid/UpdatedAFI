using System.Collections.Generic;
using AFI.Feature.QuoteForm.Areas.AFIWEB.Models.Form_Models;

namespace AFI.Feature.QuoteForm.Areas.AFIWEB.Models
{
    public class HomePageAdvisorQuoteViewModel : HomePageQuoteViewModel
    {
        public List<Options> AdvisorTypes { get; set; }
    }
}