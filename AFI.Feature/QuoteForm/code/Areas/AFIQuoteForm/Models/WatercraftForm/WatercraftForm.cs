using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.WatercraftForm
{
    public class WatercraftForm : IQuoteForm
    {
        public Wayfinder wayfinder { get; set; }
        public WatercraftFormData form { get; set; }
    }
}