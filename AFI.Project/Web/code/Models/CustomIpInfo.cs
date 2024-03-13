using Newtonsoft.Json;

namespace AFI.Project.Web.Models
{
    public class CustomIpInfo
    {
        [JsonProperty("ip")]
        public string Ip { get; set; }

        [JsonProperty("hostname")]
        public string Hostname { get; set; }

        [JsonProperty("city")]
        public string City { get; set; }

        [JsonProperty("region")]
        public string Region { get; set; }

        [JsonProperty("country_name")]
        public string Country { get; set; }

        [JsonProperty("country_code2")]
        public string Country_code2 { get; set; }

        [JsonProperty("state_prov")]
        public string State_prov { get; set; }

        [JsonProperty("latitude")]
        public string Latitude { get; set; }

        [JsonProperty("longitude")]
        public string Longitude { get; set; }

        [JsonProperty("continent_name")]
        public string Loc { get; set; }

        [JsonProperty("org")]
        public string Org { get; set; }

        [JsonProperty("zipcode")]
        public string Postal { get; set; }
    }
}