using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using TECin2.API.DTOs;
using TECin2.API.Services;

namespace TECin2.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController(ILoginService loginService) : Controller
    {
        private readonly ILoginService _loginService = loginService;

        [AllowAnonymous]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> Login([FromBody] LogInRequest login)
        {
            try
            {
                LogInResponse? response = await _loginService.Login(login);
                if (response == null)
                {
                    return NotFound("Username or password is incorrect");
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
