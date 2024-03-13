using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Extensions;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Handlers;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Helpers;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.AutoForm;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.AutoForm.VinLookup;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.BusinessForm;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.CollectorForm;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.Common;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.FloodForm;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.HillAFBForm;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.LeadGenerationForm;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.HomenonownerForm;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.HomeownerForm;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.CondoForm;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.MobilehomeForm;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.MotorcycleForm;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.Motorhome;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.RenterForm;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.UmbrellaForm;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.WatercraftForm;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Repository;
using AFI.Feature.QuoteForm.Areas.AFIWEB.Models;
using AFI.Feature.QuoteForm.Areas.AFIWEB.Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.Web.Http;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.QuoteLeadForm;

namespace AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Controllers
{
    [RoutePrefix(APIRoutes.API_ROUTE_PREFIX)]
    public class FormAPIController : ApiController
    {
        private readonly IAutoQuoteFormRepository _autoRepository;
        private readonly IWatercraftQuoteFormRepository _watercraftRepository;
        private readonly IMotorcycleQuoteFormRepository _motorcycleRepository;
        private readonly IBusinessQuoteFormRepository _businessRepository;
        private readonly IHillAFBQuoteFormRepository _hillRepository;
        private readonly ILeadGenerationFormRepository _leadGenerationRepository;
        private readonly IUmbrellaQuoteFormRepository _umbrellaRepository;
        private readonly IFloodQuoteFormRepository _floodRepository;
        private readonly ICollectorVehicleQuoteFormRepository _collectorVehicleRepository;
        private readonly IHomeownerQuoteFormRepository _homehownerRepository;
        private readonly IHomenonownerQuoteFormRepository _homenonownerRepository;
        private readonly IMobilehomeQuoteFormRepository _mobilehomeRepository;
        private readonly IMotorhomeQuoteFormRepository _motorhomeRepository;
        private readonly IRenterQuoteFormRepository _renterRepository;
        private readonly IProductFormHandler _productFormHandler;
        private readonly IAutoFormHandler _autoFormHandler;
        private readonly ICondoQuoteFormRepository _condoRepository;
        private readonly IQuoteLeadFormRepository _quoteLeadRepository;

        private readonly IFormSaveHandler<MotorhomeForm> _motorhomeFormSaveHandler;
        private readonly IFormSaveHandler<HomeownerForm> _homeownerFormSaveHandler;
        private readonly IFormSaveHandler<HomenonownerForm> _homenonownerFormSaveHandler;
        private readonly IFormSaveHandler<MobilehomeForm> _mobilehomeFormSaveHandler;
        private readonly IFormSaveHandler<RenterForm> _renterFormSaveHandler;
        private readonly IFormSaveHandler<AutoForm> _autoFormSaveHandler;
        private readonly IFormSaveHandler<WatercraftForm> _watercraftFormSaveHandler;
        private readonly IFormSaveHandler<MotorcycleForm> _motorcycleFormSaveHandler;
        private readonly IFormSaveHandler<FloodForm> _floodFormSaveHandler;
        private readonly IFormSaveHandler<BusinessForm> _businessFormSaveHandler;
        private readonly IFormSaveHandler<HillAFBForm> _hillAFBFormSaveHandler;
        private readonly IFormSaveHandler<LeadGenerationForm> _leadGenerationFormSaveHandler;
        private readonly IFormSaveHandler<UmbrellaForm> _umbrellaFormSaveHandler;
        private readonly IFormSaveHandler<CollectorVehicleForm> _collectorVehicleFormSaveHandler;
        private readonly IFormSaveHandler<CondoForm> _condoFormSaveHandler;
        private readonly IFormSaveHandler<QuoteLeadForm> _quoteLeadFormSaveHandler;
        private readonly IReCaptchaService _reCaptchaService;

