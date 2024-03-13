using System;

namespace AFI.Feature.Data.DataModels
{
	public class QuoteUmbrella
	{
		public int Key { get; set; }
		public string CoverageAmount { get; set; }
		public string UnderlyingInsurance { get; set; }
		public string NumberOfDrivers { get; set; }
		public string DriversUnder25 { get; set; }
		public string NumberOfVehicles { get; set; }
		public string VehicleUnderlyingInsurance { get; set; }
		public bool OwnRentalProperty { get; set; }
		public string NumberOfRentalProperties { get; set; }
		public string RentalUnderlyingInsurance { get; set; }
		public byte[] changeset { get; set; }
		public string DriversOver70 { get; set; }
		public string Incident1Driver { get; set; }
		public string Incident1Type { get; set; }
		public DateTime? Incident1Date { get; set; }
		public string Incident2Driver { get; set; }
		public string Incident2Type { get; set; }
		public DateTime? Incident2Date { get; set; }
		public string Incident3Driver { get; set; }
		public string Incident3Type { get; set; }
		public DateTime? Incident3Date { get; set; }
		public string Incident4Driver { get; set; }
		public string Incident4Type { get; set; }
		public DateTime? Incident4Date { get; set; }
		public string Incident5Driver { get; set; }
		public string Incident5Type { get; set; }
		public DateTime? Incident5Date { get; set; }
		public string DriversUnder22 { get; set; }

	}
}