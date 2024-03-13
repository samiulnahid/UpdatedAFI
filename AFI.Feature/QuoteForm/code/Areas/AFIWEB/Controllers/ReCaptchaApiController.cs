using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using AFI.Feature.QuoteForm.Areas.AFIWEB.Services;
using Newtonsoft.Json;
using Sitecore.Mvc.Controllers;

namespace AFI.Feature.QuoteForm.Areas.AFIWEB.Controllers
{
    [RoutePrefix("api/recaptcha")]
    public class ReCaptchaApiController : SitecoreController
    {
        private readonly IReCaptchaService _reCaptchaService;

        public ReCaptchaApiController(IReCaptchaService reCaptchaService)
        {
            _reCaptchaService = reCaptchaService;
        }

        [HttpGet]
        [Route("verify")]
        public ActionResult Verify(string token)
        {
            var verificationResponse = _reCaptchaService.VerifyResponse(token);
            var responseSerialized = Content(JsonConvert.SerializeObject(verificationResponse), "application/json");
            Session["reCaptchaToken"] = responseSerialized;
            return responseSerialized;
        }
    }
}