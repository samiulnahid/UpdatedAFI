using AFI.Feature.Data.Providers;
using Dapper;
using AFI.Feature.Data.DataModels;
using Sitecore.Diagnostics;
using System.Collections.Generic;

namespace AFI.Feature.Data.Repositories
{
    public interface IQuoteHillAFBRepository
    {
        IEnumerable<QuoteHillAFB> GetAll();
		QuoteHillAFB GetByKey(int key);
        int? Create(QuoteHillAFB entity);
        int CreateOrUpdate(QuoteHillAFB entity);
    }

    public class QuoteHillAFBRepository : IQuoteHillAFBRepository
	{
		private readonly IDbConnectionProvider _dbConnectionProvider;

		public QuoteHillAFBRepository(IDbConnectionProvider dbConnectionProvider)
		{
			_dbConnectionProvider = dbConnectionProvider;
		}

		public IEnumerable<QuoteHillAFB> GetAll()
		{
			using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
			{
				var sql = "select * from dbo.[QuoteHillAFB]";
				return db.Query<QuoteHillAFB>(sql);
			}
		}
		
		public QuoteHillAFB GetByKey(int key)
		{
			using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
			{
				var sql = "select top 1 * from dbo.[QuoteHillAFB] where [Key] = @Key";
				return db.QueryFirstOrDefault<QuoteHillAFB>(sql, new { key });
			}
		}
		
		public int? Create(QuoteHillAFB entity)
		{
			using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
			{
				db.Open();
				using (var transaction = db.BeginTransaction())
				{
					try
					{
						var sql = "insert into dbo.[QuoteHillAFB] ([Key], [PolicyHolderMailingAddress2]) values (@Key, @PolicyHolderMailingAddress2); select scope_identity();";
						var id = db.QueryFirstOrDefault<int?>(sql, entity, transaction);
						transaction.Commit();
						return id;
					}
					catch (System.Exception ex)
					{
						Log.Error($"{nameof(QuoteHillAFBRepository)}: Error while attempting to insert QuoteHillAFB.", ex, this);
						transaction.Rollback();
						return 0;
					}
				}
			}
		}

        public int CreateOrUpdate(QuoteHillAFB entity)
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
                                    INSERT ([Key], [PolicyHolderMailingAddress2]) 
                                    VALUES (@Key, @PolicyHolderMailingAddress2);";

                        var count = db.Execute(sql, entity, transaction);
                        transaction.Commit();
                        return count;

                    }
                    catch (System.Exception ex)
                    {
                        Log.Error($"{nameof(QuoteHillAFBRepository)}: Error while attempting to update QuoteHillAFB.", ex, this);
                        transaction.Rollback();
                        return 0;
                    }
                }
            }
        }
    }
}