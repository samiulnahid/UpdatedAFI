using System;

namespace AFI.Feature.Data.DataModels
{
	public class QuoteUmbrellaWatercraft
	{
		public int Key { get; set; }
		public int QuoteKey { get; set; }
		public string WatercraftType { get; set; }
		public string WatercraftLength { get; set; }
		public string WatercraftHorsepower { get; set; }
		public string WatercraftUnderlyingInsurance { get; set; }
		public byte[] changeset { get; set; }

	}
}