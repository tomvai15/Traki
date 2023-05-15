namespace Traki.Domain.Settings
{
    public class SecuritySettings
    {
        public string Secret { get; set; }
        public string Key { get; set; }
        public string InitializationVector { get; set; }
    }
}
