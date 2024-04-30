using Sitecore.Data.Items;
using System;
using System.Configuration;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Data.SqlClient;
using System.Reflection;
using AFI.Feature.QuoteForm.Areas.AFIWEB.Models;
using AFI.Feature.QuoteForm.Areas.AFIWEB.Helpers.Configurations;
using System.Text.RegularExpressions;
using AFI.Feature.QuoteForm.Areas.AFIWEB.Helpers.Strings;
using AFI.Feature.QuoteForm.Areas.AFIWEB.Services;
using Dapper;
using Sitecore.Diagnostics;
using System.Net.Mail;
using System.Net;
//using Sitecore.Xdb.Reporting;

namespace AFI.Feature.QuoteForm.Areas.AFIWEB.Job
{
    public class SurveyClaimMailTest
    {
        private static readonly string AFIConnectionString = ConfigurationManager.ConnectionStrings["AFIDB"].ConnectionString;
        private static readonly string ConnectionString = ConfigurationManager.ConnectionStrings["MarketingDB"].ConnectionString;

        public void Execute(Item[] items, Sitecore.Tasks.CommandItem command, Sitecore.Tasks.ScheduleItem schedule)
        {
            string claimTestMail = ConfigurationManager.AppSettings["ClaimTestMail"];
            List<SurveyForm> mismatchedData = LoadMismatchedData(ConnectionString, AFIConnectionString);

            Sitecore.Data.Items.Item emailtemplate = Sitecore.Context.ContentDatabase.GetItem(ClaimSurveyMail.Email_ItemId);

            string subject = emailtemplate[ClaimSurveyMail.Subject];
            string body = emailtemplate[ClaimSurveyMail.Body];
            string recipients = emailtemplate[ClaimSurveyMail.Recipients];
            string sender = emailtemplate[ClaimSurveyMail.Sender];

            // Now you have the mismatched data
            foreach (SurveyForm data in mismatchedData)
            {
       
                Sitecore.Diagnostics.Log.Info($"Survay ClaimNumber: {data.ClaimNumber}", "Mehedi");

                #region EMAIL                                    
                //Sitecore.Data.Items.Item emailtemplate = Sitecore.Context.ContentDatabase.GetItem(ClaimSurveyMail.Email_ItemId);
                try
                {
                    // EMAIL 

                    //string subject = emailtemplate[ClaimSurveyMail.Subject];
                    //string body = emailtemplate[ClaimSurveyMail.Body];
                    //string recipients = emailtemplate[ClaimSurveyMail.Recipients];
                    //string sender = emailtemplate[ClaimSurveyMail.Sender];

                    Sitecore.Diagnostics.Log.Info($"Survay Email Send", "Mehedi");
                    #region  Email
                    try
                    {
                        var email_client = ParseEmail(sender, subject, body, recipients, data, claimTestMail);
                        Sitecore.Diagnostics.Log.Info($"Survay ParseEmail completed", "Mehedi");
                      //  var isSend = emailService(email_client);
                         var isSend =true;
                        Sitecore.Diagnostics.Log.Info($"Survay emailService sent claim: " + data.ClaimNumber, "Mehedi");
                        if (isSend)
                        {
                            Sitecore.Diagnostics.Log.Info($"Survay claim insert ClaimsSent " + data.ClaimNumber, "Mehedi");
                            _insertAFIDB(data);
                            Sitecore.Diagnostics.Log.Info($"Survay claim insert Email Log " + data.ClaimNumber, "Mehedi");
                            _insertEmailLog(email_client, data.ClaimNumber, data.EmailAddress);
                        }

                    }
                    catch (Exception ex)
                    {
                        Sitecore.Diagnostics.Log.Info($"Survay email Send error" + ex.Message, "Mehedi");
                    }

                    #endregion

                }
                catch (Exception ex)
                {
                    Sitecore.Diagnostics.Log.Info($"Survay email Send error " + ex.Message, "Mehedi");
                }
                #endregion
            }


        }
        private static List<SurveyForm> LoadMismatchedData(string marketingConnectionString, string claimsConnectionString)
        {
            List<SurveyForm> mismatchedData = new List<SurveyForm>();

            // Query to get all data from MarketingClaims table in Database1
            string marketingQuery = "SELECT Id, ClaimNumber FROM [dbo].[Survey.vwClaims]";

            // Query to get all data from ClaimsSent table in Database2
            string claimsQuery = "SELECT Id, ClaimNumber FROM [Survey].[ClaimsSent]";

            // Execute queries on both databases
            List<SurveyForm> marketingData = ExecuteQuery(marketingConnectionString, marketingQuery);
            List<SurveyForm> claimsData = ExecuteQuery(claimsConnectionString, claimsQuery);

            // Compare data and find mismatches
            foreach (SurveyForm marketingClaim in marketingData)
            {
                bool foundMatch = false;
                foreach (SurveyForm sentClaim in claimsData)
                {
                    if (marketingClaim.ClaimNumber == sentClaim.ClaimNumber)
                    {
                        foundMatch = true;
                        break;
                    }
                }
                if (!foundMatch)
                {
                    mismatchedData.Add(marketingClaim);
                }
            }

            return mismatchedData;
        }
        private static List<SurveyForm> ExecuteQuery(string connectionString, string query)
        {
            List<SurveyForm> result = new List<SurveyForm>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand command = new SqlCommand(query, connection);
                connection.Open();
                SqlDataReader reader = command.ExecuteReader();
                while (reader.Read())
                {
                    SurveyForm surveyForm = new SurveyForm();
                    surveyForm.id = reader["Id"].ToString();
                    surveyForm.ClaimNumber = reader["ClaimNumber"].ToString();
                    result.Add(surveyForm);
                }
            }

