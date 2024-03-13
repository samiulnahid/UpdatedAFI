using System.Runtime.Serialization;
using AFI.Feature.QuoteForm.Areas.AFIWEB.Models.Form_Models;

namespace AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.QuoteLeadForm
{
    public class QuoteLeadFormData : FormData
    {
        public UniqueQuoteLeadFormData unique { get; set; }
        public QuoteLeadFormData()
        {
        }
    }

    public class UniqueQuoteLeadFormData
    {
        //INFO: This class is empty because the full JSON doesn't have unique values but it needs to be sent as an empty object.
    }
}