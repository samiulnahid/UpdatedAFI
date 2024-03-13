using AFI.Feature.Data.Providers;
using Dapper;
using AFI.Feature.Data.DataModels;
using Sitecore.Diagnostics;
using System.Collections.Generic;
using System.Linq;

namespace AFI.Feature.Data.Repositories
{
    public interface IQuoteHomeownerLossRepository
    {
        IEnumerable<QuoteHomeownerLoss> GetAllForQuote(int quoteKey);
        void Create(QuoteHomeownerLoss entity);
        int CreateOrUpdate(IEnumerable<QuoteHomeownerLoss> entities);
    }

    public class QuoteHomeownerLossRepository : IQuoteHomeownerLossRepository
    {
		private readonly IDbConnectionProvider _dbConnectionProvider;

		public QuoteHomeownerLossRepository(IDbConnectionProvider dbConnectionProvider)
		{
			_dbConnectionProvider = dbConnectionProvider;
		}

		public IEnumerable<QuoteHomeownerLoss> GetAllForQuote(int quoteKey)
		{
			using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
			{
				var sql = "select * from dbo.[QuoteHomeownerLoss] where [QuoteKey] = @quoteKey";
				return db.Query<QuoteHomeownerLoss>(sql, new { quoteKey });
			}
		}

		public void Create(QuoteHomeownerLoss entity)
		{
			using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
			{
				db.Open();
				using (var transaction = db.BeginTransaction())
				{
					try
					{
						var sql = "insert into dbo.[QuoteHomeownerLoss] ([Key], [QuoteKey], [Month], [Year], [CauseOfLoss], [AmountOfLoss]) values (@Key, @QuoteKey, @Month, @Year, @CauseOfLoss, @AmountOfLoss)";
						db.Execute(sql, entity, transaction);
						transaction.Commit();
					}
					catch (System.Exception ex)
					{
						Log.Error($"{nameof(QuoteHomeownerLossRepository)}: Error while attempting to insert QuoteHomeownerLoss.", ex, this);
						transaction.Rollback();
					}
				}
			}
		}

        public int CreateOrUpdate(IEnumerable<QuoteHomeownerLoss> entities)
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
                        var sql = "DELETE FROM dbo.[QuoteHomeownerLoss] WHERE [QuoteKey] = @QuoteKey";
                        db.Execute(sql, new { QuoteKey = QUOTE_KEY }, transaction);
                    }
                    catch (System.Exception ex)
                    {
                        Log.Error($"{nameof(QuoteHomeownerLossRepository)}: Error while attempting to Delete from QuoteHomeownerLoss.", ex, this);
                        transaction.Rollback();
                        return 0;
                    }

                    try
                    {
                        var counter = 0;
                        var sql = "insert into dbo.[QuoteHomeownerLoss] ([Key], [QuoteKey], [Month], [Year], [CauseOfLoss], [AmountOfLoss]) values (@Key, @QuoteKey, @Month, @Year, @CauseOfLoss, @AmountOfLoss)";
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
                        Log.Error($"{nameof(QuoteHomeownerLossRepository)}: Error while attempting to insert QuoteHomeownerLoss.", ex, this);
                        transaction.Rollback();
                        return 0;
                    }
                }
            }
        }
    }
}