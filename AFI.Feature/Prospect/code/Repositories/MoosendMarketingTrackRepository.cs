using AFI.Feature.Prospect.Providers;
using Dapper;
using AFI.Feature.Prospect.Models;
using Sitecore.Diagnostics;
using System.Collections.Generic;
using System.Linq;
using System;

namespace AFI.Feature.Prospect.Repositories
{
    public interface IProspectMarketingTrackRepository
	{
        IEnumerable<ProspectMarketingTrack> GetAllForMarketingTrack();
		IEnumerable<ProspectMarketingTrack> GetAllForSource(string source);

		void Create(ProspectMarketingTrack entity);
        
    }

    public class ProspectMarketingTrackRepository : IProspectMarketingTrackRepository
	{
		private readonly IDatabaseConnectionProvider _dbConnectionProvider;

		public ProspectMarketingTrackRepository(IDatabaseConnectionProvider dbConnectionProvider)
		{
			_dbConnectionProvider = dbConnectionProvider;
		}

		public IEnumerable<ProspectMarketingTrack> GetAllForMarketingTrack()
		{
			using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
			{
				var sql = "select * from dbo.[Prospect_Marketing_Track]";
				return db.Query<ProspectMarketingTrack>(sql);
			}
		}
		public IEnumerable<ProspectMarketingTrack> GetAllForSource(string source)
		{
			using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
			{
				var sql = "select * from dbo.[Prospect_Marketing_Track] where [Source] = @source";
				return db.Query<ProspectMarketingTrack>(sql, new { source });
			}
		}
	
		public void Create(ProspectMarketingTrack entity)
		{
			using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
			{
				db.Open();
				using (var transaction = db.BeginTransaction())
				{
					try
					{
						entity.CreatedDate = DateTime.Now;

						var sql = "insert into dbo.[Prospect_Marketing_Track] ( [MarketingProspectId], [Source], [CreatedBy], [CreatedDate]) values (@MarketingProspectId, @Source, @CreatedBy, @CreatedDate)";
						db.Execute(sql, entity, transaction);
						transaction.Commit();
					}
					catch (System.Exception ex)
					{
						Log.Error($"{nameof(ProspectMarketingTrackRepository)}: Error while attempting to insert Marketing Prospect Track.", ex, this);
						transaction.Rollback();
					}
				}
			}
		}

		
	}
}