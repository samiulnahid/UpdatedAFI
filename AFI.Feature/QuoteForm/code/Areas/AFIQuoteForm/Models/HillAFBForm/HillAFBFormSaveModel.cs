using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.JsonConverters;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.Common;
using Newtonsoft.Json;

namespace AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.HillAFBForm
{
    public class HillAFBFormSaveModel : CommonFormSaveModel
    {
        public string PolicyHolderMailingAddress2 { get; set; }
    }
}