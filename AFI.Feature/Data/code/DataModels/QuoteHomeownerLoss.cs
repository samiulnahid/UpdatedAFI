using System;

namespace AFI.Feature.Data.DataModels
{
	public class QuoteHomeownerLoss
	{
		public int Key { get; set; }
		public int QuoteKey { get; set; }
		public byte[] changeset { get; set; }
		public string Month { get; set; }
		public string Year { get; set; }
		public string CauseOfLoss { get; set; }
		public string AmountOfLoss { get; set; }

	}
}