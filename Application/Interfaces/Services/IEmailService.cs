using Shared.Helpers;

namespace Application.Interfaces.Services
{
    public interface IEmailService
    {
        Task SendVerificationCode(string emailAddress, string code, EmailSetting? emailSetting = null);
    }
}
