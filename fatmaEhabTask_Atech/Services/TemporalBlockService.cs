using fatmaEhabTask_Atech.Repositories.Interfaces;
using fatmaEhabTask_Atech.Services.Interfaces;

namespace fatmaEhabTask_Atech.Services
{
    public class TemporalBlockService : ITemporalBlockService
    {
        private readonly ICountryRepository _countryRepository;

        public TemporalBlockService(ICountryRepository countryRepository)
        {
            _countryRepository = countryRepository;
        }

        public void AddTemporalBlock(string countryCode, int durationMinutes)
        {
            var expiration = DateTime.UtcNow.AddMinutes(durationMinutes);
            _countryRepository.AddTemporalBlock(countryCode, expiration);
        }

        public bool IsTemporarilyBlocked(string countryCode)
            => _countryRepository.IsTemporarilyBlocked(countryCode);
    }
}
