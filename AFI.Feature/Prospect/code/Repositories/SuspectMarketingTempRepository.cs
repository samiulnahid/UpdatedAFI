using AFI.Feature.Prospect.Providers;
using Dapper;
using AFI.Feature.Prospect.Models;
using Sitecore.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System;
using System.Data;

namespace AFI.Feature.Prospect.Repositories
{
    public interface ISuspectMarketingRepositoryTemp
    {
        IEnumerable<SuspectMarketingTemp> GetAllForMarketing();

		IEnumerable<SuspectMarketingTemp> GetAllForMarketing(int page, int pageSize, string leadStatus, string isSynced, string coverage, string coverageType);
		IEnumerable<SuspectMarketingTemp> GetAllForLeadSource(string leadSource);
		IEnumerable<SuspectMarketingTemp> GetAllForLeadStatus(string leadStatus);
		IEnumerable<SuspectMarketingTemp> GetAllForLeadOwner(string leadOwner); 

		int Create(SuspectMarketingTemp entity);
      //  int MapAndCreate(CommonFormSaveModel quote);
        int CreateOrUpdate(SuspectMarketingTemp entity);
		int SyncTempApprovedtoProspect(string ids);

		IEnumerable<SuspectMarketingTemp> GetAllForDownloadTempSuspect(string leadStatus, string isSynced, string coverage, string coverageType); 
	}

    public class SuspectMarketingRepositoryTemp : ISuspectMarketingRepositoryTemp
    {
		private readonly IDatabaseConnectionProvider _dbConnectionProvider;

		public SuspectMarketingRepositoryTemp(IDatabaseConnectionProvider dbConnectionProvider)
		{
			_dbConnectionProvider = dbConnectionProvider;
		}

		public IEnumerable<SuspectMarketingTemp> GetAllForMarketing()
		{
			using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
			{
				var sql = "select * from dbo.[AFI_Marketing_Suspect_Temp] Order by 1 desc";
				return db.Query<SuspectMarketingTemp>(sql);
			}
		}
		public IEnumerable<SuspectMarketingTemp> GetAllForLeadSource(string leadSource)
		{
			using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
			{
				var sql = "select * from dbo.[AFI_Marketing_Suspect_Temp] where [LeadSource] = @leadSource";
				return db.Query<SuspectMarketingTemp>(sql, new { leadSource });
			}
		}
		public IEnumerable<SuspectMarketingTemp> GetAllForLeadStatus(string leadStatus)
		{
			using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
			{
				var sql = "select * from dbo.[AFI_Marketing_Suspect_Temp] where [LeadStatus] = @leadStatus";
				return db.Query<SuspectMarketingTemp>(sql, new { leadStatus });
			}
		}
		public IEnumerable<SuspectMarketingTemp> GetAllForLeadOwner(string leadOwner)
		{
			using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
			{
				var sql = "select * from dbo.[AFI_Marketing_Suspect_Temp] where [LeadOwner] = @leadOwner";
				return db.Query<SuspectMarketingTemp>(sql, new { leadOwner });
			}
		}

		public int Create(SuspectMarketingTemp entity)
		{
			using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
			{
				var count = 0;
				db.Open();
				using (var transaction = db.BeginTransaction())
				{
					try
					{
						entity.DateCreated = DateTime.Now;

						var sql = "insert into dbo.[AFI_Marketing_Suspect_Temp] ( [FirstName], [LastName], [Email], [Phone], [Address], [City], [State], [ZipCode], [Country], [DateOfBirth], [Occupation], [PreferredCoverage], [LeadSource], [LeadStatus], [LeadOwner], [LeadScore], [DateCreated], [LastUpdated], [EntityType], [EntityID], [IsSynced], [IsValid], [IsBlockCountry]) values (@FirstName, @LastName, @Email, @Phone, @Address, @City, @State, @ZipCode, @Country, @DateOfBirth, @Occupation, @PreferredCoverage, @LeadSource, @LeadStatus, @LeadOwner, @LeadScore, @DateCreated, @LastUpdated, @EntityType, @EntityID, @IsSynced, @IsValid, @IsBlockCountry)";
						count = db.Execute(sql, entity, transaction);
						transaction.Commit();
						return count;
					}
					catch (System.Exception ex)
					{
						Log.Error($"{nameof(SuspectMarketingRepositoryTemp)}: Error while attempting to insert Marketing Suspect Temp.", ex, this);
						transaction.Rollback();
						return count;
					}
				}
			}
		}

