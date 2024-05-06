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


        public bool Execute(int count, string listId)
        {
            try
            {
                MoosendSettingItem();

                emailListID = listId; 


                List<ProxyVoteMember> dataList = _AFIReportRepository.GetAllVotingMemberDataByCount(count);// Load All Member Data

                if (dataList != null)
                {
                    int batchSize = 500;
                    int currentIndex = 0;

                    while (currentIndex < dataList.Count)
                    {

                        var batch = dataList.Skip(currentIndex).Take(batchSize);

                        // Multiple
                        var subscribers = new List<object>();

                        foreach (ProxyVoteMember item in batch)
                        {
                            var customFields = new List<string>
                            {
                                $"MemberNumber={item.MemberNumber}",
                                $"PIN={item.PIN}",
                                $"Enabled={item.Enabled}",
                                $"AnnualReport={item.AnnualReport}",
                                $"EmailFinancials={item.EmailFinancials}",
                                $"IsEmailUpdated={item.IsEmailUpdated}",
                                $"RankAbbreviation={item.Prefix}",
                                $"Salutation={item.Salutation}",
                                $"FirstName={item.InsuredFirstName}",
                                $"LastName={item.InsuredLastName}",
                                $"ClientType={item.ClientType}",
                                $"MilitaryStatus={item.ServiceStatus}",
                                $"AddressLine1={item.MailingAddressLine1}",
                                $"AddressLine2={item.MailingAddressLine2}",
                                $"City={item.MailingCityName}",
                                $"MailingCountyName={item.MailingCountyName}",
                                $"State={item.MailingStateAbbreviation}",
                                $"PostalCode={item.MailingZip}",
                                $"Country={item.MailingCountry}",
                                $"MembershipDate={item.MembershipDate}",
                                $"YearsAsMember={item.YearsAsMember}",
                                $"Gender={item.Gender}",
                                $"Deceased={item.Deceased}",
                                $"MarketingCode={item.MarketingCode}",
                                $"ProperFirstName={item.ProperFirstName}",
                                $"MiddleName={item.MiddleName}",
                                $"Suffix={item.Suffix}",
                                $"CreateDate={item.CreateDate}",
                            };

                            var subscriber = new
                            {
                                Name = (item.Salutation) ?? string.Empty,
                                Email = (item.EmailAddress) ?? string.Empty,
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

                        var _postList = _multipleDataAsync(jsonContent);

                        // Update Members Sync to SitecoreSend
                        _AFIReportRepository.UpdateProxyMemberSync(batch);

                        currentIndex += batchSize;
                    }

                }

                return true;
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error($"{nameof(ProxyVoteMember)}: Error ProxyVote Member Synce", ex, this);
                return false;
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
