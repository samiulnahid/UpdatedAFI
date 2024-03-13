using System.Collections.Generic;
using System.Web.Mvc;
using AFI.Feature.Identifiers;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Repository;
using Sitecore.Data.Fields;

namespace AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Controllers
{
    public class FormsValidationController : Controller
    {
        private readonly ISessionRepository _sessionRepository;
        private const string ZipCodeSessionKey = "afi:zipcode";

        public FormsValidationController(ISessionRepository sessionRepository)
        {
            this._sessionRepository = sessionRepository;
        }

        private void StoreZipCodeInSession(string zipCode)
        {
            if (string.IsNullOrWhiteSpace(zipCode)) return;
            _sessionRepository.RemoveValues(ZipCodeSessionKey);
            _sessionRepository.SaveValues(ZipCodeSessionKey, new[] { new KeyValuePair<string, string>("ZipCode", zipCode) });
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Auto(string zipCode)
        {
            StoreZipCodeInSession(zipCode);
            return View("Forms/AutoForm");
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Business(string zipCode)
        {
            StoreZipCodeInSession(zipCode);
            return View("Forms/BusinessForm");
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult HillAFB(string zipCode)
        {
            StoreZipCodeInSession(zipCode);
            return View("Forms/HillAFBForm");
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult LeadGeneration(string zipCode)
        {
            StoreZipCodeInSession(zipCode);
            return View("Forms/LeadGenerationForm");
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult CollectorVehicle(string zipCode)
        {
            StoreZipCodeInSession(zipCode);
            return View("Forms/CollectorVehicleForm");
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Flood(string zipCode)
        {
            StoreZipCodeInSession(zipCode);
            return View("Forms/FloodForm");
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Homeowner(string zipCode)
        {
            StoreZipCodeInSession(zipCode);
            return View("Forms/HomeownerForm");
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Homenonowner(string zipCode)
        {
            StoreZipCodeInSession(zipCode);
            return View("Forms/HomenonownerForm");
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Condo(string zipCode)
        {
            StoreZipCodeInSession(zipCode);
            return View("Forms/CondoForm");
        }
        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Mobilehome(string zipCode)
        {
            StoreZipCodeInSession(zipCode);
            return View("Forms/MobilehomeForm");
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Motorcycle(string zipCode)
        {
            StoreZipCodeInSession(zipCode);
            return View("Forms/MotorcycleForm");
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Motorhome(string zipCode)
        {
            StoreZipCodeInSession(zipCode);
            return View("Forms/MotorhomeForm");
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Umbrella(string zipCode)
        {
            StoreZipCodeInSession(zipCode);
            return View("Forms/UmbrellaForm");
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Watercraft(string zipCode)
        {
            StoreZipCodeInSession(zipCode);
            return View("Forms/WatercraftForm");
        }

        [AcceptVerbs(HttpVerbs.Get)]
        public ActionResult Renter(string zipCode)
        {
            StoreZipCodeInSession(zipCode);
            return View("Forms/RenterForm");
        }
        
    }
}