using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace AFI.Feature.QuoteForm.Areas.AFIWEB.Models
{
    public class ReportView
    {
        public string Key { get; set; }
        public string changeset { get; set; }
        public string FirstName { get; set; }
        public string MiddleInitial { get; set; }
        public string LastName { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string ZipCode { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string PhoneType { get; set; }
        public string HowToContact { get; set; }
        public string BirthDate { get; set; }
        public string Gender { get; set; }
        public string SSN { get; set; }
        public string SpouseOfMilitary { get; set; }
        public string AFIMember { get; set; }
        public string AFIMemberLength { get; set; }
        public string ServiceBranch { get; set; }
        public string ServiceRank { get; set; }
        public string ServiceStatus { get; set; }
        public string ServiceDischargeType { get; set; }
        public string CommissioningProgram { get; set; }
        public string EmploymentStatus { get; set; }
        public string MaritalStatus { get; set; }
        public string AFIExistingPolicyType { get; set; }
        public string ResidenceStatus { get; set; }
        public string WantToReceiveInfo { get; set; }
        public string HowDidYouHearAboutUs { get; set; }
        public string CNTCGroupID { get; set; }
        public string Suffix { get; set; }
        public string Prefix { get; set; }
        public string InsuredParent { get; set; }
        public string PropertyStreet { get; set; }
        public string PropertyCity { get; set; }
        public string PropertyState { get; set; }
        public string PropertyZipCode { get; set; }
        public string ServiceSpouseFirstName { get; set; }
        public string ServiceSpouseLastName { get; set; }
        public string SpouseBirthDate { get; set; }
        public string SpouseSSN { get; set; }
        public string SpouseFirstName { get; set; }
        public string SpouseLastName { get; set; }
        public string SpouseGender { get; set; }
        public string SpouseSuffix { get; set; }
        public string CallForReview { get; set; }
        public string ReviewPhoneNum { get; set; }
        public string CNTCLegacyNum { get; set; }
        public string CNTCLegacySuffix { get; set; }
        public string FirstCommandAdvisorName { get; set; }
        public string UnderMoratorium { get; set; }

        public string CoverageType { get; set; }

        public string Eligibility { get; set; }
        public string Remarks { get; set; }
        public string ReadDisclaimer { get; set; }
        public string Started { get; set; }
        public string Finished { get; set; }
        public string ResponseType { get; set; }
        public string ResponseDescription { get; set; }
        public string Offer { get; set; }
        public string OfferDescription { get; set; }
        public string IP_Address { get; set; }
        public string ExtraInfo { get; set; }
        public string IsSuspicious { get; set; }
        public string IsInterested { get; set; }
        public string IsPossibleDuplicate { get; set; }
        public string IsComplete { get; set; }

        public string BodilyInjury { get; set; }
        public string PropertyDamage { get; set; }
        public string MedicalCoverage { get; set; }
        public string UninsuredBodilyInjury { get; set; }
        public string PersonalInjury { get; set; }
        public string ComprehensiveDeductible { get; set; }
        public string CollisionDeductible { get; set; }
        public string CurrentInsurance { get; set; }
        public string CurrentPolicyDate { get; set; }
        public string CurrentPolicyAction { get; set; }
        public string CurrentBodilyInjury { get; set; }
        public string IncidentsLast3 { get; set; }

        public string PersonalEffects1 { get; set; }
        public string PersonalEffects2 { get; set; }
        public string CurrentBodilyInjuryLimit { get; set; }
        public string NumberOfLicensedDrivers { get; set; }
        public string NumberOfDailyUseVehiclesInHousehold { get; set; }
        public string DescribeAnyAutoRelatedAccidents { get; set; }

        public string AgeLicensed { get; set; }
        public string SafetyCourse { get; set; }
        public string HouseholdMovingViolation { get; set; }
        public string ExperienceYears { get; set; }
        public string GoodStudentDiscount { get; set; }

        public string Incident { get; set; }
        public string Date { get; set; }
        public string DriverName { get; set; }

        public string VIN { get; set; }
        public string Make { get; set; }
        public string Year { get; set; }
        public string Model { get; set; }
        public string BodyStyle { get; set; }
        public string GaragingZip { get; set; }
        public string AntiTheftDevice { get; set; }
        public string LiabilityOnly { get; set; }

        public string VehicleUse { get; set; }
        public string AnnualMileage { get; set; }
        public string MilesOneWay { get; set; }


        public string LocationDifferentThanMailing { get; set; }
        public string Address { get; set; }

        public string IsCondo { get; set; }
        public string CondoFloor { get; set; }
        public string FloorsInStructure { get; set; }
        public string FoundationType { get; set; }
        public string ConstructionDate { get; set; }
        public string OccupiedType { get; set; }
        public string TotalLivingArea { get; set; }
        public string StructureValue { get; set; }
        public string GarageType { get; set; }
        public string GarageValue { get; set; }
        public string FloodZone { get; set; }
        public string AwareOfFloodLosses { get; set; }
        public string NumberOfLosses { get; set; }
        public string MortgageInsAmount { get; set; }

        public string PurchaseDate { get; set; }

        public string Zip { get; set; }
        public string Country { get; set; }
        public string Type { get; set; }
        public string Style { get; set; }
        public string NumberOfStories { get; set; }
        public string TownhouseUnitType { get; set; }
        public string TownhouseUnitLevel { get; set; }
        public string NumberOfUnits { get; set; }

        public string Condition { get; set; }
        public string ConstructionType { get; set; }
        public string ExteriorConstructionPercent { get; set; }
        public string YearBuilt { get; set; }
        public string RoofAge { get; set; }
        public string RoofMaterial { get; set; }
        public string BasementFoundation { get; set; }
        public string SmokeDetectors { get; set; }
        public string BurglarAlarm { get; set; }
        public string GatedCommunity { get; set; }
        public string PrivatePatrol { get; set; }
        public string SprinklerSystem { get; set; }
        public string RespondingFireDept { get; set; }
        public string MilesToFireDept { get; set; }
        public string DistanceToHydrant { get; set; }
        public string CityLimits { get; set; }
        public string CoverageRemediation { get; set; }
        public string QuoteAmount { get; set; }
        public string LossHistory { get; set; }

        public string CurrentCarrier { get; set; }
        public string NumberOfOccupants { get; set; }
        public string ClaimDetails { get; set; }
        public string ClaimNumber { get; set; }
        public string MobileHouseStyle { get; set; }
        public string RecentLossesAmount { get; set; }
        public string RecentLossesDetails { get; set; }
        public string AmountToBeQuoted { get; set; }
        public string Within1000ofHydrant { get; set; }
        public string County { get; set; }
        public string WoodStove { get; set; }
        public string ImprovementList { get; set; }
        public string IsOccupantSmoke { get; set; }
        public string NumbersOfBedrooms { get; set; }
        public string NumbersOfBathrooms { get; set; }

        public string AlarmSystemType { get; set; }
        public string AttachedGarage { get; set; }
        public string IsNewHomePurchase { get; set; }
        public string EstimatedPremium { get; set; }
        public string EstimatedDeductible { get; set; }
        public string QuoteVidNumber { get; set; }
        public string EstimatedWindHail { get; set; }
        public string LossOfUseAmount { get; set; }
        public string PersonalPropertyAmount { get; set; }
        public string InBrushFire { get; set; }

        public string TopPremiumRange { get; set; }
        public string BottomPremiumRange { get; set; }
        public string MonthlyPremium { get; set; }
        // Update Report
    
        public int Quotes_Started { get; set; }
        public int Quotes_Completed { get; set; }
        public int Quotes_Abandoned { get; set; }

        public int Policyholder_Info { get; set; }
        public int Property_Info { get; set; }
        public int Coverage_Info { get; set; }
        public int Submit_Quote { get; set; }

        public string Section { get; set; }
        public string FieldName { get; set; }
        public string Completed { get; set; }
        public string Incomplete { get; set; }
        public string FormState { get; set; }
        public string Total { get; set; }
    }
}