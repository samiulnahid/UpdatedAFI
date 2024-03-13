using AFI.Feature.Data.Providers;
using Dapper;
using AFI.Feature.Data.DataModels;
using Sitecore.Diagnostics;
using System.Collections.Generic;

namespace AFI.Feature.Data.Repositories
{
    public interface IQuoteCommercialRepository
    {
        IEnumerable<QuoteCommercial> GetAll();
        QuoteCommercial GetByKey(int key);
        int? Create(QuoteCommercial entity);
        int CreateOrUpdate(QuoteCommercial entity);
    }

    public class QuoteCommercialRepository : IQuoteCommercialRepository
    {
		private readonly IDbConnectionProvider _dbConnectionProvider;

		public QuoteCommercialRepository(IDbConnectionProvider dbConnectionProvider)
		{
			_dbConnectionProvider = dbConnectionProvider;
		}

		public IEnumerable<QuoteCommercial> GetAll()
		{
			using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
			{
				var sql = "select * from dbo.[QuoteCommercial]";
				return db.Query<QuoteCommercial>(sql);
			}
		}
		
		public QuoteCommercial GetByKey(int key)
		{
			using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
			{
				var sql = "select top 1 * from dbo.[QuoteCommercial] where [Key] = @Key";
				return db.QueryFirstOrDefault<QuoteCommercial>(sql, new { key });
			}
		}
		
		public int? Create(QuoteCommercial entity)
		{
			using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
			{
				db.Open();
				using (var transaction = db.BeginTransaction())
				{
					try
					{
						var sql = "insert into dbo.[QuoteCommercial] ([Key], [FirstName], [LastName], [PhoneNumber], [PhoneNumberExt], [EmailAddress], [BusinessName], [BusinessType], [BusinessAddress], [BusinessCity], [BusinessState], [BusinessZip], [InsuranceTypeWanted], [BestTimeToCall], [Comments], [BusinessWebsiteUrl], [BusinessTaxID], [CurrentInsuranceCompany], [InsuranceCompany], [PolicyRenewalDate], [PolicyRenewalType]) values (@Key, @FirstName, @LastName, @PhoneNumber, @PhoneNumberExt, @EmailAddress, @BusinessName, @BusinessType, @BusinessAddress, @BusinessCity, @BusinessState, @BusinessZip, @InsuranceTypeWanted, @BestTimeToCall, @Comments, @BusinessWebsiteUrl, @BusinessTaxID, @CurrentInsuranceCompany, @InsuranceCompany, @PolicyRenewalDate, @PolicyRenewalType); select scope_identity();";
						var id = db.QueryFirstOrDefault<int?>(sql, entity, transaction);

                        var sqlQ = "UPDATE [dbo].[AFI_Marketing_Suspect_Temp] SET [FirstName]=@FirstName, [LastName]=@LastName, [Email]=@Email, [Phone]=@Phone, [Address]=@Address, [City]=@City, [State]=@State, [ZipCode]=@ZipCode, [LastUpdated]=GETDATE() where [EntityID]=" + entity.Key;
                        var newId = db.Execute(sqlQ, new
                        {
                            FirstName = entity.FirstName,
                            LastName = entity.LastName,
                            Email = entity.EmailAddress,
                            Phone = entity.PhoneNumber,
                            Address = entity.BusinessAddress,
                            City = entity.BusinessCity,
                            State = entity.BusinessState,
                            ZipCode = entity.BusinessZip
                        }, transaction);


                        transaction.Commit();
						return id;
					}
					catch (System.Exception ex)
					{
						Log.Error($"{nameof(QuoteCommercialRepository)}: Error while attempting to insert QuoteCommercial.", ex, this);
						transaction.Rollback();
						return 0;
					}
				}
			}
		}

        public int CreateOrUpdate(QuoteCommercial entity)
        {
            using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
            {
                db.Open();
                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                        var sql = @"MERGE [dbo].[QuoteCommercial] as target
                                    USING (SELECT @Key) as source ([Key])
                                    ON (target.[Key] = source.[Key])
                                    WHEN MATCHED THEN
                                    UPDATE 
                                    SET [Key] = @Key,[FirstName] = @FirstName,[LastName] = @LastName,[PhoneNumber] = @PhoneNumber,[PhoneNumberExt] = @PhoneNumberExt,[EmailAddress] = @EmailAddress,[BusinessName] = @BusinessName,[BusinessType] = @BusinessType,[BusinessAddress] = @BusinessAddress,[BusinessCity] = @BusinessCity,[BusinessState] = @BusinessState,[BusinessZip] = @BusinessZip,[InsuranceTypeWanted] = @InsuranceTypeWanted,[BestTimeToCall] = @BestTimeToCall,[Comments] = @Comments,[BusinessWebsiteUrl] = @BusinessWebsiteUrl,[BusinessTaxID] = @BusinessTaxID,[CurrentInsuranceCompany] = @CurrentInsuranceCompany,[InsuranceCompany] = @InsuranceCompany,[PolicyRenewalDate] = @PolicyRenewalDate,[PolicyRenewalType] = @PolicyRenewalType
                                    WHEN NOT MATCHED THEN
                                    INSERT ([Key], [FirstName], [LastName], [PhoneNumber], [PhoneNumberExt], [EmailAddress], [BusinessName], [BusinessType], [BusinessAddress], [BusinessCity], [BusinessState], [BusinessZip], [InsuranceTypeWanted], [BestTimeToCall], [Comments], [BusinessWebsiteUrl], [BusinessTaxID], [CurrentInsuranceCompany], [InsuranceCompany], [PolicyRenewalDate], [PolicyRenewalType]) 
                                    VALUES (@Key, @FirstName, @LastName, @PhoneNumber, @PhoneNumberExt, @EmailAddress, @BusinessName, @BusinessType, @BusinessAddress, @BusinessCity, @BusinessState, @BusinessZip, @InsuranceTypeWanted, @BestTimeToCall, @Comments, @BusinessWebsiteUrl, @BusinessTaxID, @CurrentInsuranceCompany, @InsuranceCompany, @PolicyRenewalDate, @PolicyRenewalType);";

                        var count = db.Execute(sql, entity, transaction);

                        var sqlQ = "UPDATE [dbo].[AFI_Marketing_Suspect_Temp] SET [FirstName]=@FirstName, [LastName]=@LastName, [Email]=@Email, [Phone]=@Phone, [Address]=@Address, [City]=@City, [State]=@State, [ZipCode]=@ZipCode, [LastUpdated]=GETDATE() where [EntityID]=" + entity.Key;
                        var newId = db.Execute(sqlQ, new
                        {
                            FirstName = entity.FirstName,
                            LastName = entity.LastName,
                            Email = entity.EmailAddress,
                            Phone = entity.PhoneNumber,
                            Address = entity.BusinessAddress,
                            City = entity.BusinessCity,
                            State = entity.BusinessState,
                            ZipCode = entity.BusinessZip
                        }, transaction);



                        transaction.Commit();
                        return count;

                    }
                    catch (System.Exception ex)
                    {
                        Log.Error($"{nameof(QuoteCommercialRepository)}: Error while attempting to update QuoteCommercial.", ex, this);
                        transaction.Rollback();
                        return 0;
                    }
                }
            }
        }
    }
}