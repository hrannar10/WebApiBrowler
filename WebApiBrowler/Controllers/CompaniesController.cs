using System.Collections.Generic;
using System.Net;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
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
    public class CompaniesController : Controller
    {
        private readonly ICompanyService _companyService;
        private readonly IMapper _mapper;

        public CompaniesController(
            ICompanyService companyService,
            IMapper mapper)
        {
            _companyService = companyService;
            _mapper = mapper;
        }

        /// <summary>
        /// Creates new company.
        /// </summary>
        /// <param name="companyDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("[controller]")]
        [SwaggerResponse((int)HttpStatusCode.OK)]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, typeof(string))]
        [SwaggerResponse((int)HttpStatusCode.Unauthorized)]
        public IActionResult Create([FromBody] Requests.CompanyDto companyDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            // map dto to entity
            var company = _mapper.Map<Company>(companyDto);

            try
            {
                // save
                _companyService.Create(company);
                return Ok();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get all companies.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("[controller]")]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(List<Responses.CompanyDto>))]
        [SwaggerResponse((int)HttpStatusCode.Unauthorized)]
        public IActionResult GetAll()
        {
            var companies = _companyService.GetAll();
            var companyDtos = _mapper.Map<IList<Responses.CompanyDto>>(companies);
            return Ok(companyDtos);
        }

        /// <summary>
        /// Get a company by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("[controller]/{id}")]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(Responses.CompanyDto))]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, typeof(string))]
        [SwaggerResponse((int)HttpStatusCode.Unauthorized)]
        public IActionResult GetById(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var company = _companyService.GetById(id);
            var companyDto = _mapper.Map<Responses.CompanyDto>(company);
            return Ok(companyDto);
        }

        /// <summary>
        /// Updates company with given id.
        /// </summary>
        /// <param name="id"></param>
        /// <param name="companyDto"></param>
        /// <returns></returns>
        [HttpPut]
        [Route("[controller]/{id}")]
        [SwaggerResponse((int)HttpStatusCode.OK)]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, typeof(string))]
        [SwaggerResponse((int)HttpStatusCode.Unauthorized)]
        public IActionResult Update(int id, [FromBody]Requests.CompanyDto companyDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // map dto to entity and set id
            var company = _mapper.Map<Company>(companyDto);
            company.Id = id;

            try
            {
                // save
                _companyService.Update(company);
                return Ok();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Deletes a specific company with given id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("[controller]/{id}")]
        [SwaggerResponse((int)HttpStatusCode.OK)]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, typeof(string))]
        [SwaggerResponse((int)HttpStatusCode.Unauthorized)]
        public IActionResult Delete(int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _companyService.Delete(id);
            return Ok();
        }

        /// <summary>
        /// Add user to company.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("[controller]/add")]
        [SwaggerResponse((int)HttpStatusCode.OK)]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, typeof(string))]
        [SwaggerResponse((int)HttpStatusCode.Unauthorized)]
        public IActionResult AddUser([FromBody]Requests.ModUserCompanyDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _companyService.AddUser(model.CompanyId, model.UserId);
            return Ok();
        }

        /// <summary>
        /// Remove user from company.
        /// </summary>
        /// <param name="model"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("[controller]/remove")]
        [SwaggerResponse((int)HttpStatusCode.OK)]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, typeof(string))]
        [SwaggerResponse((int)HttpStatusCode.Unauthorized)]
        public IActionResult RemoveUser([FromBody]Requests.ModUserCompanyDto model)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _companyService.RemoveUser(model.CompanyId, model.UserId);
            return Ok();
        }
    }
}