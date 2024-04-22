using System;
using System.Collections.Generic;
using System.Linq;

using System.Web.Mvc;
using AFI.Feature.QuoteForm.Areas.AFIWEB.Repository;
using AFI.Feature.QuoteForm.Areas.AFIWEB.Models;
using System.Text;
using AFI.Feature.QuoteForm.Areas.AFIWEB.Helpers.Configurations;
using System.Text.RegularExpressions;
using AFI.Feature.QuoteForm.Areas.AFIWEB.Helpers.Strings;
using AFI.Feature.QuoteForm.Areas.AFIWEB.Services;
using Newtonsoft.Json;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models;
using AFI.Feature.QuoteForm.Areas.AFIWEB.Models.Form_Models;
using System.Net.Mail;
using AFI.Feature.WebQuoteService.WebQuoteService;
using Org.BouncyCastle.OpenSsl;
using Sitecore.Data.Query;
using Sitecore.Shell.Framework.Commands;
using Com.Alacriti.Checkout;
using Com.Alacriti.Checkout.Client;
using Com.Alacriti.Checkout.Util;
using Com.Alacriti.Checkout.Model;
using Newtonsoft.Json.Linq;
using Org.BouncyCastle.Crypto;
using context = System.Web.HttpContext;
using System.Threading.Tasks;
using AFI.Feature.WebQuoteService;

using System.Globalization;
using Sitecore.Data.Items;
using System.Net;
using System.Configuration;
using System.Net.Http;
using Sitecore.Data;
using Sitecore.Configuration;
using Sitecore.Analytics.Data.Bulk;
using Microsoft.Ajax.Utilities;
using System.IO;
using AFI.Feature.QuoteForm.Areas.AFIWEB.Models;
using AFI.Feature.QuoteForm.ExperienceForms;
using Sitecore.Web.UI.HtmlControls;
using AFI.Feature.Data.Repositories;
using Sitecore.Diagnostics;

namespace AFI.Feature.QuoteForm.Areas.AFIWEB.Controllers
{

    public class AFIReportController : Controller
    {
        private readonly IAFIReportRepository _AFIReportRepository;
        private readonly ICorviasFormRepository _CorviasFormRepository;
        private readonly IUCFormRepository _UCFormRepository;
        private readonly IEmailService _emailService;
        private readonly IHillAFBFormRepository _HillAFBFormRepository;
        private readonly IFormRepository _formRepository;

        public AFIReportController(IAFIReportRepository AFIReportRepository, IEmailService emailService, ICorviasFormRepository corviasFormRepository, IUCFormRepository uCFormRepository, IHillAFBFormRepository hillAFBFormRepository, IFormRepository formRepository)
        {
            _AFIReportRepository = AFIReportRepository;
            _emailService = emailService;
            _CorviasFormRepository = corviasFormRepository;
            _UCFormRepository = uCFormRepository;
            _HillAFBFormRepository = hillAFBFormRepository;
            _formRepository = formRepository;
        }
        public ActionResult Report()
        {
            var data = _AFIReportRepository.GetAll();
            return View(data);
        }
        public ActionResult UpdatedReport(string startDate = "", string endDate = "")
        {
            var data = _AFIReportRepository.GetUpdateReports(startDate, endDate);
            var performance = _AFIReportRepository.GetSectionPerformance(startDate, endDate);

            AFIReportView aFIReport = new AFIReportView();
            aFIReport.UpdatedReports = data;
            aFIReport.SectionPerformances = performance;

            return View("/Areas/AFIWEB/Views/AFIReport/UpdatedReport.cshtml", aFIReport);
        }
        public ActionResult UpdatedReportDetails(string startDate = "", string endDate = "", string coverageType = "")
        {
            var coverageInfo = _AFIReportRepository.GetUpdateReports(startDate, endDate, coverageType);
            var coverageDetailsList = _AFIReportRepository.GetUpdateReportDetails(coverageType, startDate, endDate);

            AFIReportView aFIReport = new AFIReportView();
            aFIReport.CoverageInfo = coverageInfo != null ? coverageInfo.SingleOrDefault() : new ReportView();
            aFIReport.UpdatedReports = coverageDetailsList;

            return View("/Areas/AFIWEB/Views/AFIReport/UpdatedReportDetails.cshtml", aFIReport);
        }
        public ActionResult GetQuoteHOReport(string startDate = "", string endDate = "")
        {
            var csvReports = _AFIReportRepository.GetQHActivityTabularReport(startDate, endDate);

            AFIReportView aFIReport = new AFIReportView();
            aFIReport.QHCSVReports = csvReports;

            return View("/Areas/AFIWEB/Views/AFIReport/QuoteHubReport.cshtml", aFIReport);
        }
        public ActionResult GetQuoteHubActivity(string startDate = "", string endDate = "")
        {
            var homeOwnerReports = _AFIReportRepository.GetQHSummaryReport(startDate, endDate);
            var hoActivity = _AFIReportRepository.GetQHActivity(startDate, endDate);

            AFIReportView aFIReport = new AFIReportView();
            aFIReport.QuotehubReports = homeOwnerReports;
            aFIReport.QHActivityReports = hoActivity;


            return View("/Areas/AFIWEB/Views/AFIReport/QuoteHubActivityReport.cshtml", aFIReport);
        }
        public ActionResult GetQuoteHOReportDetails(string startDate = "", string endDate = "", string coverageType = "", string formState = "")
        {
            var csvReports = _AFIReportRepository.GetQHActivityTabularReport(startDate, endDate);
            if (!string.IsNullOrEmpty(formState))
            {
                csvReports = csvReports.Where(x => x.FormState == formState).ToList();
            }
            else
            {
                if (string.IsNullOrEmpty(coverageType))
                {
                    csvReports = csvReports.Where(x => x.FormState == null).ToList();
                }
            }

            AFIReportView aFIReport = new AFIReportView();
            aFIReport.QHCSVReports = csvReports;

            return View("/Areas/AFIWEB/Views/AFIReport/QuoteHubReportDetails.cshtml", aFIReport);
        }
        public ActionResult AFIEXMUnSubscription(string email = "", string campaign = "", string memberid = "")
        {
            EXMUnSubscription model = new EXMUnSubscription();
            if (!string.IsNullOrEmpty(email))
            {
                model.Email = email;
                model.Campaign = campaign;
                model.Status = "Requested";
                model.Date = DateTime.Now;
                model.MemberID = memberid;
                var checkStatus = _AFIReportRepository.CheckStatus(model.Email);
                if (checkStatus != null && !string.IsNullOrEmpty(checkStatus.Status) && checkStatus.Status.ToLower() == "requested")
                {
                    model.Status = "You already have a pending request!";
                    model.Id = checkStatus.Id;
                }
                else
                {
                    var insertData = _AFIReportRepository.InsertData(model);
                    if (insertData >= 0)
                    {
                        model.Id = insertData;
                        model.Status = "Your requested has been recorded!";
                    }
                }
            }
            return View("/Views/AFI/Forms/UnSubscription.cshtml", model);
        }
        public ActionResult GetTabularReport(string startDate = "", string endDate = "", string coverageType = "", string type = "")
        {


            var csvReports = _AFIReportRepository.GetQHActivityTabularReport(startDate, endDate);

            AFIReportView aFIReport = new AFIReportView();
            aFIReport.QHCSVReports = csvReports;

            if (!string.IsNullOrEmpty(type))
            {
                string csv = ListToCSV(aFIReport.QHActivityReports);

                return File(new System.Text.UTF8Encoding().GetBytes(csv), "text/csv", "report.csv");
            }
            else
            {
                return View("/Areas/AFIWEB/Views/AFIReport/TabularReport.cshtml", aFIReport);

            }

        }
        public FileContentResult ExtractToCSV(string startDate = "", string endDate = "", string coverageType = "")
        {

            IEnumerable<QHCSVReport> QuotehubReports = _AFIReportRepository.GetQHActivityTabularReport(startDate, endDate);

            string csv = ListToCSV(QuotehubReports);
            return File(new System.Text.UTF8Encoding().GetBytes(csv), "text/csv", "report.csv");
        }

        private string ListToCSV<T>(IEnumerable<T> list)
        {
            StringBuilder sList = new StringBuilder();

            Type type = typeof(T);
            var props = type.GetProperties();
            sList.Append(string.Join(",", props.Select(p => p.Name)));
            sList.Append(Environment.NewLine);

            foreach (var element in list)
            {
                sList.Append(string.Join(",", props.Select(p => p.GetValue(element, null))));
                sList.Append(Environment.NewLine);
            }

            return sList.ToString();
        }
        public ActionResult GetVoteCountReport(string voatingPeriodId = "", string type = "")
        {

            var coverageDetailsList = _AFIReportRepository.GetVoteCountReport(voatingPeriodId);
            AFIReportView aFIReport = new AFIReportView();
            aFIReport.VoteReports = coverageDetailsList;
            if (!string.IsNullOrEmpty(type))
            {
                string csv = ListToCSV(aFIReport.VoteReports);

                return File(new System.Text.UTF8Encoding().GetBytes(csv), "text/csv", "report.csv");
            }
            else
            {
                return View("/Areas/AFIWEB/Views/AFIReport/VoteCountReport.cshtml", aFIReport);

            }

        }
        public FileContentResult ExtractToCSVVote(string voatingPeriodId = "")
        {
            if (string.IsNullOrEmpty(voatingPeriodId))
            {
                voatingPeriodId = "11";
            }
            IEnumerable<VoteReport> VoteReports = _AFIReportRepository.GetVoteCountReport(voatingPeriodId);
            string csv = ListToCSV(VoteReports);

            return File(new System.Text.UTF8Encoding().GetBytes(csv), "text/csv", "report.csv");
        }

        public ActionResult AutoReport()
        {
            var data = _AFIReportRepository.GetAllAutoReport();
            return View("/Areas/AFIWEB/Views/AFIReport/AutoReport.cshtml", data);
        }


        public ActionResult AutoReportDetails(long key)
        {
            var data = _AFIReportRepository.GetAutoDetailsByKey(key);
            return View("/Areas/AFIWEB/Views/AFIReport/AutoReportDetails.cshtml", data);
        }
        public ActionResult BusinessReport()
        {
            var data = _AFIReportRepository.GetAllBusinessReport();
            return View("/Areas/AFIWEB/Views/AFIReport/BusinessReport.cshtml", data);
        }

        public ActionResult BusinessReportDetails(long key)
        {
            var data = _AFIReportRepository.GetBusinessDetailsByKey(key);
            return View("/Areas/AFIWEB/Views/AFIReport/BusinessReportDetails.cshtml", data);
        }
        public ActionResult FloodReport()
        {
            var data = _AFIReportRepository.GetAllFloodReport();
            return View("/Areas/AFIWEB/Views/AFIReport/FloodReport.cshtml", data);
        }

        public ActionResult FloodReportDetails(long key)
        {
            var data = _AFIReportRepository.GetFloodDetailsByKey(key);
            return View("/Areas/AFIWEB/Views/AFIReport/FloodReportDetails.cshtml", data);
        }
        public ActionResult HomeReport()
        {
            var data = _AFIReportRepository.GetAllHomeReport();
            return View("/Areas/AFIWEB/Views/AFIReport/HomeReport.cshtml", data);
        }

