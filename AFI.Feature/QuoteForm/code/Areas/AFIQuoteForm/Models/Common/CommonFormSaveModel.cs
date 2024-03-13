using System;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.JsonConverters;
using Newtonsoft.Json;

namespace AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.Common
{
    public class CommonFormSaveModel
    {
        [JsonConverter(typeof(CustomTimestampConverter))]
        public DateTime StartDate { get; set; }
        public int? MemberNumber { get; set; }
        public int? QuoteId { get; set; }
        public bool IsStepOne { get; set; }
        public string PartnerEmployeeId { get; set; }
        public string OfferDescription { get; set; }
        public string ResponseDescription { get; set; }
        public string ResponseType { get; set; }
        public string FirstCommandAdvisorName { get; set; }
        public string SaveQuoteEmailAddress { get; set; }
        public bool SubmitQuoteMoreInfo { get; set; }
        public string SubmitQuoteContactInfo { get; set; }
        public string SubmitQuoteContactMethod { get; set; }
        public string BranchOfService { get; set; }

        public string EligibilityStatus { get; set; }
        public string EligibilityFirstName { get; set; }
        public string EligibilityLastName { get; set; }

        public string PolicyHolderFirstName { get; set; }
        public string PolicyHolderLastName { get; set; }
        public string PolicyHolderSuffix { get; set; }
        public DateTime? PolicyHolderDob { get; set; }
        public string PolicyHolderGender { get; set; }
        public string PolicyHolderResidenceStatus { get; set; }
        public string PolicyHolderMaritalStatus { get; set; }
        public string PolicyHolderEmail { get; set; }
        public string PolicyHolderPhoneNumber { get; set; }
        public string PolicyHolderPhoneType { get; set; }
        public string PolicyHolderSsn { get; set; }
        public string PolicyHolderMailingAddress { get; set; }
        public string PolicyHolderState { get; set; }
        public string PolicyHolderCity { get; set; }
        public string PolicyHolderZip { get; set; }
        public int? PolicyHolderAgeWhenLicensed { get; set; }
        [JsonConverter(typeof(YesNoBooleanConverter))]
        public bool PolicyHolderGoodStudent { get; set; }
        public string PolicyHolderOccupation { get; set; }
        public string PolicyHolderEducation { get; set; }
        public int? PolicyHolderOperatingExperience { get; set; }

        public string CohabitantFirstName { get; set; }
        public string CohabitantLastName { get; set; }
        public string CohabitantGender { get; set; }
        public string CohabitantMaritalStatus { get; set; }
        public DateTime? CohabitantDob { get; set; }
        public int? CohabitantAgeWhenLicensed { get; set; }
        [JsonConverter(typeof(YesNoBooleanConverter))]
        public bool CohabitantGoodStudent { get; set; }
        public string CohabitantOccupation { get; set; }
        public string CohabitantEducation { get; set; }
        public string CohabitantSuffix { get; set; }
        public string CohabitantSsn { get; set; }
        [JsonConverter(typeof(YesNoBooleanConverter))]
        public bool CohabitantIsOperator { get; set; }
        public int? CohabitantOperatingExperience { get; set; }
        public string SpouseFirstName { get; set; }
        public string SpouseLastName { get; set; }
        public string SpouseSuffix { get; set; }
        public DateTime? SpouseDob { get; set; }
        public string SpouseGender { get; set; }
        public string SpouseSsn { get; set; }
        public string SpouseOccupation { get; set; }
        public string SpouseEducation { get; set; }
        [JsonConverter(typeof(YesNoBooleanConverter))]
        public bool SpouseIsOperator { get; set; }
        public int? SpouseAgeWhenLicensed { get; set; }

        public string ReturningMessage { get; set; }
        public string MilitaryStatus { get; set; }
        public string MilitaryRank { get; set; }

        #region QuoteAutoIncident

        [JsonConverter(typeof(YesNoBooleanConverter))]
        public bool HouseholdViolationsPreviousClaims { get; set; }

        public int? HouseholdViolations0DriverKey { get; set; }
        public int? HouseholdViolations1DriverKey { get; set; }
        public int? HouseholdViolations2DriverKey { get; set; }
        public int? HouseholdViolations3DriverKey { get; set; }
        public int? HouseholdViolations4DriverKey { get; set; }

        public string HouseholdViolations0Driver { get; set; }
        public string HouseholdViolations1Driver { get; set; }
        public string HouseholdViolations2Driver { get; set; }
        public string HouseholdViolations3Driver { get; set; }
        public string HouseholdViolations4Driver { get; set; }

        public string HouseholdViolations0Type { get; set; }
        public string HouseholdViolations1Type { get; set; }
        public string HouseholdViolations2Type { get; set; }
        public string HouseholdViolations3Type { get; set; }
        public string HouseholdViolations4Type { get; set; }

        public DateTime? HouseholdViolations0Date { get; set; }
        public DateTime? HouseholdViolations1Date { get; set; }
        public DateTime? HouseholdViolations2Date { get; set; }
        public DateTime? HouseholdViolations3Date { get; set; }
        public DateTime? HouseholdViolations4Date { get; set; }

        #endregion QuoteAutoIncident

        #region QuoteAuto

        public string BodilyInjuryLiability { get; set; }
        public string PropertyDamageLiability { get; set; }
        public string MedicalPayment { get; set; }
        public string UninsuredMotoristBodilyInjury { get; set; }
        public string PersonalInjuryProtection { get; set; }
        public string CurrentInsuranceCompany { get; set; }
        public string OtherInsuranceCompany { get; set; }
        public DateTime? PolicyRenewalDate { get; set; }
        public string PolicyRenewalType { get; set; }
        public int? LicensedDriverCount { get; set; }

        #endregion QuoteAuto

        #region QuoteBusiness
        public string BusinessPhysicalAddress { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Zip { get; set; }
        #endregion QuoteBusiness

        #region Umbrella

        public string PolicyHolderPrimaryResidenceAddress { get; set; }
        public string PolicyHolderPrimaryResidenceCity { get; set; }
        public string PolicyHolderPrimaryResidenceState { get; set; }
        public string PolicyHolderPrimaryResidenceZip { get; set; }

        #endregion Umbrella
    }
}