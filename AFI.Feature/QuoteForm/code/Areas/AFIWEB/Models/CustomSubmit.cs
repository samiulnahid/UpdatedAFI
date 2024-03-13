using System.Linq;
using Sitecore.Diagnostics;
using Sitecore.ExperienceForms.Models;
using Sitecore.ExperienceForms.Processing;
using Sitecore.ExperienceForms.Processing.Actions;
using static System.FormattableString;
using System;
using AFI.Feature.QuoteForm.Areas.AFIWEB.Repository;
using AFI.Feature.Data.Providers;
using AFI.Feature.QuoteForm.Areas.AFIWEB.Models;
using System.Web;
using System.Configuration;
using System.Data.SqlClient;
using System.Reflection;
using System.Data;
using System.Collections.Generic;
using Sitecore.ExperienceForms.Mvc.Models.Fields;
using System.Web.Mvc;
using Sitecore.Shell.Framework.Commands.ContentEditor.Validators;
using Sitecore.Analytics.Tracking;
using AFI.Feature.QuoteForm.Areas.AFIWEB.Helpers.Configurations;
using Sitecore.Data.Items;
using System.Text.RegularExpressions;
using AFI.Feature.QuoteForm.Areas.AFIWEB.Helpers.Strings;
using AFI.Feature.QuoteForm.Areas.AFIWEB.Services;
using System.Diagnostics;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Handlers;
using AFIConstants = AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Helpers.Constants;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Repository;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.LeadGenerationForm;
using AFI.Feature.Data.DataModels;
using AFI.Feature.WebQuoteService.Models;
using AFI.Feature.Data.Repositories;
using AFI.Feature.WebQuoteService.Repositories;

namespace Sitecore.ExperienceForms.Samples.SubmitActions
{

    /// <summary>
    /// Step 01 Login
    /// </summary>
    public class SubmitLogin : SubmitActionBase<string>
    {
        private static readonly string ConnectionString = ConfigurationManager.ConnectionStrings["AFIDB"].ConnectionString;

        public SubmitLogin(ISubmitActionData submitActionData) : base(submitActionData)
        {

        }

        /// <summary>
        /// Tries to convert the specified <paramref name="value" /> to an instance of the specified target type.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="target">The target object.</param>
        /// <returns>
        /// true if <paramref name="value" /> was converted successfully; otherwise, false.
        /// </returns>
        protected override bool TryParse(string value, out string target)
        {
            target = string.Empty;
            return true;
        }

        /// <summary>
        /// Executes the action with the specified <paramref name="data" />.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="formSubmitContext">The form submit context.</param>
        /// <returns>
        ///   <c>true</c> if the action is executed correctly; otherwise <c>false</c>
        /// </returns>
        protected override bool Execute(string data, FormSubmitContext formSubmitContext)
        {
            Assert.ArgumentNotNull(formSubmitContext, nameof(formSubmitContext));
            string memberNumber = ""; string pINNumber = ""; string email = ""; string votingPeriodId = "";
            if (!formSubmitContext.HasErrors)
            {

                var txtmemberNumber = formSubmitContext.Fields.FirstOrDefault(f => f.Name.Equals("txtMemberNumber"));
                if (txtmemberNumber != null)
                {
                    memberNumber = _getValue(txtmemberNumber);
                }
                var txtpinNumber = formSubmitContext.Fields.FirstOrDefault(f => f.Name.Equals("txtPinNumber"));
                if (txtpinNumber != null)
                {
                    pINNumber = _getValue(txtpinNumber);
                }
                var txtemail = formSubmitContext.Fields.FirstOrDefault(f => f.Name.Equals("txtEmail"));
                if (txtemail != null)
                {
                    email = _getValue(txtemail);
                }
                var txtVotaingPeriod = formSubmitContext.Fields.FirstOrDefault(f => f.Name.Equals("txtVotaingPeriod"));
                if (txtVotaingPeriod != null)
                {
                    votingPeriodId = _getValue(txtVotaingPeriod);
                }

                ProxyVoteMember member = new ProxyVoteMember();

                using (SqlConnection connection = new SqlConnection(ConnectionString))
                {
                    SqlCommand command = new SqlCommand(@"Select top 1* from ProxyVote.Member Where MemberNumber='" + memberNumber + "' and PIN='" + pINNumber + "' and VotingPeriodId=" + votingPeriodId, connection);

                    try 
                    {
                        connection.Open();
                        SqlDataReader reader = command.ExecuteReader();
                        member = UtilityManager.DataReaderMap<ProxyVoteMember>(reader);
                        reader.Close();

                        if (member != null)
                        {
                            HttpContext.Current.Session["memberId"] = member.MemberId;
                            HttpContext.Current.Session["votingPeriodId"] = votingPeriodId;
                            HttpContext.Current.Session["membername"] = member.FullName;
                            HttpContext.Current.Session["memberemail"] = member.EmailAddress;
                            HttpContext.Current.Session["txt_email"] = email;
                            if (!member.Enabled) // member already voted.
                            {
                                command = new SqlCommand(@"Select mcb.MemberId,mcb.CandidateId,mcb.VotingPeriodId,mcb.Ballot,mcb.DateTimeCast from [ProxyVote].[MemberCandidateBallot] AS mcb inner join [ProxyVote].[Candidate] AS c ON mcb.CandidateId = c.CandidateId 
Where mcb.MemberId = " + member.MemberId + " And mcb.VotingPeriodId=" + member.VotingPeriodId, connection);
                                reader = command.ExecuteReader();
                                ProxyVoteMemberCandidateBallot proxyVote = new ProxyVoteMemberCandidateBallot();
                                proxyVote = UtilityManager.DataReaderMap<ProxyVoteMemberCandidateBallot>(reader);
                                reader.Close();
                                if (proxyVote != null)
                                {
                                    HttpContext.Current.Session["Ballots"] = proxyVote;
                                    HttpContext.Current.Session["memberId"] = string.Empty;
                                    HttpContext.Current.Session["votingPeriodId"] = string.Empty;
                                    string redirectUrl = GetUrlById(InternalUrl.Vote_Message_ID);
                                    HttpContext.Current.Response.Redirect(redirectUrl, true);


                                }
                            }

                            Logger.Info(Invariant($"Form {formSubmitContext.FormId} submitted successfully."), this);
                        }
                        else
                        {

                            Logger.Warn(Invariant($"Form {formSubmitContext.FormId} Please enter correct Member Number and PIN."), this);
                            return false;
                        }
                    }
                    catch (Exception e)
                    {
                        throw new Exception("Exception retrieving reviews. " + e.Message);
                    }
                    finally
                    {
                        connection.Close();
                    }
                }
                Logger.Info(Invariant($"Form {formSubmitContext.FormId} submitted successfully."), this);
            }
            else
            {
                Logger.Warn(Invariant($"Form {formSubmitContext.FormId} submitted with errors: {string.Join(", ", formSubmitContext.Errors.Select(t => t.ErrorMessage))}."), this);
            }

            return true;
        }

        public string GetUrlById(string item_ID)
        {
            string newURL = string.Empty;
            Item pageItem = Sitecore.Context.Database.GetItem(item_ID);
            newURL = Sitecore.Links.LinkManager.GetItemUrl(pageItem);
            return newURL;
        }

        private string _getValue(IViewModel field)
        {
            var property = field.GetType().GetProperty("Value");
            var data = property.GetValue(field);
            return data.ToString();
        }
    }

