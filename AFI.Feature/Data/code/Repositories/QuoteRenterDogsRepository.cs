using AFI.Feature.Data.Providers;
using Dapper;
using AFI.Feature.Data.DataModels;
using Sitecore.Diagnostics;
using System.Collections.Generic;
using System.Linq;

namespace AFI.Feature.Data.Repositories
{
    public interface IQuoteRenterDogsRepository
    {
        IEnumerable<QuoteRenterDogs> GetAll();
        QuoteRenterDogs GetById(int id);
        int Create(QuoteRenterDogs entity);
        int CreateOrUpdate(IEnumerable<QuoteRenterDogs> entities);
    }

    public class QuoteRenterDogsRepository : IQuoteRenterDogsRepository
    {
		private readonly IDbConnectionProvider _dbConnectionProvider;

		public QuoteRenterDogsRepository(IDbConnectionProvider dbConnectionProvider)
		{
			_dbConnectionProvider = dbConnectionProvider;
		}

		public IEnumerable<QuoteRenterDogs> GetAll()
		{
			using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
			{
				var sql = "select * from dbo.[QuoteRenterDogs]";
				return db.Query<QuoteRenterDogs>(sql);
			}
		}
		
		public QuoteRenterDogs GetById(int id)
		{
			using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
			{
				var sql = "select top 1 * from dbo.[QuoteRenterDogs] where [ID] = @ID";
				return db.QueryFirstOrDefault<QuoteRenterDogs>(sql, new { id });
			}
		}
		
		public int Create(QuoteRenterDogs entity)
		{
			using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
			{
				db.Open();
				using (var transaction = db.BeginTransaction())
				{
					try
					{
						var sql = "insert into dbo.[QuoteRenterDogs] ([Key], [DogMixedOrPurebred], [DogBreed], [DogBreedDesc], [DogBreed2], [DogBreed2Desc], [Age], [Gender], [YearsOwned], [WeightinPounds], [SpayedNeutered], [Confined], [HowConfined], [ShotsCurrent], [IsLicensed], [IsTrained], [Precautions]) values (@Key, @DogMixedOrPurebred, @DogBreed, @DogBreedDesc, @DogBreed2, @DogBreed2Desc, @Age, @Gender, @YearsOwned, @WeightinPounds, @SpayedNeutered, @Confined, @HowConfined, @ShotsCurrent, @IsLicensed, @IsTrained, @Precautions); select scope_identity();";
						var id = db.QueryFirstOrDefault<int>(sql, entity, transaction);
						transaction.Commit();
						return id;
					}
					catch (System.Exception ex)
					{
						Log.Error($"{nameof(QuoteRenterDogsRepository)}: Error while attempting to insert QuoteRenterDogs.", ex, this);
						transaction.Rollback();
						return 0;
					}
				}
			}
		}

        public int CreateOrUpdate(IEnumerable<QuoteRenterDogs> entities)
        {
            if (!entities.Any()) { return 0; }

            int QUOTE_KEY = entities.First().Key;

            using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
            {
                db.Open();
                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                        var sql = "DELETE FROM dbo.[QuoteRenterDogs] WHERE [Key] = @Key";
                        db.Execute(sql, new { QuoteKey = QUOTE_KEY }, transaction);
                    }
                    catch (System.Exception ex)
                    {
                        Log.Error($"{nameof(QuoteRenterDogsRepository)}: Error while attempting to Delete from QuoteRenterDogs.", ex, this);
                        transaction.Rollback();
                        return 0;
                    }

                    try
                    {
                        var counter = 0;
                        var sql = "insert into dbo.[QuoteRenterDogs] ([Key], [DogMixedOrPurebred], [DogBreed], [DogBreedDesc], [DogBreed2], [DogBreed2Desc], [Age], [Gender], [YearsOwned], [WeightinPounds], [SpayedNeutered], [Confined], [HowConfined], [ShotsCurrent], [IsLicensed], [IsTrained], [Precautions]) values (@Key, @DogMixedOrPurebred, @DogBreed, @DogBreedDesc, @DogBreed2, @DogBreed2Desc, @Age, @Gender, @YearsOwned, @WeightinPounds, @SpayedNeutered, @Confined, @HowConfined, @ShotsCurrent, @IsLicensed, @IsTrained, @Precautions);";
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
                        Log.Error($"{nameof(QuoteRenterDogsRepository)}: Error while attempting to insert QuoteRenterDogs.", ex, this);
                        transaction.Rollback();
                        return 0;
                    }
                }
            }
        }
    }
}