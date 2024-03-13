using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.JsonConverters;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.Common;
using Newtonsoft.Json;

namespace AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.BusinessForm
{
    public class BusinessFormSaveModel : CommonFormSaveModel
    {
        public string BusinessName { get; set; }
        public string BusinessType { get; set; }
        public string BusinessWebsite { get; set; }
        public string BusinessTaxId { get; set; }
        [JsonConverter(typeof(ArrayString), '|')]
        public string TypeOfInsurance { get; set; }

        public string InsuranceCompany { get; set; }
    }
}