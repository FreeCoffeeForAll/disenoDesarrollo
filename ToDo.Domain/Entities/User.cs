using System.ComponentModel.DataAnnotations;
using ToDo.Domain.Annotations;

namespace ToDo.Domain.Entities
{
    public class User
    {
        [Key]
        [CacheKey]
        public int Id { get; set; }

        [Required]
        [StringLength(50, MinimumLength = 2)]
        public string Name { get; set; }

        public string Email { get; set; }

        public List<Permission> Permissions { get; set; }
    }
}
