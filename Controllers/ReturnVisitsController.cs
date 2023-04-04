using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceReportAPI.Contracts;
using ServiceReportAPI.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ServiceReportAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class ReturnVisitsController : ControllerBase
    {
        private readonly IReturnVisitsRepository _repository;

        public ReturnVisitsController(IReturnVisitsRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        // GET: api/<ReturnVisitsController>
        [HttpGet("{id}")]
        public async Task<IActionResult> Get(long id)
        {
            try
            {
                var result = await _repository.GetReturnVisits(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }

        // POST api/<ReturnVisitsController>
        [HttpPost]
        public async Task<IActionResult> Post(ReturnVisit visit)
        {
            try
            {
                var result = await _repository.CreateReturnVisit(visit);
                return Ok(result);
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }

        // PUT api/<ReturnVisitsController>/5
        [HttpPut]
        public async Task<IActionResult> Put(ReturnVisit visit)
        {
            try
            {
                var result = await _repository.UpdateReturnVisit(visit);
                return Ok(result);
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }

        // DELETE api/<ReturnVisitsController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