        public ActionResult Homereportdetails(long key)
        {
            var data = _AFIReportRepository.GetHomeDetailsByKey(key);
            return View("/Areas/AFIWEB/Views/AFIReport/Homereportdetails.cshtml", data);
        }
        public ActionResult MotorcycleReport()
        {
            var data = _AFIReportRepository.GetAllMotorcycleReport();
            return View("/Areas/AFIWEB/Views/AFIReport/MotorcycleReport.cshtml", data);
        }
        public ActionResult CorviasForm()
        {
            return View("/Areas/AFIWEB/Views/AFIReport/CorviasForm.cshtml");
        }
        public ActionResult CorviasFormThanks()
        {
            return View("/Areas/AFIWEB/Views/AFIReport/Corviasthankyou.cshtml");
        }
        public JsonResult GetMilitaryRanks(string type)
        {
            List<Options> ranks = new List<Options>();
            CommonData commonData = new CommonData();
            if (type == "Army")
            { ranks = commonData.militaryRanksArmy; }
            if (type == "Army National Guard")
            { ranks = commonData.militaryRanksNationalGuard; }
            if (type == "Air Force")
            { ranks = commonData.militaryRanksAirForce; }
            if (type == "Air National Guard")
            { ranks = commonData.militaryRanksAirNationalGuard; }
            if (type == "Coast Guard")
            { ranks = commonData.militaryRanksCoastGuard; }
            if (type == "Marine Corps")
            { ranks = commonData.militaryRanksMarines; }
            if (type == "Navy")
            { ranks = commonData.militaryRanksNavy; }
            if (type == "NOAA")
            { ranks = commonData.militaryRanksNoaa; }
            if (type == "PHS")
            { ranks = commonData.militaryRanksPhs; }
            if (type == "Space Force")
            { ranks = commonData.militaryRanksSpaceForce; }

            var json = JsonConvert.SerializeObject(ranks);
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetMilitaryRanksForQH(string type)
        {
            List<Options> ranks = new List<Options>();
            CommonData commonData = new CommonData();
            if (type == "Army")
            { ranks = commonData.militaryRanksArmyQH; }
            if (type == "Army National Guard")
            { ranks = commonData.militaryRanksNationalGuardQH; }
            if (type == "Air Force")
            { ranks = commonData.militaryRanksAirForceQH; }
            if (type == "Air National Guard")
            { ranks = commonData.militaryRanksAirNationalGuardQH; }
            if (type == "Coast Guard")
            { ranks = commonData.militaryRanksCoastGuardQH; }
            if (type == "Marine Corps")
            { ranks = commonData.militaryRanksMarinesQH; }
            if (type == "Navy")
            { ranks = commonData.militaryRanksNavyQH; }
            if (type == "NOAA")
            { ranks = commonData.militaryRanksNoaaQH; }
            if (type == "Public Health Service")
            { ranks = commonData.militaryRanksPhsQH; }
            if (type == "Space Force")
            { ranks = commonData.militaryRanksSpaceForceQH; }

            var json = JsonConvert.SerializeObject(ranks);
            return Json(json, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public ActionResult CorviasFormSubmit(CorviasModel cm)
        {
            var response = _CorviasFormRepository.InsertForm(cm);
            if (response > 0)
            {
                #region EMAIL                                    
                Sitecore.Data.Items.Item emailtemplate = Sitecore.Context.Database.GetItem(CorviasMail.Email_ItemId);
                try
                {
                    // EMAIL ADMIN

                    string admin_Email_Subject = emailtemplate[CorviasMail.Admin_Email_Subject];
                    string admin_Email_Body = emailtemplate[CorviasMail.Admin_Email_Body];
                    string admin_Email_Recipients = emailtemplate[CorviasMail.Admin_Email_Recipients];
                    string admin_Email_Sender = emailtemplate[CorviasMail.Admin_Email_Sender];

                    var email = ParseEmail(admin_Email_Sender, admin_Email_Subject, admin_Email_Body, admin_Email_Recipients, cm);
                    Sitecore.Diagnostics.Log.Info($"corvias test Email Send", "Mehedi");
                    #region Test Email
                    try
                    {
                        SmtpClient smtp = new SmtpClient();
                        smtp.Host = "smtp.sendgrid.net";
                        smtp.Credentials = new System.Net.NetworkCredential("apikey", "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");
                        smtp.Port = 587;
                        smtp.EnableSsl = false;
                        smtp.Send(email.FromEmail, admin_Email_Recipients, email.Subject, email.Body);

                        Sitecore.Diagnostics.Log.Info($"corvias test email Send done", "Mehedi");
                    }
                    catch (Exception ex)
                    {

                        Sitecore.Diagnostics.Log.Info($"corvias test email Send error" + ex.Message, "Mehedi");
                    }


                    #endregion
                    Sitecore.Diagnostics.Log.Info($"corvias Email Send", "Mehedi");
                    _emailService.Send(email);
                    Sitecore.Diagnostics.Log.Info($"corvias Email Send", "Mehedi");
                    // EMAIL CLIENT
                    string email_Subject = emailtemplate[CorviasMail.Client_Confirmation_Email_Subject];
                    string email_Recipients = emailtemplate[cm.Email];
                    string email_Sender = emailtemplate[CorviasMail.Client_Confirmation_Email_Sender];
                    string email_Body = emailtemplate[CorviasMail.Client_Confirmation_Email_Body];
                    var email_client = ParseEmail(email_Sender, email_Subject, email_Body, email_Recipients, cm);
                    _emailService.Send(email_client);
                }
                catch (Exception ex) { }
                #endregion
                //  return RedirectToAction("CorviasFormThanks", "AFIReport");
            }
            //else
            //    return View("/Areas/AFIWEB/Views/AFIReport/Corviasthankyou.cshtml");
            var json = JsonConvert.SerializeObject(response);
            return Json(json, JsonRequestBehavior.AllowGet);
        }
        #region Hill AFB Form
        public ActionResult HillAFBForm()
        {
            return View("~/Views/AFI/Quote/HillAFBForm.cshtml");
        }



        [HttpPost]
        public ActionResult HillAFBFormSubmit(InsuranceApplication form)
        {

            #region OnePageInsertMemberInfo API
            DateTime dob, spouseDOB, scheduledate, time;
            string _InsDOB = "", _InsSpouseDOB = "";

            if (!string.IsNullOrEmpty(form.InsDOB))
            {
                if (DateTime.TryParseExact(form.InsDOB, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out dob))
                {
                    _InsDOB = dob.ToString("MM/dd/yyyy");
                }
            }
            if (!string.IsNullOrEmpty(form.MaritalStatusInformationBirthDate))
            {

                if (DateTime.TryParseExact(form.MaritalStatusInformationBirthDate, "yyyy-MM-dd", CultureInfo.InvariantCulture, DateTimeStyles.None, out spouseDOB))
                {
                    _InsSpouseDOB = spouseDOB.ToString("MM/dd/yyyy");
                }
            }

            DateTime.TryParseExact(form.ScheduleDate, "MM/dd/yyyy", CultureInfo.InvariantCulture, DateTimeStyles.None, out scheduledate);
            DateTime.TryParseExact(form.ScheduleTime, "h:mm tt", CultureInfo.InvariantCulture, DateTimeStyles.None, out time);

            //bool isRank = false;
            //if (!string.IsNullOrEmpty(form.MilitaryRank))
            //{
            //    isRank = !string.IsNullOrEmpty(form.Militarysalutation) && form.Militarysalutation == "on";
            //}
            //if (!string.IsNullOrEmpty(form.spousemilitaryRank))
            //{
            //    isRank = !string.IsNullOrEmpty(form.spousemilitaryRank) && form.currentmilitarysalutation == "on";
            //}


            QHAPIModel application = new QHAPIModel
            {
                InsFirstName = form.InsFirstName,
                InsLastName = form.InsLastName,
                InsDOB = !string.IsNullOrEmpty(_InsDOB) ? _InsDOB : (!string.IsNullOrEmpty(form.InsDOB) ? form.InsDOB : ""),
                InsSocialSecNumber = form.InsSocialSecNumber,
                InsPhoneNumber = form.InsPhoneNumber,
                InsEmailAddress = form.InsEmailAddress,
                InsIdEligibility = (!string.IsNullOrEmpty(form.EligibilityMilitary)) ? 21
                                    : (!string.IsNullOrEmpty(form.CurrentMilitarySpouse)) ? 22
                                    : 0,
                InsSpouseFirstName = form.MaritalStatusFirstName ?? "",
                InsSpouseLastName = form.MaritalStatusLastName ?? "",
                InsSpouseDOB = string.IsNullOrWhiteSpace(_InsSpouseDOB) ?
                   (string.IsNullOrWhiteSpace(form.MaritalStatusInformationBirthDate) ? "" : form.MaritalStatusInformationBirthDate) :
                   _InsSpouseDOB,
                InsSpouseSocialSecNumber = form.MaritalStatusssn ?? "",
                InsMailAddress1 = form.InsMailAddress1,
                InsMailAddress2 = form.InsMailAddress2 ?? "",
                InsMailZip = form.InsMailZip,
                InsMailCity = form.InsMailCity,
                InsMailState = form.InsMailState,
                InsCovToBegin = form.InsCovToBegin,
                IsBusinessOrRevenueOnPremises = (form.IsBusinessOrRevenueOnPremises == "Yes") ? 2254 : (form.IsBusinessOrRevenueOnPremises == "No") ? 2252 : 0,
                OpCovOption = (form.CoverageType == "Basic") ? "B" : (form.CoverageType == "Standard") ? "S" : (form.CoverageType == "Premium") ? "P" : "",
                CancDec5Yrs = form.CancDec5Yrs == "Yes" ? true : false,
                LockingSafety = form.LockingSafety == "Yes" ? true : false,
                AggressiveDogsBreed = form.AggressiveDogsBreed == "Yes" ? true : false,
                IsCompany = form.IsCompany == "Yes" ? true : false,
                OtherLocations = form.OtherLocations == "Yes" ? true : false,
                ScheduleDate = !string.IsNullOrEmpty(form.ScheduleDate) ? scheduledate.ToString("MM/dd/yyyy") : "",
                ScheduleTime = !string.IsNullOrEmpty(form.ScheduleTime) ? time.ToString("h:mm tt") : "",
                SusLoss3Yrs = (form.SusLoss3Yrs == "Yes") ? 2255 : (form.SusLoss3Yrs == "No") ? 2256 : 0,
                NumOfLosses = !string.IsNullOrEmpty(form.NumOfLosses) ? Convert.ToInt32(form.NumOfLosses) : 0,
                HasOpenClaims = (form.HasOpenClaims == "Yes") ? 2266 : (form.HasOpenClaims == "No") ? 2267 : 0,
                BranchOfService = form.BranchOfService ?? form.SpousebranchOfService,
                MilitaryStatus = form.MilitaryStatus ?? form.spousemilitaryStatus,
                MilitaryRank = form.MilitaryRank ?? form.spousemilitaryRank,
                MaritalStatus = form.MaritalStatus,
                Salutation = form.Salutation ?? "",
                Suffix = form.Suffix ?? "",
                SpsSalutation = form.Spousesalutation ?? "",
                SpsSuffix = form.Suffixdetailsspouse ?? "",
                IsRankAsSalutation = (!string.IsNullOrEmpty(form.MilitaryRank) && form.Militarysalutation == "on") || (!string.IsNullOrEmpty(form.spousemilitaryRank) && form.currentmilitarysalutation == "on"),//(form.Militarysalutation == "on" || form.currentmilitarysalutation == "on") ? true : false,
            };

            var _jsonBody = JsonConvert.SerializeObject(application);

            Item item = Sitecore.Context.Database.GetItem("{0521D736-A5BF-4DDC-8746-C8B83110429F}");
            string _url = "", _username = "", _password = "";
            if (item != null)
            {
                _url = item.Fields["APIURL"].Value;
                _username = item.Fields["UserName"].Value;
                _password = item.Fields["Password"].Value;
            }

            WebClient client = new WebClient();

            string encodedCredentials = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_username}:{_password}"));
            client.Headers.Add("Authorization", $"Basic {encodedCredentials}");

            // Set the content for the POST request
            string postData = _jsonBody;
            string response = "";

            // Set the content of the POST request and its data type
            client.Headers.Add("Content-Type", "application/json");

            Sitecore.Diagnostics.Log.Info("Hill AFB Content" + _jsonBody, "HillAFB Status");
            // Make the POST request
            try
            {
                response = client.UploadString(_url, postData);
            }
            catch (WebException webError)
            {
                Sitecore.Diagnostics.Log.Info("Hill AFB Content" + webError.Message, "HillAFB Status");
                // return Json(new { success = response + webError.Message }, JsonRequestBehavior.AllowGet);
            }
            #endregion

            #region Orbipay Payment API and InsertPaymentDetailsForOnePage
            //Session["confirmation_number"] = "X12ADA885";

            var responseObject = JObject.Parse(response);
            Sitecore.Diagnostics.Log.Info("Hill AFB QuoteHdr Response" + responseObject + "Is Payment" + form.IsPayment, "HillAFB Status");
            string statusCode = responseObject.GetValue("StatusCode").ToString();
            string idQuoteValue = responseObject.GetValue("idQuoteValue").ToString();
            string idRiskaddressValue = responseObject.GetValue("idRiskaddressValue").ToString();

            Session["IdRiskAddress"] = idRiskaddressValue;
            Session["IdQuote"] = idQuoteValue;
            Session["ChipMemberId"] = responseObject.GetValue("ChipMemberId").ToString();

            string idQuote = form.QuoteId; //responseObject.GetValue("idQuoteValue").ToString();
            string addressId = form.RiskAddressId; //responseObject.GetValue("idRiskaddressValue").ToString();
            string statusMessage = "";
            if (statusCode == "200" && form.IsPayment)
            {
                Sitecore.Diagnostics.Log.Info("Hill AFB Payment Start: token:" + form.token + " digiSign:" + form.digiSign + " quote: " + idQuote + " amount: " + form.Amount + " addressId" + addressId, "HillAFB Status");
                //string token = Request.Form["token"];
                //string digiSign = Request.Form["digiSign"];

                var payment = MakeAPaymentTwo(form.token, form.digiSign, idQuote, form.Amount, addressId);

                if (payment != null)
                {
                    if (payment.Error != null)
                    {
                        if (payment.Error.Count > 0)
                        {

                            foreach (var error in payment.Error)
                            {
                                statusCode = error.Code;
                                statusMessage = error.Message;
                            }
                            string message = "{\"StatusCode\":\"" + statusCode + "\",\"StatusMessage\":\"Payment Error:" + statusMessage + "\"}";
                            Sitecore.Diagnostics.Log.Info("Hill AFB payment Response" + message, "HillAFB Status");
                            return Json(new { success = message }, JsonRequestBehavior.AllowGet);

                        }
                    }
                    else if (!string.IsNullOrEmpty(payment.ConfirmationNumber))
                    {
                        #region InsertPaymentDetailsForOnePage API
                        string confirmation_number = payment.ConfirmationNumber;
                        Session["confirmation_number"] = confirmation_number;

                        string schedule_type = payment.PaymentStatus;
                        Sitecore.Diagnostics.Log.Info("Hill AFB payment Response" + confirmation_number + ",schedule_type " + schedule_type, "HillAFB Status");
                        string _details = _SavePaymentDetails(confirmation_number, schedule_type, Convert.ToInt32(idQuoteValue), Convert.ToInt32(idRiskaddressValue), form.Amount, form.PaymentPlan);

                        Sitecore.Diagnostics.Log.Info("Hill AFB payment details Response" + _details, "HillAFB Status");

                        return Json(new { success = _details }, JsonRequestBehavior.AllowGet);
                        #endregion
                    }
                }


            }
            #endregion

            return Json(new { success = response }, JsonRequestBehavior.AllowGet);



        }

        private Com.Alacriti.Checkout.Model.Payment MakeAPaymentTwo(string token, string digiSign, string customerAccountReference, string amount, string customerReference)
        {

            Com.Alacriti.Checkout.Model.Payment payment = new Payment();

            string isLocal = ConfigurationManager.AppSettings["isLocal"].ToString();
            if (string.IsNullOrEmpty(isLocal))
            {
                isLocal = "0";
            }
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                string IsAzureEnvironment = string.Empty;

                IsAzureEnvironment = ConfigurationManager.AppSettings["IsAzureEnvironment"];

                if (IsAzureEnvironment == "false")
                {
                    //Code for rackspace
                    var sr = new System.IO.StreamReader("\\Content\\orbipay_checkout_config.json");
                    Checkout.initProperties(sr);
                }
                else if (IsAzureEnvironment == "true")
                {
                    // new change changed sdk
                    string paymentConfigPath = context.Current.Server.MapPath("~/Content/PaymentContent/orbipay_checkout_config.json");  //Not Required
                    var sr = new System.IO.StreamReader(paymentConfigPath);//Not Required

                    AsymmetricKeyParameter privateKey = null;

                    try
                    {
                        string idpempath = string.Empty;
                        string PaymentPrvKeyPath = ConfigurationManager.AppSettings["PaymentPrvKeyPath"].ToString();
                        Sitecore.Diagnostics.Log.Info("Vishal PaymentPrvKeyPath :" + PaymentPrvKeyPath, this);


                        string idpempath2 = context.Current.Server.MapPath("~/Content/PaymentContent/4983998325.pem"); //azure
                        Sitecore.Diagnostics.Log.Info("Vishal idpempath2 :" + idpempath2, this);

                        //string idpempath = context.Current.Server.MapPath("\\Content\\4983998325.pem"); //rackspace

                        if (!string.IsNullOrEmpty(PaymentPrvKeyPath))
                        {
                            Sitecore.Diagnostics.Log.Info("Vishal Inside if  PaymentPrvKeyPath is  :" + PaymentPrvKeyPath, this);
                            idpempath = context.Current.Server.MapPath(PaymentPrvKeyPath);

                        }
                        Sitecore.Diagnostics.Log.Info("Vishal Private Key Path :" + idpempath, this);
                        using (var reader = System.IO.File.OpenText(idpempath))
                        {
                            string PaymentPvtKeyPwdStr = string.Empty;
                            string PaymentPvtKeyPwd = ConfigurationManager.AppSettings["PaymentPvtKeyPwd"].ToString();
                            Sitecore.Diagnostics.Log.Info("Vishal PaymentPvtKeyPwd Key Path :" + PaymentPvtKeyPwd, this);
                            if (!string.IsNullOrEmpty(PaymentPvtKeyPwd))
                            {
                                PaymentPvtKeyPwdStr = PaymentPvtKeyPwd;
                                Sitecore.Diagnostics.Log.Info("Vishal in side if condition PaymentPvtKeyPwdStr PaymentPvtKeyPwd :" + PaymentPvtKeyPwdStr + "-" + PaymentPvtKeyPwd, this);
                            }

                            //Org.BouncyCastle.OpenSsl.PemReader pemReader = new Org.BouncyCastle.OpenSsl.PemReader(reader, new PasswordFinder("AlacClient4983998325"));
                            Org.BouncyCastle.OpenSsl.PemReader pemReader = new Org.BouncyCastle.OpenSsl.PemReader(reader, new PasswordFinder(PaymentPvtKeyPwdStr));
                            object privateKeyObject = pemReader.ReadObject();
                            AsymmetricCipherKeyPair keyPair = (AsymmetricCipherKeyPair)privateKeyObject;
                            privateKey = keyPair.Private;
                        }
                    }
                    catch (Exception e)
                    {
                        Sitecore.Diagnostics.Log.Info(e.Message, this);
                        Sitecore.Diagnostics.Log.Info("Error thron : 400,unable to decrypt", this);
                        //throw new ApiException(400, "unable to decrypt");
                    }

                    AsymmetricKeyParameter publicKey = null;
                    try
                    {
                        string coserverpath = string.Empty;
                        string PaymentPubKeyPath = ConfigurationManager.AppSettings["PaymentPubKeyPath"].ToString();
                        //string coserverpath = context.Current.Server.MapPath("~/Content/PaymentContent/coserver_pub.pem"); //for azure 
                        //string coserverpath = context.Current.Server.MapPath("\\Content\\coserver_pub.pem");
                        Sitecore.Diagnostics.Log.Info("Vishal PaymentPubKeyPath" + "-" + PaymentPubKeyPath, this);

                        if (!string.IsNullOrEmpty(PaymentPubKeyPath))
                        {
                            coserverpath = context.Current.Server.MapPath(PaymentPubKeyPath);
                            Sitecore.Diagnostics.Log.Info("Vishal in side if condition coserverpath PaymentPubKeyPath" + coserverpath + "-" + PaymentPubKeyPath, this);
                        }
                        Sitecore.Diagnostics.Log.Info("Public Key Path :" + coserverpath, this);
                        using (var reader = System.IO.File.OpenText(coserverpath))
                        {
                            var pemReader = new Org.BouncyCastle.OpenSsl.PemReader(reader);
                            publicKey = (Org.BouncyCastle.Crypto.AsymmetricKeyParameter)pemReader.ReadObject();
                            Sitecore.Diagnostics.Log.Info("Vishal publicKey>>" + publicKey, this);
                        }
                    }
                    catch (Exception e)
                    {
                        Sitecore.Diagnostics.Log.Info(e.Message, this);
                        Sitecore.Diagnostics.Log.Info("Error thron : 400,unable to publicKey", this);
                        // throw new ApiException(400, "unable to read publicKey");
                    }

                    Dictionary<string, Org.BouncyCastle.Crypto.AsymmetricKeyParameter> conf_dict = new Dictionary<string, Org.BouncyCastle.Crypto.AsymmetricKeyParameter>();

                    conf_dict.Add("CHECKOUT_PUBLIC_KEY", publicKey);
                    conf_dict.Add("CLIENT_PRIVATE_KEY", privateKey);
                    Checkout.initProperties(conf_dict);

                    Sitecore.Diagnostics.Log.Info(" Checkout.initProperties>> Reach after here. ", this);
                    // new changed sdk end
                }


                string cstr_acnt_reference = customerAccountReference;
                string customer_reference = customerReference;


                string clientKey = ConfigurationManager.AppSettings["clientKey"];

                string[] SigArray = new string[2];
                //Refer values from config file
                SigArray[0] = ConfigurationManager.AppSettings["CPOSigKey1"];
                SigArray[1] = ConfigurationManager.AppSettings["CPOSigKey2"];


                string sigkey = SigArray[new Random().Next(0, SigArray.Length - 1)];
                string liveMode = ConfigurationManager.AppSettings["liveMode"];

                Sitecore.Diagnostics.Log.Info(" Vishal >> payment >>" + amount, this);
                Sitecore.Diagnostics.Log.Info(" Vishal >> token >>" + token, this);
                Sitecore.Diagnostics.Log.Info(" Vishal >> digiSign >>" + digiSign, this);

                payment = new Com.Alacriti.Checkout.Api.Payment(cstr_acnt_reference, amount)
                  .forClient(clientKey, sigkey, clientKey)
                  .withToken(token, digiSign)
                  .confirm(liveMode);

                Sitecore.Diagnostics.Log.Info(" Checkout.initProperties>> Reach after here. ", this);
                return payment;
            }
            catch (Exception ex)
            {

                Sitecore.Diagnostics.Log.Info("Last Catch eception in makePayment :" + ex.Message, this);
                return null;
            }



        }

        private Com.Alacriti.Checkout.Model.Payment MakeAPayment(string token, string digiSign, string customerAccountReference, string amount, string customerReference)
        {
            Com.Alacriti.Checkout.Model.Payment payment = new Com.Alacriti.Checkout.Model.Payment();

            string isLocal = ConfigurationManager.AppSettings["isLocal"].ToString();
            if (string.IsNullOrEmpty(isLocal))
            {
                isLocal = "0";
            }
            try
            {
                ServicePointManager.Expect100Continue = true;
                ServicePointManager.SecurityProtocol = SecurityProtocolType.Tls12;

                string IsAzureEnvironment = string.Empty;

                IsAzureEnvironment = ConfigurationManager.AppSettings["IsAzureEnvironment"];

                if (IsAzureEnvironment == "true")
                {
                    // new change changed sdk
                    var configPath = Server.MapPath("~/Content/PaymentContent/orbipay_checkout_config.json");
                    var sr = new System.IO.StreamReader(configPath);
                    Checkout.initProperties(sr);

                    AsymmetricKeyParameter privateKey = null;

                    try
                    {
                        string idpempath = string.Empty;
                        string PaymentPrvKeyPath = ConfigurationManager.AppSettings["PaymentPrvKeyPath"].ToString();

                        string idpempath2 = context.Current.Server.MapPath("~/Content/PaymentContent/4983998325.pem"); //azure


                        if (!string.IsNullOrEmpty(PaymentPrvKeyPath))
                        {

                            idpempath = context.Current.Server.MapPath(PaymentPrvKeyPath);

                        }

                        using (var reader = System.IO.File.OpenText(idpempath))
                        {
                            string PaymentPvtKeyPwdStr = string.Empty;
                            string PaymentPvtKeyPwd = ConfigurationManager.AppSettings["PaymentPvtKeyPwd"].ToString();

                            if (!string.IsNullOrEmpty(PaymentPvtKeyPwd))
                            {
                                PaymentPvtKeyPwdStr = PaymentPvtKeyPwd;
                            }

                            //Org.BouncyCastle.OpenSsl.PemReader pemReader = new Org.BouncyCastle.OpenSsl.PemReader(reader, new PasswordFinder("AlacClient4983998325"));
                            Org.BouncyCastle.OpenSsl.PemReader pemReader = new Org.BouncyCastle.OpenSsl.PemReader(reader,
                                new PasswordFinder(PaymentPvtKeyPwdStr));
                            object privateKeyObject = pemReader.ReadObject();
                            AsymmetricCipherKeyPair keyPair = (AsymmetricCipherKeyPair)privateKeyObject;
                            privateKey = keyPair.Private;
                        }
                    }
                    catch (Exception e)
                    {
                        Sitecore.Diagnostics.Log.Info("Hill AFB payment Exception: Error thron : 400,unable to decrypt" + e.Message, "HillAFB Status");
                        throw new ApiException(400, "unable to decrypt");
                    }

                    AsymmetricKeyParameter publicKey = null;
                    try
                    {
                        string coserverpath = string.Empty;
                        string PaymentPubKeyPath = ConfigurationManager.AppSettings["PaymentPubKeyPath"].ToString();
                        //string coserverpath = context.Current.Server.MapPath("~/Content/PaymentContent/coserver_pub.pem"); //for azure 
                        //string coserverpath = context.Current.Server.MapPath("\\Content\\coserver_pub.pem");


                        if (!string.IsNullOrEmpty(PaymentPubKeyPath))
                        {
                            coserverpath = context.Current.Server.MapPath(PaymentPubKeyPath);

                        }

                        Sitecore.Diagnostics.Log.Info("Hill AFB payment:Public Key Path " + coserverpath, "HillAFB Status");
                        using (var reader = System.IO.File.OpenText(coserverpath))
                        {
                            var pemReader = new Org.BouncyCastle.OpenSsl.PemReader(reader);
                            publicKey = (Org.BouncyCastle.Crypto.AsymmetricKeyParameter)pemReader.ReadObject();
                        }
                    }
                    catch (Exception e)
                    {
                        Sitecore.Diagnostics.Log.Info("Hill AFB payment Exception:400,unable to publicKey " + e.Message, "HillAFB Status");
                        throw new ApiException(400, "unable to read publicKey");
                    }

                    Dictionary<string, Org.BouncyCastle.Crypto.AsymmetricKeyParameter> conf_dict = new Dictionary<string, Org.BouncyCastle.Crypto.AsymmetricKeyParameter>();

                    conf_dict.Add("CHECKOUT_PUBLIC_KEY", publicKey);
                    conf_dict.Add("CLIENT_PRIVATE_KEY", privateKey);
                    Checkout.initProperties(conf_dict);



                    // new changed sdk end
                }



                string clientKey = ConfigurationManager.AppSettings["clientKey"];
                string[] sigKeys = new[] { ConfigurationManager.AppSettings["CPOSigKey1"], ConfigurationManager.AppSettings["CPOSigKey2"] };
                string sigKey = sigKeys[new Random().Next(sigKeys.Length - 1)];
                string liveMode = ConfigurationManager.AppSettings["liveMode"];

                payment = new Com.Alacriti.Checkout.Api.Payment(customerAccountReference, amount)
                    .forClient(clientKey, sigKey, clientKey)
                    .withToken(token, digiSign)
                    .confirm(liveMode);


                return payment;

            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Info("Hill AFB payment Exception" + ex.Message, "HillAFB Status");
            }

            return payment;
        }

        private string _SavePaymentDetails(string confirmation_number, string schedule_type, int idQuote, int addressId, string amount, string paymentPlan)
        {
            QHAPIPayment data = new QHAPIPayment
            {
                IdPayment = 0,
                Amount = amount,
                confirmation_number = confirmation_number,
                IdQuote = idQuote,
                IdRiskAddress = addressId,
                PaymentPlan = paymentPlan,
                payment_status = schedule_type
            };
            var _jsonBody = JsonConvert.SerializeObject(data);

            Sitecore.Diagnostics.Log.Info("Hill AFB Payment details after Orbipay" + _jsonBody, "HillAFB Status");

            Item item = Sitecore.Context.Database.GetItem("{0521D736-A5BF-4DDC-8746-C8B83110429F}");
            string _url = "", _username = "", _password = "";
            if (item != null)
            {
                _url = item.Fields["PaymentDetailsAPIURL"].Value;
                _username = item.Fields["UserName"].Value;
                _password = item.Fields["Password"].Value;
            }

            WebClient client = new WebClient();

            string encodedCredentials = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_username}:{_password}"));
            client.Headers.Add("Authorization", $"Basic {encodedCredentials}");

            // Set the content for the POST request
            string postData = _jsonBody;
            string response = "";

            // Set the content of the POST request and its data type
            client.Headers.Add("Content-Type", "application/json");

            Sitecore.Diagnostics.Log.Info("Hill AFB Payment" + _jsonBody, "HillAFB Status");
            // Make the POST request
            try
            {
                response = client.UploadString(_url, postData);
            }
            catch (WebException webError)
            {
                return response + webError.Message;
            }
            return response;
        }

