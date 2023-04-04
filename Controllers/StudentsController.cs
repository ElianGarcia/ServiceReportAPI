using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServiceReportAPI.Contracts;
using ServiceReportAPI.Models;

namespace ServiceReportAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentsRepository _repository;
        private readonly IReturnVisitsRepository _visitsRepository;

        public StudentsController(IStudentsRepository repository, IReturnVisitsRepository visitsRepository)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _visitsRepository = visitsRepository ?? throw new ArgumentNullException(nameof(visitsRepository));
        }

        // GET: api/<StudentsController>/5
        [HttpGet("{UserId}")]
        public async Task<IActionResult> Get(long UserId)
        {
            try
            {
                var result = await _repository.GetStudents(UserId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }
        
        [HttpGet("{StudentId}/{param}")]
        public async Task<IActionResult> GetStudent(long StudentId, short param)
        {
            try
            {
                var result = await _repository.GetStudent(StudentId);
                return Ok(result);
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message + param);
            }
        }

        // POST api/<StudentsController>
        [HttpPost]
        public async Task<IActionResult> Post(Student student)
        {
            try
            {
                var result = await _repository.CreateStudent(student);
                return Ok(result);
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }

        [Route("Visit")]
        [HttpPost]
        public async Task<IActionResult> Visit(ReturnVisit visit)
        {
            try
            {
                var result = await _visitsRepository.CreateReturnVisit(visit);
                return Ok(result);
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }

        // PUT api/<StudentsController>/5
        [HttpPut]
        public async Task<IActionResult> Put(Student student)
        {
            try
            {
                var result = await _repository.UpdateStudent(student);
                return Ok(result);
            }
            catch (Exception ex)
            {
                //log error
                return StatusCode(500, ex.Message);
            }
        }

        // DELETE api/<StudentsController>/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(long id)
        {
            try
            {
                var result = await _repository.DeleteStudent(id);
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
