using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.AutoForm;

namespace AFI.Feature.QuoteForm.Areas.AFIWEB.Models.Form_Models
{
    public class SimpleField : ISectionField
    {
        public string id { get; set; }
        public string label { get; set; }
        public string note { get; set; }
        public string placeholder { get; set; }
        public string value { get; set; }
        public IEnumerable options { get; set; }
    }

    public class ArrayField : ISectionField
    {
        public string id { get; set; }
        public string label { get; set; }
        public string note { get; set; }
        public string placeholder { get; set; }
        public string[] value { get; set; }
        public IEnumerable options { get; set; }
    }

    public class PipAmountField : ISectionField
    {
        public string id { get; set; }
        public string label { get; set; }
        public string note { get; set; }
        public string placeholder { get; set; }
        public string value { get; set; }

        public IEnumerable options { get; set; }
    }

    public interface ISectionField
    {
        string id { get; set; }
        string label { get; set; }
        string note { get; set; }
        string placeholder { get; set; }
        IEnumerable options { get; set; }
    }
}