using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.Common;

namespace AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.MotorcycleForm
{
    public class MotorcycleForm : IQuoteForm
    {
        public Wayfinder wayfinder { get; set; }
        public MotorcycleFormData form { get; set; }
    }
}