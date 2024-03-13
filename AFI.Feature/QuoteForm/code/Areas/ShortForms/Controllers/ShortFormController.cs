using System.Web.Mvc;
using AFI.Feature.Identifiers;
using AFI.Feature.QuoteForm.Areas.AFIWEB.Repository;
using AFI.Feature.QuoteForm.Areas.ShortForms.Models;
using AFI.Feature.QuoteForm.Areas.ShortForms.Repository;
using Sitecore.Data.Fields;
using Sitecore.Links;

namespace AFI.Feature.QuoteForm.Areas.ShortForms.Controllers
{
    public class ShortFormController : Controller
    {
        private readonly IShortFormRepository _repository;
        private readonly IGlobalSettingsRepository _globalSettingsRepository;

        public ShortFormController(IShortFormRepository repository, IGlobalSettingsRepository globalSettingsRepository)
        {
            _repository = repository;
            _globalSettingsRepository = globalSettingsRepository;
        }

        // GET: ShortForms/ShortForm
        //public ActionResult Index()
        //{
        //    //return PartialView(new ShortFormViewModel());

        //    return View("/Views/AFI/Forms/ShortForms.cshtml", PartialView(new ShortFormViewModel()));
        //}

        public ActionResult Short_Form()
        {
            //return PartialView(new ShortFormViewModel());
            ShortFormViewModel _shortFormViewModel = new ShortFormViewModel();

            return View("/Views/AFI/Forms/ShortForms.cshtml", _shortFormViewModel);
        }

        [HttpPost]
        public ActionResult Index(ShortFormViewModel form)
        {
			if (form == null || form.IsEmpty())
			{
				return PartialView(form);
			}

            ShortFormModel model = new ShortFormModel
            {
                FirstName = form.FirstName,
                LastName = form.LastName,
                EmailAddress = form.Email,
                PhoneNumber = form.Phone,
                Address = form.Address,
                City = form.City,
                State = form.State,
                ZipCode = form.Zip
            };
            _repository.SubmitForm(model);
            LinkField confirmationUrlField = _globalSettingsRepository.GetSettingField(Templates.Global_Settings.FieldNames.Short_Form_Confirmation_Page);
            string confirmationUrl = confirmationUrlField.GetFriendlyUrl();
            return string.IsNullOrWhiteSpace(confirmationUrl) ? (ActionResult) PartialView(form) : Redirect(confirmationUrl);
        }

     

    }
}