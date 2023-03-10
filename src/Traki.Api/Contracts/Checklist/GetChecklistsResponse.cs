namespace Traki.Api.Contracts.Checklist
{
    public class GetChecklistsResponse
    {
        public IEnumerable<ChecklistDto> Checklists { get; set; }
    }
}
