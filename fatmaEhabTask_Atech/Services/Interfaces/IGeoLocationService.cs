namespace fatmaEhabTask_Atech.Services.Interfaces
{
    public interface IGeoLocationService
    {
        Task<string?> GetCountryCodeFromIp(string ip);
        Task<(string countryCode, string countryName)?> CountryInfoByCode(string code);
        //Task<string?> GetCountryNameFromCode(string code);

        Task<(string ip, string countryCode, string countryName, string isp)?> GetFullDetails(string ip);

    }
}
