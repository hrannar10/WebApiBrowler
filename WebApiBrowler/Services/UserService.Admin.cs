using System;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using WebApiBrowler.Entities;

namespace WebApiBrowler.Services
{
    [Authorize(Policy = "Admin")]
    public partial class UserService
    {
        // Todo: add await/task
        public class Admin{
            private readonly UserManager<AppUser> _userManager;
            private readonly RoleManager<AppRole> _roleManager;

            public Admin(
                UserManager<AppUser> userManager, 
                RoleManager<AppRole> roleManager)
            {
                _userManager = userManager;
                _roleManager = roleManager;
            }

            // Todo: Add to admin role
            public static void AddAdmin(AppUser user)
            {
                throw new NotImplementedException();
            }

            // Todo: Remove from admin role
            [Authorize(Policy = "SuperAdmin")]
            public void RemoveAdmin(AppUser user)
            {
                throw new NotImplementedException();
            }

            // Todo: Add to voice role
            public void AddVoice(AppUser user)
            {
                throw new NotImplementedException();
            }

            // Todo: Remove from voice role
            public void RemoveVoice(AppUser user)
            {
                throw new NotImplementedException();
            }

            // Todo: View all users in admin role
            public void ViewAdmins()
            {
                throw new NotImplementedException();
            }

            // Todo: View all user in voice role
            public void ViewVoices()
            {
                throw new NotImplementedException();
            }
        }
    }
}
