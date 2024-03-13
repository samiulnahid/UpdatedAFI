using AFI.Feature.Data.Providers;
using Dapper;
using AFI.Feature.Data.DataModels;
using Sitecore.Diagnostics;
using System.Collections.Generic;
using System;
using System.Diagnostics;
using System.Web;
using Sitecore.Publishing.Explanations;
using Sitecore.Threading.Locks;
using Sitecore.SecurityModel;
using Sitecore.Data;

namespace AFI.Feature.Data.Repositories
{
    public interface IQuoteRepository
    {
        IEnumerable<Quote> GetAll();
        Quote GetByKey(int key);
        Quote GetBySaveForLaterKey(Guid key);
        int Create(Quote entity);
        Quote RecordExists(Quote entity);
        int Update(Quote entity);
    }

    public class QuoteRepository : IQuoteRepository
    {
        private readonly IDbConnectionProvider _dbConnectionProvider;

        public QuoteRepository(IDbConnectionProvider dbConnectionProvider)
        {
            _dbConnectionProvider = dbConnectionProvider;
        }

        public IEnumerable<Quote> GetAll()
        {
            using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
            {
                var sql = "select * from dbo.[Quote]";
                return db.Query<Quote>(sql);
            }
        }

        public Quote GetByKey(int key)
        {
            using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
            {
                var sql = "select top 1 * from dbo.[Quote] where [Key] = @Key";
                return db.QueryFirstOrDefault<Quote>(sql, new { key });
            }
        }

        public Quote GetBySaveForLaterKey(Guid key)
        {
            using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
            {
                var sql = "select top 1 * from dbo.[Quote] where [SaveForLaterKey] = @Key AND DATEDIFF(day, SaveForLaterCreateDate, GETDATE()) < 14";
                return db.QueryFirstOrDefault<Quote>(sql, new { key });
            }
        }

