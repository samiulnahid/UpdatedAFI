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
using System.Web.Mvc;
using Newtonsoft.Json.Linq;
using AFI.Feature.SitecoreSend.Repositories;
using AFI.Feature.SitecoreSend.Models;
using static AFI.Feature.SitecoreSend.Constant;
using System.Threading;

namespace AFI.Feature.SitecoreSend.Job
{
    public class SyncVoteMemberToMoosendByCount
    {

        public static string api_key = string.Empty;
        public static string api_url = string.Empty;

        Service service = new Service();
        AFIMoosendRepository _AFIReportRepository = new AFIMoosendRepository();


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
        private string emailListID;


        public  bool Execute(int count, string listId)
        {
            try
            {
                MoosendSettingItem();

                emailListID = listId;

                List<ProxyVoteMemberMoosend> dataList;

                do
                {
                    // Load all member data
                    dataList = _AFIReportRepository.GetAllVotingMemberDataByCount(count);


                    if (dataList != null)
                    {
                        int batchSize = 300;
                        int currentIndex = 0;

                        while (currentIndex < dataList.Count)
                        {

                            var batch = dataList.Skip(currentIndex).Take(batchSize);

                            // Multiple
                            var subscribers = new List<object>();

                            foreach (ProxyVoteMemberMoosend item in batch)
                            {
                               

                                var customFields = new List<string>
                            {
                                $"MemberNumber={item.MemberNumber}",
                                $"PIN={item.PINNumber}",
                                $"RankAbbreviation={item.RankAbbreviation}",
                                $"Salutation={item.Salutation}",
                                $"FirstName={item.FirstName}",
                                $"MiddleName={item.MiddleName}",
                                $"LastName={item.LastName}",
                                $"ClientType={item.ClientType}",
                                $"MilitaryStatus={item.MilitaryStatus}",
                                $"AddressLine1={item.AddressLine1}",
                                $"AddressLine2={item.AddressLine2}",
                                $"City={item.City}",
                                $"MailingCountyName={item.MailingCountyName}",
                                $"State={item.State}",
                                $"PostalCode={item.PostalCode}",
                                $"Country={item.Country}",
                                $"MembershipDate={item.MembershipDate}",
                                $"YearsAsMember={item.YearsAsMember}",
                                $"Gender={item.Gender}",
                                $"MarketingCode={item.MarketingCode}",
                                $"ProperFirstName={item.ProperFirstName}",
                                $"Suffix={item.Suffix}",
                                $"CreateDate={item.CreateDate}",
                            };

                                var subscriber = new
                                {
                                    Name = RemoveSpecialCharacters(item.Salutation) ?? string.Empty,
                                    Email = (item.Email) ?? string.Empty,
                                    Mobile = string.Empty,
                                    CustomFields = customFields
                                };

                                subscribers.Add(subscriber);
                            }

                            var jsonContent = JsonConvert.SerializeObject(new
                            {
                                HasExternalDoubleOptIn = true,
                                Subscribers = subscribers
                            });

                            var responseData = Task.Run(() => _multipleDataAsync(jsonContent)).Result;

                            var response = JsonConvert.DeserializeObject<dynamic>(responseData);

                            Sitecore.Diagnostics.Log.Error($"{nameof(ProxyVoteMemberMoosend)}: Error ProxyVote Member Synce", response.Error, this);

                            // Handling the response as needed
                            var membersResponse = new List<VoteMultiResponse>();

                            foreach (var item in response.Context)
                            {
                                var member = new VoteMultiResponse
                                {
                                    ID = item.ID.ToString(),
                                    Email = item.Email.ToString()
                                };
                                membersResponse.Add(member);
                            }

                            // Update
                            _AFIReportRepository.UpdateMoosendProxyMemberSync(membersResponse);

                            Thread.Sleep(10000);

                            currentIndex += batchSize;
                        }

                    }
                }
                while (dataList != null && dataList.Any()); // Repeat if there's data left to synchronize

                return true;
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error($"{nameof(ProxyVoteMemberMoosend)}: Error ProxyVote Member Synce", ex, this);
                return false;
            }
        }

        // Method to remove special characters from a string
        private string RemoveSpecialCharacters(string input)
        {
            if (string.IsNullOrEmpty(input))
                return input;

            // Define characters to be removed
            char[] specialChars = { '<', '>', '"', '\'', '%', '&', '+', '?' };

            // Remove special characters from the string
            return new string(input.Where(c => !specialChars.Contains(c)).ToArray());
        }

        private async Task<string> _multipleDataAsync(string item)
        {
            using (var httpClient = new HttpClient { BaseAddress = baseAddress })
            {
                httpClient.DefaultRequestHeaders.TryAddWithoutValidation("accept", "application/json");

                using (var content = new StringContent(item, System.Text.Encoding.Default, "application/json"))
                {
                    using (var response = await httpClient.PostAsync($"subscribers/{emailListID}/subscribe_many.json?apiKey={SitecoreSendApiKey}", content))
                    {
                        return await response.Content.ReadAsStringAsync();
                    }
                }
            }
        }
        //private async Task _multipleDataAsync(string item)
        //{
        //    using (var httpClient = new HttpClient { BaseAddress = baseAddress })
        //    {

        //        httpClient.DefaultRequestHeaders.TryAddWithoutValidation("accept", "application/json");

        //        using (var content = new StringContent(item, System.Text.Encoding.Default, "application/json"))
        //        {
        //            using (var response = await httpClient.PostAsync($"subscribers/{emailListID}/subscribe_many.json?apiKey={SitecoreSendApiKey}", content))
        //            {
        //                string responseData = await response.Content.ReadAsStringAsync();
        //            }
        //        }
        //    }
        //}

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


    }
}
