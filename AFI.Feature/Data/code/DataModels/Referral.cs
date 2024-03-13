using System;

namespace AFI.Feature.Data.DataModels
{
	public class Referral
	{
		public int Key { get; set; }
		public string ReferringAFIMemberNumber { get; set; }
		public string AFIMemberNumber { get; set; }
		public string FirstName { get; set; }
		public string LastName { get; set; }
		public string RelationshipToMember { get; set; }
		public string Email { get; set; }
		public string Street { get; set; }
		public string City { get; set; }
		public string State { get; set; }
		public string ZipCode { get; set; }
		public string PhoneNumber { get; set; }
		public string CSRComments { get; set; }
		public DateTime CreatedDate { get; set; }
		public string NamePrefix { get; set; }
	}
}
