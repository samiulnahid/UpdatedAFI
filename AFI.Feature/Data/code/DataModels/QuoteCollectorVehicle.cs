using System;

namespace AFI.Feature.Data.DataModels
{
	public class QuoteCollectorVehicle
	{
		public int Key { get; set; }
		public int QuoteKey { get; set; }
		public int? Year { get; set; }
		public string Make { get; set; }
		public string Model { get; set; }
		public string Type { get; set; }
		public int? EstimatedValue { get; set; }
		public string VehicleStorage { get; set; }
		public string DescribeHowVehicleIsDriven { get; set; }
        public string ComprehensiveDeductible { get; set; }
        public string CollisionDeductible { get; set; }
        public bool LiabilityOnly { get; set; }
    }
}