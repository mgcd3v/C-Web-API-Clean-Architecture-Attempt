using Application.Interfaces.Services;
using Microsoft.Extensions.Configuration;

namespace Application.Services
{
    public class AppSettingService : IAppSettingService
    {
        #region Variables
        protected readonly IConfiguration _configuration;
        #endregion

        #region Public Functions
        public AppSettingService() 
        {
            _configuration = new ConfigurationBuilder()
                                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                                .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
                                .Build();
        }

        public T? Get<T>(string key, string? node = null)
        {
            string finalKey = (node == null) ? key : node + ":" + key;
            return _configuration.GetSection(finalKey).Get<T>();
        }
        #endregion
    }
}
