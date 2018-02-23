using System.Linq;
using System.Security.Claims;
using Microsoft.AspNetCore.Identity;
using WebApiBrowler.Entities;
using WebApiBrowler.Helpers;

namespace WebApiBrowler.Services
{
    public static class IdentityDataInitializer
    {
        public static void SeedData
        (UserManager<AppUser> userManager,
            RoleManager<AppRole> roleManager)
        {
            SeedRoles(roleManager);
            SeedUsers(userManager, roleManager);
        }

        public static void SeedUsers
            (UserManager<AppUser> userManager,
            RoleManager<AppRole> roleManager)
        {
            if (userManager.FindByNameAsync("minuZ").Result == null)
            {
                var user = new AppUser
                {
                    UserName = "minuZ",
                    Email = "minuZ",
                    FirstName = "Hrannar Mar",
                    LastName = "Agustsson"
                };

                var result = userManager.CreateAsync(user, "123456").Result;

                var newUser = userManager.FindByNameAsync("minuZ").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(newUser, Constants.Roles.SuperAdmin).Wait();
                    userManager.AddToRoleAsync(newUser, Constants.Roles.Admin).Wait();
                }
            }

            if (userManager.FindByNameAsync("Joe").Result == null)
            {
                var user = new AppUser
                {
                    UserName = "Joe",
                    Email = "Joe",
                    FirstName = "Joe",
                    LastName = "The Test"
                };

                var result = userManager.CreateAsync(user, "123456").Result;

                if (result.Succeeded)
                {
                    userManager.AddToRoleAsync(user, Constants.Roles.User).Wait();
                }
            }
        }

        public static void SeedRoles
            (RoleManager<AppRole> roleManager)
        {
            if (!roleManager.RoleExistsAsync(Constants.Roles.SuperAdmin).Result)
            {
                var role = new AppRole
                {
                    Name = Constants.Roles.SuperAdmin,
                    Description = "The First Order."
                };
                var roleResult = roleManager.CreateAsync(role).Result;
                var res = roleManager.AddClaimAsync(role,
                    new Claim(Constants.CustomClaimTypes.Permission, Constants.Permission.SupremeLeader)).Result;
                res = roleManager.AddClaimAsync(role,
                    new Claim(Constants.CustomClaimTypes.Permission, Constants.Permission.Delete)).Result;
                res = roleManager.AddClaimAsync(role,
                    new Claim(Constants.CustomClaimTypes.Permission, Constants.Permission.Update)).Result;
                res = roleManager.AddClaimAsync(role,
                    new Claim(Constants.CustomClaimTypes.Permission, Constants.Permission.Create)).Result;
                res = roleManager.AddClaimAsync(role,
                    new Claim(Constants.CustomClaimTypes.Permission, Constants.Permission.View)).Result;
            }

            if (!roleManager.RoleExistsAsync(Constants.Roles.Admin).Result)
            {
                var role = new AppRole
                {
                    Name = Constants.Roles.Admin,
                    Description = "Perform normal operations."
                };
                var roleResult = roleManager.CreateAsync(role).Result;
                var res = roleManager.AddClaimAsync(role,
                    new Claim(Constants.CustomClaimTypes.Permission, Constants.Permission.Delete)).Result;
                res = roleManager.AddClaimAsync(role,
                    new Claim(Constants.CustomClaimTypes.Permission, Constants.Permission.Update)).Result;
                res = roleManager.AddClaimAsync(role,
                    new Claim(Constants.CustomClaimTypes.Permission, Constants.Permission.Create)).Result;
                res = roleManager.AddClaimAsync(role,
                    new Claim(Constants.CustomClaimTypes.Permission, Constants.Permission.View)).Result;
            }

            if (!roleManager.RoleExistsAsync(Constants.Roles.Voice).Result)
            {
                var role = new AppRole
                {
                    Name = Constants.Roles.Voice,
                    Description = "Perform some admin operations."
                };
                var roleResult = roleManager.CreateAsync(role).Result;
                var res = roleManager.AddClaimAsync(role,
                    new Claim(Constants.CustomClaimTypes.Permission, Constants.Permission.Update)).Result;
                res = roleManager.AddClaimAsync(role,
                    new Claim(Constants.CustomClaimTypes.Permission, Constants.Permission.Create)).Result;
                res = roleManager.AddClaimAsync(role,
                    new Claim(Constants.CustomClaimTypes.Permission, Constants.Permission.View)).Result;
            }

            if (!roleManager.RoleExistsAsync(Constants.Roles.User).Result)
            {
                var role = new AppRole
                {
                    Name = Constants.Roles.User,
                    Description = "Perform all the operations."
                };
                var roleResult = roleManager.CreateAsync(role).Result;
                var res = roleManager.AddClaimAsync(role,
                    new Claim(Constants.CustomClaimTypes.Permission, Constants.Permission.View)).Result;
            }
        }
    }
}
