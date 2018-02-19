using Microsoft.AspNetCore.Identity;

namespace WebApiBrowler.Entities
{
    public class AppRole : IdentityRole
    {
        public string Description { get; set; }
    }
}
