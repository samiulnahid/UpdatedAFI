
using Newtonsoft.Json;
using Sitecore.Common.HttpClient;
using Sitecore.Data.Items;
using Sitecore.DependencyInjection;
using Sitecore.Diagnostics;
using Sitecore.Xml.Patch;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.UI.WebControls;
using Sitecore.Data;
using AFI.Feature.SitecoreSend.Models;

namespace AFI.Feature.SitecoreSend.Job
{
    public class ImportContactMarketingDBtoMoosend
    {
        public static string api_key = string.Empty;
        public static string api_url = string.Empty;
        public static string dedicated_listId = string.Empty;
        public static string welcome_listid = string.Empty;
        public static string other_listid = string.Empty;

        public void MoosendSettingItem()
        {
            Item moosendSetting = Sitecore.Context.ContentDatabase.GetItem(Constant.MoosendSetting.MoosendSettingId);
            if (moosendSetting != null)
            {
                api_key = GetFieldValue(moosendSetting, Constant.MoosendSetting.Fields.APIKEY);
                api_url = GetFieldValue(moosendSetting, Constant.MoosendSetting.Fields.APIURL);
                dedicated_listId = GetFieldValue(moosendSetting, Constant.MoosendSetting.Fields.DedicatedListID);
                welcome_listid = GetFieldValue(moosendSetting, Constant.MoosendSetting.Fields.WelcomeListID);
                other_listid = GetFieldValue(moosendSetting, Constant.MoosendSetting.Fields.OtherListID);
            }
            baseAddress = new Uri(api_url);
            SitecoreSendApiKey = api_key;
            emailListID = welcome_listid;
        }
        private string GetFieldValue(Item item, ID fieldId)
        {
            var field = item.Fields[fieldId];
            return field != null ? field.Value : String.Empty;
        }


        private Uri baseAddress;
        private string SitecoreSendApiKey;
        private string emailListID;
        private static readonly string connectionString = ConfigurationManager.ConnectionStrings["MarketingDB"].ConnectionString;

        public void Execute(Item[] items, Sitecore.Tasks.CommandItem command, Sitecore.Tasks.ScheduleItem schedule)
        {
            MoosendSettingItem();
            Sitecore.Diagnostics.Log.Info("AFI Import Contact Sitecore scheduled task is being run!", this);

            List<MarketingContact> dataList = new List<MarketingContact>();

            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                SqlCommand cmd = new SqlCommand(@"WITH RankedCarriers AS ( SELECT mc.MemberNumber, mc.FirstName, mc.LastName, mc.Salutation, mc.Email, mc.AgentTitle, mc.AgentName, mc.AgentEmail, mc.AgentPhone, mc.AgentExt, c.DateUsedInQuery, c.CarrierName, c.PolicyCarrier, c.CarrierClaimsEmail, c.CarrierCode, c.CarrierURL, c.CarrierURLUserFriendly, c.CarrierServiceBillingPhone, c.CarrierClaimsPhone, c.CarrierRoadsidePhone, ROW_NUMBER() OVER (PARTITION BY mc.MemberNumber ORDER BY mc.FirstName) AS RowNum FROM [SiteCorpWelcomeJourney].[vwMemberWithIssuingAgentMaxDateResult] mc JOIN [SiteCorpWelcomeJourney].[vwMembersCarriers] c ON mc.MemberNumber = c.MemberNumber WHERE c.CarrierName IS NOT NULL ) SELECT MemberNumber, FirstName, LastName, Salutation, Email, AgentTitle, AgentName, AgentEmail, AgentPhone, AgentExt,DateUsedInQuery, CarrierName, CarrierClaimsEmail, CarrierURL, CarrierCode, PolicyCarrier, CarrierURLUserFriendly, CarrierServiceBillingPhone, CarrierClaimsPhone, CarrierRoadsidePhone FROM RankedCarriers WHERE RowNum = 1;
", connection);
                connection.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        MarketingContact model = new MarketingContact();
                        model.MemberNumber = reader["MemberNumber"].ToString();
                        model.FirstName = reader["FirstName"].ToString();
                        model.LastName = reader["LastName"].ToString();
                        model.Salutation = reader["Salutation"].ToString();
                        model.Email = reader["Email"].ToString();
                        model.AgentTitle = reader["AgentTitle"].ToString();
                        model.AgentName = reader["AgentName"].ToString();
                        model.AgentEmail = reader["AgentEmail"].ToString();
                        model.AgentPhone = reader["AgentPhone"].ToString();
                        model.AgentExt = reader["AgentExt"].ToString();
                        model.PolicyEffectiveDate = reader["DateUsedInQuery"].ToString();
                        model.PolicyCarrier = reader["PolicyCarrier"].ToString();
                        model.CarrierCode = reader["CarrierCode"].ToString();
                        model.CarrierName = reader["CarrierName"].ToString();
                        model.CarrierURL = reader["CarrierURL"].ToString();
                        model.CarrierURLUserFriendly = reader["CarrierURLUserFriendly"].ToString();
                        model.CarrierServiceBillingPhone = reader["CarrierServiceBillingPhone"].ToString();
                        string cliems = reader["CarrierClaimsPhone"].ToString();
                        if (!string.IsNullOrEmpty(reader["CarrierClaimsEmail"].ToString()))
                        {
                            cliems = reader["CarrierClaimsPhone"].ToString() + " or " + reader["CarrierClaimsEmail"].ToString();
                        }
                        model.CarrierClaimsPhone = cliems;
                        model.CarrierRoadsidePhone = reader["CarrierRoadsidePhone"].ToString();

                        dataList.Add(model);
                    }

                    reader.Close();
                }


