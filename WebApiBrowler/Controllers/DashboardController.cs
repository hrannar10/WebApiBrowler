using System.Linq;
using System.Net;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using WebApiBrowler.Dtos;
using WebApiBrowler.Entities;

namespace WebApiBrowler.Controllers
{
    [Produces("application/json")]
    //[Authorize(Policy = "ApiUser")]
    public class DashboardController : Controller
    {
        private readonly ClaimsPrincipal _caller;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;


        public DashboardController(
            UserManager<AppUser> userManager,
            RoleManager<AppRole> roleManager,
            IHttpContextAccessor httpContextAccessor)
        {

            _userManager = userManager;
            _roleManager = roleManager;
            _caller = httpContextAccessor.HttpContext.User;
        }

        /// <summary>
        /// Retrieve the user info.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("[controller]")]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(Responses.UserInfoDto))]
        public IActionResult Home()
        {
            var userId = _caller.Claims.Single(c => c.Type == "id").Value;
            var user = _userManager.Users.FirstOrDefault(i => i.Id == userId);

            if (user == null)
            {
                return BadRequest("No user found.");
            }

            var roles = _roleManager.Roles;

            return Ok(new Responses.UserInfoDto
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                PictureUrl = user.PictureUrl,
                FacebookId = user.FacebookId
            });
        }
    }
}