using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Policy;
using Microsoft.AspNetCore.Http;
using System.Buffers;
using System.Net;
using System.Security.Claims;
using System.Text;

namespace FinanceControl.Authorization
{
    public class MinimumAgeHandler : AuthorizationHandler<MinimumAgeRequirement>
    {


        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MinimumAgeRequirement requirement)
        {
            if (!context.User.HasClaim(x => x.Type == ClaimTypes.DateOfBirth))
            {
                context.Fail();
                return Task.CompletedTask;
            }

            var birthDate = DateTime.ParseExact(context.User.FindFirst(x => x.Type == ClaimTypes.DateOfBirth).Value, "MM/dd/yyyy HH:mm:ss", null);

            var userAge = DateTime.Today.Year - birthDate.Year;

            if (birthDate > DateTime.Today.AddYears(-userAge))
                userAge--;

            if (userAge >= requirement.MinimumAge)
            {
                context.Succeed(requirement);
                return Task.CompletedTask;
            }

            AuthorizationFailure.Failed(context.PendingRequirements);

            return Task.CompletedTask;

        }
    }


}

