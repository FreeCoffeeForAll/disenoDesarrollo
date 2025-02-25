using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDo.Domain.Entities.ValueObjects;
using ToDo.Domain.Entities;

namespace ToDo.Domain.DTOs.Task
{
    public class EditTaskDTO
    {
        public EditTaskDTO()
        {
            ChangeDate = DateTime.Now;
        }

        public EditTaskDTO(int id, string subject, Effort effort, bool completed)
            : this()
        {
            Id = id;
            Subject = subject;
            Effort = effort;
            Completed = completed;
        }

        [Required]
        public int Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Subject { get; private set; }

        [Required]
        public Effort Effort { get; private set; }

        public bool Completed { get; set; }

        [Required]
        public DateTime ChangeDate { get; private set; }
    }
}
