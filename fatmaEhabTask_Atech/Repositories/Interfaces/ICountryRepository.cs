using fatmaEhabTask_Atech.Models;

namespace fatmaEhabTask_Atech.Repositories.Interfaces
{
    public interface ICountryRepository
    {
        bool BlockCountry(string countryCode, string? countryName);
        bool UnblockCountry(string countryCode);
        IEnumerable<BlockedCountry> GetAllBlocked();
        IEnumerable<BlockedCountry> FilterBlocked(string? searchTerm);
        bool IsBlocked(string countryCode);
        void AddTemporalBlock(string countryCode, DateTime expiration);
        void RemoveExpiredTemporalBlocks();
        bool IsTemporarilyBlocked(string countryCode);
    }
}
