using System;

namespace AFI.Feature.Data.DataModels
{
	public class QuoteUmbrellaVehicle
	{
		public int Key { get; set; }
		public int QuoteKey { get; set; }
		public string VehicleType { get; set; } 
		public string VehicleUnderlyingInsurance { get; set; } 
		public byte[] changeset { get; set; }

	}
}