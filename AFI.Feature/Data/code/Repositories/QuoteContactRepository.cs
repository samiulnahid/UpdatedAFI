using AFI.Feature.Data.Providers;
using Dapper;
using AFI.Feature.Data.DataModels;
using Sitecore.Diagnostics;
using System.Collections.Generic;
using System.Diagnostics;
using Sitecore.Shell.Applications.ContentEditor;

namespace AFI.Feature.Data.Repositories
{
    public interface IQuoteContactRepository
    {
        IEnumerable<QuoteContact> GetAll();
        QuoteContact GetByKey(int key);
        int? Create(QuoteContact entity);
        int CreateOrUpdate(QuoteContact entity);
    }

    public class QuoteContactRepository : IQuoteContactRepository
    {
		private readonly IDbConnectionProvider _dbConnectionProvider;

		public QuoteContactRepository(IDbConnectionProvider dbConnectionProvider)
		{
			_dbConnectionProvider = dbConnectionProvider;
		}

		public IEnumerable<QuoteContact> GetAll()
		{
			using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
			{
				var sql = "select * from dbo.[QuoteContact]";
				return db.Query<QuoteContact>(sql);
			}
		}
		
		public QuoteContact GetByKey(int key)
		{
			using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
			{
				var sql = "select top 1 * from dbo.[QuoteContact] where [Key] = @Key";
				return db.QueryFirstOrDefault<QuoteContact>(sql, new { key });
			}
		}
		
		public int? Create(QuoteContact entity)
		{
			using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
			{
				db.Open();
				using (var transaction = db.BeginTransaction())
				{
					try
					{
						var sql = "insert into dbo.[QuoteContact] ([Key], [FirstName], [MiddleInitial], [LastName], [Street], [City], [State], [ZipCode], [Email], [PhoneNumber], [PhoneType], [HowToContact], [BirthDate], [Gender], [SSN], [SpouseOfMilitary], [AFIMember], [AFIMemberLength], [ServiceBranch], [ServiceRank], [ServiceStatus], [ServiceDischargeType], [CommissioningProgram], [EmploymentStatus], [MaritalStatus], [AFIExistingPolicyType], [ResidenceStatus], [WantToReceiveInfo], [HowDidYouHearAboutUs], [CNTCGroupID], [Suffix], [Prefix], [InsuredParent], [PropertyStreet], [PropertyCity], [PropertyState], [PropertyZipCode], [ServiceSpouseFirstName], [ServiceSpouseLastName], [SpouseBirthDate], [SpouseSSN], [SpouseFirstName], [SpouseLastName], [SpouseGender], [SpouseSuffix], [CallForReview], [ReviewPhoneNum], [CNTCLegacyNum], [CNTCLegacySuffix], [FirstCommandAdvisorName], [UnderMoratorium], [ReviewEmail]) values (@Key, @FirstName, @MiddleInitial, @LastName, @Street, @City, @State, @ZipCode, @Email, @PhoneNumber, @PhoneType, @HowToContact, @BirthDate, @Gender, @SSN, @SpouseOfMilitary, @AFIMember, @AFIMemberLength, @ServiceBranch, @ServiceRank, @ServiceStatus, @ServiceDischargeType, @CommissioningProgram, @EmploymentStatus, @MaritalStatus, @AFIExistingPolicyType, @ResidenceStatus, @WantToReceiveInfo, @HowDidYouHearAboutUs, @CNTCGroupID, @Suffix, @Prefix, @InsuredParent, @PropertyStreet, @PropertyCity, @PropertyState, @PropertyZipCode, @ServiceSpouseFirstName, @ServiceSpouseLastName, @SpouseBirthDate, @SpouseSSN, @SpouseFirstName, @SpouseLastName, @SpouseGender, @SpouseSuffix, @CallForReview, @ReviewPhoneNum, @CNTCLegacyNum, @CNTCLegacySuffix, @FirstCommandAdvisorName, @UnderMoratorium, @ReviewEmail); select scope_identity();";
						var id = db.QueryFirstOrDefault<int?>(sql, entity, transaction);


                        var sqlQ = "UPDATE [dbo].[AFI_Marketing_Suspect_Temp] SET [FirstName]=@FirstName, [LastName]=@LastName, [Email]=@Email, [Phone]=@Phone, [Address]=@Address, [City]=@City, [State]=@State, [ZipCode]=@ZipCode, [Country]=@Country, [DateOfBirth]=@DateOfBirth, [LastUpdated]=GETDATE() where [EntityID]="+entity.Key;
                        var newId = db.Execute(sqlQ, new
                        {
                            FirstName = entity.FirstName,
                            LastName = entity.LastName,
                            Email = entity.Email,
                            Phone = entity.PhoneNumber,
                            Address = entity.Street,
                            City = entity.City,
                            State = entity.State,
                            ZipCode = entity.ZipCode,
                            DateOfBirth = entity.BirthDate
                        }, transaction); 


                        transaction.Commit();
						return id;
					}
					catch (System.Exception ex)
					{
						Log.Error($"{nameof(QuoteContactRepository)}: Error while attempting to insert QuoteContact.", ex, this);
						transaction.Rollback();
						return 0;
					}
				}
			}
		}

