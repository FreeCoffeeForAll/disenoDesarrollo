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
    public class ListTaskDTO
    {
        public ListTaskDTO() { }

        public ListTaskDTO(int id, string subject, string user, string effort, bool completed) 
        { 
            Id = id;
            Subject = subject;
            User = user;
            Effort = effort;
            Completed = completed;
        }

        public int Id { get; set; }

        public string Subject { get; set; }

        public string User { get; set; }

        public string Effort { get; set; }

        public bool Completed { get; set; }
    }
}
