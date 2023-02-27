using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Options;
using System.Buffers;
using System.Text;
using System.Text.Encodings.Web;
using System.Web.Http.Results;

namespace FinanceControl.Authorization
{
    public class CustomAuthorization : IAuthorizationMiddlewareResultHandler
    {
        private readonly IAuthorizationMiddlewareResultHandler _handler;

        public CustomAuthorization()
        {
            _handler = new AuthorizationMiddlewareResultHandler();
        }

        public async Task HandleAsync(
            RequestDelegate requestDelegate,
            HttpContext httpContext,
            AuthorizationPolicy authorizationPolicy,
            PolicyAuthorizationResult policyAuthorizationResult)
        {

            if (policyAuthorizationResult.Forbidden && policyAuthorizationResult.AuthorizationFailure != null)
            {

                if (policyAuthorizationResult.AuthorizationFailure!.FailedRequirements
                .OfType<MinimumAgeRequirement>().Any())
                {

                    httpContext.Response.Headers.ContentType = "text/application";

                    httpContext.Response.StatusCode = StatusCodes.Status403Forbidden;

                    var msg = "Você precisa ter +18 anos para realizar essa operação!";

                    var bytesMsg = Encoding.UTF8.GetBytes(msg);

                    await httpContext.Response.Body.WriteAsync(bytesMsg);

                    return;
                }

                if (policyAuthorizationResult.AuthorizationFailure!.FailedRequirements
                .OfType<RolesAuthorizationRequirement>().Any())
                {
                    httpContext.Response.Headers.ContentType = "text/application";

                    httpContext.Response.StatusCode = StatusCodes.Status403Forbidden;

                    var msg = "Você não tem autorização para realizar essa operação!";

                    var bytesMsg = Encoding.UTF8.GetBytes(msg);

                    await httpContext.Response.Body.WriteAsync(bytesMsg);

                    return;
                }
            }

            await _handler.HandleAsync(requestDelegate, httpContext, authorizationPolicy, policyAuthorizationResult);
        }

    }

}

