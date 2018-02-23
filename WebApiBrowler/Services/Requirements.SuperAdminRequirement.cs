using Microsoft.AspNetCore.Authorization;

namespace WebApiBrowler.Services
{
    public partial class Requirements
    {
        public class SuperAdminRequirement : IAuthorizationRequirement
        {
            public string UserName { get; set; }

            public SuperAdminRequirement(string userName)
            {
                UserName = userName;
            }
        }
    }
}
