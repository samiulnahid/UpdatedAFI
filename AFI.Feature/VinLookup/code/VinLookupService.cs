using AFI.Feature.VinLookup.Models;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.ServiceModel;

namespace AFI.Feature.VinLookup
{
	public interface IVinLookupService
	{
		IEnumerable<int> GetYears();
		IDictionary<int, string> GetMakes(int year);
		IDictionary<int, string> GetModels(int year, int makeId);
		IDictionary<int, string> GetBodyStyles(int modelId);
		VehicleInformation GetVehicleInformationByVin(string vin);
	}

	public class VinLookupService : IVinLookupService
	{
		private readonly AFI.Feature.VinLookup.ChromeData.AccountInfo _accountInfo;
		private readonly string _endpointAddress;
		private readonly AFI.Feature.VinLookup.ChromeData.Description7aPortTypeClient _service;

		public VinLookupService() : this(ConfigurationManager.AppSettings["ChromeVINUser"], ConfigurationManager.AppSettings["ChromeVINPass"], ConfigurationManager.AppSettings["ChromeVINEndpoint"]) { }

		public VinLookupService(string userNumber, string passcode, string endpoint)
		{
			_accountInfo = new AFI.Feature.VinLookup.ChromeData.AccountInfo()
			{
				number = userNumber,
				secret = passcode,
				country = "US",
				language = "en",
				behalfOf = "ADS"
			};

			_endpointAddress = endpoint;
			_service = GenerateService();
		}

		public IEnumerable<int> GetYears()
		{
			try
			{
				var service = _service;
				var yearRequest = new AFI.Feature.VinLookup.ChromeData.getModelYearsRequest()
				{
					ModelYearsRequest = new AFI.Feature.VinLookup.ChromeData.BaseRequest { accountInfo = _accountInfo }
				};

				var years = service.getModelYears(yearRequest.ModelYearsRequest);

				return years.modelYear;

			}
			catch (Exception)
			{
				throw;
			}
		}

		public IDictionary<int, string> GetMakes(int year)
		{
			try
			{
				var service = _service;
				var makeRequest = new AFI.Feature.VinLookup.ChromeData.DivisionsRequest()
				{
					 accountInfo = _accountInfo,
					 modelYear = year
				};

				var makes = service.getDivisions(makeRequest);

				return makes.division.ToDictionary(d => d.id, d=> d.Value);

			}
			catch (Exception)
			{
				throw;
			}
		}

		public IDictionary<int, string> GetModels(int year, int makeId)
		{
			try
			{
				var service = _service;
				var modelRequest = new AFI.Feature.VinLookup.ChromeData.ModelsRequest()
				{
					accountInfo = _accountInfo,
					modelYear = year,
					Item = makeId,
					ItemElementName = AFI.Feature.VinLookup.ChromeData.ItemChoiceType.divisionId
				};

				var models = service.getModels(modelRequest);

				return models.model.ToDictionary(m => m.id, m => m.Value);

			}
			catch (Exception)
			{
				throw;
			}
		}

		public IDictionary<int, string> GetBodyStyles(int modelId)
		{
			try
			{
				var service = _service;
				var stylesRequest = new AFI.Feature.VinLookup.ChromeData.StylesRequest()
				{
					accountInfo = _accountInfo,
					modelId = modelId
				};

				var bodyStyles = service.getStyles(stylesRequest);

				var output = bodyStyles.style.ToDictionary(m => m.id, m => m.Value);

				if (output.Keys.Count() == 0)
				{
					output.Add(0, "No Body Style for Model");
				}

				return output;

			}
			catch (Exception)
			{
				throw;
			}
		}

		public VehicleInformation GetVehicleInformationByVin(string vin)
		{
			try
			{
				var service = _service;
				var vinRequest = new AFI.Feature.VinLookup.ChromeData.VehicleDescriptionRequest()
				{
					accountInfo = _accountInfo,
					Items = new[] { vin },
					ItemsElementName = new[] { AFI.Feature.VinLookup.ChromeData.ItemsChoiceType.vin }
				};

				var vehicleInfo = service.describeVehicle(vinRequest);

				var output = new VehicleInformation()
				{
					Vin = vin,
					Year = vehicleInfo.modelYear,
					Make = vehicleInfo.bestMakeName,
					Model = vehicleInfo.bestModelName,
					BodyStyle = vehicleInfo.bestStyleName
				};

				return output;

			}
			catch (Exception)
			{
				throw;
			}
		}

		private AFI.Feature.VinLookup.ChromeData.Description7aPortTypeClient GenerateService()
		{			
			var binding = new BasicHttpBinding(BasicHttpSecurityMode.None);
			var endpoint = new EndpointAddress(_endpointAddress);

			return new AFI.Feature.VinLookup.ChromeData.Description7aPortTypeClient(binding: binding, remoteAddress: endpoint);
		}
	}
}
