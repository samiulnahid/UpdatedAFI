using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.Common
{
    [DataContract]
    public class ProductAvailabilityResponse
    {
        [DataMember(Name = "available")]
        public bool Available { get; set; }
    }

    public class ProductAvailabilityPostModel
    {
        public string Type { get; set; }
        public string ZipCode { get; set; }
    }
}