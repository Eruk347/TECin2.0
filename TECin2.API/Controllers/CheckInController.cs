using Microsoft.AspNetCore.Mvc;
using TECin2.API.DTOs;
using TECin2.API.Services;

namespace TECin2.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CheckInController(ICheckInService checkInService) : Controller
    {
        private readonly ICheckInService _checkInService = checkInService;

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]//bliver håndteret på et højere niveau, pga [FromRoute]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CheckIn([FromBody] CheckInRequest checkIn)
        {
            try
            {
                CheckInResponse? checkinResponse = await _checkInService.CheckIn(checkIn);

                if (checkinResponse == null)
                {
                    return NotFound();
                }
                return Ok(checkinResponse);
            }
            catch (System.Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpGet("{information}")]
        //[Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]//bliver håndteret på et højere niveau, pga [FromRoute]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAll([FromRoute] string information)//information = "groupid,date(YYYYMMDD)"
        {
            try
            {
                string[] infoSplit = information.Split(',');

                int groupId = Convert.ToInt32(infoSplit[0]);

                string date = infoSplit[1];
                int year = Convert.ToInt32(date[..4]);
                int month = Convert.ToInt32(date.Substring(2, 2));
                int day = Convert.ToInt32(date.Substring(4, 2));

                DateOnly datePicked = new(year, month, day);

                List<CheckInResponseLong> response = await _checkInService.GetAllCheckInStatusesFromGroup(groupId, datePicked);

                if (response == null)
                {
                    return Problem("Got no list; NULL");
                }
                if (response.Count == 0)
                {
                    return NoContent();
                }
                return Ok(response);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }


    }
}