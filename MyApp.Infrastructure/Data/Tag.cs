using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.Infrastructure.Data
{
    public class Tag: BlogEntity
    {
        public string Name { get; set; }
        public HeadingTag HeadingTag { get; set; }
        public virtual List<Post> Posts { get; set; }
    }
}
