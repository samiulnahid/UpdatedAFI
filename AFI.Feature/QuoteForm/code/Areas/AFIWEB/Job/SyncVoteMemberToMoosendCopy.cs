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
using AFI.Feature.QuoteForm.Areas.AFIWEB.Repository;
using AFI.Feature.QuoteForm.Areas.AFIWEB.Models;
using static AFI.Feature.QuoteForm.Areas.AFIWEB.Constant;

namespace AFI.Feature.QuoteForm.Areas.AFIWEB.Job
{
    public class SyncVoteMemberToMoosendCopy
    {

        public static string api_key = string.Empty;
        public static string api_url = string.Empty;

        Service service = new Service();

        private readonly IAFIReportRepositoryX _AFIReportRepository;
        public SyncVoteMemberToMoosendCopy(IAFIReportRepositoryX AFIReportRepository)
        {
            _AFIReportRepository = AFIReportRepository;
        }
       
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
            MoosendSettingItem();

            var dataList = _AFIReportRepository.GetAllVotingMemberData();// Load All Member Data
            var groupedItems = dataList.GroupBy(item => item.VotingPeriodId);

            foreach (var group in groupedItems)
            {
                string ResponseMessage = "";

                var votingPeriod = _AFIReportRepository.GetVotingPeriodById(group.Key);
                var listId = "";
                var exEmailListData = _AFIReportRepository.GetEmailListDataByVotingPeriod(votingPeriod.Title); 

                if (exEmailListData == null)
                {
                    var data = new
                    {
                        Name = votingPeriod.Title,
                        ConfirmationPage = string.Empty,
                        RedirectAfterUnsubscribePage = string.Empty
                    };

                    var responseData = service.MailingListCreate(data, baseAddress, SitecoreSendApiKey).Result;
                    MoosendResponseModel response = JsonConvert.DeserializeObject<MoosendResponseModel>(responseData);

                    if (!string.IsNullOrEmpty(response.Context))
                    {
                        EmailListData emailListData = new EmailListData();
                        emailListData.ListName = votingPeriod.Title;
                        emailListData.ListId = response.Context;
                        emailListData.CreatedBy = "System";
                        emailListData.CreatedDate = DateTime.Now;
                        ResponseMessage = MoosendMessage.Success;

                        _AFIReportRepository.InsertEmailListData(emailListData);

                        listId = response.Context;
                    }
                    else
                    {
                        ResponseMessage = response.Error;
                        return false;
                    }

                }
                else
                {
                    listId = exEmailListData.ListId;
                }

                foreach (var item in group.Take(1))
                {
                    string jsonBody = "";
                    if (item != null)
                    {
                        JObject jsonData = JObject.FromObject(new
                        {
                            MemberNumber = item.MemberNumber,
                            PIN = item.PIN,
                            VotingPeriodId = item.VotingPeriodId,
                            ResidentialOccupied = item.ResidentialOccupied,
                            ResidentialDwelling = item.ResidentialDwelling,
                            Renters = item.Renters,
                            Flood = item.Flood,
                            Life = item.Life,
                            PersonalLiabilityRenters = item.PersonalLiabilityRenters,
                            PersonalLiabilityCatastrophy = item.PersonalLiabilityCatastrophy,
                            RV = item.RV,
                            Auto = item.Auto,
                            Watercraft = item.Watercraft,
                            Motorcycle = item.Motorcycle,
                            Supplemental = item.Supplemental,
                            AnnualReport = item.AnnualReport,

                            StatutoryFinancialStatements = item.StatutoryFinancialStatements,
                            MobileHome = item.MobileHome,
                            PetHealth = item.PetHealth,
                            Business = item.Business,
                            LongTermCare = item.LongTermCare,
                            MailFinancials = item.MailFinancials,
                            EmailFinancials = item.EmailFinancials

                        });

                        string name = (item.FullName) ?? string.Empty;
                        string email = (item.EmailAddress) ?? string.Empty;

                        JObject json = JObject.FromObject(new
                        {
                            Email = email,
                            Name = name,
                            CustomFields = jsonData

                        });
                        jsonBody = JsonConvert.SerializeObject(json);

                    }

                    JObject jsonObject = JObject.Parse(jsonBody);

                    // Extract keys recursively
                    var keys = GetKeys(jsonObject);
                    foreach (var key in keys)
                    {
                        string KeyResponseMessage = "";
                        Console.WriteLine(key);
                        if (key != null && !new[] { "Email", "Name", "CustomFields" }.Contains(key.ToString()))
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
                                return false;
                            }

                        }
                    }

                }
            }

            foreach (var group in groupedItems)
            {
                var votingPeriod = _AFIReportRepository.GetVotingPeriodById(group.Key);
                EmailListData emailListData = _AFIReportRepository.GetEmailListDataByVotingPeriod(votingPeriod.Title);

                foreach (var item in group)
                {
                    string ResponseMessage = "";
                    string SubscriberId = "";
                  //  bool isSynced = item.IsSynced;
                    string jsonBody = "";
                    if (item != null)
                    {
                        JObject jsonData = JObject.FromObject(new
                        {
                            MemberNumber = item.MemberNumber,
                            PIN = item.PIN,
                            VotingPeriodId = item.VotingPeriodId,
                            ResidentialOccupied = item.ResidentialOccupied,
                            ResidentialDwelling = item.ResidentialDwelling,
                            Renters = item.Renters,
                            Flood = item.Flood,
                            Life = item.Life,
                            PersonalLiabilityRenters = item.PersonalLiabilityRenters,
                            PersonalLiabilityCatastrophy = item.PersonalLiabilityCatastrophy,
                            RV = item.RV,
                            Auto = item.Auto,
                            Watercraft = item.Watercraft,
                            Motorcycle = item.Motorcycle,
                            Supplemental = item.Supplemental,
                            AnnualReport = item.AnnualReport,

                            StatutoryFinancialStatements = item.StatutoryFinancialStatements,
                            MobileHome = item.MobileHome,
                            PetHealth = item.PetHealth,
                            Business = item.Business,
                            LongTermCare = item.LongTermCare,
                            MailFinancials = item.MailFinancials,
                            EmailFinancials = item.EmailFinancials

                        });

                        string name = (item.FullName) ?? string.Empty;
                        string email = (item.EmailAddress) ?? string.Empty;

                        JObject json = JObject.FromObject(new
                        {
                            Email = email,
                            Name = name,
                            CustomFields = jsonData

                        });
                        jsonBody = JsonConvert.SerializeObject(json);

                    }

                    var currentData = JsonConvert.DeserializeObject<dynamic>(jsonBody);
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
                    }
                    else
                    {
                        ResponseMessage = response.Error;
                        return false;
                    }
                   
                   
                }
            }

            return true;
        }
        private object CreateFinalData(dynamic currentData, bool hasExternalDoubleOptIn, List<string> customFields)
        {
            return new
            {
                Name = currentData.Name,
                Email = currentData.Email,
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


    }
}
