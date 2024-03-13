using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Web;

namespace AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.AutoForm.VinLookup
{
    [DataContract]
    public class AutoByVinPostModel
    {
        [DataMember(Name = "vin")]
        public string Vin { get; set; }
    }

    [DataContract]
    public class AutoMakeByYearPostModel
    {
        [DataMember(Name = "year")]
        public string Year { get; set; }
    }

    [DataContract]
    public class AutoModelByMakePostModel
    {
        [DataMember(Name = "makeId")]
        public string MakeId { get; set; }

        [DataMember(Name = "year")]
        public string Year { get; set; }
    }

    [DataContract]
    public class AutoBodyTypeByModelPostModel
    {
        [DataMember(Name = "modelId")]
        public string ModelId { get; set; }
    }
}