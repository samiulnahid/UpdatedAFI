using System;

namespace AFI.Feature.Data.DataModels
{
	public class QuoteVIP
	{
		public int Key { get; set; }
		public int QuoteKey { get; set; }
		public string TotalCoverageCost { get; set; }
		public string InterestLevel { get; set; }
		public byte[] changeset { get; set; }
		public DateTime? CreateDate { get; set; }

	}
}