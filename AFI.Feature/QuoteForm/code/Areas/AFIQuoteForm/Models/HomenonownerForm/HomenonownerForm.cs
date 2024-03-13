using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.Common;

namespace AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.HomenonownerForm
{
    public class HomenonownerForm : IQuoteForm
    {
        public Wayfinder wayfinder { get; set; }
        public HomenonownerFormData form { get; set; }
    }
}