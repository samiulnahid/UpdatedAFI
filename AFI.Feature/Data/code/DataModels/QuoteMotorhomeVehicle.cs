using System;

namespace AFI.Feature.Data.DataModels
{
	public class QuoteMotorhomeVehicle
	{
		public int Key { get; set; }
		public int QuoteKey { get; set; }
		public bool KeptAtResidence { get; set; }
		public string GaragingZipCode { get; set; }
		public string PurchaseYear { get; set; }
		public int? Value { get; set; }
		public string Make { get; set; }
		public string VehicleType { get; set; }
		public int Year { get; set; }
		public byte[] changeset { get; set; }
		public string Model { get; set; }
		public string Usage { get; set; }
		public int? ValuePersonalEffects { get; set; }
		public int? Length { get; set; }
		public int? NumberOfSlideOuts { get; set; }

	}
}