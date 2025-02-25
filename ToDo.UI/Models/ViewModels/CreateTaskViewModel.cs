using ToDo.Domain.DTOs.Task;
using ToDo.Domain.DTOs.User;

namespace ToDo.UI.Models.ViewModels
{
    public class CreateTaskViewModel
    {
        public CreateTaskViewModel() 
        {
            Task = new CreateTaskDTO();
            Users = new List<ListUserDTO>();
        }

        public List<ListUserDTO> Users { get; set; }

        public CreateTaskDTO Task { get; set; }
    }
}
