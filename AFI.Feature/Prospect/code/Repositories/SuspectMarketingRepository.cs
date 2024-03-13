using AFI.Feature.Prospect.Providers;
using Dapper;
using AFI.Feature.Prospect.Models;
using Sitecore.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System;

namespace AFI.Feature.Prospect.Repositories
{
    public interface ISuspectMarketingRepository
    {

        IEnumerable<SuspectMarketing> GetAllForMarketing();
        IEnumerable<SuspectMarketing> GetAllFromProspectMarketing(int page, int pageSize, string leadStatus, string isSynced, string coverage);
        IEnumerable<SuspectMarketing> GetAllForLeadSource(string leadSource);
        IEnumerable<SuspectMarketing> GetAllForLeadStatus(string leadStatus);
        IEnumerable<SuspectMarketing> GetAllForLeadOwner(string leadOwner);
        int UpdateIsSyncedFromProspect(string ids);
        List<SuspectMarketing> GetProspectsByIds(string ids);
        int Create(SuspectMarketing entity);
        int CreateOrUpdate(SuspectMarketing entity);
        IEnumerable<SuspectMarketing> GetAllForDownloadSuspect(string leadStatus, string isSynced, string coverage);
    }

    public class SuspectMarketingRepository : ISuspectMarketingRepository
    {
        private readonly IDatabaseConnectionProvider _dbConnectionProvider;

        public SuspectMarketingRepository(IDatabaseConnectionProvider dbConnectionProvider)
        {
            _dbConnectionProvider = dbConnectionProvider;
        }

        public IEnumerable<SuspectMarketing> GetAllForMarketing()
        {
            using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
            {
                var sql = "select * from dbo.[AFI_Marketing_Suspect] Order by 1 desc";
                return db.Query<SuspectMarketing>(sql);
            }
        }
        public IEnumerable<SuspectMarketing> GetAllForLeadSource(string leadSource)
        {
            using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
            {
                var sql = "select * from dbo.[AFI_Marketing_Suspect] where [LeadSource] = @leadSource";
                return db.Query<SuspectMarketing>(sql, new { leadSource });
            }
        }
        public IEnumerable<SuspectMarketing> GetAllForLeadStatus(string leadStatus)
        {
            using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
            {
                var sql = "select * from dbo.[AFI_Marketing_Suspect] where [LeadStatus] = @leadStatus";
                return db.Query<SuspectMarketing>(sql, new { leadStatus });
            }
        }
        public IEnumerable<SuspectMarketing> GetAllForLeadOwner(string leadOwner)
        {
            using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
            {
                var sql = "select * from dbo.[AFI_Marketing_Suspect] where [LeadOwner] = @leadOwner";
                return db.Query<SuspectMarketing>(sql, new { leadOwner });
            }
        }

        public int Create(SuspectMarketing entity)
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

