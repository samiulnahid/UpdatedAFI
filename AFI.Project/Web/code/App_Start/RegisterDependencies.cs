using AFI.Feature.Data.Providers;
using AFI.Feature.Prospect.Providers;
using AFI.Feature.Data.Repositories;
using AFI.Feature.VinLookup;
using AFI.Feature.WebQuoteService.Repositories;
using AFI.Feature.ZipLookup;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Handlers;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.AutoForm;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.BusinessForm;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.CollectorForm;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.FloodForm;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.HillAFBForm;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.LeadGenerationForm;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.HomenonownerForm;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.HomeownerForm;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.MobilehomeForm;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.MotorcycleForm;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.Motorhome;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.RenterForm;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.UmbrellaForm;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.WatercraftForm;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Repository;
using AFI.Feature.QuoteForm.Areas.AFIWEB.ModelBuilders;
using AFI.Feature.QuoteForm.Areas.AFIWEB.Processors;
using AFI.Feature.QuoteForm.Areas.AFIWEB.Repository;
using AFI.Feature.QuoteForm.Areas.AFIWEB.Services;
using AFI.Feature.QuoteForm.Areas.ShortForms.Repository;
using AFI.Foundation.Helper.Models;
//using Elision.Foundation.Kernel;
using Microsoft.Extensions.DependencyInjection;
using Sitecore.DependencyInjection;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.CondoForm;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.QuoteLeadForm;
//using Quote_Repo = AFI.Feature.Quote.Repository;
//using Quote_Contr = AFI.Feature.Quote.Controllers;
namespace AFI.Project.Web
{
    public class RegisterDependencies : IServicesConfigurator
    {
        public void Configure(IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IGlobalSettingsRepository, GlobalSettingsRepository>();
            serviceCollection.AddTransient<INewsLetterRepository, NewsLetterRepository>();
            serviceCollection.AddTransient<IFormRepository, FormRepository>();
            serviceCollection.AddTransient<IAutoQuoteFormRepository, AutoQuoteFormRepository>();
            serviceCollection.AddTransient<IWatercraftQuoteFormRepository, WatercraftQuoteFormRepository>();
            serviceCollection.AddTransient<IMotorcycleQuoteFormRepository, MotorcycleQuoteFormRepository>();
            serviceCollection.AddTransient<IBusinessQuoteFormRepository, BusinessQuoteFormRepository>();
            serviceCollection.AddTransient<IHillAFBQuoteFormRepository, HillAFBQuoteFormRepository>();
            serviceCollection.AddTransient<ILeadGenerationFormRepository, LeadGenerationFormRepository>();
            serviceCollection.AddTransient<IUmbrellaQuoteFormRepository, UmbrellaQuoteFormRepository>();
            serviceCollection.AddTransient<IFloodQuoteFormRepository, FloodQuoteFormRepository>();
            serviceCollection.AddTransient<ICollectorVehicleQuoteFormRepository, CollectorVehicleQuoteFormRepository>();
            serviceCollection.AddTransient<IMotorhomeQuoteFormRepository, MotorhomeQuoteFormRepository>();
            serviceCollection.AddTransient<IHomeownerQuoteFormRepository, HomeownerQuoteFormRepository>();
            serviceCollection.AddTransient<IHomenonownerQuoteFormRepository, HomenonownerQuoteFormRepository>();
            serviceCollection.AddTransient<IMobilehomeQuoteFormRepository, MobilehomeQuoteFormRepository>();
            serviceCollection.AddTransient<IProductFormHandler, ProductFormHandler>();
            serviceCollection.AddTransient<ICommonRepository, CommonRepository>();
            serviceCollection.AddTransient<IAPIRepository, APIRepository>();
            serviceCollection.AddTransient<IPressReleaseRepository, PressReleaseRepository>();
            //serviceCollection.AddTransient<ISearchRepository, SearchRepository>();
            serviceCollection.AddTransient<IEmailService, EmailService>();
            serviceCollection.AddTransient<IReCaptchaService, ReCaptchaService>();
			//serviceCollection.AddTransient<ISiteSearchResultModelBuilder, SiteSearchResultModelBuilder>();
		
			serviceCollection.AddTransient<IFormEmailHandler, FormEmailHandler>();
			serviceCollection.AddTransient<IContactUsFormViewModelBuilder, ContactUsFormViewModelBuilder>();
			serviceCollection.AddTransient<IContactUsFormProcessor, ContactUsFormProcessor>();
			serviceCollection.AddTransient<IContactUsEmailBuilder, ContactUsEmailBuilder>();

            serviceCollection.AddTransient<IZipLookupService, ZipLookupService>();
            serviceCollection.AddTransient<IVinLookupService, VinLookupService>();
			serviceCollection.AddSingleton<IPartnerAdvisorRepository, PartnerAdvisorRepository>();

            serviceCollection.AddTransient<IShortFormRepository, ShortFormRepository>();
            serviceCollection.AddTransient<ICondoQuoteFormRepository, CondoQuoteFormRepository>();



            // Data Registrations
            serviceCollection.AddSingleton<IDatabaseConnectionProvider, DatabaseConnectionProvider>();
            serviceCollection.AddSingleton<IDbConnectionProvider, DbConnectionProvider>();
			serviceCollection.AddTransient<IReferralRepository, ReferralRepository>();
            serviceCollection.AddTransient<IQuoteRepository, QuoteRepository>();
            serviceCollection.AddTransient<IQuoteAutoRepository, QuoteAutoRepository>();
            serviceCollection.AddTransient<IQuoteAutoDriverRepository, QuoteAutoDriverRepository>();
            serviceCollection.AddTransient<IQuoteAutoIncidentRepository, QuoteAutoIncidentRepository>();
            serviceCollection.AddTransient<IQuoteAutoVehicleRepository, QuoteAutoVehicleRepository>();
            serviceCollection.AddTransient<IQuoteCollectorVehicleRepository, QuoteCollectorVehicleRepository>();
            serviceCollection.AddTransient<IQuoteCommercialRepository, QuoteCommercialRepository>();
            serviceCollection.AddTransient<IQuoteHillAFBRepository, QuoteHillAFBRepository>();
            serviceCollection.AddTransient<IQuoteLeadGenerationRepository, QuoteLeadGenerationRepository>();
            serviceCollection.AddTransient<IQuoteContactRepository, QuoteContactRepository>();
            serviceCollection.AddTransient<IQuoteCountyFilterRepository, QuoteCountyFilterRepository>();
            serviceCollection.AddTransient<IQuoteFloodRepository, QuoteFloodRepository>();
            serviceCollection.AddTransient<IQuoteMobilehomeRepository, QuoteMobilehomeRepository>();
            serviceCollection.AddTransient<IQuoteHomeownerRepository, QuoteHomeownerRepository>();
            serviceCollection.AddTransient<IQuoteHomeownerLossRepository, QuoteHomeownerLossRepository>();
            serviceCollection.AddTransient<IQuoteHomenonownerRepository, QuoteHomenonownerRepository>();
            serviceCollection.AddTransient<IRenterQuoteFormRepository, RenterQuoteFormRepository>();
            serviceCollection.AddTransient<IQuoteMotorcycleVehicleRepository, QuoteMotorcycleVehicleRepository>();
            serviceCollection.AddTransient<IQuoteMotorhomeVehicleRepository, QuoteMotorhomeVehicleRepository>();
            serviceCollection.AddTransient<IQuoteRecreationVehicleRepository, QuoteRecreationVehicleRepository>();
            serviceCollection.AddTransient<IQuoteRenterRepository, QuoteRenterRepository>();
            serviceCollection.AddTransient<IQuoteRenterApplicationRepository, QuoteRenterApplicationRepository>();
            serviceCollection.AddTransient<IQuoteRenterDogsRepository, QuoteRenterDogsRepository>();
            serviceCollection.AddTransient<IQuoteSpouseRepository, QuoteSpouseRepository>();
            serviceCollection.AddTransient<IQuoteStateRepository, QuoteStateRepository>();
            serviceCollection.AddTransient<IQuoteUmbrellaRepository, QuoteUmbrellaRepository>();
            serviceCollection.AddTransient<IQuoteUmbrellaVehicleRepository, QuoteUmbrellaVehicleRepository>();
            serviceCollection.AddTransient<IQuoteUmbrellaWatercraftRepository, QuoteUmbrellaWatercraftRepository>();
            serviceCollection.AddTransient<IQuoteVIPRepository, QuoteVIPRepository>();
            serviceCollection.AddTransient<IQuoteVIPItemsRepository, QuoteVIPItemsRepository>();
            serviceCollection.AddTransient<IQuoteWatercraftVehicleRepository, QuoteWatercraftVehicleRepository>();
            serviceCollection.AddTransient<IQuoteZipCodeFilterRepository, QuoteZipCodeFilterRepository>();
            serviceCollection.AddTransient<IQuoteCondoRepository, QuoteCondoRepository>();
            // Handlers
            serviceCollection.AddTransient<IFormSaveHandler<AutoForm>, AutoFormSaveHandler>();
            serviceCollection.AddTransient<IFormSaveHandler<MotorcycleForm>, MotorcycleFormSaveHandler>();
            serviceCollection.AddTransient<IFormSaveHandler<MotorhomeForm>, MotorhomeFormSaveHandler>();
            serviceCollection.AddTransient<IFormSaveHandler<WatercraftForm>, WatercraftFormSaveHandler>();
			serviceCollection.AddTransient<IFormSaveHandler<FloodForm>, FloodFormSaveHandler>();
            serviceCollection.AddTransient<IFormSaveHandler<BusinessForm>, BusinessFormSaveHandler>();
            serviceCollection.AddTransient<IFormSaveHandler<HillAFBForm>, HillAFBFormSaveHandler>();
           serviceCollection.AddTransient<IFormSaveHandler<LeadGenerationForm>, LeadGenerationFormSaveHandler>();
            serviceCollection.AddTransient<IFormSaveHandler<UmbrellaForm>, UmbrellaFormSaveHandler>();
            serviceCollection.AddTransient<IFormSaveHandler<CollectorVehicleForm>, CollectorVehicleFormSaveHandler>();
            serviceCollection.AddTransient<IFormSaveHandler<HomeownerForm>, HomeOwnerFormSaveHandler>();
            serviceCollection.AddTransient<IFormSaveHandler<HomenonownerForm>, HomeNonOwnerFormSaveHandler>();
            serviceCollection.AddTransient<IFormSaveHandler<MobilehomeForm>, MobileHomeFormSaveHandler>();
            serviceCollection.AddTransient<IFormSaveHandler<RenterForm>, RenterFormSaveHandler>();
			serviceCollection.AddTransient<ISessionRepository, SessionRepository>();
            serviceCollection.AddTransient<IAutoFormHandler, AutoFormHandler>();
            serviceCollection.AddTransient<IAFIWebServicesSaveHandler, AFIWebServicesSaveHandler>();
            serviceCollection.AddTransient<IAFIFormsMapRepository, AFIFormsMapRepository>();
            serviceCollection.AddTransient<IAFIFormsSentToRepository, AFIFormsSentToRepository>();
            serviceCollection.AddTransient<IAFIReportRepository, AFIReportRepository>();
            serviceCollection.AddTransient<ICorviasFormRepository, CorviasFormRepository>();
            serviceCollection.AddTransient<IUCFormRepository, UCFormRepository>();
            serviceCollection.AddTransient<IFormSaveHandler<CondoForm>, CondoFormSaveHandler>();
            serviceCollection.AddTransient<IHillAFBFormRepository, HillAFBFormRepository>();

            serviceCollection.AddTransient<IQuoteLeadRepository, QuoteLeadRepository>();
            serviceCollection.AddTransient<IQuoteLeadFormRepository, QuoteLeadFormRepository>();
            serviceCollection.AddTransient<IFormSaveHandler<QuoteLeadForm>, QuoteLeadFormSaveHandler>();
            //serviceCollection.AddTransient<IVoteRepository, VoteRepository>();
            // Controller Registrations
            serviceCollection.AddMvcControllers("AFI.Project.Web*");
         
        }
    }
}