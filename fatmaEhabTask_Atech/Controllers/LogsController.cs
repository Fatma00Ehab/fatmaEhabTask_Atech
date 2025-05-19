using fatmaEhabTask_Atech.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace fatmaEhabTask_Atech.Controllers
{
    [ApiController]
    [Route("api/logs")]
    public class LogsController : ControllerBase
    {
        private readonly ILogRepository _logRepo;

        public LogsController(ILogRepository logRepo)
        {
            _logRepo = logRepo;
        }

        [HttpGet("blocked-attempts")]
        public IActionResult GetLogs(int page = 1, int pageSize = 10)
        {
            var logs = _logRepo.GetAll()
                        .Skip((page - 1) * pageSize)
                        .Take(pageSize);
            return Ok(logs);
        }

    }
}
