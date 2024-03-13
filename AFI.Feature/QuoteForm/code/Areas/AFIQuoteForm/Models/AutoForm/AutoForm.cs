using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.Common;
using AFI.Feature.QuoteForm.Areas.AFIWEB.Models.Form_Models;

namespace AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.AutoForm
{
    public class AutoForm : IQuoteForm
    {
        public Wayfinder wayfinder { get; set; }
        public AutoFormData form { get; set; }
    }
}