
using Microsoft.AspNetCore.Mvc;
using MyApp.Web.Helper;
using MyApp.Web.Models.Account;

namespace MyApp.Web.Controllers
{
    public class TestBindingController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        
        public IActionResult NoBinding() {

            if (Request?.Method =="POST")
            {

                var name = Request.Form["name"];
                var birthyear = Request.Form["birthyear"];
                ViewData["name"] = name;
                ViewData["birthyear"] = birthyear;
            }
            return View(); 
        }
        public IActionResult SimpleBinding(string name, string birthyear)
        {
            ViewData["name"] = name;
            ViewData["birthyear"] = birthyear;
            return View();
        }
        public IActionResult ClassBinding(UserInfor user)
        {
            var u = new UserInfor();
            if(user != null)
            {
                Mapper.PropertyCoppier<UserInfor, UserInfor>.Copy(user, u);
            }
            return View(user);
        }

    }
}