        public FormAPIController(IAutoQuoteFormRepository autoRepository,
                                 IWatercraftQuoteFormRepository watercraftRepository,
                                 IMotorcycleQuoteFormRepository motorcycleRepository,
                                 IBusinessQuoteFormRepository businessRepository,
                                 IHillAFBQuoteFormRepository hillRepository,
                                  ILeadGenerationFormRepository leadGenerationRepository,
                                 IUmbrellaQuoteFormRepository umbrellaRepository,
                                 IFloodQuoteFormRepository floodRepository,
                                 ICollectorVehicleQuoteFormRepository collectorVehicleRepository,
                                 IHomeownerQuoteFormRepository homehownerRepository,
                                 IHomenonownerQuoteFormRepository homenonownerRepository,
                                 IMobilehomeQuoteFormRepository mobilehomeRepository,
                                 IMotorhomeQuoteFormRepository motorhomeRepository,
                                 IProductFormHandler productFormHandler,
                                 IAutoFormHandler autoFormHandler,
                        ICondoQuoteFormRepository condoRepository,
                         IQuoteLeadFormRepository quoteLeadRepository,

                                 IFormSaveHandler<MotorhomeForm> motorhomeFormSaveHandler,
                                 IFormSaveHandler<AutoForm> autoFormSaveHandler,
                                 IFormSaveHandler<WatercraftForm> watercraftFormSaveHandler,
                                 IFormSaveHandler<MotorcycleForm> motorcycleFormSaveHandler,
                                 IFormSaveHandler<FloodForm> floodFormSaveHandler,
                                 IFormSaveHandler<BusinessForm> businessFormSaveHandler,
                                 IFormSaveHandler<HillAFBForm> hillAFBFormSaveHandler,
                                   IFormSaveHandler<LeadGenerationForm> leadGenerationFormSaveHandler,
                                 IFormSaveHandler<UmbrellaForm> umbrellaFormSaveHandler,
                                 IFormSaveHandler<CollectorVehicleForm> collectorVehicleFormSaveHandler,
                                 IReCaptchaService reCaptchaService,
                                 IRenterQuoteFormRepository renterRepository,
                                 IFormSaveHandler<HomeownerForm> homeownerFormSaveHandler,
                                 IFormSaveHandler<HomenonownerForm> homenonownerFormSaveHandler,
                                 IFormSaveHandler<RenterForm> renterFormSaveHandler,
                                 IFormSaveHandler<MobilehomeForm> mobilehomeFormSaveHandler,
                                 IFormSaveHandler<CondoForm> condoFormSaveHandler,
                                  IFormSaveHandler<QuoteLeadForm> quoteLeadFormSaveHandler
            )
        {
            _autoRepository = autoRepository;
            _watercraftRepository = watercraftRepository;
            _motorcycleRepository = motorcycleRepository;
            _businessRepository = businessRepository;
            _hillRepository = hillRepository;
            _leadGenerationRepository = leadGenerationRepository;
            _umbrellaRepository = umbrellaRepository;
            _floodRepository = floodRepository;
            _collectorVehicleRepository = collectorVehicleRepository;
            _autoFormSaveHandler = autoFormSaveHandler;
            _watercraftFormSaveHandler = watercraftFormSaveHandler;
            _motorcycleFormSaveHandler = motorcycleFormSaveHandler;
            _productFormHandler = productFormHandler;
            _autoFormHandler = autoFormHandler;
            _motorhomeFormSaveHandler = motorhomeFormSaveHandler;
            _homehownerRepository = homehownerRepository;
            _homenonownerRepository = homenonownerRepository;
            _mobilehomeRepository = mobilehomeRepository;
            _motorhomeRepository = motorhomeRepository;
            _floodFormSaveHandler = floodFormSaveHandler;
            _businessFormSaveHandler = businessFormSaveHandler;
            _hillAFBFormSaveHandler = hillAFBFormSaveHandler;
            _leadGenerationFormSaveHandler = leadGenerationFormSaveHandler;
            _umbrellaFormSaveHandler = umbrellaFormSaveHandler;
            _collectorVehicleFormSaveHandler = collectorVehicleFormSaveHandler;
            _reCaptchaService = reCaptchaService;
            _renterRepository = renterRepository;
            _homeownerFormSaveHandler = homeownerFormSaveHandler;
            _homenonownerFormSaveHandler = homenonownerFormSaveHandler;
            _renterFormSaveHandler = renterFormSaveHandler;
            _mobilehomeFormSaveHandler = mobilehomeFormSaveHandler;
            _condoFormSaveHandler = condoFormSaveHandler;
            _condoRepository = condoRepository;
            _quoteLeadRepository = quoteLeadRepository;
            _quoteLeadFormSaveHandler = quoteLeadFormSaveHandler;
        }

        [HttpGet]
        [Route(APIRoutes.RETURN_AUTO_FORM)]
        public AutoForm GetAutoFormData(string key)
        {
            var form = _autoRepository.GetForm();
            _autoFormSaveHandler.ApplySessionFieldChanges(form, key);
            return form;
        }

