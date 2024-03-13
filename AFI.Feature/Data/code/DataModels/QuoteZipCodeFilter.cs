using System;

namespace AFI.Feature.Data.DataModels
{
	public class QuoteZipCodeFilter
	{
		public int ID { get; set; }
		public string QuoteZipCode { get; set; }
		public bool MobilehomesQuote { get; set; }
		public bool HomeownersQuote { get; set; }
		public bool CondoQuote { get; set; }
		public bool RentersQuote { get; set; }
		public bool AutoQuote { get; set; }
		public bool UmbrellaQuote { get; set; }
		public bool FloodQuote { get; set; }
		public bool WatercraftQuote { get; set; }
		public bool MotorcycleQuote { get; set; }
		public bool MotorhomeQuote { get; set; }
		public bool VIPQuote { get; set; }
		public bool Commercial { get; set; }
		public bool CollectorVehicle { get; set; }
		public bool EstimateQuote { get; set; }
		public bool IssueRenterOnline { get; set; }
		public bool AskApplicationRenters { get; set; }
		public bool Moratorium { get; set; }

	}
}