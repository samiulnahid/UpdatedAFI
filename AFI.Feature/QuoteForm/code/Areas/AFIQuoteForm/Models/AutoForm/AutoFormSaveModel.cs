using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.JsonConverters;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.Common;
using Newtonsoft.Json;

namespace AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.AutoForm
{
    public class AutoFormSaveModel : CommonFormSaveModel
    {
        public string Vehicle0Vin { get; set; }
        public string Vehicle1Vin { get; set; }
        public string Vehicle2Vin { get; set; }
        public string Vehicle3Vin { get; set; }
        public string Vehicle4Vin { get; set; }

        public string Vehicle0Year { get; set; }
        public string Vehicle1Year { get; set; }
        public string Vehicle2Year { get; set; }
        public string Vehicle3Year { get; set; }
        public string Vehicle4Year { get; set; }

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

        public string Vehicle0BodyType { get; set; }
        public string Vehicle1BodyType { get; set; }
        public string Vehicle2BodyType { get; set; }
        public string Vehicle3BodyType { get; set; }
        public string Vehicle4BodyType { get; set; }

        public string Vehicle0Use { get; set; }
        public string Vehicle1Use { get; set; }
        public string Vehicle2Use { get; set; }
        public string Vehicle3Use { get; set; }
        public string Vehicle4Use { get; set; }

        public int? Vehicle0MilesOneWay { get; set; }
        public int? Vehicle1MilesOneWay { get; set; }
        public int? Vehicle2MilesOneWay { get; set; }
        public int? Vehicle3MilesOneWay { get; set; }
        public int? Vehicle4MilesOneWay { get; set; }

        public string Vehicle0AnnualMileage { get; set; }
        public string Vehicle1AnnualMileage { get; set; }
        public string Vehicle2AnnualMileage { get; set; }
        public string Vehicle3AnnualMileage { get; set; }
        public string Vehicle4AnnualMileage { get; set; }

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

        public string Vehicle0Address { get; set; }
        public string Vehicle1Address { get; set; }
        public string Vehicle2Address { get; set; }
        public string Vehicle3Address { get; set; }
        public string Vehicle4Address { get; set; }

        public string Vehicle0City { get; set; }
        public string Vehicle1City { get; set; }
        public string Vehicle2City { get; set; }
        public string Vehicle3City { get; set; }
        public string Vehicle4City { get; set; }

        public string Vehicle0State { get; set; }
        public string Vehicle1State { get; set; }
        public string Vehicle2State { get; set; }
        public string Vehicle3State { get; set; }
        public string Vehicle4State { get; set; }

        public string Vehicle0Zip { get; set; }
        public string Vehicle1Zip { get; set; }
        public string Vehicle2Zip { get; set; }
        public string Vehicle3Zip { get; set; }
        public string Vehicle4Zip { get; set; }

        public string Vehicle0CollisionDeductible { get; set; }
        public string Vehicle1CollisionDeductible { get; set; }
        public string Vehicle2CollisionDeductible { get; set; }
        public string Vehicle3CollisionDeductible { get; set; }
        public string Vehicle4CollisionDeductible { get; set; }

        public string Vehicle0ComprehensiveDeductible { get; set; }
        public string Vehicle1ComprehensiveDeductible { get; set; }
        public string Vehicle2ComprehensiveDeductible { get; set; }
        public string Vehicle3ComprehensiveDeductible { get; set; }
        public string Vehicle4ComprehensiveDeductible { get; set; }

        public string AdditionalDriver0FirstName { get; set; }
        public string AdditionalDriver1FirstName { get; set; }
        public string AdditionalDriver2FirstName { get; set; }

        public string AdditionalDriver0LastName { get; set; }
        public string AdditionalDriver1LastName { get; set; }
        public string AdditionalDriver2LastName { get; set; }

        public string AdditionalDriver0Gender { get; set; }
        public string AdditionalDriver1Gender { get; set; }
        public string AdditionalDriver2Gender { get; set; }

        public string AdditionalDriver0MaritalStatus { get; set; }
        public string AdditionalDriver1MaritalStatus { get; set; }
        public string AdditionalDriver2MaritalStatus { get; set; }

        public DateTime? AdditionalDriver0Dob { get; set; }
        public DateTime? AdditionalDriver1Dob { get; set; }
        public DateTime? AdditionalDriver2Dob { get; set; }

        public string AdditionalDriver0AgeWhenLicensed { get; set; }
        public string AdditionalDriver1AgeWhenLicensed { get; set; }
        public string AdditionalDriver2AgeWhenLicensed { get; set; }

        [JsonConverter(typeof(YesNoBooleanConverter))]
        public bool AdditionalDriver0GoodStudent { get; set; }
        [JsonConverter(typeof(YesNoBooleanConverter))]
        public bool AdditionalDriver1GoodStudent { get; set; }
        [JsonConverter(typeof(YesNoBooleanConverter))]
        public bool AdditionalDriver2GoodStudent { get; set; }

        public string AdditionalDriver0Occupation { get; set; }
        public string AdditionalDriver1Occupation { get; set; }
        public string AdditionalDriver2Occupation { get; set; }

        public string AdditionalDriver0Education { get; set; }
        public string AdditionalDriver1Education { get; set; }
        public string AdditionalDriver2Education { get; set; }
    }
}