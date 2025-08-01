using Microsoft.AspNetCore.Authorization;

namespace SLN.Utility.JWT;

/// <summary>
/// ref: RolesAuthorizationRequirement
/// </summary>
public class MyRequirement : AuthorizationHandler<MyRequirement>, IAuthorizationRequirement
{
    protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, MyRequirement requirement)
    {
        if (context.User != null)
        {
            if (context.User.HasClaim(x => x.Type == "role"))
            {
                context.Succeed(requirement);
            }
        }

        return Task.CompletedTask;
    }
}