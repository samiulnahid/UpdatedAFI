using System.Collections.Generic;
using System.Runtime.Serialization;
using AFI.Feature.QuoteForm.Areas.AFIWEB.Models.Form_Models;

namespace AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.AutoForm.VinLookup
{
    [DataContract]
    public class VehicleModelInformation
    {
        [DataMember(Name = "model")]
        public List<Options> ModelList { get; set; }
    }
}