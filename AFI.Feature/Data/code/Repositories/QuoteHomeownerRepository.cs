using AFI.Feature.Data.Providers;
using Dapper;
using AFI.Feature.Data.DataModels;
using Sitecore.Diagnostics;
using System.Collections.Generic;

namespace AFI.Feature.Data.Repositories
{
    public interface IQuoteHomeownerRepository
    {
        IEnumerable<QuoteHomeowner> GetAll();
        QuoteHomeowner GetByKey(int key);
        int Create(QuoteHomeowner entity);
        int CreateOrUpdate(QuoteHomeowner entity);
    }

    public class QuoteHomeownerRepository : IQuoteHomeownerRepository
    {
		private readonly IDbConnectionProvider _dbConnectionProvider;

		public QuoteHomeownerRepository(IDbConnectionProvider dbConnectionProvider)
		{
			_dbConnectionProvider = dbConnectionProvider;
		}

		public IEnumerable<QuoteHomeowner> GetAll()
		{
			using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
			{
				var sql = "select * from dbo.[QuoteHomeowner]";
				return db.Query<QuoteHomeowner>(sql);
			}
		}
		
		public QuoteHomeowner GetByKey(int key)
		{
			using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
			{
				var sql = "select top 1 * from dbo.[QuoteHomeowner] where [Key] = @Key";
				return db.QueryFirstOrDefault<QuoteHomeowner>(sql, new { key });
			}
		}
		
		public int Create(QuoteHomeowner entity)
		{
			using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
			{
				db.Open();
				using (var transaction = db.BeginTransaction())
				{
					try
					{
						var sql = "insert into dbo.[QuoteHomeowner] ([Key], [PurchaseDate], [Address], [City], [State], [Zip], [Country], [Type], [Style], [NumberOfStories], [TownhouseUnitType], [TownhouseUnitLevel], [NumberOfUnits], [TotalLivingArea], [Condition], [ConstructionType], [ExteriorConstructionPercent], [YearBuilt], [RoofAge], [RoofMaterial], [BasementFoundation], [SmokeDetectors], [BurglarAlarm], [GatedCommunity], [PrivatePatrol], [SprinklerSystem], [RespondingFireDept], [MilesToFireDept], [DistanceToHydrant], [CityLimits], [CoverageRemediation], [QuoteAmount], [LossHistory], [CurrentCarrier], [NumberOfOccupants], [ClaimDetails], [ClaimNumber], [MobileHouseStyle], [RecentLossesAmount], [RecentLossesDetails], [AmountToBeQuoted], [Within1000ofHydrant], [County], [WoodStove], [ImprovementList], [IsOccupantSmoke], [NumbersOfBedrooms], [NumbersOfBathrooms], [GarageType], [AlarmSystemType], [AttachedGarage], [IsNewHomePurchase], [EstimatedPremium], [EstimatedDeductible], [QuoteVidNumber], [EstimatedWindHail], [LossOfUseAmount], [PersonalPropertyAmount]) values (@Key, @PurchaseDate, @Address, @City, @State, @Zip, @Country, @Type, @Style, @NumberOfStories, @TownhouseUnitType, @TownhouseUnitLevel, @NumberOfUnits, @TotalLivingArea, @Condition, @ConstructionType, @ExteriorConstructionPercent, @YearBuilt, @RoofAge, @RoofMaterial, @BasementFoundation, @SmokeDetectors, @BurglarAlarm, @GatedCommunity, @PrivatePatrol, @SprinklerSystem, @RespondingFireDept, @MilesToFireDept, @DistanceToHydrant, @CityLimits, @CoverageRemediation, @QuoteAmount, @LossHistory, @CurrentCarrier, @NumberOfOccupants, @ClaimDetails, @ClaimNumber, @MobileHouseStyle, @RecentLossesAmount, @RecentLossesDetails, @AmountToBeQuoted, @Within1000ofHydrant, @County, @WoodStove, @ImprovementList, @IsOccupantSmoke, @NumbersOfBedrooms, @NumbersOfBathrooms, @GarageType, @AlarmSystemType, @AttachedGarage, @IsNewHomePurchase, @EstimatedPremium, @EstimatedDeductible, @QuoteVidNumber, @EstimatedWindHail, @LossOfUseAmount, @PersonalPropertyAmount); select scope_identity();";
						var id = db.QueryFirstOrDefault<int>(sql, entity, transaction);
						transaction.Commit();
						return id;
					}
					catch (System.Exception ex)
					{
						Log.Error($"{nameof(QuoteHomeownerRepository)}: Error while attempting to insert QuoteHomeowner.", ex, this);
						transaction.Rollback();
						return 0;
					}
				}
			}
		}

