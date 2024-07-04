using Microsoft.AspNetCore.Mvc;
using MyApp.Web.Models;
using MyApp.Web.Models.Account;

namespace MyApp.Web.Controllers
{
    public class AccountController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Login(LoginModel loginModel,string returnUrl)
        {
            var tokenProvider = new TokenProvider();
            ViewData["ReturnUrl"] = returnUrl;
            var model = new LoginModel();
            return View(model);
        }
    }
}
