using Newtonsoft.Json;
using System.ComponentModel.DataAnnotations.Schema;

namespace MyApp.Infrastructure.Data
{
    [Table("Blog")]
    public class Blog: BaseEntity
    {
        public string Url { get; set; }
        public string Content { get; set; }
    }
}
