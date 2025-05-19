using fatmaEhabTask_Atech.Models;
using fatmaEhabTask_Atech.Repositories.Interfaces;
using fatmaEhabTask_Atech.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace fatmaEhabTask_Atech.Controllers
{
    [ApiController]
    [Route("api/countries")]
    public class CountriesController : ControllerBase
    {
        private readonly ICountryRepository _repository;
        private readonly ITemporalBlockService _temporalBlockService;
        private readonly IGeoLocationService _geoService;

        public CountriesController(ICountryRepository repository, ITemporalBlockService temporalBlockService, IGeoLocationService geoService)
        {
            _repository = repository;
            _temporalBlockService = temporalBlockService;
            _geoService = geoService;
        }


        [HttpPost("block")]
        public async Task<IActionResult> BlockCountry([FromBody] string code)
        {
            var info = await _geoService.CountryInfoByCode(code);
            if (info == null)
                return BadRequest("Invalid country code.");

            var (countryCode, countryName) = info.Value;

            var result = _repository.BlockCountry(countryCode, countryName);
            if (!result)
                return BadRequest("Already blocked.");

            return Ok($"Blocked {countryCode} ({countryName})");


        }




        [HttpDelete("block/{code}")]
        public IActionResult UnblockCountry(string code)
        {
            if (_repository.UnblockCountry(code))
                return Ok($"Unblocked {code}");
            return NotFound("Not found.");
        }

        [HttpGet("blocked")]
        public IActionResult GetBlockedCountries([FromQuery] string? search, int page = 1, int pageSize = 10)
        {
            var filtered = _repository.FilterBlocked(search);
            var paginated = filtered.Skip((page - 1) * pageSize).Take(pageSize);
            return Ok(paginated);
        }


        [HttpPost("temporal-block")]
        public async Task<IActionResult> TemporalBlock([FromBody] TemporalBlockRequest req)
        {
            if (string.IsNullOrWhiteSpace(req.CountryCode) || req.DurationMinutes is < 1 or > 1440)
                return BadRequest("Invalid input.");

          
            var countryInfo = await _geoService.CountryInfoByCode(req.CountryCode);
            if (countryInfo == null)
                return BadRequest("Invalid country code.");

            if (_repository.IsTemporarilyBlocked(req.CountryCode))
                return Conflict("Country already temporarily blocked.");

            _temporalBlockService.AddTemporalBlock(req.CountryCode.ToUpper(), req.DurationMinutes);
            return Ok($"Temporarily blocked {req.CountryCode.ToUpper()} for {req.DurationMinutes} minutes.");
        }

    }
}
