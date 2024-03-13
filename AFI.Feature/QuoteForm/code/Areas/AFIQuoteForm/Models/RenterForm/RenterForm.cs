using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.Common;

namespace AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.RenterForm
{
    public class RenterForm : IQuoteForm
    {
        public Wayfinder wayfinder { get; set; }
        public RenterFormData form { get; set; }
    }
}