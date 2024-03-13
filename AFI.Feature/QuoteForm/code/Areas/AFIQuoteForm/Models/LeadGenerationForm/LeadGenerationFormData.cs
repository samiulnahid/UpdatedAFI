using AFI.Feature.QuoteForm.Areas.AFIWEB.Models.Form_Models;

namespace AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.LeadGenerationForm
{
    public class LeadGenerationFormData : FormData
    {
        public UniqueLeadGenerationFormData unique { get; set; }
        public LeadGenerationFormData()
        {
        }
    }

    public class UniqueLeadGenerationFormData
    {
        //INFO: This class is empty because the full JSON doesn't have unique values but it needs to be sent as an empty object.
    }
}