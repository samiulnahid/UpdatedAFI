using System;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;
using AFI.Feature.QuoteForm.Areas.AFIWEB.Models.Form_Models;

namespace AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.AutoForm.VinLookup
{
    [DataContract]
    public class VehicleVinInformation
    {
        [DataMember(Name = "year")]
        public Options Year { get; set; }
        [DataMember(Name = "make")]
        public Options Make { get; set; }
        [DataMember(Name = "model")]
        public Options Model { get; set; }
        [DataMember(Name = "bodyType")]
        public Options BodyType { get; set; }
    }
}