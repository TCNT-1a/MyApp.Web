using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyApp.Infrastructure.Data;
using MyApp.Web.Filter;
using System;

namespace MyApp.Web.Controllers
{
    public abstract class BaseDtoController<TEntity, TDto> : ControllerBase where TEntity : class
    {
        private readonly BloggingContext _context;
        private readonly ILogger _logger;
        protected BaseDtoController(BloggingContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/[controller]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TDto>>> Get()
        {
            var data = await _context.Set<TEntity>().ToListAsync();
            return Ok(ConvertToDtos(data));
        }

        // GET: api/[controller]/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TDto>> Get(int id)
        {
            var entity = await _context.Set<TEntity>().FindAsync(id);
            if (entity == null)
            {
                return NotFound();
            }
            return Ok(ConvertToDto(entity));
        }

        // POST: api/[controller]
        [HttpPost]
        [ServiceFilter(typeof(ValidateModelAttribute))]
        public virtual async Task<ActionResult<TEntity>> Post(TEntity entity)
        {
            _context.Set<TEntity>().Add(entity);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(Get), new { id = entity.GetType().GetProperty("Id")?.GetValue(entity) }, entity);
        }

        // PUT: api/[controller]/5
        [HttpPut("{id}")]
        public async Task<IActionResult> Put(int id, TEntity entity)
        {
            if ((int)entity.GetType().GetProperty("Id")?.GetValue(entity) != id)
            {
                return BadRequest();
            }

            _context.Entry(entity).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EntityExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }
            return NoContent();
        }

        // DELETE: api/[controller]/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var entity = await _context.Set<TEntity>().FindAsync(id);
            if (entity == null)
            {
                return NotFound();
            }

            _context.Set<TEntity>().Remove(entity);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/[controller]/paged?pageNumber=1&pageSize=10
        [HttpGet("paged")]
        public async Task<ActionResult<IEnumerable<TEntity>>> GetPaged(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                var entities = await _context.Set<TEntity>()
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                return Ok(entities);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while getting paged entities.");
                return StatusCode(500, "Internal server error");
            }
        }

        private bool EntityExists(int id)
        {
            return _context.Set<TEntity>().Find(id) != null;
        }
        protected List<TDto> ConvertToDtos(IEnumerable<TEntity> entity)
        {
            if (typeof(TEntity) != typeof(TDto))
            {
                List<TDto> dtos = new List<TDto>();
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
