using System;

namespace AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.Common
{
    public class CommonFormSaveResponseModel
    {
        public Guid QuoteKey { get; set; }
        public int QuoteId { get; set; }
        public string MemberNumber { get; set; }
        public bool IsRecaptchaValid { get; set; } = true;
    }
}