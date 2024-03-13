using System;

namespace AFI.Feature.Data.DataModels
{
	public class QuoteRenter
	{
		public int Key { get; set; }
		public string LivingQuarters { get; set; }
		public string StorageAmount { get; set; }
		public string StorageType { get; set; }
		public string Quality { get; set; }
		public string NumLivingRooms { get; set; }
		public string NumKitchens { get; set; }
		public string NumDiningRooms { get; set; }
		public string NumBedrooms { get; set; }
		public string NumDens { get; set; }
		public string NumFamilyRooms { get; set; }
		public byte[] changeset { get; set; }
		public string PersonalPropertyValue { get; set; }
		public string Deductible { get; set; }
		public bool Comprehensive { get; set; }
		public bool Replacement { get; set; }
		public bool IdentityFraud { get; set; }
		public bool AdditionalCoverages { get; set; }
		public bool BusinessProperty { get; set; }
		public bool PersonalLiability { get; set; }
		public string Premium { get; set; }
		public string TotalNumberOfFurnishedRooms { get; set; }
		public string ReplacementCostITV { get; set; }
		public bool NeedsUnderwritingCheck { get; set; }
		public bool NeedsBillingWork { get; set; }
		public bool IssuedSuccessfully { get; set; }
		public decimal? PaymentAmount { get; set; }
		public bool IsInterested { get; set; }
		public bool AlreadyPaid { get; set; }
		public decimal? BasePremiumAmount { get; set; }
		public decimal? ComprehensiveCoverageEndorsementAmount { get; set; }
		public decimal? ReplacementCostCoverageAmount { get; set; }
		public decimal? IdentityFraudExpenseCoverageAmount { get; set; }
		public decimal? AdditionalCoveragesAmount { get; set; }
		public decimal? BusinessPropertyAmount { get; set; }
		public decimal? LiabilityPremium { get; set; }
		public bool ApplicationCompleted { get; set; }
		public bool ApplicationStarted { get; set; }

	}
}