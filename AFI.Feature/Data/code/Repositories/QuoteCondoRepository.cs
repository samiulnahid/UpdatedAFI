using AFI.Feature.Data.Providers;
using Dapper;
using AFI.Feature.Data.DataModels;
using Sitecore.Diagnostics;
using System.Collections.Generic;

namespace AFI.Feature.Data.Repositories
{
    public interface IQuoteCondoRepository
    {
        IEnumerable<QuoteCondo> GetAll();
		QuoteCondo GetByKey(int key);
        int Create(QuoteCondo entity);
        int CreateOrUpdate(QuoteCondo entity);
    }

    public class QuoteCondoRepository : IQuoteCondoRepository
	{
		private readonly IDbConnectionProvider _dbConnectionProvider;

		public QuoteCondoRepository(IDbConnectionProvider dbConnectionProvider)
		{
			_dbConnectionProvider = dbConnectionProvider;
		}

		public IEnumerable<QuoteCondo> GetAll()
		{
			using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
			{
				var sql = "select * from dbo.[QuoteCondo]";
				return db.Query<QuoteCondo>(sql);
			}
		}
		
		public QuoteCondo GetByKey(int key)
		{
			using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
			{
				var sql = "select top 1 * from dbo.[QuoteCondo] where [Key] = @Key";
				return db.QueryFirstOrDefault<QuoteCondo>(sql, new { key });
			}
		}
		
		public int Create(QuoteCondo entity)
		{
			using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
			{
				db.Open();
				using (var transaction = db.BeginTransaction())
				{
					try
					{
						var sql = "insert into dbo.[QuoteCondo] ([Key], [PurchaseDate], [Address], [City], [State], [Zip], [Country], [Type], [Style], [NumberOfStories], [TownhouseUnitType], [TownhouseUnitLevel], [NumberOfUnits], [TotalLivingArea], [Condition], [ConstructionType], [ExteriorConstructionPercent], [YearBuilt], [RoofAge], [RoofMaterial], [BasementFoundation], [SmokeDetectors], [BurglarAlarm], [GatedCommunity], [PrivatePatrol], [SprinklerSystem], [RespondingFireDept], [MilesToFireDept], [DistanceToHydrant], [CityLimits], [CoverageRemediation], [QuoteAmount], [LossHistory], [CurrentCarrier], [NumberOfOccupants], [ClaimDetails], [ClaimNumber], [MobileHouseStyle], [RecentLossesAmount], [RecentLossesDetails], [AmountToBeQuoted], [Within1000ofHydrant], [County], [WoodStove], [ImprovementList], [IsOccupantSmoke], [NumbersOfBedrooms], [NumbersOfBathrooms], [GarageType], [AlarmSystemType], [AttachedGarage], [IsNewHomePurchase], [EstimatedPremium], [EstimatedDeductible], [QuoteVidNumber], [EstimatedWindHail], [LossOfUseAmount], [PersonalPropertyAmount], [AreTenantsRequiredToCarryLiabilityCoverage], [LengthOfLeasingAgreement], [PropertyReasonForVacancy]) values (@Key, @PurchaseDate, @Address, @City, @State, @Zip, @Country, @Type, @Style, @NumberOfStories, @TownhouseUnitType, @TownhouseUnitLevel, @NumberOfUnits, @TotalLivingArea, @Condition, @ConstructionType, @ExteriorConstructionPercent, @YearBuilt, @RoofAge, @RoofMaterial, @BasementFoundation, @SmokeDetectors, @BurglarAlarm, @GatedCommunity, @PrivatePatrol, @SprinklerSystem, @RespondingFireDept, @MilesToFireDept, @DistanceToHydrant, @CityLimits, @CoverageRemediation, @QuoteAmount, @LossHistory, @CurrentCarrier, @NumberOfOccupants, @ClaimDetails, @ClaimNumber, @MobileHouseStyle, @RecentLossesAmount, @RecentLossesDetails, @AmountToBeQuoted, @Within1000ofHydrant, @County, @WoodStove, @ImprovementList, @IsOccupantSmoke, @NumbersOfBedrooms, @NumbersOfBathrooms, @GarageType, @AlarmSystemType, @AttachedGarage, @IsNewHomePurchase, @EstimatedPremium, @EstimatedDeductible, @QuoteVidNumber, @EstimatedWindHail, @LossOfUseAmount, @PersonalPropertyAmount,@AreTenantsRequiredToCarryLiabilityCoverage, @LengthOfLeasingAgreement,@PropertyReasonForVacancy); select scope_identity();";
						var id = db.QueryFirstOrDefault<int>(sql, entity, transaction);
						transaction.Commit();
						return id;
					}
					catch (System.Exception ex)
					{
						Log.Error($"{nameof(QuoteCondoRepository)}: Error while attempting to insert QuoteCondo.", ex, this);
						transaction.Rollback();
						return 0;
					}
				}
			}
		}

