using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ToDo.Application.Contracts.Services.Infrastructure
{
    public interface IGoogleRecaptchaService
    {
        Task<bool> VerifyToken(string token);
    }
}
