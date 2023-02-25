namespace Traki.Api.Contracts.Project
{
    public class GetProjectsResponse
    {
        public IEnumerable<ProjectDto> Projects { get; set; }
    }
}