        [HttpPost]
        public ActionResult PaymentDetailsSubmit(QHAPIPayment data)
        {
            var _jsonBody = JsonConvert.SerializeObject(data);

            Item item = Sitecore.Context.Database.GetItem("{0521D736-A5BF-4DDC-8746-C8B83110429F}");
            string _url = "", _username = "", _password = "";
            if (item != null)
            {
                _url = item.Fields["PaymentDetailsAPIURL"].Value;
                _username = item.Fields["UserName"].Value;
                _password = item.Fields["Password"].Value;
            }

            WebClient client = new WebClient();

            string encodedCredentials = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_username}:{_password}"));
            client.Headers.Add("Authorization", $"Basic {encodedCredentials}");

            // Set the content for the POST request
            string postData = _jsonBody;
            string response = "";

            // Set the content of the POST request and its data type
            client.Headers.Add("Content-Type", "application/json");

            Sitecore.Diagnostics.Log.Info("Hill AFB Payment" + _jsonBody, "HillAFB Status");
            // Make the POST request
            try
            {
                response = client.UploadString(_url, postData);
            }
            catch (WebException webError)
            {
                return Json(new { success = response + webError.Message }, JsonRequestBehavior.AllowGet);
            }
            return Json(new { success = response }, JsonRequestBehavior.AllowGet);

        }

