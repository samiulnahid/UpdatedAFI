using AFI.Feature.Data.Providers;
using Dapper;
using AFI.Feature.Data.DataModels;
using Sitecore.Diagnostics;
using System.Collections.Generic;
using System.Linq;

namespace AFI.Feature.Data.Repositories
{
    public interface IQuoteAutoDriverRepository
    {
        IEnumerable<QuoteAutoDriver> GetAllForQuote(int quoteKey);
        void Create(QuoteAutoDriver entity);
        int CreateOrUpdate(IEnumerable<QuoteAutoDriver> entities);
    }

    public class QuoteAutoDriverRepository : IQuoteAutoDriverRepository
    {
		private readonly IDbConnectionProvider _dbConnectionProvider;

		public QuoteAutoDriverRepository(IDbConnectionProvider dbConnectionProvider)
		{
			_dbConnectionProvider = dbConnectionProvider;
		}

		public IEnumerable<QuoteAutoDriver> GetAllForQuote(int quoteKey)
		{
			using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
			{
				var sql = "select * from dbo.[QuoteAutoDriver] where [QuoteKey] = @quoteKey";
				return db.Query<QuoteAutoDriver>(sql, new { quoteKey });
			}
		}

		public void Create(QuoteAutoDriver entity)
		{
			using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
			{
				db.Open();
				using (var transaction = db.BeginTransaction())
				{
					try
					{
						var sql = "insert into dbo.[QuoteAutoDriver] ([Key], [QuoteKey], [FirstName], [LastName], [MaritalStatus], [BirthDate], [Gender], [AgeLicensed], [SSN], [SafetyCourse], [HouseholdMovingViolation], [ExperienceYears], [GoodStudentDiscount], [Occupation], [Education]) values (@Key, @QuoteKey, @FirstName, @LastName, @MaritalStatus, @BirthDate, @Gender, @AgeLicensed, @SSN, @SafetyCourse, @HouseholdMovingViolation, @ExperienceYears, @GoodStudentDiscount, @Occupation, @Education)";
						db.Execute(sql, entity, transaction);
						transaction.Commit();
					}
					catch (System.Exception ex)
					{
						Log.Error($"{nameof(QuoteAutoDriverRepository)}: Error while attempting to insert QuoteAutoDriver.", ex, this);
						transaction.Rollback();
					}
				}
			}
		}

		public int CreateOrUpdate(IEnumerable<QuoteAutoDriver> entities)
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
						var sql = "DELETE FROM dbo.[QuoteAutoDriver] WHERE [QuoteKey] = @QuoteKey";
						db.Execute(sql, new { QuoteKey = QUOTE_KEY }, transaction);
					}
					catch (System.Exception ex)
					{
						Log.Error($"{nameof(QuoteAutoDriverRepository)}: Error while attempting to Delete from QuoteAutoDriver.", ex, this);
						transaction.Rollback();
						return 0;
					}

					try
					{
						var counter = 0;
						var sql = "insert into dbo.[QuoteAutoDriver] ([Key], [QuoteKey], [FirstName], [LastName], [MaritalStatus], [BirthDate], [Gender], [AgeLicensed], [SSN], [SafetyCourse], [HouseholdMovingViolation], [ExperienceYears], [GoodStudentDiscount], [Occupation], [Education]) values (@Key, @QuoteKey, @FirstName, @LastName, @MaritalStatus, @BirthDate, @Gender, @AgeLicensed, @SSN, @SafetyCourse, @HouseholdMovingViolation, @ExperienceYears, @GoodStudentDiscount, @Occupation, @Education)";
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
						Log.Error($"{nameof(QuoteAutoDriverRepository)}: Error while attempting to insert QuoteAutoDriver.", ex, this);
						transaction.Rollback();
						return 0;
					}
				}
			}
		}
	}
}