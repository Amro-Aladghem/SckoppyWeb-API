using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Service
{
    public class AppSettingsService
    {
        private readonly IConfiguration _configuration;

        public  AppSettingsService(IConfiguration configuration)
        {
            _configuration = configuration;
        }
        public string baseURL => _configuration.GetSection("ApiBaseUrl").Value;
    }
}
