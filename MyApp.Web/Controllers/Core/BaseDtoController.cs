using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyApp.Infrastructure.Data;
using MyApp.Web.Filter;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Dynamic;

namespace MyApp.Web.Controllers.Core
{
    //soft delete
    public abstract class BaseDtoController<TEntity, TCreateUpdateDto, TGetDto> : ControllerBase where TEntity : BaseEntity
    {
        private readonly BloggingContext _context;
        private readonly ILogger _logger;
        private readonly IMapper _mapper;
       
        protected BaseDtoController(BloggingContext context, ILogger logger, IMapper mapper)
        {
            _context = context;
            _logger = logger;
            _mapper = mapper;
        }

        
        /// <summary>
        ///GET: api/[controller] 
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [SwaggerOperation(Summary = "Get all entities.")]
        [Produces("application/json")]
        public async Task<ActionResult<IEnumerable<TGetDto>>> Get()
        {
            var data = await _context.Set<TEntity>().Where(p => p.IsDeleted == false).ToListAsync();
            return Ok(ConvertToDtos(data));
        }

        /// <summary>
        /// GET: api/[controller]/{id}
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpGet("{id}")]
        [SwaggerOperation(Summary = "Get a entity.", Description = "Requires id of entity as slug {id}.")]
        [Produces("application/json")]
        public async Task<ActionResult<TGetDto>> Get(int id)
        {
            var entity = await _context.Set<TEntity>().FirstOrDefaultAsync(p => p.Id == id && p.IsDeleted == false);
            if (entity == null)
            {
                return NotFound();
            }
            var getDto = Activator.CreateInstance<TGetDto>();
            _mapper.Map(entity, getDto);
            return Ok(getDto);
        }

        /// <summary>
        /// POST: api/[controller]
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
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

        /// <summary>
        /// PUT: api/[controller]/{id}
        /// </summary>
        /// <param name="id"></param>
        /// <param name="dto"></param>
        /// <returns></returns>
        [HttpPut("{id}")]
        [SwaggerOperation(Summary = "Update a entity.", Description = "Requires id of entity as slug {id}.")]
        [SwaggerResponse(201, "The entity is exist in database.")]
        [SwaggerResponse(400, "The entity is not exist.")]
        [Consumes("application/json")]
        [Produces("application/json")]
        public async Task<IActionResult> Put(int id, TCreateUpdateDto dto)
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

        /// <summary>
        /// DELETE: api/[controller]/{id}
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpDelete("{id}")]
        [SwaggerOperation(Summary = "Delete a entity.", Description = "Requires id of entity as slug {id}.")]
        [SwaggerResponse(201, "The entity is exist in database.")]
        [SwaggerResponse(400, "The entity is not exist.")]
        [Produces("application/json")]
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _context.Set<TEntity>().FirstOrDefaultAsync(p=>p.Id == id && p.IsDeleted==false);
            if (entity == null)
            {
                return NotFound();
            }
            entity.MarkAsDeleted();
            _context.Set<TEntity>().Update(entity);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        /// <summary>
        /// RESTORE: api/[controller]/{id}
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        [HttpPatch("{id}")]
        [SwaggerOperation(Summary = "Restore a entity.", Description = "Requires id of entity as slug {id}.")]
        //[SwaggerResponse(201, "The entity is exist in database.", typeof(Task<IActionResult>))]
        //[SwaggerResponse(400, "The entity is not exist.")]
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

        /// <summary>
        /// GET: api/[controller]/paged?pageNumber=1
        /// </summary>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>&pageSize=10
        [HttpGet("paged")]
        public async Task<ActionResult<IEnumerable<TGetDto>>> GetPaged(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var entities = await _context.Set<TEntity>().Where(p=>p.IsDeleted ==false)
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
        protected List<TGetDto> ConvertToDtos(IEnumerable<TEntity> entity)
        {
            if (typeof(TEntity) != typeof(TGetDto))
            {
                List<TGetDto> dtos = new();
                foreach (var item in entity)
                {
                    var dto = Activator.CreateInstance<TGetDto>();
                    //var dto = ConvertToDto(item);
                    _mapper.Map(item, dto);
                    dtos.Add(dto);
                }
                return dtos;
            }
            else
            {
                return (List<TGetDto>)entity;
            }
        }
        protected TGetDto MapperFrom(TEntity entity)
        {
            var dto = Activator.CreateInstance<TGetDto>();
            foreach (var prop in typeof(TGetDto).GetProperties())
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
