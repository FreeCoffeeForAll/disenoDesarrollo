using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using ToDo.Domain.Annotations;
using ToDo.Domain.Entities.ValueObjects;

namespace ToDo.Domain.Entities
{
    public class Task
    {
        public Task() { }

        [Key]
        [CacheKey]
        public int Id { get; private set; }

        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Subject { get; private set; }

        [Required]
        [ForeignKey("User")]
        public int UserId { get; private set; }

        public User User { get; private set; }

        [Required]
        public Effort Effort { get; private set; }

        public bool Completed { get; private set; }

        [Required]
        public DateTime CreationDate { get; private set; }

        public DateTime ChangeDate { get; private set; }

        public static Task Create(string subject, int userId, Effort effort)
        {
            return
                new Task()
                {
                    Subject = subject,
                    UserId = userId,
                    Effort = effort,
                    CreationDate = DateTime.Now,
                    ChangeDate = DateTime.Now,
                };
        }

        public void Update(string subject, Effort effort, bool completed)
        {
            Subject = subject;
            Effort = effort;
            Completed = completed;
            ChangeDate = DateTime.Now;
        }

    }
}
