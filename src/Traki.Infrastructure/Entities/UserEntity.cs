namespace Traki.Infrastructure.Entities
{
    public class UserEntity
    {
        public int Id { get; set; }
        public string Email { get; set; }
        public string HashedPassword { get; set; }
        public string Role { get; set; }
        public string? EncryptedRefreshToken { get; set; }
    }
}
