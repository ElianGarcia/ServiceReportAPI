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
    public class GoalsController : ControllerBase
    {
        private readonly IGoalsRepository _repository;

        public GoalsController(IGoalsRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        // GET: api/<GoalsController>
        [HttpGet("{UserId}")]
        public async Task<IActionResult> Get(long UserId)
        {
            try
            {
                var result = await _repository.GetGoal(UserId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }

        [Route("getProgress/{id}")]
        [HttpGet]
        public async Task<IActionResult> GetProgress(long id)
        {
            try
            {
                var result = await _repository.GetProgress(id);
                return Ok(result);
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }

        // POST api/<GoalsController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] Goal goal)
        {
            try
            {
                var result = await _repository.CreateGoal(goal);
                return Ok(result);
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }

        // PUT api/<GoalsController>/5
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] Goal goal)
        {
            try
            {
                var result = await _repository.UpdateGoal(goal);
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
