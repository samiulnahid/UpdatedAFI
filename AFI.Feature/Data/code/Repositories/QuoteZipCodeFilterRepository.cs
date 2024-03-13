using AFI.Feature.Data.Providers;
using Dapper;
using AFI.Feature.Data.DataModels;
using Sitecore.Diagnostics;
using System.Collections.Generic;

namespace AFI.Feature.Data.Repositories
{
    public interface IQuoteZipCodeFilterRepository
    {
        IEnumerable<QuoteZipCodeFilter> GetAll();
        QuoteZipCodeFilter GetById(int id);
        int Create(QuoteZipCodeFilter entity);
    }

    public class QuoteZipCodeFilterRepository : IQuoteZipCodeFilterRepository
    {
		private readonly IDbConnectionProvider _dbConnectionProvider;

		public QuoteZipCodeFilterRepository(IDbConnectionProvider dbConnectionProvider)
		{
			_dbConnectionProvider = dbConnectionProvider;
		}

		public IEnumerable<QuoteZipCodeFilter> GetAll()
		{
			using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
			{
				var sql = "select * from dbo.[QuoteZipCodeFilter]";
				return db.Query<QuoteZipCodeFilter>(sql);
			}
		}
		
		public QuoteZipCodeFilter GetById(int id)
		{
			using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
			{
				var sql = "select top 1 * from dbo.[QuoteZipCodeFilter] where [ID] = @ID";
				return db.QueryFirstOrDefault<QuoteZipCodeFilter>(sql, new { id });
			}
		}
		
		public int Create(QuoteZipCodeFilter entity)
		{
			using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
			{
				db.Open();
				using (var transaction = db.BeginTransaction())
				{
					try
					{
						var sql = "insert into dbo.[QuoteZipCodeFilter] ([QuoteZipCode], [HomeownersQuote], [RentersQuote], [AutoQuote], [UmbrellaQuote], [FloodQuote], [WatercraftQuote], [MotorcycleQuote], [MotorhomeQuote], [VIPQuote], [Commercial], [CollectorVehicle], [EstimateQuote], [IssueRenterOnline], [AskApplicationRenters], [Moratorium]) values (@QuoteZipCode, @HomeownersQuote, @RentersQuote, @AutoQuote, @UmbrellaQuote, @FloodQuote, @WatercraftQuote, @MotorcycleQuote, @MotorhomeQuote, @VIPQuote, @Commercial, @CollectorVehicle, @EstimateQuote, @IssueRenterOnline, @AskApplicationRenters, @Moratorium); select scope_identity();";
						var id = db.QueryFirstOrDefault<int>(sql, entity, transaction);
						transaction.Commit();
						return id;
					}
					catch (System.Exception ex)
					{
						Log.Error($"{nameof(QuoteZipCodeFilterRepository)}: Error while attempting to insert QuoteZipCodeFilter.", ex, this);
						transaction.Rollback();
						return 0;
					}
				}
			}
		}
	}
}