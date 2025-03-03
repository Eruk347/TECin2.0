using Microsoft.AspNetCore.Mvc;
using TECin2.API.DTOs;
using TECin2.API.Services;

namespace TECin2.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class StudentController(IStudentService studentService) : Controller
    {
        private readonly IStudentService _studentService = studentService;

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                List<StudentResponse?> students = await _studentService.GetAllStudents();
                if (students == null)
                {
                    return Problem("Got no list; NULL");
                }
                if (students.Count == 0)
                {
                    return NoContent();
                }
                return Ok(students);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpGet("{studentId}")] //https://localhost:5001/api/author/1 - 1 bliver sat ind i linjen i stedet for userId
        //[Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]//bliver håndteret på et højere niveau, pga [FromRoute]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetById([FromRoute] string studentId)
        {
            try
            {
                StudentResponse? studentResponse = await _studentService.GetStudentById(studentId);
                if (studentResponse == null)
                {
                    return NotFound();
                }
                return Ok(studentResponse);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpPost]
        //[Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] StudentRequest newStudent)
        {
            try
            {
                var accesstoken = Request.Headers.Authorization.ToString().Replace("bearer ", "");
                StudentResponse? studentResponse = await _studentService.CreateStudent(newStudent, accesstoken);
                if (studentResponse == null)
                {
                    return BadRequest();
                }
                return Ok(studentResponse);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpPut("{studentId}")]
        //[Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]//bliver håndteret på et højere niveau, pga [FromRoute]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update([FromRoute] string studentId, [FromBody] StudentRequest updateStudent)
        {
            try
            {
                var accesstoken = Request.Headers.Authorization.ToString().Replace("bearer ", "");
                StudentResponse? studentResponse = await _studentService.UpdateStudent(studentId, updateStudent, accesstoken);
                if (studentResponse == null)
                {
                    return NotFound();
                }
                return Ok(studentResponse);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
        [HttpDelete("{studentId}")]
        // [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]//bliver håndteret på et højere niveau, pga [FromRoute]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete([FromRoute] string studentId)
        {
            try
            {
                var accesstoken = Request.Headers.Authorization.ToString().Replace("bearer ", "");
                StudentResponse? studentResponse = await _studentService.DeleteStudent(studentId, accesstoken);
                if (studentResponse == null)
                {
                    return NotFound();
                }
                return Ok(studentResponse);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
    }
}
