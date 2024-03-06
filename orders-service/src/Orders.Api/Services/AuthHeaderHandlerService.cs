using Microsoft.AspNetCore.Authentication.JwtBearer;
using Orders.Application.Common;
using System.Net.Http.Headers;

namespace Orders.Api.Services;

public class AuthorizationHeaderHandler(IHttpContextAccessor httpContextAccessor) : DelegatingHandler
{
    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (httpContextAccessor.HttpContext is null)
        {
            return await base.SendAsync(request, cancellationToken);
        }

        var loggedUserProvider = httpContextAccessor.HttpContext.RequestServices.GetRequiredService<ILoggedUserProvider>();

        request.Headers.Authorization = new AuthenticationHeaderValue(
            JwtBearerDefaults.AuthenticationScheme, 
            loggedUserProvider.LoggedUser.Authorization);

        return await base.SendAsync(request, cancellationToken);
    }
}
