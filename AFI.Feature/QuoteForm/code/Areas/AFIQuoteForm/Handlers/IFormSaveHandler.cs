using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.Common;

namespace AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Handlers
{
    public interface IFormSaveHandler<T> where T : IQuoteForm
    {
        CommonFormSaveResponseModel HandlePost(string json, string sessionId);
        void ApplySessionFieldChanges(T form, string key);
        void ApplySessionFieldChanges(T form);
    }
}