        [HttpGet]
        [Route(APIRoutes.GET_AUTO_FORM)]
        public AutoForm GetAutoFormData()
        {
            var form = _autoRepository.GetForm();
            _autoFormSaveHandler.ApplySessionFieldChanges(form);
            return form;
        }

        [HttpGet]
        [Route(APIRoutes.GET_WATERCRAFT_FORM)]
        public WatercraftForm GetWaterCraftFormData()
        {
            var form = _watercraftRepository.GetForm();
            _watercraftFormSaveHandler.ApplySessionFieldChanges(form);
            return form;
        }

        [HttpGet]
        [Route(APIRoutes.RETURN_WATERCRAFT_FORM)]
        public WatercraftForm GetWaterCraftFormData(string key)
        {
            var form = _watercraftRepository.GetForm();
            _watercraftFormSaveHandler.ApplySessionFieldChanges(form, key);
            return form;
        }

        [HttpGet]
        [Route(APIRoutes.GET_MOTORCYCLE_FORM)]
        public MotorcycleForm GetMotorcycleFormData()
        {
            var form = _motorcycleRepository.GetForm();
            _motorcycleFormSaveHandler.ApplySessionFieldChanges(form);
            return form;
        }

        [HttpGet]
        [Route(APIRoutes.RETURN_MOTORCYCLE_FORM)]
        public MotorcycleForm GetMotorcycleFormData(string key)
        {
            var form = _motorcycleRepository.GetForm();
            _motorcycleFormSaveHandler.ApplySessionFieldChanges(form, key);
            return form;
        }

        [HttpGet]
        [Route(APIRoutes.RETURN_BUSINESS_FORM)]
        public BusinessForm GetBusinessFormData(string key)
        {
            var form = _businessRepository.GetForm();
            _businessFormSaveHandler.ApplySessionFieldChanges(form, key);
            return form;
        }

        [HttpGet]
        [Route(APIRoutes.GET_BUSINESS_FORM)]
        public BusinessForm GetBusinessFormData()
        {
            var form = _businessRepository.GetForm();
            _businessFormSaveHandler.ApplySessionFieldChanges(form);
            return form;
        }

        [HttpGet]
        [Route(APIRoutes.GET_QuoteLead_FORM)]
        public QuoteLeadForm GetQuoteLeadForm()
        {
            var form = _quoteLeadRepository.GetForm();
            //Temporary Comment for auto load city zip, state
            _quoteLeadFormSaveHandler.ApplySessionFieldChanges(form);
            return form;
        }

        [HttpGet]
        [Route(APIRoutes.RETURN_QuoteLead_FORM)]
        public QuoteLeadForm GetQuoteLeadFormData(string key)
        {
            var form = _quoteLeadRepository.GetForm();
            _quoteLeadFormSaveHandler.ApplySessionFieldChanges(form, key);
            return form;
        }
        [HttpGet]
        [Route(APIRoutes.GET_HILLAFB_FORM)]
        public HillAFBForm GetHillAfbForm()
        {
            var form = _hillRepository.GetForm();
            _hillAFBFormSaveHandler.ApplySessionFieldChanges(form);
            return form;
        }

        [HttpGet]
        [Route(APIRoutes.RETURN_HillAFB_FORM)]
        public HillAFBForm GetHillAFBFormData(string key)
        {
            var form = _hillRepository.GetForm();
            _hillAFBFormSaveHandler.ApplySessionFieldChanges(form, key);
            return form;
        }

        [HttpGet]
        [Route(APIRoutes.GET_LEADGENERATION_FORM)]
        public LeadGenerationForm GetLeadGenerationForm()
        {
            var form = _leadGenerationRepository.GetForm();
            _leadGenerationFormSaveHandler.ApplySessionFieldChanges(form);
            return form;
        }

        [HttpGet]
        [Route(APIRoutes.RETURN_LEADGENERATION_FORM)]
        public LeadGenerationForm GetLeadGenerationFormData(string key)
        {
            var form = _leadGenerationRepository.GetForm();
            _leadGenerationFormSaveHandler.ApplySessionFieldChanges(form, key);
            return form;
        }

        [HttpGet]
        [Route(APIRoutes.GET_UMBRELLA_FORM)]
        public UmbrellaForm GetUmbrellaFormData()
        {
            var form = _umbrellaRepository.GetForm();
            _umbrellaFormSaveHandler.ApplySessionFieldChanges(form);
            return form;
        }

