using AFI.Feature.Data.Providers;
using Dapper;
using AFI.Feature.Data.DataModels;
using Sitecore.Diagnostics;
using System.Collections.Generic;
using System.Linq;

namespace AFI.Feature.Data.Repositories
{
    public interface IQuoteWatercraftVehicleRepository
    {
        IEnumerable<QuoteWatercraftVehicle> GetAllForQuote(int quoteKey);
        void Create(QuoteWatercraftVehicle entity);
		int CreateOrUpdate(IEnumerable<QuoteWatercraftVehicle> entities);
    }

    public class QuoteWatercraftVehicleRepository : IQuoteWatercraftVehicleRepository
    {
		private readonly IDbConnectionProvider _dbConnectionProvider;

		public QuoteWatercraftVehicleRepository(IDbConnectionProvider dbConnectionProvider)
		{
			_dbConnectionProvider = dbConnectionProvider;
		}

		public IEnumerable<QuoteWatercraftVehicle> GetAllForQuote(int quoteKey)
		{
			using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
			{
				var sql = "select * from dbo.[QuoteWatercraftVehicle] where [QuoteKey] = @quoteKey";
				return db.Query<QuoteWatercraftVehicle>(sql, new { quoteKey });
			}
		}

		public void Create(QuoteWatercraftVehicle entity)
		{
			using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
			{
				db.Open();
				using (var transaction = db.BeginTransaction())
				{
					try
					{
						var sql = "insert into dbo.[QuoteWatercraftVehicle] ([Key], [QuoteKey], [KeptAtResidence], [GaragingZipCode], [PurchaseYear], [HullMaterial], [NumberOfMotors], [TotalHorsepower], [PropulsionType], [MaxSpeed], [Value], [Year], [Make], [Model], [IncludesTrailer], [BodyStyle], [LengthInFeet], [ComprehensiveDeductible], [CollisionDeductible], [LiabilityOnly]) values (@Key, @QuoteKey, @KeptAtResidence, @GaragingZipCode, @PurchaseYear, @HullMaterial, @NumberOfMotors, @TotalHorsepower, @PropulsionType, @MaxSpeed, @Value, @Year, @Make, @Model, @IncludesTrailer, @BodyStyle, @LengthInFeet, @ComprehensiveDeductible, @CollisionDeductible, @LiabilityOnly)";
						db.Execute(sql, entity, transaction);
						transaction.Commit();
					}
					catch (System.Exception ex)
					{
						Log.Error($"{nameof(QuoteWatercraftVehicleRepository)}: Error while attempting to insert QuoteWatercraftVehicle.", ex, this);
						transaction.Rollback();
					}
				}
			}
		}

		public int CreateOrUpdate(IEnumerable<QuoteWatercraftVehicle> entities)
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
						var sql = "DELETE FROM dbo.[QuoteWatercraftVehicle] WHERE [QuoteKey] = @QuoteKey";
						db.Execute(sql, new { QuoteKey = QUOTE_KEY }, transaction);
					}
					catch (System.Exception ex)
					{
						Log.Error($"{nameof(QuoteWatercraftVehicleRepository)}: Error while attempting to Delete from QuoteWatercraftVehicle.", ex, this);
						transaction.Rollback();
						return 0;
					}

					try
					{
						var counter = 0;
						//var sql = "insert into dbo.[QuoteWatercraftVehicle] ([Key], [QuoteKey], [KeptAtResidence], [GaragingZipCode], [PurchaseYear], [HullMaterial], [NumberOfMotors], [TotalHorsepower], [PropulsionType], [MaxSpeed], [Value], [Year], [Make], [Model], [IncludesTrailer], [BodyStyle], [LengthInFeet], [ComprehensiveDeductible], [CollisionDeductible], [LiabilityOnly], [TrailerValue]) values (@Key, @QuoteKey, @KeptAtResidence, @GaragingZipCode, @PurchaseYear, @HullMaterial, @NumberOfMotors, @TotalHorsepower, @PropulsionType, @MaxSpeed, @Value, @Year, @Make, @Model, @IncludesTrailer, @BodyStyle, @LengthInFeet, @ComprehensiveDeductible, @CollisionDeductible, @LiabilityOnly, @TrailerValue)";
						var sql = "insert into dbo.[QuoteWatercraftVehicle] ([Key], [QuoteKey], [KeptAtResidence], [GaragingZipCode], [PurchaseYear], [HullMaterial], [NumberOfMotors], [TotalHorsepower], [PropulsionType], [MaxSpeed], [Value], [Year], [Make], [Model], [IncludesTrailer], [BodyStyle], [LengthInFeet], [ComprehensiveDeductible], [CollisionDeductible], [LiabilityOnly], [TrailerValue], [GaragingStreet], [GaragingCity], [GaragingState]) values (@Key, @QuoteKey, @KeptAtResidence, @GaragingZipCode, @PurchaseYear, @HullMaterial, @NumberOfMotors, @TotalHorsepower, @PropulsionType, @MaxSpeed, @Value, @Year, @Make, @Model, @IncludesTrailer, @BodyStyle, @LengthInFeet, @ComprehensiveDeductible, @CollisionDeductible, @LiabilityOnly, @TrailerValue, @GaragingStreet, @GaragingCity, @GaragingState)";
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
						Log.Error($"{nameof(QuoteWatercraftVehicleRepository)}: Error while attempting to insert QuoteWatercraftVehicle.", ex, this);
						transaction.Rollback();
						return 0;
					}
				}
			}
		}
	}
}