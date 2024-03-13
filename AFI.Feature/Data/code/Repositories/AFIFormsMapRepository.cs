using System.Collections;
using System.Collections.Generic;
using AFI.Feature.Data.DataModels;
using AFI.Feature.Data.Providers;
using Dapper;

namespace AFI.Feature.Data.Repositories
{
    public interface IAFIFormsMapRepository
    {
        IEnumerable<AFIFormsMapID> GetAll();
        AFIFormsMapID GetByCoverageType(string coverageType);
    }
    public class AFIFormsMapRepository : IAFIFormsMapRepository
    {
        private readonly IDbConnectionProvider _dbConnectionProvider;

        public AFIFormsMapRepository(IDbConnectionProvider dbConnectionProvider)
        {
            _dbConnectionProvider = dbConnectionProvider;
        }


        public IEnumerable<AFIFormsMapID> GetAll()
        {
            using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
            {
                var sql = "select * from dbo.[AFI_Forms_MapID]";
                return db.Query<AFIFormsMapID>(sql);
            }
        }

        public AFIFormsMapID GetByCoverageType(string coverageType)
        {
            using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
            {
                var sql = "select top 1 * from dbo.[AFI_Forms_MapID] where [CoverageType] = @CoverageType";
                return db.QueryFirstOrDefault<AFIFormsMapID>(sql, new { coverageType });
            }
        }
    }
}