using Microsoft.EntityFrameworkCore.Query;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDo.Application.Contracts.Cache
{
    public interface ICacheStore
    {
        void Add<T>(T item, ICacheKey<T> key) 
            where T : class;

        T Get<T>(ICacheKey<T> key)
            where T : class;
    }
}
