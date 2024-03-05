using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Users.Api.Configurations;
using Users.Application.Services.Interfaces;
using Users.Domain.Models;

namespace Users.Api.Controllers.Users;

[Route("api/users")]
[ApiController]
public class UsersController(
    IUserService userService,
    IOptionsSnapshot<JwtConfiguration> jwtConfigSnapshot) : ControllerBase
{
    private readonly IUserService _userService = userService;
    private readonly JwtConfiguration _jwtConfig = jwtConfigSnapshot.Value;

    [HttpPost]
    public async Task<ActionResult<User>> Post([FromBody] CreateUserRequest request)
    {
        var user = new User(Id: string.Empty, request.FirstName, request.LastName, request.Email);

        var createResult = await _userService.CreateAsync(user, request.Password);

        if (!createResult.Success)
        {
            return BadRequest();
        }

        return Ok(user);
    }

    [HttpPost("login")]
    public async Task<ActionResult<string>> Post([FromBody] LoginRequest request)
    {
        var loginResult = await _userService.LoginAsync(request.Email, request.Password);

        if (loginResult.User is null)
        {
            return Unauthorized();
        }

        var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtConfig.Key));
        var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);
        var user = loginResult.User;

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
