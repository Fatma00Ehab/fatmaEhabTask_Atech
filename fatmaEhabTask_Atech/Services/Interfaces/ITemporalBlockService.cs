namespace fatmaEhabTask_Atech.Services.Interfaces
{
    public interface ITemporalBlockService
    {
        void AddTemporalBlock(string countryCode, int durationMinutes);
        bool IsTemporarilyBlocked(string countryCode);
    }
}
