using Traki.Api.Contracts.Drawing.Defect;

namespace Traki.Api.Contracts.Project
{
    public class ProjectDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string ClientName { get; set; }
        public string Address { get; set; }
        public string CreationDate { get; set; }
        public string ImageName { get; set; }
        public AuthorDto? Author { get; set; }
    }
}
