using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AFI.Feature.QuoteForm.Areas.AFIWEB.Models.Form_Models;

namespace AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.UmbrellaForm
{
    public class UmbrellaFormData : FormData
    {
        public UniqueUmbrellaData unique { get; set; }

        public UmbrellaFormData()
        {
        }
    }
}