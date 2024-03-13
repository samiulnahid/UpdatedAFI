using AFI.Feature.Data.Providers;
using Dapper;
using AFI.Feature.Data.DataModels;
using Sitecore.Diagnostics;
using System.Collections.Generic;

namespace AFI.Feature.Data.Repositories
{
    public interface IQuoteLeadGenerationRepository
	{
        IEnumerable<QuoteLeadGeneration> GetAll();
		QuoteLeadGeneration GetByKey(int key);
        int? Create(QuoteLeadGeneration entity);
        int CreateOrUpdate(QuoteLeadGeneration entity);
    }

    public class QuoteLeadGenerationRepository : IQuoteLeadGenerationRepository
	{
		private readonly IDbConnectionProvider _dbConnectionProvider;

		public QuoteLeadGenerationRepository(IDbConnectionProvider dbConnectionProvider)
		{
			_dbConnectionProvider = dbConnectionProvider;
		}

		public IEnumerable<QuoteLeadGeneration> GetAll()
		{
			using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
			{
				var sql = "select * from dbo.[QuoteLeadGeneration]";
				return db.Query<QuoteLeadGeneration>(sql);
			}
		}
		
		public QuoteLeadGeneration GetByKey(int key)
		{
			using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
			{
				var sql = "select top 1 * from dbo.[QuoteLeadGeneration] where [Key] = @Key";
				return db.QueryFirstOrDefault<QuoteLeadGeneration>(sql, new { key });
			}
		}
		
		public int? Create(QuoteLeadGeneration entity)
		{
			using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
			{
				db.Open();
				using (var transaction = db.BeginTransaction())
				{
					try
					{
						var sql = "insert into dbo.[QuoteLeadGeneration] ([Key], [PolicyHolderMailingAddress2]) values (@Key, @PolicyHolderMailingAddress2); select scope_identity();";
						var id = db.QueryFirstOrDefault<int?>(sql, entity, transaction);
						transaction.Commit();
						return id;
					}
					catch (System.Exception ex)
					{
						Log.Error($"{nameof(QuoteLeadGenerationRepository)}: Error while attempting to insert QuoteLeadGeneration.", ex, this);
						transaction.Rollback();
						return 0;
					}
				}
			}
		}

        public int CreateOrUpdate(QuoteLeadGeneration entity)
        {
            using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
            {
                db.Open();
                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                        var sql = @"MERGE [dbo].[QuoteLeadGeneration] as target
                                    USING (SELECT @Key) as source ([Key])
                                    ON (target.[Key] = source.[Key])
                                    WHEN MATCHED THEN
                                    UPDATE 
                                    SET [Key] = @Key,[PolicyHolderMailingAddress2] = @PolicyHolderMailingAddress2 
                                    WHEN NOT MATCHED THEN
                                    INSERT ([Key], [PolicyHolderMailingAddress2]) 
                                    VALUES (@Key, @PolicyHolderMailingAddress2);";

                        var count = db.Execute(sql, entity, transaction);
                        transaction.Commit();
                        return count;

                    }
                    catch (System.Exception ex)
                    {
                        Log.Error($"{nameof(QuoteLeadGenerationRepository)}: Error while attempting to update LeadGeneration.", ex, this);
                        transaction.Rollback();
                        return 0;
                    }
                }
            }
        }
    }
}