using System;
using System.Configuration;
using AFI.Feature.QuoteForm.Areas.AFIWEB.Models;
using DEG.ServiceCore;
using DEG.ServiceCore.Authentication;

namespace AFI.Feature.QuoteForm.Areas.AFIWEB.Services
{
    public interface IReCaptchaService
    {
        ReCaptchaVerifyResponse VerifyResponse(string reCaptchaResponse);
    }

    public class ReCaptchaService : ServiceBase, IReCaptchaService
    {
        private const string VERIFICATION_URL = "https://www.google.com/recaptcha/api/siteverify";
        public ReCaptchaService() : base(new NoAuthentication()) { }

        public ReCaptchaVerifyResponse VerifyResponse(string reCaptchaResponse)
        {
            var secret = GetReCaptchaSecret();
            var response = new ReCaptchaVerifyResponse { Success = false };

            if (string.IsNullOrWhiteSpace(secret))
                return response;

            var verificationUrl = $"{VERIFICATION_URL}?secret={secret}&response={reCaptchaResponse}";

            try
            {
                response = GetObject<ReCaptchaVerifyResponse>(verificationUrl);
            }
            catch (Exception ex)
            {
                Sitecore.Diagnostics.Log.Error($"{nameof(ReCaptchaService)}: An error occured when verifying ReCAPTCHA response: '{ex.Message}'", ex, this);
            }
            return response ?? new ReCaptchaVerifyResponse { Success = false };
        }

        private string GetReCaptchaSecret()
        {
			var secret = ConfigurationManager.AppSettings["ReCaptcha.Secret"];
            if (string.IsNullOrWhiteSpace(secret))
            {
                Sitecore.Diagnostics.Log.Error(
					$"{nameof(ReCaptchaService)}: The setting 'ReCaptcha.Secret' is required for website functionality, but was not found.",
                    this);
                return null;
            }
            return secret;
        }
    }
}