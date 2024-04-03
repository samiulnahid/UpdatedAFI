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
    public class ImportDedicatedAgentToMoosend
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
            emailListID = dedicated_listId;
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
                SqlCommand cmd = new SqlCommand(@"Select * from [DedicatedAgent].[vwMemberAll]", connection); // To-do
                connection.Open();
                using (SqlDataReader reader = cmd.ExecuteReader())
                {

                    while (reader.Read())
                    {
                        MarketingContact model = new MarketingContact();
                        model.MemberNumber = reader["MemberNumber"].ToString();
                        model.FirstName = reader["MemberFirstName"].ToString();
                        model.LastName = reader["MemberLastName"].ToString();
                        model.Email = reader["MemberEmail"].ToString();
                        model.Salutation = reader["Salutation"].ToString();
                        model.AgentName = reader["AgentName"].ToString();
                        model.AgentEmail = reader["AgentEmail"].ToString();
                        model.AgentPhone = reader["AgentPhone"].ToString();
                        model.AgentExt = reader["AgentExt"].ToString();

                        dataList.Add(model);
                    }

                    reader.Close();
                }


                Sitecore.Diagnostics.Log.Info("AFI Import Contact start Map> ", "Marketing DB Connection close" + this);
                connection.Close();

                if (dataList != null)
                {
                    Sitecore.Diagnostics.Log.Info("AFI Import Contact start Map> ", "Marketing DB " + this);

                    int batchSize = 500;
                    int currentIndex = 0;

                    while (currentIndex < dataList.Count)
                    {

                        var batch = dataList.Skip(currentIndex).Take(batchSize);

                        // Multiple
                        var subscribers = new List<object>();

                        foreach (MarketingContact item in batch)
                        {
                            var customFields = new List<string>
                            {
                                $"MemberNumber={item.MemberNumber}",
                                $"FirstName={item.FirstName}",
                                $"LastName={item.LastName}",
                                $"Salutation={item.Salutation}",
                                $"AgentName={item.AgentName}",
                                $"AgentEmail={item.AgentEmail}",
                                $"AgentPhone={item.AgentPhone}",
                                $"AgentExt={item.AgentExt}",
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

                        currentIndex += batchSize;
                    }
                    

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

    }
}