using Microsoft.AspNetCore.Mvc;
using MyApp.Infrastructure.Data;
using MyApp.Web.Models;
using MyApp.Web.Models.Account;

namespace MyApp.Web.Controllers
{
    public class AccountController : Controller
    {
        private BloggingContext _context;
        public AccountController(BloggingContext context)
        {
            this._context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Login(LoginModel loginModel,string returnUrl)
        {
            var tokenProvider = new TokenProvider(_context);
            tokenProvider.LoginUser(loginModel.TenDangNhap, loginModel.MatKhau, true);

            ViewData["ReturnUrl"] = returnUrl;
            var model = new LoginModel();
            return View(model);
        }
    }
}