        [HttpGet]
        [Route(APIRoutes.RETURN_UMBRELLA_FORM)]
        public UmbrellaForm GetUmbrellaFormData(string key)
        {
            var form = _umbrellaRepository.GetForm();
            _umbrellaFormSaveHandler.ApplySessionFieldChanges(form, key);
            return form;

        }

        [HttpGet]
        [Route(APIRoutes.RETURN_FLOOD_FORM)]
        public FloodForm GetFloodFormData(string key)
        {
            var form = _floodRepository.GetForm();
            _floodFormSaveHandler.ApplySessionFieldChanges(form, key);
            return form;
        }

        [HttpGet]
        [Route(APIRoutes.GET_FLOOD_FORM)]
        public FloodForm GetFloodFormData()
        {
            var form = _floodRepository.GetForm();
            _floodFormSaveHandler.ApplySessionFieldChanges(form);
            return form;
        }
        [HttpGet]
        [Route(APIRoutes.GET_COLLECTOR_FORM)]
        public CollectorVehicleForm GetCollectorFormData()
        {
            var form = _collectorVehicleRepository.GetForm();
            _collectorVehicleFormSaveHandler.ApplySessionFieldChanges(form);
            return form;
        }

        [HttpGet]
        [Route(APIRoutes.RETURN_COLLECTOR_FORM)]
        public CollectorVehicleForm GetCollectorFormData(string key)
        {
            var form = _collectorVehicleRepository.GetForm();
            _collectorVehicleFormSaveHandler.ApplySessionFieldChanges(form, key);
            return form;
        }

        [HttpGet]
        [Route(APIRoutes.GET_MOTORHOME_FORM)]
        public MotorhomeForm GetMotorhomeFormData()
        {
            var form = _motorhomeRepository.GetForm();
            _motorhomeFormSaveHandler.ApplySessionFieldChanges(form);
            return form;
        }

        [HttpGet]
        [Route(APIRoutes.RETURN_MOTORHOME_FORM)]
        public MotorhomeForm GetMotorhomeFormData(string key)
        {
            var form = _motorhomeRepository.GetForm();
            _motorhomeFormSaveHandler.ApplySessionFieldChanges(form, key);
            return form;
        }

        [HttpGet]
        [Route(APIRoutes.GET_HOMEOWNER_FORM)]
        public HomeownerForm GetHomeownerFormData()
        {
            var form = _homehownerRepository.GetForm();
            _homeownerFormSaveHandler.ApplySessionFieldChanges(form);
            return form;
        }
        [HttpGet]
        [Route(APIRoutes.GET_HOMENONOWNER_FORM)]
        public HomenonownerForm GetHomenonownerFormData()
        {
            var form = _homenonownerRepository.GetForm();
            _homenonownerFormSaveHandler.ApplySessionFieldChanges(form);
            return form;
        }
        [HttpGet]
        [Route(APIRoutes.GET_CONDO_FORM_N)]
        public CondoForm GetCondoNFormData()
        {
            var form = _condoRepository.GetForm();
            _condoFormSaveHandler.ApplySessionFieldChanges(form);
            return form;
        }
        [HttpGet]
        [Route(APIRoutes.GET_MOBILEHOME_FORM)]
        public MobilehomeForm GetMobilehomeFormData()
        {
            var form = _mobilehomeRepository.GetForm();
            _mobilehomeFormSaveHandler.ApplySessionFieldChanges(form);
            return form;
        }

        [HttpGet]
        [Route(APIRoutes.GET_RENTER_FORM)]
        public RenterForm GetRenterFormData()
        {
            var form = _renterRepository.GetForm();
            _renterFormSaveHandler.ApplySessionFieldChanges(form);
            return form;
        }

        [HttpGet]
        [Route(APIRoutes.GET_CONDO_FORM)]
        public RenterForm GetCondoFormData()
        {
            var form = _renterRepository.GetForm();
            _renterFormSaveHandler.ApplySessionFieldChanges(form);
            return form;
        }
        [HttpPost]
        [Route(APIRoutes.GET_PRODUCT_AVAILABILITY)]
        public ProductAvailabilityResponse GetProductAvailability()
        {
            var productAvailabilityModel = FormExtensions.GetDeserializedForm<ProductAvailabilityPostModel>(HttpContext.Current.Request.InputStream);
            return _productFormHandler.GetProductAvailability(productAvailabilityModel);
        }

