using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyApp.Infrastructure.Data;

namespace MyApp.Web.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BlogController : ControllerBase
    {
        private readonly BloggingContext _dbContext;
        public BlogController(BloggingContext context)
        {
            this._dbContext = context;
        }
        [HttpGet("/blog")]
        public  Blog getBlogs()
        {
            var r =  _dbContext.Blogs.FirstOrDefault();
            return r;
        }
    }
}
