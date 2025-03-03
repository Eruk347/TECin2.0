using Microsoft.AspNetCore.Mvc;
using TECin2.API.DTOs;
using TECin2.API.Services;

namespace TECin2.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SettingController(ISettingService settingService) : Controller
    {
        private readonly ISettingService _settingService = settingService;

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                List<SettingResponse?> settings = await _settingService.GetAllSetting();
                if (settings == null)
                {
                    return Problem("Got no list; NULL");
                }
                if (settings.Count == 0)
                {
                    return NoContent();
                }

                return Ok(settings);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpGet("{settingId}")] //https://localhost:5001/api/author/1 - 1 bliver sat ind i linjen i stedet for userId
        //[Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]//bliver håndteret på et højere niveau, pga [FromRoute]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetById([FromRoute] int settingId)
        {
            try
            {
                SettingResponse? settingResponse = await _settingService.GetSettingById(settingId);
                if (settingResponse == null)
                {
                    return NotFound();
                }

                return Ok(settingResponse);
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
        public async Task<IActionResult> Create([FromBody] SettingRequest newSetting)
        {
            try
            {
                var accesstoken = Request.Headers.Authorization.ToString().Replace("bearer ", "");
                SettingResponse? settingResponse = await _settingService.CreateSetting(newSetting);
                if (settingResponse == null)
                {
                    return BadRequest();
                }

                return Ok(settingResponse);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpPut("{settingId}")]
        //[Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]//bliver håndteret på et højere niveau, pga [FromRoute]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update([FromRoute] int settingId, [FromBody] SettingRequest updateSetting)
        {
            try
            {
                var accesstoken = Request.Headers.Authorization.ToString().Replace("bearer ", "");
                SettingResponse? settingResponse = await _settingService.UpdateSetting(settingId, updateSetting);
                if (settingResponse == null)
                {
                    return NotFound();
                }

                return Ok(settingResponse);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpDelete("{settingId}")]
        // [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]//bliver håndteret på et højere niveau, pga [FromRoute]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete([FromRoute] int settingId)
        {
            try
            {
                var accesstoken = Request.Headers.Authorization.ToString().Replace("bearer ", "");
                SettingResponse? settingResponse = await _settingService.DeleteSetting(settingId);
                if (settingResponse == null)
                {
                    return NotFound();
                }

                return Ok(settingResponse);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
    }
}