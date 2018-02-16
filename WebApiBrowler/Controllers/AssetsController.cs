using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Swashbuckle.AspNetCore.SwaggerGen;
using WebApiBrowler.Dtos.Response;
using WebApiBrowler.Entities;
using WebApiBrowler.Helpers;
using WebApiBrowler.Models;
using WebApiBrowler.Services;
using AssetDtoRequest = WebApiBrowler.Dtos.Request.AssetDtoRequest;
using AssetTypeDtoResponse = WebApiBrowler.Dtos.Response.AssetTypeDtoResponse;

namespace WebApiBrowler.Controllers
{
    [Authorize]
    [Produces("application/json")]
    public class AssetsController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly ClaimsPrincipal _caller;
        private readonly IAssetService _assetService;
        private readonly IMapper _mapper;

        public AssetsController(
            UserManager<AppUser> userManager,
            IAssetService assetService, 
            IMapper mapper,
            IHttpContextAccessor httpContextAccessor)
        {
            _userManager = userManager;
            _assetService = assetService;
            _mapper = mapper;
            _caller = httpContextAccessor.HttpContext.User;
        }

        /// <summary>
        /// Creates new asset.
        /// </summary>
        /// <param name="assetDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("[controller]")]
        [SwaggerResponse((int)HttpStatusCode.OK)]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, typeof(string))]
        [SwaggerResponse((int)HttpStatusCode.Unauthorized)]
        public IActionResult Create([FromBody]AssetDtoRequest assetDto)
        {
            // map dto to entity
            var asset = _mapper.Map<Asset>(assetDto);

            try
            {
                // save
                _assetService.Create(asset);
                return Ok();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get all assets.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("[controller]")]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(List<Dtos.Response.AssetDtoResponse>))]
        [SwaggerResponse((int)HttpStatusCode.Unauthorized)]
        public IActionResult GetAll()
        {
            var assets = _assetService.GetAll();
            var assetDtos = _mapper.Map<IList<Dtos.Response.AssetDtoResponse>>(assets);

            foreach (var assetDto in assetDtos)
            {
                var user = _userManager.Users.FirstOrDefault(x => x.Id == assetDto.CreatedBy);
                if (user == null) continue;
                assetDto.CreatedBy = user.FirstName + " " + user.LastName;

                user = _userManager.Users.FirstOrDefault(x => x.Id == assetDto.ModifiedBy);
                if (user == null) continue;
                assetDto.ModifiedBy = user.FirstName + " " + user.LastName;
            }

            return Ok(assetDtos);
        }

        /// <summary>
        /// Get an asset by id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet]
        [Route("[controller]/{id}")]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(Dtos.Response.AssetDtoResponse))]
        [SwaggerResponse((int)HttpStatusCode.Unauthorized)]
        public IActionResult GetById(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var asset = _assetService.GetById(id);

            if (asset == null) return NoContent();

            var assetDto = _mapper.Map<Dtos.Response.AssetDtoResponse>(asset);

            var user = _userManager.Users.FirstOrDefault(x => x.Id == assetDto.CreatedBy);
            if (user != null)
            {
                assetDto.CreatedBy = user.FirstName + " " + user.LastName;
            }

            user = _userManager.Users.FirstOrDefault(x => x.Id == assetDto.ModifiedBy);
            if (user != null)
            {
                assetDto.ModifiedBy = user.FirstName + " " + user.LastName;

            }


            return Ok(assetDto);
        }

        /// <summary>
        /// Updates asset with given id.
        /// </summary>
        /// <returns></returns>
        [HttpPut]
        [Route("[controller]/{id}")]
        [SwaggerResponse((int)HttpStatusCode.OK)]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, typeof(string))]
        [SwaggerResponse((int)HttpStatusCode.Unauthorized)]
        public IActionResult Update(Guid id, [FromBody]Dtos.Request.AssetDtoRequest assetDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            
            // map dto to entity and set id
            var asset = _mapper.Map<Asset>(assetDto);
            asset.Id = id;

            try
            {
                // save
                _assetService.Update(asset);
                return Ok();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Deletes a specific asset with given id.
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete]
        [Route("[controller]/{id}")]
        [SwaggerResponse((int)HttpStatusCode.OK)]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, typeof(string))]
        [SwaggerResponse((int)HttpStatusCode.Unauthorized)]
        public IActionResult Delete(Guid id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _assetService.Delete(id);
            return Ok();
        }

        /// <summary>
        /// Get all types for asset.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("[controller]/types")]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(List<AssetTypeDtoResponse>))]
        [SwaggerResponse((int)HttpStatusCode.Unauthorized)]
        public IActionResult GetTypes()
        {
            var types = _assetService.GetTypes();
            var typesDto = _mapper.Map<IList<AssetTypeDtoResponse>>(types);

            return Ok(typesDto);
        }

        // Todo: Role superuser
        /// <summary>
        /// Creates new asset type.
        /// </summary>
        /// <param name="assetTypeDto"></param>
        /// <returns></returns>
        [HttpPost]
        [Route("[controller]/types")]
        [SwaggerResponse((int)HttpStatusCode.OK)]
        [SwaggerResponse((int)HttpStatusCode.BadRequest, typeof(string))]
        [SwaggerResponse((int)HttpStatusCode.Unauthorized)]
        public IActionResult CreateType([FromBody] AssetTypeDtoResponse assetTypeDto)
        {
            // map dto to entity
            var assetType = _mapper.Map<AssetType>(assetTypeDto);

            try
            {
                // save
                _assetService.CreateAssetType(assetType);
                return Ok();
            }
            catch (AppException ex)
            {
                // return error message if there was an exception
                return BadRequest(ex.Message);
            }
        }

        /// <summary>
        /// Get list of all available status.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("[controller]/status")]
        [SwaggerResponse((int)HttpStatusCode.OK, typeof(List<AssetStatusDtoResponse>))]
        [SwaggerResponse((int)HttpStatusCode.Unauthorized)]
        public IActionResult GetStatus()
        {
            var status = new List<AssetStatusDtoResponse>();
            var count = 0;
            foreach (var name in Enum.GetNames(typeof(Enums.AssetStatus)))
            {
                var assetStatus = new AssetStatusDtoResponse()
                {
                    Id = count,
                    Name = name
                };

                status.Add(assetStatus);
                count++;
            }
            return Ok(status);
        }
    }
}