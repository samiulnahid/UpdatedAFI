using AFI.Feature.Data.Providers;
using Dapper;
using AFI.Feature.Data.DataModels;
using Sitecore.Diagnostics;
using System.Collections.Generic;
using System.Linq;

namespace AFI.Feature.Data.Repositories
{
    public interface IQuoteRecreationVehicleRepository
    {
        IEnumerable<QuoteRecreationVehicle> GetAllForQuote(int quoteKey);
        void Create(QuoteRecreationVehicle entity);
		int CreateOrUpdate(IEnumerable<QuoteRecreationVehicle> entities);
    }

    public class QuoteRecreationVehicleRepository : IQuoteRecreationVehicleRepository
    {
		private readonly IDbConnectionProvider _dbConnectionProvider;

		public QuoteRecreationVehicleRepository(IDbConnectionProvider dbConnectionProvider)
		{
			_dbConnectionProvider = dbConnectionProvider;
		}

		public IEnumerable<QuoteRecreationVehicle> GetAllForQuote(int quoteKey)
		{
			using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
			{
				var sql = "select * from dbo.[QuoteRecreationVehicle] where [QuoteKey] = @quoteKey";
				return db.Query<QuoteRecreationVehicle>(sql, new { quoteKey });
			}
		}

		public void Create(QuoteRecreationVehicle entity)
		{
			using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
			{
				db.Open();
				using (var transaction = db.BeginTransaction())
				{
					try
					{
						var sql = "insert into dbo.[QuoteRecreationVehicle] ([Key], [QuoteKey], [KeptAtResidence], [GaragingZipCode], [PurchaseYear], [Value], [LiabilityOnly], [VehicleType], [Year], [ValueCustomParts], [CCSize], [Make], [Model]) values (@Key, @QuoteKey, @KeptAtResidence, @GaragingZipCode, @PurchaseYear, @Value, @LiabilityOnly, @VehicleType, @Year, @ValueCustomParts, @CCSize, @Make, @Model)";
						db.Execute(sql, entity, transaction);
						transaction.Commit();
					}
					catch (System.Exception ex)
					{
						Log.Error($"{nameof(QuoteRecreationVehicleRepository)}: Error while attempting to insert QuoteRecreationVehicle.", ex, this);
						transaction.Rollback();
					}
				}
			}
		}

		public int CreateOrUpdate(IEnumerable<QuoteRecreationVehicle> entities)
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
						var sql = "DELETE FROM dbo.[QuoteRecreationVehicle] WHERE [QuoteKey] = @QuoteKey";
						db.Execute(sql, new { QuoteKey = QUOTE_KEY }, transaction);
					}
					catch (System.Exception ex)
					{
						Log.Error($"{nameof(QuoteRecreationVehicleRepository)}: Error while attempting to Delete from QuoteRecreationVehicle.", ex, this);
						transaction.Rollback();
						return 0;
					}

					try
					{
						var counter = 0;
						var sql = "insert into dbo.[QuoteRecreationVehicle] ([Key], [QuoteKey], [KeptAtResidence], [GaragingZipCode], [PurchaseYear], [Value], [LiabilityOnly], [VehicleType], [Year], [ValueCustomParts], [CCSize], [Make], [Model]) values (@Key, @QuoteKey, @KeptAtResidence, @GaragingZipCode, @PurchaseYear, @Value, @LiabilityOnly, @VehicleType, @Year, @ValueCustomParts, @CCSize, @Make, @Model)";
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
						Log.Error($"{nameof(QuoteRecreationVehicleRepository)}: Error while attempting to insert QuoteRecreationVehicle.", ex, this);
						transaction.Rollback();
						return 0;
					}
				}
			}
		}
	}
}