        public int CreateOrUpdate(QuoteHomeowner entity)
        {
            using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
            {
                db.Open();
                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                        var sql = @"MERGE [dbo].[QuoteHomeowner] as target
                                    USING (SELECT @Key) as source ([Key])
                                    ON (target.[Key] = source.[Key])
                                    WHEN MATCHED THEN
                                    UPDATE 
                                    SET [Key] = @Key,[PurchaseDate] = @PurchaseDate, [PurchaseClosingDate] = @PurchaseClosingDate, [Address] = @Address,[City] = @City,[State] = @State,[Zip] = @Zip,[Country] = @Country,[Type] = @Type,[Style] = @Style,[NumberOfStories] = @NumberOfStories,[TownhouseUnitType] = @TownhouseUnitType,[TownhouseUnitLevel] = @TownhouseUnitLevel,[NumberOfUnits] = @NumberOfUnits,[TotalLivingArea] = @TotalLivingArea, [Condition] = @Condition, [ConstructionType] = @ConstructionType, [ExteriorConstructionPercent] = @ExteriorConstructionPercent, [YearBuilt] = @YearBuilt, [RoofAge] = @RoofAge, [RoofMaterial] = @RoofMaterial, [BasementFoundation] = @BasementFoundation, [SmokeDetectors] = @SmokeDetectors, [BurglarAlarm] = @BurglarAlarm, [GatedCommunity] = @GatedCommunity, [PrivatePatrol] = @PrivatePatrol, [SprinklerSystem] = @SprinklerSystem, [RespondingFireDept] = @RespondingFireDept, [MilesToFireDept] = @MilesToFireDept, [DistanceToHydrant] = @DistanceToHydrant, [CityLimits] = @CityLimits, [CoverageRemediation] = @CoverageRemediation, [QuoteAmount] = @QuoteAmount, [LossHistory] = @LossHistory, [CurrentCarrier] = @CurrentCarrier, [NumberOfOccupants] = @NumberOfOccupants, [ClaimDetails] = @ClaimDetails, [ClaimNumber] = @ClaimNumber, [MobileHouseStyle] = @MobileHouseStyle, [RecentLossesAmount] = @RecentLossesAmount, [RecentLossesDetails] = @RecentLossesDetails, [AmountToBeQuoted] = @AmountToBeQuoted, [Within1000ofHydrant] = @Within1000ofHydrant, [County] = @County, [WoodStove] = @WoodStove, [ImprovementList] = @ImprovementList, [IsOccupantSmoke] = @IsOccupantSmoke, [NumbersOfBedrooms] = @NumbersOfBedrooms, [NumbersOfBathrooms] = @NumbersOfBathrooms, [GarageType] = @GarageType, [AlarmSystemType] = @AlarmSystemType, [AttachedGarage] = @AttachedGarage, [IsNewHomePurchase] = @IsNewHomePurchase, [EstimatedPremium] = @EstimatedPremium, [EstimatedDeductible] = @EstimatedDeductible, [QuoteVidNumber] = @QuoteVidNumber, [EstimatedWindHail] = @EstimatedWindHail, [LossOfUseAmount] = @LossOfUseAmount, [PersonalPropertyAmount] = @PersonalPropertyAmount
                                    WHEN NOT MATCHED THEN
                                    INSERT ([Key], [PurchaseDate], [PurchaseClosingDate], [Address], [City], [State], [Zip], [Country], [Type], [Style], [NumberOfStories], [TownhouseUnitType], [TownhouseUnitLevel], [NumberOfUnits], [TotalLivingArea], [Condition], [ConstructionType], [ExteriorConstructionPercent], [YearBuilt], [RoofAge], [RoofMaterial], [BasementFoundation], [SmokeDetectors], [BurglarAlarm], [GatedCommunity], [PrivatePatrol], [SprinklerSystem], [RespondingFireDept], [MilesToFireDept], [DistanceToHydrant], [CityLimits], [CoverageRemediation], [QuoteAmount], [LossHistory], [CurrentCarrier], [NumberOfOccupants], [ClaimDetails], [ClaimNumber], [MobileHouseStyle], [RecentLossesAmount], [RecentLossesDetails], [AmountToBeQuoted], [Within1000ofHydrant], [County], [WoodStove], [ImprovementList], [IsOccupantSmoke], [NumbersOfBedrooms], [NumbersOfBathrooms], [GarageType], [AlarmSystemType], [AttachedGarage], [IsNewHomePurchase], [EstimatedPremium], [EstimatedDeductible], [QuoteVidNumber], [EstimatedWindHail], [LossOfUseAmount], [PersonalPropertyAmount])
                                    VALUES (@Key, @PurchaseDate, @PurchaseClosingDate, @Address, @City, @State, @Zip, @Country, @Type, @Style, @NumberOfStories, @TownhouseUnitType, @TownhouseUnitLevel, @NumberOfUnits, @TotalLivingArea, @Condition, @ConstructionType, @ExteriorConstructionPercent, @YearBuilt, @RoofAge, @RoofMaterial, @BasementFoundation, @SmokeDetectors, @BurglarAlarm, @GatedCommunity, @PrivatePatrol, @SprinklerSystem, @RespondingFireDept, @MilesToFireDept, @DistanceToHydrant, @CityLimits, @CoverageRemediation, @QuoteAmount, @LossHistory, @CurrentCarrier, @NumberOfOccupants, @ClaimDetails, @ClaimNumber, @MobileHouseStyle, @RecentLossesAmount, @RecentLossesDetails, @AmountToBeQuoted, @Within1000ofHydrant, @County, @WoodStove, @ImprovementList, @IsOccupantSmoke, @NumbersOfBedrooms, @NumbersOfBathrooms, @GarageType, @AlarmSystemType, @AttachedGarage, @IsNewHomePurchase, @EstimatedPremium, @EstimatedDeductible, @QuoteVidNumber, @EstimatedWindHail, @LossOfUseAmount, @PersonalPropertyAmount);";

                        var count = db.Execute(sql, entity, transaction);
                        transaction.Commit();
                        return count;

                    }
                    catch (System.Exception ex)
                    {
                        Log.Error($"{nameof(QuoteHomeownerRepository)}: Error while attempting to update QuoteHomeowner.", ex, this);
                        transaction.Rollback();
                        return 0;
                    }
                }
            }
        }
    }
}