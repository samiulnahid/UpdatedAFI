using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.Common;

namespace AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.CondoForm
{
    public class CondoForm : IQuoteForm
    {
        public Wayfinder wayfinder { get; set; }
        public CondoFormData form { get; set; }
    }
}