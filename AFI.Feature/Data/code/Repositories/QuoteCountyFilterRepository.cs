using AFI.Feature.Data.Providers;
using Dapper;
using AFI.Feature.Data.DataModels;
using Sitecore.Diagnostics;
using System.Collections.Generic;

namespace AFI.Feature.Data.Repositories
{
    public interface IQuoteCountyFilterRepository
    {
        IEnumerable<QuoteCountyFilter> GetAll();
        QuoteCountyFilter GetById(int id);
        int Create(QuoteCountyFilter entity);
    }

    public class QuoteCountyFilterRepository : IQuoteCountyFilterRepository
    {
		private readonly IDbConnectionProvider _dbConnectionProvider;

		public QuoteCountyFilterRepository(IDbConnectionProvider dbConnectionProvider)
		{
			_dbConnectionProvider = dbConnectionProvider;
		}

		public IEnumerable<QuoteCountyFilter> GetAll()
		{
			using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
			{
				var sql = "select * from dbo.[QuoteCountyFilter]";
				return db.Query<QuoteCountyFilter>(sql);
			}
		}
		
		public QuoteCountyFilter GetById(int id)
		{
			using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
			{
				var sql = "select top 1 * from dbo.[QuoteCountyFilter] where [ID] = @ID";
				return db.QueryFirstOrDefault<QuoteCountyFilter>(sql, new { id });
			}
		}
		
		public int Create(QuoteCountyFilter entity)
		{
			using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
			{
				db.Open();
				using (var transaction = db.BeginTransaction())
				{
					try
					{
						var sql = "insert into dbo.[QuoteCountyFilter] ([CountyName], [HomeownersQuote], [RentersQuote], [AutoQuote], [UmbrellaQuote], [FloodQuote], [WatercraftQuote], [MotorcycleQuote], [MotorhomeQuote], [VIPQuote], [Commercial], [CollectorVehicle], [EstimateQuote], [StateAbrev], [IssueRenterOnline], [AskApplicationRenters], [Moratorium]) values (@CountyName, @HomeownersQuote, @RentersQuote, @AutoQuote, @UmbrellaQuote, @FloodQuote, @WatercraftQuote, @MotorcycleQuote, @MotorhomeQuote, @VIPQuote, @Commercial, @CollectorVehicle, @EstimateQuote, @StateAbrev, @IssueRenterOnline, @AskApplicationRenters, @Moratorium); select scope_identity();";
						var id = db.QueryFirstOrDefault<int>(sql, entity, transaction);
						transaction.Commit();
						return id;
					}
					catch (System.Exception ex)
					{
						Log.Error($"{nameof(QuoteCountyFilterRepository)}: Error while attempting to insert QuoteCountyFilter.", ex, this);
						transaction.Rollback();
						return 0;
					}
				}
			}
		}
	}
}