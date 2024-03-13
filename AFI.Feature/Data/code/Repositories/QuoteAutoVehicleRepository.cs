using AFI.Feature.Data.Providers;
using Dapper;
using AFI.Feature.Data.DataModels;
using Sitecore.Diagnostics;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace AFI.Feature.Data.Repositories
{
    public interface IQuoteAutoVehicleRepository
    {
        IEnumerable<QuoteAutoVehicle> GetAllForQuote(int quoteKey);
        void Create(QuoteAutoVehicle entity);
		int CreateOrUpdate(IEnumerable<QuoteAutoVehicle> entities);
	}

    public class QuoteAutoVehicleRepository : IQuoteAutoVehicleRepository
    {
		private readonly IDbConnectionProvider _dbConnectionProvider;

		public QuoteAutoVehicleRepository(IDbConnectionProvider dbConnectionProvider)
		{
			_dbConnectionProvider = dbConnectionProvider;
		}

		public IEnumerable<QuoteAutoVehicle> GetAllForQuote(int quoteKey)
		{
			using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
			{
				var sql = "select * from dbo.[QuoteAutoVehicle] where [QuoteKey] = @quoteKey";
				return db.Query<QuoteAutoVehicle>(sql, new { quoteKey });
			}
		}

		public void Create(QuoteAutoVehicle entity)
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
						var sql = "insert into dbo.[QuoteAutoVehicle] ([Key], [QuoteKey], [VIN], [Make], [Year], [Model], [BodyStyle], [GaragingZip], [AntiTheftDevice], [LiabilityOnly], [VehicleUse], [AnnualMileage], [MilesOneWay], [GaragingStreet], [GaragingCity], [GaragingState], [ComprehensiveDeductible], [CollisionDeductible]) values (@Key, @QuoteKey, @VIN, @Make, @Year, @Model, @BodyStyle, @GaragingZip, @AntiTheftDevice, @LiabilityOnly, @VehicleUse, @AnnualMileage, @MilesOneWay, @GaragingStreet, @GaragingCity, @GaragingState, @ComprehensiveDeductible, @CollisionDeductible)";
						db.Execute(sql, entity, transaction);
						transaction.Commit();
						sw.Stop();
                        Sitecore.Diagnostics.Log.Info("STOPWATCH: auto quote create " + sw.Elapsed, "stopwatch");
					}
					catch (System.Exception ex)
					{
                        sw.Stop();
                        Sitecore.Diagnostics.Log.Info("STOPWATCH: auto quote create error" + sw.Elapsed, "stopwatch");
						Log.Error($"{nameof(QuoteAutoVehicleRepository)}: Error while attempting to insert QuoteAutoVehicle.", ex, this);
						transaction.Rollback();
					}
				}
			}
		}

		public int CreateOrUpdate(IEnumerable<QuoteAutoVehicle> entities)
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
						var sql = "DELETE FROM dbo.[QuoteAutoVehicle] WHERE [QuoteKey] = @QuoteKey";
						db.Execute(sql, new { QuoteKey = QUOTE_KEY }, transaction);
					}
					catch (System.Exception ex)
					{
						Log.Error($"{nameof(QuoteAutoVehicleRepository)}: Error while attempting to Delete from QuoteAutoVehicle.", ex, this);
						transaction.Rollback();
						sw.Stop();
                        Sitecore.Diagnostics.Log.Info("STOPWATCH: auto quote create or update " + sw.Elapsed, "stopwatch");
						return 0;
					}

					try
					{
						var counter = 0;
						var sql = "insert into dbo.[QuoteAutoVehicle] ([Key], [QuoteKey], [VIN], [Make], [Year], [Model], [BodyStyle], [GaragingZip], [AntiTheftDevice], [LiabilityOnly], [VehicleUse], [AnnualMileage], [MilesOneWay], [GaragingStreet], [GaragingCity], [GaragingState], [ComprehensiveDeductible], [CollisionDeductible]) values (@Key, @QuoteKey, @VIN, @Make, @Year, @Model, @BodyStyle, @GaragingZip, @AntiTheftDevice, @LiabilityOnly, @VehicleUse, @AnnualMileage, @MilesOneWay, @GaragingStreet, @GaragingCity, @GaragingState, @ComprehensiveDeductible, @CollisionDeductible)";
						foreach (var entity in entities)
						{
							db.Execute(sql, entity, transaction);
							counter++;
						}
						transaction.Commit();
                        sw.Stop();
                        Sitecore.Diagnostics.Log.Debug("STOPWATCH: auto quote create or update " + sw.Elapsed, "stopwatch");
						return counter;
					}
					catch (System.Exception ex)
					{
						Log.Error($"{nameof(QuoteAutoVehicleRepository)}: Error while attempting to insert QuoteAutoVehicle.", ex, this);
						transaction.Rollback();
                        sw.Stop();
                        Sitecore.Diagnostics.Log.Debug("STOPWATCH: auto quote create or update " + sw.Elapsed, "stopwatch");
						return 0;
					}
				}
			}
		}
        
	}
}