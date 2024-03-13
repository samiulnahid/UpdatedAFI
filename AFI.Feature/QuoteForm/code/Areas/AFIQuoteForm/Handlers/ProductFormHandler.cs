using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AFI.Feature.Data.DataModels;
using AFI.Feature.Data.Repositories;
using AFI.Feature.ZipLookup;
using AFI.Feature.ZipLookup.Models;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Handlers.Validators;
using AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Models.Common;

namespace AFI.Feature.QuoteForm.Areas.AFIQuoteForm.Handlers
{
    public class ProductFormHandler : IProductFormHandler
    {
        private readonly IZipLookupService _zipLookupService;
        private readonly IQuoteStateRepository _stateRepository;
        private readonly IQuoteZipCodeFilterRepository _zipCodeFilterRepository;
        private readonly IQuoteCountyFilterRepository _countyFilterRepository;
        private ProductAvailabilityPostModel _postModel;
        private ZipLookupModel _lookupModel;

        public ProductFormHandler(IZipLookupService zipLookupService, IQuoteStateRepository stateRepository, IQuoteZipCodeFilterRepository zipCodeFilterRepository, IQuoteCountyFilterRepository countyFilterRepository)
        {
            _zipLookupService = zipLookupService;
            _stateRepository = stateRepository;
            _zipCodeFilterRepository = zipCodeFilterRepository;
            _countyFilterRepository = countyFilterRepository;
        }
        public ProductAvailabilityResponse GetProductAvailability(ProductAvailabilityPostModel productValidationModel)
        {
            _postModel = productValidationModel;
            try
            {
                _lookupModel = _zipLookupService.LookupZipCode(productValidationModel.ZipCode);
                if (string.IsNullOrWhiteSpace(_lookupModel.ZipCode)) return new ProductAvailabilityResponse();
                bool validState = ValidateState();
                bool validZip = (validState) ? ValidateZip() : false;
                bool validCounty = (validState && validZip) ? ValidateCounty(): false;
                return new ProductAvailabilityResponse()
                {
                    Available = validState && 
                                validZip && 
                                validCounty
                };
            }
            catch (ArgumentNullException)
            {
                return new ProductAvailabilityResponse();
            }
        }

        private bool ValidateState()
        {
            IEnumerable<QuoteState> states = _stateRepository.GetAll();
			switch (_postModel.Type.ToLowerInvariant())
			{
				case "auto":
					return AutoStateValidator.Validate(states, _lookupModel.StateAbbreviation);
				case "homeowners":
					return HomeownersStateValidator.Validate(states, _lookupModel.StateAbbreviation);
				case "renters":
					return RentersStateValidator.Validate(states, _lookupModel.StateAbbreviation);
				case "umbrella":
					return UmbrellaStateValidator.Validate(states, _lookupModel.StateAbbreviation);
				case "flood":
					return FloodStateValidator.Validate(states, _lookupModel.StateAbbreviation);
				case "watercraft":
					return WatercraftStateValidator.Validate(states, _lookupModel.StateAbbreviation);
				case "motorhome":
					return MotorhomeStateValidator.Validate(states, _lookupModel.StateAbbreviation);
				case "motorcycle":
					return MotorcycleStateValidator.Validate(states, _lookupModel.StateAbbreviation);
				case "travel trailer":
					return MotorhomeStateValidator.Validate(states, _lookupModel.StateAbbreviation);
				case "mobile home":
					return HomeownersStateValidator.Validate(states, _lookupModel.StateAbbreviation);
				case "mobilehome":
					return HomeownersStateValidator.Validate(states, _lookupModel.StateAbbreviation);
				case "business":
					return BusinessStateValidator.Validate(states, _lookupModel.StateAbbreviation);
				case "collector vehicle":
					return CollectorVehicleStateValidator.Validate(states, _lookupModel.StateAbbreviation);
				case "homenonowners":
					return HomenonownersStateValidator.Validate(states, _lookupModel.StateAbbreviation);
				case "condo":
					return CondoStateValidator.Validate(states, _lookupModel.StateAbbreviation);
				default:
					return false;
			}
        }

