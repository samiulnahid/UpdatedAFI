using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.Common;

namespace AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.FloodForm
{
    public class FloodForm : IQuoteForm
    {
        public Wayfinder wayfinder { get; set; }
        public FloodFormData form { get; set; }
    }
}