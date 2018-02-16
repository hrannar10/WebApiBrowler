using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using WebApiBrowler.Dtos;
using WebApiBrowler.Entities;
using WebApiBrowler.Helpers;

namespace WebApiBrowler.Controllers
{
    [Produces("application/json")]
    public class AccountsController : Controller
    {
        private readonly ApplicationDbContext _appDbContext;
        private readonly UserManager<AppUser> _userManager;
        private readonly IMapper _mapper;

        public AccountsController(
            UserManager<AppUser> userManager, 
            IMapper mapper, 
            ApplicationDbContext appDbContext)
        {
            _userManager = userManager;
            _mapper = mapper;
            _appDbContext = appDbContext;
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
        public async Task<IActionResult> Post([FromBody]RegistrationDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userIdentity = _mapper.Map<AppUser>(model);

            var result = await _userManager.CreateAsync(userIdentity, model.Password);

            if (!result.Succeeded) return new BadRequestObjectResult(Errors.AddErrorsToModelState(result, ModelState));

            await _appDbContext.Customers.AddAsync(new Customer { Id = Guid.Parse(userIdentity.Id), Location = model.Location });
            await _appDbContext.SaveChangesAsync();

            return new OkObjectResult("Account created");
        }

        /// <summary>
        /// Get all users.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("[controller]/")]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(List<Customer>))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest)]
        public IActionResult GetAllUsers()
        {
            return Ok(_userManager.Users);
        }

        /// <summary>
        /// Get user by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("[controller]/{id}")]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(Object))]
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
        public IActionResult Update(Guid id, [FromBody]UserDto userDto)
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