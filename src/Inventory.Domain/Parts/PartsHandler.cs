using Inventory.Infrastructure.Parts;
using Microsoft;

namespace Inventory.Domain.Parts
{
    public interface IPartsHandler
    {
        public Task<List<Part>> GetParts();
    }
    public class PartsHandler: IPartsHandler
    {
        private readonly IPartsRepository partsRepository;

        public PartsHandler(IPartsRepository partsRepository)
        {
            Requires.NotNull(partsRepository, nameof(partsRepository));

            this.partsRepository = partsRepository;
        }

        public async Task<List<Part>> GetParts()
        {
            IEnumerable<PartDto> partDtos = await partsRepository.GetParts();

            List<Part> parts = new List<Part>();

            foreach (PartDto partDto in partDtos)
            {
                parts.Add(new Part { 
                    Count = partDto.Count, 
                    Name = partDto.Name 
                });  
            }

            return parts;
        }
    }
}
