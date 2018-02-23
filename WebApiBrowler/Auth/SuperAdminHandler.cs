using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using WebApiBrowler.Helpers;
using WebApiBrowler.Services;

namespace WebApiBrowler.Auth
{
    public class SuperAdminHandler : AuthorizationHandler<Requirements.SuperAdminRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context,
            Requirements.SuperAdminRequirement requirement)
        {
            if (!context.User.HasClaim(c => c.Type == "Permission"))
            {
                return Task.CompletedTask;
            }

            var userName = context.User.FindFirst(Constants.Permission.SupremeLeader).Value;

            //if (userName == "minuZ")
            //{
                
            //}
            context.Succeed(requirement);
            return Task.CompletedTask;
        }
    }
}
