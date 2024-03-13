using AFI.Feature.WebQuoteService.Models;


using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Runtime.Caching;
using System.ServiceModel;
using System.Text;
using System.Web.Script.Serialization;
using System.Xml.Serialization;

namespace AFI.Feature.WebQuoteService.Repositories
{
    public interface IPartnerAdvisorRepository
    {
        IEnumerable<PartnerAdvisor> GetAll();
        string SendQuoteContactToClientAndMF(ContactInformation contactInfo, int quoteKey, bool isTest);
        bool QuoteIsDuplicate(int quoteKey);
        void SendActivityToEpic(int memberNumber, string state, string lob, DateTime startDate, DateTime finishedDate, bool isPossibleDup);
        void UpdateMarketingSource(int memberNumber, string offer, int quoteKey);
        void InsertAFIForm(AFIForm model);
        string GetCurrentEndpoint();
    }

    public class PartnerAdvisorRepository : IPartnerAdvisorRepository
    {
        private const string CACHE_KEY = "PARTNER_ADVISOR_CACHE";
        private const int PARTNER_ID = 4;
        private readonly string _endpointAddress;

        public PartnerAdvisorRepository() : this(ConfigurationManager.AppSettings["WEBQUOTESERVICE_ENDPOINT"]) { }

        public PartnerAdvisorRepository(string endpoint)
        {
            _endpointAddress = endpoint;
            //
            // Hard coding during testing
            //
            if (String.IsNullOrEmpty(_endpointAddress))
            {
                _endpointAddress = "http://172.16.5.74/AgentPortal/Services/WebQuoteService.svc";
            }

        }

        public IEnumerable<PartnerAdvisor> GetAll()
        {
            var output = GetPartnerAdvisorsFromCache(GetPartnerAdvisorsFromService);

            return output?.Distinct(new PartnerAdvisorEqualityComparer());
        }

        public string SendQuoteContactToClientAndMF(ContactInformation contactInfo, int quoteKey, bool isTest)
        {
            var svc = GetServiceClient();
            var contactInfoXML = "";

            using (var stringWriter = new System.IO.StringWriter())
            {
                var serializer = new XmlSerializer(contactInfo.GetType());
                serializer.Serialize(stringWriter, contactInfo);
                contactInfoXML = stringWriter.ToString();
            }
            //
            // To call this web service,  you have to be on AFI's VPN.
            //
            var deserializer = new XmlSerializer(typeof(WebQuoteServiceResponse));
            string responseXml = svc.SendQuoteContactToClientAndMF(contactInfoXML, quoteKey, false, isTest);
            WebQuoteServiceResponse response;
            using (MemoryStream stream = new MemoryStream(Encoding.ASCII.GetBytes(responseXml)))
            {
                response = (WebQuoteServiceResponse)deserializer.Deserialize(stream);
            }

            if (!string.Equals(response.ResponseCode, "SUCCESS", StringComparison.InvariantCultureIgnoreCase))
            {
                throw new ArgumentException(response.ResponseCode);
            }

            ContactInformationResponse responseData = new ContactInformationResponse();
            deserializer = new XmlSerializer(typeof(ContactInformationResponse));
            using (var stream = new MemoryStream(Encoding.ASCII.GetBytes(response.ResponseData)))
            {
                responseData = (ContactInformationResponse)deserializer.Deserialize(stream);
            }

            return responseData.ContactInfo.CNTCGroupID.ToString();
        }

        public bool QuoteIsDuplicate(int quoteKey)
        {
            var svc = GetServiceClient();
            return svc.QuoteIsDuplicate(quoteKey);
        }

        public void SendActivityToEpic(int memberNumber, string state, string lob, DateTime startDate, DateTime finishedDate, bool isPossibleDup)
        {
            var svc = GetServiceClient();
            svc.SendQuoteActivityToEpic(state, lob, memberNumber, true, startDate, finishedDate, isPossibleDup);
        }

