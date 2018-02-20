using System;
using System.Collections.Generic;
using System.Security.Claims;
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
            Task<bool> RemoveAdmin(AppUser user);
            Task<bool> AddVoice(AppUser user);
            Task<bool> RemoveVoice(AppUser user);
            Task<ICollection<AppUser>> ViewAdmins();
            Task<ICollection<AppUser>> ViewVoices();

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

            /// <summary>
            /// Adds user to admin role.
            /// </summary>
            /// <param name="user"></param>
            /// <returns></returns>
            public async Task<bool> AddAdmin(AppUser user)
            {
                var role = await CreateRoleIfNotExists(Constants.Roles.Admin);
                var result = new IdentityResult();

                if (!await _userManager.IsInRoleAsync(user, role.Name))
                {
                    result = await _userManager.AddToRoleAsync(user, role.Name);
                }
                return result.Succeeded;
            }

            /// <summary>
            /// Removes user from admin role.
            /// </summary>
            /// <param name="user"></param>
            /// <returns></returns>
            //[Authorize(Policy = "SuperAdmin")]
            public async Task<bool> RemoveAdmin(AppUser user)
            {
                var result = new IdentityResult();
                if (!await _userManager.IsInRoleAsync(user, Constants.Roles.Admin))
                {
                    result = await _userManager.RemoveFromRoleAsync(user, Constants.Roles.Admin);
                }

                return result.Succeeded;
            }

            /// <summary>
            /// Adds user to voice role.
            /// </summary>
            /// <param name="user"></param>
            /// <returns></returns>
            public async Task<bool> AddVoice(AppUser user)
            {
                var role = await CreateRoleIfNotExists(Constants.Roles.Voice);
                var result = new IdentityResult();

                if (!await _userManager.IsInRoleAsync(user, role.Name))
                {
                    result = await _userManager.AddToRoleAsync(user, role.Name);
                }
                return result.Succeeded;
            }

            /// <summary>
            /// Removes user from voice role.
            /// </summary>
            /// <param name="user"></param>
            /// <returns></returns>
            public async Task<bool> RemoveVoice(AppUser user)
            {
                var result = new IdentityResult();
                if (!await _userManager.IsInRoleAsync(user, Constants.Roles.Voice))
                {
                    result = await _userManager.RemoveFromRoleAsync(user, Constants.Roles.Voice);
                }

                return result.Succeeded;
            }

            // Todo: View all users in admin role
            //[Authorize(Policy = "SuperAdmin")]
            public async Task<ICollection<AppUser>> ViewAdmins()
            {
                var role = await _roleManager.FindByNameAsync(Constants.Roles.Admin);

                return role.Users;
            }

            // Todo: View all user in voice role
            public async Task<ICollection<AppUser>> ViewVoices()
            {
                var role = await _roleManager.FindByNameAsync(Constants.Roles.Voice);

                return role.Users;
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

                switch (roleName)
                {
                    case Constants.Roles.Admin:
                        role.Description = "Perform admin operations.";
                        break;
                    case Constants.Roles.Voice:
                        role.Description = "Perform create, update and normal operations.";
                        break;
                    case Constants.Roles.User:
                        role.Description = "Perform normal operations.";
                        break;
                    default:
                        break;
                }

                await _roleManager.CreateAsync(role);

                // Adds permissions to roles
                switch (roleName)
                {
                    case Constants.Roles.Admin:
                        await _roleManager.AddClaimAsync(role,
                            new Claim(Constants.CustomClaimTypes.Permission, Constants.Permission.Delete));
                        goto case Constants.Roles.Voice;

                    case Constants.Roles.Voice:
                        await _roleManager.AddClaimAsync(role,
                            new Claim(Constants.CustomClaimTypes.Permission, Constants.Permission.Update));
                        await _roleManager.AddClaimAsync(role,
                            new Claim(Constants.CustomClaimTypes.Permission, Constants.Permission.Create));
                        goto case Constants.Roles.User;

                    case Constants.Roles.User:
                        await _roleManager.AddClaimAsync(role,
                            new Claim(Constants.CustomClaimTypes.Permission, Constants.Permission.View));
                        break;
                    default:
                        break;
                }

                return role;
            }
        }
    }
}