        private bool ValidateZip()
        {
            IEnumerable<QuoteZipCodeFilter> zipCodes = _zipCodeFilterRepository.GetAll();
            if (!zipCodes.Any()) return true;
			switch (_postModel.Type.ToLowerInvariant())
			{
				case "auto":
					return AutoZipCodeValidator.Validate(zipCodes, _lookupModel.StateAbbreviation);
				case "homeowners":
					return HomeownersZipCodeValidator.Validate(zipCodes, _lookupModel.StateAbbreviation);
				case "mobilehome":
					return HomeownersZipCodeValidator.Validate(zipCodes, _lookupModel.StateAbbreviation);
				case "mobile home":
					return HomeownersZipCodeValidator.Validate(zipCodes, _lookupModel.StateAbbreviation);
				case "renters":
					return RentersZipCodeValidator.Validate(zipCodes, _lookupModel.StateAbbreviation);
				case "umbrella":
					return UmbrellaZipCodeValidator.Validate(zipCodes, _lookupModel.StateAbbreviation);
				case "flood":
					return FloodZipCodeValidator.Validate(zipCodes, _lookupModel.StateAbbreviation);
				case "watercraft":
					return WatercraftZipCodeValidator.Validate(zipCodes, _lookupModel.StateAbbreviation);
				case "motorhome":
					return MotorhomeZipCodeValidator.Validate(zipCodes, _lookupModel.StateAbbreviation);
				case "motorcycle":
					return MotorcycleZipCodeValidator.Validate(zipCodes, _lookupModel.StateAbbreviation);
				case "travel trailer":
					return MotorhomeZipCodeValidator.Validate(zipCodes, _lookupModel.StateAbbreviation); 
				case "business":
					return BusinessZipCodeValidator.Validate(zipCodes, _lookupModel.StateAbbreviation);
				case "collector vehicle":
					return CollectorVehicleZipCodeValidator.Validate(zipCodes, _lookupModel.StateAbbreviation);
				case "homenonowners":
					return HomenonownersZipCodeValidator.Validate(zipCodes, _lookupModel.StateAbbreviation);
				case "condo":
					return CondoZipCodeValidator.Validate(zipCodes, _lookupModel.StateAbbreviation);
				default:
					return false;
			}
        }

        private bool ValidateCounty()
        {
            IEnumerable<QuoteCountyFilter> counties = _countyFilterRepository.GetAll();
			switch (_postModel.Type.ToLowerInvariant())
			{
				case "auto":
                    return true; //AutoCountyValidator.Validate(counties, _lookupModel.County, _lookupModel.StateAbbreviation);
				case "homeowners":
					return HomeownersCountyValidator.Validate(counties, _lookupModel.County, _lookupModel.StateAbbreviation);
				case "mobilehome":
					return true;
				case "renters":
					return RentersCountyValidator.Validate(counties, _lookupModel.County, _lookupModel.StateAbbreviation);
				case "umbrella":
                    return true; //UmbrellaCountyValidator.Validate(counties, _lookupModel.County, _lookupModel.StateAbbreviation);
				case "flood":
                    return true; //FloodCountyValidator.Validate(counties, _lookupModel.County, _lookupModel.StateAbbreviation);
				case "watercraft":
                    return true; //WatercraftCountyValidator.Validate(counties, _lookupModel.County, _lookupModel.StateAbbreviation);
				case "motorhome":
                    return true; //MotorhomeCountyValidator.Validate(counties, _lookupModel.County, _lookupModel.StateAbbreviation);
				case "motorcycle":
                    return true; //MotorcycleCountyValidator.Validate(counties, _lookupModel.County, _lookupModel.StateAbbreviation);
				case "travel trailer":
                    return true;// MotorhomeCountyValidator.Validate(counties, _lookupModel.County, _lookupModel.StateAbbreviation);
				case "mobile home":
                    return true; //HomeownersCountyValidator.Validate(counties, _lookupModel.County, _lookupModel.StateAbbreviation);
				case "business":
                    return true; //BusinessCountyValidator.Validate(counties, _lookupModel.County, _lookupModel.StateAbbreviation);
				case "collector vehicle":
                    return true; //CollectorVehicleCountyValidator.Validate(counties, _lookupModel.County, _lookupModel.StateAbbreviation);
				case "homenonowners":
					return true; //CollectorVehicleCountyValidator.Validate(counties, _lookupModel.County, _lookupModel.StateAbbreviation);
				case "condo":
					return true; //CollectorVehicleCountyValidator.Validate(counties, _lookupModel.County, _lookupModel.StateAbbreviation);
				default:
					return false;
			}
        }
    }

    public interface IProductFormHandler
    {
        ProductAvailabilityResponse GetProductAvailability(ProductAvailabilityPostModel productValidationModel);
    }
}