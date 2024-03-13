using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.Common;

namespace AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.Motorhome
{
    public class MotorhomeForm : IQuoteForm
    {
        public Wayfinder wayfinder { get; set; }
        public MotorhomeFormData form { get; set; }
    }
}