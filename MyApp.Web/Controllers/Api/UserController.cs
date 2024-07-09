using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyApp.Infrastructure.Data;
using MyApp.Infrastructure.Models.User;
using MyApp.Web.Controllers.Core;
using MyApp.Web.Filter;
using MyApp.Web.Models;

namespace MyApp.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : AbsBaseDtoController<User, UserCreateUpdateDto, UserGetDto>
    {
        private readonly BloggingContext _context;
        private readonly IMapper _mapper;
        public UserController(BloggingContext context, ILogger<User> logger, IMapper mapper) : base(context, logger, mapper)
        {
            this._context = context;
        }
        [HttpPost]
        
        [ServiceFilter(typeof(ValidateModelAttribute))]
        
        public override async Task<ActionResult<User>> Post(User entity)
        {
            entity.Password = HashPassword(entity.Password);
            _context.Set<User>().Add(entity);
            await _context.SaveChangesAsync();
            var newEntity = new UserCreateUpdateDto
            {
                FullName = entity.FullName,
                UserName = entity.UserName,
                BirthDate = entity.BirthDate,
                Email = entity.Email
            };
            var obj = 
            CreatedAtAction(nameof(Get), new { id = newEntity.GetType().GetProperty("Id")?.GetValue(newEntity) }, newEntity);
            return obj;
        }

        private static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
    }
}
