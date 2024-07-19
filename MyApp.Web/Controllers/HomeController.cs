using Microsoft.AspNetCore.Mvc;
using MyApp.Web.Helper;
namespace MyApp.Web.Controllers
{
    

    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("hoten")))
            {
                ViewBag.TrangThai = "Session chua khoi tao";
                ViewBag.HoTen = "";
                ViewBag.Tuoi = "";
                ViewBag.ThoiGian = "";
                HttpContext.Session.Set<string>("hoten","Nguyen Van A");
                HttpContext.Session.Set<int>("tuoi", 32);
                HttpContext.Session.Set<DateTime>("thoigian", DateTime.Now.ToLocalTime());
                
            }
            else
            {
                ViewBag.TrangThai = "Session duoc khoi tao";
                ViewBag.HoTen = HttpContext.Session.Get<string>("hoten");
                ViewBag.Tuoi = HttpContext.Session.Get<int>("tuoi");
                ViewBag.ThoiGian = HttpContext.Session.Get<DateTime>("thoigian");
            }
            return View();
        }
    }
}
