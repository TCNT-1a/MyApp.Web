using Microsoft.AspNetCore.Mvc;
using MyApp.Infrastructure.Data;
using MyApp.Web.Helper;
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
        public IActionResult Login(string returnUrl)
        {
            ViewData["ReturnUrl"] = returnUrl;
            var model = new LoginModel();
            return View(model);
        }
        [HttpPost]
        public IActionResult Login(LoginModel loginModel, string returnUrl="")
        {
            if (ModelState.IsValid)
            {
                var tokenProvider = new TokenProvider(_context);
                var token = tokenProvider.LoginUser(loginModel.TenDangNhap, loginModel.MatKhau, true);
                if(token != "")
                {
                    if(returnUrl == "")
                    {
                        //return Redirect("~/Home/Index");
                        return RedirectToAction("Index","Home");
                    }
                        
                    return Redirect(returnUrl);
                }
            }
            return View(loginModel);
        }
    }
}