		public int CreateOrUpdate(SuspectMarketingTemp entity)
		{
			
			using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
			{
				db.Open();
				using (var transaction = db.BeginTransaction())
				{
                    try
                    {
                        entity.DateCreated = DateTime.Now;
						entity.LastUpdated = DateTime.Now;

						var sql = @"UPDATE [dbo].[AFI_Marketing_Suspect_Temp]  
										SET [FirstName]=@FirstName, [LastName]=@LastName, [Email]=@Email, [Phone]=@Phone, [Address]=@Address, [City]=@City, [State]=@State, [ZipCode]=@ZipCode, [Country]=@Country, [DateOfBirth]=@DateOfBirth, [Occupation]=@Occupation, [PreferredCoverage]=@PreferredCoverage, [LeadSource]=@LeadSource, [LeadStatus]=@LeadStatus, [LeadOwner]=@LeadOwner, [LeadScore]=@LeadScore, [LastUpdated]=@LastUpdated, [EntityType]=@EntityType, [EntityID]=@EntityID, [IsSynced]=@IsSynced, [IsValid]=@IsValid, [IsBlockCountry]=@IsBlockCountry where [ID]=@ID";

						var script = @"insert into dbo.[AFI_Marketing_Suspect_Temp] ( [FirstName], [LastName], [Email], [Phone], [Address], [City], [State], [ZipCode], [Country], [DateOfBirth], [Occupation], [PreferredCoverage], [LeadSource], [LeadStatus], [LeadOwner], [LeadScore], [DateCreated], [LastUpdated], [EntityType], [EntityID], [IsSynced], [IsValid], [IsBlockCountry]) values (@FirstName, @LastName, @Email, @Phone, @Address, @City, @State, @ZipCode, @Country, @DateOfBirth, @Occupation, @PreferredCoverage, @LeadSource, @LeadStatus, @LeadOwner, @LeadScore, @DateCreated, @LastUpdated, @EntityType, @EntityID, @IsSynced, @IsValid, @IsBlockCountry)";

						var count = db.Execute(sql, entity, transaction);
						if (count>0)
						{
							transaction.Commit();
						}
					
						else if (count == 0 )
						{
							count = db.Execute(script, entity, transaction);
							transaction.Commit();
						}

						return count;
                    }
                    catch (System.Exception ex)
                    {
                        Log.Error($"{nameof(SuspectMarketingRepositoryTemp)}: Error while attempting to Delete from Marketing Suspect Temp.", ex, this);
                        transaction.Rollback();
                        return 0;
                    }

                }
			}
		}

