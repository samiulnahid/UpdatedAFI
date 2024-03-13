using System;

namespace AFI.Feature.Data.DataModels
{
	public class QuoteAutoVehicle
	{
		public int Key { get; set; }
		public int QuoteKey { get; set; }
		public string VIN { get; set; }
		public string Make { get; set; }
		public string Year { get; set; }
		public string Model { get; set; }
		public string BodyStyle { get; set; }
		public string GaragingZip { get; set; }
		public bool? AntiTheftDevice { get; set; }
		public bool? LiabilityOnly { get; set; }
		public byte[] changeset { get; set; }
		public string VehicleUse { get; set; }
		public string AnnualMileage { get; set; }
		public int? MilesOneWay { get; set; }
        public string GaragingStreet { get; set; }
        public string GaragingCity { get; set; }
        public string GaragingState { get; set; }
        public string ComprehensiveDeductible { get; set; }
        public string CollisionDeductible { get; set; }

	}
}