    /// <summary>
    /// Step 02
    /// </summary>
    public class SubmitVote : SubmitActionBase<string>
    {
        private static readonly string ConnectionString = ConfigurationManager.ConnectionStrings["AFIDB"].ConnectionString;
        private readonly IEmailService _emailService;

        public SubmitVote(ISubmitActionData submitActionData) : base(submitActionData)
        {
            _emailService = new EmailService();
        }

        protected override bool TryParse(string value, out string target)
        {
            target = string.Empty;
            return true;
        }


        protected override bool Execute(string data, FormSubmitContext formSubmitContext)
        {
            Assert.ArgumentNotNull(formSubmitContext, nameof(formSubmitContext));
            var memberId = HttpContext.Current.Session["memberId"] != null ? System.Convert.ToInt32(HttpContext.Current.Session["memberId"]) : 0;
            int votingPeriodId = HttpContext.Current.Session["votingPeriodId"] != null ? System.Convert.ToInt32(HttpContext.Current.Session["votingPeriodId"]) : 0;
            if (memberId > 0)
            {
                if (!formSubmitContext.HasErrors)
                {
                    if (formSubmitContext.Fields.Count > 0)
                    {
                        foreach (IViewModel field in formSubmitContext.Fields.Where(x => x.TemplateId == "{5B672865-55D2-413E-B699-FDFC7E732CCF}"))
                        {
                            string lblchecked = ""; int candidate_id = 0;

                            var candidate_name = ((TitleFieldViewModel)field).Title;
                            candidate_name = Regex.Replace(candidate_name, "</?(a|A).*?>", "");
                            var child_items = ((ListViewModel)field).Items;
                            foreach (var item in child_items)
                            {
                                if (item.Selected)
                                {
                                    lblchecked = item.Value;
                                }
                            }


                            using (SqlConnection connection = new SqlConnection(ConnectionString))
                            {

                                SqlCommand command = new SqlCommand(@"SELECT c.[CandidateId] FROM [ProxyVote].[Candidate] c Where c.VotingPeriodId=" + votingPeriodId + " AND c.Name='" + candidate_name + "'", connection);
                                connection.Open();
                                SqlDataReader reader = command.ExecuteReader();
                                while (reader.Read())
                                {
                                    candidate_id = reader.GetInt32(reader.GetOrdinal("CandidateId"));
                                }
                                reader.Close();
                                if (candidate_id > 0)
                                {
                                    Sitecore.Diagnostics.Log.Info($"Ballot Start", "Mehedi");
                                    ProxyVoteMemberCandidateBallot proxyVote = new ProxyVoteMemberCandidateBallot();
                                    proxyVote.Ballot = lblchecked;
                                    proxyVote.CandidateId = candidate_id;
                                    proxyVote.DateTimeCast = DateTime.Now;
                                    proxyVote.MemberId = memberId;
                                    proxyVote.VotingPeriodId = votingPeriodId;

                                    command = new SqlCommand(@"insert into [ProxyVote].[MemberCandidateBallot]([MemberId], [CandidateId], [VotingPeriodId], [Ballot], [DateTimeCast]) values(@MemberId, @CandidateId, @VotingPeriodId, @Ballot, @DateTimeCast); select scope_identity()", connection);
                                    foreach (var vote in proxyVote.GetType().GetProperties())
                                    {

                                        string name = vote.Name;
                                        var value = vote.GetValue(proxyVote, null);

                                        command.Parameters.Add(new SqlParameter("@" + name, value == null ? DBNull.Value : value));

                                    }
                                    try
                                    {
                                        command.ExecuteNonQuery();
                                    }
                                    catch (Exception e)
                                    {
                                        //throw new Exception("Exception retrieving reviews. " + e.Message);

                                    }
                                    finally
                                    {
                                        connection.Close();
                                    }
                                    Sitecore.Diagnostics.Log.Info($"Ballot End", "Mehedi");
                                }
                                else
                                {
                                    Logger.Info(Invariant($"Form {formSubmitContext.FormId} submitted successfully."), this);
                                }
                            }

                        }
                        using (SqlConnection connection = new SqlConnection(ConnectionString))
                        {
                            Sitecore.Diagnostics.Log.Info($"Vote total", "Mehedi");
                            int total = 0;
                            SqlCommand command = new SqlCommand(@"Select Count(*) Total from ProxyVote.MemberCandidateBallot where VotingPeriodId=" + votingPeriodId + " AND MemberId=" + memberId, connection);
                            connection.Open();
                            SqlDataReader reader = command.ExecuteReader();
                            while (reader.Read())
                            {
                                total = reader.GetInt32(reader.GetOrdinal("Total"));
                            }
                            reader.Close();
                            if (total > 0)
                            {
                                Sitecore.Diagnostics.Log.Info($"Update Start", "Mehedi");
                                command = new SqlCommand(@"UPDATE [ProxyVote].[Member] SET  [Enabled] =0 WHERE MemberId=" + memberId, connection);
                                try
                                {
                                    command.ExecuteNonQuery();


                                    //#region EMAIL                                    
                                    Sitecore.Diagnostics.Log.Info($"Email Start", "Mehedi");
                                    Sitecore.Data.Items.Item emailtemplate = Sitecore.Context.Database.GetItem(ProxyVoteMail.Email_ItemId);
                                    string email_Subject = emailtemplate[ProxyVoteMail.Email_Subject];
                                    string email_To = HttpContext.Current.Session["txt_email"] != null ? HttpContext.Current.Session["txt_email"].ToString() : "";
                                    string email_From = emailtemplate[ProxyVoteMail.Email_Sender];
                                    string email_Body = emailtemplate[ProxyVoteMail.Email_Body];
                                    if (!string.IsNullOrEmpty(email_To))
                                    {
                                        Sitecore.Diagnostics.Log.Info($"Email Build", "Mehedi");
                                        var email = BuildConfirmationEmail(email_From, email_To, email_Subject, email_Body);
                                        Sitecore.Diagnostics.Log.Info($"Email Send", "Mehedi");
                                        _emailService.Send(email);
                                        Sitecore.Diagnostics.Log.Info($"Email End", "Mehedi");
                                    }
                                    //#endregion

                                }
                                catch (Exception e)
                                {
                                    //  throw new Exception("Exception retrieving reviews. " + e.Message);

                                }
                                finally
                                {
                                    connection.Close();

                                }
                            }

                        }
                    }

                    Logger.Info(Invariant($"Form {formSubmitContext.FormId} submitted successfully."), this);
                }
                else
                {
                    Logger.Warn(Invariant($"Form {formSubmitContext.FormId} submitted with errors: {string.Join(", ", formSubmitContext.Errors.Select(t => t.ErrorMessage))}."), this);
                }

            }
            else
            {
                string redirectUrl = GetUrlById(InternalUrl.Vote_Message_ID);
                HttpContext.Current.Response.Redirect(redirectUrl, true);
            }

            return true;
        }

