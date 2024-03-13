using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.Common;
using Newtonsoft.Json;

namespace AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.QuoteLeadForm
{
    public class QuoteLeadFormSaveModel : CommonFormSaveModel
    {
        public string PolicyHolderMailingAddress2 { get; set; }
        public string PropertyAddress2 { get; set; }
    }
}