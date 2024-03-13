
using System;

namespace AFI.Feature.Data.DataModels
{
	public class QuoteLead
	{
		public int Key { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string Suffix { get; set; }
		public string SpouseFirstName { get; set; }
		public string SpouseLastName { get; set; }
		public string PhoneNumber { get; set; }
		public string PhoneNumberExt { get; set; }
		public string EmailAddress { get; set; } 
		public string PolicyHolderMailingAddress { get; set; }
		public string PolicyHolderMailingAddress2 { get; set; }
		public string PolicyHolderCity { get; set; }
		public string PolicyHolderState { get; set; }
		public string PolicyHolderZip { get; set; } 
		public string PropertyAddress2 { get; set; } 

	}
}