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
    [Authorize]
    public class DashboardController : Controller
    {
        private readonly ClaimsPrincipal _caller;
        private readonly ApplicationDbContext _appDbContext;

        public DashboardController(
            UserManager<AppUser> userManager, 
            ApplicationDbContext appDbContext, 
            IHttpContextAccessor httpContextAccessor)
        {
            _caller = httpContextAccessor.HttpContext.User;
            _appDbContext = appDbContext;
        }

        /// <summary>
        /// Retrieve the user info.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("[controller]")]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(UserInfoDto))]
        public async Task<IActionResult> Home()
        {
            var userId = _caller.Claims.Single(c => c.Type == "id");
            var customer = await _appDbContext.Customers.Include(c => c.Identity).SingleAsync(c => c.Identity.Id == userId.Value);

            return new OkObjectResult(new UserInfoDto
            {
                FirstName = customer.Identity.FirstName,
                LastName = customer.Identity.LastName,
                PictureUrl = customer.Identity.PictureUrl,
                FacebookId = customer.Identity.FacebookId,
                Location = customer.Location,
                Locale = customer.Locale,
                Gender = customer.Gender
            });
        }
    }
}