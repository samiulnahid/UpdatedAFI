using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AFI.Feature.QuoteForm.Areas.AFIWEB.Models;
using AFI.Feature.Data.Providers;
using Dapper;
using AFI.Feature.Data.DataModels;
using FTData = AFI.Feature.Data.DataModels;

using Sitecore.ExperienceForms.Samples.SubmitActions;
using System.Data;
using System.Data.SqlClient;
using System.Configuration;
using System.Diagnostics;
using Sitecore.Diagnostics;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Repository;
using AFI.Feature.QuoteForm.Areas.AFIWEB.Helpers;

namespace AFI.Feature.QuoteForm.Areas.AFIWEB.Repository
{
    public interface ICorviasFormRepository
    {
        int InsertForm(CorviasModel form);
    }
    public class CorviasFormRepository : ICorviasFormRepository
    {
        private readonly IDbConnectionProvider _dbConnectionProvider;

        public CorviasFormRepository(IDbConnectionProvider dbConnectionProvider)
        {
            _dbConnectionProvider = dbConnectionProvider;
        }

        public int InsertForm(CorviasModel form)
        {
            var quote = new FTData.Quote();
            quote.Eligibility = form.Eligibility == "on" ? "Military" : " ";
            quote.CoverageType = CoverageTypes.Corvias;
            quote.ZipCode = form.ZipCode;
            quote.Started = DateTime.Now;
            quote.Finished = DateTime.Now;
            quote.Key = Create(quote);
            if (quote.Key > 0)
            {
                form.QuoteKey = quote.Key;
                var quoteContact = new QuoteContact();
                quoteContact.Key = quote.Key;
                quoteContact.FirstName = form.FirstName;
                quoteContact.LastName = form.LastName;
                quoteContact.Street = form.Address;
                quoteContact.State = form.State;
                quoteContact.City = form.City;
                quoteContact.ZipCode = form.ZipCode;
                quoteContact.ServiceBranch = form.BranchOfService;
                quoteContact.ServiceStatus = form.MilitaryStatus;
                quoteContact.ServiceRank = form.MilitaryRank;
                quoteContact.PhoneType = "Home";
                quoteContact.Email = form.Email;
                quoteContact.PhoneNumber = form.PhoneNumber;
                quoteContact.SSN = form.SSN;
                quoteContact.HowToContact = "Email";
                quoteContact.MaritalStatus = "";
                quoteContact.UnderMoratorium = false;
                quoteContact.WantToReceiveInfo = false;
                quoteContact.AFIMember = false;
                quoteContact.CallForReview = false;
                quoteContact.BirthDate = DateTime.Now;
                var contactId = CreateQuoteContact(quoteContact);

                form.CardNumber = form.CardNumber != null ? CryptoService.Encrypt(form.CardNumber) : string.Empty;
                form.SecurityCode = form.SecurityCode !=null ? CryptoService.Encrypt(form.SecurityCode) : string.Empty;
                var corviasId = CreateCorvias(form);
            }
            return quote.Key;
        }
        #region Insert Quote
        public int Create(FTData.Quote entity)
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
                        transaction.Commit();
                        sw.Stop();
                        Sitecore.Diagnostics.Log.Info("STOPWATCH: create auto " + sw.Elapsed, "stopwatch");
                        return id;
                    }
                    catch (System.Exception ex)
                    {
                        Log.Error($"Error while attempting to insert Quote.", ex, this);
                        transaction.Rollback();
                        sw.Stop();
                        Sitecore.Diagnostics.Log.Info("STOPWATCH: create auto quote contact" + sw.Elapsed, "stopwatch");
                        return 0;
                    }
                }
            }
        }
        #endregion

        #region Quote Contact
        public int? CreateQuoteContact(QuoteContact entity)
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
                        transaction.Commit();
                        return id;
                    }
                    catch (System.Exception ex)
                    {
                        Log.Error($"Error while attempting to insert QuoteContact.", ex, this);
                        transaction.Rollback();
                        return 0;
                    }
                }
            }
        }

        #endregion

        #region Quote Corvias
        public int CreateCorvias(CorviasModel entity)
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
                        var sql = @"insert into dbo.[QuoteCorvias] ([QuoteKey], [CoverageType], [CoverageMonth], [CoverageYear], [AdditionalQuestions], [PaymentMethod], [AgentCallDate], [AgentCallTime], [NameOnCard], [CardNumber], [ExpiryDate], [SecurityCode], [Routing], [AccountNumber])
                                    VALUES (@QuoteKey, @CoverageType, @CoverageMonth, @CoverageYear, @AdditionalQuestions, @PaymentMethod, @AgentCallDate, @AgentCallTime, @NameOnCard, @CardNumber, @ExpiryDate, @SecurityCode, @Routing, @AccountNumber);";

                        var count = db.Execute(sql, entity, transaction);
                        transaction.Commit();
                        sw.Stop();
                        Sitecore.Diagnostics.Log.Info("STOPWATCH: quote save" + sw.Elapsed, "stopwatch");
                        return count;

                    }
                    catch (System.Exception ex)
                    {
                        Log.Error($"Error while attempting to update QuoteContact.", ex, this);
                        transaction.Rollback();
                        sw.Stop();
                        Sitecore.Diagnostics.Log.Info("STOPWATCH: quote save" + sw.Elapsed, "stopwatch");
                        return 0;
                    }
                }
            }
        }
        #endregion
    }
}