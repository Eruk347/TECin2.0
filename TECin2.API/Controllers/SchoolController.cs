using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TECin2.API.DTOs;
using TECin2.API.Services;

namespace TECin2.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SchoolController(ISchoolService schoolService) : Controller
    {
        private readonly ISchoolService _schoolService = schoolService;

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                List<SchoolResponse?> schools = await _schoolService.GetAllSchools();

                if (schools == null)
                {
                    return Problem("Got no list; NULL");
                }
                if (schools.Count == 0)
                {
                    return NoContent();
                }

                return Ok(schools);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpGet("{schoolId}")] //https://localhost:5001/api/author/1 - 1 bliver sat ind i linjen i stedet for userId
        //[Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]//bliver håndteret på et højere niveau, pga [FromRoute]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetById([FromRoute] int schoolId)
        {
            try
            {
                SchoolResponse? schoolResponse = await _schoolService.GetSchoolById(schoolId);

                if (schoolResponse == null)
                {
                    return NotFound();
                }

                return Ok(schoolResponse);
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
        public async Task<IActionResult> Create([FromBody] SchoolRequest newSchool)
        {
            try
            {
                var accesstoken = Request.Headers.Authorization.ToString().Replace("bearer ", "");
                SchoolResponse? schoolResponse = await _schoolService.CreateSchool(newSchool, accesstoken);

                if (schoolResponse == null)
                {
                    return BadRequest();
                }

                return Ok(schoolResponse);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpPut("{schoolId}")]
        //[Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]//bliver håndteret på et højere niveau, pga [FromRoute]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update([FromRoute] int schoolId, [FromBody] SchoolRequest updateSchool)
        {
            try
            {
                var accesstoken = Request.Headers.Authorization.ToString().Replace("bearer ", "");
                SchoolResponse? schoolResponse = await _schoolService.UpdateSchool(schoolId, updateSchool, accesstoken);

                if (schoolResponse == null)
                {
                    return NotFound();
                }

                return Ok(schoolResponse);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpDelete("{schoolId}")]
        //[Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]//bliver håndteret på et højere niveau, pga [FromRoute]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete([FromRoute] int schoolId)
        {
            try
            {
                var accesstoken = Request.Headers.Authorization.ToString().Replace("bearer ", "");
                SchoolResponse? schoolResponse = await _schoolService.DeleteSchool(schoolId, accesstoken);

                if (schoolResponse == null)
                {
                    return NotFound();
                }

                return Ok(schoolResponse);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
    }
}
