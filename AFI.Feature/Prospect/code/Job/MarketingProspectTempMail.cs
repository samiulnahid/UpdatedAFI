using System;
using System.Linq;
using System.Text;
using System.IO;
using Sitecore.Data.Items;
using System.Configuration;
using System.Data.SqlClient;
using AFI.Feature.Prospect.Constants;
using System.Net.Mail;
using System.Net;
using AFI.Foundation.Helper.Models;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AFI.Feature.Prospect.Job
{
    public class MarketingProspectTempMail
    {
        
        private static readonly string ConnectionString = ConfigurationManager.ConnectionStrings["AFIDB"].ConnectionString;

        public void Execute(Item[] items, Sitecore.Tasks.CommandItem command, Sitecore.Tasks.ScheduleItem schedule)
        {
            Sitecore.Diagnostics.Log.Info("Ip Logs >> scheduled task is being run!", this);

            //Sitecore.Data.Items.Item emailtemplate = Sitecore.Context.ContentDatabase.GetItem(Template.MarketingProspectMail.Email_ItemId);
            //string sender = emailtemplate[Template.MarketingProspectMail.Sender];

            int totalCount = 0;
            
            using (SqlConnection connection = new SqlConnection(ConnectionString))
            {

                try
                {

                    string sqlQuery = "SELECT COUNT(*) FROM [dbo].[AFI_Marketing_Prospect_Temp] WHERE 1 = 1 AND IsSynced = 'false' AND IsValid = 'true' AND IsBlockCountry = 'false'";

                    connection.Open();

                    using (SqlCommand cmd = new SqlCommand(sqlQuery, connection))
                    {
                        
                        object result = cmd.ExecuteScalar();

                        // If the result is not null and can be converted to an integer, set totalCount
                        if (result != null && int.TryParse(result.ToString(), out totalCount))
                        {
                            Sitecore.Diagnostics.Log.Info("Marketing Prospect Temp Mail >> Count Total Valid Temp Data", this);
                        }
                        else
                        {
                            totalCount = -1;
                            Sitecore.Diagnostics.Log.Info("Marketing Prospect Temp Mail >>No Valid Temp Data Found", this);
                        }
                        
                    }

                }
                catch (Exception ex)
                {
                    Sitecore.Diagnostics.Log.Info("Marketing Prospect Temp Mail >> Exception " + ex, this);

                }
                finally
                {
                    connection.Close();
                    Sitecore.Diagnostics.Log.Info("Marketing Prospect Temp Mail >>End Count of Valid Temp Data", this);
                }

            }

            if (totalCount > 0)
            {
                #region EMAIL   
                
                Sitecore.Data.Items.Item emailtemplate = Sitecore.Context.ContentDatabase.GetItem(Template.MarketingProspectMail.Email_ItemId);
                try
                {
                    // EMAIL 

                    string subject = emailtemplate[Template.MarketingProspectMail.Subject];
                    string body = emailtemplate[Template.MarketingProspectMail.Body];
                    string recipients = emailtemplate[Template.MarketingProspectMail.Recipients];
                    string sender = emailtemplate[Template.MarketingProspectMail.Sender];

                    Sitecore.Diagnostics.Log.Info($"Marketing Prospect Temp Email Send", "Mehedi");
                   
                    try
                    {
                        var email_client = ParseEmail(sender, subject, body, recipients, totalCount);
                        Sitecore.Diagnostics.Log.Info($"Marketing Prospect Temp Email Complate", "Mehedi");
                        var isSend = emailService(email_client);
                        // var isSend =true;
                        Sitecore.Diagnostics.Log.Info($"Survay email Send error ", "Mehedi");
                        if (isSend)
                        {
                            Sitecore.Diagnostics.Log.Info($"Survay email Send error ", "Mehedi");
                        }

                    }
                    catch (Exception ex)
                    {
                        Sitecore.Diagnostics.Log.Info($"Survay email Send error" + ex.Message, "Mehedi");
                    }

                }
                catch (Exception ex)
                {
                    Sitecore.Diagnostics.Log.Info($"Survay email Send error " + ex.Message, "Mehedi");
                }
                #endregion

               #region Custom Email

                //string email = "samiulislam212421@gmail.com";
                //string message = "messageTextBox.Text";

                //if (!string.IsNullOrWhiteSpace(email) && !string.IsNullOrWhiteSpace(message))
                //{
                //    //try
                //    //{
                //        // Create and send the email
                //        MailMessage mail = new MailMessage();
                //        mail.From = new MailAddress("shakilhasan5769@gmail.com");
                //        mail.To.Add(email);
                //        mail.Subject = "Message from your ASP.NET Web Forms App";
                //        mail.Body = message;

                //        SmtpClient smtpClient = new SmtpClient("smtp.gmail.com", 587);
                //        smtpClient.UseDefaultCredentials = false;
                //        smtpClient.Credentials = new System.Net.NetworkCredential("shakilhasan5769@gmail.com", "shakil212421");
                //        smtpClient.EnableSsl = true;

                //        smtpClient.Send(mail);

                //        // Optionally, redirect to a thank you page after sending the email
                //        //Response.Redirect("ThankYou.aspx");
                //    //}
                //    //catch (Exception ex)
                //    //{
                //    //    // Handle any exceptions (e.g., display error message)
                //    //    //Response.Write("An error occurred: " + ex.Message);
                //    //}
                //}
                //else
                //{
                //    // Handle validation errors (e.g., display error message)
                //    //Response.Write("Please fill in both email and message fields.");
                //}

                #endregion
            }


        }

        private Email ParseEmail(string emailFromList, string emailSubject, string body, string emailRecipient, int count)
        {

            Regex re = new Regex(@"\{\{(\w+)\}\}", RegexOptions.Compiled);

            string bodyParsed = $"Total {count} valid item" + body ;
            string subjectParsed = emailSubject;
            var _recipient = emailRecipient != null ? emailRecipient : "test@gmail.com";
            return new Email
            {
                IsBodyHtml = true,
                To = new List<string> { _recipient, emailRecipient },
                Subject = subjectParsed,
                Body = bodyParsed,
                FromEmail = emailFromList
            };
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
                Sitecore.Diagnostics.Log.Info($"Survay email Send error " + ex.Message, "Mehedi");
            }
            return true;

        }

    }
}