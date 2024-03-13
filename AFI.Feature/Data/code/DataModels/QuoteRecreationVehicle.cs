using System;

namespace AFI.Feature.Data.DataModels
{
	public class QuoteRecreationVehicle
	{
		public int Key { get; set; }
		public int QuoteKey { get; set; }
		public bool KeptAtResidence { get; set; }
		public string GaragingZipCode { get; set; }
		public string PurchaseYear { get; set; }
		public int? Value { get; set; }
		public bool LiabilityOnly { get; set; }
		public string VehicleType { get; set; }
		public int Year { get; set; }
		public int? ValueCustomParts { get; set; }
		public string CCSize { get; set; }
		public byte[] changeset { get; set; }
		public string Make { get; set; }
		public string Model { get; set; }

	}
}