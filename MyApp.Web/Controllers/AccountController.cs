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
            
            ViewData["Title"] = "Login";
            ViewData["Id"] = "UserName";
            ViewData["Placeholder"] = "UserName";
            ViewData["submit"] = "Submit";
            
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
        public IActionResult XuatCauChao2(int n=2, string ten = "Nguyen")
        {
            ViewData["CauChao"] = "Xin chao, toi la "+ ten;
            ViewData["SoLan"] = n;
            return View();
        }
        public IActionResult XuatCauChao()
        {
            var model = new LoginModel()
            {
                TenDangNhap="Nguyen Van BBB",
                MatKhau="12345"
            };
            return View(model);
        }
        public IActionResult ChucMungSinhNhat(string hoten = "nguyen", int birthyear = 5)
        {
            //var model = new UserInfor(){
            //    Name = hoten,
            //    BirthYear = birthyear
            //};
            var model = new UserInfor();
            return View(model);
        }
        [HttpPost]
        public IActionResult ChucMungSinhNhat(UserInfor user)
        {
            //var model = new UserInfor(){
            //    Name = hoten,
            //    BirthYear = birthyear
            //};
            var model = new UserInfor();

            return View(user);
        }
    }
}
