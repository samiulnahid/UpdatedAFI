using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.Common;

namespace AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.LeadGenerationForm
{
    public class LeadGenerationForm : IQuoteForm
    {
        public Wayfinder wayfinder { get; set; }
        public LeadGenerationFormData form { get; set; }
    }
}