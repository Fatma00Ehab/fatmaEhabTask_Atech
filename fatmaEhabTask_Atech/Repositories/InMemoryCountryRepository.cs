using fatmaEhabTask_Atech.Models;
using fatmaEhabTask_Atech.Repositories.Interfaces;
using System.Collections.Concurrent;

namespace fatmaEhabTask_Atech.Repositories
{
    public class InMemoryCountryRepository : ICountryRepository
    {
        private readonly ConcurrentDictionary<string, BlockedCountry> _blockedCountries = new();
        private readonly ConcurrentDictionary<string, DateTime> _temporalBlocks = new();

        public bool BlockCountry(string countryCode, string? countryName)
            => _blockedCountries.TryAdd(countryCode.ToUpper(), new BlockedCountry { CountryCode = countryCode.ToUpper(), CountryName = countryName, BlockedAt = DateTime.UtcNow });

        public bool UnblockCountry(string countryCode)
            => _blockedCountries.TryRemove(countryCode.ToUpper(), out _);

        public IEnumerable<BlockedCountry> GetAllBlocked() => _blockedCountries.Values;

        public IEnumerable<BlockedCountry> FilterBlocked(string? searchTerm)
        {
            if (string.IsNullOrEmpty(searchTerm)) return GetAllBlocked();

            searchTerm = searchTerm.ToUpper();
            return _blockedCountries.Values.Where(c =>
                c.CountryCode.Contains(searchTerm) || (c.CountryName != null && c.CountryName.Contains(searchTerm, StringComparison.OrdinalIgnoreCase)));
        }

        public bool IsBlocked(string countryCode)
            => _blockedCountries.ContainsKey(countryCode.ToUpper());

        public void AddTemporalBlock(string countryCode, DateTime expiration)
        {
            _temporalBlocks.TryAdd(countryCode.ToUpper(), expiration);
        }

        public void RemoveExpiredTemporalBlocks()
        {
            var now = DateTime.UtcNow;
            foreach (var entry in _temporalBlocks)
            {
                if (entry.Value <= now)
                {
                    _temporalBlocks.TryRemove(entry.Key, out _);
                    _blockedCountries.TryRemove(entry.Key, out _);
                }
            }
        }

        public bool IsTemporarilyBlocked(string countryCode)
            => _temporalBlocks.ContainsKey(countryCode.ToUpper());
    }
}
