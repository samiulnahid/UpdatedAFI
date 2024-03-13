using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.Common;

namespace AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.HillAFBForm
{
    public class HillAFBForm : IQuoteForm
    {
        public Wayfinder wayfinder { get; set; }
        public HillAFBFormData form { get; set; }
    }
}