using AFI.Feature.Data.Providers;
using Dapper;
using AFI.Feature.Data.DataModels;
using Sitecore.Diagnostics;
using System.Collections.Generic;

namespace AFI.Feature.Data.Repositories
{
	public interface IReferralRepository
	{
		void InsertReferral(Referral referral);
		IEnumerable<Referral> GetAll();
	}

	public class ReferralRepository : IReferralRepository
	{
		private readonly IDbConnectionProvider _dbConnectionProvider;

		public ReferralRepository(IDbConnectionProvider dbConnectionProvider)
		{
			_dbConnectionProvider = dbConnectionProvider;
		}

		public IEnumerable<Referral> GetAll()
		{
			using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
			{
				var sql = "select * from dbo.Referral";
				return db.Query<Referral>(sql);
			}
		}

		public void InsertReferral(Referral referral)
		{
			using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
			{
				db.Open();
				using (var transaction = db.BeginTransaction())
				{
					try
					{
						var sql = "insert into dbo.Referral (ReferringAFIMemberNumber, AFIMemberNumber, FirstName, LastName, RelationshipToMember, Email, Street, City, State, PhoneNumber, CreatedDate, NamePrefix, CSRComments) values(@ReferringAFIMemberNumber, @AFIMemberNumber, @FirstName, @LastName, @RelationshipToMember, @Email, @Street, @City, @State, @PhoneNumber, @CreatedDate, @NamePrefix, @CSRComments)";
						db.Execute(sql, referral, transaction);
						transaction.Commit();
					}
					catch (System.Exception ex)
					{
						Log.Error($"{nameof(ReferralRepository)}: Error while attempting to insert referral.", ex, this);
						transaction.Rollback();
					}
				}
			}
		}
	}
}
