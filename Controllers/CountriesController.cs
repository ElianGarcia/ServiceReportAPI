using Microsoft.AspNetCore.Mvc;
using ServiceReportAPI.Contracts;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ServiceReportAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CountriesController : ControllerBase
    {
        private readonly ICountriesRepository _repository;

        public CountriesController(ICountriesRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        // GET: api/<CountriesController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var countries = await _repository.GetCountries();
                return Ok(countries);
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }
    }
}
