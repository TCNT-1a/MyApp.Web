using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MyApp.Infrastructure.Data
{
    public class Post: BaseEntity
    {
        public string Title { get; set; }
        public int PostView { get; set; }
        public string Content { get; set; }
        public virtual User Author { get; set; }
        public virtual List<Tag> Tags { get; set; }
        public virtual Category Category { get; set; }
        public virtual HeadingTag HeadingTag { get; set; }
    }
}
