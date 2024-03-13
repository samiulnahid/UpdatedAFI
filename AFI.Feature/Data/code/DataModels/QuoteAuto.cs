using System;

namespace AFI.Feature.Data.DataModels
{
	public class QuoteAuto
	{
		public int Key { get; set; }
		public string BodilyInjury { get; set; }
		public string PropertyDamage { get; set; }
		public string MedicalCoverage { get; set; }
		public string UninsuredBodilyInjury { get; set; }
		public string PersonalInjury { get; set; }
		public string ComprehensiveDeductible { get; set; }
		public string CollisionDeductible { get; set; }
		public string CurrentInsurance { get; set; }
		public DateTime? CurrentPolicyDate { get; set; }
		public string CurrentPolicyAction { get; set; }
		public string CurrentBodilyInjury { get; set; }
		public bool IncidentsLast3 { get; set; }
		public byte[] changeset { get; set; }
		public int? PersonalEffects1 { get; set; }
		public int? PersonalEffects2 { get; set; }
		public string CurrentBodilyInjuryLimit { get; set; }
		public int? NumberOfLicensedDrivers { get; set; }
		public int? NumberOfDailyUseVehiclesInHousehold { get; set; }
		public string DescribeAnyAutoRelatedAccidents { get; set; }

	}
}