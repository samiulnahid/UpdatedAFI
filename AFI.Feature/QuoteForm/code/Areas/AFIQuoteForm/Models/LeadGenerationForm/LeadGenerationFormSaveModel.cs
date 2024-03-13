using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.JsonConverters;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.Common;
using Newtonsoft.Json;

namespace AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.LeadGenerationForm
{
    public class LeadGenerationFormSaveModel : CommonFormSaveModel
    {
        public string PolicyHolderMailingAddress2 { get; set; }
    }
}