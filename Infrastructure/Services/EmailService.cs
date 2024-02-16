using Application.Interfaces.Services;
using Application.Services;
using Domain.Interfaces.Repositories;
using Shared.Helpers;
using System.Net;
using System.Net.Mail;
using System.Text;

namespace Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        #region Variables
        protected readonly IAppSettingService _appSettingService;

        private readonly EmailSetting _emailSetting;
        #endregion

        #region Public Functions
        public EmailService(IAppSettingService appSettingService)
        {
            _appSettingService = new EnvSettingService();

            _emailSetting = new()
            {
                SenderAddress = _appSettingService.Get<string>("SenderAddress", "SMTPConfig"),
                SenderDisplayName  = _appSettingService.Get<string>("SenderDisplayName", "SMTPConfig"),
                Host = _appSettingService.Get<string>("Host", "SMTPConfig"),
                Port = _appSettingService.Get<int>("Port", "SMTPConfig"),
                UserName  = _appSettingService.Get<string>("UserName", "SMTPConfig"),
                Password  = _appSettingService.Get<string>("Password", "SMTPConfig"),
                EnableSSL  = _appSettingService.Get<bool>("EnableSSL", "SMTPConfig"),
                UseDefaultCredentials  = _appSettingService.Get<bool>("UseDefaultCredentials", "SMTPConfig"),
                IsBodyHTML  = _appSettingService.Get<bool>("IsBodyHTML", "SMTPConfig")
            };
        }

        public async Task SendVerificationCode(string emailAddress, string code, EmailSetting? emailSetting = null)
        {
            var forgotPasswordVerificationAliveMinutes = _appSettingService.Get<string>("AliveMinutes", "ForgotPasswordVerification");
            var templateFileName = Path.Combine(Directory.GetCurrentDirectory(), Constants.EMAILTEMPLATEFOLDERNAME, string.Format(@"{0}", Constants.SENDCODETEMPLATEFILENAME));
            var placeHolders = new List<KeyValuePair<string, string>>()
            {
                new KeyValuePair<string, string>("{{Code}}", code),
                new KeyValuePair<string, string>("{{MinutesAlive}}", forgotPasswordVerificationAliveMinutes)
            };
            var finalBody = GetFinalBodyValue(GetEmailBody(templateFileName), placeHolders);
            var emailOptions = new EmailOptions
            {
                Subject = Constants.EMAILVERIFICATIONCODESUBJECT,
                Body = finalBody,
                ToEmails = new List<string> { emailAddress }
            };
            await SendEmail(emailOptions, emailSetting);
        }
        #endregion

        #region Private Functions
        private async Task SendEmail(EmailOptions emailOptions, EmailSetting? emailSetting = null)
        {
            var senderAddress = (emailSetting != null)? emailSetting.SenderAddress : _emailSetting.SenderAddress;
            var senderDisplayName = (emailSetting != null) ? emailSetting.SenderDisplayName : _emailSetting.SenderDisplayName;
            var host = (emailSetting != null) ? emailSetting.Host : _emailSetting.Host;
            var port = (emailSetting != null) ? emailSetting.Port : _emailSetting.Port;
            var userName = (emailSetting != null) ? emailSetting.UserName : _emailSetting.UserName;
            var password = (emailSetting != null) ? emailSetting.Password : _emailSetting.Password;
            var enableSsl = (emailSetting != null) ? emailSetting.EnableSSL : _emailSetting.EnableSSL;
            var useDefaultCredentials = (emailSetting != null) ? emailSetting.UseDefaultCredentials : _emailSetting.UseDefaultCredentials;
            var isBodyHtml = (emailSetting != null) ? emailSetting.IsBodyHTML : _emailSetting.IsBodyHTML;

            MailMessage mail = new()
            {
                Subject = emailOptions.Subject,
                Body = emailOptions.Body,
                From = new MailAddress(senderAddress ?? "", senderDisplayName),
                IsBodyHtml = Convert.ToBoolean(isBodyHtml)
            };
            foreach (var toEmail in emailOptions.ToEmails)
            {
                mail.To.Add(toEmail);
            }
            mail.BodyEncoding = Encoding.Default;

            SmtpClient smtpClient = new()
            {
                Host = host ?? "",
                Port = port,
                Credentials = new NetworkCredential(userName, password),
                EnableSsl = Convert.ToBoolean(enableSsl),
                UseDefaultCredentials = Convert.ToBoolean(useDefaultCredentials),
            };
            await smtpClient.SendMailAsync(mail);
        }

        private static string GetEmailBody(string templateFileName)
        {
            var body = File.ReadAllText(templateFileName);
            return body;
        }

        private static string GetFinalBodyValue(string body, List<KeyValuePair<string, string>> keyValuePairs)
        {
            if (!string.IsNullOrEmpty(body) && keyValuePairs != null)
            {
                foreach (var placeholder in keyValuePairs)
                {
                    if (body.Contains(placeholder.Key))
                    {
                        body = body.Replace(placeholder.Key, placeholder.Value);
                    }
                }
            }

            return body;
        }
        #endregion
    }
}
