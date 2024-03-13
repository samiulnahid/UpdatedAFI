using System;

namespace AFI.Feature.Data.DataModels
{
	public class QuoteVIPItems
	{
		public int Key { get; set; }
		public int QuoteKey { get; set; }
		public string PropertyType { get; set; }
		public string ItemTitle { get; set; }
		public string StampCoinNumberItems { get; set; }
		public string CameraItemDescription { get; set; }
		public string CameraBrand { get; set; }
		public string CameraSerialNumber { get; set; }
		public string CameraAccessories { get; set; }
		public string SilverwareVasesNumberPlaceSetting { get; set; }
		public string SilverwareVasesNumberPiecesPerPlaceSetting { get; set; }
		public string SilverwareVasesManufacturer { get; set; }
		public string SilverwareVasesNumberAdditionalPieces { get; set; }
		public string FineArtsAntiquesAge { get; set; }
		public string FineArtsAntiquesSize { get; set; }
		public string WeaponMake { get; set; }
		public string WeaponModel { get; set; }
		public string WeaponSerialNumber { get; set; }
		public string JewelryColor { get; set; }
		public string JewelryCut { get; set; }
		public string JewelryClarity { get; set; }
		public string JewelryCaratWeight { get; set; }
		public string JewelryMountingMetal { get; set; }
		public string JewelryWatchManufacturer { get; set; }
		public string FurType { get; set; }
		public string FurArticle { get; set; }
		public string InstrumentType { get; set; }
		public string ClassRingStoneType { get; set; }
		public string ClassRingStoneWeight { get; set; }
		public string ClassRingMountingMetal { get; set; }
		public string Description { get; set; }
		public string EstimatedValue { get; set; }
		public string CoverageCost { get; set; }
		public byte[] changeset { get; set; }
		public DateTime? CreateDate { get; set; }

	}
}