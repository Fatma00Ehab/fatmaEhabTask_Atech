using fatmaEhabTask_Atech.Models;
using fatmaEhabTask_Atech.Repositories.Interfaces;
using fatmaEhabTask_Atech.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace fatmaEhabTask_Atech.Controllers
{
    [ApiController]
    [Route("api/ip")]
    public class IpController : ControllerBase
    {
        private readonly IGeoLocationService _geoService;
        private readonly ICountryRepository _countryRepo;
        private readonly ILogRepository _logRepo;

        public IpController(IGeoLocationService geoService, ICountryRepository countryRepo, ILogRepository logRepo)
        {
            _geoService = geoService;
            _countryRepo = countryRepo;
            _logRepo = logRepo;
        }

        [HttpGet("lookup")]
        public async Task<IActionResult> Lookup([FromQuery] string? ipAddress)
        {
            var ip = string.IsNullOrWhiteSpace(ipAddress) ? HttpContext.Connection.RemoteIpAddress?.ToString() : ipAddress;
            if (string.IsNullOrWhiteSpace(ip)) return BadRequest("Invalid IP.");

             



            var info = await _geoService.GetFullDetails(ip);
            if (info == null)
                return BadRequest("Could not retrieve IP info.");

            return Ok(new
            {
                ip = info.Value.ip,
                countryCode = info.Value.countryCode,
                countryName = info.Value.countryName,
                isp = info.Value.isp
            });

        }


        [HttpGet("check-block")]
        public async Task<IActionResult> CheckBlock()
        {
            var ip = HttpContext.Connection.RemoteIpAddress?.ToString();
            if (string.IsNullOrEmpty(ip))
                return BadRequest("Unable to determine client IP.");

            var userAgent = Request.Headers["User-Agent"].ToString();
            var countryCode = await _geoService.GetCountryCodeFromIp(ip) ?? "N/A";

            var isBlocked = _countryRepo.IsBlocked(countryCode);

             
            _logRepo.Add(new BlockedAttemptLog
            {
                IpAddress = ip,
                Timestamp = DateTime.UtcNow,
                CountryCode = countryCode,
                IsBlocked = isBlocked,
                UserAgent = userAgent
            });

            return Ok(new { IP = ip, Country = countryCode, Blocked = isBlocked });
        }

    }
}
