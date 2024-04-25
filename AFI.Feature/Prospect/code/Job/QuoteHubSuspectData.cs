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

                };

                using (SqlConnection db = new SqlConnection(AFIConnectionString))
                {
                    db.Open();
                    try
                    {
                        var sql = "insert into dbo.[AFI_Marketing_Suspect_Temp] ( [FirstName], [LastName], [Email], [Phone], [Address], [City], [State], [ZipCode], [Country], [DateOfBirth], [Occupation], [PreferredCoverage], [LeadSource], [LeadStatus], [LeadOwner], [LeadScore], [DateCreated], [LastUpdated], [EntityType], [EntityID], [IsSynced], [IsValid], [IsBlockCountry]) values (@FirstName, @LastName, @Email, @Phone, @Address, @City, @State, @ZipCode, @Country, @DateOfBirth, @Occupation, @PreferredCoverage, @LeadSource, @LeadStatus, @LeadOwner, @LeadScore, @DateCreated, @LastUpdated, @EntityType, @EntityID, @IsSynced, @IsValid, @IsBlockCountry)";

                        using (SqlCommand cmd = new SqlCommand(sql, db))
                        {
                            cmd.Parameters.AddWithValue("@ListName", data.FirstName);
                            cmd.Parameters.AddWithValue("@ListId", data.LastName);
                            cmd.Parameters.AddWithValue("@CreatedDate", DateTime.Now);

                            // Execute the query and retrieve the inserted ID
                            cmd.ExecuteNonQuery();

                           
                        }
                    }
                    catch (Exception ex)
                    {
                        Log.Error("Error while attempting to insert Suspect Temp Data.", ex, this);
                        // You may want to handle or log the exception accordingly
                    }
                    finally
                    {
                        db.Close();
                    }
                }
            }

        }

    
    }
}