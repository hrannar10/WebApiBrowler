using System.Collections.Generic;
using Microsoft.AspNetCore.Identity;

namespace WebApiBrowler.Entities
{
    public class AppRole : IdentityRole
    {
        public AppRole() {}
        public virtual ICollection<AppUser> Users { get; set; } = new List<AppUser>();

        public AppRole(string roleName) : base(roleName)
        {

        }

        public AppRole(string roleName, string description) : base(roleName)
        {
            Description = description;
        }

        public string Description { get; set; }
    }
}
