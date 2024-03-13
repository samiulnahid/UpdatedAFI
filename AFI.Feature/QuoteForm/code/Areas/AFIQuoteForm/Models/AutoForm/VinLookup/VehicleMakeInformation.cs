using System.Collections.Generic;
using System.Runtime.Serialization;
using AFI.Feature.QuoteForm.Areas.AFIWEB.Models.Form_Models;

namespace AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.AutoForm.VinLookup
{
    [DataContract]
    public class VehicleMakeInformation
    {
        [DataMember(Name = "make")]
        public List<Options> MakeList { get; set; }
    }
}