                        var sql = "insert into dbo.[AFI_Marketing_Suspect] ( [FirstName], [LastName], [Email], [Phone], [Address], [City], [State], [ZipCode], [Country], [DateOfBirth], [Occupation], [PreferredCoverage], [LeadSource], [LeadStatus], [LeadOwner], [LeadScore], [DateCreated], [LastUpdated]) values (@FirstName, @LastName, @Email, @Phone, @Address, @City, @State, @ZipCode, @Country, @DateOfBirth, @Occupation, @PreferredCoverage, @LeadSource, @LeadStatus, @LeadOwner, @LeadScore, @DateCreated, @LastUpdated)";
                        count = db.Execute(sql, entity, transaction);
                        transaction.Commit();
                        return count;
                    }
                    catch (System.Exception ex)
                    {
                        Log.Error($"{nameof(SuspectMarketingRepository)}: Error while attempting to insert Marketing Prospect.", ex, this);
                        transaction.Rollback();
                        return count;
                    }
                }
            }
        }

        public int CreateOrUpdate(SuspectMarketing entity)
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
                        var sql = @"UPDATE [dbo].[AFI_Marketing_Suspect]  
										SET [FirstName]=@FirstName, [LastName]=@LastName, [Email]=@Email, [Phone]=@Phone, [Address]=@Address, [City]=@City, [State]=@State, [ZipCode]=@ZipCode, [Country]=@Country, [DateOfBirth]=@DateOfBirth, [Occupation]=@Occupation, [PreferredCoverage]=@PreferredCoverage, [LeadSource]=@LeadSource, [LeadStatus]=@LeadStatus, [LeadOwner]=@LeadOwner, [LeadScore]=@LeadScore, [LastUpdated]=@LastUpdated where [ProspectID]=@ProspectID";

                        var script = @"insert into dbo.[AFI_Marketing_Suspect] ( [FirstName], [LastName], [Email], [Phone], [Address], [City], [State], [ZipCode], [Country], [DateOfBirth], [Occupation], [PreferredCoverage], [LeadSource], [LeadStatus], [LeadOwner], [LeadScore], [DateCreated], [LastUpdated]) values (@FirstName, @LastName, @Email, @Phone, @Address, @City, @State, @ZipCode, @Country, @DateOfBirth, @Occupation, @PreferredCoverage, @LeadSource, @LeadStatus, @LeadOwner, @LeadScore, @DateCreated, @LastUpdated)";

                        var count = db.Execute(sql, entity, transaction);
                        if (count > 0)
                        {
                            transaction.Commit();
                        }

                        else if (count == 0)
                        {
                            count = db.Execute(script, entity, transaction);
                            transaction.Commit();
                        }

                        return count;
                    }
                    catch (System.Exception ex)
                    {
                        Log.Error($"{nameof(SuspectMarketingRepository)}: Error while attempting to Delete from Marketing Prospect.", ex, this);
                        transaction.Rollback();
                        return 0;
                    }

                }
            }
        }

        public IEnumerable<SuspectMarketing> GetAllFromProspectMarketing(int page = 1, int pageSize = 50, string leadStatus = null, string isSynced = null, string coverage = null)
        {
            int ofsetItem = (page - 1) * pageSize;

            using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
            {
                var sql = "select *, COUNT(*) OVER() AS TotalCount from dbo.[AFI_Marketing_Suspect] WHERE 1 = 1";

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

                sql += " ORDER BY SuspectID OFFSET @Offset ROWS FETCH NEXT @PageSize ROWS ONLY; ";

                var parameters = new DynamicParameters();
                parameters.Add("@Offset", ofsetItem);
                parameters.Add("@PageSize", pageSize);

                if (!string.IsNullOrEmpty(leadStatus) && leadStatus.ToLower() != "all")
                {
                    parameters.Add("@LeadStatus", leadStatus);
                }

                if (!string.IsNullOrEmpty(coverage) && coverage.ToLower() != "all")
                {
                    parameters.Add("@PreferredCoverage", coverage);
                }

                return db.Query<SuspectMarketing>(sql, parameters);

            }

        }

        public int UpdateIsSyncedFromProspect(string ids)
        {
            using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
            {
                var count = 0;
                db.Open();
                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                        // UPDATE AFI_Marketing_Suspect_Temp
                        var updateSql = @"UPDATE [dbo].[AFI_Marketing_Suspect] SET IsSynced = 1 WHERE SuspectID IN @Ids";

                        count += db.Execute(updateSql, new { Ids = ids.Split(',') }, transaction);

                        transaction.Commit();
                        return count;

                    }
                    catch (System.Exception ex)
                    {
                        Log.Error($"{nameof(SuspectMarketingRepository)}: Error while attempting to insert Marketing Prospect.", ex, this);
                        transaction.Rollback();
                        return count;
                    }
                }
            }
        }

        public List<SuspectMarketing> GetProspectsByIds(string ids)
        {
            List<SuspectMarketing> prospects = new List<SuspectMarketing>();

            using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
            {
                string sqlQuery = "SELECT * FROM dbo.[AFI_Marketing_Suspect] WHERE SuspectID IN @Ids";
                prospects = db.Query<SuspectMarketing>(sqlQuery, new { Ids = ids.Split(',') }).ToList();

            }
            return prospects;
        }

        public IEnumerable<SuspectMarketing> GetAllForDownloadSuspect(string leadStatus = null, string isSynced = null, string coverage = null)
        {
            
            using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
            {
                var sql = "select * from dbo.[AFI_Marketing_Suspect] WHERE 1 = 1";

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

                return db.Query<SuspectMarketing>(sql, parameters);

            }

        }
    }
}