        [HttpPost]
        [Route(APIRoutes.GET_AUTO_BY_VIN)]
        public VehicleVinInformation GetAutoByVin()
        {
            var autoByVinModel = FormExtensions.GetDeserializedForm<AutoByVinPostModel>(HttpContext.Current.Request.InputStream);
            return _autoFormHandler.GetByVin(autoByVinModel);
        }

        [HttpPost]
        [Route(APIRoutes.GET_AUTO_MAKE_BY_YEAR)]
        public VehicleMakeInformation GetAutoMakeByYear()
        {
            var byYearPostModel = FormExtensions.GetDeserializedForm<AutoMakeByYearPostModel>(HttpContext.Current.Request.InputStream);
            return _autoFormHandler.GetMakesByYear(byYearPostModel);
        }

        [HttpPost]
        [Route(APIRoutes.GET_AUTO_MODEL_BY_MAKE)]
        public VehicleModelInformation GetModelByMake()
        {
            var byMakePostModel = FormExtensions.GetDeserializedForm<AutoModelByMakePostModel>(HttpContext.Current.Request.InputStream);
            return _autoFormHandler.GetModelsByMake(byMakePostModel);
        }

        [HttpPost]
        [Route(APIRoutes.GET_AUTO_BODY_STYLES_BY_MODEL)]
        public VehicleBodyTypeInformation GetBodyTypeByModel()
        {
            var byModelPostModel = FormExtensions.GetDeserializedForm<AutoBodyTypeByModelPostModel>(HttpContext.Current.Request.InputStream);
            return _autoFormHandler.GetBodyTypesByModel(byModelPostModel);
        }

        [HttpPost]
        [Route(APIRoutes.SAVE_AUTO_FORM)]
        public HttpResponseMessage SaveAutoForm()
        {
            return HandleSave(_autoFormSaveHandler);
        }

        [HttpPost]
        [Route(APIRoutes.SAVE_BUSINESS_FORM)]
        public HttpResponseMessage SaveBusinessForm()
        {
            return HandleSave(_businessFormSaveHandler);
        }

        [HttpPost]
        [Route(APIRoutes.SAVE_HillAFB_FORM)]
        public HttpResponseMessage SaveHillAFBForm()
        {
            return HandleSave(_hillAFBFormSaveHandler);
        }

        [HttpPost]
        [Route(APIRoutes.SAVE_QuoteLead_FORM)]
        public HttpResponseMessage SaveQuoteLeadForm()
        {
            return HandleSave(_quoteLeadFormSaveHandler);
        }

        [HttpPost]
        [Route(APIRoutes.SAVE_LEADGENERATION_FORM)]
        public HttpResponseMessage SaveLeadGenerationForm()
        {
            return HandleSave(_leadGenerationFormSaveHandler);
        }
        [HttpPost]
        [Route(APIRoutes.SAVE_COLLECTOR_FORM)]
        public HttpResponseMessage SaveCollectorForm()
        {
            return HandleSave(_collectorVehicleFormSaveHandler);
        }

        [HttpPost]
        [Route(APIRoutes.SAVE_FLOOD_FORM)]
        public HttpResponseMessage SaveFloodForm()
        {
            return HandleSave(_floodFormSaveHandler);
        }

        [HttpPost]
        [Route(APIRoutes.SAVE_UMBRELLA_FORM)]
        public HttpResponseMessage SaveUmbrellaForm()
        {
            return HandleSave(_umbrellaFormSaveHandler);
        }

        [HttpPost]
        [Route(APIRoutes.SAVE_WATERCRAFT_FORM)]
        public HttpResponseMessage SaveWatercraftForm()
        {
            return HandleSave(_watercraftFormSaveHandler);
        }

        [HttpPost]
        [Route(APIRoutes.SAVE_MOTORCYCLE_FORM)]
        public HttpResponseMessage SaveMotorcycleForm()
        {
            return HandleSave(_motorcycleFormSaveHandler);
        }

        [HttpPost]
        [Route(APIRoutes.SAVE_MOTORHOME_FORM)]
        public HttpResponseMessage SaveMotorhomeForm()
        {
            return HandleSave(_motorhomeFormSaveHandler);
        }

