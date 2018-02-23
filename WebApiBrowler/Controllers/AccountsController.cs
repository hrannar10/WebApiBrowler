using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using WebApiBrowler.Dtos;
using WebApiBrowler.Entities;
using WebApiBrowler.Helpers;
using WebApiBrowler.Services;

namespace WebApiBrowler.Controllers
{
    [Authorize]
    [Produces("application/json")]
    public class AccountsController : Controller
    {
        private readonly ApplicationDbContext _appDbContext;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<AppRole> _roleManager;
        private readonly IMapper _mapper;
        private readonly UserService.IAdmin _admin;

        public AccountsController(
            UserManager<AppUser> userManager,
            IMapper mapper, 
            ApplicationDbContext appDbContext, 
            RoleManager<AppRole> roleManager, 
            UserService.IAdmin admin)
        {
            _userManager = userManager;
            _mapper = mapper;
            _appDbContext = appDbContext;
            _roleManager = roleManager;
            _admin = admin;
        }

        /// <summary>
        /// Create new user account.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [AllowAnonymous]
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

        /// <summary>
        /// View all admin users.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("[controller]/ViewAdmins")]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(List<Responses.UserInfoDto>))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ViewAdmins()
        {
            var users = await _admin.ViewAdmins();

            var userDtos = _mapper.Map<List<Responses.UserInfoDto>>(users);

            foreach (var userDto in userDtos)
            {
                var user = _userManager.Users.FirstOrDefault(x => x.Id == userDto.Id);
                userDto.Roles = (List<string>)await _userManager.GetRolesAsync(user);
            }

            return Ok(userDtos);
        }

        /// <summary>
        /// Adds user to admin role.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("[controller]/Admin")]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(bool))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AddAdmin(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByIdAsync(id.ToString());

            var result = await _admin.AddAdmin(user);

            return Ok(result);
        }

        /// <summary>
        /// Removes user from admin role.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("[controller]/Admin")]
        [Authorize(Policy = "SuperUser")]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(bool))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> RemoveAdmin(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByIdAsync(id.ToString());

            var result = await _admin.RemoveAdmin(user);

            return Ok(result);
        }

        /// <summary>
        /// View all voice users.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("[controller]/ViewVoices")]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(List<Responses.UserInfoDto>))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> ViewVoices()
        {
            var users = await _admin.ViewVoices();

            var userDtos = _mapper.Map<List<Responses.UserInfoDto>>(users);

            foreach (var userDto in userDtos)
            {
                var user = _userManager.Users.FirstOrDefault(x => x.Id == userDto.Id);
                userDto.Roles = (List<string>)await _userManager.GetRolesAsync(user);
            }

            return Ok(userDtos);
        }

        /// <summary>
        /// Adds user to voice role.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("[controller]/Voice")]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(bool))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> AddVoice(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByIdAsync(id.ToString());

            var result = await _admin.AddVoice(user);

            return Ok(result);
        }

        /// <summary>
        /// Removes user from voice role.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("[controller]/Voice")]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(bool))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> RemoveVoice(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var user = await _userManager.FindByIdAsync(id.ToString());

            var result = await _admin.RemoveVoice(user);

            return Ok(result);
        }

        /// <summary>
        /// Get all users.
        /// </summary>
        /// <returns></returns>
        [Authorize(Policy = "SuperUser")]
        [HttpGet]
        [Route("[controller]/")]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(List<Responses.UserInfoDto>))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest)]
        public async Task<IActionResult> GetAllUsers()
        {
            var users = _userManager.Users;
            var userDtos = _mapper.Map<List<Responses.UserInfoDto>>(users);

            foreach (var userDto in userDtos)
            {
                var user = _userManager.Users.FirstOrDefault(x => x.Id == userDto.Id);
                userDto.Roles = (List<string>) await _userManager.GetRolesAsync(user);

                var userss = await _userManager.GetUsersInRoleAsync(Constants.Roles.SuperAdmin);

                var role = await _roleManager.FindByNameAsync(userDto.Roles[0]);
                var claims = await _roleManager.GetClaimsAsync(role);

                userDto.Claims = claims.ToArray();
            }

            return Ok(userDtos);
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