        [HttpPost]
        public ActionResult ApplicationThankYou(QHAPIPaymentThanks data)
        {
            QHAPIPaymentUpdate update = new QHAPIPaymentUpdate();
            update.IdQuote = data.IdQuote;
            update.IdRiskAddress = data.IdRiskAddress;
            update.ScheduleDate = data.txtCallDate;
            string _time = (!string.IsNullOrEmpty(data.drpHH) ? data.drpHH : "") +
                          (!string.IsNullOrEmpty(data.drpMM) ? ":" + data.drpMM : "") +
                          (!string.IsNullOrEmpty(data.hdnAMPM) ? " " + data.hdnAMPM : "");
            update.ScheduleTime = _time;



            var _jsonBody = JsonConvert.SerializeObject(update);

            Item item = Sitecore.Context.Database.GetItem("{0521D736-A5BF-4DDC-8746-C8B83110429F}");
            string _url = "", _username = "", _password = "";
            if (item != null)
            {
                _url = item.Fields["ScheduleUpdateAPIURL"].Value;
                _username = item.Fields["UserName"].Value;
                _password = item.Fields["Password"].Value;
            }

            WebClient client = new WebClient();

            string encodedCredentials = Convert.ToBase64String(Encoding.UTF8.GetBytes($"{_username}:{_password}"));
            client.Headers.Add("Authorization", $"Basic {encodedCredentials}");

            // Set the content for the POST request
            string postData = _jsonBody;
            string response = "";

            // Set the content of the POST request and its data type
            client.Headers.Add("Content-Type", "application/json");

            Sitecore.Diagnostics.Log.Info("Hill AFB Schedule Update" + _jsonBody, "HillAFB Status");
            // Make the POST request
            try
            {
                response = client.UploadString(_url, postData);
            }
            catch (WebException webError)
            {
                return Json(new { success = response + webError.Message }, JsonRequestBehavior.AllowGet);
            }
            Session["confirmation_number"] = string.Empty;
            Session["IdRiskAddress"] = string.Empty;
            Session["IdQuote"] = string.Empty;
            return Json(new { success = response }, JsonRequestBehavior.AllowGet);
        }

