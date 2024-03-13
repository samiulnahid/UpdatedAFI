using AFI.Feature.Data.Providers;
using Dapper;
using AFI.Feature.Data.DataModels;
using Sitecore.Diagnostics;
using System.Collections.Generic;

namespace AFI.Feature.Data.Repositories
{
    public interface IQuoteVIPRepository
    {
        IEnumerable<QuoteVIP> GetAllForQuote(int quoteKey);
        void Create(QuoteVIP entity);
    }

    public class QuoteVIPRepository : IQuoteVIPRepository
    {
		private readonly IDbConnectionProvider _dbConnectionProvider;

		public QuoteVIPRepository(IDbConnectionProvider dbConnectionProvider)
		{
			_dbConnectionProvider = dbConnectionProvider;
		}

		public IEnumerable<QuoteVIP> GetAllForQuote(int quoteKey)
		{
			using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
			{
				var sql = "select * from dbo.[QuoteVIP] where [QuoteKey] = @quoteKey";
				return db.Query<QuoteVIP>(sql, new { quoteKey });
			}
		}

		public void Create(QuoteVIP entity)
		{
			using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
			{
				db.Open();
				using (var transaction = db.BeginTransaction())
				{
					try
					{
						var sql = "insert into dbo.[QuoteVIP] ([Key], [QuoteKey], [TotalCoverageCost], [InterestLevel], [Changeset], [CreateDate]) values (@Key, @QuoteKey, @TotalCoverageCost, @InterestLevel, @Changeset, @CreateDate)";
						db.Execute(sql, entity, transaction);
						transaction.Commit();
					}
					catch (System.Exception ex)
					{
						Log.Error($"{nameof(QuoteVIPRepository)}: Error while attempting to insert QuoteVIP.", ex, this);
						transaction.Rollback();
					}
				}
			}
		}
	}
}