using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.Infrastructure.Models.User
{
    public class UserCreateUpdateDto
    {
        [Required(ErrorMessage = "FullName is required")]
        public string FullName { get; set; }
        [Required(ErrorMessage = "UserName is required")]
        public string UserName { get; set; }
        [Required]
        public string Email { get; set; }
        public DateTime BirthDate { get; set; }
        public string Role { get; set; }
    }
}
