using Microsoft.AspNetCore.Authorization;

namespace Abandon.NET.Utility.Authorization;

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