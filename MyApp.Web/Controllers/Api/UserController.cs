using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyApp.Infrastructure.Data;
using MyApp.Infrastructure.Models.User;
using MyApp.Web.Controllers.Core;
using MyApp.Web.Filter;
using MyApp.Web.Helper;
using MyApp.Web.Models;
using System;
namespace MyApp.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : BaseDtoController<User, UserCreateUpdateDto, UserGetDto>
    {
        private readonly BloggingContext _context;
        private readonly IMapper _mapper;
        public UserController(BloggingContext context, ILogger<User> logger, IMapper mapper) : base(context, logger, mapper)
        {
            this._context = context;
            this._mapper = mapper;
        }


        [HttpPost]
        [ServiceFilter(typeof(ValidateModelAttribute))]
        public override async Task<ActionResult<User>> Post(User entity)
        {
            entity.Password = PasswordHelper.HashPassword(entity.Password);
            this._context.Users.Add(entity);
            await this._context.SaveChangesAsync();
            var newEntity = new UserCreateUpdateDto();
            _mapper.Map(entity, newEntity);
            var id = new { id = entity.GetType().GetProperty("Id")?.GetValue(entity) };
            var obj =
            CreatedAtAction(nameof(Get), id, newEntity);
            return obj;
        }

        private static string HashPassword(string password)
        {
            return BCrypt.Net.BCrypt.HashPassword(password);
        }
    }
}

