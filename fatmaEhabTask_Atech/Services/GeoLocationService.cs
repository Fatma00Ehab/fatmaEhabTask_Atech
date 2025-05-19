using fatmaEhabTask_Atech.Services.Interfaces;
using System.Text.Json.Serialization;

namespace fatmaEhabTask_Atech.Services
{
    public class GeoLocationService : IGeoLocationService
    {
        private readonly HttpClient _httpClient;
        private readonly string _apiKey;
        private readonly string _baseUrl;

        public GeoLocationService(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _baseUrl = config["GeoApi:BaseUrl"];
            _apiKey = config["GeoApi:ApiKey"];
        }

        public async Task<string?> GetCountryCodeFromIp(string ip)
        {
            var url = $"{_baseUrl}?apiKey={_apiKey}&ip={ip}";

            try
            {
                var response = await _httpClient.GetFromJsonAsync<GeolocationResponse>(url);
                return response?.Country_Code2;
            }
            catch (HttpRequestException)
            {
                return null;
            }
        }

        public class GeolocationResponse
        {
            [JsonPropertyName("country_code2")]
            public string? Country_Code2 { get; set; }
        }


        public async Task<(string?, string?)> GetCountryInfoByCode(string code)
        {
            var url = $"{_baseUrl}/{code}/json/?key={_apiKey}";
            var response = await _httpClient.GetFromJsonAsync<IpLookupResponse>(url);
            return (response?.Country, response?.Country_name);
        }

 

        public async Task<(string countryCode, string countryName)?> CountryInfoByCode(string code)
        {
            var url = $"https://restcountries.com/v3.1/alpha/{code}";

            try
            {
                var response = await _httpClient.GetFromJsonAsync<List<RestCountryResponse>>(url);
                var country = response?.FirstOrDefault();

                if (country?.Name?.Common == null)
                    return null;

                return (code.ToUpper(), country.Name.Common);
            }
            catch (HttpRequestException)
            {
                return null; 
            }
        }

        public async Task<(string ip, string countryCode, string countryName, string isp)?> GetFullDetails(string ip)
        {
            var url = $"{_baseUrl}?apiKey={_apiKey}&ip={ip}";

            try
            {
                var response = await _httpClient.GetFromJsonAsync<IpLookupResponse>(url);
                if (response == null || string.IsNullOrEmpty(response.Country))
                    return null;

                return (response.Ip ?? ip, response.Country!, response.Country_name ?? "Unknown", response.Isp ?? "Unknown");
            }
            catch (HttpRequestException)
            {
                return null;
            }
        }




        public class RestCountryResponse
        {
            public NameObject? Name { get; set; }
        }

        public class NameObject
        {
            public string? Common { get; set; }
        }


        public class IpLookupResponse
        {
            [JsonPropertyName("ip")]
            public string? Ip { get; set; }

            [JsonPropertyName("country_code2")]
            public string? Country { get; set; }   

            [JsonPropertyName("country_name")]
            public string? Country_name { get; set; }

            [JsonPropertyName("organization")]
            public string? Isp { get; set; }
        }
    }
}