        public FileResult DownloadProofofInsurance()
        {
            AFI.Feature.WebQuoteService.WebQuoteService.WebQuoteServiceClient q = new AFI.Feature.WebQuoteService.WebQuoteService.WebQuoteServiceClient();
            string IdRiskAddress = "0";

            if (Session["IdRiskAddress"] != null)
            {
                IdRiskAddress = Session["IdRiskAddress"]?.ToString();
            }
            CertificateRenter _CertificateRenter = new CertificateRenter();
            using (var client = new HttpClient())
            {

                Item item = Sitecore.Context.Database.GetItem("{0521D736-A5BF-4DDC-8746-C8B83110429F}");
                string _url = "", _username = "", _password = "";
                if (item != null)
                {
                    _url = item.Fields["GetCertificateDataOPAPIURL"].Value;
                    _username = item.Fields["UserName"].Value;
                    _password = item.Fields["Password"].Value;
                }

                client.BaseAddress = new Uri(_url);
                var responseTask = client.GetAsync("GetCertificateData?IdRiskAddress=" + IdRiskAddress);
                responseTask.Wait();
                //To store result of web api response.   
                var result = responseTask.Result;
                if (result.IsSuccessStatusCode)
                {
                    var readTask = result.Content.ReadAsStringAsync();
                    readTask.Wait();
                    var parsedObject = JObject.Parse(readTask.Result.ToString());
                    var popupJson = parsedObject["ResponseData"].ToString();
                    var CertificateRenter = JsonConvert.DeserializeObject<List<CertificateRenter>>(popupJson.ToString());
                    _CertificateRenter = CertificateRenter.FirstOrDefault();
                }
            }
            CertificateInfo ci = new CertificateInfo();
            ci.CompletionDate = _CertificateRenter.CompletionDate;
            ci.DateOfIssue = _CertificateRenter.DateOfIssue;
            ci.PolicyNumber = _CertificateRenter.PolicyNumber;
            ci.MemberNumber = _CertificateRenter.MemberNumber;
            ci.EffectiveDate = _CertificateRenter.EffectiveDate;
            ci.TermDate = _CertificateRenter.TermDate;
            ci.PrimaryName = _CertificateRenter.PrimaryName;
            ci.PrimaryStreet1 = _CertificateRenter.PrimaryStreet1;
            ci.PrimaryStreet2 = _CertificateRenter.PrimaryStreet2;
            ci.PrimaryCity = _CertificateRenter.PrimaryCity;
            ci.PrimaryStateAbrev = _CertificateRenter.PrimaryStateAbrev;
            ci.PrimaryZip = _CertificateRenter.PrimaryZip;
            ci.AdditionalInsuredName = _CertificateRenter.AdditionalInsuredName;
            ci.AdditionalInsuredStreet1 = _CertificateRenter.AdditionalInsuredStreet1;
            ci.AdditionalInsuredStreet2 = _CertificateRenter.AdditionalInsuredStreet2;
            ci.AdditionalInsuredCity = _CertificateRenter.AdditionalInsuredCity;
            ci.AdditionalInsuredStateAbrev = _CertificateRenter.AdditionalInsuredStateAbrev;
            ci.AdditionalInsuredZip = _CertificateRenter.AdditionalInsuredZip;
            ci.EachOccurrenceAmount = _CertificateRenter.EachOccurrenceAmount;
            ci.MedExpAmount = _CertificateRenter.MedExpAmount;
            ci.PersonalPropertyLimit = _CertificateRenter.PersonalPropertyLimit;//Proof OF Insurance
            CertificateReturn ret = q.CreateProofOfInsurance(ci);
            return File(ret.FileData, System.Net.Mime.MediaTypeNames.Application.Pdf, ret.FileName);
        }
        #endregion

        public ActionResult UCForm()
        {
            return View("/Areas/AFIWEB/Views/AFIReport/UCForm.cshtml");
        }

        [HttpPost]
        public ActionResult UCFormSubmit(CorviasModel cm)
        {
            var response = _UCFormRepository.InsertForm(cm);
            if (response > 0)
            {
                #region EMAIL                                    
                Sitecore.Data.Items.Item emailtemplate = Sitecore.Context.Database.GetItem(UCMail.Email_ItemId);
                try
                {
                    // EMAIL ADMIN

                    string admin_Email_Subject = emailtemplate[CorviasMail.Admin_Email_Subject];
                    string admin_Email_Body = emailtemplate[CorviasMail.Admin_Email_Body];
                    string admin_Email_Recipients = emailtemplate[CorviasMail.Admin_Email_Recipients];
                    string admin_Email_Sender = emailtemplate[CorviasMail.Admin_Email_Sender];

                    var email = ParseEmail(admin_Email_Sender, admin_Email_Subject, admin_Email_Body, admin_Email_Recipients, cm);
                    Sitecore.Diagnostics.Log.Info($"UC test Email Send", "Mehedi");
                    #region Test Email
                    try
                    {
                        //SmtpClient smtp = new SmtpClient();
                        //smtp.Host = "smtp.sendgrid.net";
                        //smtp.Credentials = new System.Net.NetworkCredential("apikey", "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");
                        //smtp.Port = 587;
                        //smtp.EnableSsl = false;
                        //smtp.Send(email.FromEmail, admin_Email_Recipients, email.Subject, email.Body);

                        using (MailMessage mail = new MailMessage())
                        {
                            mail.From = new MailAddress(admin_Email_Sender);
                            mail.To.Add(admin_Email_Recipients);
                            mail.Subject = email.Subject;
                            mail.Body = email.Body;
                            mail.IsBodyHtml = true;

                            using (SmtpClient smtp = new SmtpClient("smtp.gmail.com", 587))
                            {
                                smtp.Credentials = new System.Net.NetworkCredential("test.sgff@gmail.com", "testingg");
                                smtp.EnableSsl = true;
                                smtp.Send(mail);
                            }
                            Sitecore.Diagnostics.Log.Info($"UC test email Send done", "Mehedi");
                        }
                    }
                    catch (Exception ex)
                    {

                        Sitecore.Diagnostics.Log.Info($"UC test email Send error" + ex.Message, "Mehedi");
                    }


                    #endregion
                    Sitecore.Diagnostics.Log.Info($"UC Email Send", "Mehedi");
                    _emailService.Send(email);
                    Sitecore.Diagnostics.Log.Info($"UC Email Send", "Mehedi");
                    // EMAIL CLIENT
                    string email_Subject = emailtemplate[CorviasMail.Client_Confirmation_Email_Subject];
                    string email_Recipients = emailtemplate[cm.Email];
                    string email_Sender = emailtemplate[CorviasMail.Client_Confirmation_Email_Sender];
                    string email_Body = emailtemplate[CorviasMail.Client_Confirmation_Email_Body];
                    var email_client = ParseEmail(email_Sender, email_Subject, email_Body, email_Recipients, cm);
                    _emailService.Send(email_client);
                }
                catch (Exception ex) { }
                #endregion

            }

            var json = JsonConvert.SerializeObject(response);
            return Json(json, JsonRequestBehavior.AllowGet);
        }
        public ActionResult UCFormThanks()
        {
            return View("/Areas/AFIWEB/Views/AFIReport/UCFormThanks.cshtml");
        }

        private Email ParseEmail(string emailFromList, string emailSubject, string body, string emailRecipient, CorviasModel cm)
        {
            Regex re = new Regex(@"\{\{(\w+)\}\}", RegexOptions.Compiled);
            IDictionary<string, string> mappingKeys = new Dictionary<string, string>();
            mappingKeys.Add("firstname", cm.FirstName != "" ? cm.FirstName : "");
            mappingKeys.Add("lastname", cm.LastName != "" ? cm.LastName : "");
            mappingKeys.Add("address", cm.Address != "" ? cm.Address : "");
            mappingKeys.Add("city", cm.City != "" ? cm.City : "");
            mappingKeys.Add("state", cm.State != "" ? cm.State : "");
            mappingKeys.Add("zip", cm.ZipCode != "" ? cm.ZipCode : "");
            mappingKeys.Add("phone", cm.PhoneNumber != "" ? cm.PhoneNumber : "");
            mappingKeys.Add("email", cm.Email != "" ? cm.Email : "");
            mappingKeys.Add("ssn", cm.SSN != "" ? cm.SSN : "");

            mappingKeys.Add("coverage", cm.CoverageType != "" ? cm.CoverageType : "");
            mappingKeys.Add("coverageprice", cm.CoverageMonth != "" ? cm.CoverageMonth : "" + " " + cm.CoverageYear != "" ? cm.CoverageYear : "");
            mappingKeys.Add("branchofservice", cm.BranchOfService != "" ? cm.BranchOfService : "");
            mappingKeys.Add("militarystatus", cm.MilitaryStatus != "" ? cm.MilitaryStatus : "");
            mappingKeys.Add("militaryrank", cm.MilitaryRank != "" ? cm.MilitaryRank : "");
            mappingKeys.Add("questions", cm.AdditionalQuestions != "" ? cm.AdditionalQuestions : "");
            mappingKeys.Add("paymentmethod", cm.PaymentMethod != "" ? cm.PaymentMethod : "");
            mappingKeys.Add("routing", cm.Routing != "" ? cm.Routing : "");
            mappingKeys.Add("accountnumber", cm.AccountNumber != "" ? cm.AccountNumber : "");
            mappingKeys.Add("calldate ", cm.AgentCallDate != "" ? cm.AgentCallDate : "" + " " + cm.AgentCallTime != "" ? cm.AgentCallTime : "");

            string bodyParsed = re.ReplaceTokens(body, mappingKeys);
            string subjectParsed = re.ReplaceTokens(emailSubject, mappingKeys);
            return new Email
            {
                IsBodyHtml = true,
                To = new List<string> { emailRecipient },
                Subject = subjectParsed,
                Body = bodyParsed,
                FromEmail = emailFromList
            };
        }

