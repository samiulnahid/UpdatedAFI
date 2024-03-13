using AFI.Feature.Data.Providers;
using Dapper;
using AFI.Feature.Data.DataModels;
using Sitecore.Diagnostics;
using System.Collections.Generic;

namespace AFI.Feature.Data.Repositories
{
    public interface IQuoteRenterRepository
    {
        IEnumerable<QuoteRenter> GetAll();
        QuoteRenter GetByKey(int key);
        int Create(QuoteRenter entity);
        int CreateOrUpdate(QuoteRenter entity);
    }

    public class QuoteRenterRepository : IQuoteRenterRepository
    {
		private readonly IDbConnectionProvider _dbConnectionProvider;

		public QuoteRenterRepository(IDbConnectionProvider dbConnectionProvider)
		{
			_dbConnectionProvider = dbConnectionProvider;
		}

		public IEnumerable<QuoteRenter> GetAll()
		{
			using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
			{
				var sql = "select * from dbo.[QuoteRenter]";
				return db.Query<QuoteRenter>(sql);
			}
		}
		
		public QuoteRenter GetByKey(int key)
		{
			using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
			{
				var sql = "select top 1 * from dbo.[QuoteRenter] where [Key] = @Key";
				return db.QueryFirstOrDefault<QuoteRenter>(sql, new { key });
			}
		}
		
		public int Create(QuoteRenter entity)
		{
			using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
			{
				db.Open();
				using (var transaction = db.BeginTransaction())
				{
					try
					{
						var sql = "insert into dbo.[QuoteRenter] ([Key], [LivingQuarters], [StorageAmount], [StorageType], [Quality], [NumLivingRooms], [NumKitchens], [NumDiningRooms], [NumBedrooms], [NumDens], [NumFamilyRooms], [PersonalPropertyValue], [Deductible], [Comprehensive], [Replacement], [IdentityFraud], [AdditionalCoverages], [BusinessProperty], [PersonalLiability], [Premium], [TotalNumberOfFurnishedRooms], [ReplacementCostITV], [NeedsUnderwritingCheck], [NeedsBillingWork], [IssuedSuccessfully], [PaymentAmount], [IsInterested], [AlreadyPaid], [BasePremiumAmount], [ComprehensiveCoverageEndorsementAmount], [ReplacementCostCoverageAmount], [IdentityFraudExpenseCoverageAmount], [AdditionalCoveragesAmount], [BusinessPropertyAmount], [LiabilityPremium], [ApplicationCompleted], [ApplicationStarted]) values (@Key, @LivingQuarters, @StorageAmount, @StorageType, @Quality, @NumLivingRooms, @NumKitchens, @NumDiningRooms, @NumBedrooms, @NumDens, @NumFamilyRooms, @PersonalPropertyValue, @Deductible, @Comprehensive, @Replacement, @IdentityFraud, @AdditionalCoverages, @BusinessProperty, @PersonalLiability, @Premium, @TotalNumberOfFurnishedRooms, @ReplacementCostITV, @NeedsUnderwritingCheck, @NeedsBillingWork, @IssuedSuccessfully, @PaymentAmount, @IsInterested, @AlreadyPaid, @BasePremiumAmount, @ComprehensiveCoverageEndorsementAmount, @ReplacementCostCoverageAmount, @IdentityFraudExpenseCoverageAmount, @AdditionalCoveragesAmount, @BusinessPropertyAmount, @LiabilityPremium, @ApplicationCompleted, @ApplicationStarted); select scope_identity();";
						var id = db.QueryFirstOrDefault<int>(sql, entity, transaction);
						transaction.Commit();
						return id;
					}
					catch (System.Exception ex)
					{
						Log.Error($"{nameof(QuoteRenterRepository)}: Error while attempting to insert QuoteRenter.", ex, this);
						transaction.Rollback();
						return 0;
					}
				}
			}
		}

