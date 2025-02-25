using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ToDo.Application.Contracts.Cache;
using ToDo.Domain.Annotations;

namespace ToDo.Application.Cache
{
    public class CacheKey<T> : ICacheKey<T>
        where T : class
    {
        private readonly string _prefix;

        public CacheKey() { }

        public CacheKey(string prefix) 
        {
            _prefix = prefix;
        }

        public string Key
        {
            get
            {
                var result = string.Empty;
                var type = typeof(T);

                var properties =
                    type.GetProperties()
                        .Where(w => w.IsDefined(typeof(CacheKeyAttribute), true));

                result =
                    string.Concat(_prefix ?? string.Empty, "_", 
                        string.Join("_", properties.SelectMany(s => s.Name)));

                return result;
            }
        }

        public override string ToString()
        {
            return Key;
        }
    }
}