        [HttpPost]
        [Route(APIRoutes.SAVE_HOMEOWNER_FORM)]
        public HttpResponseMessage SaveHomeownerForm()
        {
            return HandleSave(_homeownerFormSaveHandler);
        }
        [HttpPost]
        [Route(APIRoutes.SAVE_HOMENONOWNER_FORM)]
        public HttpResponseMessage SaveHomenonownerForm()
        {
            return HandleSave(_homenonownerFormSaveHandler);
        }
        [HttpPost]
        [Route(APIRoutes.SAVE_CONDO_FORM)]
        public HttpResponseMessage SaveCondoForm()
        {
            return HandleSave(_condoFormSaveHandler);
        }
        [HttpPost]
        [Route(APIRoutes.SAVE_MOBILEHOME_FORM)]
        public HttpResponseMessage SaveMobilehomeForm()
        {
            return HandleSave(_mobilehomeFormSaveHandler);
        }

        [HttpPost]
        [Route(APIRoutes.SAVE_RENTER_FORM)]
        public HttpResponseMessage SaveRenterForm()
        {
            return HandleSave(_renterFormSaveHandler);
        }

        
        private bool ReCaptchaValidation(string recaptchaToken)
        {
            ReCaptchaVerifyResponse response = _reCaptchaService.VerifyResponse(recaptchaToken);
            if (response.Success) return true;
            try
            {
                Sitecore.Diagnostics.Log.Info($"Quote Form ReCaptcha invalid for token: {recaptchaToken}, error codes from ReCaptcha service: {string.Join(",", response.ErrorCodes)}", Constants.QUOTE_FORMS_LOGGER_NAME);
            }
            catch (Exception e)
            {
                Sitecore.Diagnostics.Log.Error("Cannot log ReCaptcha error", e, Constants.QUOTE_FORMS_LOGGER_NAME);
            }
            return false;
        }

        private string GetReCaptchaToken(string jsonBody)
        {
            var obj = JObject.Parse(jsonBody);
            var token = (string)obj["recaptchaToken"];
            return token;
        }

        private HttpResponseMessage HandleSave<T>(IFormSaveHandler<T> handler) where T : IQuoteForm
        {
            Sitecore.Diagnostics.Log.Info("Invoke HandleSave FormApiController for form Type: " + typeof(T).Name, Constants.QUOTE_FORMS_LOGGER_NAME);
            Stopwatch sw = new Stopwatch();
            try
            {
                var cookie = Request.Headers.GetCookies("ASP.NET_SessionId").FirstOrDefault();
                if (cookie == null)
                {
                    return Request.CreateResponse(HttpStatusCode.BadRequest);
                }

                sw.Start();
                var json = GetContentFromBody();
                CommonFormSaveResponseModel handlerResponse = new CommonFormSaveResponseModel();
                string reCaptchaToken = GetReCaptchaToken(json);

                sw.Stop();
                Sitecore.Diagnostics.Log.Info("HandleSave FormApiController JSON ContentBody and parse time elapsed " + sw.ElapsedMilliseconds, Constants.QUOTE_FORMS_LOGGER_NAME);

                handlerResponse.IsRecaptchaValid = true;
                //if (string.IsNullOrEmpty(reCaptchaToken) || !ReCaptchaValidation(reCaptchaToken))
                //{
                //    handlerResponse.IsRecaptchaValid = false;
                //    return Request.CreateResponse(HttpStatusCode.OK, handlerResponse);
                //}

                sw.Restart();
                handlerResponse = handler.HandlePost(json, cookie["ASP.NET_SessionId"]?.Value);
                sw.Stop();
                Sitecore.Diagnostics.Log.Info("HandleSave FormApiController Form HandlePost time elapsed" + sw.ElapsedMilliseconds, Constants.QUOTE_FORMS_LOGGER_NAME);
                return Request.CreateResponse(HttpStatusCode.OK, handlerResponse);
            }
            catch (InvalidOperationException ioe)
            {
                Sitecore.Diagnostics.Log.Error("Quote Forms error - Invalid Operation", ioe, Constants.QUOTE_FORMS_LOGGER_NAME);
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            catch (JsonReaderException jre)
            {
                Sitecore.Diagnostics.Log.Error("Quote Forms error - JSON Reader", jre, Constants.QUOTE_FORMS_LOGGER_NAME);
                return Request.CreateResponse(HttpStatusCode.BadRequest);
            }
            catch (System.Exception ex)
            {
                Sitecore.Diagnostics.Log.Error("Quote Forms error", ex, Constants.QUOTE_FORMS_LOGGER_NAME);
                return Request.CreateResponse(HttpStatusCode.InternalServerError);
            }
        }


        private string GetContentFromBody()
        {
            return Request.Content.ReadAsStringAsync().GetAwaiter().GetResult();
        }
    }
}