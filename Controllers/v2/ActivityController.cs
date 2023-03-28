using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServiceReportAPI.Contracts;
using ServiceReportAPI.Models;

namespace ServiceReportAPI.Controllers.v2
{
    [Route("api/[controller]")]
    [ApiController]
    public class ActivityController : ControllerBase
    {
        private readonly IActivityRepository _repository;

        public ActivityController(IActivityRepository repository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        }

        // GET api/<ActivityController>/5
        [HttpGet("{UserId}")]
        public async Task<IActionResult> Get(long UserId)
        {
            try
            {
                var result = await _repository.GetTodaysActivity(UserId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }

        [HttpGet("{UserId}/{Date}")]
        public async Task<IActionResult> Get(long UserId, DateTime Date)
        {
            try
            {
                var result = await _repository.GetActivityByDate(UserId, Date);
                return Ok(result);
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }

        // POST api/<ActivityController>
        [HttpPost]
        public async Task<IActionResult> Post(Activity activity)
        {
            try
            {
                var result = await _repository.CreateActivity(activity);
                return Ok(result);
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }

        // PUT api/<ActivityController>/5
        [HttpPut]
        public async Task<IActionResult> Put(Activity activity)
        {
            try
            {
                var result = await _repository.UpdateActivity(activity);
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
