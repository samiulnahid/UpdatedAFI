using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.Common;

namespace AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.MobilehomeForm
{
    public class MobilehomeForm : IQuoteForm
    {
        public Wayfinder wayfinder { get; set; }
        public MobilehomeFormData form { get; set; }
    }
}