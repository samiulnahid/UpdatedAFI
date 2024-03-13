using AFI.Feature.Data.Providers;
using Dapper;
using AFI.Feature.Data.DataModels;
using Sitecore.Diagnostics;
using System.Collections.Generic;

namespace AFI.Feature.Data.Repositories
{
    public interface IQuoteRenterApplicationRepository
    {
        IEnumerable<QuoteRenterApplication> GetAll();
        QuoteRenterApplication GetByKey(int key);
        int Create(QuoteRenterApplication entity);
        int CreateOrUpdate(QuoteRenterApplication entity);
    }

    public class QuoteRenterApplicationRepository : IQuoteRenterApplicationRepository
    {
		private readonly IDbConnectionProvider _dbConnectionProvider;

		public QuoteRenterApplicationRepository(IDbConnectionProvider dbConnectionProvider)
		{
			_dbConnectionProvider = dbConnectionProvider;
		}

		public IEnumerable<QuoteRenterApplication> GetAll()
		{
			using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
			{
				var sql = "select * from dbo.[QuoteRenterApplication]";
				return db.Query<QuoteRenterApplication>(sql);
			}
		}
		
		public QuoteRenterApplication GetByKey(int key)
		{
			using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
			{
				var sql = "select top 1 * from dbo.[QuoteRenterApplication] where [Key] = @Key";
				return db.QueryFirstOrDefault<QuoteRenterApplication>(sql, new { key });
			}
		}
		
		public int Create(QuoteRenterApplication entity)
		{
			using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
			{
				db.Open();
				using (var transaction = db.BeginTransaction())
				{
					try
					{
						var sql = "insert into dbo.[QuoteRenterApplication] ([Key], [PropertyIsOwned], [ItemsForSalenotInsured], [AFIOnlyCompany], [ReplacementPolicy], [HasDogs], [NumberOfDogs], [HasPool], [PoolType], [DivingBoard], [Slide], [PoolFence], [HasTrampoline], [TrampolineHasFence], [TrmapolineNet], [WillInstallNet], [DateToInstallNet], [AnyBusinessActivites], [AnyBusinessActivitesDesc], [InterestedIncidentalCoverage], [HomeDaycare], [NumberOfKids], [InterestedHomeDaycareCov], [Employees], [Horses], [ExoticAnimals], [ExoticAnimalsDesc], [HowContained], [HasBitten], [FarmingOrRanching], [SubjectOfLawsuit], [SitOnAcreage], [HowManyAcres], [HasPonds], [PondSize], [PondFenced], [HuntingAllowed], [WhoIsAllowedToHunt], [HuntHowOften], [Compensated], [AcreageUsedFor], [AnyFarmAnimals], [NumLivestock], [InterestedInLivestockCov], [AnimalsForProfit], [OwnOrRentOtherProperty], [InsuranceCancelled], [InsuranceCancelledDesc], [ReadPolicyLimits], [StartCoverageDate], [PriorAddress], [PriorCity], [PriorState], [PriorZip], [CurrentInsurance], [NumStructAtOther], [HowManyLocations], [PersonallyOccupy], [ExtendLiability], [AnyVacant], [HowManyLocationsOnAcreage], [HowManyLocationsVacantLand], [HowManyLocationsNotFarming], [HowManyLocationsNotFarmingDesc], [RentedToOthers], [WillInstallPoolFence], [DateToInstallPoolFence], [NumberOfClaims], [ClaimsDesc]) values (@Key, @PropertyIsOwned, @ItemsForSalenotInsured, @AFIOnlyCompany, @ReplacementPolicy, @HasDogs, @NumberOfDogs, @HasPool, @PoolType, @DivingBoard, @Slide, @PoolFence, @HasTrampoline, @TrampolineHasFence, @TrmapolineNet, @WillInstallNet, @DateToInstallNet, @AnyBusinessActivites, @AnyBusinessActivitesDesc, @InterestedIncidentalCoverage, @HomeDaycare, @NumberOfKids, @InterestedHomeDaycareCov, @Employees, @Horses, @ExoticAnimals, @ExoticAnimalsDesc, @HowContained, @HasBitten, @FarmingOrRanching, @SubjectOfLawsuit, @SitOnAcreage, @HowManyAcres, @HasPonds, @PondSize, @PondFenced, @HuntingAllowed, @WhoIsAllowedToHunt, @HuntHowOften, @Compensated, @AcreageUsedFor, @AnyFarmAnimals, @NumLivestock, @InterestedInLivestockCov, @AnimalsForProfit, @OwnOrRentOtherProperty, @InsuranceCancelled, @InsuranceCancelledDesc, @ReadPolicyLimits, @StartCoverageDate, @PriorAddress, @PriorCity, @PriorState, @PriorZip, @CurrentInsurance, @NumStructAtOther, @HowManyLocations, @PersonallyOccupy, @ExtendLiability, @AnyVacant, @HowManyLocationsOnAcreage, @HowManyLocationsVacantLand, @HowManyLocationsNotFarming, @HowManyLocationsNotFarmingDesc, @RentedToOthers, @WillInstallPoolFence, @DateToInstallPoolFence, @NumberOfClaims, @ClaimsDesc); select scope_identity();";
						var id = db.QueryFirstOrDefault<int>(sql, entity, transaction);
						transaction.Commit();
						return id;
					}
					catch (System.Exception ex)
					{
						Log.Error($"{nameof(QuoteRenterApplicationRepository)}: Error while attempting to insert QuoteRenterApplication.", ex, this);
						transaction.Rollback();
						return 0;
					}
				}
			}
		}

