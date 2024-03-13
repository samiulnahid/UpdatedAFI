using AFI.Feature.Data.Providers;
using Dapper;
using AFI.Feature.Data.DataModels;
using Sitecore.Diagnostics;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AFI.Feature.Data.Repositories
{
	public interface IQuoteMotorcycleVehicleRepository
	{
		IEnumerable<QuoteMotorcycleVehicle> GetAllForQuote(int quoteKey);
		void Create(QuoteMotorcycleVehicle entity);
		int CreateOrUpdate(IEnumerable<QuoteMotorcycleVehicle> entities);
	}

	public class QuoteMotorcycleVehicleRepository : IQuoteMotorcycleVehicleRepository
	{
		private readonly IDbConnectionProvider _dbConnectionProvider;

		public QuoteMotorcycleVehicleRepository(IDbConnectionProvider dbConnectionProvider)
		{
			_dbConnectionProvider = dbConnectionProvider;
		}

		public IEnumerable<QuoteMotorcycleVehicle> GetAllForQuote(int quoteKey)
		{
			using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
			{
				var sql = "select * from dbo.[QuoteMotorcycleVehicle] where [QuoteKey] = @quoteKey";
				return db.Query<QuoteMotorcycleVehicle>(sql, new { quoteKey });
			}
		}

		public void Create(QuoteMotorcycleVehicle entity)
		{
			Stopwatch sw = new Stopwatch();
			sw.Start();
			using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
			{
				db.Open();
				using (var transaction = db.BeginTransaction())
				{
					try
					{
						var sql = "insert into dbo.[QuoteMotorcycleVehicle] ([Key], [QuoteKey], [Year], [Make], [Model], [VehicleType], [PurchaseYear], [Value], [AccessoryCoverageAmount], [IsLiabilityOnly], [CCSize], [ComprehensiveDeductible], [CollisionDeductible]) values (@Key, @QuoteKey, @Year, @Make, @Model, @VehicleType, @PurchaseYear, @Value, @AccessoryCoverageAmount, @IsLiabilityOnly, @CCSize, @ComprehensiveDeductible, @CollisionDeductible)";
						db.Execute(sql, entity, transaction);
						transaction.Commit();
						sw.Stop();
						Sitecore.Diagnostics.Log.Info("STOPWATCH: motorcycle quote create " + sw.Elapsed, "stopwatch");
					}
					catch (System.Exception ex)
					{
						sw.Stop();
						Sitecore.Diagnostics.Log.Info("STOPWATCH: motorcycle quote create error " + sw.Elapsed, "stopwatch");
						Log.Error($"{nameof(QuoteMotorcycleVehicleRepository)}: Error while attempting to insert QuoteMotorcycleVehicle.", ex, this);
						transaction.Rollback();
					}
				}
			}
		}

		public int CreateOrUpdate(IEnumerable<QuoteMotorcycleVehicle> entities)
		{
			Stopwatch sw = new Stopwatch();
			sw.Start();
			if (!entities.Any()) { return 0; }

			int QUOTE_KEY = entities.First().QuoteKey;

			using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
			{
				db.Open();
				using (var transaction = db.BeginTransaction())
				{
					try
					{
						var sql = "DELETE FROM dbo.[QuoteMotorcycleVehicle] WHERE [QuoteKey] = @QuoteKey";
						db.Execute(sql, new { QuoteKey = QUOTE_KEY }, transaction);
					}
					catch (System.Exception ex)
					{
						Log.Error($"{nameof(QuoteMotorcycleVehicleRepository)}: Error while attempting to Delete from QuoteMotorcycleVehicle.", ex, this);
						transaction.Rollback();
						sw.Stop();
						Sitecore.Diagnostics.Log.Info("STOPWATCH: motorcycle quote create or update " + sw.Elapsed, "stopwatch");
						return 0;
					}

					try
					{
						var counter = 0;
						//var sql = "insert into dbo.[QuoteMotorcycleVehicle] ([Key], [QuoteKey], [Year], [Make], [Model], [VehicleType], [PurchaseYear], [Value], [AccessoryCoverageAmount], [IsLiabilityOnly], [CCSize], [ComprehensiveDeductible], [CollisionDeductible]) values (@Key, @QuoteKey, @Year, @Make, @Model, @VehicleType, @PurchaseYear, @Value, @AccessoryCoverageAmount, @IsLiabilityOnly, @CCSize, @ComprehensiveDeductible, @CollisionDeductible)";
						var sql = "insert into dbo.[QuoteMotorcycleVehicle] ([Key], [QuoteKey], [Year], [Make], [Model], [VehicleType], [PurchaseYear], [Value], [AccessoryCoverageAmount], [IsLiabilityOnly], [CCSize], [ComprehensiveDeductible], [CollisionDeductible],[GaragingStreet],[GaragingCity],[GaragingState],[GaragingZip]) values (@Key, @QuoteKey, @Year, @Make, @Model, @VehicleType, @PurchaseYear, @Value, @AccessoryCoverageAmount, @IsLiabilityOnly, @CCSize, @ComprehensiveDeductible, @CollisionDeductible,@GaragingStreet,@GaragingCity,@GaragingState,@GaragingZip)";
						foreach (var entity in entities)
						{
							db.Execute(sql, entity, transaction);
							counter++;
						}
						transaction.Commit();
						sw.Stop();
						Sitecore.Diagnostics.Log.Info("STOPWATCH: motorcycle quote create or update " + sw.Elapsed, "stopwatch");
						return counter;
					}
					catch (System.Exception ex)
					{
						Log.Error($"{nameof(QuoteMotorcycleVehicleRepository)}: Error while attempting to insert QuoteMotorcycleVehicle.", ex, this);
						transaction.Rollback();
						sw.Stop();
						Sitecore.Diagnostics.Log.Info("STOPWATCH: motorcycle quote create or update " + sw.Elapsed, "stopwatch");
						return 0;
					}
				}
			}
		}
	}
}