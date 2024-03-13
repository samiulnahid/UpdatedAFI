using System;

namespace AFI.Feature.Data.DataModels
{
	public class QuoteFlood
	{
		public int Key { get; set; }
		public bool LocationDifferentThanMailing { get; set; }
		public string Address { get; set; }
		public string ZipCode { get; set; }
		public string City { get; set; }
		public string State { get; set; }
		public bool IsCondo { get; set; }
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
		public bool AwareOfFloodLosses { get; set; }
		public string NumberOfLosses { get; set; }
		public string MortgageInsAmount { get; set; }
		public byte[] changeset { get; set; }

	}
}