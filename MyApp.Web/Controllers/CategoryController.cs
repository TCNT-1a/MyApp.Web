using Microsoft.AspNetCore.Mvc;
using MyApp.Infrastructure.Data;
using MyApp.Web.Controllers.Core;

namespace MyApp.Web.Controllers
{
    public class CategoryController : Controller
    {
        private BloggingContext _context;
        public CategoryController(BloggingContext context)
        {
            this._context = context;
        }
        public IActionResult Index()
        {
            return View();
        }

    }
}