        public string GetUrlById(string item_ID)
        {
            string newURL = string.Empty;
            Item pageItem = Sitecore.Context.Database.GetItem(item_ID);
            newURL = Sitecore.Links.LinkManager.GetItemUrl(pageItem);
            return newURL;
        }

        public Email BuildConfirmationEmail(string fromEmail, string toEmail, string emailSubject, string consumerMessageBody)
        {
            var email = new Email();
            email.IsBodyHtml = true;
            email.To = new[] { toEmail };
            email.FromEmail = fromEmail;
            email.Subject = emailSubject;
            email.Body = consumerMessageBody;

            return email;
        }
        private Email ParseEmail(string emailFromList, string emailSubject, string body, string emailRecipient)
        {
            Regex re = new Regex(@"\{\{(\w+)\}\}", RegexOptions.Compiled);
            IDictionary<string, string> mappingKeys = new Dictionary<string, string>();

            string bodyParsed = re.ReplaceTokens(body, mappingKeys);
            string subjectParsed = re.ReplaceTokens(emailSubject, mappingKeys);
            return new Email
            {
                IsBodyHtml = true,
                To = new List<string> { emailRecipient },
                Subject = subjectParsed,
                Body = bodyParsed,
                FromEmail = emailFromList
            };
        }

    }

