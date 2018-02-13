using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using WebApiBrowler.Dtos;
using WebApiBrowler.Helpers;
using WebApiBrowler.Models.Entities;

namespace WebApiBrowler.Controllers
{
    [Produces("application/json")]
    public class AccountsController : Controller
    {
        private readonly ApplicationDbContext _appDbContext;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;

        public AccountsController(
            ApplicationDbContext appDbContext, 
            UserManager<AppUser> userManager, 
            IMapper mapper)
        {
            _appDbContext = appDbContext;
            _userManager = userManager;
            _mapper = mapper;
        }

        /// <summary>
        /// Create new user account.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("[controller]/")]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(string))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> Post([FromBody] RegistrationDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userIdentity = _mapper.Map<AppUser>(model);

            var result = await _userManager.CreateAsync(userIdentity, model.Password);

            if (!result.Succeeded)
            {
                return new BadRequestObjectResult(Errors.AddErrorsToModelState(result, ModelState));
            }

            await _appDbContext.Customers.AddAsync(new Customer
            {
                IdentityId = userIdentity.Id,
                Location = model.Location
            });

            return new OkObjectResult("Account created");
        }
    }
}