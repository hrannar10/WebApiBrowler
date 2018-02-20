using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using WebApiBrowler.Entities;
using WebApiBrowler.Helpers;

namespace WebApiBrowler.Services
{
    [Authorize]
    public partial class UserService
    {
        public interface IAdmin
        {
            Task<bool> AddAdmin(AppUser user);
            void RemoveAdmin(AppUser user);
            void AddVoice(AppUser user);
            void RemoveVoice(AppUser user);
            void ViewAdmins();
            void ViewVoices();

        }

        public class Admin : IAdmin{
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
            public async Task<bool> AddAdmin(AppUser user)
            {
                var adminRole = await CreateRoleIfNotExists(Constants.Roles.Admin);
                var result = new IdentityResult();

                if (!await _userManager.IsInRoleAsync(user, adminRole.Name))
                {
                    result = await _userManager.AddToRoleAsync(user, adminRole.Name);
                }
                return result.Succeeded;
            }

            // Todo: Remove from admin role
            //[Authorize(Policy = "SuperAdmin")]
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

            /// <summary>
            /// Checks if role name exists, create it if not, 
            /// then return role name object.
            /// </summary>
            /// <param name="roleName"></param>
            /// <returns></returns>
            private async Task<AppRole> CreateRoleIfNotExists(string roleName)
            {
                var role = await _roleManager.FindByNameAsync(roleName);
                if (role != null) return role;

                role = new AppRole(roleName);
                await _roleManager.CreateAsync(role);

                return role;
            }
        }
    }
}
