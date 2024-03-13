using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models
{
    public interface IQuoteForm
    {
        Wayfinder wayfinder { get; set; }
    }
}