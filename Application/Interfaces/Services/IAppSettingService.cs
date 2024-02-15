namespace Application.Interfaces.Services
{
    public interface IAppSettingService
    {
        public T Get<T>(string key, string? node = null);
    }
}
