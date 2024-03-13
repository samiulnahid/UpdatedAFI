using System;

namespace AFI.Feature.Data.DataModels
{
	public class QuoteCommercial
	{
		public int Key { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string PhoneNumber { get; set; }
		public string PhoneNumberExt { get; set; }
		public string EmailAddress { get; set; }
		public string BusinessName { get; set; }
		public string BusinessType { get; set; }
		public string BusinessAddress { get; set; }
		public string BusinessCity { get; set; }
		public string BusinessState { get; set; }
		public string BusinessZip { get; set; }
		public string InsuranceTypeWanted { get; set; }
		public string BestTimeToCall { get; set; }
		public string Comments { get; set; }
		public string BusinessWebsiteUrl { get; set; }
		public string BusinessTaxID { get; set; }
        public string CurrentInsuranceCompany { get; set; }
        public string InsuranceCompany { get; set; }
        public DateTime? PolicyRenewalDate { get; set; }
        public string PolicyRenewalType { get; set; }

	}
}