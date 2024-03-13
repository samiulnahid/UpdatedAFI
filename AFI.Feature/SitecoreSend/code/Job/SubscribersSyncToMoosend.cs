using AFI.Feature.SitecoreSend.Models;
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
using AFI.Feature.SitecoreSend.Repositories;
using static AFI.Feature.SitecoreSend.Constant;
using System.Web.Mvc;
using Newtonsoft.Json.Linq;

namespace AFI.Feature.SitecoreSend.Job
{
    public class SubscribersSyncToMoosend
    {

        public static string api_key = string.Empty;
        public static string api_url = string.Empty;

        Service service = new Service();
        AFIMoosendRepository repository = new AFIMoosendRepository();

        public void MoosendSettingItem()
        {
            Item moosendSetting = Sitecore.Context.ContentDatabase.GetItem(Constant.MoosendSetting.MoosendSettingId);
            if (moosendSetting != null)
            {
                api_key = GetFieldValue(moosendSetting, Constant.MoosendSetting.Fields.APIKEY);

                api_url = GetFieldValue(moosendSetting, Constant.MoosendSetting.Fields.APIURL);

            }
            baseAddress = new Uri(api_url);
            SitecoreSendApiKey = api_key;

        }
        private string GetFieldValue(Item item, ID fieldId)
        {
            var field = item.Fields[fieldId];
            return field != null ? field.Value : String.Empty;
        }


        private Uri baseAddress;
        private string SitecoreSendApiKey;
        private string emailListID;
     

        public void Execute(Item[] items, Sitecore.Tasks.CommandItem command, Sitecore.Tasks.ScheduleItem schedule)
        {
            MoosendSettingItem();

            Sitecore.Diagnostics.Log.Info("AFI Sync Contact Sitecore scheduled task is being run!", this);

            List<MoosendListSubscriber> dataList = repository.GetAllListSubscriberByIsSynced(false);// Load All IsSynced false data
            var groupedItems = dataList.GroupBy(item => item.ListName);

            foreach (var group in groupedItems)
            {
                EmailListData emailListData = new EmailListData();
                emailListData=  repository.GetEmailListDataBylistName(group.Key);

                //var responseData = service.MailingListCreate(group.Key, string.Empty, string.Empty).Result;
                //MoosendResponseModel response = JsonConvert.DeserializeObject<MoosendResponseModel>(responseData);

                //EmailListData data = new EmailListData();
                //if (!string.IsNullOrEmpty(response.Context))
                //{
                //    data.ListName = group.Key;
                //    data.ListId = response.Context;
                //    data.CreatedBy = "System";
                //    repository.InsertEmailListData(data);
                //}

               

                foreach (var item in group.Take(1))
                {

                    JObject jsonObject = JObject.Parse(item.JsonBody);

                    // Extract keys recursively
                    var keys = GetKeys(jsonObject);
                    foreach (var key in keys)
                    {
                        Console.WriteLine(key);
                        if (key != null && !new[] { "Email", "Name", "Mobile", "CustomFields" }.Contains(key.ToString()))
                        {

                            var responseData = Task.Run(() => service.CustomCreate(key, emailListData.ListId, baseAddress, SitecoreSendApiKey)).Result; // New/ Existing ListId
                            MoosendResponseModel response = JsonConvert.DeserializeObject<MoosendResponseModel>(responseData);
                        }
                    }

                }
            }

            foreach (var group in groupedItems)
            {
                EmailListData emailListData = repository.GetEmailListDataBylistName(group.Key);

                foreach (var item in group)
                {
                    string ResponseMessage = "";
                    string SubscriberId = "";
                    bool isSynced = item.IsSynced;

                    var currentData = JsonConvert.DeserializeObject<dynamic>(item.JsonBody);
                    var customFieldsList = new List<string>();
                    foreach (var field in currentData.CustomFields)
                    {
                        customFieldsList.Add($"{field.Name}={field.Value}");
                    }

                    // Create the final object using properties from currentData
                    var finalData = CreateFinalData(currentData, true, customFieldsList);


                    var responseData = service.SubscribersCreate(finalData, emailListData.ListId,baseAddress, SitecoreSendApiKey).Result;
                    SingleApiResponse response = JsonConvert.DeserializeObject<SingleApiResponse>(responseData);

                    if (response.Context != null)
                    {
                        SubscriberId = response.Context.ID;
                        ResponseMessage = MoosendMessage.Success;
                        isSynced = true;
                    }
                    else
                    {
                        ResponseMessage = response.Error;
                    }

                    item.SendListId = emailListData.ListId;
                    item.SubscriberId = SubscriberId;
                    item.IsSynced = isSynced;
                    item.SyncedTime = DateTime.Now;

                    repository.ListSubscriberUpdate(item);

                    string logDescription = JsonFieldcombined(item.JsonBody, group.Key, emailListData.ListId, SubscriberId, ResponseMessage);

                    repository.InserAFIMoosendtLog(logDescription, LogType.Subscriber);
                }
            }



        }
        private object CreateFinalData(dynamic currentData, bool hasExternalDoubleOptIn, List<string> customFields)
        {
            return new
            {
                Name = currentData.Name,
                Email = currentData.Email,
                Mobile = currentData.Mobile,
                HasExternalDoubleOptIn = hasExternalDoubleOptIn,
                CustomFields = customFields
            };
        }
        static IEnumerable<string> GetKeys(JObject jObject)
        {
            foreach (var property in jObject.Properties())
            {
                yield return property.Name;

                if (property.Value is JObject subObject)
                {
                    // Recursively get keys for sub-objects
                    foreach (var subKey in GetKeys(subObject))
                    {
                        yield return subKey;
                    }
                }
            }
        }


