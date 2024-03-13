using AFI.Feature.Data.Providers;
using Dapper;
using AFI.Feature.Data.DataModels;
using Sitecore.Diagnostics;
using System.Collections.Generic;
using System.Linq;

namespace AFI.Feature.Data.Repositories
{
    public interface IQuoteUmbrellaWatercraftRepository
    {
        IEnumerable<QuoteUmbrellaWatercraft> GetAllForQuote(int quoteKey);
        void Create(QuoteUmbrellaWatercraft entity);
        int CreateOrUpdate(IEnumerable<QuoteUmbrellaWatercraft> entities);
    }

    public class QuoteUmbrellaWatercraftRepository : IQuoteUmbrellaWatercraftRepository
    {
		private readonly IDbConnectionProvider _dbConnectionProvider;

		public QuoteUmbrellaWatercraftRepository(IDbConnectionProvider dbConnectionProvider)
		{
			_dbConnectionProvider = dbConnectionProvider;
		}

		public IEnumerable<QuoteUmbrellaWatercraft> GetAllForQuote(int quoteKey)
		{
			using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
			{
				var sql = "select * from dbo.[QuoteUmbrellaWatercraft] where [QuoteKey] = @quoteKey";
				return db.Query<QuoteUmbrellaWatercraft>(sql, new { quoteKey });
			}
		}

		public void Create(QuoteUmbrellaWatercraft entity)
		{
			using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
			{
				db.Open();
				using (var transaction = db.BeginTransaction())
				{
					try
					{
						var sql = "insert into dbo.[QuoteUmbrellaWatercraft] ([Key], [QuoteKey], [WatercraftType], [WatercraftLength], [WatercraftHorsepower], [WatercraftUnderlyingInsurance]) values (@Key, @QuoteKey, @WatercraftType, @WatercraftLength, @WatercraftHorsepower, @WatercraftUnderlyingInsurance)";
						db.Execute(sql, entity, transaction);
						transaction.Commit();
					}
					catch (System.Exception ex)
					{
						Log.Error($"{nameof(QuoteUmbrellaWatercraftRepository)}: Error while attempting to insert QuoteUmbrellaWatercraft.", ex, this);
						transaction.Rollback();
					}
				}
			}
		}

        public int CreateOrUpdate(IEnumerable<QuoteUmbrellaWatercraft> entities)
        {
            if (!entities.Any()) { return 0; }

            int QUOTE_KEY = entities.First().QuoteKey;

            using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
            {
                db.Open();
                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                        var sql = "DELETE FROM dbo.[QuoteUmbrellaWatercraft] WHERE [QuoteKey] = @QuoteKey";
                        db.Execute(sql, new { QuoteKey = QUOTE_KEY }, transaction);
                    }
                    catch (System.Exception ex)
                    {
                        Log.Error($"{nameof(QuoteUmbrellaWatercraftRepository)}: Error while attempting to Delete from QuoteUmbrellaWatercraft.", ex, this);
                        transaction.Rollback();
                        return 0;
                    }

                    try
                    {
                        var counter = 0;
                        var sql = "insert into dbo.[QuoteUmbrellaWatercraft] ([Key], [QuoteKey], [WatercraftType], [WatercraftLength], [WatercraftHorsepower], [WatercraftUnderlyingInsurance]) values (@Key, @QuoteKey, @WatercraftType, @WatercraftLength, @WatercraftHorsepower, @WatercraftUnderlyingInsurance)";
                        foreach (var entity in entities)
                        {
                            db.Execute(sql, entity, transaction);
                            counter++;
                        }
                        transaction.Commit();
                        return counter;
                    }
                    catch (System.Exception ex)
                    {
                        Log.Error($"{nameof(QuoteUmbrellaWatercraftRepository)}: Error while attempting to insert QuoteUmbrellaWatercraft.", ex, this);
                        transaction.Rollback();
                        return 0;
                    }
                }
            }
        }
    }
}