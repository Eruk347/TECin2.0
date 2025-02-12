using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TECin2.API.DTOs;
using TECin2.API.Services;

namespace TECin2.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GroupController(IGroupService groupService) : Controller
    {
        private readonly IGroupService _groupService = groupService;

        [HttpGet]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetAll()
        {
            try
            {
                List<GroupResponse?> groups = await _groupService.GetAllGroups();

                if (groups == null)
                {
                    return Problem("Got no list; NULL");
                }
                if (groups.Count == 0)
                {
                    return NoContent();
                }
                return Ok(groups);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpGet("{groupId}")] //https://localhost:5001/api/author/1 - 1 bliver sat ind i linjen i stedet for userId
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]//bliver håndteret på et højere niveau, pga [FromRoute]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetById([FromRoute] int groupId)
        {
            try
            {
                GroupResponse? groupResponse = await _groupService.GetGroupById(groupId);

                if (groupResponse == null)
                {
                    return NotFound();
                }

                return Ok(groupResponse);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpPost]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Create([FromBody] GroupRequest newGroup)
        {
            try
            {
                var accesstoken = Request.Headers.Authorization.ToString().Replace("bearer ", "");
                GroupResponse? groupResponse = await _groupService.CreateGroup(newGroup, accesstoken);

                if (groupResponse == null)
                {
                    return BadRequest();
                }

                return Ok(groupResponse);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpPut("{groupId}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]//bliver håndteret på et højere niveau, pga [FromRoute]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Update([FromRoute] int groupId, [FromBody] GroupRequest updateGroup)
        {
            try
            {
                var accesstoken = Request.Headers.Authorization.ToString().Replace("bearer ", "");
                GroupResponse? groupResponse = await _groupService.UpdateGroup(groupId, updateGroup, accesstoken);

                if (groupResponse == null)
                {
                    return NotFound();
                }

                return Ok(groupResponse);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }

        [HttpDelete("{groupId}")]
        [Authorize]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]//bliver håndteret på et højere niveau, pga [FromRoute]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Delete([FromRoute] string groupId)
        {
            try
            {
                var accesstoken = Request.Headers.Authorization.ToString().Replace("bearer ", "");
                string[] groupIdSplit = groupId.Split(',');
                int[] groupIds = [Convert.ToInt32(groupIdSplit[0]), Convert.ToInt32(groupIdSplit[1])];

                GroupResponse? groupResponse = await _groupService.DeleteGroup(groupIds[0], groupIds[1], accesstoken);

                if (groupResponse == null)
                {
                    return NotFound();
                }

                return Ok(groupResponse);
            }
            catch (Exception ex)
            {
                return Problem(ex.Message);
            }
        }
    }
}