        public int SyncTempApprovedtoProspect(string ids)
        {
            using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
            {
                var count = 0;
                db.Open();
                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                        // INSERT INTO AFI_Marketing_Prospect
                        var insertSql = @"
                    INSERT INTO [AFI_Marketing_Suspect] (
                        [FirstName], [LastName], [Email], [Phone], [Address], [City],
                        [State], [ZipCode], [Country], [DateOfBirth], [Occupation],
                        [PreferredCoverage], [LeadSource], [LeadStatus], [LeadOwner],
                        [LeadScore], [DateCreated]
                    )
                    SELECT
                        [FirstName], [LastName], [Email], [Phone], [Address], [City],
                        [State], [ZipCode], [Country], [DateOfBirth], [Occupation],
                        [PreferredCoverage], [LeadSource], [LeadStatus], [LeadOwner],
                        [LeadScore], [DateCreated]
                    FROM [AFI_Marketing_Suspect_Temp] MT
                    WHERE MT.ID IN @Ids AND MT.[IsSynced] = 0 AND MT.[IsValid] = 1 AND MT.[IsBlockCountry] = 0";

                        // UPDATE AFI_Marketing_Suspect_Temp
                        var updateSql = @"
                    UPDATE [AFI_Marketing_Suspect_Temp]
                    SET IsSynced = 1
                    WHERE ID IN @Ids";

                        count = db.Execute(insertSql, new { Ids = ids.Split(',') }, transaction);
                        count += db.Execute(updateSql, new { Ids = ids.Split(',') }, transaction);

                        transaction.Commit();
						return count;

                }
                    catch (System.Exception ex)
                {
                    Log.Error($"{nameof(SuspectMarketingRepositoryTemp)}: Error while attempting to insert Marketing Suspect.", ex, this);
                    transaction.Rollback();
                    return count;
                }
            }
            }
        }


		public IEnumerable<SuspectMarketingTemp> GetAllForMarketing(int page = 1, int pageSize = 50, string leadStatus = null, string isSynced = null, string coverage = null , string coverageType = null)
		{
			int ofsetItem = (page - 1) * pageSize;

			using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
			{
				var sql = " select *, COUNT(*) OVER() AS TotalCount from dbo.[AFI_Marketing_Suspect_Temp] WHERE 1 = 1";

				if (!string.IsNullOrEmpty(leadStatus) && leadStatus.ToLower() != "all")
				{
					sql += " AND LeadStatus = @LeadStatus";
				}

				if (!string.IsNullOrEmpty(isSynced) && isSynced.ToLower() != "all")
				{
					if (isSynced.ToLower() == "synced")
					{
						sql += $" AND IsSynced = '{true}'";
					}
					else if (isSynced.ToLower() == "asynced")
					{
						sql += $" AND IsSynced = '{false}'"; // Corrected missing "AND" here
					}

				}

				if (!string.IsNullOrEmpty(coverage) && coverage.ToLower() != "all")
				{
					sql += " AND PreferredCoverage = @PreferredCoverage"; // Corrected missing "AND" here
				}
				if (!string.IsNullOrEmpty(coverageType) && coverageType.ToLower() != "all")
				{
					sql += " AND EntityType = @EntityType"; // Corrected missing "AND" here
				}

				sql += " ORDER BY ID OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY ; ";

				var parameters = new DynamicParameters();
				parameters.Add("@Offset", ofsetItem);
				parameters.Add("@PageSize", pageSize);

				if (!string.IsNullOrEmpty(leadStatus) && leadStatus.ToLower() != "all")
				{
					parameters.Add("@LeadStatus", leadStatus);
				}

				//if (!string.IsNullOrEmpty(isSynced))
				//{
				//	parameters.Add("@IsSynced", isSynced);
				//}

				if (!string.IsNullOrEmpty(coverage) && coverage.ToLower() != "all")
				{
					parameters.Add("@PreferredCoverage", coverage);
				}
				if (!string.IsNullOrEmpty(coverageType) && coverageType.ToLower() != "all")
				{
					parameters.Add("@EntityType", coverageType);
				}

				return db.Query<SuspectMarketingTemp>(sql, parameters);

			}
		}

		public IEnumerable<SuspectMarketingTemp> GetAllForDownloadTempSuspect(string leadStatus = null, string isSynced = null, string coverage = null, string coverageType = null)
		{

			using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
			{
				var sql = "select * from dbo.[AFI_Marketing_Suspect_Temp] WHERE 1 = 1";

				if (!string.IsNullOrEmpty(leadStatus) && leadStatus.ToLower() != "all")
				{
					sql += " AND LeadStatus = @LeadStatus";
				}

				if (!string.IsNullOrEmpty(isSynced) && isSynced.ToLower() != "all")
				{
					if (isSynced.ToLower() == "synced")
					{
						sql += $" AND IsSynced = '{true}'";
					}
					else if (isSynced.ToLower() == "asynced")
					{
						sql += $" AND IsSynced = '{false}'"; // Corrected missing "AND" here
					}

				}

				if (!string.IsNullOrEmpty(coverage) && coverage.ToLower() != "all")
				{
					sql += " AND PreferredCoverage = @PreferredCoverage"; // Corrected missing "AND" here
				}

				if (!string.IsNullOrEmpty(coverageType) && coverageType.ToLower() != "all")
				{
					sql += " AND EntityType = @EntityType"; // Corrected missing "AND" here
				}


				//sql += " ORDER BY SuspectID OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY; ";

				var parameters = new DynamicParameters();


				if (!string.IsNullOrEmpty(leadStatus) && leadStatus.ToLower() != "all")
				{
					parameters.Add("@LeadStatus", leadStatus);
				}

				if (!string.IsNullOrEmpty(coverage) && coverage.ToLower() != "all")
				{
					parameters.Add("@PreferredCoverage", coverage);
				}
				if (!string.IsNullOrEmpty(coverageType) && coverageType.ToLower() != "all")
				{
					parameters.Add("@EntityType", coverageType);
				}

				return db.Query<SuspectMarketingTemp>(sql, parameters);

			}

		}

		

	}
}