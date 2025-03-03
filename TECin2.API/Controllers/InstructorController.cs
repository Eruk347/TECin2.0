using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TECin2.API.DTOs;
using TECin2.API.Services;

namespace TECin2.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class InstructorController(IInstructorService instructorService) : Controller
    {
        private readonly IInstructorService _instructorService = instructorService;

        [HttpGet]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                List<InstructorResponse?> instructors = await _instructorService.GetAllInstructors();
                if (instructors == null)
                {
                    return Problem("Got no list; NULL");
                }
                if (instructors.Count == 0)
                {
                    return NoContent();
                }
                return Ok(instructors);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpGet("{instructorId}")] //https://localhost:5001/api/author/1 - 1 bliver sat ind i linjen i stedet for userId
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]//bliver håndteret på et højere niveau, pga [FromRoute]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetById([FromRoute] string instructorId)
        {
            try
            {
                InstructorResponse? instructorResponse = await _instructorService.GetInstruktoById(instructorId);

                if (instructorResponse == null)
                {
                    return NotFound();
                }
                return Ok(instructorResponse);
            }
            catch (System.Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpPost]
        //[Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] InstructorRequest newInstructor)
        {
            try
            {
                var accesstoken = Request.Headers.Authorization.ToString().Replace("bearer ", "");
                InstructorResponse? instructorResponse = await _instructorService.CreateInstructor(newInstructor, accesstoken);
                if (instructorResponse == null)
                {
                    return BadRequest();
                }
                return Ok(instructorResponse);
            }
            catch (System.Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpPut("{instructorId}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]//bliver håndteret på et højere niveau, pga [FromRoute]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update([FromRoute] string instructorId, [FromBody] InstructorRequest newInstructor)
        {
            try
            {
                var accesstoken = Request.Headers.Authorization.ToString().Replace("bearer ", "");
                InstructorResponse? instructorResponse = await _instructorService.UpdateInstructor(instructorId, newInstructor, accesstoken);
                if (instructorResponse == null)
                {
                    return NotFound();
                }
                return Ok(instructorResponse);
            }
            catch (System.Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpDelete("{instructorId}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]//bliver håndteret på et højere niveau, pga [FromRoute]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete([FromRoute] string instructorId)
        {
            try
            {
                var accesstoken = Request.Headers.Authorization.ToString().Replace("bearer ", "");
                InstructorResponse? instructorResponse = await _instructorService.DeleteInstructor(instructorId, accesstoken);
                if (instructorResponse == null)
                {
                    return NotFound();
                }
                return Ok(instructorResponse);
            }
            catch (System.Exception ex)
            {
                return Problem(ex.Message);
            }
        }
    }
}