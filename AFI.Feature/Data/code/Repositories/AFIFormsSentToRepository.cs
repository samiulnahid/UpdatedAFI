using System.Collections.Generic;
using AFI.Feature.Data.DataModels;
using AFI.Feature.Data.Providers;
using Dapper;
using Sitecore.Diagnostics;

namespace AFI.Feature.Data.Repositories
{
    public interface IAFIFormsSentToRepository
    {
        IEnumerable<AFIFormsSentTo> GetAll();
        AFIFormsSentTo GetByQuoteID(string quoteID);
        void Create(AFIFormsSentTo entity);
    }

    public class AFIFormsSentToRepository : IAFIFormsSentToRepository
    {
        private readonly IDbConnectionProvider _dbConnectionProvider;

        public AFIFormsSentToRepository(IDbConnectionProvider dbConnectionProvider)
        {
            _dbConnectionProvider = dbConnectionProvider;
        }

        public IEnumerable<AFIFormsSentTo> GetAll()
        {
            using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
            {
                var sql = "select * from dbo.[AFI_Forms_SentTo]";
                return db.Query<AFIFormsSentTo>(sql);
            }
        }

        public AFIFormsSentTo GetByQuoteID(string quoteID)
        {
            using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
            {
                var sql = "select top 1 * from dbo.[AFI_Forms_SentTo] where [QuoteID] = @QuoteID";
                return db.QueryFirstOrDefault<AFIFormsSentTo>(sql, new { quoteID });
            }
        }

        public void Create(AFIFormsSentTo entity)
        {
            using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
            {
                db.Open();
                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                        var sql = "insert into dbo.[AFI_Forms_SentTo] ([QuoteType], [QuoteID], [CreatedDate]) values (@QuoteType, @QuoteID, @CreateDate)";
                        db.Execute(sql, entity, transaction);
                        transaction.Commit();
                    }
                    catch (System.Exception ex)
                    {
                        Log.Error($"{nameof(AFIFormsSentToRepository)}: Error while attempting to insert AFIFormsSentTo.", ex, this);
                        transaction.Rollback();
                    }
                }
            }
        }
    }
}