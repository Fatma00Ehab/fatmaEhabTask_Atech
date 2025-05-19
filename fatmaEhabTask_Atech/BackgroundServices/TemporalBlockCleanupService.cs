using fatmaEhabTask_Atech.Repositories.Interfaces;

namespace fatmaEhabTask_Atech.BackgroundServices
{
    public class TemporalBlockCleanupService : BackgroundService
    {
        private readonly ICountryRepository _repository;
        private readonly ILogger<TemporalBlockCleanupService> _logger;

        public TemporalBlockCleanupService(ICountryRepository repository, ILogger<TemporalBlockCleanupService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                _repository.RemoveExpiredTemporalBlocks();
                _logger.LogInformation("Expired blocks cleaned.");
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }
    }
}
