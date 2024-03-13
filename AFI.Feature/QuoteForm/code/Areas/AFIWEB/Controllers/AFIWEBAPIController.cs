using System.Web.Http;
using AFI.Feature.QuoteForm.Areas.AFIWEB.Models;
using AFI.Feature.QuoteForm.Areas.AFIWEB.Processors;
using AFI.Feature.QuoteForm.Areas.AFIWEB.Repository;
using Identifiers = AFI.Feature.Identifiers;

namespace AFI.Feature.QuoteForm.Areas.AFIWEB.Controllers
{
	[System.Web.Http.RoutePrefix(AFIWEBRoutes.API_ROUTE_PREFIX)]
    public class AFIWEBAPIController : ApiController
    {
		private readonly IContactUsFormProcessor _contactUsFormProcessor;
        private readonly IGlobalSettingsRepository _GlobalSettingsRepository;

        public AFIWEBAPIController(IContactUsFormProcessor contactUsFormProcessor, IGlobalSettingsRepository globalSettingsRepository)
        {
			_contactUsFormProcessor = contactUsFormProcessor;
            _GlobalSettingsRepository = globalSettingsRepository;
        }
        [System.Web.Http.HttpPost]
        [System.Web.Http.Route(AFIWEBRoutes.GET_CONTACT_US_FORM)]
        public ContactUsResponse Contactus(ContactUsPostModel model)
        {
			var output = new ContactUsResponse();
			var response = _contactUsFormProcessor.Process(model);

			if (!response.WasSuccessful)
			{
				output.success = false;
				output.message = response.FailureMessage;
			} else
			{
				output.success = true;
				output.title = _GlobalSettingsRepository.GetSetting(Identifiers.Templates.Global_Settings.FieldNames.Contact_Us_Form_Response_Message_Title_Text);
				output.ctaButtonText = _GlobalSettingsRepository.GetSetting(Identifiers.Templates.Global_Settings.FieldNames.Contact_Us_Form_Response_Message_CTA_Button_Text);
				output.ctaInstructionText = _GlobalSettingsRepository.GetSetting(Identifiers.Templates.Global_Settings.FieldNames.Contact_Us_Form_Response_Message_CTA_Instruction_Text);
				output.message = _GlobalSettingsRepository.GetSetting(Identifiers.Templates.Global_Settings.FieldNames.Contact_Us_Form_Response_Message_Message_Text);
			}

			return output;
        }
	}
}