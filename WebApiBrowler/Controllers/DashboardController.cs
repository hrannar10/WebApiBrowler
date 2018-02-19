using System;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Swashbuckle.AspNetCore.SwaggerGen;
using WebApiBrowler.Dtos;
using WebApiBrowler.Entities;
using WebApiBrowler.Helpers;

namespace WebApiBrowler.Controllers
{
    [Produces("application/json")]
    [Authorize(Policy = "ApiUser")]
    public class DashboardController : Controller
    {
        private readonly ClaimsPrincipal _caller;
        private readonly UserManager<AppUser> _userManager;
        private readonly ApplicationDbContext _appDbContext;


        public DashboardController(
            UserManager<AppUser> userManager, 
            ApplicationDbContext appDbContext, 
            IHttpContextAccessor httpContextAccessor)
        {

            _userManager = userManager;
            _appDbContext = appDbContext;
            _caller = httpContextAccessor.HttpContext.User;
        }

        /// <summary>
        /// Retrieve the user info.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("[controller]")]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(Responses.UserInfoDto))]
        public async Task<IActionResult> Home()
        {
            var userId = _caller.Claims.Single(c => c.Type == "id").Value;
            var user = _userManager.Users.FirstOrDefault(i => i.Id == userId);

            if (user == null)
            {
                return BadRequest("No user found, you should not exists");
            }

            return Ok(new Responses.UserInfoDto
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                PictureUrl = user.PictureUrl,
                FacebookId = user.FacebookId,
            });
        }
    }
}