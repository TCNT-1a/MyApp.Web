using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.Infrastructure.Data
{
    public abstract class BlogEntity:BaseEntity
    {
        public string Slug { get; set; }
        public bool isPublished { get; set; }
    }
}
