using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using WebApiBrowler.Dtos;
using WebApiBrowler.Entities;
using WebApiBrowler.Helpers;
using WebApiBrowler.Models;
using WebApiBrowler.Services;

namespace WebApiBrowler.Controllers
{
    [Produces("application/json")]
    public class AccountsController : Controller
    {
        private readonly ApplicationDbContext _appDbContext;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IMapper _mapper;

        public AccountsController(
            UserManager<AppUser> userManager,
            IMapper mapper, 
            ApplicationDbContext appDbContext, 
            RoleManager<AppRole> roleManager)
        {
            _userManager = userManager;
            _mapper = mapper;
            _appDbContext = appDbContext;
            _roleManager = roleManager;
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
        public async Task<IActionResult> Post([FromBody]Requests.RegistrationDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userIdentity = _mapper.Map<AppUser>(model);

            var result = await _userManager.CreateAsync(userIdentity, model.Password);
            if (!result.Succeeded) return new BadRequestObjectResult(Errors.AddErrorsToModelState(result, ModelState));

            var userRole = await _roleManager.FindByNameAsync(Constants.Roles.User);
            if (userRole == null)
            {
                userRole = new AppRole(Constants.Roles.User);
                await _roleManager.CreateAsync(userRole);

                await _roleManager.AddClaimAsync(userRole,
                    new Claim(Constants.CustomClaimTypes.Permission, Constants.Permission.View));
            }

            if (!await _userManager.IsInRoleAsync(userIdentity, userRole.Name))
            {
                await _userManager.AddToRoleAsync(userIdentity, userRole.Name);
            }
            
            return new OkObjectResult("Account created");
        }

        [HttpPost]
        [Route("[controller]/Admin")]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(string))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest)]
        public IActionResult AddAdmin([FromBody] AppUser user)
        {
            UserService.Admin.AddAdmin(user);

            return Ok();
        }

        /// <summary>
        /// Get all users.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("[controller]/")]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(List<Responses.UserInfoDto>))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest)]
        public IActionResult GetAllUsers()
        {
            var users = _userManager.GetUsersInRoleAsync("admin");

            return Ok(_userManager.Users);
        }

        /// <summary>
        /// Get user by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("[controller]/{id}")]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(Responses.UserInfoDto))]
        [SwaggerResponse((int)HttpStatusCode.Unauthorized)]
        public IActionResult GetById(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = _userManager.Users.FirstOrDefault(i => Guid.Parse(i.Id) == id);

            return Ok(user);
        }

        /// <summary>
        /// Updates user with given id.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userDto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("[controller]/{id}")]
        [SwaggerResponse((int)HttpStatusCode.OK)]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, typeof(string))]
        [SwaggerResponse((int)HttpStatusCode.Unauthorized)]
        public IActionResult Update(Guid id, [FromBody]Requests.UserDto userDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
           
            try
            {
                var user = _userManager.Users.FirstOrDefault(i => Guid.Parse(i.Id) == id);

                if (user == null) return BadRequest("No user to update");

                user.FacebookId = userDto.FacebookId;
                user.FirstName = userDto.FirstName;
                user.LastName = userDto.LastName;
                user.PictureUrl = userDto.PictureUrl;

                // save 
                _userManager.UpdateAsync(user);

                return Ok();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Deletes a specific user.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("[controller]/{id}")]
        [SwaggerResponse((int)HttpStatusCode.OK)]
        [SwaggerResponse((int)HttpStatusCode.Unauthorized)]
        public IActionResult Delete(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = _userManager.Users.FirstOrDefault(i => Guid.Parse(i.Id) == id);

            _userManager.DeleteAsync(user);
            return Ok();
        }

    }
}