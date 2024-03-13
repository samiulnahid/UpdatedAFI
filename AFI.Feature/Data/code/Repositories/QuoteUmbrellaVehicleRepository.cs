using AFI.Feature.Data.Providers;
using Dapper;
using AFI.Feature.Data.DataModels;
using Sitecore.Diagnostics;
using System.Collections.Generic;
using System.Linq;

namespace AFI.Feature.Data.Repositories
{
    public interface IQuoteUmbrellaVehicleRepository
    {
        IEnumerable<QuoteUmbrellaVehicle> GetAllForQuote(int quoteKey);
        void Create(QuoteUmbrellaVehicle entity);
        int CreateOrUpdate(IEnumerable<QuoteUmbrellaVehicle> entities);
    }

    public class QuoteUmbrellaVehicleRepository : IQuoteUmbrellaVehicleRepository
    {
		private readonly IDbConnectionProvider _dbConnectionProvider;

		public QuoteUmbrellaVehicleRepository(IDbConnectionProvider dbConnectionProvider)
		{
			_dbConnectionProvider = dbConnectionProvider;
		}

		public IEnumerable<QuoteUmbrellaVehicle> GetAllForQuote(int quoteKey)
		{
			using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
			{
				var sql = "select * from dbo.[QuoteUmbrellaVehicle] where [QuoteKey] = @quoteKey";
				return db.Query<QuoteUmbrellaVehicle>(sql, new { quoteKey });
			}
		}

		public void Create(QuoteUmbrellaVehicle entity)
		{
			using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
			{
				db.Open();
				using (var transaction = db.BeginTransaction())
				{
					try
					{
						var sql = "insert into dbo.[QuoteUmbrellaVehicle] ([Key], [QuoteKey], [VehicleType], [VehicleUnderlyingInsurance]) values (@Key, @QuoteKey, @VehicleType, @VehicleUnderlyingInsurance)";
						db.Execute(sql, entity, transaction);
						transaction.Commit();
					}
					catch (System.Exception ex)
					{
						Log.Error($"{nameof(QuoteUmbrellaVehicleRepository)}: Error while attempting to insert QuoteUmbrellaVehicle.", ex, this);
						transaction.Rollback();
					}
				}
			}
		}

        public int CreateOrUpdate(IEnumerable<QuoteUmbrellaVehicle> entities)
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
                        var sql = "DELETE FROM dbo.[QuoteUmbrellaVehicle] WHERE [QuoteKey] = @QuoteKey";
                        db.Execute(sql, new { QuoteKey = QUOTE_KEY }, transaction);
                    }
                    catch (System.Exception ex)
                    {
                        Log.Error($"{nameof(QuoteUmbrellaVehicleRepository)}: Error while attempting to Delete from QuoteUmbrellaVehicle.", ex, this);
                        transaction.Rollback();
                        return 0;
                    }

                    try
                    {
                        var counter = 0;
                        var sql = "insert into dbo.[QuoteUmbrellaVehicle] ([Key], [QuoteKey], [VehicleType], [VehicleUnderlyingInsurance]) values (@Key, @QuoteKey, @VehicleType, @VehicleUnderlyingInsurance)";
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
                        Log.Error($"{nameof(QuoteUmbrellaVehicleRepository)}: Error while attempting to insert QuoteUmbrellaVehicle.", ex, this);
                        transaction.Rollback();
                        return 0;
                    }
                }
            }
        }
    }
}