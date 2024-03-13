using AFI.Feature.QuoteForm.Areas.AFIWEB.Models.Form_Models;

namespace AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.AutoForm
{
    public class AutoFormData : FormData
    {
        public UniqueCarData unique { get; set; }

        public AutoFormData()
        {
            unique = new UniqueCarData();
        }
    }
}