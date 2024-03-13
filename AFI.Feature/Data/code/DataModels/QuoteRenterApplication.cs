using System;

namespace AFI.Feature.Data.DataModels
{
	public class QuoteRenterApplication
	{
		public int Key { get; set; }
		public bool PropertyIsOwned { get; set; }
		public bool ItemsForSalenotInsured { get; set; }
		public bool AFIOnlyCompany { get; set; }
		public bool ReplacementPolicy { get; set; }
		public bool HasDogs { get; set; }
		public int? NumberOfDogs { get; set; }
		public bool HasPool { get; set; }
		public string PoolType { get; set; }
		public bool DivingBoard { get; set; }
		public bool Slide { get; set; }
		public bool PoolFence { get; set; }
		public bool HasTrampoline { get; set; }
		public bool TrampolineHasFence { get; set; }
		public bool TrmapolineNet { get; set; }
		public bool WillInstallNet { get; set; }
		public DateTime? DateToInstallNet { get; set; }
		public bool AnyBusinessActivites { get; set; }
		public string AnyBusinessActivitesDesc { get; set; }
		public bool InterestedIncidentalCoverage { get; set; }
		public bool HomeDaycare { get; set; }
		public int? NumberOfKids { get; set; }
		public bool InterestedHomeDaycareCov { get; set; }
		public bool Employees { get; set; }
		public bool Horses { get; set; }
		public bool ExoticAnimals { get; set; }
		public string ExoticAnimalsDesc { get; set; }
		public string HowContained { get; set; }
		public bool HasBitten { get; set; }
		public bool FarmingOrRanching { get; set; }
		public bool SubjectOfLawsuit { get; set; }
		public bool SitOnAcreage { get; set; }
		public int? HowManyAcres { get; set; }
		public bool HasPonds { get; set; }
		public string PondSize { get; set; }
		public bool PondFenced { get; set; }
		public bool HuntingAllowed { get; set; }
		public string WhoIsAllowedToHunt { get; set; }
		public string HuntHowOften { get; set; }
		public bool Compensated { get; set; }
		public string AcreageUsedFor { get; set; }
		public bool AnyFarmAnimals { get; set; }
		public int? NumLivestock { get; set; }
		public bool InterestedInLivestockCov { get; set; }
		public bool AnimalsForProfit { get; set; }
		public bool OwnOrRentOtherProperty { get; set; }
		public bool InsuranceCancelled { get; set; }
		public string InsuranceCancelledDesc { get; set; }
		public bool ReadPolicyLimits { get; set; }
		public DateTime? StartCoverageDate { get; set; }
		public string PriorAddress { get; set; }
		public string PriorCity { get; set; }
		public string PriorState { get; set; }
		public string PriorZip { get; set; }
		public string CurrentInsurance { get; set; }
		public int? NumStructAtOther { get; set; }
		public int? HowManyLocations { get; set; }
		public bool PersonallyOccupy { get; set; }
		public bool ExtendLiability { get; set; }
		public bool AnyVacant { get; set; }
		public int? HowManyLocationsOnAcreage { get; set; }
		public int? HowManyLocationsVacantLand { get; set; }
		public int? HowManyLocationsNotFarming { get; set; }
		public string HowManyLocationsNotFarmingDesc { get; set; }
		public bool RentedToOthers { get; set; }
		public bool WillInstallPoolFence { get; set; }
		public DateTime? DateToInstallPoolFence { get; set; }
		public string NumberOfClaims { get; set; }
		public string ClaimsDesc { get; set; }

	}
}