            return result;
        }
        private void _insertEmailLog(Email data, string claimNumber, string toEmail)
        {
            var _to = "";
            foreach (var address in data.To)
            {
                if (string.IsNullOrWhiteSpace(address))
                    continue;

                _to += address;
            }
            using (SqlConnection db = new SqlConnection(AFIConnectionString))
            {
                db.Open();
                try
                {
                    var sql = "INSERT INTO [AFI_Email_Log] ([Type],[ClaimNumber],[Subject],[Body],[From_Email],[To_Email],[CreatedDate],[Source]) values(@Type,@ClaimNumber,@Subject,@Body,@From_Email,@To_Email,@CreatedDate,@Source)";
                    SqlCommand cmd = new SqlCommand(sql, db);
                    cmd.Parameters.AddWithValue("@Type", "Claim");
                    cmd.Parameters.AddWithValue("@ClaimNumber", claimNumber);
                    cmd.Parameters.AddWithValue("@Subject", data.Subject);
                    cmd.Parameters.AddWithValue("@Body", data.Body);
                    cmd.Parameters.AddWithValue("@From_Email", data.FromEmail == null ? data.FromName : data.FromEmail);
                    cmd.Parameters.AddWithValue("@To_Email", _to);
                    cmd.Parameters.AddWithValue("@CreatedDate", DateTime.Now);
                    cmd.Parameters.AddWithValue("@Source", System.Net.Dns.GetHostEntry(System.Net.Dns.GetHostName()).HostName);
                    cmd.ExecuteNonQuery();
                }
                catch (System.Exception ex)
                {
                    Log.Error($"Error while attempting to insert Email Log record.", ex, this);
                }

                db.Close();
            }
        }
        private bool emailService(Email email)
        {
            try
            {
                var host = ConfigurationManager.AppSettings["SMTPhost"];
                var port = ConfigurationManager.AppSettings["SMTPport"];
                var username = ConfigurationManager.AppSettings["SMTPuserName"];
                var password = ConfigurationManager.AppSettings["SMTPpassword"];
                var authMethod = ConfigurationManager.AppSettings["SMTPauthenticationMethod"];
                var tls = ConfigurationManager.AppSettings["SMTPstartTls"];
                var formEmail = ConfigurationManager.AppSettings["SMTPfromEmail"];

                int _port = string.IsNullOrEmpty(port) ? 0 : Convert.ToInt32(port);

                var client = new SmtpClient(host: host, port: _port);
                client.Credentials = new NetworkCredential(username, password);

                var message = new MailMessage();
                message.Subject = email.Subject;
                message.Body = email.Body;
                message.IsBodyHtml = email.IsBodyHtml;
                message.From = string.IsNullOrWhiteSpace(email.FromName)
                    ? new MailAddress(email.FromEmail)
                    : new MailAddress(email.FromEmail, email.FromName);

                foreach (var attachement in email.Attachments)
                {
                    message.Attachments.Add(attachement);
                }

                foreach (var address in email.To)
                {
                    if (string.IsNullOrWhiteSpace(address))
                        continue;

                    message.To.Add(address);
                }

                foreach (var address in email.Cc)
                {
                    if (string.IsNullOrWhiteSpace(address))
                        continue;

                    message.CC.Add(address);
                }

                foreach (var address in email.Bcc)
                {
                    if (string.IsNullOrWhiteSpace(address))
                        continue;

                    message.Bcc.Add(address);
                }

                client.Send(message);
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error($"{nameof(EmailService)}: Error sending email", ex, this);
                throw;
            }
            return true;

        }

