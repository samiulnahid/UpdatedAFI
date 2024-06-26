﻿using AFI.Feature.SitecoreSend.Models;
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
                string ResponseMessage = "";

                var listId = "";
              var  exEmailListData = repository.GetEmailListDataBylistName(group.Key);

                if (exEmailListData == null)
                {
                    var data = new
                    {
                        Name = group.Key,
                        ConfirmationPage = string.Empty,
                        RedirectAfterUnsubscribePage = string.Empty
                    };

                    var responseData = service.MailingListCreate(data, baseAddress, SitecoreSendApiKey).Result;
                    MoosendResponseModel response = JsonConvert.DeserializeObject<MoosendResponseModel>(responseData);

                    if (!string.IsNullOrEmpty(response.Context))
                    {
                        EmailListData emailListData = new EmailListData();
                        emailListData.ListName = group.Key;
                        emailListData.ListId = response.Context;
                        emailListData.CreatedBy = "System";
                        ResponseMessage = MoosendMessage.Success;

                        repository.InsertEmailListData(emailListData);

                        listId = response.Context;

                        string logDescription = JsonFieldcombined(string.Empty, group.Key, string.Empty, emailListData.ListId, string.Empty, ResponseMessage);

                        repository.InserAFIMoosendtLog(logDescription, LogType.EmailList, data.ToString(), responseData);
                    }


                }
                else
                {
                    listId = exEmailListData.ListId;
                }

                foreach (var item in group.Take(1))
                {

                    JObject jsonObject = JObject.Parse(item.JsonBody);

                    // Extract keys recursively
                    var keys = GetKeys(jsonObject);
                    foreach (var key in keys)
                    {
                        string KeyResponseMessage = "";
                        Console.WriteLine(key);
                        if (key != null && !new[] { "Email", "Name", "Mobile", "CustomFields" }.Contains(key.ToString()))
                        {
                            var data = new
                            {
                                Name = key,
                                CustomFieldType = "Text"
                            };

                            var responseData = Task.Run(() => service.CustomCreate(data, listId, baseAddress, SitecoreSendApiKey)).Result; // New/ Existing ListId

                            MoosendResponseModel response = JsonConvert.DeserializeObject<MoosendResponseModel>(responseData);

                            if (response.Context != null)
                            {
                                KeyResponseMessage = MoosendMessage.Success;
                            }
                            else
                            {
                                KeyResponseMessage = response.Error;
                            }

                            string logDescription = JsonFieldcombined(string.Empty, group.Key, key, listId, string.Empty, KeyResponseMessage);

                            repository.InserAFIMoosendtLog(logDescription, LogType.CustomField, data.ToString(), responseData);
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
                    if (currentData.CustomFields != null)
                    {
                        foreach (var field in currentData.CustomFields)
                        {
                            customFieldsList.Add($"{field.Name}={field.Value}");
                        }
                    }
                    // Create the final object using properties from currentData
                    var finalData = CreateFinalData(currentData, true, customFieldsList);

                    var responseData = service.SubscribersCreate(finalData, emailListData.ListId, baseAddress, SitecoreSendApiKey).Result;
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
                    if (response.Error != null)
                    {
                        isSynced = false;
                    }
                    item.EmailListId = emailListData.Id;
                    item.SendListId = emailListData.ListId;
                    item.SubscriberId = SubscriberId;
                    item.IsSynced = isSynced;
                    item.SyncedTime = DateTime.Now;

                    repository.ListSubscriberUpdate(item);

                    string logDescription = JsonFieldcombined(item.JsonBody, group.Key, string.Empty, emailListData.ListId, SubscriberId, ResponseMessage);

                    repository.InserAFIMoosendtLog(logDescription, LogType.Subscriber, item.JsonBody, responseData);
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


        private string JsonFieldcombined(string jsonBody, string listName, string fieldName, string listId, string subscriberId, string ResponseMessage)
        {

            if (!string.IsNullOrEmpty(jsonBody))
            {
                JObject jsonData = JObject.Parse(jsonBody);

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
                string combinedJson = createdData.ToString();
                return combinedJson;
            }
            else
            {
                string jsonName = !string.IsNullOrEmpty(fieldName) ? "CustomFieldName" : "ListName";
                string JsonNameValue = !string.IsNullOrEmpty(fieldName) ? fieldName : listName;

                JObject createdData = new JObject
                {
                    { "ListId", listId },
                    { jsonName , JsonNameValue },
                    { "Message", ResponseMessage }
                };
                string combinedJson = createdData.ToString();
                return combinedJson;
            }

            // Serialize the new data into a JSON string

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
