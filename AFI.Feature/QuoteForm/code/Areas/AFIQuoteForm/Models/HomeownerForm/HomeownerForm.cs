using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.Common;

namespace AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.HomeownerForm
{
    public class HomeownerForm : IQuoteForm
    {
        public Wayfinder wayfinder { get; set; }
        public HomeownerFormData form { get; set; }
    }
}