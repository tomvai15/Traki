namespace Traki.Api.Contracts.Checklist
{
    public class GetChecklistsResponse
    {
        public IEnumerable<OldChecklistDto> Checklists { get; set; }
    }
}
