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
using System.Threading;

namespace AFI.Feature.SitecoreSend.Job
{
    public class SyncSubscribersAFIDBtoMoosend
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
            try
            {
                MoosendSettingItem();

                Sitecore.Diagnostics.Log.Info("AFI Sync Contact Sitecore scheduled task is being run!", this);

                List<MoosendListSubscriber> dataList;

                dataList = repository.GetAllListSubscriberByIsSynced(false);// Load All IsSynced false data

                var groupedItems = dataList.GroupBy(item => item.ListName);

                //int batchSize;
                //int currentIndex;

                foreach (var group in groupedItems)
                {
                    EmailListData emailListData = repository.GetEmailListDataBylistName(group.Key);


                    #region Subscribers Single Data Async

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

                        //var responseData = ProcessData(finalData, emailListData.ListId);

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
                        if (isSynced)
                        {
                            item.EmailListId = emailListData.Id;
                            item.SendListId = emailListData.ListId;
                            item.SubscriberId = SubscriberId;
                            item.IsSynced = isSynced;
                            item.SyncedTime = DateTime.Now;

                            repository.ListSubscriberUpdate(item);

                        }
                        string logDescription = JsonFieldcombined(item.JsonBody, group.Key, string.Empty, emailListData.ListId, SubscriberId, ResponseMessage);

                        repository.InserAFIMoosendtLog(logDescription, LogType.Subscriber, item.JsonBody, responseData);

                        // await Task.Delay(10000); // Delay for 10 seconds between each request
                        Thread.Sleep(2000); // Delay for 2 seconds between each request

                    }

                    #endregion

                    #region Subscribers Multiple Data Async 

                    //emailListID = emailListData.ListId;
                    //if (group != null)
                    //{
                    //    batchSize = 500;
                    //    currentIndex = 0;

                    //    while (currentIndex < group.Count())
                    //    {

                    //        var batch = group.Skip(currentIndex).Take(batchSize);


                    //        // Multiple
                    //        var subscribers = new List<object>();

                    //        foreach (MoosendListSubscriber item in batch)
                    //        {
                    //            var currentData = JsonConvert.DeserializeObject<dynamic>(item.JsonBody);
                    //            var customFieldsList = new List<string>();
                    //            if (currentData.CustomFields != null)
                    //            {
                    //                foreach (var field in currentData.CustomFields)
                    //                {
                    //                    customFieldsList.Add($"{field.Name}={field.Value}");
                    //                }
                    //            }
                    //            // Create the final object using properties from currentData
                    //            //var finalData = CreateFinalData(currentData, true, customFieldsList);

                    //            var subscriber = new
                    //            {
                    //                Name = currentData.Name ?? string.Empty,
                    //                Email = currentData.Email ?? string.Empty,
                    //                Mobile = currentData.Mobile ?? string.Empty,
                    //                CustomFields = customFieldsList

                    //            };

                    //            subscribers.Add(subscriber);
                    //        }

                    //        var jsonContent = JsonConvert.SerializeObject(new
                    //        {
                    //            HasExternalDoubleOptIn = true,
                    //            Subscribers = subscribers
                    //        });

                    //        var _postList = _multipleDataAsync(jsonContent);

                    //        currentIndex += batchSize;
                    //    }

                    //}

                    #endregion
                }

            }
            catch (Exception ex)
            {

                Sitecore.Diagnostics.Log.Info("Subscribers Sync Error: " + ex, this);
            }

        }

        public async Task ProcessData(object finalData, string mailinglistid)
        {
            // Call SubscribersCreate method for each data point
            var response = await service.SubscribersCreate(finalData, mailinglistid, baseAddress, SitecoreSendApiKey);

            // Introduce a delay to avoid rate limiting
            await Task.Delay(10000); // Delay for 10 seconds between each request
           

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
