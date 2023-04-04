using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceReportAPI.Contracts;
using ServiceReportAPI.Models;

namespace ServiceReportAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class SettingsController : ControllerBase
    {
        private readonly ISettingsRepository _repository;

        public SettingsController(ISettingsRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        // GET: api/<GoalsController>
        [HttpGet("{UserId}")]
        public async Task<IActionResult> Get(long UserId)
        {
            try
            {
                var result = await _repository.GetSettings(UserId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }

        [HttpPut]
        public async Task<IActionResult> Put(Settings settings)
        {
            try
            {
                var result = await _repository.UpdateSettings(settings);
                return Ok(result);
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }
    }
}