    /// <summary>
    /// Step 03
    /// </summary>
    public class SubmitComplete : SubmitActionBase<string>
    {
        private static readonly string ConnectionString = ConfigurationManager.ConnectionStrings["AFIDB"].ConnectionString;
        /// <summary>
        /// Initializes a new instance of the <see cref="LogSubmit"/> class.
        /// </summary>
        /// <param name="submitActionData">The submit action data.</param>
        public SubmitComplete(ISubmitActionData submitActionData) : base(submitActionData)
        {
        }

        /// <summary>
        /// Tries to convert the specified <paramref name="value" /> to an instance of the specified target type.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="target">The target object.</param>
        /// <returns>
        /// true if <paramref name="value" /> was converted successfully; otherwise, false.
        /// </returns>
        protected override bool TryParse(string value, out string target)
        {
            target = string.Empty;
            return true;
        }

        /// <summary>
        /// Executes the action with the specified <paramref name="data" />.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="formSubmitContext">The form submit context.</param>
        /// <returns>
        ///   <c>true</c> if the action is executed correctly; otherwise <c>false</c>
        /// </returns>
        protected override bool Execute(string data, FormSubmitContext formSubmitContext)
        {
            Assert.ArgumentNotNull(formSubmitContext, nameof(formSubmitContext));
            var memberId = HttpContext.Current.Session["memberId"] != null ? System.Convert.ToInt32(HttpContext.Current.Session["memberId"]) : 0;
            if (memberId > 0)
            {
                if (!formSubmitContext.HasErrors)
                {
                    if (formSubmitContext.Fields.Count > 0)
                    {


                        using (SqlConnection connection = new SqlConnection(ConnectionString))
                        {

                            SqlCommand command = new SqlCommand(@"SELECT [MemberId] , [MemberNumber] , [PIN] , [VotingPeriodId] , [Enabled] , [EmailAddress] , [FullName] , [ResidentialOccupied] , [ResidentialDwelling] , [Renters] , [Flood] , [Life] , [PersonalLiabilityRenters] , [PersonalLiabilityCatastrophy] , [Auto] , [RV] , [Watercraft] , [Motorcycle] , [Supplemental] , [AnnualReport] , [StatutoryFinancialStatements] , [AdditionalInfo] , [MobileHome] , [PetHealth] , [Business] , [LongTermCare] , [MailFinancials] , [EmailFinancials] FROM [AFIWEB_BETA].[ProxyVote].[Member]
WHERE MemberId=" + memberId, connection);
                            connection.Open();
                            SqlDataReader reader = command.ExecuteReader();
                            ProxyVoteMember member = UtilityManager.DataReaderMap<ProxyVoteMember>(reader);
                            reader.Close();

                            foreach (IViewModel field in formSubmitContext.Fields.Where(x => x.TemplateId == "{5B672865-55D2-413E-B699-FDFC7E732CCF}"))
                            {

                                var child_items = ((ListViewModel)field).Items;
                                foreach (var item in child_items)
                                {
                                    // Homeowners, Umbrella, Motorhome, Valuable Item

                                    if (item.Value == member.ResidentialDwelling.ToString())
                                    {
                                        member.ResidentialDwelling = item.Selected;
                                    }
                                    if (item.Value == member.Renters.ToString())
                                    {
                                        member.Renters = item.Selected;
                                    }
                                    if (item.Value == member.Flood.ToString())
                                    {
                                        member.Flood = item.Selected;
                                    }
                                    if (item.Value == member.Business.ToString())
                                    {
                                        member.Business = item.Selected;
                                    }

                                    if (item.Value == member.Auto.ToString())
                                    {
                                        member.Auto = item.Selected;
                                    }

                                    if (item.Value == member.Watercraft.ToString())
                                    {
                                        member.Watercraft = item.Selected;
                                    }
                                    if (item.Value == member.Motorcycle.ToString())
                                    {
                                        member.Motorcycle = item.Selected;
                                    }

                                    if (item.Value == member.AnnualReport.ToString())
                                    {
                                        member.AnnualReport = item.Selected;
                                    }
                                    if (item.Value == member.StatutoryFinancialStatements.ToString())
                                    {
                                        member.StatutoryFinancialStatements = item.Selected;
                                    }
                                    if (item.Value == member.MobileHome.ToString())
                                    {
                                        member.MobileHome = item.Selected;
                                    }
                                    if (item.Value == member.PetHealth.ToString())
                                    {
                                        member.PetHealth = item.Selected;
                                    }

                                }
                            }

                            member.Enabled = false;

                            command = new SqlCommand(@"UPDATE [ProxyVote].[Member] SET [MemberNumber] = @MemberNumber , [PIN] = @PIN , [VotingPeriodId] = @VotingPeriodId , [Enabled] = @Enabled , [EmailAddress] = @EmailAddress , [FullName] =@FullName , [ResidentialOccupied] = @ResidentialOccupied , [ResidentialDwelling] = @ResidentialDwelling , [Renters] = @Renters , [Flood] = @Flood , [Life] = @Life , [PersonalLiabilityRenters] = @PersonalLiabilityRenters , [PersonalLiabilityCatastrophy] = @PersonalLiabilityCatastrophy , [Auto] = @Auto , [RV] = @RV , [Watercraft] = @Watercraft , [Motorcycle] = @Motorcycle , [Supplemental] = @Supplemental , [AnnualReport] = @AnnualReport , [StatutoryFinancialStatements] = @StatutoryFinancialStatements , [MobileHome] = @MobileHome ,
[PetHealth] = @PetHealth , [Business] = @Business , [LongTermCare] = @LongTermCare , [MailFinancials] = @MailFinancials , [EmailFinancials] = @EmailFinancials WHERE MemberId=" + memberId, connection);
                            foreach (var item in member.GetType().GetProperties())
                            {

                                string name = item.Name;
                                var value = item.GetValue(member, null);

                                command.Parameters.Add(new SqlParameter("@" + name, value == null ? DBNull.Value : value));

                            }
                            try
                            {
                                command.ExecuteNonQuery();
                            }
                            catch (Exception e)
                            {
                                throw new Exception("Exception retrieving reviews. " + e.Message);

                            }
                            finally
                            {
                                connection.Close();

                                Logger.Info(Invariant($"Form {formSubmitContext.FormId} submitted successfully."), this);

                            }
                        }
                    }

                }
                else
                {
                    Logger.Warn(Invariant($"Form {formSubmitContext.FormId} submitted with errors: {string.Join(", ", formSubmitContext.Errors.Select(t => t.ErrorMessage))}."), this);
                }
            }
            else
            {
                string redirectUrl = GetUrlById(InternalUrl.Vote_Message_ID);
                HttpContext.Current.Response.Redirect(redirectUrl, true);
            }


            return true;
        }


        private string GetUrlById(string item_ID)
        {
            string newURL = string.Empty;
            Item pageItem = Sitecore.Context.Database.GetItem(item_ID);
            newURL = Sitecore.Links.LinkManager.GetItemUrl(pageItem);
            return newURL;
        }
    }

    public class SubmitRequestInfo : SubmitActionBase<string>
    {
        private readonly IEmailService _emailService;
        public SubmitRequestInfo(ISubmitActionData submitActionData) : base(submitActionData)
        {
            _emailService = new EmailService();
        }

        /// <summary>
        /// Tries to convert the specified <paramref name="value" /> to an instance of the specified target type.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="target">The target object.</param>
        /// <returns>
        /// true if <paramref name="value" /> was converted successfully; otherwise, false.
        /// </returns>
        protected override bool TryParse(string value, out string target)
        {
            target = string.Empty;
            return true;
        }

        /// <summary>
        /// Executes the action with the specified <paramref name="data" />.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="formSubmitContext">The form submit context.</param>
        /// <returns>
        ///   <c>true</c> if the action is executed correctly; otherwise <c>false</c>
        /// </returns>
        protected override bool Execute(string data, FormSubmitContext formSubmitContext)
        {
            Assert.ArgumentNotNull(formSubmitContext, nameof(formSubmitContext));
            string name = ""; string street = ""; string city = ""; string state = ""; string zip = ""; string email = "";
            if (!formSubmitContext.HasErrors)
            {
                Sitecore.Diagnostics.Log.Info($"Read form data Start", "Mehedi");
                var txtName = formSubmitContext.Fields.FirstOrDefault(f => f.Name.Equals("Name"));
                if (txtName != null)
                {
                    name = _getValue(txtName);
                }
                var txtemail = formSubmitContext.Fields.FirstOrDefault(f => f.Name.Equals("Email"));
                if (txtemail != null)
                {
                    email = _getValue(txtemail);
                }
                var txtStreet = formSubmitContext.Fields.FirstOrDefault(f => f.Name.Equals("Street"));
                if (txtStreet != null)
                {
                    street = _getValue(txtStreet);
                }

                var txtCity = formSubmitContext.Fields.FirstOrDefault(f => f.Name.Equals("City"));
                if (txtCity != null)
                {
                    city = _getValue(txtCity);
                }

                var txtState = formSubmitContext.Fields.FirstOrDefault(f => f.Name.Equals("State"));
                if (txtState != null)
                {
                    state = _getValue(txtState);
                }

                var txtZip = formSubmitContext.Fields.FirstOrDefault(f => f.Name.Equals("Zip"));
                if (txtZip != null)
                {
                    zip = _getValue(txtZip);
                }
                string lblchecked = "";
                foreach (IViewModel field in formSubmitContext.Fields.Where(x => x.TemplateId == "{5B672865-55D2-413E-B699-FDFC7E732CCF}"))
                {

                    var item_name = ((TitleFieldViewModel)field).Title;
                    var child_items = ((ListViewModel)field).Items;
                    foreach (var item in child_items)
                    {
                        if (item.Selected)
                        {
                            lblchecked += item.Value;
                        }
                    }
                }
                Sitecore.Diagnostics.Log.Info($"Read form Data End", "Mehedi");

                #region EMAIL                                    
                Sitecore.Diagnostics.Log.Info($"Email Start", "Mehedi");
                Sitecore.Data.Items.Item emailtemplate = Sitecore.Context.Database.GetItem(SC.Request_Info_Email_ItemId);
                string email_Subject = emailtemplate[SC.Email_Subject];
                string email_To = emailtemplate[SC.Email_Recipients];
                string email_From = emailtemplate[SC.Email_Sender];
                string email_Body = emailtemplate[SC.Email_Body];
                if (!string.IsNullOrEmpty(email_To))
                {
                    Regex re = new Regex(@"\{\{(\w+)\}\}", RegexOptions.Compiled);
                    IDictionary<string, string> mappingKeys = new Dictionary<string, string>();

                    mappingKeys.Add("fullName", name);
                    mappingKeys.Add("email", email);
                    mappingKeys.Add("street", street);
                    mappingKeys.Add("city", city);
                    mappingKeys.Add("state", state);
                    mappingKeys.Add("zip", zip);
                    mappingKeys.Add("product", lblchecked);
                    string bodyParsed = re.ReplaceTokens(email_Body, mappingKeys);
                    Sitecore.Diagnostics.Log.Info($"Email Build", "Mehedi");
                    var send_email = BuildConfirmationEmail(email_From, email_To, email_Subject, bodyParsed);
                    Sitecore.Diagnostics.Log.Info($"Email Send", "Mehedi");
                    _emailService.Send(send_email);
                    Sitecore.Diagnostics.Log.Info($"Email End", "Mehedi");
                }


                #endregion

                Logger.Info(Invariant($"Form {formSubmitContext.FormId} submitted successfully."), this);
            }
            else
            {
                Logger.Warn(Invariant($"Form {formSubmitContext.FormId} submitted with errors: {string.Join(", ", formSubmitContext.Errors.Select(t => t.ErrorMessage))}."), this);
            }

            return true;
        }

        public string GetUrlById(string item_ID)
        {
            string newURL = string.Empty;
            Item pageItem = Sitecore.Context.Database.GetItem(item_ID);
            newURL = Sitecore.Links.LinkManager.GetItemUrl(pageItem);
            return newURL;
        }

        private string _getValue(IViewModel field)
        {
            var property = field.GetType().GetProperty("Value");
            var data = property.GetValue(field);
            return data.ToString();
        }
        public Email BuildConfirmationEmail(string fromEmail, string toEmail, string emailSubject, string consumerMessageBody)
        {
            var email = new Email();
            email.IsBodyHtml = true;
            email.To = new[] { toEmail };
            email.FromEmail = fromEmail;
            email.Subject = emailSubject;
            email.Body = consumerMessageBody;

            return email;
        }
        private Email ParseEmail(string emailFromList, string emailSubject, string body, string emailRecipient)
        {
            Regex re = new Regex(@"\{\{(\w+)\}\}", RegexOptions.Compiled);
            IDictionary<string, string> mappingKeys = new Dictionary<string, string>();

            string bodyParsed = re.ReplaceTokens(body, mappingKeys);
            string subjectParsed = re.ReplaceTokens(emailSubject, mappingKeys);
            return new Email
            {
                IsBodyHtml = true,
                To = new List<string> { emailRecipient },
                Subject = subjectParsed,
                Body = bodyParsed,
                FromEmail = emailFromList
            };
        }
    }

    public class SubmitLead : SubmitActionBase<string>
    {
       
        private readonly IEmailService _emailService;
        private readonly IDbConnectionProvider _dbConnectionProvider;
        //  private readonly IAFIFormsSentToRepository _afiFormsSentToRepository;
        private readonly IPartnerAdvisorRepository _partnerAdvisorRepository;
        private readonly IAFIFormsMapRepository _aFIFormsMapRepository;

        public SubmitLead(ISubmitActionData submitActionData) : base(submitActionData)
        {
            _dbConnectionProvider = new DbConnectionProvider();
            _emailService = new EmailService();
            // _afiFormsSentToRepository = new AFIFormsSentToRepository(_dbConnectionProvider);
            _partnerAdvisorRepository = new PartnerAdvisorRepository();
            _aFIFormsMapRepository = new AFIFormsMapRepository(_dbConnectionProvider);
        }

        /// <summary>
        /// Tries to convert the specified <paramref name="value" /> to an instance of the specified target type.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="target">The target object.</param>
        /// <returns>
        /// true if <paramref name="value" /> was converted successfully; otherwise, false.
        /// </returns>
        protected override bool TryParse(string value, out string target)
        {
            target = string.Empty;
            return true;
        }

        /// <summary>
        /// Executes the action with the specified <paramref name="data" />.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="formSubmitContext">The form submit context.</param>
        /// <returns>
        ///   <c>true</c> if the action is executed correctly; otherwise <c>false</c>
        /// </returns>
        protected override bool Execute(string data, FormSubmitContext formSubmitContext)
        {
            Assert.ArgumentNotNull(formSubmitContext, nameof(formSubmitContext));
            string firstname = ""; string lastname = ""; string address = ""; string city = ""; string state = ""; string zip = ""; string email = ""; string phone = "";
            if (!formSubmitContext.HasErrors)
            {
                Sitecore.Diagnostics.Log.Info($"Read form data Start", "Mehedi");
                var txtfirstname = formSubmitContext.Fields.FirstOrDefault(f => f.Name.Equals("firstname"));
                if (txtfirstname != null)
                {
                    firstname = _getValue(txtfirstname);
                }
                var txtlastname = formSubmitContext.Fields.FirstOrDefault(f => f.Name.Equals("lastname"));
                if (txtlastname != null)
                {
                    lastname = _getValue(txtlastname);
                }
                var txtemail = formSubmitContext.Fields.FirstOrDefault(f => f.Name.Equals("emailaddress"));
                if (txtemail != null)
                {
                    email = _getValue(txtemail);
                }
                var txtPhone = formSubmitContext.Fields.FirstOrDefault(f => f.Name.Equals("phone"));
                if (txtPhone != null)
                {
                    phone = _getValue(txtPhone);
                }
                var txtaddress = formSubmitContext.Fields.FirstOrDefault(f => f.Name.Equals("address"));
                if (txtaddress != null)
                {
                    address = _getValue(txtaddress);
                }

                var txtCity = formSubmitContext.Fields.FirstOrDefault(f => f.Name.Equals("city"));
                if (txtCity != null)
                {
                    city = _getValue(txtCity);
                }

                var txtState = formSubmitContext.Fields.FirstOrDefault(f => f.Name.Equals("state"));
                if (txtState != null)
                {              
                    state = GetDDLValue(txtState);
                }

                var txtZip = formSubmitContext.Fields.FirstOrDefault(f => f.Name.Equals("zip"));
                if (txtZip != null)
                {
                    zip = _getValue(txtZip);
                }


                Sitecore.Diagnostics.Log.Info($"Read form Data End", "Mehedi");


                Stopwatch sw = new Stopwatch();
                sw.Start();
                try
                {
                    var _coverageType = _aFIFormsMapRepository.GetByCoverageType(CoverageTypes.LeadGeneration);
                    Sitecore.Diagnostics.Log.Info($"Load AFIForm Data", "Mehedi");
                    AFIForm afiForm = new AFIForm();
                    afiForm.Address1 = address;
                    afiForm.City = city;
                    afiForm.CoverageTypeId = _coverageType.CoverageTypeID;
                    afiForm.CreatedDate = DateTime.Now;
                    afiForm.CreationUser = email;
                    afiForm.CurrentlyAutoInsured = true;
                    afiForm.DisplayInformation = firstname + " " + lastname;
                    afiForm.Eligibility = "";
                    afiForm.Email = email;
                    afiForm.Expidite = false;
                    afiForm.FormTypeId = _coverageType.FormTypeID;
                    afiForm.IsBusinessTest = false;
                    afiForm.IsCondo = false;
                    afiForm.IsFinished = true;
                    afiForm.IsFirstCommand = false;
                    afiForm.IsInterested = false;
                    afiForm.IsMobileManufactured = false;
                    afiForm.IsPossibleDup = false;
                    afiForm.IsRental = false;
                    afiForm.IsSlt = false;
                    afiForm.IsTest = false;
                    afiForm.IsVacant = false;
                    afiForm.NeedsUnderwriting = false;
                    afiForm.Priority = 5;// static
                    afiForm.QuoteId = " ";// static
                    afiForm.ReceivedPremium = false;
                    afiForm.StateAbbrv = state;
                    afiForm.UpdateUser = email;
                    afiForm.UpdatedDate = DateTime.Now;
                    Sitecore.Diagnostics.Log.Info($"End Load AFIForm Data", "Mehedi");

                   // sw.Restart();
                    //Sitecore.Diagnostics.Log.Info($"Load AFIForm Data", "Mehedi");
                    //AFIFormsSentTo sentTo = new AFIFormsSentTo();
                    //sentTo.CreateDate = DateTime.Now;
                    //sentTo.QuoteID = "202156";
                    //sentTo.QuoteType = CoverageTypes.LeadGeneration;
                    //_afiFormsSentToRepository.Create(sentTo);
                    //Sitecore.Diagnostics.Log.Info($"Load AFIForm Data", "Mehedi");
                   // sw.Stop();
                   // Sitecore.Diagnostics.Log.Info("WS ProcessSubmitInformationToAFI Create afiFormsSentToRepository time elapsed" + sw.ElapsedMilliseconds, AFIConstants.QUOTE_FORMS_LOGGER_NAME);

                    sw.Restart();
                    _partnerAdvisorRepository.InsertAFIForm(afiForm);
                  
                }
                catch (Exception ex)
                {

                }
                sw.Stop();
                Sitecore.Diagnostics.Log.Info("WS ProcessSubmitInformationToAFI InsertAFIForm partnerAdvisorRepository time elapsed" + sw.ElapsedMilliseconds, AFIConstants.QUOTE_FORMS_LOGGER_NAME);

                try
                {
                    #region EMAIL                                    
                    Sitecore.Diagnostics.Log.Info($"Lead Email Start", "Mehedi");
                    Sitecore.Data.Items.Item emailtemplate = Sitecore.Context.Database.GetItem(LeadGenMail.Email_ItemId);
                    Sitecore.Diagnostics.Log.Info($"Mehedi Email Item", "Mehedi");
                    string admin_email_Subject = emailtemplate[LeadGenMail.Admin_Email_Subject];
                    string email_To = emailtemplate[LeadGenMail.Admin_Email_Recipients];
                    string admin_email_From = emailtemplate[LeadGenMail.Admin_Email_Sender];
                    string admin_email_Body = emailtemplate[LeadGenMail.Admin_Email_Body];
                    string client_email_Subject = emailtemplate[LeadGenMail.Client_Confirmation_Email_Subject];
                    string client_email_Sender = emailtemplate[LeadGenMail.Client_Confirmation_Email_Sender];
                    string client_email_Body = emailtemplate[LeadGenMail.Client_Confirmation_Email_Body];
                    if (!string.IsNullOrEmpty(email))
                    {
                        Sitecore.Diagnostics.Log.Info($"Mehedi Email Map start", "Mehedi");
                        Regex re = new Regex(@"\{\{(\w+)\}\}", RegexOptions.Compiled);
                        IDictionary<string, string> mappingKeys = new Dictionary<string, string>();

                        mappingKeys.Add("firstname", firstname);
                        mappingKeys.Add("lastname", lastname);
                        mappingKeys.Add("email", email);
                        mappingKeys.Add("phone", phone);
                        mappingKeys.Add("address", address);
                        mappingKeys.Add("city", city);
                        mappingKeys.Add("state", state);
                        mappingKeys.Add("zip", zip);
                        mappingKeys.Add("coveragetype", CoverageTypes.LeadGeneration);

                        if(!string.IsNullOrEmpty(admin_email_Subject) && !string.IsNullOrEmpty(admin_email_Body) && !string.IsNullOrEmpty(email_To))
                        {
                            string bodyParsed = re.ReplaceTokens(admin_email_Body, mappingKeys);
                            Sitecore.Diagnostics.Log.Info($"Mehedi Email Map end", "Mehedi");
                            Sitecore.Diagnostics.Log.Info($"Admin Email Build", "Mehedi");
                            var send_email = BuildConfirmationEmail(admin_email_From, email_To, admin_email_Subject, bodyParsed);
                            Sitecore.Diagnostics.Log.Info($"Admin Email Send", "Mehedi");
                            _emailService.Send(send_email);
                            Sitecore.Diagnostics.Log.Info($"Admin Email End", "Mehedi");
                        }
                      
                        // Send to Client
                        string subject_client = re.ReplaceTokens(client_email_Subject, mappingKeys);
                        string body_client = re.ReplaceTokens(client_email_Body, mappingKeys);
                        Sitecore.Diagnostics.Log.Info($"Client Email Build", "Mehedi");
                        var send_email_client = BuildConfirmationEmail(client_email_Sender, email, subject_client, body_client);
                        Sitecore.Diagnostics.Log.Info($"Client Email Send", "Mehedi");
                        _emailService.Send(send_email_client);
                        Sitecore.Diagnostics.Log.Info($"Client Email End", "Mehedi");
                    }


                    #endregion
                }
                catch (Exception ex)
                {
                    Sitecore.Diagnostics.Log.Info($"Mehedi email Exception "+ex.Message, "Mehedi");

                }
                

                Logger.Info(Invariant($"Form {formSubmitContext.FormId} submitted successfully."), this);
            }
            else
            {
                Logger.Warn(Invariant($"Form {formSubmitContext.FormId} submitted with errors: {string.Join(", ", formSubmitContext.Errors.Select(t => t.ErrorMessage))}."), this);
            }

            return true;
        }

        public string GetUrlById(string item_ID)
        {
            string newURL = string.Empty;
            Item pageItem = Sitecore.Context.Database.GetItem(item_ID);
            newURL = Sitecore.Links.LinkManager.GetItemUrl(pageItem);
            return newURL;
        }

        private string _getValue(IViewModel field)
        {
            var property = field.GetType().GetProperty("Value");
            var data = property.GetValue(field);
            return data.ToString();
        }
        private static IViewModel GetFieldById(Guid id, IList<IViewModel> fields)
        {
            return fields.FirstOrDefault(f => Guid.Parse(f.ItemId) == id);
        }

        private static string GetDDLValue(object field)
        {

            if (field is DropDownListViewModel)
            {
                DropDownListViewModel dropdownField = field as DropDownListViewModel;
                string selectedtext = dropdownField.Value.FirstOrDefault();
                return selectedtext;
            }
            // Similar condition you can apply for checkbox using CheckBoxViewModel

            return field?.GetType().GetProperty("Value")?.GetValue(field, null)?.ToString() ?? string.Empty;
        }
        public Email BuildConfirmationEmail(string fromEmail, string toEmail, string emailSubject, string consumerMessageBody)
        {
            var email = new Email();
            email.IsBodyHtml = true;
            email.To = new[] { toEmail };
            email.FromEmail = fromEmail;
            email.Subject = emailSubject;
            email.Body = consumerMessageBody;

            return email;
        }
        private Email ParseEmail(string emailFromList, string emailSubject, string body, string emailRecipient)
        {
            Regex re = new Regex(@"\{\{(\w+)\}\}", RegexOptions.Compiled);
            IDictionary<string, string> mappingKeys = new Dictionary<string, string>();

            string bodyParsed = re.ReplaceTokens(body, mappingKeys);
            string subjectParsed = re.ReplaceTokens(emailSubject, mappingKeys);
            return new Email
            {
                IsBodyHtml = true,
                To = new List<string> { emailRecipient },
                Subject = subjectParsed,
                Body = bodyParsed,
                FromEmail = emailFromList
            };
        }
    }
}