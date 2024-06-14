using System.ComponentModel.DataAnnotations.Schema;

namespace MyApp.Infrastructure.Data
{
    [Table("Blog")]
    public class Blog
    {
        public int BlogId { get; set; }
        public string Url { get; set; }
    }
}