        public int Create(Quote entity)
        {
            HttpCookie ck_responsetype = HttpContext.Current.Request.Cookies["responsetype"];
            HttpCookie ck_responsedescription = HttpContext.Current.Request.Cookies["responsedescription"];
            HttpCookie ck_utm_campaign = HttpContext.Current.Request.Cookies["utm_campaign"];
            HttpCookie ck_utm_content = HttpContext.Current.Request.Cookies["utm_content"];
            HttpCookie ck_utm_medium = HttpContext.Current.Request.Cookies["utm_medium"];
            HttpCookie ck_utm_source = HttpContext.Current.Request.Cookies["utm_source"];
            HttpCookie ck_utm_term = HttpContext.Current.Request.Cookies["utm_term"];
            HttpCookie ck_gclid = HttpContext.Current.Request.Cookies["gclid"];
            HttpCookie ck_msclkid = HttpContext.Current.Request.Cookies["msclkid"];
            HttpCookie ck_fbclid = HttpContext.Current.Request.Cookies["fbclid"];
            HttpCookie ck_test = HttpContext.Current.Request.Cookies["test"];


            var responsetype = string.Empty;
            var responsedescription = string.Empty;
            var utm_campaign = string.Empty;
            var utm_content = string.Empty;
            var utm_medium = string.Empty;
            var utm_source = string.Empty;
            var utm_term = string.Empty;
            var gclid = string.Empty;
            var msclkid = string.Empty;
            var fbclid = string.Empty;

            if (ck_responsetype != null)
            {
                responsetype = !string.IsNullOrEmpty(ck_responsetype.Value) ? ck_responsetype.Value : string.Empty;
                if (!string.IsNullOrEmpty(responsetype))
                    entity.ResponseType = responsetype;
            }

            if (ck_responsedescription != null)
            {
                responsedescription = !string.IsNullOrEmpty(ck_responsedescription.Value) ? ck_responsedescription.Value : string.Empty;
                if (!string.IsNullOrEmpty(responsedescription))
                    entity.ResponseDescription = responsedescription;
            }

            if (ck_utm_campaign != null)
                utm_campaign = !string.IsNullOrEmpty(ck_utm_campaign.Value) ? ck_utm_campaign.Value : string.Empty;


            if (ck_utm_content != null)
                utm_content = !string.IsNullOrEmpty(ck_utm_content.Value) ? ck_utm_content.Value : string.Empty;

            if (ck_utm_medium != null)
                utm_medium = !string.IsNullOrEmpty(ck_utm_medium.Value) ? ck_utm_medium.Value : string.Empty;

            if (ck_utm_source != null)
                utm_source = !string.IsNullOrEmpty(ck_utm_source.Value) ? ck_utm_source.Value : string.Empty;

            if (ck_utm_term != null)
                utm_term = !string.IsNullOrEmpty(ck_utm_term.Value) ? ck_utm_term.Value : string.Empty;

            if (ck_gclid != null)
                gclid = !string.IsNullOrEmpty(ck_gclid.Value) ? ck_gclid.Value : string.Empty;

            if (ck_msclkid != null)
                msclkid = !string.IsNullOrEmpty(ck_msclkid.Value) ? ck_msclkid.Value : string.Empty;

            if (ck_fbclid != null)
                fbclid = !string.IsNullOrEmpty(ck_fbclid.Value) ? ck_fbclid.Value : string.Empty;



            entity.WebCampaign = utm_campaign;
            entity.WebContent = utm_content;
            entity.WebMedium = utm_medium;
            entity.WebSource = utm_source;
            entity.WebTerm = utm_term;
            if (!string.IsNullOrEmpty(gclid))
            {
                entity.WebClickID = gclid;
                entity.gclid = "1";
                entity.msclkid = "0";
                entity.fbclid = "0";
            }
            if (!string.IsNullOrEmpty(msclkid))
            {
                entity.WebClickID = msclkid;
                entity.gclid = "0";
                entity.msclkid = "1";
                entity.fbclid = "0";
            }
            if (!string.IsNullOrEmpty(fbclid))
            {
                entity.WebClickID = fbclid;
                entity.gclid = "0";
                entity.msclkid = "0";
                entity.fbclid = "1";
            }


            Stopwatch sw = new Stopwatch();
            sw.Start();
            using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
            {
                db.Open();
                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                        var sql = "insert into dbo.[Quote] ([CoverageType], [ZipCode], [Eligibility], [Remarks], [ReadDisclaimer], [Started], [Finished], [ResponseType], [ResponseDescription], [Offer], [OfferDescription], [IP_Address], [ExtraInfo], [IsSuspicious], [IsInterested], [SaveForLaterKey], [SaveForLaterCreateDate],[WebCampaign],[WebContent],[WebMedium],[WebSource],[WebTerm],[WebClickID],[gclid],[msclkid],[fbclid]) values (@CoverageType, @ZipCode, @Eligibility, @Remarks, @ReadDisclaimer, @Started, @Finished, @ResponseType, @ResponseDescription, @Offer, @OfferDescription, @IP_Address, @ExtraInfo, @IsSuspicious, @IsInterested, @SaveForLaterKey, @SaveForLaterCreateDate, @WebCampaign, @WebContent, @WebMedium, @WebSource, @WebTerm, @WebClickID, @gclid, @msclkid, @fbclid); select scope_identity();";
                        var id = db.QueryFirstOrDefault<int>(sql, entity, transaction);
                        //   transaction.Commit();
                        // sw.Stop();
                        Sitecore.Diagnostics.Log.Info("STOPWATCH: create auto " + sw.Elapsed, "stopwatch");

                        var sqlQ = "insert into dbo.[AFI_Marketing_Suspect_Temp] ( [ZipCode], [PreferredCoverage], [LeadSource], [LeadStatus], [LeadOwner], [LeadScore], [DateCreated], [LastUpdated], [EntityType], [EntityID], [IsValid]) values (@ZipCode, @PreferredCoverage, @LeadSource, @LeadStatus, @LeadOwner, @LeadScore, @DateCreated, @LastUpdated, @EntityType, @EntityID, @IsValid)";
                        var newId = db.QueryFirstOrDefault<int>(sqlQ, new
                        {
                            ZipCode = entity.ZipCode,
                            PreferredCoverage = entity.CoverageType,
                            LeadSource = entity.ResponseDescription,
                            LeadStatus = "New",
                            LeadOwner = "Sitecore Form",
                            LeadScore = "0",
                            DateCreated = entity.Started,
                            LastUpdated = entity.Finished,
                            EntityType = "Quote",
                            EntityID = id,
                            IsValid=false
                        }, transaction);
                        transaction.Commit();
                        sw.Stop();
                        Sitecore.Diagnostics.Log.Info("STOPWATCH: create auto " + sw.Elapsed, "stopwatch");


                        return id;
                    }
                    catch (System.Exception ex)
                    {
                        Log.Error($"{nameof(QuoteRepository)}: Error while attempting to insert Quote.", ex, this);
                        transaction.Rollback();
                        sw.Stop();
                        Sitecore.Diagnostics.Log.Info("STOPWATCH: create auto quote contact" + sw.Elapsed, "stopwatch");
                        return 0;
                    }
                }
            }
        }

        public int Update(Quote entity)
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
                        HttpCookie ck_responsetype = HttpContext.Current.Request.Cookies["responsetype"];
                        HttpCookie ck_responsedescription = HttpContext.Current.Request.Cookies["responsedescription"];
                        var responsetype = string.Empty;
                        var responsedescription = string.Empty;
                        if (ck_responsetype != null)
                        {
                            responsetype = !string.IsNullOrEmpty(ck_responsetype.Value) ? ck_responsetype.Value : string.Empty;
                            if (!string.IsNullOrEmpty(responsetype))
                                entity.ResponseType = responsetype;
                        }

                        if (ck_responsedescription != null)
                        {
                            responsedescription = !string.IsNullOrEmpty(ck_responsedescription.Value) ? ck_responsedescription.Value : string.Empty;
                            if (!string.IsNullOrEmpty(responsedescription))
                                entity.ResponseDescription = responsedescription;
                        }

                        //INFO: [Started] column removed from Update since this column it's only used when Quote is created
                        var sql = "UPDATE [dbo].[Quote] SET[CoverageType] = @CoverageType,[ZipCode] = @ZipCode,[Eligibility] = @Eligibility,[Remarks] = @Remarks,[ReadDisclaimer] = @ReadDisclaimer,[Finished] = @Finished,[ResponseType] = @ResponseType,[ResponseDescription] = @ResponseDescription,[Offer] = @Offer,[OfferDescription] = @OfferDescription,[IP_Address] = @IP_Address,[ExtraInfo] = @ExtraInfo,[IsSuspicious] = @IsSuspicious,[IsInterested] = @IsInterested,[SaveForLaterKey] = @SaveForLaterKey,[SaveForLaterCreateDate] = @SaveForLaterCreateDate WHERE[Key] = @Key";
                        var count = db.Execute(sql, entity, transaction);

                        //INFO: Marketing Prospect - Completed Quote so Delete this record form Prospect Temp 
                        if (entity.Finished != null)
                        {                         
                            int entityId = entity.Key;
                            var sqlQ = "DELETE FROM [AFI_Marketing_Suspect_Temp] WHERE EntityID = @EntityID";
                            db.Execute(sqlQ, new { EntityID = entityId }, transaction);
                        }


                        transaction.Commit();
                        sw.Stop();
                        Sitecore.Diagnostics.Log.Info("STOPWATCH: update auto quote contact " + sw.Elapsed, "stopwatch");
                        return count;

                    }
                    catch (System.Exception ex)
                    {
                        Log.Error($"{nameof(QuoteRepository)}: Error while attempting to update Quote.", ex, this);
                        transaction.Rollback();
                        sw.Stop();
                        Sitecore.Diagnostics.Log.Info("STOPWATCH: update auto quote contact" + sw.Elapsed, "stopwatch");
                        return 0;
                    }
                }
            }
        }


        public Quote RecordExists(Quote entity)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();
            using (var db = _dbConnectionProvider.GetAFIDatabaseConnection())
            {
                try
                {
                    var sql = "SELECT top 1 * FROM dbo.[Quote] Where [CoverageType]=@CoverageType AND [ZipCode]=@ZipCode AND [Eligibility]=@Eligibility AND [Started]=@Started AND [ResponseType]=@ResponseType AND [ResponseDescription]=@ResponseDescription;";
                    var result = db.QueryFirstOrDefault<Quote>(sql, entity);
                    sw.Stop();
                    Sitecore.Diagnostics.Log.Info("STOPWATCH: Check record exists" + sw.Elapsed, "stopwatch");
                    if (result != null)
                    {
                        return result;
                    }
                    else
                    {
                        return null;
                    }
                }
                catch (System.Exception ex)
                {
                    Log.Error($"{nameof(QuoteRepository)}: Error while attempting to Check record exists.", ex, this);
                    sw.Stop();
                    Sitecore.Diagnostics.Log.Info("STOPWATCH: Check record exists" + sw.Elapsed, "stopwatch");
                    return null;
                }
            }
        }
    }
}