        public int CreateOrUpdate(QuoteRenterApplication entity)
        {
            using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
            {
                db.Open();
                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                        var sql = @"MERGE [dbo].[QuoteRenterApplication] as target
                                    USING (SELECT @Key) as source ([Key])
                                    ON (target.[Key] = source.[Key])
                                    WHEN MATCHED THEN
                                    UPDATE 
                                    SET [Key] = @Key,[PropertyIsOwned] = @PropertyIsOwned, [ItemsForSalenotInsured] = @ItemsForSalenotInsured, [AFIOnlyCompany] = @AFIOnlyCompany, [ReplacementPolicy] = @ReplacementPolicy, [HasDogs] = @HasDogs, [NumberOfDogs] = @NumberOfDogs, [HasPool] = @HasPool, [PoolType] = @PoolType, [DivingBoard] = @DivingBoard, [Slide] = @Slide, [PoolFence] = @PoolFence, [HasTrampoline] = @HasTrampoline, [TrampolineHasFence] = @TrampolineHasFence, [TrmapolineNet] = @TrmapolineNet, [WillInstallNet] = @WillInstallNet, [DateToInstallNet] = @DateToInstallNet, [AnyBusinessActivites] = @AnyBusinessActivites, [AnyBusinessActivitesDesc] = @AnyBusinessActivitesDesc, [InterestedIncidentalCoverage] = @InterestedIncidentalCoverage, [HomeDaycare] = @HomeDaycare, [NumberOfKids] = @NumberOfKids, [InterestedHomeDaycareCov] = @InterestedHomeDaycareCov, [Employees] = @Employees, [Horses] = @Horses, [ExoticAnimals] = @ExoticAnimals, [ExoticAnimalsDesc] = @ExoticAnimalsDesc, [HowContained] = @HowContained, [HasBitten] = @HasBitten, [FarmingOrRanching] = @FarmingOrRanching, [SubjectOfLawsuit] = @SubjectOfLawsuit, [SitOnAcreage] = @SitOnAcreage, [HowManyAcres] = @HowManyAcres, [HasPonds] = @HasPonds, [PondSize] = @PondSize, [PondFenced] = @PondFenced, [HuntingAllowed] = @HuntingAllowed, [WhoIsAllowedToHunt] = @WhoIsAllowedToHunt, [HuntHowOften] = @HuntHowOften, [Compensated] = @Compensated, [AcreageUsedFor] = @AcreageUsedFor, [AnyFarmAnimals] = @AnyFarmAnimals, [NumLivestock] = @NumLivestock, [InterestedInLivestockCov] = @InterestedInLivestockCov, [AnimalsForProfit] = @AnimalsForProfit, [OwnOrRentOtherProperty] = @OwnOrRentOtherProperty, [InsuranceCancelled] = @InsuranceCancelled, [InsuranceCancelledDesc] = @InsuranceCancelledDesc, [ReadPolicyLimits] = @ReadPolicyLimits, [StartCoverageDate] = @StartCoverageDate, [PriorAddress] = @PriorAddress, [PriorCity] = @PriorCity, [PriorState] = @PriorState, [PriorZip] = @PriorZip, [CurrentInsurance] = @CurrentInsurance, [NumStructAtOther] = @NumStructAtOther, [HowManyLocations] = @HowManyLocations, [PersonallyOccupy] = @PersonallyOccupy, [ExtendLiability] = @ExtendLiability, [AnyVacant] = @AnyVacant, [HowManyLocationsOnAcreage] = @HowManyLocationsOnAcreage, [HowManyLocationsVacantLand] = @HowManyLocationsVacantLand, [HowManyLocationsNotFarming] = @HowManyLocationsNotFarming, [HowManyLocationsNotFarmingDesc] = @HowManyLocationsNotFarmingDesc, [RentedToOthers] = @RentedToOthers, [WillInstallPoolFence] = @WillInstallPoolFence, [DateToInstallPoolFence] = @DateToInstallPoolFence, [NumberOfClaims] = @NumberOfClaims, [ClaimsDesc] = @ClaimsDesc
                                    WHEN NOT MATCHED THEN
                                    INSERT ([Key], [PropertyIsOwned], [ItemsForSalenotInsured], [AFIOnlyCompany], [ReplacementPolicy], [HasDogs], [NumberOfDogs], [HasPool], [PoolType], [DivingBoard], [Slide], [PoolFence], [HasTrampoline], [TrampolineHasFence], [TrmapolineNet], [WillInstallNet], [DateToInstallNet], [AnyBusinessActivites], [AnyBusinessActivitesDesc], [InterestedIncidentalCoverage], [HomeDaycare], [NumberOfKids], [InterestedHomeDaycareCov], [Employees], [Horses], [ExoticAnimals], [ExoticAnimalsDesc], [HowContained], [HasBitten], [FarmingOrRanching], [SubjectOfLawsuit], [SitOnAcreage], [HowManyAcres], [HasPonds], [PondSize], [PondFenced], [HuntingAllowed], [WhoIsAllowedToHunt], [HuntHowOften], [Compensated], [AcreageUsedFor], [AnyFarmAnimals], [NumLivestock], [InterestedInLivestockCov], [AnimalsForProfit], [OwnOrRentOtherProperty], [InsuranceCancelled], [InsuranceCancelledDesc], [ReadPolicyLimits], [StartCoverageDate], [PriorAddress], [PriorCity], [PriorState], [PriorZip], [CurrentInsurance], [NumStructAtOther], [HowManyLocations], [PersonallyOccupy], [ExtendLiability], [AnyVacant], [HowManyLocationsOnAcreage], [HowManyLocationsVacantLand], [HowManyLocationsNotFarming], [HowManyLocationsNotFarmingDesc], [RentedToOthers], [WillInstallPoolFence], [DateToInstallPoolFence], [NumberOfClaims], [ClaimsDesc])
                                    VALUES (@Key, @PropertyIsOwned, @ItemsForSalenotInsured, @AFIOnlyCompany, @ReplacementPolicy, @HasDogs, @NumberOfDogs, @HasPool, @PoolType, @DivingBoard, @Slide, @PoolFence, @HasTrampoline, @TrampolineHasFence, @TrmapolineNet, @WillInstallNet, @DateToInstallNet, @AnyBusinessActivites, @AnyBusinessActivitesDesc, @InterestedIncidentalCoverage, @HomeDaycare, @NumberOfKids, @InterestedHomeDaycareCov, @Employees, @Horses, @ExoticAnimals, @ExoticAnimalsDesc, @HowContained, @HasBitten, @FarmingOrRanching, @SubjectOfLawsuit, @SitOnAcreage, @HowManyAcres, @HasPonds, @PondSize, @PondFenced, @HuntingAllowed, @WhoIsAllowedToHunt, @HuntHowOften, @Compensated, @AcreageUsedFor, @AnyFarmAnimals, @NumLivestock, @InterestedInLivestockCov, @AnimalsForProfit, @OwnOrRentOtherProperty, @InsuranceCancelled, @InsuranceCancelledDesc, @ReadPolicyLimits, @StartCoverageDate, @PriorAddress, @PriorCity, @PriorState, @PriorZip, @CurrentInsurance, @NumStructAtOther, @HowManyLocations, @PersonallyOccupy, @ExtendLiability, @AnyVacant, @HowManyLocationsOnAcreage, @HowManyLocationsVacantLand, @HowManyLocationsNotFarming, @HowManyLocationsNotFarmingDesc, @RentedToOthers, @WillInstallPoolFence, @DateToInstallPoolFence, @NumberOfClaims, @ClaimsDesc);";

                        var count = db.Execute(sql, entity, transaction);
                        transaction.Commit();
                        return count;

                    }
                    catch (System.Exception ex)
                    {
                        Log.Error($"{nameof(QuoteRenterApplicationRepository)}: Error while attempting to update QuoteRenterApplication.", ex, this);
                        transaction.Rollback();
                        return 0;
                    }
                }
            }
        }
    }
}