﻿using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core.Entities;
using Core.Interfaces.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.Extensions.Logging;

namespace RestApi.Controllers
{
    [Produces("application/json","application/xml")]
    [Consumes("application/json", "application/xml")]
    [Route("api/[controller]")]
    [ApiController]
    public class ServicesController : ControllerBase
    {
        private readonly IServiceRepository _serviceRepository;
        private readonly ILogger _logger;

        public ServicesController(IServiceRepository serviceRepository, 
            ILogger<ServicesController> logger)
        {
            _serviceRepository = serviceRepository;
            _logger = logger;
        }

        /// <summary>
        /// Get all services
        /// </summary>
        /// <returns>All services</returns>
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Service>>> Get()
        {
            try
            {
                _logger.LogInformation("Getting all service");

                var services = await _serviceRepository.Get();

                if (services == null) return NotFound();

                return Ok(services);
            }
            catch (Exception e)
            {
                _logger.LogError("Error during getting all services", e);
            }

            return BadRequest();
        }

        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        /// <response code="200">Returns the requested service</response>
        [HttpGet("{id}", Name = "ServiceGet")]
        public async Task<ActionResult<Service>> Get(int id)
        {
            try
            {
                _logger.LogInformation($"Getting service with id={id}");

                var service = await _serviceRepository.GetById(id);

                if (service == null) return NotFound();

                return Ok(service);
            }
            catch(Exception e)
            {
                _logger.LogError($"Error during getting service with id={id}", e);
            }

            return BadRequest();
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody]Service service)
        {            
            try
            {
                _logger.LogInformation("Adding new service.");

                await _serviceRepository.Add(service);
                var url = Url.Link("ServiceGet", new { id = service.Id });
                return Created(url, service);
            }
            catch(Exception e)
            {
                _logger.LogError("Error during adding new service", e);
            }

            return BadRequest();
        }

        [HttpPatch("{id}")]
        [HttpPut("{id}")]
        public async Task<ActionResult<Service>> Put(int id, [FromBody]Service service)
        {
            try
            {
                _logger.LogInformation($"Updating service with id={id}");

                var oldService = await _serviceRepository.GetById(id);
                if (oldService == null) return NotFound();

                oldService.Name = service.Name ?? oldService.Name;
                oldService.Description = service.Description ?? oldService.Description;

                await _serviceRepository.Update(oldService);
                return Ok(oldService);
            }
            catch (Exception e)
            {
                _logger.LogError($"Error during updating service with id={id}", e);
            }

            return BadRequest();
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            try
            {
                _logger.LogInformation($"Deleting service with id={id}");

                var service = await _serviceRepository.GetById(id);
                if (service == null)
                    return NotFound();

                await _serviceRepository.Delete(service);

                return Ok();
            }
            catch (Exception e)
            {
                _logger.LogError($"Error during delete service with id={id}", e);
            }

            return BadRequest();
        }
    }
}