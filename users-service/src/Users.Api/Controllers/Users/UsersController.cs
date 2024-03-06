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

/// <summary>
/// Users controller.
/// </summary>
/// <param name="userService">The <see cref="IUserService"/> to handle user requests.</param>
/// <param name="jwtConfigSnapshot">The JWT configuration to generate the token.</param>
[Route("api/users")]
[ApiController]
[Produces("application/json")]
public class UsersController(
    IUserService userService,
    IOptionsSnapshot<JwtConfiguration> jwtConfigSnapshot) : ControllerBase
{
    private readonly IUserService _userService = userService;
    private readonly JwtConfiguration _jwtConfig = jwtConfigSnapshot.Value;

    /// <summary>
    /// Creates a user with the specified data.
    /// </summary>
    /// <param name="request">The data to create the user.</param>
    /// <returns>The newly created user.</returns>
    /// <response code="200">Returns the newly created user.</response>
    /// <response code="400">If the provided data to create the user is not valid.</response>
    [HttpPost]
    [ProducesResponseType(typeof(User), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(Dictionary<string, string>), StatusCodes.Status400BadRequest)]
    public async Task<ActionResult<User>> Post([FromBody] CreateUserRequest request)
    {
        var user = new User(Id: string.Empty, request.FirstName, request.LastName, request.Email);

        var createResult = await _userService.CreateAsync(user, request.Password);

        if (!createResult.Success)
        {
            if (createResult.Errors.Count > 0)
            {
                return BadRequest(createResult.Errors);
            }

            return Conflict();
        }

        return Ok(user);
    }

    /// <summary>
    /// Logs in a user with the specified data.
    /// </summary>
    /// <param name="request">The data to login the user.</param>
    /// <returns>The authorization token.</returns>
    /// <response code="200">Returns the newly created user.</response>
    /// <response code="401">If the user does not exist or cannot be logged in with the provided data.</response>
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
