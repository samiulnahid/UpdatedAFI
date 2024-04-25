using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.SqlClient;
using System.Configuration;
using Sitecore.Data.Items;
using AFI.Feature.Prospect.Models;
using Dapper;
using Sitecore.Diagnostics;

namespace AFI.Feature.Prospect.Job
{
    public class QuoteHubSuspectData
    {
        private static readonly string AFIConnectionString = ConfigurationManager.ConnectionStrings["AFIDB"].ConnectionString;
        private static readonly string QHConnectionString = ConfigurationManager.ConnectionStrings["QHAFIDB"].ConnectionString;


        public void Execute(Item[] items, Sitecore.Tasks.CommandItem command, Sitecore.Tasks.ScheduleItem schedule)
        {
            Sitecore.Diagnostics.Log.Info("Qhote Hub Suspect Sitecore scheduled task is being run!", this);

            List<QuoteHub> quotes = new List<QuoteHub>();

            using (var db = new SqlConnection(QHConnectionString))
            {
                var sql = @"SELECT [QuoteHdr].*
                            FROM [dbo].[QuoteHdr]
                            LEFT JOIN [dbo].[PersonalInfo] ON [QuoteHdr].IdQuote = [PersonalInfo].IdQuote
                            WHERE [PersonalInfo].IdQuote IS NULL;";

                quotes = db.Query<QuoteHub>(sql).ToList();
            }

           

            foreach (var data in quotes)
            {
                SuspectMarketingTemp entity = new SuspectMarketingTemp()
                {
                    FirstName = data.FirstName,
                    LastName = data.LastName,
                    Email = data.EmailId,
                    Phone = "", 
                    Address = data.MailingAddr1 + " " + data.MailingAddr2,
                    City = data.MailingCity,
                    State = data.MailingState,
                    ZipCode = data.MailingZip,
                    Country = data.MailingCountryName,
                    DateOfBirth = data.DOB,
                    Occupation = "",
                    PreferredCoverage = "", 
                    LeadSource = "",
                    LeadStatus = "", 
                    LeadOwner = "",
                    LeadScore = 0,
                    DateCreated = data.CreatedOn,
                    LastUpdated = data.ModifiedOn,
                    EntityType = "QH", 
                    EntityID = data.IdQuote, 
                    IsSynced = false,
                    IsValid = true,
                    IsBlockCountry = false,
                    
                };

                using (var db = new SqlConnection(AFIConnectionString))
                {
                    db.Open();
                    using (var transaction = db.BeginTransaction())
                    {
                        try
                        {
                           // entity.DateCreated = DateTime.Now;

                            var sql = "insert into dbo.[AFI_Marketing_Suspect_Temp] ( [FirstName], [LastName], [Email], [Phone], [Address], [City], [State], [ZipCode], [Country], [DateOfBirth], [Occupation], [PreferredCoverage], [LeadSource], [LeadStatus], [LeadOwner], [LeadScore], [DateCreated], [LastUpdated], [EntityType], [EntityID], [IsSynced], [IsValid], [IsBlockCountry]) values (@FirstName, @LastName, @Email, @Phone, @Address, @City, @State, @ZipCode, @Country, @DateOfBirth, @Occupation, @PreferredCoverage, @LeadSource, @LeadStatus, @LeadOwner, @LeadScore, @DateCreated, @LastUpdated, @EntityType, @EntityID, @IsSynced, @IsValid, @IsBlockCountry)";
                            db.Execute(sql, entity, transaction);
                            transaction.Commit();
                           
                        }
                        catch (System.Exception ex)
                        {
                            Sitecore.Diagnostics.Log.Info("Error while attempting to insert Marketing Suspect Temp.", this);
                            Sitecore.Diagnostics.Log.Error($"{nameof(SuspectMarketingTemp)}: Error while attempting to insert Marketing Suspect Temp.", ex, this);
                            transaction.Rollback();
                        }
                    }
                }
               
            }

        }

    
    }
}