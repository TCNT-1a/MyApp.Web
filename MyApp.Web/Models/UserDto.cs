using MyApp.Infrastructure.Data;
using System.ComponentModel.DataAnnotations;

namespace MyApp.Web.Models
{
    public class UserDto
    {
        [Required(ErrorMessage = "FullName is required")]
        public string FullName { get; set; }
        [Required(ErrorMessage = "UserName is required")]
        public string UserName { get; set; }
        [Required]
        public string Email { get; set; }
        public DateTime BirthDate { get; set; }
    }
}