        private void _insertAFIDB(SurveyForm data)
        {
            using (SqlConnection db = new SqlConnection(AFIConnectionString))
            {
                db.Open();
                try
                {
                    var sql = "INSERT INTO [Survey].[ClaimsSent] ([ClaimNumber],[Datesurveysent]) values('" + data.ClaimNumber + "','" + DateTime.Now + "')";
                    SqlCommand cmd = new SqlCommand(sql, db);
                    cmd.ExecuteNonQuery();
                }
                catch (System.Exception ex)
                {
                    Log.Error($"Error while attempting to insert Quote.", ex, this);
                }

                db.Close();
            }

        }

        private Email ParseEmail(string emailFromList, string emailSubject, string body, string emailRecipient, SurveyForm data, string claimTestMail)
        {

            Regex re = new Regex(@"\{\{(\w+)\}\}", RegexOptions.Compiled);
            IDictionary<string, string> mappingKeys = new Dictionary<string, string>();
            mappingKeys.Add("Sl", data.Salutation != "" ? data.Salutation : "");
            mappingKeys.Add("LastName", data.LastName != "" ? data.LastName : "");
            mappingKeys.Add("ClaimNumber", data.ClaimNumber != "" ? data.ClaimNumber : "");

            string bodyParsed = re.ReplaceTokens(body, mappingKeys);
            string subjectParsed = re.ReplaceTokens(emailSubject, mappingKeys);
            var _recipient = claimTestMail != null ? claimTestMail : data.EmailAddress;
            return new Email
            {
                IsBodyHtml = true,
                To = new List<string> { _recipient, emailRecipient },
                Subject = subjectParsed,
                Body = bodyParsed,
                FromEmail = emailFromList
            };
        }
        private bool _CheckSentData(SurveyForm data)
        {
            var isSent = false;
            using (SqlConnection connection = new SqlConnection(AFIConnectionString))
            {
                SqlCommand cmd = new SqlCommand(@"Select Id from [Survey].[ClaimsSent] Where ClaimNumber='" + data.ClaimNumber.Trim() + "'", connection);
                connection.Open();
                int id = Convert.ToInt32(cmd.ExecuteScalar());
                if (id > 0)
                {
                    isSent = true;
                }
            }

            
            return isSent;
        }

        #region Utility Methods
        public static T DataReaderMap<T>(IDataReader dr)
        {
            T obj = default(T);
            while (dr.Read())
            {
                obj = Activator.CreateInstance<T>();
                foreach (PropertyInfo prop in obj.GetType().GetProperties())
                {
                    if (!prop.CustomAttributes.Any(x => x.AttributeType.Name == "NotMappedAttribute") && !object.Equals(dr[prop.Name], DBNull.Value))
                    {
                        prop.SetValue(obj, dr[prop.Name], null);
                    }
                }
            }
            return obj;
        }
        public static List<T> DataReaderMapToList<T>(IDataReader dr)
        {
            List<T> list = new List<T>();
            T obj = default(T);
            while (dr.Read())
            {
                obj = Activator.CreateInstance<T>();
                foreach (PropertyInfo prop in obj.GetType().GetProperties())
                {

                    if (!prop.CustomAttributes.Any(x => x.AttributeType.Name == "NotMappedAttribute") && !object.Equals(dr[prop.Name], DBNull.Value))
                    {
                        prop.SetValue(obj, dr[prop.Name], null);
                    }
                }
                list.Add(obj);
            }
            return list;
        }
        #endregion
    }

    public class ClaimMail2
    {
        private readonly IEmailService _emailService;

        public ClaimMail2(IEmailService emailService)
        {
            _emailService = emailService;
        }

    }
}