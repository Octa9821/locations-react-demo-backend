using LocationsAPI.Models;
using LocationsAPI.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace LocationsAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationService _authenticationService;
        private readonly ITokenService _tokenService;

        public AuthenticationController(IAuthenticationService authenticationService, ITokenService tokenService)
        {
            _authenticationService = authenticationService;
            _tokenService = tokenService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel loginModel)
        {
            var user = await _authenticationService.GetUserAsync(loginModel.Username, loginModel.Password);
            if (user == null)
            {
                return NotFound();
            }

            var token = _tokenService.CreateToken(user);
            if (token == null)
            {
                return BadRequest();
            }

            return Ok(token);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] LoginModel loginModel)
        {
            var result = await _authenticationService.CreateUserAsync(loginModel.Username, loginModel.Password);
            if (!result)
            {

                return BadRequest();
            }

            return Ok();
        }
    }
}
