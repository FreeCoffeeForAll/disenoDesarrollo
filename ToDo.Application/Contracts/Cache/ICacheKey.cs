using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDo.Application.Contracts.Cache
{
    public interface ICacheKey<T> 
    {
        string Key { get; }
    }
}
