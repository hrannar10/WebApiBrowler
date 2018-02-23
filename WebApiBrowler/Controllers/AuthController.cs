using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Security.Principal;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using Swashbuckle.AspNetCore.SwaggerGen;
using WebApiBrowler.Auth;
using WebApiBrowler.Dtos;
using WebApiBrowler.Entities;
using WebApiBrowler.Helpers;
using WebApiBrowler.Models;

namespace WebApiBrowler.Controllers
{
    [Produces("application/json")]
    public class AuthController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IJwtFactory _jwtFactory;
        private readonly JwtIssuerOptions _jwtOptions;

        public AuthController(
            UserManager<AppUser> userManager, 
            IJwtFactory jwtFactory, 
            IOptions<JwtIssuerOptions> jwtOptions, 
            RoleManager<AppRole> roleManager)
        {
            _userManager = userManager;
            _jwtFactory = jwtFactory;
            _roleManager = roleManager;
            _jwtOptions = jwtOptions.Value;
        }

        /// <summary>
        /// Login user.
        /// </summary>
        /// <param name="credentials"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("[controller]/")]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(Responses.TokenDto))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Post([FromBody]Requests.CredentialsDto credentials)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var identity = await GetClaimsIdentity(credentials.UserName, credentials.Password);
            if (identity == null)
            {
                return BadRequest(Errors.AddErrorToModelState("login_failure", "Invalid username or password.", ModelState));
            }

            var jwt = await Tokens.GenerateJwt(identity, _jwtFactory, credentials.UserName, _jwtOptions, new JsonSerializerSettings { Formatting = Formatting.Indented });
            return new OkObjectResult(jwt);
        }

        private async Task<ClaimsIdentity> GetClaimsIdentity(string userName, string password)
        {
            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
                return await Task.FromResult<ClaimsIdentity>(null);

            // get the user to verifty
            var userToVerify = await _userManager.FindByNameAsync(userName);

            if (userToVerify == null) return await Task.FromResult<ClaimsIdentity>(null);

            // check the credentials
            if (await _userManager.CheckPasswordAsync(userToVerify, password))
            {
                var user = await _userManager.FindByNameAsync(userToVerify.UserName);
                var roles = await _userManager.GetRolesAsync(user);
                var claims = new List<Claim>();


                foreach (var role in roles)
                {
                    var r = await _roleManager.FindByNameAsync(role);
                    var roleClaims = await _roleManager.GetClaimsAsync(r);

                    foreach (var roleClaim in roleClaims)
                    {
                        if (!ContainsClaim(roleClaim, claims))
                        {
                            claims.Add(roleClaim);
                        }
                    }
                }

                var claimsIdentiy = new ClaimsIdentity(new GenericIdentity(userName, "Token"), new[]
                {
                    new Claim(Helpers.Constants.Strings.JwtClaimIdentifiers.Id, userToVerify.Id)
                });

                var blhe = new Claim(Helpers.Constants.Strings.JwtClaimIdentifiers.Rol, roles[0]);
                claimsIdentiy.AddClaim(blhe);

                if (roles.Count > 1)
                {
                    var bleh = new Claim(Helpers.Constants.Strings.JwtClaimIdentifiers.Rol, roles[1]);
                    claimsIdentiy.AddClaim(blhe);
                }



                return claimsIdentiy;
            }

            // Credentials are invalid, or account doesn't exist
            return await Task.FromResult<ClaimsIdentity>(null);
        }

        private bool ContainsClaim(Claim claim, List<Claim> claims)
        {
            return claims.Any(c => c.Value == claim.Value && c.Type == claim.Type);
        }
    }
}