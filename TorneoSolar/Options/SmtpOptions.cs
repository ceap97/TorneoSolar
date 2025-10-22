namespace TorneoSolar.Options
{
    public class SmtpOptions
    {
        public string Host { get; set; } = "smtp.gmail.com";
        public int Port { get; set; } = 587;
        public bool EnableSsl { get; set; } = true;
        public string FromEmail { get; set; } = string.Empty;
        public string FromName { get; set; } = "Torneo Solar";
        public string Password { get; set; } = string.Empty;
    }
}
