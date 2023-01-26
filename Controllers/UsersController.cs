using Microsoft.AspNetCore.Mvc;
using ServiceReportAPI.Contracts;
using ServiceReportAPI.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace ServiceReportAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly IUsersRepository _repository;
        private readonly IGoalsRepository _goalsRepository;

        public UsersController(IUsersRepository repository, IGoalsRepository goalsRepository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _goalsRepository = goalsRepository ?? throw new ArgumentNullException(nameof(goalsRepository));
        }

        // GET: api/<UsersController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            try
            {
                var result = await _repository.GetUsers();
                return Ok(result);
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }
        
        [HttpGet]
        [Route("Login")]
        public async Task<IActionResult> Get(User user)
        {
            try
            {
                var result = await _repository.GetUser(user);
                return Ok(result);
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }

        // POST api/<UsersController>
        [HttpPost]
        public async Task<IActionResult> Post([FromBody] User user)
        {
            var created = 0;

            try
            {
                var result = await _repository.CreateUser(user);

                if (result is not null)
                    created = await _goalsRepository.CreateGoal(new Goal(0, 10, 10, 10, (long)result.UserId));

                return Ok(created);
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }

        // PUT api/<UsersController>/5
        [HttpPut]
        public async Task<IActionResult> Put([FromBody] User user)
        {
            try
            {
                var result = await _repository.UpdateUser(user);
                return Ok(result);
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }

        // DELETE api/<UsersController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                var result = await _repository.DeleteUser(id);
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
