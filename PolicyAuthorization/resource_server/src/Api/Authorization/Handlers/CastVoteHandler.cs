using Api.Authorization.Requirements;
using Core.Entities;
using Core.Extensions;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Api.Authorization.Handlers
{
    public class CastVoteHandler : AuthorizationHandler<CastVoteRequirement>
    {
        private readonly AuthenticationConfiguration _authConfig;
        public CastVoteHandler(AuthenticationConfiguration authConfig)
        {
            _authConfig = authConfig;
        }

        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, CastVoteRequirement requirement)
        {
            if (!context.User.HasClaim(c => c.Type == ClaimTypes.DateOfBirth && c.Issuer == _authConfig.Issuer))
            {
                context.Fail();
                return Task.CompletedTask;
            }

            int age = context.User.FindFirst(ClaimTypes.DateOfBirth).Value.CalculateAge();
            if (age < requirement.MinimumAge)
            {
                context.Fail();
                return Task.CompletedTask;
            }

            context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }
}
