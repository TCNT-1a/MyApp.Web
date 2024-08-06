using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using MyApp.Infrastructure.Data;
using MyApp.Web.Controllers.Core;

namespace MyApp.Web.Controllers
{
    [Authorize(Roles = "user")]
    public class CategoryController : Controller
    {
        private BloggingContext _context;
        public CategoryController(BloggingContext context)
        {
            this._context = context;
        }
        public IActionResult Index()
        {
            ViewData["title"] = "Category";
            var model = _context.Categories.ToList();

            return View(model);
        }

    }
}
