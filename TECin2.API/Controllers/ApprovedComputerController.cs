using Microsoft.AspNetCore.Mvc;
using TECin2.API.DTOs;
using TECin2.API.Services;

namespace TECin2.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ApprovedComputerController(IApprovedComputersService approvedComputersService) : Controller
    {
        private readonly IApprovedComputersService _approvedComputersService = approvedComputersService;

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                List<ApprovedComputerResponse?> computers = await _approvedComputersService.GetAllApprovedComputers();
                if (computers == null)
                {
                    return Problem("Got no list; NULL");
                }
                if (computers.Count == 0)
                {
                    return NoContent();
                }
                return Ok(computers);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpGet("{approvedComputerId}")] //https://localhost:5001/api/author/1 - 1 bliver sat ind i linjen i stedet for userId
        //[Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]//bliver håndteret på et højere niveau, pga [FromRoute]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetById([FromRoute] int approvedComputerId)
        {
            try
            {
                ApprovedComputerResponse? computerResponse = await _approvedComputersService.GetApprovedComputerById(approvedComputerId);
                if (computerResponse == null)
                {
                    return NotFound();
                }
                return Ok(computerResponse);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] ApprovedComputerRequest newApprovedComputer)
        {
            try
            {
                ApprovedComputerResponse? computerResponse = await _approvedComputersService.CreateApprovedComputer(newApprovedComputer, "accessToken");
                if (computerResponse == null)
                {
                    return BadRequest();
                }
                return Ok(computerResponse);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpPut("{approvedComputerId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update([FromRoute] int approvedComputerId, [FromBody] ApprovedComputerRequest newApprovedComputer)
        {
            try
            {
                ApprovedComputerResponse? computerResponse = await _approvedComputersService.UpdateApprovedComputer(approvedComputerId, newApprovedComputer, "accessToken");
                if (computerResponse == null)
                {
                    return NotFound();
                }
                return Ok(computerResponse);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpDelete("{approvedComputerId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete([FromRoute] int approvedComputerId)
        {
            try
            {
                ApprovedComputerResponse? computerResponse = await _approvedComputersService.DeleteApprovedComputer(approvedComputerId, "accessToken");
                if (computerResponse == null)
                {
                    return NotFound();
                }
                return Ok(computerResponse);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
    }
}
