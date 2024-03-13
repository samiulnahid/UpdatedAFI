using System;

namespace AFI.Feature.Data.DataModels
{
	public class Quote
	{
		public int Key { get; set; }
		public byte[] changeset { get; set; }
		public string CoverageType { get; set; }
		public string ZipCode { get; set; }
		public string Eligibility { get; set; }
		public string Remarks { get; set; }
		public bool ReadDisclaimer { get; set; }
		public DateTime? Started { get; set; }
		public DateTime? Finished { get; set; }
		public string ResponseType { get; set; }
		public string ResponseDescription { get; set; }
		public string Offer { get; set; }
		public string OfferDescription { get; set; }
		public string IP_Address { get; set; }
		public string ExtraInfo { get; set; }
		public bool IsSuspicious { get; set; }
		public bool IsInterested { get; set; }
		public Guid SaveForLaterKey { get; set; }
        public DateTime? SaveForLaterCreateDate { get; set; }
		public string WebCampaign { get; set; }
		public string WebContent { get; set; }
		public string WebMedium { get; set; }
		public string WebSource { get; set; }
		public string WebTerm { get; set; }
		public string WebClickID { get; set; }
		public string gclid { get; set; }
		public string msclkid { get; set; }
		public string fbclid { get; set; }

	}
}