using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MyApp.Infrastructure.Data;
using MyApp.Web.Controllers.Core;
using MyApp.Web.Helper;
using MyApp.Web.Models.Account;

namespace MyApp.Web.Controllers
{
    //[Route("taikhoan")]
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

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
        //
        // [HttpPost]
        // public IActionResult Login([FromForm] LoginModel loginModel, [FromQuery] string? returnUrl)
        // {
        //     if (ModelState.IsValid)
        //     {
        //         var tokenProvider = new TokenProvider(_context);
        //         var token = tokenProvider.LoginUser(loginModel.UserName, loginModel.Password, true);
        //         if (!string.IsNullOrEmpty(token))
        //         {
        //             if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
        //             {
        //                 return Redirect(returnUrl);
        //             }
        //             else
        //                 return RedirectToAction("Index", "Home");
        //         }
        //     }
        //     return View(loginModel);
        // }

        // //
        [HttpPost]
        public async Task<IActionResult> Login([FromForm] LoginModel loginModel, [FromQuery] string? returnUrl)
        {
            if (ModelState.IsValid)
            {
                var tokenProvider = new TokenProvider(_context);
                var claims = tokenProvider.GetClaimUser(loginModel.UserName, loginModel.Password, true);
                var claimsIdentity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
                var authProperties = new AuthenticationProperties
                {
                    IsPersistent = loginModel.RememberMe
                };

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(claimsIdentity), authProperties);
                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }
                else
                    return RedirectToAction("Index", "Home");

            }
            return View(loginModel);
        }
    }

}
