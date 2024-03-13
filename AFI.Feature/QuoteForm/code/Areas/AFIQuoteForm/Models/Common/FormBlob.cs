using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AFI.Feature.QuoteForm.Areas.AFIWEB.Models.Form_Models;

namespace AFI.Feature.QuoteForm.Areas.AFIWEB.Models
{
    public class FormBlob
    {
        public List<Step> WayFinder { get; set; }
        public FormData Form { get; set; }
    }
}