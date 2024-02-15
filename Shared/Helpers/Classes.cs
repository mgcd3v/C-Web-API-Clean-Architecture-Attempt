namespace Shared.Helpers
{
    public class User
    {
        public int Id { get; set; } = default;
        public string Name { get; set; } = string.Empty;
    }

    public class Parameter
    {
        public string Name { get; set; } = string.Empty;
        public string? Value { get; set; }
    }

    public class EmailOptions
    {
        public List<string> ToEmails { get; set; } = new List<string>();
        public string Subject { get; set; } = string.Empty;
        public string Body { get; set; } = string.Empty;
    }

    public class TokenSetting
    {
        public string Token { get; set; } = string.Empty;
        public int TokenLifeCycleDays { get; set; } = default;
        public int TokenLifeCycleMinutes { get; set; } = default;
    }

    public class EmailSetting
    {
        public string SenderAddress { get; set; } = string.Empty;
        public string SenderDisplayName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string Host { get; set; } = string.Empty;
        public int Port { get; set; } = default;
        public bool EnableSSL { get; set; } = default;
        public bool UseDefaultCredentials { get; set; } = default;
        public bool IsBodyHTML { get; set; } = default;
    }
}
