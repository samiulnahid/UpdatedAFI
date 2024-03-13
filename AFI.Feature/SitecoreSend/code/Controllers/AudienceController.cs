using AFI.Feature.SitecoreSend.Models;
using AFI.Feature.SitecoreSend.Repositories;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using Sitecore.Data;
using Sitecore.Data.Comparers;
using Sitecore.Data.Items;
using Sitecore.StringExtensions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;
using System.Web;
using System.Web.Helpers;
using System.Web.Mvc;
using static AFI.Feature.SitecoreSend.Constant;

namespace AFI.Feature.SitecoreSend.Controllers
{
    public class AudienceController : Controller
    {
        public static string api_key = string.Empty;
        public static string api_url = string.Empty;
        AFIMoosendRepository repository = new AFIMoosendRepository();

        public void MoosendSettingItem()
        {
            Item moosendSetting = Sitecore.Context.Database.GetItem(Constant.MoosendSetting.MoosendSettingId);
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

        #region SecurityKey
        [HttpPost]
        public JsonResult RequestSecurityToken(string email)
        {

            if (!ValidateInputs(out JsonResult validationError, email))
            {
                return validationError;
            }
            AFISecurityKeyData validKeyData = repository.GetValidSecurityKeyByEmail(email);

            if (validKeyData != null)
            {
                validKeyData.StateTime = DateTime.Now;
                validKeyData.EndTime = DateTime.Now.AddHours(1);
                repository.UpdateSecurityKey(validKeyData);

                ResponseModelSecurity responseModel = new ResponseModelSecurity { Code = MoosendMessage.SuccessCode, Token = validKeyData.SecurityKey, Message = MoosendMessage.SecurityCodeMessage };
                return Json(responseModel, JsonRequestBehavior.AllowGet);
            }
            else
            {
                var securityKey = GenerateRandomSecurityKey(12);
                AFISecurityKeyData data = new AFISecurityKeyData
                {
                    Email = email,
                    SecurityKey = securityKey,
                    StateTime = DateTime.Now,
                    EndTime = DateTime.Now.AddHours(1)
                };

                repository.InsertAFISecurityKeyData(data);
                ResponseModelSecurity responseModel = new ResponseModelSecurity { Code = MoosendMessage.SuccessCode, Token = validKeyData.SecurityKey, Message = MoosendMessage.SecurityCodeMessage };
                return Json(responseModel, JsonRequestBehavior.AllowGet);
            }

        }

        static string GenerateRandomSecurityKey(int keyLength)
        {
            using (RNGCryptoServiceProvider rng = new RNGCryptoServiceProvider())
            {
                byte[] keyBytes = new byte[keyLength];
                rng.GetBytes(keyBytes);

                // Convert the byte array to a hexadecimal string
                return BitConverter.ToString(keyBytes).Replace("-", string.Empty);
            }
        }
        #endregion

        #region Mail List


        private bool ValidateInputs(out JsonResult errorResult, params string[] fields)
        {
            errorResult = null;

            foreach (var field in fields)
            {
                if (string.IsNullOrWhiteSpace(field))
                {
                    errorResult = Json(MoosendMessage.EmptyField, JsonRequestBehavior.AllowGet);
                    return false;
                }
            }
            return true;
        }

        private bool SecurityKeyMatches(string securityKey)
        {

            if (repository.IsValidStateTimeAndEndTime(securityKey))
            {
                return true;
            }
            else
            {
                return false;
            }

        }


        // https://afi.local/api/sitecoresend/createlist
        [HttpPost]
        public JsonResult Createlist(string name)
        {
            MoosendSettingItem();
            var httpRequest = System.Web.HttpContext.Current.Request;
            string SecurityToken = httpRequest.Headers["SecurityToken"];

            if (!ValidateInputs(out JsonResult validationError, name, SecurityToken))
            {
                return validationError;
            }

            if (!SecurityKeyMatches(SecurityToken))
            {
                return Json(MoosendMessage.MissingSecurityCode, JsonRequestBehavior.AllowGet);
            }

            if (repository.IsListNameDuplicate(name))
            {
                return Json(MoosendMessage.DuplicateList, JsonRequestBehavior.AllowGet);
            }

            try
            {
                var responseData = Task.Run(() => MailingListCreate(name, string.Empty, string.Empty)).Result;
                MoosendResponseModel response = JsonConvert.DeserializeObject<MoosendResponseModel>(responseData);

                if (!string.IsNullOrEmpty(response.Context))
                {
                    EmailListData data = new EmailListData
                    {
                        ListName = name,
                        ListId = response.Context,
                        SecurityKey = SecurityToken,
                        CreatedBy = "System"
                    };

                    repository.InsertEmailListData(data);

                    ResponseModel responseModel = new ResponseModel { Code = MoosendMessage.SuccessCode, Message = MoosendMessage.Success };
                    return Json(responseModel, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(responseData, JsonRequestBehavior.AllowGet);
                }
            }
            catch (HttpRequestException ex)
            {
                return Json($"HTTP Request Exception: {ex.Message}", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json($"Exception: {ex.Message}", JsonRequestBehavior.AllowGet);
            }

        }


        public async Task<string> MailingListCreate(string name, string confirmationPage, string redirectAfterUnsubscribePage)
        {
            using (var httpClient = new HttpClient { BaseAddress = baseAddress })
            {
                try
                {
                    httpClient.DefaultRequestHeaders.TryAddWithoutValidation("accept", "application/json");

                    var data = new
                    {
                        Name = name,
                        ConfirmationPage = confirmationPage,
                        RedirectAfterUnsubscribePage = redirectAfterUnsubscribePage
                    };

                    var jsonContent = new StringContent(JsonConvert.SerializeObject(data), System.Text.Encoding.Default, "application/json");

                    using (var response = await httpClient.PostAsync($"lists/create.json?apiKey={SitecoreSendApiKey}", jsonContent))
                    {
                        response.EnsureSuccessStatusCode(); // Throws exception if not successful

                        string responseData = await response.Content.ReadAsStringAsync();
                        return responseData;
                    }
                }
                catch (HttpRequestException ex)
                {
                    return $"HTTP Request Exception: {ex.Message}";
                }
                catch (Exception ex)
                {
                    return $"Exception: {ex.Message}";
                }
            }
        }

        // https://afi.local/api/sitecoresend/updatelist
        [HttpPost]
        public JsonResult Updatelist(string name, string oldListName)
        {
            MoosendSettingItem();
            var httpRequest = System.Web.HttpContext.Current.Request;
            string securitykey = httpRequest.Headers["SecurityToken"];

            if (!ValidateInputs(out JsonResult validationError, name, securitykey, oldListName))
            {
                return validationError;
            }

            if (!SecurityKeyMatches(securitykey))
            {
                return Json(MoosendMessage.MissingSecurityCode, JsonRequestBehavior.AllowGet);
            }

            EmailListData emailListData = repository.GetEmailListDataBylistName(oldListName);
            if (emailListData != null)
            {
                var responseData = Task.Run(() => MailingListUpdate(name, emailListData.ListId, string.Empty, string.Empty)).Result;
                MoosendResponseModel response = JsonConvert.DeserializeObject<MoosendResponseModel>(responseData);

                if (!string.IsNullOrEmpty(response.Context))
                {
                    emailListData.ListName = name;

                    repository.UpdateEmailListName(emailListData);

                    ResponseModel responseModel = new ResponseModel { Code = MoosendMessage.SuccessCode, Message = MoosendMessage.Update };
                    return Json(responseModel, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(responseData, JsonRequestBehavior.AllowGet);
                }

            }
            else
            {
                return Json(MoosendMessage.ListNameInvalid, JsonRequestBehavior.AllowGet);
            }


        }
        public async Task<string> MailingListUpdate(string name, string mailinglistid, string confirmationPage, string redirectAfterUnsubscribePage)
        {
            using (var httpClient = new HttpClient { BaseAddress = baseAddress })
            {
                try
                {
                    httpClient.DefaultRequestHeaders.TryAddWithoutValidation("accept", "application/json");

                    var data = new
                    {
                        Name = name,
                        ConfirmationPage = confirmationPage,
                        RedirectAfterUnsubscribePage = redirectAfterUnsubscribePage
                    };

                    var jsonContent = new StringContent(JsonConvert.SerializeObject(data), System.Text.Encoding.Default, "application/json");

                    using (var response = await httpClient.PostAsync($"lists/{mailinglistid}/update.json?apiKey={SitecoreSendApiKey}", jsonContent))
                    {
                        response.EnsureSuccessStatusCode(); // Throws exception if not successful

                        string responseData = await response.Content.ReadAsStringAsync();
                        return responseData;
                    }
                }
                catch (HttpRequestException ex)
                {
                    return $"HTTP Request Exception: {ex.Message}";
                }
                catch (Exception ex)
                {
                    return $"Exception: {ex.Message}";
                }
            }
        }

        // https://afi.local/api/sitecoresend/Deletelist
        [HttpDelete]
        public JsonResult Deletelist(string ListName)
        {
            MoosendSettingItem();
            var httpRequest = System.Web.HttpContext.Current.Request;
            string securitykey = httpRequest.Headers["SecurityToken"];


            if (!SecurityKeyMatches(securitykey))
            {
                return Json(MoosendMessage.MissingSecurityCode, JsonRequestBehavior.AllowGet);
            }
            EmailListData emailListData = repository.GetEmailListDataBylistName(ListName);
            if (emailListData != null)
            {
                var responseData = Task.Run(() => MailingListDelete(emailListData.ListId)).Result;
                return Json(responseData, JsonRequestBehavior.AllowGet);
            }
            else
            {
                return Json(MoosendMessage.ListNameInvalid, JsonRequestBehavior.AllowGet);
            }


        }
        public async Task<string> MailingListDelete(string mailinglistid)
        {
            using (var httpClient = new HttpClient { BaseAddress = baseAddress })
            {
                try
                {
                    httpClient.DefaultRequestHeaders.TryAddWithoutValidation("accept", "application/json");

                    using (var response = await httpClient.DeleteAsync($"/lists/{mailinglistid}/delete.json?apiKey={SitecoreSendApiKey}"))
                    {
                        string responseData = await response.Content.ReadAsStringAsync();
                        return responseData;
                    }

                }
                catch (HttpRequestException ex)
                {
                    return $"HTTP Request Exception: {ex.Message}";
                }
                catch (Exception ex)
                {
                    return $"Exception: {ex.Message}";
                }
            }
        }

        // https://afi.local/api/sitecoresend/GetAllList
        [HttpGet]
        public JsonResult GetAllList(string pageSize, string page)
        {
            MoosendSettingItem();
            var httpRequest = System.Web.HttpContext.Current.Request;
            string securitykey = httpRequest.Headers["SecurityToken"];

            if (!SecurityKeyMatches(securitykey))
            {
                return Json(MoosendMessage.MissingSecurityCode, JsonRequestBehavior.AllowGet);
            }

            try
            {
                page = string.IsNullOrEmpty(page) ? "1" : page;
                pageSize = string.IsNullOrEmpty(pageSize) ? "20" : pageSize;

                using (var httpClient = new HttpClient())
                {
                    string apiUrl = $"{baseAddress}/lists/{page}/{pageSize}.json?apiKey={SitecoreSendApiKey}&WithStatistics=true&ShortBy=CreatedOn&SortMethod=ASC";

                    using (var response = httpClient.GetAsync(apiUrl).Result)
                    {
                        response.EnsureSuccessStatusCode();

                        string responseData = response.Content.ReadAsStringAsync().Result;

                        return Json(responseData, JsonRequestBehavior.AllowGet);
                    }
                }
            }
            catch (HttpRequestException ex)
            {
                return Json($"HTTP Request Exception: {ex.Message}", JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json($"Exception: {ex.Message}", JsonRequestBehavior.AllowGet);
            }

        }



        //[HttpPost]
        //public JsonResult CustomfieldsCreate(CustomField data, string ListName, string securitykey)
        //{
        //    MoosendSettingItem();

        //    if (!ValidateInputs(out JsonResult validationError, ListName, securitykey))
        //    {
        //        return validationError;
        //    }

        //    if (!SecurityKeyMatches(securitykey))
        //    {
        //        return Json(MoosendMessage.MissingSecurityCode, JsonRequestBehavior.AllowGet);
        //    }

        //    EmailListData emailListData = repository.GetEmailListDataBylistName(ListName);
        //    if (emailListData != null)
        //    {
        //        if (data != null)
        //        {

        //            var responseData = Task.Run(() => CustomfieldsCreate(data, emailListData.ListId)).Result;
        //            return Json(responseData, JsonRequestBehavior.AllowGet);

        //        }
        //        else
        //        {
        //            return Json(MoosendMessage.EmptyData, JsonRequestBehavior.AllowGet);
        //        }
        //    }
        //    else
        //    {
        //        return Json(MoosendMessage.ListNameInvalid, JsonRequestBehavior.AllowGet);
        //    }


        //}

        [HttpPost]
        public JsonResult CustomfieldsCreate(CustomFieldList data)
        {
            MoosendSettingItem();
            var httpRequest = System.Web.HttpContext.Current.Request;
            string securitykey = httpRequest.Headers["SecurityToken"];

            if (!ValidateInputs(out JsonResult validationError, data.ListName, securitykey))
            {
                return validationError;
            }

            if (!SecurityKeyMatches(securitykey))
            {
                return Json(MoosendMessage.MissingSecurityCode, JsonRequestBehavior.AllowGet);
            }

            EmailListData emailListData = repository.GetEmailListDataBylistName(data.ListName);

            if (emailListData != null)
            {
                if (data != null)
                {
                    var responseData = "";
                    var groupedFieldStutas = new Dictionary<string, List<string>>();

                    foreach (CustomField field in data.Fields)
                    {
                        responseData = Task.Run(() => CustomfieldsCreate(field, emailListData.ListId)).Result;
                        MoosendResponseModel response = JsonConvert.DeserializeObject<MoosendResponseModel>(responseData);

                        ResponseModel ResponseMessage = new ResponseModel();
                        if (string.IsNullOrEmpty(response.Error))
                        {
                            ResponseMessage.Code = MoosendMessage.SuccessCode;
                            ResponseMessage.Message = $"'{field.Name}' field Successfully Created";
                        }
                        else
                        {
                            ResponseMessage.Code = response.Code;
                            ResponseMessage.Message = response.Error;
                        }

                        string code = ResponseMessage.Code;
                        string message = ResponseMessage.Message;
                        if (!groupedFieldStutas.ContainsKey(code))
                        {
                            groupedFieldStutas[code] = new List<string>();
                        }
                        groupedFieldStutas[code].Add(message);
                    }

                    var modifiedList = groupedFieldStutas.Select(message => new
                    {
                        Code = message.Key,
                        Messages = message.Value
                    }).ToList();

                    return Json(modifiedList, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    return Json(MoosendMessage.EmptyData, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(MoosendMessage.ListNameInvalid, JsonRequestBehavior.AllowGet);
            }

        }

        public async Task<string> CustomfieldsCreate(CustomField data, string mailinglistid)
        {
            using (var httpClient = new HttpClient { BaseAddress = baseAddress })
            {
                try
                {
                    httpClient.DefaultRequestHeaders.TryAddWithoutValidation("accept", "application/json");


                    var jsonContent = new StringContent(JsonConvert.SerializeObject(data), System.Text.Encoding.Default, "application/json");

                    using (var response = await httpClient.PostAsync($"lists/{mailinglistid}/customfields/create.json?apiKey={SitecoreSendApiKey}", jsonContent))
                    {
                        response.EnsureSuccessStatusCode(); // Throws exception if not successful

                        string responseData = await response.Content.ReadAsStringAsync();
                        return responseData;
                    }
                }
                catch (HttpRequestException ex)
                {
                    return $"HTTP Request Exception: {ex.Message}";
                }
                catch (Exception ex)
                {
                    return $"Exception: {ex.Message}";
                }
            }
        }



        // https://afi.local/api/sitecoresend/CreateSubscribers
        [HttpPost]
        public JsonResult CreateSubscribers(CreateSubscribersRequest data, string ListName)
        {
            MoosendSettingItem();
            var httpRequest = System.Web.HttpContext.Current.Request;
            string securitykey = httpRequest.Headers["SecurityToken"];
            if (!ValidateInputs(out JsonResult validationError, ListName, securitykey))
            {
                return validationError;
            }

            if (!SecurityKeyMatches(securitykey))
            {
                return Json(MoosendMessage.MissingSecurityCode, JsonRequestBehavior.AllowGet);
            }

            EmailListData emailListData = repository.GetEmailListDataBylistName(ListName);
            if (emailListData != null)
            {
                if (data != null)
                {

                    var responseData = Task.Run(() => SubscribersCreate(data, emailListData.ListId)).Result;

                    ApiResponse response = JsonConvert.DeserializeObject<ApiResponse>(responseData);
                    if (response.Context != null)
                    {
                        Task.Run(() => StoreSubscribersResponse(response.Context, securitykey, emailListData.Id));

                        ResponseModel responseModel = new ResponseModel { Code = MoosendMessage.SuccessCode, Message = MoosendMessage.Success };
                        return Json(responseModel, JsonRequestBehavior.AllowGet);
                    }
                    else
                    {
                        ResponseModel responseModel = new ResponseModel { Code = MoosendMessage.ErrorCode, Message = response.Error };
                        return Json(responseModel, JsonRequestBehavior.AllowGet);
                    }


                }
                else
                {
                    return Json(MoosendMessage.EmptyData, JsonRequestBehavior.AllowGet);
                }
            }
            else
            {
                return Json(MoosendMessage.ListNameInvalid, JsonRequestBehavior.AllowGet);
            }

        }


        public async Task<string> SubscribersCreate(CreateSubscribersRequest data, string mailinglistid)
        {
            using (var httpClient = new HttpClient { BaseAddress = baseAddress })
            {
                try
                {
                    httpClient.DefaultRequestHeaders.TryAddWithoutValidation("accept", "application/json");

                    var jContent = JsonConvert.SerializeObject(new
                    {
                        HasExternalDoubleOptIn = true,
                        Subscribers = data.Subscribers
                    });

                    var jsonContent = new StringContent(jContent, System.Text.Encoding.Default, "application/json");

                    using (var response = await httpClient.PostAsync($"subscribers/{mailinglistid}/subscribe_many.json?apiKey={SitecoreSendApiKey}", jsonContent))
                    {
                        response.EnsureSuccessStatusCode(); // Throws exception if not successful

                        string responseData = await response.Content.ReadAsStringAsync();
                        return responseData;
                    }
                }
                catch (HttpRequestException ex)
                {
                    return $"HTTP Request Exception: {ex.Message}";
                }
                catch (Exception ex)
                {
                    return $"Exception: {ex.Message}";
                }
            }
        }
        private void StoreSubscribersResponse(List<SubscriberData> responseData, string securitykey, int ListId)
        {
            try
            {
                foreach (var subscriberData in responseData)
                {
                    string mobile = subscriberData.Mobile ?? string.Empty;
                    string name = subscriberData.Name ?? string.Empty;
                    string customFields = string.Join(", ", subscriberData.CustomFields.Select(cf => $"{cf.Name}: {cf.Value}"));
                    // Create an instance of your model and populate it with data from the response
                    MoosendListSubscriber subscriber = new MoosendListSubscriber
                    {
                        EmailListId = ListId,
                        SubscriberId = subscriberData.ID,
                        Email = subscriberData.Email,
                        Name = name,
                        Mobile = mobile,
                        JsonBody = customFields,
                        Source = "API", // Set the source accordingly
                        CreatedBy = "System", // Set the creator accordingly
                        CreatedDate = DateTime.UtcNow // Set the creation date accordingly
                    };

                    // Call your repository method to insert the subscriber into the database
                    repository.ListSubscriberInsert(subscriber);
                }

            }
            catch (Exception ex)
            {


            }


        }
        #endregion

        #region Subscribers AFI
        [HttpPost]
        public JsonResult subscribers(CreateSubscribersRequest data, string ListName, string source)
        {
            //  MoosendSettingItem();

            string securitykey = Request.Headers["SecurityToken"];
            if (!ValidateInputs(out JsonResult validationError, ListName, securitykey))
            {
                return validationError;
            }

            if (!SecurityKeyMatches(securitykey))
            {
                return Json(MoosendMessage.InvalidToken, JsonRequestBehavior.AllowGet);
            }
            var insertedId = 0;

            var emailList = new EmailListData
            {
                ListName = data.ListName,
                SecurityKey = securitykey,
                CreatedBy = "System",
                CreatedDate = DateTime.Now,
                ListId = string.Empty

            };
            insertedId = repository.InsertEmailListData(emailList);
            if (insertedId > 0)
            {
                foreach (var subscriber in data.Subscribers)
                {
                    string mobile = subscriber.Mobile ?? string.Empty;
                    string name = subscriber.Name ?? string.Empty;
                    string jsonBody = JsonConvert.SerializeObject(subscriber);

                    var listSubscriber = new MoosendListSubscriber
                    {
                        SendListId = string.Empty,
                        ListName = ListName,
                        Name = name,
                        Email = subscriber.Email,
                        Mobile = mobile,
                        JsonBody = jsonBody,
                        SubscriberId = string.Empty,
                        Source = source,
                        CreatedBy = "System",
                        CreatedDate = DateTime.UtcNow,
                        IsSynced = false,
                        SyncedTime = null

                    };
                    repository.ListSubscriberInsert(listSubscriber);
                }
            }
            ResponseModel responseModel = new ResponseModel { Code = MoosendMessage.SuccessCode, Message = MoosendMessage.SuccessImport };
            return Json(responseModel, JsonRequestBehavior.AllowGet);


        }
        #endregion
    }
}