                Sitecore.Diagnostics.Log.Info("AFI Import Contact start Map> ", "Marketing DB Connection close" + this);
                connection.Close();

                if (dataList != null)
                {
                    Sitecore.Diagnostics.Log.Info("AFI Import Contact start Map> ", "Marketing DB " + this);

                    #region Single Send
                    // Single send
                    //foreach (MarketingContact item in dataList)
                    //{
                    //    try
                    //    {


                    //        var customFields = new List<string>
                    //            {
                    //              $"MemberNumber={item.MemberNumber}",
                    //                $"FirstName={item.FirstName}",
                    //                $"LastName={item.LastName}",
                    //                $"Salutation={item.Salutation}",
                    //                $"AgentTitle={item.AgentTitle}",
                    //                $"AgentName={item.AgentName}",
                    //                $"AgentEmail={item.AgentEmail}",
                    //                $"AgentPhone={item.AgentPhone}",
                    //                $"AgentExt={item.AgentExt}",
                    //                $"PolicyEffectiveDate={item.PolicyEffectiveDate}",
                    //                $"PolicyCarrier={item.PolicyCarrier}",
                    //                $"CarrierCode={item.CarrierCode}",
                    //                $"CarrierName={item.CarrierName}",
                    //                $"CarrierURL={item.CarrierURL}",
                    //                $"CarrierServiceBillingPhone={item.CarrierServiceBillingPhone}",
                    //                $"CarrierClaimsPhone={item.CarrierClaimsPhone}",
                    //                $"CarrierRoadsidePhone={item.CarrierRoadsidePhone}"

                    //            };

                    //        string jsonContent = JsonConvert.SerializeObject(new
                    //        {
                    //            Name = $"{item.FirstName} {item.LastName}",
                    //            Email = item.Email,
                    //            HasExternalDoubleOptIn = false,
                    //            CustomFields = customFields
                    //        });

                    //        var sendtoAPI = _sendDataAsync(jsonContent);

                    //    }
                    //    catch (Exception ex)
                    //    {

                    //    }

                    //}
                    #endregion

                    // Multiple
                    var subscribers = new List<object>();

                    foreach (MarketingContact item in dataList)
                    {
                        var customFields = new List<string>
                        {
                            $"MemberNumber={item.MemberNumber}",
                            $"FirstName={item.FirstName}",
                            $"LastName={item.LastName}",
                            $"Salutation={item.Salutation}",
                            $"AgentTitle={item.AgentTitle}",
                            $"AgentName={item.AgentName}",
                            $"AgentEmail={item.AgentEmail}",
                            $"AgentPhone={item.AgentPhone}",
                            $"AgentExt={item.AgentExt}",
                            $"PolicyEffectiveDate={item.PolicyEffectiveDate}",
                            $"PolicyCarrier={item.PolicyCarrier}",
                            $"CarrierCode={item.CarrierCode}",
                            $"CarrierName={item.CarrierName}",
                            $"CarrierURL={item.CarrierURL}",
                            $"CarrierServiceBillingPhone={item.CarrierServiceBillingPhone}",
                            $"CarrierClaimsPhone={item.CarrierClaimsPhone}",
                            $"CarrierRoadsidePhone={item.CarrierRoadsidePhone}"
                        };

                        var subscriber = new
                        {
                            Name = $"{item.FirstName} {item.LastName}",
                            Email = item.Email,
                            CustomFields = customFields
                        };

                        subscribers.Add(subscriber);
                    }

                    var jsonContent = JsonConvert.SerializeObject(new
                    {
                        HasExternalDoubleOptIn = true,
                        Subscribers = subscribers
                    });

                    var _postList = _multipleDataAsync(jsonContent);

                    Sitecore.Diagnostics.Log.Info("AFI Import Contact Load completed  ", this);
                }

            }
        }

        private async Task _multipleDataAsync(string item)
        {
            using (var httpClient = new HttpClient { BaseAddress = baseAddress })
            {

                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("accept", "application/json");

                using (var content = new StringContent(item, System.Text.Encoding.Default, "application/json"))
                {
                    using (var response = await httpClient.PostAsync($"subscribers/{emailListID}/subscribe_many.json?apiKey={SitecoreSendApiKey}", content))
                    {
                        string responseData = await response.Content.ReadAsStringAsync();
                    }
                }
            }
        }

        private async Task _sendDataAsync(string item)
        {
            using (var httpClient = new HttpClient { BaseAddress = baseAddress })
            {

                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("accept", "application/json");

                using (var content = new StringContent(item, System.Text.Encoding.Default, "application/json"))
                {
                    using (var response = await httpClient.PostAsync($"subscribers/{emailListID}/subscribe.json?apiKey={SitecoreSendApiKey}", content))
                    {
                        string responseData = await response.Content.ReadAsStringAsync();
                    }
                }
            }
        }

        private async Task<bool> ExecuteCall(string data)
        {
            try
            {
                using (var httpClient = new HttpClient { BaseAddress = baseAddress })
                {
                    httpClient.DefaultRequestHeaders.TryAddWithoutValidation("accept", "application/json");

                    using (var content = new StringContent(data, Encoding.Default, "application/json"))
                    {
                        using (var response = await httpClient.PostAsync($"subscribers/{emailListID}/subscribe.json?apiKey={SitecoreSendApiKey}", content))
                        {
                            string response_ = await response.Content.ReadAsStringAsync();
                            if (!string.IsNullOrEmpty(response_))
                            {
                                return true;
                            }
                            else
                            {
                                return false;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                Log.Error(ex.Message.ToString(), "ExecuteCall");
                return false;
            }
        }
    }
}