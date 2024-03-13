using AFI.Feature.Data.Providers;
using Dapper;
using AFI.Feature.Data.DataModels;
using Sitecore.Diagnostics;
using System.Collections.Generic;

namespace AFI.Feature.Data.Repositories
{
    public interface IQuoteAutoRepository
    {
        IEnumerable<QuoteAuto> GetAll();
        QuoteAuto GetByKey(int key);
        int? Create(QuoteAuto entity);
        int CreateOrUpdate(QuoteAuto entity);
    }

    public class QuoteAutoRepository : IQuoteAutoRepository
    {
		private readonly IDbConnectionProvider _dbConnectionProvider;

		public QuoteAutoRepository(IDbConnectionProvider dbConnectionProvider)
		{
			_dbConnectionProvider = dbConnectionProvider;
		}

		public IEnumerable<QuoteAuto> GetAll()
		{
			using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
			{
				var sql = "select * from dbo.[QuoteAuto]";
				return db.Query<QuoteAuto>(sql);
			}
		}
		
		public QuoteAuto GetByKey(int key)
		{
			using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
			{
				var sql = "select top 1 * from dbo.[QuoteAuto] where [Key] = @Key";
				return db.QueryFirstOrDefault<QuoteAuto>(sql, new { key });
			}
		}
		
		public int? Create(QuoteAuto entity)
		{
			using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
			{
				db.Open();
				using (var transaction = db.BeginTransaction())
				{
					try
					{
						var sql = "insert into dbo.[QuoteAuto] ([Key], [BodilyInjury], [PropertyDamage], [MedicalCoverage], [UninsuredBodilyInjury], [PersonalInjury], [ComprehensiveDeductible], [CollisionDeductible], [CurrentInsurance], [CurrentPolicyDate], [CurrentPolicyAction], [CurrentBodilyInjury], [IncidentsLast3], [PersonalEffects1], [PersonalEffects2], [CurrentBodilyInjuryLimit], [NumberOfLicensedDrivers], [NumberOfDailyUseVehiclesInHousehold], [DescribeAnyAutoRelatedAccidents]) values (@Key, @BodilyInjury, @PropertyDamage, @MedicalCoverage, @UninsuredBodilyInjury, @PersonalInjury, @ComprehensiveDeductible, @CollisionDeductible, @CurrentInsurance, @CurrentPolicyDate, @CurrentPolicyAction, @CurrentBodilyInjury, @IncidentsLast3, @PersonalEffects1, @PersonalEffects2, @CurrentBodilyInjuryLimit, @NumberOfLicensedDrivers, @NumberOfDailyUseVehiclesInHousehold, @DescribeAnyAutoRelatedAccidents); select scope_identity();";
						var id = db.QueryFirstOrDefault<int?>(sql, entity, transaction);
						transaction.Commit();
						return id;
					}
					catch (System.Exception ex)
					{
						Log.Error($"{nameof(QuoteAutoRepository)}: Error while attempting to insert QuoteAuto.", ex, this);
						transaction.Rollback();
						return 0;
					}
				}
			}
		}

        public int CreateOrUpdate(QuoteAuto entity)
        {
            using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
            {
                db.Open();
                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                        var sql = @"MERGE [dbo].[QuoteAuto] as target
                                    USING (SELECT @Key) as source ([Key])
                                    ON (target.[Key] = source.[Key])
                                    WHEN MATCHED THEN
                                    UPDATE 
                                    SET [BodilyInjury] = @BodilyInjury,[PropertyDamage] = @PropertyDamage,[MedicalCoverage] = @MedicalCoverage,[UninsuredBodilyInjury] = @UninsuredBodilyInjury,[PersonalInjury] = @PersonalInjury,[ComprehensiveDeductible] = @ComprehensiveDeductible,[CollisionDeductible] = @CollisionDeductible,[CurrentInsurance] = @CurrentInsurance,[CurrentPolicyDate] = @CurrentPolicyDate,[CurrentPolicyAction] = @CurrentPolicyAction,[CurrentBodilyInjury] = @CurrentBodilyInjury,[IncidentsLast3] = @IncidentsLast3,[PersonalEffects1] = @PersonalEffects1,[PersonalEffects2] = @PersonalEffects2,[CurrentBodilyInjuryLimit] = @CurrentBodilyInjuryLimit,[NumberOfLicensedDrivers] = @NumberOfLicensedDrivers,[NumberOfDailyUseVehiclesInHousehold] = @NumberOfDailyUseVehiclesInHousehold,[DescribeAnyAutoRelatedAccidents] = @DescribeAnyAutoRelatedAccidents
                                    WHEN NOT MATCHED THEN
                                    INSERT ([Key], [BodilyInjury], [PropertyDamage], [MedicalCoverage], [UninsuredBodilyInjury], [PersonalInjury], [ComprehensiveDeductible], [CollisionDeductible], [CurrentInsurance], [CurrentPolicyDate], [CurrentPolicyAction], [CurrentBodilyInjury], [IncidentsLast3], [PersonalEffects1], [PersonalEffects2], [CurrentBodilyInjuryLimit], [NumberOfLicensedDrivers], [NumberOfDailyUseVehiclesInHousehold], [DescribeAnyAutoRelatedAccidents]) 
                                    VALUES (@Key, @BodilyInjury, @PropertyDamage, @MedicalCoverage, @UninsuredBodilyInjury, @PersonalInjury, @ComprehensiveDeductible, @CollisionDeductible, @CurrentInsurance, @CurrentPolicyDate, @CurrentPolicyAction, @CurrentBodilyInjury, @IncidentsLast3, @PersonalEffects1, @PersonalEffects2, @CurrentBodilyInjuryLimit, @NumberOfLicensedDrivers, @NumberOfDailyUseVehiclesInHousehold, @DescribeAnyAutoRelatedAccidents);";

                        var count = db.Execute(sql, entity, transaction);
                        transaction.Commit();
                        return count;

                    }
                    catch (System.Exception ex)
                    {
                        Log.Error($"{nameof(QuoteAutoRepository)}: Error while attempting to update QuoteAuto.", ex, this);
                        transaction.Rollback();
                        return 0;
                    }
                }
            }
        }
    }
}