using Microsoft.AspNetCore.Mvc;
using TECin2.API.DTOs;
using TECin2.API.Services;

namespace TECin2.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ComputerLocationController(IComputerLocationService computerLocationService) : Controller
    {
        private readonly IComputerLocationService _computerLocationService = computerLocationService;
        
        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                List<ComputerLocationResponse?> computerLocations = await _computerLocationService.GetAllComputerLocations();
                if (computerLocations == null)
                {
                    return Problem("Got no list; NULL");
                }
                if (computerLocations.Count == 0)
                {
                    return NoContent();
                }
                return Ok(computerLocations);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpGet("{computerLocationId}")] //https://localhost:5001/api/author/1 - 1 bliver sat ind i linjen i stedet for userId
        //[Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]//bliver håndteret på et højere niveau, pga [FromRoute]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetById([FromRoute] int computerLocationId)
        {
            try
            {
                ComputerLocationResponse? computerLocationResponse = await _computerLocationService.GetComputerLocationById(computerLocationId);
                if (computerLocationResponse == null)
                {
                    return NotFound();
                }
                return Ok(computerLocationResponse);
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
        public async Task<IActionResult> Create([FromBody] ComputerLocationRequest newComputerLocation)
        {
            try
            {
                var accesstoken = Request.Headers.Authorization.ToString().Replace("bearer ", "");
                ComputerLocationResponse? computerLocationResponse = await _computerLocationService.CreateComputerLocation(newComputerLocation, accesstoken);
                if (computerLocationResponse == null)
                {
                    return BadRequest();
                }
                return Ok(computerLocationResponse);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpPut("{computerLocationId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update([FromRoute] int computerLocationId, [FromBody] ComputerLocationRequest updateComputerLocation)
        {
            try
            {
                var accesstoken = Request.Headers.Authorization.ToString().Replace("bearer ", "");
                ComputerLocationResponse? computerLocationResponse = await _computerLocationService.UpdateComputerLocation(computerLocationId, updateComputerLocation, accesstoken);
                if (computerLocationResponse == null)
                {
                    return NotFound();
                }
                return Ok(computerLocationResponse);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpDelete("{computerLocationId}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete([FromRoute] int computerLocationId)
        {
            try
            {
                var accesstoken = Request.Headers.Authorization.ToString().Replace("bearer ", "");
                ComputerLocationResponse? computerLocationResponse = await _computerLocationService.DeleteComputerLocation(computerLocationId, accesstoken);
                if (computerLocationResponse == null)
                {
                    return NotFound();
                }
                return Ok(computerLocationResponse);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
    }
}
