using AFI.Feature.Data.Providers;
using Dapper;
using AFI.Feature.Data.DataModels;
using Sitecore.Diagnostics;
using System.Collections.Generic;

namespace AFI.Feature.Data.Repositories
{
    public interface IQuoteUmbrellaRepository
    {
        IEnumerable<QuoteUmbrella> GetAll();
        QuoteUmbrella GetByKey(int key);
        int? Create(QuoteUmbrella entity);
        int CreateOrUpdate(QuoteUmbrella entity);
    }

    public class QuoteUmbrellaRepository : IQuoteUmbrellaRepository
    {
		private readonly IDbConnectionProvider _dbConnectionProvider;

		public QuoteUmbrellaRepository(IDbConnectionProvider dbConnectionProvider)
		{
			_dbConnectionProvider = dbConnectionProvider;
		}

		public IEnumerable<QuoteUmbrella> GetAll()
		{
			using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
			{
				var sql = "select * from dbo.[QuoteUmbrella]";
				return db.Query<QuoteUmbrella>(sql);
			}
		}
		
		public QuoteUmbrella GetByKey(int key)
		{
			using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
			{
				var sql = "select top 1 * from dbo.[QuoteUmbrella] where [Key] = @Key";
				return db.QueryFirstOrDefault<QuoteUmbrella>(sql, new { key });
			}
		}
		
		public int? Create(QuoteUmbrella entity)
		{
			using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
			{
				db.Open();
				using (var transaction = db.BeginTransaction())
				{
					try
					{
						var sql = "insert into dbo.[QuoteUmbrella] ([Key], [CoverageAmount], [UnderlyingInsurance], [NumberOfDrivers], [DriversUnder25], [NumberOfVehicles], [VehicleUnderlyingInsurance], [OwnRentalProperty], [NumberOfRentalProperties], [RentalUnderlyingInsurance], [DriversOver70], [Incident1Driver], [Incident1Type], [Incident1Date], [Incident2Driver], [Incident2Type], [Incident2Date], [Incident3Driver], [Incident3Type], [Incident3Date], [Incident4Driver], [Incident4Type], [Incident4Date], [Incident5Driver], [Incident5Type], [Incident5Date], [DriversUnder22]) values (@Key, @CoverageAmount, @UnderlyingInsurance, @NumberOfDrivers, @DriversUnder25, @NumberOfVehicles, @VehicleUnderlyingInsurance, @OwnRentalProperty, @NumberOfRentalProperties, @RentalUnderlyingInsurance, @DriversOver70, @Incident1Driver, @Incident1Type, @Incident1Date, @Incident2Driver, @Incident2Type, @Incident2Date, @Incident3Driver, @Incident3Type, @Incident3Date, @Incident4Driver, @Incident4Type, @Incident4Date, @Incident5Driver, @Incident5Type, @Incident5Date, @DriversUnder22); select scope_identity();";
						var id = db.QueryFirstOrDefault<int?>(sql, entity, transaction);
						transaction.Commit();
						return id;
					}
					catch (System.Exception ex)
					{
						Log.Error($"{nameof(QuoteUmbrellaRepository)}: Error while attempting to insert QuoteUmbrella.", ex, this);
						transaction.Rollback();
						return 0;
					}
				}
			}
		}
        

        public int CreateOrUpdate(QuoteUmbrella entity)
        {
            using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
            {
                db.Open();
                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                        var sql = @"MERGE [dbo].[QuoteUmbrella] as target
                                    USING (SELECT @Key) as source ([Key])
                                    ON (target.[Key] = source.[Key])
                                    WHEN MATCHED THEN
                                    UPDATE 
                                    SET [Key] = @Key,[CoverageAmount] = @CoverageAmount,[UnderlyingInsurance] = @UnderlyingInsurance,[NumberOfDrivers] = @NumberOfDrivers,[DriversUnder25] = @DriversUnder25,[NumberOfVehicles] = @NumberOfVehicles,[VehicleUnderlyingInsurance] = @VehicleUnderlyingInsurance,[OwnRentalProperty] = @OwnRentalProperty,[NumberOfRentalProperties] = @NumberOfRentalProperties,[RentalUnderlyingInsurance] = @RentalUnderlyingInsurance,[DriversOver70] = @DriversOver70,[Incident1Driver] = @Incident1Driver,[Incident1Type] = @Incident1Type,[Incident1Date] = @Incident1Date,[Incident2Driver] = @Incident2Driver,[Incident2Type] = @Incident2Type,[Incident2Date] = @Incident2Date,[Incident3Driver] = @Incident3Driver,[Incident3Type] = @Incident3Type,[Incident3Date] = @Incident3Date,[Incident4Driver] = @Incident4Driver,[Incident4Type] = @Incident4Type,[Incident4Date] = @Incident4Date,[Incident5Driver] = @Incident5Driver,[Incident5Type] = @Incident5Type,[Incident5Date] = @Incident5Date,[DriversUnder22] = @DriversUnder22
                                    WHEN NOT MATCHED THEN
                                    INSERT ([Key], [CoverageAmount], [UnderlyingInsurance], [NumberOfDrivers], [DriversUnder25], [NumberOfVehicles], [VehicleUnderlyingInsurance], [OwnRentalProperty], [NumberOfRentalProperties], [RentalUnderlyingInsurance], [DriversOver70], [Incident1Driver], [Incident1Type], [Incident1Date], [Incident2Driver], [Incident2Type], [Incident2Date], [Incident3Driver], [Incident3Type], [Incident3Date], [Incident4Driver], [Incident4Type], [Incident4Date], [Incident5Driver], [Incident5Type], [Incident5Date], [DriversUnder22])
                                    VALUES (@Key, @CoverageAmount, @UnderlyingInsurance, @NumberOfDrivers, @DriversUnder25, @NumberOfVehicles, @VehicleUnderlyingInsurance, @OwnRentalProperty, @NumberOfRentalProperties, @RentalUnderlyingInsurance, @DriversOver70, @Incident1Driver, @Incident1Type, @Incident1Date, @Incident2Driver, @Incident2Type, @Incident2Date, @Incident3Driver, @Incident3Type, @Incident3Date, @Incident4Driver, @Incident4Type, @Incident4Date, @Incident5Driver, @Incident5Type, @Incident5Date, @DriversUnder22);";

                        var count = db.Execute(sql, entity, transaction);
                        transaction.Commit();
                        return count;

                    }
                    catch (System.Exception ex)
                    {
                        Log.Error($"{nameof(QuoteUmbrellaRepository)}: Error while attempting to update QuoteUmbrella.", ex, this);
                        transaction.Rollback();
                        return 0;
                    }
                }
            }
        }
    }
}