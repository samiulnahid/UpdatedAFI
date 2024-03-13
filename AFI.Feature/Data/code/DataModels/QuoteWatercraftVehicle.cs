using System;

namespace AFI.Feature.Data.DataModels
{
	public class QuoteWatercraftVehicle
	{
		public int Key { get; set; }
		public int QuoteKey { get; set; }
		public bool KeptAtResidence { get; set; }
		public string GaragingZipCode { get; set; }
		public string PurchaseYear { get; set; }
		public string HullMaterial { get; set; }
		public int NumberOfMotors { get; set; }
		public int? TotalHorsepower { get; set; }
		public string PropulsionType { get; set; }
		public string MaxSpeed { get; set; }
		public int Value { get; set; }
		public byte[] changeset { get; set; }
		public int Year { get; set; }
		public string Make { get; set; }
		public string Model { get; set; }
		public bool IncludesTrailer { get; set; }
		public string BodyStyle { get; set; }
		public float? LengthInFeet { get; set; }

        public string ComprehensiveDeductible { get; set; }
        public string CollisionDeductible { get; set; }
        public bool? LiabilityOnly { get; set; }
		public int TrailerValue { get; set; }

		public string GaragingStreet { get; set; }
		public string GaragingState { get; set; }
		public string GaragingCity { get; set; }
	}
}