        public int CreateOrUpdate(QuoteRenter entity)
        {
            using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
            {
                db.Open();
                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                        var sql = @"MERGE [dbo].[QuoteRenter] as target
                                    USING (SELECT @Key) as source ([Key])
                                    ON (target.[Key] = source.[Key])
                                    WHEN MATCHED THEN
                                    UPDATE 
                                    SET [LivingQuarters]=@LivingQuarters, [StorageAmount]=@StorageAmount, [StorageType]=@StorageType, [Quality]=@Quality, [NumLivingRooms]=@NumLivingRooms, [NumKitchens]=@NumKitchens, [NumDiningRooms]=@NumDiningRooms, [NumBedrooms]=@NumBedrooms, [NumDens]=@NumDens, [NumFamilyRooms]=@NumFamilyRooms, [PersonalPropertyValue]=@PersonalPropertyValue, [Deductible]=@Deductible, [Comprehensive]=@Comprehensive, [Replacement]=@Replacement, [IdentityFraud]=@IdentityFraud, [AdditionalCoverages]=@AdditionalCoverages, [BusinessProperty]=@BusinessProperty, [PersonalLiability]=@PersonalLiability, [Premium]=@Premium, [TotalNumberOfFurnishedRooms]=@TotalNumberOfFurnishedRooms, [ReplacementCostITV]=@ReplacementCostITV, [NeedsUnderwritingCheck]=@NeedsUnderwritingCheck, [NeedsBillingWork]=@NeedsBillingWork, [IssuedSuccessfully]=@IssuedSuccessfully, [PaymentAmount]=@PaymentAmount, [IsInterested]=@IsInterested, [AlreadyPaid]=@AlreadyPaid, [BasePremiumAmount]=@BasePremiumAmount, [ComprehensiveCoverageEndorsementAmount]=@ComprehensiveCoverageEndorsementAmount, [ReplacementCostCoverageAmount]=@ReplacementCostCoverageAmount, [IdentityFraudExpenseCoverageAmount]=@IdentityFraudExpenseCoverageAmount, [AdditionalCoveragesAmount]=@AdditionalCoveragesAmount, [BusinessPropertyAmount]=@BusinessPropertyAmount, [LiabilityPremium]=@LiabilityPremium, [ApplicationCompleted]=@ApplicationCompleted, [ApplicationStarted]=@ApplicationStarted
                                    WHEN NOT MATCHED THEN
                                    INSERT ([Key], [LivingQuarters], [StorageAmount], [StorageType], [Quality], [NumLivingRooms], [NumKitchens], [NumDiningRooms], [NumBedrooms], [NumDens], [NumFamilyRooms], [PersonalPropertyValue], [Deductible], [Comprehensive], [Replacement], [IdentityFraud], [AdditionalCoverages], [BusinessProperty], [PersonalLiability], [Premium], [TotalNumberOfFurnishedRooms], [ReplacementCostITV], [NeedsUnderwritingCheck], [NeedsBillingWork], [IssuedSuccessfully], [PaymentAmount], [IsInterested], [AlreadyPaid], [BasePremiumAmount], [ComprehensiveCoverageEndorsementAmount], [ReplacementCostCoverageAmount], [IdentityFraudExpenseCoverageAmount], [AdditionalCoveragesAmount], [BusinessPropertyAmount], [LiabilityPremium], [ApplicationCompleted], [ApplicationStarted])
                                    VALUES (@Key, @LivingQuarters, @StorageAmount, @StorageType, @Quality, @NumLivingRooms, @NumKitchens, @NumDiningRooms, @NumBedrooms, @NumDens, @NumFamilyRooms, @PersonalPropertyValue, @Deductible, @Comprehensive, @Replacement, @IdentityFraud, @AdditionalCoverages, @BusinessProperty, @PersonalLiability, @Premium, @TotalNumberOfFurnishedRooms, @ReplacementCostITV, @NeedsUnderwritingCheck, @NeedsBillingWork, @IssuedSuccessfully, @PaymentAmount, @IsInterested, @AlreadyPaid, @BasePremiumAmount, @ComprehensiveCoverageEndorsementAmount, @ReplacementCostCoverageAmount, @IdentityFraudExpenseCoverageAmount, @AdditionalCoveragesAmount, @BusinessPropertyAmount, @LiabilityPremium, @ApplicationCompleted, @ApplicationStarted);";

                        var count = db.Execute(sql, entity, transaction);
                        transaction.Commit();
                        return count;

                    }
                    catch (System.Exception ex)
                    {
                        Log.Error($"{nameof(QuoteRenterRepository)}: Error while attempting to update QuoteRenter.", ex, this);
                        transaction.Rollback();
                        return 0;
                    }
                }
            }
        }
    }
}