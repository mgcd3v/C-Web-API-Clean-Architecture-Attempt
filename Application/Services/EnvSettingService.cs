using Application.Interfaces.Services;

namespace Application.Services
{
    public class EnvSettingService : IAppSettingService
    {
        #region Variables
        #endregion

        #region Public Functions
        public EnvSettingService()
        {
        }

        public T Get<T>(string key, string? node = null)
        {
            string finalKey = (node == null) ? key : node + "." + key;
            return (T)Convert.ChangeType(Environment.GetEnvironmentVariable(finalKey) ?? "", typeof(T));
        }
        #endregion
    }
}
