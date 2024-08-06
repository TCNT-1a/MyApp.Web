using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace MyApp.Web.Models.Account
{
    public class LoginModel
    {
        [Required(ErrorMessage = "Vui lòng nhập Tên đăng nhập")]
        [Display(Name = "UserName")]
        public string UserName { get; set; }


        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        [Required(ErrorMessage = "Vui lòng nhập Mật khẩu")]
        public string Password { get; set; }
        [Display(Name = "Remember")]
        public bool RememberMe { get; set; }
        public string? ReturnUrl { get; set; }
    }
}
