using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AFI.Feature.QuoteForm.Areas.AFIWEB.Models.Form_Models
{
    public class DependentOptions
    {
        public string name { get; set; }
        public List<Options> options { get; set; }
    }
}