using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using ToDo.Application.Contracts.Services.Infrastructure;
using ToDo.Domain.Configurations;
using ToDo.Domain.PlainModels;

namespace ToDo.Infrastructure.Services
{
    public class GoogleRecaptchaService : IGoogleRecaptchaService
    {
        private readonly GoogleRecaptcha _settings;

        public GoogleRecaptchaService(IOptions<GoogleRecaptcha> options)
        {
            _settings = options.Value;
        }

        public async Task<bool> VerifyToken(string token)
        {
            var url = $"{_settings.VerifyUrl}?secret={_settings.SecretKey}&response={token}";

            try
            { 
                using(HttpClient client = new HttpClient()) 
                { 
                    var result = await client.GetAsync(url);
                    if (!result.IsSuccessStatusCode) 
                    {
                        return false;
                    }

                    var content = await result.Content.ReadAsStringAsync();
                    var response = JsonSerializer.Deserialize<GoogleRecaptchaResponse>(content);

                    return response.Success && response.Score > 0.5F;
                }
            }
            catch
            { return false; }
        }
    }
}
