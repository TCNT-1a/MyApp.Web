using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace MyApp.Web.Models
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Vui lòng nhập Tên đăng nhập")]
        [Display(Name = "Tên đăng nhập")]
        public string TenDangNhap { get; set; }
        [DataType(DataType.Password)]
        [Display(Name = "Mật khẩu")]
        [Required(ErrorMessage = "Vui lòng nhập Mật khẩu")]
        public string MatKhau { get; set; }
        [Display(Name = "Remember?")]
        public bool GhiNho { get; set; }
    }
}
