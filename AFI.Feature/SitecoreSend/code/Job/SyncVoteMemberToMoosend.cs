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
    public class SyncVoteMemberToMoosend
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
     

        public bool Execute()
        {
            try
            {
                MoosendSettingItem();

                string latestVotingPeriodTitle = _AFIReportRepository.GetLatestVotingPeriodTitle();

                string ResponseMessage = "";
                var exEmailListData = _AFIReportRepository.GetEmailListDataBylistName(latestVotingPeriodTitle);
                if (exEmailListData == null)
                {
                    var data = new
                    {
                        Name = latestVotingPeriodTitle,
                        ConfirmationPage = string.Empty,
                        RedirectAfterUnsubscribePage = string.Empty
                    };

                    var responseData = service.MailingListCreate(data, baseAddress, SitecoreSendApiKey).Result;
                    MoosendResponseModel response = JsonConvert.DeserializeObject<MoosendResponseModel>(responseData);

                    if (!string.IsNullOrEmpty(response.Context))
                    {
                        EmailListData emailListData = new EmailListData();
                        emailListData.ListName = latestVotingPeriodTitle;
                        emailListData.ListId = response.Context;
                        emailListData.CreatedBy = "System";
                        emailListData.CreatedDate = DateTime.Now;
                        ResponseMessage = MoosendMessage.Success;

                        _AFIReportRepository.InsertEmailListData(emailListData);

                        emailListID = response.Context;
                    }
                    else
                    {
                        ResponseMessage = response.Error;
                        // return false;
                    }

                }
                else
                {
                    emailListID = exEmailListData.ListId;
                }

                string jsonBody = "";
                ProxyVoteMember member = new ProxyVoteMember();

                JObject jsonData = JObject.FromObject(new
                {
                    member.MemberNumber,
                    member.PIN,
                    member.VotingPeriodId,
                    member.Enabled,
                    member.ResidentialOccupied,
                    member.ResidentialDwelling,
                    member.Renters,
                    member.Flood,
                    member.Life,
                    member.PersonalLiabilityRenters,
                    member.PersonalLiabilityCatastrophy,
                    member.Auto,
                    member.RV,
                    member.Watercraft,
                    member.Motorcycle,
                    member.Supplemental,
                    member.AnnualReport,
                    member.StatutoryFinancialStatements,
                    member.MobileHome,
                    member.PetHealth,
                    member.Business,
                    member.LongTermCare,
                    member.MailFinancials,
                    member.EmailFinancials,
                    member.IsEmailUpdated,
                    member.Prefix,
                    member.Salutation,
                    member.InsuredFirstName,
                    member.InsuredLastName,
                    member.ClientType,
                    member.ServiceStatus,
                    member.MailingAddressLine1,
                    member.MailingAddressLine2,
                    member.MailingCityName,
                    member.MailingCountyName,
                    member.MailingStateAbbreviation,
                    member.MailingZip,
                    member.MailingCountry,
                    member.MembershipDate,
                    member.YearsAsMember,
                    member.Gender,
                    member.Deceased,

                    member.MarketingCode,
                    member.ProperFirstName,
                    member.MiddleName,
                    member.Suffix,

                    member.CreateDate

                });

                string name = string.Empty;
                string email = string.Empty;
                string mobile = string.Empty;

                JObject json = JObject.FromObject(new
                {
                    Email = email,
                    Name = name,
                    Mobile = mobile,
                    CustomFields = jsonData

                });
                jsonBody = JsonConvert.SerializeObject(json);

                JObject jsonObject = JObject.Parse(jsonBody);

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

                        var responseData = Task.Run(() => service.CustomCreate(data, emailListID, baseAddress, SitecoreSendApiKey)).Result; // New/ Existing ListId

                        MoosendResponseModel response = JsonConvert.DeserializeObject<MoosendResponseModel>(responseData);

                        if (response.Context != null)
                        {
                            KeyResponseMessage = MoosendMessage.Success;
                        }
                        else
                        {
                            KeyResponseMessage = response.Error;
                            //  return false;
                        }

                    }
                }


                List<ProxyVoteMember> dataList = _AFIReportRepository.GetAllVotingMemberData();// Load All Member Data

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
                                $"VotingPeriodId={item.VotingPeriodId}",
                                $"Enabled={item.Enabled}",
                                $"ResidentialOccupied={item.ResidentialOccupied}",
                                $"ResidentialDwelling={item.ResidentialDwelling}",
                                $"Renters={item.Renters}",
                                $"Flood={item.Flood}",
                                $"Life={item.Life}",
                                $"PersonalLiabilityRenters={item.PersonalLiabilityRenters}",
                                $"PersonalLiabilityCatastrophy={item.PersonalLiabilityCatastrophy}",
                                $"Auto={item.Auto}",
                                $"RV={item.RV}",
                                $"Watercraft={item.Watercraft}",
                                $"Motorcycle={item.Motorcycle}",
                                $"Supplemental={item.Supplemental}",
                                $"AnnualReport={item.AnnualReport}",
                                $"StatutoryFinancialStatements={item.StatutoryFinancialStatements}",
                                $"MobileHome={item.MobileHome}",
                                $"PetHealth={item.PetHealth}",
                                $"Business={item.Business}",
                                $"LongTermCare={item.LongTermCare}",
                                $"MailFinancials={item.MailFinancials}",
                                $"EmailFinancials={item.EmailFinancials}",
                                 $"IsEmailUpdated={item.IsEmailUpdated}",
                                $"Prefix={item.Prefix}",
                                $"Salutation={item.Salutation}",
                                $"InsuredFirstName={item.InsuredFirstName}",
                                $"InsuredLastName={item.InsuredLastName}",
                                $"ClientType={item.ClientType}",
                                $"ServiceStatus={item.ServiceStatus}",
                                $"MailingAddressLine1={item.MailingAddressLine1}",
                                $"MailingAddressLine2={item.MailingAddressLine2}",
                                $"MailingCityName={item.MailingCityName}",
                                $"MailingCountyName={item.MailingCountyName}",
                                $"MailingStateAbbreviation={item.MailingStateAbbreviation}",
                                $"MailingZip={item.MailingZip}",
                                $"MailingCountry={item.MailingCountry}",
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
                                Name = (item.FullName) ?? string.Empty,
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

                        currentIndex += batchSize;

                    }

                }

                return true;
            }
            catch (Exception )
            {
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
