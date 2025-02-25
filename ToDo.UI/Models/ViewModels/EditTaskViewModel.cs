using System.ComponentModel.DataAnnotations;
using ToDo.Domain.DTOs.Task;
using ToDo.Domain.DTOs.User;
using ToDo.Domain.Entities.ValueObjects;

namespace ToDo.UI.Models.ViewModels
{
    public class EditTaskViewModel
    {
        [Required]
        [StringLength(100, MinimumLength = 2)]
        public string Subject { get; set; }

        [Required]
        public int Weeks { get; set; }

        [Required]
        public int Days { get; set; }

        [Required]
        public int Hours { get; set; }

        [Required]
        public int Minutes { get; set; }

        public bool Completed { get; set; }
    }
}
