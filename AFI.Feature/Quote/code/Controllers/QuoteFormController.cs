using AFI.Feature.Data.DataModels.Extensions;
using AFI.Feature.Data.Repositories;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Repository;
using AFI.Feature.QuoteForm.Areas.AFIWEB.ModelBuilders;
using AFI.Feature.QuoteForm.Areas.AFIWEB.Repository;
using AFI.Feature.QuoteForm.Areas.AFIWEB.Services;
using AFI.Feature.QuoteForm.Areas.AFIWEB.Models;
using AFI.Foundation.Helper.Models;
using System.Net.Http;
using System.Threading.Tasks;
using HtmlAgilityPack;
using AFI.Foundation.Helper.Repositories;
using System.Text;
using System.IO;
using CsvHelper;
using AFI.Foundation.Helper;
using System.Collections.Specialized;
using Sitecore.Mvc.Extensions;
using System.Globalization;
using System.Net;

namespace AFI.Feature.Quote.Controllers
{
    public class QuoteFormController : Controller
    {
        private readonly IBusinessQuoteFormRepository _businessRepository;
        private readonly ICommonRepository _commonRepository;
        private readonly IFloodQuoteFormRepository _floodRepository;
        private readonly ICollectorVehicleQuoteFormRepository _CollectorVehicleFormRepository;
        private readonly ICondoQuoteFormRepository _CondoFormRepository;
        private readonly IMotorcycleQuoteFormRepository _motorCycleRepository;
        private readonly IMobilehomeQuoteFormRepository _mobilehomeFormRepository;
        private readonly IWatercraftQuoteFormRepository _watercraftRepository;
        private readonly IMotorhomeQuoteFormRepository _motorhomeRepository;


        private readonly IHillAFBQuoteFormRepository _hillafbRepository;
        private readonly IContactUsFormViewModelBuilder _contactUsFormVmb;
        private readonly IFormRepository _FormRepository;

        private readonly IQuoteLeadFormRepository _quoteLeadRepository;

        private readonly string AnalyticsQueryStringKey = "analytics:querystring";
        private readonly string AnalyticsRefererKey = "analytics:referrer";

        Repository repositoryHelper = new Repository();

        public QuoteFormController(IBusinessQuoteFormRepository businessRepository, ICommonRepository commonRepository, IFloodQuoteFormRepository floodRepository, ICollectorVehicleQuoteFormRepository collectorVehicleFormRepository, ICondoQuoteFormRepository condoFormRepository, IMotorcycleQuoteFormRepository motorCycleRepository, IMobilehomeQuoteFormRepository mobilehomeFormRepository, IWatercraftQuoteFormRepository watercraftRepository, IMotorhomeQuoteFormRepository motorhomeRepository, IHillAFBQuoteFormRepository hillafb, IContactUsFormViewModelBuilder contactUsFormVmb
            , IFormRepository formRepository, IQuoteLeadFormRepository quoteLeadRepository)
        {
            _businessRepository = businessRepository;
            _commonRepository = commonRepository;
            _floodRepository = floodRepository;
            _CollectorVehicleFormRepository = collectorVehicleFormRepository;
            _CondoFormRepository = condoFormRepository;
            _motorCycleRepository = motorCycleRepository;
            _mobilehomeFormRepository = mobilehomeFormRepository;
            _watercraftRepository = watercraftRepository;
            _motorhomeRepository = motorhomeRepository;

            _hillafbRepository = hillafb;
            _contactUsFormVmb = contactUsFormVmb;
            _FormRepository = formRepository;
            _quoteLeadRepository = quoteLeadRepository;
        }

