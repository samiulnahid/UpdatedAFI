using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.AutoForm;

namespace AFI.Feature.QuoteForm.Areas.AFIWEB.Models.Form_Models
{
    public abstract class FormData
    {
        public string id { get; set; }
        public string backButtonText { get; set; }
        public string preSaveButtonText { get; set; }
        public string saveButtonText { get; set; }
        public string nextButtonText { get; set; }
        public string submitButtonText { get; set; }
        public string selectMenuDefaultText { get; set; }
        public string saveSuccessRedirect { get; set; }
        public string submitSuccessRedirect { get; set; }
        public string quoteKey { get; set; }
        public int quoteId { get; set; }
        public string returningMessage { get; set; }
        public CommonData common { get; set; }
        public List<Section> sections { get; set; }
    }
}