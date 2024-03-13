using System;

namespace AFI.Feature.Data.DataModels
{
	public class QuoteMotorcycleVehicle
	{
		public int Key { get; set; }
		public int QuoteKey { get; set; }
		public int Year { get; set; }
		public string Make { get; set; }
		public string Model { get; set; }
		public string VehicleType { get; set; }
		public string PurchaseYear { get; set; }
		public int? Value { get; set; }
		public int? AccessoryCoverageAmount { get; set; }
		public bool IsLiabilityOnly { get; set; }
		public byte[] changeset { get; set; }
		public string CCSize { get; set; }
        public string ComprehensiveDeductible { get; set; }
        public string CollisionDeductible { get; set; }
		public string GaragingStreet { get; set; }
		public string GaragingState { get; set; }
		public string GaragingCity { get; set; }
		public string GaragingZip { get; set; }
	}
}