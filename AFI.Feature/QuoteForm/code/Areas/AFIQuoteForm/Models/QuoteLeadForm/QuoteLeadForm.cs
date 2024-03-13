using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.Common;

namespace AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.QuoteLeadForm
{
    public class QuoteLeadForm : IQuoteForm
    {
        public Wayfinder wayfinder { get; set; }
        public QuoteLeadFormData form { get; set; }
    }
}