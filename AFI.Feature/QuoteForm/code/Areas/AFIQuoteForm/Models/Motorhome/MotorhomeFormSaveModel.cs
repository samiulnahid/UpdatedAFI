using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.JsonConverters;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.Common;
using Newtonsoft.Json;

namespace AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.Motorhome
{
    public class MotorhomeFormSaveModel : CommonFormSaveModel
    {
        #region QuoteMotorhomeVehicle

        public int VehicleYear { get; set; }

        public string VehicleMake { get; set; }

        public string VehicleModel { get; set; }

        public string VehicleType { get; set; }

        public int? VehicleLength { get; set; }

        public int? VehicleSlideOutCount { get; set; }

        public string VehicleUse { get; set; }

        public string VehiclePurchaseYear { get; set; }

        public int? VehicleValue { get; set; }

        public int? VehiclePersonalEffectsValue { get; set; }

        public bool VehicleAddressSameAsMailing { get; set; }

        public string VehicleAddress { get; set; }

        public string VehicleCity { get; set; }

        public string VehicleState { get; set; }

        public string VehicleZip { get; set; }

        [JsonConverter(typeof(YesNoBooleanConverter))]
        public bool VehicleLiabilityOnlyCoverage { get; set; }

        public string VehicleComprehensiveDeductible { get; set; }

        public string VehicleCollisionDeductible { get; set; }

        #endregion QuoteMotorhomeVehicle

        #region QuoteAutoDriver

        public string PrimaryDriverFirstName { get; set; }
        public string PrimaryDriverLastName { get; set; }
        public DateTime? PrimaryDriverDob { get; set; }
        public string PrimaryDriverGender { get; set; }
        public string PrimaryDriverMaritalStatus { get; set; }
        public int? PrimaryDriverOperatingExperience { get; set; }

        public string AdditionalDriver0FirstName { get; set; }
        public string AdditionalDriver1FirstName { get; set; }
        public string AdditionalDriver2FirstName { get; set; }
        public string AdditionalDriver3FirstName { get; set; }

        public string AdditionalDriver0LastName { get; set; }
        public string AdditionalDriver1LastName { get; set; }
        public string AdditionalDriver2LastName { get; set; }
        public string AdditionalDriver3LastName { get; set; }


        public DateTime? AdditionalDriver0Dob { get; set; }
        public DateTime? AdditionalDriver1Dob { get; set; }
        public DateTime? AdditionalDriver2Dob { get; set; }
        public DateTime? AdditionalDriver3Dob { get; set; }


        public string AdditionalDriver0Gender { get; set; }
        public string AdditionalDriver1Gender { get; set; }
        public string AdditionalDriver2Gender { get; set; }
        public string AdditionalDriver3Gender { get; set; }

        public string AdditionalDriver0MaritalStatus { get; set; }
        public string AdditionalDriver1MaritalStatus { get; set; }
        public string AdditionalDriver2MaritalStatus { get; set; }
        public string AdditionalDriver3MaritalStatus { get; set; }

        public string AdditionalDriver0OperatingExperience { get; set; }
        public string AdditionalDriver1OperatingExperience { get; set; }
        public string AdditionalDriver2OperatingExperience { get; set; }
        public string AdditionalDriver3OperatingExperience { get; set; }

        #endregion QuoteAutoDriver

        #region QuoteSponsor
        [JsonConverter(typeof(YesNoBooleanConverter))]
        public bool SpouseMotorHomeOperator { get; set; }
        public int? SpouseMotorHomeOperatingExperience { get; set; }
        #endregion
    }
}