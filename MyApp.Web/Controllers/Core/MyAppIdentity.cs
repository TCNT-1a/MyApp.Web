using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace MyApp.Web.Controllers.Core
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; }
        public DateTime BirthDate { get; set; }
    }

    public class MyAppDbContext : IdentityDbContext<ApplicationUser>
    {
        public MyAppDbContext(DbContextOptions<MyAppDbContext> options)
            : base(options)
        {
        }
    }
}