        public int CreateOrUpdate(QuoteCondo entity)
        {
            using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
            {
                db.Open();
                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                        var sql = @"MERGE [dbo].[QuoteCondo] as target
                                    USING (SELECT @Key) as source ([Key])
                                    ON (target.[Key] = source.[Key])
                                    WHEN MATCHED THEN
                                    UPDATE 
                                    SET [Key] = @Key,[PurchaseDate] = @PurchaseDate, [PurchaseClosingDate] = @PurchaseClosingDate, [Address] = @Address,[City] = @City,[State] = @State,[Zip] = @Zip,[Country] = @Country,[Type] = @Type,[Style] = @Style,[NumberOfStories] = @NumberOfStories,[TownhouseUnitType] = @TownhouseUnitType,[TownhouseUnitLevel] = @TownhouseUnitLevel,[NumberOfUnits] = @NumberOfUnits,[TotalLivingArea] = @TotalLivingArea, [Condition] = @Condition, [ConstructionType] = @ConstructionType, [ExteriorConstructionPercent] = @ExteriorConstructionPercent, [YearBuilt] = @YearBuilt, [RoofAge] = @RoofAge, [RoofMaterial] = @RoofMaterial, [BasementFoundation] = @BasementFoundation, [SmokeDetectors] = @SmokeDetectors, [BurglarAlarm] = @BurglarAlarm, [GatedCommunity] = @GatedCommunity, [PrivatePatrol] = @PrivatePatrol, [SprinklerSystem] = @SprinklerSystem, [RespondingFireDept] = @RespondingFireDept, [MilesToFireDept] = @MilesToFireDept, [DistanceToHydrant] = @DistanceToHydrant, [CityLimits] = @CityLimits, [CoverageRemediation] = @CoverageRemediation, [QuoteAmount] = @QuoteAmount, [LossHistory] = @LossHistory, [CurrentCarrier] = @CurrentCarrier, [NumberOfOccupants] = @NumberOfOccupants, [ClaimDetails] = @ClaimDetails, [ClaimNumber] = @ClaimNumber, [MobileHouseStyle] = @MobileHouseStyle, [RecentLossesAmount] = @RecentLossesAmount, [RecentLossesDetails] = @RecentLossesDetails, [AmountToBeQuoted] = @AmountToBeQuoted, [Within1000ofHydrant] = @Within1000ofHydrant, [County] = @County, [WoodStove] = @WoodStove, [ImprovementList] = @ImprovementList, [IsOccupantSmoke] = @IsOccupantSmoke, [NumbersOfBedrooms] = @NumbersOfBedrooms, [NumbersOfBathrooms] = @NumbersOfBathrooms, [GarageType] = @GarageType, [AlarmSystemType] = @AlarmSystemType, [AttachedGarage] = @AttachedGarage, [IsNewHomePurchase] = @IsNewHomePurchase, [EstimatedPremium] = @EstimatedPremium, [EstimatedDeductible] = @EstimatedDeductible, [QuoteVidNumber] = @QuoteVidNumber, [EstimatedWindHail] = @EstimatedWindHail, [LossOfUseAmount] = @LossOfUseAmount, [PersonalPropertyAmount] = @PersonalPropertyAmount, [AreTenantsRequiredToCarryLiabilityCoverage] = @AreTenantsRequiredToCarryLiabilityCoverage, [LengthOfLeasingAgreement] = @LengthOfLeasingAgreement, [PropertyReasonForVacancy] = @PropertyReasonForVacancy, [Occupation]=@Occupation, [Education]=@Education, [LeakDetectionSystem]=@LeakDetectionSystem
                                    WHEN NOT MATCHED THEN
                                    INSERT ([Key], [PurchaseDate], [PurchaseClosingDate], [Address], [City], [State], [Zip], [Country], [Type], [Style], [NumberOfStories], [TownhouseUnitType], [TownhouseUnitLevel], [NumberOfUnits], [TotalLivingArea], [Condition], [ConstructionType], [ExteriorConstructionPercent], [YearBuilt], [RoofAge], [RoofMaterial], [BasementFoundation], [SmokeDetectors], [BurglarAlarm], [GatedCommunity], [PrivatePatrol], [SprinklerSystem], [RespondingFireDept], [MilesToFireDept], [DistanceToHydrant], [CityLimits], [CoverageRemediation], [QuoteAmount], [LossHistory], [CurrentCarrier], [NumberOfOccupants], [ClaimDetails], [ClaimNumber], [MobileHouseStyle], [RecentLossesAmount], [RecentLossesDetails], [AmountToBeQuoted], [Within1000ofHydrant], [County], [WoodStove], [ImprovementList], [IsOccupantSmoke], [NumbersOfBedrooms], [NumbersOfBathrooms], [GarageType], [AlarmSystemType], [AttachedGarage], [IsNewHomePurchase], [EstimatedPremium], [EstimatedDeductible], [QuoteVidNumber], [EstimatedWindHail], [LossOfUseAmount], [PersonalPropertyAmount],[AreTenantsRequiredToCarryLiabilityCoverage],[LengthOfLeasingAgreement],[PropertyReasonForVacancy],[Occupation],[Education],[LeakDetectionSystem])
                                    VALUES (@Key, @PurchaseDate, @PurchaseClosingDate, @Address, @City, @State, @Zip, @Country, @Type, @Style, @NumberOfStories, @TownhouseUnitType, @TownhouseUnitLevel, @NumberOfUnits, @TotalLivingArea, @Condition, @ConstructionType, @ExteriorConstructionPercent, @YearBuilt, @RoofAge, @RoofMaterial, @BasementFoundation, @SmokeDetectors, @BurglarAlarm, @GatedCommunity, @PrivatePatrol, @SprinklerSystem, @RespondingFireDept, @MilesToFireDept, @DistanceToHydrant, @CityLimits, @CoverageRemediation, @QuoteAmount, @LossHistory, @CurrentCarrier, @NumberOfOccupants, @ClaimDetails, @ClaimNumber, @MobileHouseStyle, @RecentLossesAmount, @RecentLossesDetails, @AmountToBeQuoted, @Within1000ofHydrant, @County, @WoodStove, @ImprovementList, @IsOccupantSmoke, @NumbersOfBedrooms, @NumbersOfBathrooms, @GarageType, @AlarmSystemType, @AttachedGarage, @IsNewHomePurchase, @EstimatedPremium, @EstimatedDeductible, @QuoteVidNumber, @EstimatedWindHail, @LossOfUseAmount, @PersonalPropertyAmount,@AreTenantsRequiredToCarryLiabilityCoverage,@LengthOfLeasingAgreement,@PropertyReasonForVacancy,@Occupation,@Education,@LeakDetectionSystem);";

                        var count = db.Execute(sql, entity, transaction);
                        transaction.Commit();
                        return count;

                    }
                    catch (System.Exception ex)
                    {
                        Log.Error($"{nameof(QuoteCondoRepository)}: Error while attempting to update QuoteCondo.", ex, this);
                        transaction.Rollback();
                        return 0;
                    }
                }
            }
        }
    }
}