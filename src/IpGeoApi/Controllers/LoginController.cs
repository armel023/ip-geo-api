using IpGeoApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace IpGeoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : ControllerBase
    {
        private readonly AuthenticationService _authenticationService;

        public LoginController(AuthenticationService authenticationService)
        {
            _authenticationService = authenticationService;
        }

        [HttpPost]
        public async Task<IActionResult> Login([FromBody] DTOs.LoginRequest request)
        {
            var result = await _authenticationService.LoginAsync(request.Username, request.Password);
            if (!result.Success || result.Data == null)
                return Unauthorized(result.Error);
            return Ok(result.Data);
        }
    }
}
