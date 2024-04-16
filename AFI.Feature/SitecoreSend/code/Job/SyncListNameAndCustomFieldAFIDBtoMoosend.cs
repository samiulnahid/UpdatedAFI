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
    public class SyncListNameAndCustomFieldAFIDBtoMoosend
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

            List<MoosendListSubscriber> dataList = repository.GetAllListSubscriberGroupByListName(false);// Load All IsSynced false data
          //  var groupedItems = dataList.GroupBy(item => item.ListName);

            foreach (var group in dataList)
            {
                string ResponseMessage = "";

                var listId = "";
              var  exEmailListData = repository.GetEmailListDataBylistName(group.ListName);

                if (exEmailListData == null)
                {
                    var data = new
                    {
                        Name = group.ListName,
                        ConfirmationPage = string.Empty,
                        RedirectAfterUnsubscribePage = string.Empty
                    };

                    var responseData = service.MailingListCreate(data, baseAddress, SitecoreSendApiKey).Result;
                    MoosendResponseModel response = JsonConvert.DeserializeObject<MoosendResponseModel>(responseData);

                    if (!string.IsNullOrEmpty(response.Context))
                    {
                        EmailListData emailListData = new EmailListData();
                        emailListData.ListName = group.ListName;
                        emailListData.ListId = response.Context;
                        emailListData.CreatedBy = "System";
                        ResponseMessage = MoosendMessage.Success;

                        repository.InsertEmailListData(emailListData);

                        listId = response.Context;

                        string logDescription = JsonFieldcombined(string.Empty, group.ListName, string.Empty, emailListData.ListId, string.Empty, ResponseMessage);

                        repository.InserAFIMoosendtLog(logDescription, LogType.EmailList, data.ToString(), responseData);
                    }


                }
                else
                {
                    listId = exEmailListData.ListId;
                }


                JObject jsonObject = JObject.Parse(group.JsonBody);

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

                        string logDescription = JsonFieldcombined(string.Empty, group.ListName, key, listId, string.Empty, KeyResponseMessage);

                        repository.InserAFIMoosendtLog(logDescription, LogType.CustomField, data.ToString(), responseData);
                    }
                }

                
            }

           
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
     
  
    }
}
