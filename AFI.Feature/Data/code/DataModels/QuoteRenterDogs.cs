using System;

namespace AFI.Feature.Data.DataModels
{
	public class QuoteRenterDogs
	{
		public int ID { get; set; }
		public int Key { get; set; }
		public string DogMixedOrPurebred { get; set; }
		public string DogBreed { get; set; }
		public string DogBreedDesc { get; set; }
		public string DogBreed2 { get; set; }
		public string DogBreed2Desc { get; set; }
		public string Age { get; set; }
		public string Gender { get; set; }
		public string YearsOwned { get; set; }
		public string WeightinPounds { get; set; }
		public string SpayedNeutered { get; set; }
		public string Confined { get; set; }
		public string HowConfined { get; set; }
		public string ShotsCurrent { get; set; }
		public string IsLicensed { get; set; }
		public string IsTrained { get; set; }
		public string Precautions { get; set; }

	}
}