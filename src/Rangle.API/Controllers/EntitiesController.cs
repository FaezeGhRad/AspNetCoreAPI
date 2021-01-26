using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json.Linq;
using Rangle.Abstractions.Entities;
using Rangle.Abstractions.Repositories;
using Rangle.API.Models;

[assembly: ApiConventionType(typeof(DefaultApiConventions))]

namespace Rangle.API.Controllers
{
    [ApiController]
    [Authorize]
    [Route("api/entities")]
    [Produces("application/json")]
    public class EntitiesController : ControllerBase
    {
        private readonly IEntityRepository _repository;
        private readonly IMapper _mapper;
        private readonly ILogger<EntitiesController> _logger;

        public EntitiesController(IEntityRepository repository, IMapper mapper, ILogger<EntitiesController> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _mapper = mapper;
            _logger = logger;
        }

        [HttpGet("keys")]
        public async Task<ActionResult<IEnumerable<object>>> GetIds(CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Getting objects Ids");

            IEnumerable<Guid> objectsKeys = await _repository.GetKeys(cancellationToken);

            return Ok(objectsKeys.Select(key => key.ToString()));
        }

        [HttpGet("{id:guid}")]
        public async Task<ActionResult<EntityModel>> Get(Guid id, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Getting an object with id : {id}", id);

            Entity entity = await _repository.Get(id, cancellationToken);

            if (entity == null)
            {
                return NotFound();
            }

            return _mapper.Map<EntityModel>(entity);
        }

        [HttpPost]
        public async Task<ActionResult<EntityModel>> Post([FromBody] object jsonObject, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Creating a new object");

            EntityModel entityModel = new EntityModel { JsonObject = jsonObject };

            Entity entityToCreate = _mapper.Map<Entity>(entityModel);

            Entity createdEntity = await _repository.Create(entityToCreate, cancellationToken);

            entityModel.Id = createdEntity.Id;

            return CreatedAtAction("Get", new { id = createdEntity.Id }, entityModel);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<EntityModel>> Put(Guid id, [FromBody] object jsonObject, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Updating a new object");

            Entity entity = await _repository.Get(id, cancellationToken);
            if (entity == null)
            {
                return NotFound();
            }

            // TODO : this logic should not be here , it can go to converter
            // remove internal _id property if exists
            JObject jObject = JObject.FromObject(jsonObject);
            jObject.Remove("_id");
            jsonObject = jObject.ToObject<object>();

            EntityModel entityModel = new EntityModel { Id = entity.Id, JsonObject = jsonObject };

            Entity entityToUpdate = _mapper.Map<Entity>(entityModel);

            await _repository.Update(entityToUpdate, cancellationToken);

            return entityModel;
        }


        [HttpDelete("{id:guid}")]
        public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken = default)
        {
            _logger.LogInformation("Deleting an object with id : {id}", id);

            Entity entity = await _repository.Get(id, cancellationToken);

            if (entity == null)
            {
                return NotFound();
            }

            await _repository.Delete(id, cancellationToken);

            return NoContent();
        }
    }
}