        public int CreateOrUpdate(QuoteContact entity)
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
                        var sql = @"MERGE [dbo].[QuoteContact] as target
                                    USING (SELECT @Key) as source ([Key])
                                    ON (target.[Key] = source.[Key])
                                    WHEN MATCHED THEN
                                    UPDATE 
                                    SET [Key] = @Key,[FirstName] = @FirstName,[MiddleInitial] = @MiddleInitial,[LastName] = @LastName,[Street] = @Street,[City] = @City,[State] = @State,[ZipCode] = @ZipCode,[Email] = @Email,[PhoneNumber] = @PhoneNumber,[PhoneType] = @PhoneType,[HowToContact] = @HowToContact,[BirthDate] = @BirthDate,[Gender] = @Gender,[SSN] = @SSN,[SpouseOfMilitary] = @SpouseOfMilitary,[AFIMember] = @AFIMember,[AFIMemberLength] = @AFIMemberLength,[ServiceBranch] = @ServiceBranch,[ServiceRank] = @ServiceRank,[ServiceStatus] = @ServiceStatus,[ServiceDischargeType] = @ServiceDischargeType,[CommissioningProgram] = @CommissioningProgram,[EmploymentStatus] = @EmploymentStatus,[MaritalStatus] = @MaritalStatus,[AFIExistingPolicyType] = @AFIExistingPolicyType,[ResidenceStatus] = @ResidenceStatus,[WantToReceiveInfo] = @WantToReceiveInfo,[HowDidYouHearAboutUs] = @HowDidYouHearAboutUs,[CNTCGroupID] = @CNTCGroupID,[Suffix] = @Suffix,[Prefix] = @Prefix,[InsuredParent] = @InsuredParent,[PropertyStreet] = @PropertyStreet,[PropertyCity] = @PropertyCity,[PropertyState] = @PropertyState,[PropertyZipCode] = @PropertyZipCode,[ServiceSpouseFirstName] = @ServiceSpouseFirstName,[ServiceSpouseLastName] = @ServiceSpouseLastName,[SpouseBirthDate] = @SpouseBirthDate,[SpouseSSN] = @SpouseSSN,[SpouseFirstName] = @SpouseFirstName,[SpouseLastName] = @SpouseLastName,[SpouseGender] = @SpouseGender,[SpouseSuffix] = @SpouseSuffix,[CallForReview] = @CallForReview,[ReviewPhoneNum] = @ReviewPhoneNum,[CNTCLegacyNum] = @CNTCLegacyNum,[CNTCLegacySuffix] = @CNTCLegacySuffix,[FirstCommandAdvisorName] = @FirstCommandAdvisorName,[UnderMoratorium] = @UnderMoratorium,[ReviewEmail] = @ReviewEmail
                                    WHEN NOT MATCHED THEN
                                    INSERT ([Key], [FirstName], [MiddleInitial], [LastName], [Street], [City], [State], [ZipCode], [Email], [PhoneNumber], [PhoneType], [HowToContact], [BirthDate], [Gender], [SSN], [SpouseOfMilitary], [AFIMember], [AFIMemberLength], [ServiceBranch], [ServiceRank], [ServiceStatus], [ServiceDischargeType], [CommissioningProgram], [EmploymentStatus], [MaritalStatus], [AFIExistingPolicyType], [ResidenceStatus], [WantToReceiveInfo], [HowDidYouHearAboutUs], [CNTCGroupID], [Suffix], [Prefix], [InsuredParent], [PropertyStreet], [PropertyCity], [PropertyState], [PropertyZipCode], [ServiceSpouseFirstName], [ServiceSpouseLastName], [SpouseBirthDate], [SpouseSSN], [SpouseFirstName], [SpouseLastName], [SpouseGender], [SpouseSuffix], [CallForReview], [ReviewPhoneNum], [CNTCLegacyNum], [CNTCLegacySuffix], [FirstCommandAdvisorName], [UnderMoratorium], [ReviewEmail])
                                    VALUES (@Key, @FirstName, @MiddleInitial, @LastName, @Street, @City, @State, @ZipCode, @Email, @PhoneNumber, @PhoneType, @HowToContact, @BirthDate, @Gender, @SSN, @SpouseOfMilitary, @AFIMember, @AFIMemberLength, @ServiceBranch, @ServiceRank, @ServiceStatus, @ServiceDischargeType, @CommissioningProgram, @EmploymentStatus, @MaritalStatus, @AFIExistingPolicyType, @ResidenceStatus, @WantToReceiveInfo, @HowDidYouHearAboutUs, @CNTCGroupID, @Suffix, @Prefix, @InsuredParent, @PropertyStreet, @PropertyCity, @PropertyState, @PropertyZipCode, @ServiceSpouseFirstName, @ServiceSpouseLastName, @SpouseBirthDate, @SpouseSSN, @SpouseFirstName, @SpouseLastName, @SpouseGender, @SpouseSuffix, @CallForReview, @ReviewPhoneNum, @CNTCLegacyNum, @CNTCLegacySuffix, @FirstCommandAdvisorName, @UnderMoratorium, @ReviewEmail);";

