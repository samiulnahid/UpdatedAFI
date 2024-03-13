using AFI.Feature.Data.Providers;
using Dapper;
using AFI.Feature.Data.DataModels;
using Sitecore.Diagnostics;
using System.Collections.Generic;
using System.Linq;

namespace AFI.Feature.Data.Repositories
{
    public interface IQuoteMotorhomeVehicleRepository
    {
        IEnumerable<QuoteMotorhomeVehicle> GetAllForQuote(int quoteKey);
        QuoteMotorhomeVehicle GetByKey(int quoteKey);
        void Create(QuoteMotorhomeVehicle entity);
		int CreateOrUpdate(IEnumerable<QuoteMotorhomeVehicle> entities);
    }

    public class QuoteMotorhomeVehicleRepository : IQuoteMotorhomeVehicleRepository
    {
		private readonly IDbConnectionProvider _dbConnectionProvider;

		public QuoteMotorhomeVehicleRepository(IDbConnectionProvider dbConnectionProvider)
		{
			_dbConnectionProvider = dbConnectionProvider;
		}

		public IEnumerable<QuoteMotorhomeVehicle> GetAllForQuote(int quoteKey)
		{
			using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
			{
				var sql = "select * from dbo.[QuoteMotorhomeVehicle] where [QuoteKey] = @quoteKey";
				return db.Query<QuoteMotorhomeVehicle>(sql, new { quoteKey });
			}
		}

        public QuoteMotorhomeVehicle GetByKey(int quoteKey)
        {
			using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
            {
                var sql = "select top 1 * from dbo.[QuoteMotorhomeVehicle] where [QuoteKey] = @quoteKey";
                return db.QueryFirstOrDefault<QuoteMotorhomeVehicle>(sql, new { quoteKey });
            }
		}

        public void Create(QuoteMotorhomeVehicle entity)
		{
			using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
			{
				db.Open();
				using (var transaction = db.BeginTransaction())
				{
					try
					{
						var sql = "insert into dbo.[QuoteMotorhomeVehicle] ([Key], [QuoteKey], [KeptAtResidence], [GaragingZipCode], [PurchaseYear], [Value], [Make], [VehicleType], [Year], [Model], [Usage], [ValuePersonalEffects], [Length], [NumberOfSlideOuts]) values (@Key, @QuoteKey, @KeptAtResidence, @GaragingZipCode, @PurchaseYear, @Value, @Make, @VehicleType, @Year, @Model, @Usage, @ValuePersonalEffects, @Length, @NumberOfSlideOuts)";
						db.Execute(sql, entity, transaction);
						transaction.Commit();
					}
					catch (System.Exception ex)
					{
						Log.Error($"{nameof(QuoteMotorhomeVehicleRepository)}: Error while attempting to insert QuoteMotorhomeVehicle.", ex, this);
						transaction.Rollback();
					}
				}
			}
		}

		public int CreateOrUpdate(IEnumerable<QuoteMotorhomeVehicle> entities)
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
						var sql = "DELETE FROM dbo.[QuoteMotorhomeVehicle] WHERE [QuoteKey] = @QuoteKey";
						db.Execute(sql, new { QuoteKey = QUOTE_KEY }, transaction);
					}
					catch (System.Exception ex)
					{
						Log.Error($"{nameof(QuoteMotorhomeVehicleRepository)}: Error while attempting to Delete from QuoteMotorhomeVehicle.", ex, this);
						transaction.Rollback();
						return 0;
					}

					try
					{
						var counter = 0;
						var sql = "insert into dbo.[QuoteMotorhomeVehicle] ([Key], [QuoteKey], [KeptAtResidence], [GaragingZipCode], [PurchaseYear], [Value], [Make], [VehicleType], [Year], [Model], [Usage], [ValuePersonalEffects], [Length], [NumberOfSlideOuts]) values (@Key, @QuoteKey, @KeptAtResidence, @GaragingZipCode, @PurchaseYear, @Value, @Make, @VehicleType, @Year, @Model, @Usage, @ValuePersonalEffects, @Length, @NumberOfSlideOuts)";
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
						Log.Error($"{nameof(QuoteMotorhomeVehicleRepository)}: Error while attempting to insert QuoteMotorhomeVehicle.", ex, this);
						transaction.Rollback();
						return 0;
					}
				}
			}
		}
	}
}