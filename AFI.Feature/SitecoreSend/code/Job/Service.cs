using AFI.Feature.SitecoreSend.Models;
using AFI.Feature.SitecoreSend.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Sitecore.Data;
using System.Security.Cryptography;
using System.Web;
using System.Web.Mvc;
using static AFI.Feature.SitecoreSend.Constant;
using System.Net.Http;

namespace AFI.Feature.SitecoreSend.Job
{
    public class Service
    {
       

        AFIMoosendRepository repository = new AFIMoosendRepository();
        public async Task<string> MailingListCreate(string name, string confirmationPage, string redirectAfterUnsubscribePage, Uri baseAddress, string SitecoreSendApiKey)
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

        public async Task<string> CustomCreate(string name, string mailinglistid, Uri baseAddress, string SitecoreSendApiKey)
        {
            using (var httpClient = new HttpClient { BaseAddress = baseAddress })
            {
                try
                {
                    httpClient.DefaultRequestHeaders.TryAddWithoutValidation("accept", "application/json");
                    var data = new
                    {
                        Name = name,
                        CustomFieldType = "Text"
                    };

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
                    return "Error";
                }
                catch (Exception ex)
                {
                    return "Error";
                }
            }
        }

        public async Task<string> SubscribersCreate(dynamic data, string mailinglistid, Uri baseAddress, string SitecoreSendApiKey)
        {
            using (var httpClient = new HttpClient { BaseAddress = baseAddress })
            {
                try
                {
                    httpClient.DefaultRequestHeaders.TryAddWithoutValidation("accept", "application/json");

                    // Serialize the final object to JSON
                    var json = JsonConvert.SerializeObject(data);

                    var jsonContent = new StringContent(json, System.Text.Encoding.Default, "application/json");

                    using (var response = await httpClient.PostAsync($"subscribers/{mailinglistid}/subscribe.json?apiKey={SitecoreSendApiKey}", jsonContent))
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

        
    }
}