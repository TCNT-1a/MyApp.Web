using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyApp.Infrastructure.Data;
using MyApp.Web.Filter;
using Swashbuckle.AspNetCore.Annotations;

using System;

namespace MyApp.Web.Controllers.Core
{
    //soft delete
    public abstract class AbsBaseDtoController<TEntity, TDto> : ControllerBase where TEntity : BaseEntity
    {
        private readonly BloggingContext _context;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;

        protected AbsBaseDtoController(BloggingContext context, ILogger logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        // GET: api/[controller]
        [HttpGet]
        [SwaggerOperation(Summary = "Get all entities.")]
        [Produces("application/json")]
        public async Task<ActionResult<IEnumerable<TDto>>> Get()
        {
            var data = await _context.Set<TEntity>().Where(p => p.IsDeleted == false).ToListAsync();
            return Ok(ConvertToDtos(data));
        }

        // GET: api/[controller]/{id}
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get a entity.", Description = "Requires id of entity as slug {id}.")]
        [SwaggerResponse(201, "The entity is exist in database.", typeof(IActionResult))]
        [SwaggerResponse(400, "The entity is not exist.")]
        [Produces("application/json")]
        public async Task<ActionResult<TDto>> Get(int id)
        {
            var entity = await _context.Set<TEntity>().FirstOrDefaultAsync(p => p.Id == id && p.IsDeleted == false);
            if (entity == null)
            {
                return NotFound();
            }
            return Ok(ConvertToDto(entity));
        }

        // POST: api/[controller]
        [HttpPost]
        [ServiceFilter(typeof(ValidateModelAttribute))]
        [SwaggerOperation(Summary = "Create a entity.", Description = "Requires entity object as POST body. .")]
        [Consumes("application/json")]
        [Produces("application/json")]
        public virtual async Task<ActionResult<TEntity>> Post(TEntity entity)
        {
            _context.Set<TEntity>().Add(entity);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(Get), new { id = entity.GetType().GetProperty("Id")?.GetValue(entity) }, entity);
        }

        // PUT: api/[controller]/{id}
        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Update a entity.", Description = "Requires id of entity as slug {id}.")]
        [SwaggerResponse(201, "The entity is exist in database.")]
        [SwaggerResponse(400, "The entity is not exist.")]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<IActionResult> Put(int id, TDto dto)
        {
            var entity = _context.Set<TEntity>().FirstOrDefault(p => p.Id == id && p.IsDeleted == false);
            if(entity == null)
                return NotFound();
            _mapper.Map(dto, entity);
            _context.Entry(entity).State = EntityState.Modified;
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return BadRequest();
            }
            return NoContent();
        }

        // DELETE: api/[controller]/{id}
        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Delete a entity.", Description = "Requires id of entity as slug {id}.")]
        [SwaggerResponse(201, "The entity is exist in database.")]
        [SwaggerResponse(400, "The entity is not exist.")]
        //[Consumes("application/json")]
        [Produces("application/json")]
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _context.Set<TEntity>().FindAsync(id);
            if (entity == null)
            {
                return NotFound();
            }
            entity.MarkAsDeleted();
            _context.Set<TEntity>().Update(entity);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // RESTORE: api/[controller]/{id}
        [HttpPatch("{id}")]
        [SwaggerOperation(Summary = "Restore a entity.", Description = "Requires id of entity as slug {id}.")]
        [SwaggerResponse(201, "The entity is exist in database.", typeof(IActionResult))]
        [SwaggerResponse(400, "The entity is not exist.")]
        [Produces("application/json")]
        public async Task<IActionResult> Restore(int id)
        {
            var entity = await _context.Set<TEntity>().FindAsync(id);
            if (entity == null)
            {
                return NotFound();
            }
            entity.Restore();
            _context.Set<TEntity>().Update(entity);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/[controller]/paged?pageNumber=1&pageSize=10
        [HttpGet("paged")]
        public async Task<ActionResult<IEnumerable<TDto>>> GetPaged(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var entities = await _context.Set<TEntity>()
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                return Ok(ConvertToDtos(entities));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting paged entities.");
                return StatusCode(500, "Internal server error");
            }
        }

        private bool EntityExists(int id)
        {
            return _context.Set<TEntity>().FirstOrDefault(p => p.Id == id && p.IsDeleted == false) != null;
        }
        protected List<TDto> ConvertToDtos(IEnumerable<TEntity> entity)
        {
            if (typeof(TEntity) != typeof(TDto))
            {
                List<TDto> dtos = new();
                foreach (var item in entity)
                {
                    var dto = Activator.CreateInstance<TDto>();
                    foreach (var prop in typeof(TDto).GetProperties())
                    {
                        var entityProp = typeof(TEntity).GetProperty(prop.Name);
                        if (entityProp != null && prop.CanWrite && entityProp.CanRead)
                        {
                            var value = entityProp.GetValue(item, null);
                            prop.SetValue(dto, value, null);
                        }
                    }
                    dtos.Add(dto);
                }
                return dtos;

            }
            else
            {
                return (List<TDto>)entity;
            }

        }
        protected TDto ConvertToDto(TEntity entity)
        {

            var dto = Activator.CreateInstance<TDto>();
            foreach (var prop in typeof(TDto).GetProperties())
            {
                var entityProp = typeof(TEntity).GetProperty(prop.Name);
                if (entityProp != null && prop.CanWrite && entityProp.CanRead)
                {
                    var value = entityProp.GetValue(entity, null);
                    prop.SetValue(dto, value, null);
                }
            }
            return dto;
        }

    }
}
