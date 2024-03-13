using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.Common;

namespace AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.UmbrellaForm
{
    public class UmbrellaForm : IQuoteForm
    {
        public Wayfinder wayfinder { get; set; }
        public UmbrellaFormData form { get; set; }
    }
}