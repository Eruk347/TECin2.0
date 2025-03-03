using Azure.Core;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TECin2.API.DTOs;
using TECin2.API.Services;

namespace TECin2.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class DepartmentController(IDepartmentService departmentService) : Controller
    {
        private readonly IDepartmentService _departmentService = departmentService;

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                List<DepartmentResponse?> departments = await _departmentService.GetAllDepartments();

                if (departments == null)
                {
                    return Problem("Got no list; NULL");
                }
                if (departments.Count == 0)
                {
                    return NoContent();
                }

                return Ok(departments);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpGet("{departmentId}")] //https://localhost:5001/api/author/1 - 1 bliver sat ind i linjen i stedet for userId
        //[Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]//bliver håndteret på et højere niveau, pga [FromRoute]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetById([FromRoute] int departmentId)
        {
            try
            {
                DepartmentResponse? departmentResponse = await _departmentService.GetDepartmentById(departmentId);

                if (departmentResponse == null)
                {
                    return NotFound();
                }

                return Ok(departmentResponse);
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
        public async Task<IActionResult> Create([FromBody] DepartmentRequest newDepartment)
        {
            try
            {
                var accesstoken = Request.Headers.Authorization.ToString().Replace("bearer ", "");
                DepartmentResponse? departmentResponse = await _departmentService.CreateDepartment(newDepartment, accesstoken);

                if (departmentResponse == null)
                {
                    return BadRequest();
                }

                return Ok(departmentResponse);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpPut("{departmentId}")]
        //[Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]//bliver håndteret på et højere niveau, pga [FromRoute]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update([FromRoute] int departmentId, [FromBody] DepartmentRequest updateDepartment)
        {
            try
            {
                var accesstoken = Request.Headers.Authorization.ToString().Replace("bearer ", "");
                DepartmentResponse? departmentResponse = await _departmentService.UpdateDepartment(departmentId, updateDepartment, accesstoken);

                if (departmentResponse == null)
                {
                    return NotFound();
                }

                return Ok(departmentResponse);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpDelete("{departmentId}")]
       // [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]//bliver håndteret på et højere niveau, pga [FromRoute]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete([FromRoute] int departmentId)
        {
            try
            {
                var accesstoken = Request.Headers.Authorization.ToString().Replace("bearer ", "");
                DepartmentResponse? departmentResponse = await _departmentService.DeleteDepartment(departmentId, accesstoken);

                if (departmentResponse == null)
                {
                    return NotFound();
                }

                return Ok(departmentResponse);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
    }
}