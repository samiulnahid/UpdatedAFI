using System;

namespace AFI.Feature.Data.DataModels
{
	public class QuoteHomeowner
	{
		public int Key { get; set; }
		public string PurchaseDate { get; set; }
		public string Address { get; set; }
		public string City { get; set; }
		public string State { get; set; }
		public string Zip { get; set; }
		public string Country { get; set; }
		public string Type { get; set; }
		public string Style { get; set; }
		public string NumberOfStories { get; set; }
		public string TownhouseUnitType { get; set; }
		public string TownhouseUnitLevel { get; set; }
		public string NumberOfUnits { get; set; }
		public string TotalLivingArea { get; set; }
		public string Condition { get; set; }
		public string ConstructionType { get; set; }
		public string ExteriorConstructionPercent { get; set; }
		public string YearBuilt { get; set; }
		public string RoofAge { get; set; }
		public string RoofMaterial { get; set; }
		public string BasementFoundation { get; set; }
		public string SmokeDetectors { get; set; }
		public string BurglarAlarm { get; set; }
		public bool GatedCommunity { get; set; }
		public bool PrivatePatrol { get; set; }
		public string SprinklerSystem { get; set; }
		public string RespondingFireDept { get; set; }
		public string MilesToFireDept { get; set; }
		public string DistanceToHydrant { get; set; }
		public bool CityLimits { get; set; }
		public string CoverageRemediation { get; set; }
		public string QuoteAmount { get; set; }
		public bool LossHistory { get; set; }
		public byte[] changeset { get; set; }
		public string CurrentCarrier { get; set; }
		public string NumberOfOccupants { get; set; }
		public string ClaimDetails { get; set; }
		public int? ClaimNumber { get; set; }
		public string MobileHouseStyle { get; set; }
		public string RecentLossesAmount { get; set; }
		public string RecentLossesDetails { get; set; }
		public string AmountToBeQuoted { get; set; }
		public bool Within1000ofHydrant { get; set; }
		public string County { get; set; }
		public bool WoodStove { get; set; }
		public string ImprovementList { get; set; }
		public bool IsOccupantSmoke { get; set; }
		public string NumbersOfBedrooms { get; set; }
		public string NumbersOfBathrooms { get; set; }
		public string GarageType { get; set; }
		public string AlarmSystemType { get; set; }
		public string AttachedGarage { get; set; }
		public bool IsNewHomePurchase { get; set; }
		public decimal? EstimatedPremium { get; set; }
		public string EstimatedDeductible { get; set; }
		public string QuoteVidNumber { get; set; }
		public string EstimatedWindHail { get; set; }
		public string LossOfUseAmount { get; set; }
		public string PersonalPropertyAmount { get; set; }
        public DateTime? PurchaseClosingDate { get; set; }
    }
}