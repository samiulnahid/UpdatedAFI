using AFI.Feature.Data.Providers;
using Dapper;
using AFI.Feature.Data.DataModels;
using Sitecore.Diagnostics;
using System.Collections.Generic;

namespace AFI.Feature.Data.Repositories
{
    public interface IQuoteFloodRepository
    {
        IEnumerable<QuoteFlood> GetAll();
        QuoteFlood GetByKey(int key);
        int? Create(QuoteFlood entity);
        int CreateOrUpdate(QuoteFlood entity);
    }

    public class QuoteFloodRepository : IQuoteFloodRepository
    {
		private readonly IDbConnectionProvider _dbConnectionProvider;

		public QuoteFloodRepository(IDbConnectionProvider dbConnectionProvider)
		{
			_dbConnectionProvider = dbConnectionProvider;
		}

		public IEnumerable<QuoteFlood> GetAll()
		{
			using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
			{
				var sql = "select * from dbo.[QuoteFlood]";
				return db.Query<QuoteFlood>(sql);
			}
		}
		
		public QuoteFlood GetByKey(int key)
		{
			using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
			{
				var sql = "select top 1 * from dbo.[QuoteFlood] where [Key] = @Key";
				return db.QueryFirstOrDefault<QuoteFlood>(sql, new { key });
			}
		}
		
		public int? Create(QuoteFlood entity)
		{
			using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
			{
				db.Open();
				using (var transaction = db.BeginTransaction())
				{
					try
					{
						var sql = "insert into dbo.[QuoteFlood] ([Key],[LocationDifferentThanMailing], [Address], [ZipCode], [City], [State], [IsCondo], [CondoFloor], [FloorsInStructure], [FoundationType], [ConstructionDate], [OccupiedType], [TotalLivingArea], [StructureValue], [GarageType], [GarageValue], [FloodZone], [AwareOfFloodLosses], [NumberOfLosses], [MortgageInsAmount]) values (@Key, @LocationDifferentThanMailing, @Address, @ZipCode, @City, @State, @IsCondo, @CondoFloor, @FloorsInStructure, @FoundationType, @ConstructionDate, @OccupiedType, @TotalLivingArea, @StructureValue, @GarageType, @GarageValue, @FloodZone, @AwareOfFloodLosses, @NumberOfLosses, @MortgageInsAmount); select scope_identity();";
						var id = db.QueryFirstOrDefault<int?>(sql, entity, transaction);

                        //var sqlQ = "UPDATE [dbo].[AFI_Marketing_Suspect_Temp] SET [FirstName]=@FirstName, [LastName]=@LastName, [Email]=@Email, [Phone]=@Phone, [Address]=@Address, [City]=@City, [State]=@State, [ZipCode]=@ZipCode, [DateOfBirth]=@DateOfBirth, [LastUpdated]=GETDATE() where [EntityID]=" + entity.Key;
                        //var newId = db.Execute(sqlQ, new
                        //{
                        //    FirstName = entity.FirstName,
                        //    LastName = entity.LastName,
                        //    Email = entity.Email,
                        //    Phone = entity.PhoneNumber,
                        //    Address = entity.Address,
                        //    City = entity.City,
                        //    State = entity.State,
                        //    ZipCode = entity.ZipCode,
                        //    DateOfBirth = entity.BirthDate
                        //}, transaction);

                        transaction.Commit();
						return id;
					}
					catch (System.Exception ex)
					{
						Log.Error($"{nameof(QuoteFloodRepository)}: Error while attempting to insert QuoteFlood.", ex, this);
						transaction.Rollback();
						return 0;
					}
				}
			}
		}

        public int CreateOrUpdate(QuoteFlood entity)
        {
            using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
            {
                db.Open();
                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                        var sql = @"MERGE [dbo].[QuoteFlood] as target
                                    USING (SELECT @Key) as source ([Key])
                                    ON (target.[Key] = source.[Key])
                                    WHEN MATCHED THEN
                                    UPDATE 
                                    SET [Key] = @Key,[LocationDifferentThanMailing] = @LocationDifferentThanMailing,[Address] = @Address,[ZipCode] = @ZipCode,[City] = @City,[State] = @State,[IsCondo] = @IsCondo,[CondoFloor] = @CondoFloor,[FloorsInStructure] = @FloorsInStructure,[FoundationType] = @FoundationType,[ConstructionDate] = @ConstructionDate,[OccupiedType] = @OccupiedType,[TotalLivingArea] = @TotalLivingArea,[StructureValue] = @StructureValue,[GarageType] = @GarageType,[GarageValue] = @GarageValue,[FloodZone] = @FloodZone,[AwareOfFloodLosses] = @AwareOfFloodLosses,[NumberOfLosses] = @NumberOfLosses,[MortgageInsAmount] = @MortgageInsAmount
                                    WHEN NOT MATCHED THEN
                                    INSERT ([Key],[LocationDifferentThanMailing], [Address], [ZipCode], [City], [State], [IsCondo], [CondoFloor], [FloorsInStructure], [FoundationType], [ConstructionDate], [OccupiedType], [TotalLivingArea], [StructureValue], [GarageType], [GarageValue], [FloodZone], [AwareOfFloodLosses], [NumberOfLosses], [MortgageInsAmount])
                                    VALUES (@Key, @LocationDifferentThanMailing, @Address, @ZipCode, @City, @State, @IsCondo, @CondoFloor, @FloorsInStructure, @FoundationType, @ConstructionDate, @OccupiedType, @TotalLivingArea, @StructureValue, @GarageType, @GarageValue, @FloodZone, @AwareOfFloodLosses, @NumberOfLosses, @MortgageInsAmount);";

                        var count = db.Execute(sql, entity, transaction);
                        transaction.Commit();
                        return count;

                    }
                    catch (System.Exception ex)
                    {
                        Log.Error($"{nameof(QuoteFloodRepository)}: Error while attempting to update QuoteFlood.", ex, this);
                        transaction.Rollback();
                        return 0;
                    }
                }
            }
        }
    }
}