        public JsonResult GetMilitaryRanks(string type)
        {
            var ranks = _commonRepository.FillRanks(type);

            var json = JsonConvert.SerializeObject(ranks.Options);
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetStatelist()
        {
            var statelist = _commonRepository.GetStateList();
            var json = JsonConvert.SerializeObject(statelist);
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetLeadStatelist()
        {
            var statelist = _commonRepository.GetLeadStateList();
            var json = JsonConvert.SerializeObject(statelist);
            return Json(json, JsonRequestBehavior.AllowGet);
        }
        public ActionResult HomepageQuoteFormSubmit(string type)
        {
           

            string _MCode = ""; string _ResponseType = ""; string _Ctype = ""; string _Url = "";
            var pageItem = Sitecore.Context.Item;
            if (pageItem != null)
            {
                _MCode = pageItem[FeatureTemplate.MarketingSection.MarketingCode];
                _ResponseType = pageItem[FeatureTemplate.MarketingSection.ResponseType];
            }
            if (!string.IsNullOrEmpty(type))
            {
                var form = _commonRepository.GetInsuranceLink(type);
                if (form.Count > 0)
                {
                    _Ctype = form.Keys.ElementAt(0);
                    _Url = form[_Ctype];

                }

                string queryString = $"?CoverageType=" + _Ctype;

                if (!string.IsNullOrEmpty(_MCode))
                {
                    queryString = ProcessAnalytics(queryString, _MCode, _ResponseType);

                }
                return Redirect(_Url + queryString);

            }
            else
            {
                return Redirect("~");
            }

        }
        private string ProcessAnalytics(string queryString, string marketingCode, string responseType)
        {
            try
            {
                var session = HttpContext.Session;
                if (session?[AnalyticsRefererKey] != null)
                {
                    Request.Headers.Add("Referer", session[AnalyticsRefererKey].ToString());
                }
                if (session?[AnalyticsQueryStringKey] != null)
                {
                    NameValueCollection queryStrings = (NameValueCollection)session[AnalyticsQueryStringKey];

                    if (!string.IsNullOrWhiteSpace(marketingCode))
                    {
                        //INFO: Session values are read-only
                        NameValueCollection marketingCodeCollection = new NameValueCollection { queryStrings };
                        marketingCodeCollection["ResponseType"] = "Internet%20Advertising";
                        marketingCodeCollection["ResponseDescription"] = marketingCode;
                        queryString += string.Concat(marketingCodeCollection.ToKeyValues().Select(c => $"&{c.Key}={c.Value}"));
                    }
                    else
                    {
                        queryString += $"&{queryStrings}";
                    }
                }
                else if (!string.IsNullOrWhiteSpace(marketingCode))
                {
                    queryString += responseType != string.Empty ? "&ResponseType=" + responseType : "&ResponseType=Internet%20Advertising";
                    queryString += $"&ResponseDescription={marketingCode}";
                }
            }
            catch (Exception)
            {


            }


            return queryString;
        }
        public ActionResult BusinessForm()
        {
            var data = _businessRepository.GetForm();
            return View("/Views/AFI/Quote/BusinessForm.cshtml", data);
        }
        public ActionResult CollectorVehicleForm()
        {
            var data = _CollectorVehicleFormRepository.GetForm();
            return View("~/Views/AFI/Quote/CollectorVehicleForm.cshtml", data);
        }

        public ActionResult FloodForm()
        {
            var data = _floodRepository.GetForm();
            return View("~/Views/AFI/Quote/FloodForm.cshtml", data);
        }
        public ActionResult CondoForm()
        {
            var data = _CondoFormRepository.GetForm();
            return View("~/Views/AFI/Quote/CondoForm.cshtml", data);
        }
        public ActionResult MotorCycleForm()
        {
            var data = _motorCycleRepository.GetForm();
            return View("~/Views/AFI/Quote/MotorCycleForm.cshtml", data);
        }
        public ActionResult MobilehomeForm()
        {
            var data = _mobilehomeFormRepository.GetForm();
            return View("~/Views/AFI/Quote/MobileHomeForm.cshtml", data);
        }
        public ActionResult WaterCraftForm()
        {
            var data = _watercraftRepository.GetForm();
            return View("~/Views/AFI/Quote/WatercraftForm.cshtml", data);
        }
        public ActionResult MotorHomeForm()
        {
            var data = _motorhomeRepository.GetForm();
            return View("~/Views/AFI/Quote/MotorhomeForm.cshtml", data);
        }
        public ActionResult HillAFBForm()
        {
            var data = _hillafbRepository.GetForm();
            return View("~/Views/AFI/Quote/AFB.cshtml", data);
        }
        public ActionResult ContactUsForm()
        {
            var data = _contactUsFormVmb.Build();
            return View("~/Views/AFI/Contact/ContactUsForm.cshtml", data);
        }
        public ActionResult ReferralForm()
        {
            ReferralFormView referral = new ReferralFormView();
            referral.Prefixes = _FormRepository.GetAllPrefixes();
            return View("~/Views/AFI/Quote/ReferralForm.cshtml", referral);
        }
        public ActionResult QuoteLead()
        {
            var form = _quoteLeadRepository.GetForm();
            return View("~/Views/AFI/Quote/QuoteLead.cshtml", form);
        }

        public JsonResult GetIpLog()
        {
            var data = repositoryHelper.GetAllIPAddressInfo();
            return Json(data, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetIPAddressInfoCountryWiseCount()
        {
            var data = repositoryHelper.GetIPAddressInfoCountryWiseCount();
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetIPAddressInfoCountryWiseCountBySearch(string country = null, DateTime? fromDate = null, DateTime? toDate = null)
        {
            var data = repositoryHelper.GetIPAddressInfoCountryWiseCount(country, fromDate, toDate);
            return Json(data, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ExportAllIPAddressInfoToCsv(string country = null, DateTime? fromDate = null, DateTime? toDate = null)
        {
            Response.Clear();
            Response.ContentType = "text/csv";
            Response.AddHeader("Content-Disposition", "attachment; filename=IPAddressInfo.csv");

            try
            {
                var recordsPerPage = 5000; // Adjust this value based on performance testing
                var pageNumber = 1;
                var totalCount = 0;
                var data = repositoryHelper.GetAllIPAddressInfo(country, fromDate, toDate).Select(d => new { d.IP, d.Country, d.City,d.State,d.PostalCode,d.FormattedAddedDate });

                while (data.Skip((pageNumber - 1) * recordsPerPage).Any())
                {
                    var batchData = data.Skip((pageNumber - 1) * recordsPerPage).Take(recordsPerPage).ToList();

                    using (var csvWriter = new CsvWriter(Response.Output, CultureInfo.InvariantCulture))
                    {
                        csvWriter.WriteRecords(batchData);
                    }

                    totalCount += batchData.Count;
                    pageNumber++;
                }

                return new EmptyResult();
            }
            catch (Exception ex)
            {
                // Log or handle any exceptions
                return new HttpStatusCodeResult(HttpStatusCode.InternalServerError);
            }
        }




    }
}