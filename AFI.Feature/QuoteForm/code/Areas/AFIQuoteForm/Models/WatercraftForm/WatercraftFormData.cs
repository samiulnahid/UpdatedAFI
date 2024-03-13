using AFI.Feature.QuoteForm.Areas.AFIWEB.Models.Form_Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.WatercraftForm
{
    public class WatercraftFormData : FormData
    {
        public UniqueWatercraftData unique { get; set; }

        public WatercraftFormData()
        {
            unique = new UniqueWatercraftData();
        }
    }
}