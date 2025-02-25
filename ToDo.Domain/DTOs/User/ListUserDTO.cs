using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDo.Domain.DTOs.User
{
    public class ListUserDTO
    {
        public ListUserDTO() { }

        public ListUserDTO(int id, string name) 
        { 
            Id = id;
            Name = name;
        }

        public int Id { get; set; }

        public string Name { get; set; }
    }
}
