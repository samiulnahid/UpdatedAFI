using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.JsonConverters;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.Common;
using Newtonsoft.Json;

namespace AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.CollectorVehicleForm
{
    public class CollectorVehicleFormSaveModel : CommonFormSaveModel
    {
        #region QuoteCollectorVehicle

        public int? Vehicle0Year { get; set; }
        public int? Vehicle1Year { get; set; }
        public int? Vehicle2Year { get; set; }
        public int? Vehicle3Year { get; set; }
        public int? Vehicle4Year { get; set; }

        public string Vehicle0Make { get; set; }
        public string Vehicle1Make { get; set; }
        public string Vehicle2Make { get; set; }
        public string Vehicle3Make { get; set; }
        public string Vehicle4Make { get; set; }

        public string Vehicle0Model { get; set; }
        public string Vehicle1Model { get; set; }
        public string Vehicle2Model { get; set; }
        public string Vehicle3Model { get; set; }
        public string Vehicle4Model { get; set; }

        public string Vehicle0Type { get; set; }
        public string Vehicle1Type { get; set; }
        public string Vehicle2Type { get; set; }
        public string Vehicle3Type { get; set; }
        public string Vehicle4Type { get; set; }

        public int? Vehicle0EstimatedValue { get; set; }
        public int? Vehicle1EstimatedValue { get; set; }
        public int? Vehicle2EstimatedValue { get; set; }
        public int? Vehicle3EstimatedValue { get; set; }
        public int? Vehicle4EstimatedValue { get; set; }

        public string Vehicle0Storage { get; set; }
        public string Vehicle1Storage { get; set; }
        public string Vehicle2Storage { get; set; }
        public string Vehicle3Storage { get; set; }
        public string Vehicle4Storage { get; set; }

        public string Vehicle0DriveDescription { get; set; }
        public string Vehicle1DriveDescription { get; set; }
        public string Vehicle2DriveDescription { get; set; }
        public string Vehicle3DriveDescription { get; set; }
        public string Vehicle4DriveDescription { get; set; }

        [JsonConverter(typeof(YesNoBooleanConverter))]
        public bool Vehicle0LiabilityOnlyCoverage { get; set; }
        [JsonConverter(typeof(YesNoBooleanConverter))]
        public bool Vehicle1LiabilityOnlyCoverage { get; set; }
        [JsonConverter(typeof(YesNoBooleanConverter))]
        public bool Vehicle2LiabilityOnlyCoverage { get; set; }
        [JsonConverter(typeof(YesNoBooleanConverter))]
        public bool Vehicle3LiabilityOnlyCoverage { get; set; }
        [JsonConverter(typeof(YesNoBooleanConverter))]
        public bool Vehicle4LiabilityOnlyCoverage { get; set; }

        public string Vehicle0ComprehensiveDeductible { get; set; }
        public string Vehicle1ComprehensiveDeductible { get; set; }
        public string Vehicle2ComprehensiveDeductible { get; set; }
        public string Vehicle3ComprehensiveDeductible { get; set; }
        public string Vehicle4ComprehensiveDeductible { get; set; }

        public string Vehicle0CollisionDeductible { get; set; }
        public string Vehicle1CollisionDeductible { get; set; }
        public string Vehicle2CollisionDeductible { get; set; }
        public string Vehicle3CollisionDeductible { get; set; }
        public string Vehicle4CollisionDeductible { get; set; }


        #endregion QuoteCollectorVehicle

        #region QuoteAuto
        //INFO: Per comment here https://degdigital.atlassian.net/browse/AFI-641?focusedCommentId=180044 Vehicle index 0 is being used
        public string Vehicle0LicensedDriverCount { get; set; }
        public string Vehicle0NumberOfDailyUseVehiclesInHousehold { get; set; }
        #endregion QuoteAuto
    }
}