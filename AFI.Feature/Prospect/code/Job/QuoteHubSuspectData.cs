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

            // Fetching existing EntityIDs from AFI_Marketing_Suspect_Temp
            List<int> entityIdList = new List<int>();
            string entityType = "QH";
            using (var db = new SqlConnection(AFIConnectionString))
            {
                var sql = "SELECT EntityID FROM [dbo].[AFI_Marketing_Suspect_Temp] WHERE EntityType = @EntityType";
                entityIdList = db.Query<int>(sql, new { EntityType = entityType }).ToList();
            }

            // Fetching new quotes
            List<QuoteHub> quotes = new List<QuoteHub>();
            using (var db = new SqlConnection(QHConnectionString))
            {
                var sql = @"SELECT [QuoteHdr].*
                    FROM [dbo].[QuoteHdr]
                    LEFT JOIN [dbo].[PersonalInfo] ON [QuoteHdr].IdQuote = [PersonalInfo].IdQuote
                    WHERE [PersonalInfo].IdQuote IS NULL;";
                quotes = db.Query<QuoteHub>(sql).ToList();
            }

            // Inserting new quotes into AFI_Marketing_Suspect_Temp
            using (var db = new SqlConnection(AFIConnectionString))
            {
                db.Open();
                using (var transaction = db.BeginTransaction())
                {
                    try
                    {
                        foreach (var data in quotes)
                        {
                            // Check if the EntityID already exists in the entityIdList
                            if (!entityIdList.Contains(data.IdQuote))
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
                                    PreferredCoverage = data.LOB,
                                    LeadSource = "",
                                    LeadStatus = "",
                                    LeadOwner = "",
                                    LeadScore = 0,
                                    DateCreated = DateTime.Now,
                                    LastUpdated = null,
                                    EntityType = entityType,
                                    EntityID = data.IdQuote,
                                    IsSynced = false,
                                    IsValid = true,
                                    IsBlockCountry = false,
                                };

                                var sqlInsert = @"INSERT INTO dbo.[AFI_Marketing_Suspect_Temp] 
                                          ([FirstName], [LastName], [Email], [Phone], [Address], [City], [State], [ZipCode], [Country], [DateOfBirth], [Occupation], [PreferredCoverage], [LeadSource], [LeadStatus], [LeadOwner], [LeadScore], [DateCreated], [LastUpdated], [EntityType], [EntityID], [IsSynced], [IsValid], [IsBlockCountry]) 
                                          VALUES 
                                          (@FirstName, @LastName, @Email, @Phone, @Address, @City, @State, @ZipCode, @Country, @DateOfBirth, @Occupation, @PreferredCoverage, @LeadSource, @LeadStatus, @LeadOwner, @LeadScore, @DateCreated, @LastUpdated, @EntityType, @EntityID, @IsSynced, @IsValid, @IsBlockCountry)";
                                db.Execute(sqlInsert, entity, transaction);
                            }
                        }
                        transaction.Commit();
                    }
                    catch (Exception ex)
                    {
                        Sitecore.Diagnostics.Log.Info("Error while attempting to insert Marketing Suspect Temp.", this);
                        Sitecore.Diagnostics.Log.Error($"{nameof(SuspectMarketingTemp)}: Error while attempting to insert Marketing Suspect Temp.", ex, this);
                        transaction.Rollback();
                    }
                }
            }


            #region insert suspect

            //foreach (var data in quotes)
            //{
            //    SuspectMarketingTemp entity = new SuspectMarketingTemp()
            //    {
            //        FirstName = data.FirstName,
            //        LastName = data.LastName,
            //        Email = data.EmailId,
            //        Phone = "",
            //        Address = data.MailingAddr1 + " " + data.MailingAddr2,
            //        City = data.MailingCity,
            //        State = data.MailingState,
            //        ZipCode = data.MailingZip,
            //        Country = data.MailingCountryName,
            //        DateOfBirth = data.DOB,
            //        Occupation = "",
            //        PreferredCoverage = data.LOB,
            //        LeadSource = "",
            //        LeadStatus = "",
            //        LeadOwner = "",
            //        LeadScore = 0,
            //        DateCreated = DateTime.Now,
            //        EntityType = "QH",
            //        EntityID = data.IdQuote,
            //        IsSynced = false,
            //        IsValid = true,
            //        IsBlockCountry = false,

            //    };

            //    using (var db = new SqlConnection(AFIConnectionString))
            //    {
            //        db.Open();
            //        using (var transaction = db.BeginTransaction())
            //        {
            //            try
            //            {
            //                // entity.DateCreated = DateTime.Now;

            //                var sql = "insert into dbo.[AFI_Marketing_Suspect_Temp] ( [FirstName], [LastName], [Email], [Phone], [Address], [City], [State], [ZipCode], [Country], [DateOfBirth], [Occupation], [PreferredCoverage], [LeadSource], [LeadStatus], [LeadOwner], [LeadScore], [DateCreated], [LastUpdated], [EntityType], [EntityID], [IsSynced], [IsValid], [IsBlockCountry]) values (@FirstName, @LastName, @Email, @Phone, @Address, @City, @State, @ZipCode, @Country, @DateOfBirth, @Occupation, @PreferredCoverage, @LeadSource, @LeadStatus, @LeadOwner, @LeadScore, @DateCreated, @LastUpdated, @EntityType, @EntityID, @IsSynced, @IsValid, @IsBlockCountry)";
            //                db.Execute(sql, entity, transaction);
            //                transaction.Commit();

            //            }
            //            catch (System.Exception ex)
            //            {
            //                Sitecore.Diagnostics.Log.Info("Error while attempting to insert Marketing Suspect Temp.", this);
            //                Sitecore.Diagnostics.Log.Error($"{nameof(SuspectMarketingTemp)}: Error while attempting to insert Marketing Suspect Temp.", ex, this);
            //                transaction.Rollback();
            //            }
            //        }
            //    }

            //}

            #endregion


        }

    
    }
}