        private string JsonFieldcombined(string data, string listName, string listId, string subscriberId, string ResponseMessage)
        {
            JObject jsonData = JObject.Parse(data);

            // Extract relevant fields
            string email = (string)jsonData["Email"];
            string name = (string)jsonData["Name"];

            // Create a new JObject with additional fields
            JObject createdData = new JObject
            {
                { "SubscriberId", subscriberId },
                { "ListId", listId },
                { "ListName", listName },
                { "Email", email },
                { "Name", name },
                { "Message", ResponseMessage }
            };
            // Serialize the new data into a JSON string
            string combinedJson = createdData.ToString();

            return combinedJson;
        }
        private void CustomStringValues(Dictionary<string, object> data, string listId)
        {
            foreach (var pair in data)
            {
                

                if (pair.Value is string)
                {
                    if (pair.Key != null && !new[] { "Email", "Name", "Mobile" }.Contains(pair.Key.ToString()))
                    {

                        var responseData = Task.Run(() => service.CustomCreate(pair.Key, listId, baseAddress, SitecoreSendApiKey)).Result;
                    MoosendResponseModel response = JsonConvert.DeserializeObject<MoosendResponseModel>(responseData);
                    }
                }
                else if (pair.Value is object)
                {
                    // Recursive call for nested objects
                    CustomStringValues((Dictionary<string, object>)pair.Value, listId);
                }
              

                //if (pair.Value != null && (pair.Value.ToString() !="Email" || pair.Value.ToString() != "Name" || pair.Value.ToString() != "Mobile"))
                //{
                //    if (pair.Value is string)
                //    {
                //        var responseData = Task.Run(() => service.CustomCreate(pair.Key, listId, baseAddress, SitecoreSendApiKey)).Result;
                //        MoosendResponseModel response = JsonConvert.DeserializeObject<MoosendResponseModel>(responseData);
                //    }
                //    else if (pair.Value is Dictionary<string, object>)
                //    {
                //        // Recursive call for nested objects
                //        CustomStringValues((Dictionary<string, object>)pair.Value, listId);
                //    }
                //}
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
