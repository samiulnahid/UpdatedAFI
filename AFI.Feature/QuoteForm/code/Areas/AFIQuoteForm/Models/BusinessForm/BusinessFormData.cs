using AFI.Feature.QuoteForm.Areas.AFIWEB.Models.Form_Models;

namespace AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.BusinessForm
{
    public class BusinessFormData : FormData
    {
        public UniqueBusinessFormData unique { get; set; }
        public BusinessFormData()
        {
        }
    }

    public class UniqueBusinessFormData
    {
        //INFO: This class is empty because the full JSON doesn't have unique values but it needs to be sent as an empty object.
    }
}