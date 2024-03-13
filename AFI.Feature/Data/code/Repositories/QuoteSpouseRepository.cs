using AFI.Feature.Data.Providers;
using Dapper;
using AFI.Feature.Data.DataModels;
using Sitecore.Diagnostics;
using System.Collections.Generic;

namespace AFI.Feature.Data.Repositories
{
    public interface IQuoteSpouseRepository
    {
        IEnumerable<QuoteSpouse> GetAll();
        QuoteSpouse GetByKey(int key);
        int Create(QuoteSpouse entity);
        int CreateOrUpdate(QuoteSpouse entity);
    }

    public class QuoteSpouseRepository : IQuoteSpouseRepository
    {
		private readonly IDbConnectionProvider _dbConnectionProvider;

		public QuoteSpouseRepository(IDbConnectionProvider dbConnectionProvider)
		{
			_dbConnectionProvider = dbConnectionProvider;
		}

		public IEnumerable<QuoteSpouse> GetAll()
		{
			using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
			{
				var sql = "select * from dbo.[QuoteSpouse]";
				return db.Query<QuoteSpouse>(sql);
			}
		}
		
		public QuoteSpouse GetByKey(int key)
		{
			using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
			{
				var sql = "select top 1 * from dbo.[QuoteSpouse] where [Key] = @Key";
				return db.QueryFirstOrDefault<QuoteSpouse>(sql, new { key });
			}
		}
		
		public int Create(QuoteSpouse entity)
		{
			using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
			{
				db.Open();
				using (var transaction = db.BeginTransaction())
				{
					try
					{
						var sql = "insert into dbo.[QuoteSpouse] ([Key], [FirstName], [LastName], [Officer], [ServiceBranch], [ServiceRank], [ServiceStatus], [ServiceDischargeType], [CommissioningProgram], [Suffix]) values (@Key, @FirstName, @LastName, @Officer, @ServiceBranch, @ServiceRank, @ServiceStatus, @ServiceDischargeType, @CommissioningProgram, @Suffix); select scope_identity();";
						var id = db.QueryFirstOrDefault<int>(sql, entity, transaction);
						transaction.Commit();
						return id;
					}
					catch (System.Exception ex)
					{
						Log.Error($"{nameof(QuoteSpouseRepository)}: Error while attempting to insert QuoteSpouse.", ex, this);
						transaction.Rollback();
						return 0;
					}
				}
			}
		}
        

        public int CreateOrUpdate(QuoteSpouse entity)
        {
            using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
            {
                db.Open();
                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                        var sql = @"MERGE [dbo].[QuoteSpouse] as target
                                    USING (SELECT @Key) as source ([Key])
                                    ON (target.[Key] = source.[Key])
                                    WHEN MATCHED THEN
                                    UPDATE 
                                    SET [Key] = @Key,[FirstName] = @FirstName,[LastName] = @LastName,[Officer] = @Officer,[ServiceBranch] = @ServiceBranch,[ServiceRank] = @ServiceRank,[ServiceStatus] = @ServiceStatus,[ServiceDischargeType] = @ServiceDischargeType,[CommissioningProgram] = @CommissioningProgram,[Suffix] = @Suffix
                                    WHEN NOT MATCHED THEN
                                    INSERT ([Key], [FirstName], [LastName], [Officer], [ServiceBranch], [ServiceRank], [ServiceStatus], [ServiceDischargeType], [CommissioningProgram], [Suffix])
                                    VALUES (@Key, @FirstName, @LastName, @Officer, @ServiceBranch, @ServiceRank, @ServiceStatus, @ServiceDischargeType, @CommissioningProgram, @Suffix);";

                        var count = db.Execute(sql, entity, transaction);
                        transaction.Commit();
                        return count;

                    }
                    catch (System.Exception ex)
                    {
                        Log.Error($"{nameof(QuoteSpouseRepository)}: Error while attempting to update QuoteSpouse.", ex, this);
                        transaction.Rollback();
                        return 0;
                    }
                }
            }
        }
    }
}