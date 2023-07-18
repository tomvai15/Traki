namespace Traki.Api.Contracts.Section
{
    public class GetSectionsResponse
    {
        public IEnumerable<SectionBaseDto> Sections { get; set; }
    }
}
