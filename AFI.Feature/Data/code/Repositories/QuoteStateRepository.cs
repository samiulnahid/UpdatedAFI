using AFI.Feature.Data.Providers;
using Dapper;
using AFI.Feature.Data.DataModels;
using Sitecore.Diagnostics;
using System.Collections.Generic;

namespace AFI.Feature.Data.Repositories
{
    public interface IQuoteStateRepository
    {
        IEnumerable<QuoteState> GetAll();
        QuoteState GetById(int id);
        int Create(QuoteState entity);
    }

    public class QuoteStateRepository : IQuoteStateRepository
    {
		private readonly IDbConnectionProvider _dbConnectionProvider;

		public QuoteStateRepository(IDbConnectionProvider dbConnectionProvider)
		{
			_dbConnectionProvider = dbConnectionProvider;
		}

		public IEnumerable<QuoteState> GetAll()
		{
			using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
			{
				var sql = "select * from dbo.[QuoteState]";
				return db.Query<QuoteState>(sql);
			}
		}
		
		public QuoteState GetById(int id)
		{
			using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
			{
				var sql = "select top 1 * from dbo.[QuoteState] where [ID] = @ID";
				return db.QueryFirstOrDefault<QuoteState>(sql, new { id });
			}
		}
		
		public int Create(QuoteState entity)
		{
			using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
			{
				db.Open();
				using (var transaction = db.BeginTransaction())
				{
					try
					{
						var sql = "insert into dbo.[QuoteState] ([QuoteStateName], [QuoteStateAbbreviation], [HomeownersQuote], [RentersQuote], [AutoQuote], [UmbrellaQuote], [FloodQuote], [WatercraftQuote], [MotorcycleQuote], [MotorhomeQuote], [VIPQuote], [Commercial], [CollectorVehicle], [EstimateQuote], [IssueRenterOnline], [AskApplicationRenters], [Moratorium]) values (@QuoteStateName, @QuoteStateAbbreviation, @HomeownersQuote, @RentersQuote, @AutoQuote, @UmbrellaQuote, @FloodQuote, @WatercraftQuote, @MotorcycleQuote, @MotorhomeQuote, @VIPQuote, @Commercial, @CollectorVehicle, @EstimateQuote, @IssueRenterOnline, @AskApplicationRenters, @Moratorium); select scope_identity();";
						var id = db.QueryFirstOrDefault<int>(sql, entity, transaction);
						transaction.Commit();
						return id;
					}
					catch (System.Exception ex)
					{
						Log.Error($"{nameof(QuoteStateRepository)}: Error while attempting to insert QuoteState.", ex, this);
						transaction.Rollback();
						return 0;
					}
				}
			}
		}
	}
}