using AFI.Feature.Data.Providers;
using Dapper;
using AFI.Feature.Data.DataModels;
using Sitecore.Diagnostics;
using System.Collections.Generic;
using System.Linq;

namespace AFI.Feature.Data.Repositories
{
    public interface IQuoteAutoIncidentRepository
    {
        IEnumerable<QuoteAutoIncident> GetAllForQuote(int quoteKey);
        void Create(QuoteAutoIncident entity);
		int CreateOrUpdate(IEnumerable<QuoteAutoIncident> entities);
	}

    public class QuoteAutoIncidentRepository : IQuoteAutoIncidentRepository
    {
		private readonly IDbConnectionProvider _dbConnectionProvider;

		public QuoteAutoIncidentRepository(IDbConnectionProvider dbConnectionProvider)
		{
			_dbConnectionProvider = dbConnectionProvider;
		}

		public IEnumerable<QuoteAutoIncident> GetAllForQuote(int quoteKey)
		{
			using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
			{
				var sql = "select * from dbo.[QuoteAutoIncident] where [QuoteKey] = @quoteKey";
				return db.Query<QuoteAutoIncident>(sql, new { quoteKey });
			}
		}

		public void Create(QuoteAutoIncident entity)
		{
			using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
			{
				db.Open();
				using (var transaction = db.BeginTransaction())
				{
					try
					{
						var sql = "insert into dbo.[QuoteAutoIncident] ([Key], [QuoteKey], [DriverKey], [Incident], [Date], [DriverName]) values (@Key, @QuoteKey, @DriverKey, @Incident, @Date, @DriverName)";
						db.Execute(sql, entity, transaction);
						transaction.Commit();
					}
					catch (System.Exception ex)
					{
						Log.Error($"{nameof(QuoteAutoIncidentRepository)}: Error while attempting to insert QuoteAutoIncident.", ex, this);
						transaction.Rollback();
					}
				}
			}
		}

		public int CreateOrUpdate(IEnumerable<QuoteAutoIncident> entities)
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
						var sql = "DELETE FROM dbo.[QuoteAutoIncident] WHERE [QuoteKey] = @QuoteKey";
						db.Execute(sql, new { QuoteKey = QUOTE_KEY }, transaction);
					}
					catch (System.Exception ex)
					{
						Log.Error($"{nameof(QuoteAutoIncidentRepository)}: Error while attempting to Delete from QuoteAutoIncident.", ex, this);
						transaction.Rollback();
						return 0;
					}

					try
					{
						var counter = 0;
						var sql = "insert into dbo.[QuoteAutoIncident] ([Key], [QuoteKey], [DriverKey], [Incident], [Date], [DriverName]) values (@Key, @QuoteKey, @DriverKey, @Incident, @Date, @DriverName)";
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
						Log.Error($"{nameof(QuoteAutoIncidentRepository)}: Error while attempting to insert QuoteAutoIncident.", ex, this);
						transaction.Rollback();
						return 0;
					}
				}
			}
		}
	}
}