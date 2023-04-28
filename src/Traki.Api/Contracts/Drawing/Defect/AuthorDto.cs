namespace Traki.Api.Contracts.Drawing.Defect
{
    public class AuthorDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Surname { get; set; }
        public string? UserIconBase64 { get; set; }
    }
}
