using IpGeoApi.Services;
using Microsoft.AspNetCore.Mvc;

namespace IpGeoApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly AuthenticationService _authService;

        public AuthenticationController(AuthenticationService authService)
        {
            _authService = authService;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] DTOs.LoginRequest request)
        {
            var result = await _authService.LoginAsync(request.Username, request.Password);
            if (!result.Success || result.Data == null)
                return Unauthorized(result.Error);
            return Ok(result.Data);
        }
    }
}
