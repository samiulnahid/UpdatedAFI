using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.JsonConverters;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.Common;
using Newtonsoft.Json;
using Sitecore.Publishing.Explanations;

namespace AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.WatercraftForm
{
    public class WatercraftFormSaveModel : CommonFormSaveModel
    {
        #region QuoteWatercraftVehicle

        public bool? Vehicle0AddressSameAsMailing { get; set; } = false;
        public bool? Vehicle1AddressSameAsMailing { get; set; } = false;
        public bool? Vehicle2AddressSameAsMailing { get; set; } = false;
        public bool? Vehicle3AddressSameAsMailing { get; set; } = false;
        public bool? Vehicle4AddressSameAsMailing { get; set; } = false;

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

        public string Vehicle0YearPurchased { get; set; }
        public string Vehicle1YearPurchased { get; set; }
        public string Vehicle2YearPurchased { get; set; }
        public string Vehicle3YearPurchased { get; set; }
        public string Vehicle4YearPurchased { get; set; }

        public string Vehicle0HullMaterials { get; set; }
        public string Vehicle1HullMaterials { get; set; }
        public string Vehicle2HullMaterials { get; set; }
        public string Vehicle3HullMaterials { get; set; }
        public string Vehicle4HullMaterials { get; set; }

        public int? Vehicle0MotorCount { get; set; }
        public int? Vehicle1MotorCount { get; set; }
        public int? Vehicle2MotorCount { get; set; }
        public int? Vehicle3MotorCount { get; set; }
        public int? Vehicle4MotorCount { get; set; }

        public int? Vehicle0Horsepower { get; set; }
        public int? Vehicle1Horsepower { get; set; }
        public int? Vehicle2Horsepower { get; set; }
        public int? Vehicle3Horsepower { get; set; }
        public int? Vehicle4Horsepower { get; set; }

        public string Vehicle0PropulsionType { get; set; }
        public string Vehicle1PropulsionType { get; set; }
        public string Vehicle2PropulsionType { get; set; }
        public string Vehicle3PropulsionType { get; set; }
        public string Vehicle4PropulsionType { get; set; }

        public string Vehicle0MaxSpeed { get; set; }
        public string Vehicle1MaxSpeed { get; set; }
        public string Vehicle2MaxSpeed { get; set; }
        public string Vehicle3MaxSpeed { get; set; }
        public string Vehicle4MaxSpeed { get; set; }

        public int? Vehicle0Value { get; set; }
        public int? Vehicle1Value { get; set; }
        public int? Vehicle2Value { get; set; }
        public int? Vehicle3Value { get; set; }
        public int? Vehicle4Value { get; set; }

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

        [JsonConverter(typeof(YesNoBooleanConverter))]
        public bool? Vehicle0TrailerIncluded { get; set; }
        [JsonConverter(typeof(YesNoBooleanConverter))]
        public bool? Vehicle1TrailerIncluded { get; set; }
        [JsonConverter(typeof(YesNoBooleanConverter))]
        public bool? Vehicle2TrailerIncluded { get; set; }
        [JsonConverter(typeof(YesNoBooleanConverter))]
        public bool? Vehicle3TrailerIncluded { get; set; }
        [JsonConverter(typeof(YesNoBooleanConverter))]
        public bool? Vehicle4TrailerIncluded { get; set; }

        public string Vehicle0Style { get; set; }
        public string Vehicle1Style { get; set; }
        public string Vehicle2Style { get; set; }
        public string Vehicle3Style { get; set; }
        public string Vehicle4Style { get; set; }


        public float? Vehicle0Length { get; set; }
        public float? Vehicle1Length { get; set; }
        public float? Vehicle2Length { get; set; }
        public float? Vehicle3Length { get; set; }
        public float? Vehicle4Length { get; set; }

        [JsonConverter(typeof(YesNoBooleanConverter))]
        public bool? Vehicle0LiabilityOnlyCoverage { get; set; }
        [JsonConverter(typeof(YesNoBooleanConverter))]
        public bool? Vehicle1LiabilityOnlyCoverage { get; set; }
        [JsonConverter(typeof(YesNoBooleanConverter))]
        public bool? Vehicle2LiabilityOnlyCoverage { get; set; }
        [JsonConverter(typeof(YesNoBooleanConverter))]
        public bool? Vehicle3LiabilityOnlyCoverage { get; set; }
        [JsonConverter(typeof(YesNoBooleanConverter))]
        public bool? Vehicle4LiabilityOnlyCoverage { get; set; }

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

        public int? Vehicle0TrailerValue { get; set; }
        public int? Vehicle1TrailerValue { get; set; }
        public int? Vehicle2TrailerValue { get; set; }
        public int? Vehicle3TrailerValue { get; set; }
        public int? Vehicle4TrailerValue { get; set; } 

        #endregion QuoteWatercraftVehicle

        #region QuoteAutoDriver

        [JsonConverter(typeof(YesNoBooleanConverter))]
        public bool CohabitantWatercraftOperator { get; set; }

        public string AdditionalDriver0FirstName { get; set; }
        public string AdditionalDriver1FirstName { get; set; }
        public string AdditionalDriver2FirstName { get; set; }
        public string AdditionalDriver3FirstName { get; set; }
        public string AdditionalDriver4FirstName { get; set; }

        public string AdditionalDriver0LastName { get; set; }
        public string AdditionalDriver1LastName { get; set; }
        public string AdditionalDriver2LastName { get; set; }
        public string AdditionalDriver3LastName { get; set; }
        public string AdditionalDriver4LastName { get; set; }

        public string AdditionalDriver0Gender { get; set; }
        public string AdditionalDriver1Gender { get; set; }
        public string AdditionalDriver2Gender { get; set; }
        public string AdditionalDriver3Gender { get; set; }
        public string AdditionalDriver4Gender { get; set; }

        public string AdditionalDriver0MaritalStatus { get; set; }
        public string AdditionalDriver1MaritalStatus { get; set; }
        public string AdditionalDriver2MaritalStatus { get; set; }
        public string AdditionalDriver3MaritalStatus { get; set; }
        public string AdditionalDriver4MaritalStatus { get; set; }
        
        public DateTime? AdditionalDriver0Dob { get; set; }
        public DateTime? AdditionalDriver1Dob { get; set; }
        public DateTime? AdditionalDriver2Dob { get; set; }
        public DateTime? AdditionalDriver3Dob { get; set; }
        public DateTime? AdditionalDriver4Dob { get; set; }

        public int? AdditionalDriver0OperatingExperience { get; set; }
        public int? AdditionalDriver1OperatingExperience { get; set; }
        public int? AdditionalDriver2OperatingExperience { get; set; }
        public int? AdditionalDriver3OperatingExperience { get; set; }
        public int? AdditionalDriver4OperatingExperience { get; set; }
        #endregion QuoteAutoDriver

        #region QuoteAuto

        public string BodilyInjuryPropertyDamage { get; set; }
        public string MedicalCoverage { get; set; }
        public string UninsuredBoatersCoverage { get; set; }

        #endregion QuoteAuto
    }
}