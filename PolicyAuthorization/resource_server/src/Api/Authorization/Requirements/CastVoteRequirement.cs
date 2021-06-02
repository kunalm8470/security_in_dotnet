using Microsoft.AspNetCore.Authorization;

namespace Api.Authorization.Requirements
{
    public class CastVoteRequirement : IAuthorizationRequirement
    {
        public int MinimumAge { get; }
        public CastVoteRequirement(int minimumAge)
        {
            MinimumAge = minimumAge;
        }
    }
}
