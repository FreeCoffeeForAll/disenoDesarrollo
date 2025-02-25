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
    public class CreateTaskDTO
    {
        public CreateTaskDTO() { }

        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Subject { get; set; }

        [Required]
        public int UserId { get; set; }

        [Required]
        public int Weeks { get; set; }

        [Required]
        public int Days { get; set; }

        [Required]
        public int Hours { get; set; }

        [Required]
        public int Minutes { get; set; }
    }
}
