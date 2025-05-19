using fatmaEhabTask_Atech.Models;

namespace fatmaEhabTask_Atech.Services.Interfaces
{
    public interface ILogRepository
    {
        void Add(BlockedAttemptLog log);
        IEnumerable<BlockedAttemptLog> GetAll();
    }
}
