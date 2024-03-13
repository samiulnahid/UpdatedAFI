using AFI.Feature.QuoteForm.Areas.AFIWEB.Models.Form_Models;

namespace AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.HillAFBForm
{
    public class HillAFBFormData : FormData
    {
        public UniqueHillAFBFormData unique { get; set; }
        public HillAFBFormData()
        {
        }
    }

    public class UniqueHillAFBFormData
    {
        //INFO: This class is empty because the full JSON doesn't have unique values but it needs to be sent as an empty object.
    }
}