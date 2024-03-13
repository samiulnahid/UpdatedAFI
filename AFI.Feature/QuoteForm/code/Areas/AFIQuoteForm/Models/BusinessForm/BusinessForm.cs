using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.Common;

namespace AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.BusinessForm
{
    public class BusinessForm : IQuoteForm
    {
        public Wayfinder wayfinder { get; set; }
        public BusinessFormData form { get; set; }
    }
}