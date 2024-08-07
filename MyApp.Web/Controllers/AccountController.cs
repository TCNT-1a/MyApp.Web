using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using MyApp.Infrastructure.Data;
using MyApp.Web.Helper;
using MyApp.Web.Models.Account;

namespace MyApp.Web.Controllers
{
    //[Route("taikhoan")]
    public class AccountController : Controller
    {
        private readonly BloggingContext _context;
        public AccountController(BloggingContext context)
        {
            this._context = context;
        }
        public IActionResult Index()
        {
            return View();
        }
        //[Route("dangnhap")]
        public IActionResult Login(string returnUrl = "")
        {
            ViewData["ReturnUrl"] = returnUrl;
            var model = new LoginModel();
            return View(model);
        }
        [HttpPost]
        public IActionResult Login([FromForm] LoginModel loginModel, [FromQuery] string? returnUrl)
        {
            if (ModelState.IsValid)
            {
                var tokenProvider = new TokenProvider(_context);
                var token = tokenProvider.LoginUser(loginModel.UserName, loginModel.Password, true);
                if (!string.IsNullOrEmpty(token))
                {
                    if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                    {
                        return Redirect(returnUrl);
                    }
                    else
                        return RedirectToAction("Index", "Home");
                }
            }
            return View(loginModel);
        }
    }

}
