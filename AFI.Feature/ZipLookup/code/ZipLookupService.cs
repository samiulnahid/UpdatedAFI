using AFI.Feature.ZipLookup.Models;

using SmartyStreets;
using System;
using System.Configuration;
using System.Linq;

namespace AFI.Feature.ZipLookup
{
	public interface IZipLookupService
	{
		ZipLookupModel LookupZipCode(string zipcode);
	}


	public class ZipLookupService : IZipLookupService
    {
		private readonly string _authId;
		private readonly string _authToken;

		public ZipLookupService()
		{
			_authId = ConfigurationManager.AppSettings["SMARTYSTREET_AUTHID"];
			_authToken = ConfigurationManager.AppSettings["SMARTYSTREET_AUTHTOKEN"]; 
			ValidateConfig();
		}

		public ZipLookupService(string authId, string authToken)
		{
			_authId = authId;
			_authToken = authToken;
			ValidateConfig();
		}

		private void ValidateConfig()
		{
			if (string.IsNullOrWhiteSpace(_authId))
			{
				throw new ArgumentException("Service Auth ID is not set.");
			}

			if (string.IsNullOrWhiteSpace(_authToken))
			{
				throw new ArgumentException("Service Auth Token is not set.");
			}
		}

		public ZipLookupModel LookupZipCode(string zipcode)
		{

			if (string.IsNullOrWhiteSpace(zipcode))
			{
				throw new ArgumentNullException("zipcode");
			}

			try
			{
				var client = new ClientBuilder(_authId, _authToken).BuildUsZipCodeApiClient();


				var lookup = new SmartyStreets.USZipCodeApi.Lookup
				{
					ZipCode = zipcode
				};

				client.Send(lookup);

				var response = lookup.Result;

				 
				return new ZipLookupModel()
				{
					ZipCode = response.ZipCodes.FirstOrDefault()?.ZipCode,
					City = response.ZipCodes.FirstOrDefault()?.DefaultCity,
					State = response.ZipCodes.FirstOrDefault()?.State,
					StateAbbreviation = response.ZipCodes.FirstOrDefault()?.StateAbbreviation,
					County = response.ZipCodes.FirstOrDefault()?.CountyName
				};
			}
			catch (Exception ex)
			{
			//	Log.Info("SmartyStreets: " + ex.Message, "SmartyStreets");
				//TODO: Add Logging here
			}

			//Default return
			return new ZipLookupModel();
		}
	}
}
