using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyApp.Infrastructure.Data;
using MyApp.Web.Filter;
using System;

namespace MyApp.Web.Controllers.Core
{
    //hard delete
    public abstract class BaseController<TEntity > : ControllerBase where TEntity : class
    {
        private readonly BloggingContext _context;
        private readonly ILogger _logger;
        protected BaseController(BloggingContext context, ILogger logger)
        {
            _context = context;
            _logger = logger;
        }

        // GET: api/[controller]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<TEntity>>> Get()
        {
            return await _context.Set<TEntity>().ToListAsync();
        }

        // GET: api/[controller]/5
        [HttpGet("{id}")]
        public async Task<ActionResult<TEntity>> Get(int id)
        {
            var entity = await _context.Set<TEntity>().FindAsync(id);
            if (entity == null)
            {
                return NotFound();
            }

            return entity;
        }

        // POST: api/[controller]
        [HttpPost]
        [ServiceFilter(typeof(ValidateModelAttribute))]
        public async Task<ActionResult<TEntity>> Post(TEntity entity)
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

    }
}
