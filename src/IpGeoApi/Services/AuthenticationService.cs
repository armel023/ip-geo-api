using IpGeoApi.DTOs;
using IpGeoApi.Utilities;
using Microsoft.AspNetCore.Identity;

namespace IpGeoApi.Services;

public class AuthenticationService
{
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IConfiguration _config;

    public AuthenticationService(SignInManager<IdentityUser> signInManager, UserManager<IdentityUser> userManager, IConfiguration config)
    {
        _signInManager = signInManager;
        _userManager = userManager;
        _config = config;
    }

    public async Task LogoutAsync()
    {
        await _signInManager.SignOutAsync();
    }

    /// <summary> Authenticate user and generate JWT token </summary> 
    /// <param name="username">Username</param>
    /// <param name="password">Password</param>
    /// <returns>Result with LoginResponse containing JWT token if successful, or error message if failed</returns>
    public async Task<Result<LoginResponse>> LoginAsync(string username, string password)
    {
        var user = await _userManager.FindByNameAsync(username);
        if (user == null)
            return Result<LoginResponse>.Fail("Invalid username or password");

        var result = await _signInManager.CheckPasswordSignInAsync(user, password, false);
        if (!result.Succeeded)
            return Result<LoginResponse>.Fail("Invalid username or password");

        // Generate JWT token
        var accessToken = GenerateJwtToken(user);
        // Return token and username
        var response = new LoginResponse(accessToken, user.UserName ?? "");
        return Result<LoginResponse>.Ok(response);
    }

    /// <summary> Generate JWT token for authenticated user </summary> 
    /// <param name="user">Authenticated user</param> 
    /// <returns>JWT token string</returns>
    private string GenerateJwtToken(IdentityUser user)
    {
        var tokenHandler = new System.IdentityModel.Tokens.Jwt.JwtSecurityTokenHandler();
        var key = System.Text.Encoding.UTF8.GetBytes(_config["Jwt:Key"]!);
        var tokenDescriptor = new Microsoft.IdentityModel.Tokens.SecurityTokenDescriptor
        {
            Subject = new System.Security.Claims.ClaimsIdentity(new[]
            {
                new System.Security.Claims.Claim(System.Security.Claims.ClaimTypes.Name, user.UserName ?? "")
            }),
            Expires = DateTime.UtcNow.AddHours(1),
            SigningCredentials = new Microsoft.IdentityModel.Tokens.SigningCredentials(
                new Microsoft.IdentityModel.Tokens.SymmetricSecurityKey(key),
                Microsoft.IdentityModel.Tokens.SecurityAlgorithms.HmacSha256Signature)
        };
        var token = tokenHandler.CreateToken(tokenDescriptor);
        var accessToken = tokenHandler.WriteToken(token);
        return accessToken;
    }
}