        public ActionResult CommunicationBanner()
        {
            return View("/Areas/AFIWEB/Views/Components/Banners/CommunicationBanner.cshtml");
        }
        public ActionResult GetSurveyFormReport(string startDate = "", string endDate = "")
        {
            var data = _formRepository.GetSurveyReport(startDate, endDate);
            return View("/Areas/AFIWEB/Views/AFIReport/SurveyFormReport.cshtml", data);
        }
        [HttpGet]
        public JsonResult GetVoteCountReportForResult(string voatingPeriodId = "")
        {
            try
            {
                var coverageDetailsList = _AFIReportRepository.GetVoteCountReportForResult(voatingPeriodId);

                if (coverageDetailsList.Any())
                {
                    string finalJson = JsonConvert.SerializeObject(coverageDetailsList);
                    return Json(finalJson, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var response = new { Success = false, Message = $"No Data Found!" };
                    string finalJson = JsonConvert.SerializeObject(response);
                    return Json(finalJson, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                var response = new { Success = false, Message = $"Error Insert Item Message " + ex.InnerException };
                string finalJson = JsonConvert.SerializeObject(response);
                return Json(finalJson, JsonRequestBehavior.AllowGet);
            }
        }
        public FileContentResult DownloadVoteReportAsCSV(string voatingPeriodId = "")
        {

            try
            {

                var coverageDetailsList = _AFIReportRepository.GetVoteCountReportForResult(voatingPeriodId);

                if (coverageDetailsList.Any())
                {
                    string csvData = ConvertToCSV(coverageDetailsList);

                    byte[] bytes = Encoding.UTF8.GetBytes(csvData);
                    Response.Headers.Add("Content-Disposition", "attachment; filename=VoteReport.csv");
                    return File(bytes, "text/csv");

                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                byte[] errorBytes = Encoding.UTF8.GetBytes($"Error: {ex.Message}");
                Response.Headers.Add("Content-Disposition", "attachment; filename=Error_Report.csv");
                return File(errorBytes, "text/csv");
            }
        }

        [HttpGet]
        public JsonResult GetPrevVoteCountReportForResult()
        {

            try
            {
                var coverageDetailsList = _AFIReportRepository.GetDemoVoteCountReportForResult();

                if (coverageDetailsList.Any())
                {
                    string finalJson = JsonConvert.SerializeObject(coverageDetailsList);
                    return Json(finalJson, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var response = new { Success = false, Message = $"No Data Found!" };
                    string finalJson = JsonConvert.SerializeObject(response);
                    return Json(finalJson, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                var response = new { Success = false, Message = $"Error Insert Item Message " + ex.InnerException };
                string finalJson = JsonConvert.SerializeObject(response);
                return Json(finalJson, JsonRequestBehavior.AllowGet);
            }
        }
        public FileContentResult DownloadPrevVoteReportAsCSV()
        {

            try
            {
                var coverageDetailsList = _AFIReportRepository.GetDemoVoteCountReportForResult();

                if (coverageDetailsList.Any())
                {
                    string csvData = ConvertToCSV(coverageDetailsList);

                    byte[] bytes = Encoding.UTF8.GetBytes(csvData);
                    Response.Headers.Add("Content-Disposition", "attachment; filename=VoteReport.csv");
                    return File(bytes, "text/csv");

                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                byte[] errorBytes = Encoding.UTF8.GetBytes($"Error: {ex.Message}");
                Response.Headers.Add("Content-Disposition", "attachment; filename=Error_Report.csv");
                return File(errorBytes, "text/csv");
            }
        }

        private static string ConvertToCSV<T>(IEnumerable<T> dataList)
        {
            var sb = new StringBuilder();
            var properties = typeof(T).GetProperties()
                                          .Where(p => p.Name != "MemberId" && p.Name != "CandidateId" && p.Name != "VotingPeriodId" && p.Name != "TotalCount" && p.Name != "IsActive" && p.Name != "IsEmailUpdated"); // Exclude Id and UpdatedTime
            // Write headers
            sb.AppendLine(string.Join(",", properties.Select(p => p.Name)));

            // Write data
            foreach (var data in dataList)
            {
                var values = new List<string>();
                foreach (var property in properties)
                {
                    var value = property.GetValue(data);
                    if (value is DateTime)
                    {
                        // Format DateTime values
                        values.Add($"\"{((DateTime)value).ToString("yyyy-MM-dd HH:mm:ss")}\"");
                    }
                    else
                    {
                        values.Add($"\"{value}\"");
                    }
                }
                sb.AppendLine(string.Join(",", values));
            }

            return sb.ToString();
        }
        [HttpGet]
        public JsonResult GetAllVotingPeriod()
        {

            try
            {
                var votingPeriodList = _AFIReportRepository.GetAllVotingPeriod();

                if (votingPeriodList.Any())
                {
                    string finalJson = JsonConvert.SerializeObject(votingPeriodList);
                    return Json(finalJson, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var response = new { Success = false, Message = $"No Data Found!" };
                    string finalJson = JsonConvert.SerializeObject(response);
                    return Json(finalJson, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                var response = new { Success = false, Message = $"Error Item Message " + ex.InnerException };
                string finalJson = JsonConvert.SerializeObject(response);
                return Json(finalJson, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult CreateCandidteItem(string periodId)
        {
            try
            {
                var candidateList = _AFIReportRepository.GetVoteCandidateList(periodId);

                if (candidateList.Any())
                {
                    Database masterDb = Factory.GetDatabase("master");

                    // Specify the path where you want to create the item
                    string parentItemPath = ConfigurationManager.AppSettings["CandidateFolderPath"]; // Change this to your desired parent item path

                    // Get the parent item
                    Item parentItem = masterDb.GetItem(parentItemPath);
                    if (parentItem != null)
                    {
                        // Delete all previous items under the parent item
                        foreach (Item child in parentItem.Children.ToList())
                        {
                            child.Delete();
                        }

                        // Define the template for the new item
                        TemplateItem template = masterDb.GetTemplate("{B9CFD36B-763A-4F0B-A401-7E8281EE69CD}"); // Change this to your template's GUID

                        if (template != null)
                        {
                            // Start the Sitecore security disabler to avoid permission issues
                            using (new Sitecore.SecurityModel.SecurityDisabler())
                            {
                                foreach (var candidate in candidateList)
                                {
                                    // Remove leading and trailing spaces from the candidate name
                                    string itemName = RemoveSpecialCharacters(candidate.Name.Trim());

                                    // Add the item under the parent item
                                    Item newItem = parentItem.Add(itemName, template);

                                    if (newItem != null)
                                    {
                                        string _candidate = $@"
                                    <div class='candidate-item-name'>
                                        <p>{candidate.Name}</p>
                                        <p>
                                            <button type='button' class='btn btn-link p-0' data-id='{candidate.CandidateId}'  data-name='{candidate.Name}'>
                                                View Profile
                                            </button>
                                        </p>
                                    </div>
                                     ";
                                        string _name = string.Format("<a href='{0}' target='_blank'>{1}</a>", candidate.Content, candidate.Name);
                                        // Set the title field to the candidate name
                                        newItem.Editing.BeginEdit();
                                        newItem["Title"] = _candidate;
                                        newItem["Value"] = candidate.CandidateId.ToString();                                      
                                        newItem.Editing.EndEdit();
                                    }
                                }
                            }
                        }
                        else
                        {
                            // Handle if the template is not found
                        }
                    }
                    else
                    {
                        // Handle if the parent item is not found
                    }
                    return Json(new { Success = true }, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var response = new { Success = false, Message = $"No Data Found!" };
                    string finalJson = JsonConvert.SerializeObject(response);
                    return Json(finalJson, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                var response = new { Success = false, Message = $"Error Insert Item Message " + ex.InnerException };
                return Json(response);
            }
        }
       
        // Method to remove special characters from a string
        private string RemoveSpecialCharacters(string str)
        {
            return Regex.Replace(str, "[^a-zA-Z0-9]+", "", RegexOptions.Compiled);
        }

        [HttpPost]
        public JsonResult SubmitMemberVote()
        {
            try
            {
                var httpRequest = HttpContext.Request;
                var dropdownValue = httpRequest.Form["dropdownValue"];

                var file = httpRequest.Files["file"];
                if (file != null && file.ContentLength > 0)
                {
                    using (var reader = new StreamReader(file.InputStream))
                    {
                        // Skip header row if exists
                        reader.ReadLine();

                        while (!reader.EndOfStream)
                        {
                            var line = reader.ReadLine();
                            var values = line.Split(',');
                            Log.Error($" Import Member csv data >" + values, this);
                            var _created =DateTime.Now;
                            //if (!string.IsNullOrEmpty(values[19]))
                            //{
                            //    _created = Convert.ToDateTime(values[19]);
                            //}
                        
                            string randomNumber = GenerateRandomPIN();

                            // Map CSV columns to Member model properties
                            var member = new ProxyVoteMember
                            {
                                MemberNumber = values[0], // 1st column
                                EmailAddress = values[1], // 2nd column
                                Prefix = values[2], // 3rd column
                                Salutation= values[3],
                                InsuredFirstName= values[4],
                                InsuredLastName= values[5],
                                ClientType= values[6],
                                ServiceStatus= values[7],
                                MailingAddressLine1= values[8],
                                MailingAddressLine2 = values[9],
                                MailingCityName = values[10],
                                MailingCountyName = values[11],
                                MailingStateAbbreviation = values[12],
                                MailingZip = values[13],
                                MailingCountry = values[14],
                                MembershipDate = values[15],
                                YearsAsMember = values[16],
                                Gender = values[17],
                                Deceased = values[18],
                                CreateDate = _created,

                                FullName= CultureInfo.CurrentCulture.TextInfo.ToTitleCase(values[3].ToLower())  +' '+ CultureInfo.CurrentCulture.TextInfo.ToTitleCase(values[4].ToLower()),
                                VotingPeriodId = dropdownValue != null ? Convert.ToInt32(dropdownValue) : 0,
                                Enabled=true,
                              
                                IsEmailUpdated= false,
                                PIN= randomNumber,
                                // Set other properties to default values or null
                            };
                            var _insert = _AFIReportRepository.InsertMemberVote(member);


                        }
                    }

                    return Json(new { success = true, message = "Members imported successfully." });
                }
                else
                {
                    return Json(new { success = false, message = "No file uploaded." });
                }
            }
            catch (Exception ex)
            {
                return Json(new { success = false, message = "Error occurred while importing members.", error = ex.Message });
            }

        }


        private string GenerateRandomPIN()
        {
            Random rand = new Random();
            int randomNumber = rand.Next(100000, 999999);
            return randomNumber.ToString();
        }


        [HttpGet]
        public JsonResult GetVotingPeriodById(string id)
        {

            try
            {
                int votingPeriodId = int.Parse(id);

                var votingPeriod = _AFIReportRepository.GetVotingPeriodById(votingPeriodId);

                if (votingPeriod != null)
                {
                    string finalJson = JsonConvert.SerializeObject(votingPeriod);
                    return Json(finalJson, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var response = new { Success = false, Message = $"Voting Period with ID {id} not found!" };
                    string finalJson = JsonConvert.SerializeObject(response);
                    return Json(finalJson, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                var response = new { Success = false, Message = $"Error retrieving Voting Period with ID {id}. Exception: {ex.Message}" };
                string finalJson = JsonConvert.SerializeObject(response);
                return Json(finalJson, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult AddVotingPeriodData(VotingPeriod votingPeriod)
        {
            try
            {

                int count = _AFIReportRepository.CreateVotingPeriodData(votingPeriod);

                var votingPeriodList = _AFIReportRepository.GetAllVotingPeriod();

                if (count > 0 && votingPeriodList.Any())
                {
                    string finalJson = JsonConvert.SerializeObject(votingPeriodList);
                    return Json(finalJson, JsonRequestBehavior.AllowGet);
                }
                else if (count == -1)
                {
                    var response = new { Success = false, Message = $"Data Already Exist" };
                    string finalJson = JsonConvert.SerializeObject(response);
                    return Json(finalJson, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var response = new { Success = false, Message = $"Data Insert Unsuccessful" };
                    string finalJson = JsonConvert.SerializeObject(response);
                    return Json(finalJson, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {

                var response = new { Success = false, Message = $"Error Insert Item Message " + ex.InnerException };
                string finalJson = JsonConvert.SerializeObject(response);
                return Json(finalJson, JsonRequestBehavior.AllowGet);
            }

            // return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult UpdateVotingPeriodData(VotingPeriod votingPeriod)
        {
            try
            {

                int count = _AFIReportRepository.UpdateVotingPeriodData(votingPeriod);

                var votingPeriodList = _AFIReportRepository.GetAllVotingPeriod();

                if (count > 0 && votingPeriodList.Any())
                {
                    string finalJson = JsonConvert.SerializeObject(votingPeriodList);
                    return Json(finalJson, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var response = new { Success = false, Message = $"Data Update Unsuccessful" };
                    string finalJson = JsonConvert.SerializeObject(response);
                    return Json(finalJson, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {

                var response = new { Success = false, Message = $"Error Update. Item Message " + ex.InnerException };
                string finalJson = JsonConvert.SerializeObject(response);
                return Json(finalJson, JsonRequestBehavior.AllowGet);
            }

            // return Json(data, JsonRequestBehavior.AllowGet);
        }

        public JsonResult DeleteVotingPeriodData(string id)
        {
            try
            {
                int votingPeriodId = int.Parse(id);
                int count = _AFIReportRepository.DeleteVotingPeriodData(votingPeriodId);

                var votingPeriodList = _AFIReportRepository.GetAllVotingPeriod();

                if (count > 0 && votingPeriodList.Any())
                {
                    string finalJson = JsonConvert.SerializeObject(votingPeriodList);
                    return Json(finalJson, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var response = new { Success = false, Message = $"Data Delete Unsuccessful" };
                    string finalJson = JsonConvert.SerializeObject(response);
                    return Json(finalJson, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {

                var response = new { Success = false, Message = $"Error Delete. Item Message " + ex.InnerException };
                string finalJson = JsonConvert.SerializeObject(response);
                return Json(finalJson, JsonRequestBehavior.AllowGet);
            }

            // return Json(data, JsonRequestBehavior.AllowGet);
        }

        [HttpGet]
        public JsonResult GetAllCandidateData()
        {

            try
            {
                var candidateDataList = _AFIReportRepository.GetAllCandidateData();

                if (candidateDataList.Any())
                {
                    string finalJson = JsonConvert.SerializeObject(candidateDataList);
                    return Json(finalJson, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var response = new { Success = false, Message = $"No Data Found!" };
                    string finalJson = JsonConvert.SerializeObject(response);
                    return Json(finalJson, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                var response = new { Success = false, Message = $"Error retrieving Candidate Data. Exception: {ex.Message}" };
                string finalJson = JsonConvert.SerializeObject(response);
                return Json(finalJson, JsonRequestBehavior.AllowGet);
            }
        }
        [HttpGet]
        public JsonResult GetAllLatestVotingPeriodCandidateData()
        {

            try
            {
                var candidateDataList = _AFIReportRepository.GetAllLatestVotingPeriodCandidateData();

                if (candidateDataList.Any())
                {
                    string finalJson = JsonConvert.SerializeObject(candidateDataList);
                    return Json(finalJson, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var response = new { Success = false, Message = $"No Data Found!" };
                    string finalJson = JsonConvert.SerializeObject(response);
                    return Json(finalJson, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                var response = new { Success = false, Message = $"Error retrieving Candidate Data. Exception: {ex.Message}" };
                string finalJson = JsonConvert.SerializeObject(response);
                return Json(finalJson, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult GetCandidateById(string id)
        {
            try
            {
                int candidateId = int.Parse(id);

                var candidate = _AFIReportRepository.GetCandidateById(candidateId);

                if (candidate != null)
                {
                    string finalJson = JsonConvert.SerializeObject(candidate);
                    return Json(finalJson, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var response = new { Success = false, Message = $"Candidate with ID {id} not found!" };
                    string finalJson = JsonConvert.SerializeObject(response);
                    return Json(finalJson, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                var response = new { Success = false, Message = $"Error retrieving Candidate with ID {id}. Exception: {ex.Message}" };
                string finalJson = JsonConvert.SerializeObject(response);
                return Json(finalJson, JsonRequestBehavior.AllowGet);
            }
        }



        [HttpPost]
        public JsonResult AddCandidateData(VoteCandidate voteCandidate)
        {
            try
            {
                string imagePath = "";

                if (voteCandidate.Txt == null || voteCandidate.Txt.ContentLength == 0)
                {
                    voteCandidate.Content = null;
                }
                else
                {
                    using (var reader = new StreamReader(voteCandidate.Txt.InputStream))
                    {
                        voteCandidate.Content = reader.ReadToEnd();
                    }
                }

                if (voteCandidate.Image == null || voteCandidate.Image.ContentLength == 0)
                {
                    // If votingPeriod.Image is null or has no content, check if the default image exists
                    string defaultImagePath = Sitecore.IO.FileUtil.MapPath("/upload/Images/default_image.png");
                    imagePath = "upload/Images/default_image.png";
                    voteCandidate.ImagePath = imagePath;
                }
                else
                {
                    string fileName = Path.GetFileName(voteCandidate.Image.FileName);
                    string fileExtension = Path.GetExtension(fileName).ToLower();
                    // Define a list of allowed file extensions
                    List<string> allowedExtensions = new List<string> { ".jpg", ".jpeg", ".png" };

                    if (!allowedExtensions.Contains(fileExtension))
                    {
                        var response = new { Success = false, Message = "Invalid file type. Only .jpg, .jpeg, and .png files are allowed." };
                        string finalJson = JsonConvert.SerializeObject(response);
                        return Json(finalJson, JsonRequestBehavior.AllowGet);
                    }

                    string imageFolderPath = Sitecore.IO.FileUtil.MapPath("/upload/Images");

                    if (!Directory.Exists(imageFolderPath))
                    {
                        Directory.CreateDirectory(imageFolderPath);
                    }

                    imagePath = System.IO.Path.Combine(imageFolderPath, fileName);
                    voteCandidate.Image.SaveAs(imagePath);
                    string relativeImagePath = imagePath.Replace(Server.MapPath("~"), "");
                    voteCandidate.ImagePath = relativeImagePath;
                }

                int id = _AFIReportRepository.CreateCandidateData(voteCandidate);

                var candidateDataList = _AFIReportRepository.GetAllCandidateData();

                if (id > 0 && candidateDataList.Any())
                {
                    string finalJson = JsonConvert.SerializeObject(candidateDataList);
                    return Json(finalJson, JsonRequestBehavior.AllowGet);
                }
                else if (id == -1)
                {
                    var response = new { Success = false, Message = $"Data Already Exists" };
                    string finalJson = JsonConvert.SerializeObject(response);
                    return Json(finalJson, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var response = new { Success = false, Message = $"Data Insertion Unsuccessful" };
                    string finalJson = JsonConvert.SerializeObject(response);
                    return Json(finalJson, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                var response = new { Success = false, Message = $"Error Inserting Item: {ex.Message}" };
                string finalJson = JsonConvert.SerializeObject(response);
                return Json(finalJson, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult UpdateCandidateData(VoteCandidate voteCandidate)
        {
            try
            {
                if (voteCandidate.Txt == null || voteCandidate.Txt.ContentLength == 0)
                {
                    voteCandidate.Content = null;
                }
                else
                {
                    using (var reader = new StreamReader(voteCandidate.Txt.InputStream))
                    {
                        voteCandidate.Content = reader.ReadToEnd();
                    }
                }
                if (voteCandidate.Image == null || voteCandidate.Image.ContentLength == 0)
                {
                    voteCandidate.ImagePath = string.Empty;
                }
                else
                {

                    string fileName = Path.GetFileName(voteCandidate.Image.FileName);
                    string fileExtension = Path.GetExtension(fileName).ToLower();
                    // Define a list of allowed file extensions
                    List<string> allowedExtensions = new List<string> { ".jpg", ".jpeg", ".png" };

                    if (!allowedExtensions.Contains(fileExtension))
                    {
                        var response = new { Success = false, Message = "Invalid file type. Only .jpg, .jpeg, and .png files are allowed." };
                        string finalJson = JsonConvert.SerializeObject(response);
                        return Json(finalJson, JsonRequestBehavior.AllowGet);
                    }

                    string imageFolderPath = Sitecore.IO.FileUtil.MapPath("/upload/Images");

                    if (!Directory.Exists(imageFolderPath))
                    {
                        Directory.CreateDirectory(imageFolderPath);
                    }

                    string imagePath = Path.Combine(imageFolderPath, fileName);
                    voteCandidate.Image.SaveAs(imagePath);
                    string relativeImagePath = imagePath.Replace(Server.MapPath("~"), "");
                    voteCandidate.ImagePath = relativeImagePath;

                }
                int id = _AFIReportRepository.UpdateCandidateData(voteCandidate);
                var candidateDataList = _AFIReportRepository.GetAllCandidateData();

                if (id > 0 && candidateDataList.Any())
                {
                    string finalJson = JsonConvert.SerializeObject(candidateDataList);
                    return Json(finalJson, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var response = new { Success = false, Message = $"Data Update Unsuccessful" };
                    string finalJson = JsonConvert.SerializeObject(response);
                    return Json(finalJson, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {

                var response = new { Success = false, Message = $"Error Update. Item Message " + ex.InnerException };
                string finalJson = JsonConvert.SerializeObject(response);
                return Json(finalJson, JsonRequestBehavior.AllowGet);
            }

        }

        public JsonResult DeleteCandidateData(string id)
        {
            try
            {
                int CandidateId = int.Parse(id);


                int count = _AFIReportRepository.DeleteCandidateData(CandidateId);
                var candidateDataList = _AFIReportRepository.GetAllCandidateData();

                if (count > 0 && candidateDataList.Any())
                {
                    string finalJson = JsonConvert.SerializeObject(candidateDataList);
                    return Json(finalJson, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var response = new { Success = false, Message = $"Data Delete Unsuccessful" };
                    string finalJson = JsonConvert.SerializeObject(response);
                    return Json(finalJson, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {

                var response = new { Success = false, Message = $"Error Delete. Item Message " + ex.InnerException };
                string finalJson = JsonConvert.SerializeObject(response);
                return Json(finalJson, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpGet]
        public JsonResult GetAllVotingMemberData(int page = 1, int pageSize = 50, int VotingId = 0, string IsEmail = "")
        {

            try
            {
                //int totalCount;
                //int totalPages;
                //IEnumerable<VoteMember> data;
                //if (VotingId == 0)
                //{
                //    var latestData = _AFIReportRepository.GetAllVotingMemberOnLatestVotingPeriod(page, pageSize);
                //    if(latestData.Count() == 0)
                //    {
                //        data = _AFIReportRepository.GetAllVotingMemberData(page, pageSize, VotingId, IsEmail);
                //    }
                //    else
                //    {
                //        data = latestData; 
                //    }
                //}
                //else
                //{
                //    data = _AFIReportRepository.GetAllVotingMemberData(page, pageSize, VotingId, IsEmail);
                //}

                var data = _AFIReportRepository.GetAllVotingMemberData(page, pageSize, VotingId, IsEmail);

                var totalCount = data.FirstOrDefault()?.TotalCount ?? 0;
                var totalPages = (int)Math.Ceiling((decimal)totalCount / pageSize);

                MemberPagination finalData = new MemberPagination
                {
                    CurrentPage = page,
                    TotalItem = totalCount,
                    TotalPages = totalPages,
                    MemberList = data
                };
                if (finalData != null && data.Count() > 0)
                {
                    string finalJson = JsonConvert.SerializeObject(finalData);
                    return Json(finalJson, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var response = new { Success = false, Message = $"No Data Found!" };
                    string finalJson = JsonConvert.SerializeObject(response);
                    return Json(finalJson, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                var response = new { Success = false, Message = $"Error retrieving Candidate Data. Exception: {ex.Message}" };
                string finalJson = JsonConvert.SerializeObject(response);
                return Json(finalJson, JsonRequestBehavior.AllowGet);
            }
        }

        [HttpGet]
        public JsonResult GetMemberVoteCount(int memberId, int votingPeriodId)
        {

            try
            {
                var totalCount = _AFIReportRepository.GetMemberVoteCountByMemberIdAndVotePeriodId(memberId, votingPeriodId);


                var response = new { Success = true, Total = totalCount };
                string finalJson = JsonConvert.SerializeObject(response);
                return Json(finalJson, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                var response = new { Success = false, Message = $"Error retrieving Candidate Data. Exception: {ex.Message}" };
                string finalJson = JsonConvert.SerializeObject(response);
                return Json(finalJson, JsonRequestBehavior.AllowGet);
            }
        }
        

        public ActionResult GetVoteCandidates()
        {
            var parentItem = Sitecore.Context.Database.GetItem(new ID("{499F27E7-C557-44B1-A86F-FF32E269E3C9}")); // Parent ID
            var model = new AFI.Feature.QuoteForm.ExperienceForms.CustomRadioButtonListViewModel
            {
                PreloadedOptions = GetDataSourceOptions(parentItem)
            };

            return View("~/Views/FormBuilder/FieldTemplates/VoteCandidateRadioButtonList.cshtml", model);
        }

        [HttpPost]
        public ActionResult InsertMembereData(VoteMember voteMember, int page = 1, int pageSize = 50, int VotingId = 0, string IsEmail="")
        {
            try
            {
                var member = new ProxyVoteMember
                {
                    MemberNumber = voteMember.MemberNumber,
                    PIN = voteMember.PIN,
                    FullName = voteMember.FullName,
                    VotingPeriodId = Convert.ToInt32(voteMember.VotingPeriodId),
                    Enabled=true,
                    EmailAddress = voteMember.EmailAddress,

                };
                var _insert = _AFIReportRepository.InsertMemberVote(member);


                if (_insert > 0)
                {
                    return RedirectToAction("GetAllVotingMemberData", new { page, pageSize, VotingId, IsEmail });
                }
                else if (_insert == -1)
                {
                    var response = new { Success = false, Message = $"Data Already Exist" };
                    string finalJson = JsonConvert.SerializeObject(response);
                    return Json(finalJson, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var response = new { Success = false, Message = $"Data Insert Unsuccessful" };
                    string finalJson = JsonConvert.SerializeObject(response);
                    return Json(finalJson, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {

                var response = new { Success = false, Message = $"Error Insert Item Message " + ex.InnerException };
                string finalJson = JsonConvert.SerializeObject(response);
                return Json(finalJson, JsonRequestBehavior.AllowGet);
            }

        }

        [HttpPost]
        public ActionResult UpdateMemberData(VoteMember voteMember, int page = 1, int pageSize = 50, int VotingId = 0, string IsEmail="")
        {
            try
            {

                int count = _AFIReportRepository.UpdateMemberData(voteMember);


                if (count > 0)
                {
                    return RedirectToAction("GetAllVotingMemberData", new { page, pageSize, VotingId, IsEmail });
                }
                else
                {
                    var response = new { Success = false, Message = $"Data Update Unsuccessful" };
                    string finalJson = JsonConvert.SerializeObject(response);
                    return Json(finalJson, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {

                var response = new { Success = false, Message = $"Error Update. Item Message " + ex.InnerException };
                string finalJson = JsonConvert.SerializeObject(response);
                return Json(finalJson, JsonRequestBehavior.AllowGet);
            }

        }

        public ActionResult DeleteMemberData(string id, int page = 1, int pageSize = 50, int VotingId = 0, string IsEmail = "")
        {
            try
            {
                int MemberId = int.Parse(id);


                int count = _AFIReportRepository.DeleteMemberData(MemberId);


                if (count > 0)
                {
                    return RedirectToAction("GetAllVotingMemberData", new { page, pageSize, VotingId, IsEmail });
                }
                else
                {
                    var response = new { Success = false, Message = $"Data Delete Unsuccessful" };
                    string finalJson = JsonConvert.SerializeObject(response);
                    return Json(finalJson, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {

                var response = new { Success = false, Message = $"Error Delete. Item Message " + ex.InnerException };
                string finalJson = JsonConvert.SerializeObject(response);
                return Json(finalJson, JsonRequestBehavior.AllowGet);
            }

        }
        public JsonResult GetCandidateVoteBallotStatus(string votingPeriodId = "", string memberId = "")
        {
            try
            {
                bool voteStatus = _AFIReportRepository.GetCandidateVoteBallotStatus(votingPeriodId, memberId);

                if (voteStatus)
                {
                    var response = new { VoteStatus = true, Message = $"Vote already submitted !" };
                    string finalJson = JsonConvert.SerializeObject(response);
                    return Json(finalJson, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var response = new { VoteStatus = false, Message = $"Please Submit the Vote" };
                    string finalJson = JsonConvert.SerializeObject(response);
                    return Json(finalJson, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                var response = new { Success = false, Message = $"Error Insert Item Message " + ex.InnerException };
                string finalJson = JsonConvert.SerializeObject(response);
                return Json(finalJson, JsonRequestBehavior.AllowGet);
            }
        }


        [HttpGet]
        public JsonResult GetTotalVotingDetails(string voatingPeriodId = "")
        {
            try
            {
                var detailsList = _AFIReportRepository.GetTotalVoteCountDetailsForResult(voatingPeriodId);

                if (detailsList != null)
                {
                    string finalJson = JsonConvert.SerializeObject(detailsList);
                    return Json(finalJson, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var response = new { Success = false, Message = $"No Data Found!" };
                    string finalJson = JsonConvert.SerializeObject(response);
                    return Json(finalJson, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                var response = new { Success = false, Message = $"Error Insert Item Message " + ex.InnerException };
                string finalJson = JsonConvert.SerializeObject(response);
                return Json(finalJson, JsonRequestBehavior.AllowGet);
            }
        }
        public IEnumerable<OptionItem> GetDataSourceOptions(Item parentItem)
        {
            var options = new List<OptionItem>();

            if (parentItem != null)
            {
                foreach (Item childItem in parentItem.Children)
                {
                    options.Add(new OptionItem
                    {
                        Text = childItem["CandidateName"], // Assuming CandidateName is the field for radio button label
                        Value = "SomeValue" // Set value as needed
                    });
                }
            }

            return options;
        }

        public JsonResult GetMemberEmailByMemberNumberAndPIN(string memberNumber = "", string pin = "")
        {
            try
            {
                string email = _AFIReportRepository.GetMemberEmailByMemberNumberAndPIN(memberNumber, pin);

                if (string.IsNullOrEmpty(email))
                {
                    var response = new { EmailStatus = false, Message = $"Enter the Email" };
                    string finalJson = JsonConvert.SerializeObject(response);
                    return Json(finalJson, JsonRequestBehavior.AllowGet);
                }
                else
                {
                    var response = new { EmailStatus = true, EmailAddress = email, Message = $"Email Exist" };
                    string finalJson = JsonConvert.SerializeObject(response);
                    return Json(finalJson, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                var response = new { Success = false, Message = $"Error Insert Item Message " + ex.InnerException };
                string finalJson = JsonConvert.SerializeObject(response);
                return Json(finalJson, JsonRequestBehavior.AllowGet);
            }
        }

        public FileContentResult DownloadMemberFilterDataCSV(int VotingId = 0, string IsEmail = "")
        {

            try
            {

                var data = _AFIReportRepository.GetAllVotingMemberData(0, 0, VotingId, IsEmail);

                if (data.Any())
                {
                    string csvData = ConvertToCSV(data);

                    byte[] bytes = Encoding.UTF8.GetBytes(csvData);
                    Response.Headers.Add("Content-Disposition", "attachment; filename=VoteMember.csv");
                    return File(bytes, "text/csv");

                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                byte[] errorBytes = Encoding.UTF8.GetBytes($"Error: {ex.Message}");
                Response.Headers.Add("Content-Disposition", "attachment; filename=Error_Report.csv");
                return File(errorBytes, "text/csv");
            }
        }
        public JsonResult GetMemberInfoByMemberNumber(string memberNumber = "")
        {
            try
            {
                var MemberInfo = _AFIReportRepository.GetMemberInfoByMemberNumber(memberNumber);

                if (MemberInfo == null)
                {
                    var response = new { Success = false, Message = $"No Data Found" };
                    string finalJson = JsonConvert.SerializeObject(response);
                    return Json(finalJson, JsonRequestBehavior.AllowGet);
                }

                else
                {

                    string finalJson = JsonConvert.SerializeObject(MemberInfo);
                    return Json(finalJson, JsonRequestBehavior.AllowGet);
                }
            }
            catch (Exception ex)
            {
                var response = new { Success = false, Message = $"Error Insert Item Message " + ex.InnerException };
                string finalJson = JsonConvert.SerializeObject(response);
                return Json(finalJson, JsonRequestBehavior.AllowGet);
            }
        }
    }
}