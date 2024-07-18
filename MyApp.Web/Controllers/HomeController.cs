using Microsoft.AspNetCore.Mvc;

namespace MyApp.Web.Controllers
{
    public static class SessionExtension
    {
        public static void Set<T> (this ISession session, string key, T value)
        {
            var val = System.Text.Json.JsonSerializer.Serialize(value);
            session.SetString(key, val);
        }
        public static T Get<T>(this ISession session, string key)
        {
            var value = session.GetString(key);
            if (value == null)
                return default;
            else
                return System.Text.Json.JsonSerializer.Deserialize<T>(value);
        }

    }

    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            if (string.IsNullOrEmpty(HttpContext.Session.GetString("hoten")))
            {
                ViewBag.TrangThai = "Session chua khoi tao";
                ViewBag.HoTen = "";
                ViewBag.Tuoi = "";
                CreateSession();
            }
            else
            {
                ViewBag.TrangThai = "Session duoc khoi tao";
                ViewBag.HoTen = HttpContext.Session.GetString("hoten");
                ViewBag.Tuoi = HttpContext.Session.GetInt32("tuoi");
            }
            return View();
        }
        public void CreateSession()
        {
            HttpContext.Session.SetString("hoten","Nguyen Van A");
            HttpContext.Session.SetInt32("tuoi",32);
        }
    }
}
