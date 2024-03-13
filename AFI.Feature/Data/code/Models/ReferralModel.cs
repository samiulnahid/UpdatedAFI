using System;
using AFI.Feature.Data.DataModels;

namespace AFI.Feature.Data.Models
{
	public class ReferralModel
	{
		public int Key { get; set; }
		public string ReferringAFIMemberNumber { get; set; }
		public string ReferringFirstName { get; set; }
		public string ReferringLastName { get; set; }
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
		public DateTime CreatedDate { get; }
		public string NamePrefix { get; set; }
		public string Rank { get; set; }

		public Referral ToDataModel()
		{
			var dm = new Referral();
			dm.Key = Key;
			dm.ReferringAFIMemberNumber = string.IsNullOrWhiteSpace(ReferringAFIMemberNumber) ? string.Join(" ", ReferringFirstName, ReferringLastName) : ReferringAFIMemberNumber;
			dm.AFIMemberNumber = AFIMemberNumber;
			dm.FirstName = FirstName;
			dm.LastName = LastName;
			dm.RelationshipToMember = RelationshipToMember;
			dm.Email = Email;
			dm.Street = Street;
			dm.City = City;
			dm.State = State;
			dm.ZipCode = ZipCode;
			dm.PhoneNumber = PhoneNumber;
			dm.CreatedDate = CreatedDate == DateTime.MinValue ? DateTime.Now : CreatedDate;
			dm.NamePrefix = string.IsNullOrWhiteSpace(Rank) ? NamePrefix : Rank;

			return dm;
		}
	}
}
