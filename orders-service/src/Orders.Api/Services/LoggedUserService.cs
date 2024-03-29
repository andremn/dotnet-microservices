﻿using Microsoft.AspNetCore.Authentication.JwtBearer;
using Orders.Application.Dtos;

namespace Orders.Api.Services;

public class LoggedUserService(IHttpContextAccessor httpContextAccessor) : ILoggedUserService
{
    public LoggedUserDto GetLoggedUser()
    {
        var user = httpContextAccessor.HttpContext?.User;
        var authorizationHeaderValue = httpContextAccessor.HttpContext?.Request.Headers.Authorization.ToString() ?? string.Empty;

        var firstName = user?.FindFirst(Constants.UserFirstNameClaimType)?.Value ?? string.Empty;
        var lastName = user?.FindFirst(Constants.UserLastNameClaimType)?.Value ?? string.Empty;
        var email = user?.FindFirst(Constants.UserEmailClaimType)?.Value ?? string.Empty;
        var id = user?.FindFirst(Constants.UserIdClaimType)?.Value ?? string.Empty;
        var authorization = authorizationHeaderValue.Replace(JwtBearerDefaults.AuthenticationScheme, string.Empty).TrimStart();

        return email is null
            ? throw new InvalidOperationException("Cannot find email for current user")
            : new LoggedUserDto(id, firstName, lastName, email, authorization);
    }
}
