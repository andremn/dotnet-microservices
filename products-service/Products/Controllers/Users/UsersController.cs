using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using Products.Config;
using Products.Model;
using Products.Services;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace Products.Controllers.Users;

[Route("api/users")]
[ApiController]
public class UsersController(
    IUserService userService,
    IOptionsSnapshot<JwtConfig> jwtConfigSnapshot) : ControllerBase
{
    private readonly IUserService _userService = userService;
    private readonly JwtConfig _jwtConfig = jwtConfigSnapshot.Value;

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] CreateUserRequest request)
    {
        var user = new User(Id: string.Empty, request.FirstName, request.LastName, request.Email);

        var createResult = await _userService.CreateUserAsync(user, request.Password);

        if (!createResult.Success)
        {
            return BadRequest();
        }

        return Ok(user);
    }

    [HttpPost("login")]
    public async Task<IActionResult> Post([FromBody] LoginRequest request)
    {
        var loginResult = await _userService.LoginAsync(request.Email, request.Password);

        if (!loginResult.Success)
        {
            return BadRequest();
        }

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.Key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var getUserResult = await _userService.GetUserByEmailAsync(request.Email);
        var user = getUserResult.User;

        if (user is null)
        {
            return Forbid();
        }

        var claims = new List<Claim>
        {
            new(Constants.UserIdClaimType, user.Id),
            new(Constants.UserEmailClaimType, user.Email),
            new(Constants.UserFirstNameClaimType, user.FirstName),
            new(Constants.UserLastNameClaimType, user.LastName)
        };

        var securityToken = new JwtSecurityToken(
            issuer: _jwtConfig.Issuer,
            audience: _jwtConfig.Issuer,
            claims: claims,
            expires: DateTime.UtcNow.Add(_jwtConfig.TimeToExpire),
            signingCredentials: credentials);

        var token = new JwtSecurityTokenHandler().WriteToken(securityToken);

        return Ok(token);
    }
}
