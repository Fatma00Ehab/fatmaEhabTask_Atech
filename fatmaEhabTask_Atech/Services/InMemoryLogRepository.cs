using fatmaEhabTask_Atech.Models;
using fatmaEhabTask_Atech.Services.Interfaces;
using System.Collections.Concurrent;

namespace fatmaEhabTask_Atech.Services
{

    public class InMemoryLogRepository : ILogRepository
    {
        private readonly ConcurrentBag<BlockedAttemptLog> _logs = new();

        public void Add(BlockedAttemptLog log)
        {
            _logs.Add(log);
        }

        public IEnumerable<BlockedAttemptLog> GetAll()
        {
             
            return _logs.OrderByDescending(log => log.Timestamp);
        }
    }
}