                        var count = db.Execute(sql, entity, transaction);


                        int entityId = entity.Key;
                        var sqlDelete = "DELETE FROM [AFI_Marketing_Suspect_Temp] WHERE Email = @Email AND EntityID <> @EntityID";
                        db.Execute(sqlDelete, new { Email= entity.Email, EntityID = entityId }, transaction);

                        var sqlQ = "UPDATE [dbo].[AFI_Marketing_Suspect_Temp] SET [FirstName]=@FirstName, [LastName]=@LastName, [Email]=@Email, [Phone]=@Phone, [Address]=@Address, [City]=@City, [State]=@State, [ZipCode]=@ZipCode, [DateOfBirth]=@DateOfBirth, [Occupation]=@Occupation, [LastUpdated]=GETDATE(), [IsValid]=@IsValid where [EntityID]=" + entity.Key;
                        var newId = db.Execute(sqlQ, new
                        {
                            FirstName = entity.FirstName,
                            LastName = entity.LastName,
                            Email = entity.Email,
                            Phone = entity.PhoneNumber,
                            Address = entity.Street,
                            City = entity.City,
                            State = entity.State,
                            ZipCode = entity.ZipCode,
                            DateOfBirth = entity.BirthDate,
                            Occupation= entity.ServiceRank,
                            IsValid=true
                        }, transaction);


                        transaction.Commit();
						sw.Stop();
                        Sitecore.Diagnostics.Log.Info("STOPWATCH: quote save" + sw.Elapsed, "stopwatch");
						return count;

                    }
                    catch (System.Exception ex)
                    {
                        Log.Error($"{nameof(QuoteContactRepository)}: Error while attempting to update QuoteContact.", ex, this);
                        transaction.Rollback();
                        sw.Stop();
                        Sitecore.Diagnostics.Log.Info("STOPWATCH: quote save" + sw.Elapsed, "stopwatch");
						return 0;
                    }
                }
            }
        }
    }
}