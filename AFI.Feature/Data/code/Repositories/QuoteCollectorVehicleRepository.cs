using AFI.Feature.Data.Providers;
using Dapper;
using AFI.Feature.Data.DataModels;
using Sitecore.Diagnostics;
using System.Collections.Generic;
using System.Linq;

namespace AFI.Feature.Data.Repositories
{
    public interface IQuoteCollectorVehicleRepository
    {
        IEnumerable<QuoteCollectorVehicle> GetAllForQuote(int quoteKey);
        void Create(QuoteCollectorVehicle entity);
		int CreateOrUpdate(IEnumerable<QuoteCollectorVehicle> entities);
    }

    public class QuoteCollectorVehicleRepository : IQuoteCollectorVehicleRepository
    {
		private readonly IDbConnectionProvider _dbConnectionProvider;

		public QuoteCollectorVehicleRepository(IDbConnectionProvider dbConnectionProvider)
		{
			_dbConnectionProvider = dbConnectionProvider;
		}

		public IEnumerable<QuoteCollectorVehicle> GetAllForQuote(int quoteKey)
		{
			using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
			{
				var sql = "select * from dbo.[QuoteCollectorVehicle] where [QuoteKey] = @quoteKey";
				return db.Query<QuoteCollectorVehicle>(sql, new { quoteKey });
			}
		}

		public void Create(QuoteCollectorVehicle entity)
		{
			using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
			{
				db.Open();
				using (var transaction = db.BeginTransaction())
				{
					try
					{
						var sql = "insert into dbo.[QuoteCollectorVehicle] ([Key], [QuoteKey], [Year], [Make], [Model], [Type], [EstimatedValue], [VehicleStorage], [DescribeHowVehicleIsDriven], [ComprehensiveDeductible], [CollisionDeductible], [LiabilityOnly]) values (@Key, @QuoteKey, @Year, @Make, @Model, @Type, @EstimatedValue, @VehicleStorage, @DescribeHowVehicleIsDriven, @ComprehensiveDeductible, @CollisionDeductible, @LiabilityOnly)";
						db.Execute(sql, entity, transaction);
						transaction.Commit();
					}
					catch (System.Exception ex)
					{
						Log.Error($"{nameof(QuoteCollectorVehicleRepository)}: Error while attempting to insert QuoteCollectorVehicle.", ex, this);
						transaction.Rollback();
					}
				}
			}
		}

		public int CreateOrUpdate(IEnumerable<QuoteCollectorVehicle> entities)
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
						var sql = "DELETE FROM dbo.[QuoteCollectorVehicle] WHERE [QuoteKey] = @QuoteKey";
						db.Execute(sql, new { QuoteKey = QUOTE_KEY }, transaction);
					}
					catch (System.Exception ex)
					{
						Log.Error($"{nameof(QuoteCollectorVehicleRepository)}: Error while attempting to Delete from QuoteCollectorVehicle.", ex, this);
						transaction.Rollback();
						return 0;
					}

					try
					{
						var counter = 0;
						var sql = "insert into dbo.[QuoteCollectorVehicle] ([Key], [QuoteKey], [Year], [Make], [Model], [Type], [EstimatedValue], [VehicleStorage], [DescribeHowVehicleIsDriven], [ComprehensiveDeductible], [CollisionDeductible], [LiabilityOnly]) values (@Key, @QuoteKey, @Year, @Make, @Model, @Type, @EstimatedValue, @VehicleStorage, @DescribeHowVehicleIsDriven, @ComprehensiveDeductible, @CollisionDeductible, @LiabilityOnly)";
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
						Log.Error($"{nameof(QuoteCollectorVehicleRepository)}: Error while attempting to insert QuoteCollectorVehicle.", ex, this);
						transaction.Rollback();
						return 0;
					}
				}
			}
		}
        
	}
}