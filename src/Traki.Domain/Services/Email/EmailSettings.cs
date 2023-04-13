namespace Traki.Domain.Services.Email
{
    public class EmailSettings
    {
        public static readonly string SectionName = "Email";
        public string Address { get; set; }
        public string Password { get; set; }
    }
}
