using System.Collections.Generic;
using System.Runtime.Serialization;
using AFI.Feature.QuoteForm.Areas.AFIWEB.Models.Form_Models;

namespace AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.FloodForm
{
    [DataContract]
    public class UniqueFloodData
    {
        [DataMember(Name = "propertyBuiltYear")]
        public string PropertyBuiltYear { get; set; }

        [DataMember(Name = "propertyOldestYearText")]
        public string PropertyOldestYearText { get; set; }
    }
}