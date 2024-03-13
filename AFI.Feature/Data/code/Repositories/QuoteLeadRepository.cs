using AFI.Feature.Data.Providers;
using Dapper;
using AFI.Feature.Data.DataModels;
using Sitecore.Diagnostics;
using System.Collections.Generic;

namespace AFI.Feature.Data.Repositories
{
    public interface IQuoteLeadRepository
    {
        IEnumerable<QuoteLead> GetAll();
		QuoteLead GetByKey(int key);
        int? Create(QuoteLead entity);
        int CreateOrUpdate(QuoteLead entity);
    }

    public class QuoteLeadRepository : IQuoteLeadRepository
	{
		private readonly IDbConnectionProvider _dbConnectionProvider;

		public QuoteLeadRepository(IDbConnectionProvider dbConnectionProvider)
		{
			_dbConnectionProvider = dbConnectionProvider;
		}

		public IEnumerable<QuoteLead> GetAll()
		{
			using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
			{
				var sql = "select * from dbo.[QuoteHillAFB]";
				return db.Query<QuoteLead>(sql);
			}
		}
		
		public QuoteLead GetByKey(int key)
		{
			using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
			{
				var sql = "select top 1 * from dbo.[QuoteHillAFB] where [Key] = @Key";
				return db.QueryFirstOrDefault<QuoteLead>(sql, new { key });
			}
		}
		
		public int? Create(QuoteLead entity)
		{
			using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
			{
				db.Open();
				using (var transaction = db.BeginTransaction())
				{
					try
					{
						var sql = "insert into dbo.[QuoteHillAFB] ([Key], [PolicyHolderMailingAddress2],[PropertyAddress2]) values (@Key, @PolicyHolderMailingAddress2,@PropertyAddress2); select scope_identity();";
						var id = db.QueryFirstOrDefault<int?>(sql, entity, transaction);
						transaction.Commit();
						return id;
					}
					catch (System.Exception ex)
					{
						Log.Error($"{nameof(QuoteLeadRepository)}: Error while attempting to insert QuoteHillAFB.", ex, this);
						transaction.Rollback();
						return 0;
					}
				}
			}
		}

        public int CreateOrUpdate(QuoteLead entity)
        {
            using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
            {
                db.Open();
                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                        var sql = @"MERGE [dbo].[QuoteHillAFB] as target
                                    USING (SELECT @Key) as source ([Key])
                                    ON (target.[Key] = source.[Key])
                                    WHEN MATCHED THEN
                                    UPDATE 
                                    SET [Key] = @Key,[PolicyHolderMailingAddress2] = @PolicyHolderMailingAddress2 
                                    WHEN NOT MATCHED THEN
                                    INSERT ([Key], [PolicyHolderMailingAddress2],[PropertyAddress2]) 
                                    VALUES (@Key, @PolicyHolderMailingAddress2, @PropertyAddress2);";

                        var count = db.Execute(sql, entity, transaction);
                        transaction.Commit();
                        return count;

                    }
                    catch (System.Exception ex)
                    {
                        Log.Error($"{nameof(QuoteLeadRepository)}: Error while attempting to update QuoteHillAFB.", ex, this);
                        transaction.Rollback();
                        return 0;
                    }
                }
            }
        }
    }
}