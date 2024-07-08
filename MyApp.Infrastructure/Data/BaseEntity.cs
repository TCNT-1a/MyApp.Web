
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace MyApp.Infrastructure.Data
{
    public abstract class BaseEntity
    {
        [Key]
        [JsonIgnore]
        public int Id { get; set; }
        [JsonIgnore]
        public DateTime CreatedDate { get; set; } = DateTime.UtcNow;
        [JsonIgnore]
        public DateTime UpdatedDate { get; set; } = DateTime.UtcNow;
        [JsonIgnore]
        public bool IsDeleted { get; set; } = false;
        public void MarkAsDeleted()
        {
            IsDeleted = true;
            UpdatedDate = DateTime.Now;
        }
        public void Restore()
        {
            IsDeleted = false;
            UpdatedDate = DateTime.Now;
        }
    }
}
