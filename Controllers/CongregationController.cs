using Microsoft.AspNetCore.Mvc;
using ServiceReportAPI.Contracts;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ServiceReportAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CongregationController : ControllerBase
    {
        private readonly ICongregationsRepository _repository;

        public CongregationController(ICongregationsRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        // GET: api/<CongregationController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var companies = await _repository.GetCongregations();
                return Ok(companies);
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }

        // GET api/<CongregationController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        // POST api/<CongregationController>
        [HttpPost]
        public void Post([FromBody] string value)
        {
        }

        // PUT api/<CongregationController>/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/<CongregationController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