        public void UpdateMarketingSource(int memberNumber, string offer, int quoteKey)
        {
            var svc = GetServiceClient();
            svc.UpdateMktgSource(memberNumber, offer, quoteKey);
        }

        public void InsertAFIForm(AFIForm model)
        {

            var svc = GetServiceClient();
            JavaScriptSerializer js = new JavaScriptSerializer();
            string jsonData = js.Serialize(model);
            Sitecore.Diagnostics.Log.Info("Mehedi > WebserviceURL : " + jsonData, "Mehedi");
            svc.InsertAfiForm(model.CNTCGroupId,
                model.QuoteId,
                model.Priority,
                model.StateAbbrv,
                model.Expidite,
                model.DisplayInformation,
                model.CreatedDate,
                model.UpdatedDate,
                model.CreationUser,
                model.UpdateUser,
                model.Email,
                model.Eligibility,
                model.CallForReview,
                model.CurrentlyAutoInsured,
                model.IsFirstCommand,
                model.ReceivedPremium,
                model.IsFinished,
                model.IsMobileManufactured,
                model.IsSlt,
                model.IsRental,
                model.IsBusinessTest,
                model.IsTest,
                model.NeedsUnderwriting,
                model.IssuedOnline,
                model.ApplicationCompleted,
                model.IsInterested,
                model.IsCondo,
                model.IsVacant,
                model.FormTypeId,
                model.CoverageTypeId,
                model.Address1,
                model.City,
                model.IsPossibleDup);
        }

        public string GetCurrentEndpoint()
        {
            try
            {
                using (var client = GetServiceClient())
                {
                    return client.Endpoint.Address.Uri.ToString();
                }
            }
            catch
            {
                return _endpointAddress;
            }
        }

        private IEnumerable<PartnerAdvisor> GetPartnerAdvisorsFromService()
        {
            List<PartnerAdvisor> output = null;

            try
            {
                using (var client = GetServiceClient())
                {
                    var partnerEmployeeList = client.GetPartnerEmployeeList(PARTNER_ID);

                    if (partnerEmployeeList.Any())
                    {
                        output = new List<PartnerAdvisor>();

                        foreach (var item in partnerEmployeeList)
                        {
                            output.Add(new PartnerAdvisor()
                            {
                                Id = item.AFI_PARTNER_EMPLOYEE_ID,
                                FirstName = item.AFI_PARTNER_EMPLOYEE_FIRST_NAME,
                                MiddleInitial = item.AFI_PARTNER_EMPLOYEE_MIDDLE_INITIAL,
                                LastName = item.AFI_PARTNER_EMPLOYEE_LAST_NAME
                            });
                        }
                    }
                }
            }
            catch
            {
                throw;
            }

            return output;
        }


        private IEnumerable<PartnerAdvisor> GetPartnerAdvisorsFromCache(Func<IEnumerable<PartnerAdvisor>> factoryMethod)
        {
            if (MemoryCache.Default == null)
            {
                return null;
            }

            var cached = MemoryCache.Default.AddOrGetExisting(CACHE_KEY, new Lazy<IEnumerable<PartnerAdvisor>>(factoryMethod), new CacheItemPolicy { AbsoluteExpiration = new DateTimeOffset(DateTime.Now.AddHours(24)) }) as Lazy<IEnumerable<PartnerAdvisor>>;

            if (cached == null)
            {
                cached = new Lazy<IEnumerable<PartnerAdvisor>>(factoryMethod);
            }

            return cached.Value;
        }

        private WebQuoteService.WebQuoteServiceClient GetServiceClient()
        {
            var binding = new BasicHttpBinding(BasicHttpSecurityMode.None);
            binding.MaxReceivedMessageSize = 1073741824;
            var endpoint = new EndpointAddress(_endpointAddress);
            Sitecore.Diagnostics.Log.Info("Mehedi > WebserviceURL : " + endpoint, "Mehedi");
            return new WebQuoteService.WebQuoteServiceClient(binding: binding, remoteAddress: endpoint);
        }


    }
}
