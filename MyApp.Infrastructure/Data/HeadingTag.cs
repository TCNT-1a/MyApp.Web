using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.Infrastructure.Data
{
    public class HeadingTag : BaseEntity
    {
        public string Title { get; set; }
        public bool NoIndex { get; set; } 
        public string Canonical { get; set; }
        public string MetaDescription { get; set; }
    }
}
