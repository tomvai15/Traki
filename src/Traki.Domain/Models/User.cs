namespace Traki.Domain.Models
{
    public class User
    {
        public int Id { get; set; }
        public string? UserIconBase64 { get; set; }
        public string? RegisterId { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string Email { get; set; }
        public string HashedPassword { get; set; }
        public string Role { get; set; }
        public string Status { get; set; }
        public string? EncryptedRefreshToken { get; set; }
        public string DeviceToken { get; set; }
        public string? RefreshToken { get; set; }
        public DateTime RefreshTokenExpiryTime { get; set; }
    }
}
