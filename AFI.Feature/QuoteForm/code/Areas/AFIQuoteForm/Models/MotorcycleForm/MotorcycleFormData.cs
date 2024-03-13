using AFI.Feature.QuoteForm.Areas.AFIWEB.Models.Form_Models;

namespace AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.MotorcycleForm
{
    public class MotorcycleFormData : FormData
    {
        public UniqueMotorcycleData unique { get; set; }
        public MotorcycleFormData()
        {
            unique = new UniqueMotorcycleData();            
        }
    }
}