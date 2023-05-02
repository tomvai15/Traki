using Traki.Domain.Models;
using Traki.Domain.Repositories;

namespace Traki.Domain.Handlers
{
    public interface IRecommendationsHandler
    {
        Task<Recommendation> GetRecommendation(int userId);
    }
    public class RecommendationsHandler: IRecommendationsHandler
    {
        private readonly IProjectsRepository _projectsRepository;
        private readonly IProductsRepository _productsRepository;
        private readonly IDefectsRepository _defectsRepository;

        public RecommendationsHandler(IProjectsRepository projectsRepository, IProductsRepository productsRepository, IDefectsRepository defectsRepository)
        {
            _projectsRepository = projectsRepository;
            _productsRepository = productsRepository;
            _defectsRepository = defectsRepository;
        }

        public async Task<Recommendation> GetRecommendation(int userId)
        {
            var projects = (await _projectsRepository.GetProjects()).Where(x=> x.AuthorId == userId);

            var productsFromProject = projects.SelectMany(x => x.Products);

            var products = (await _productsRepository.GetProductByQuery(x => x.AuthorId == userId)).ToList();

            products.AddRange(productsFromProject);

            products = products.DistinctBy(x => x.Id).ToList();

            var defects = (await _defectsRepository.GetDefectsByQuery(x => x.AuthorId == userId)).ToList();

            var defectRecommendations = defects.Select(x => new DefectRecomendation
            {
                Defect = x,
                ProductId = x.Drawing.ProductId,
                ProjectId = x.Drawing.Product.ProjectId,
                ProductName = x.Drawing.Product.Name,
            });

            
            var productRecommendations = products.Select(x => new ProductRecomendation
            {
                Product = x,
                ProtocolsCount = x.Protocols.Count(),
                DefectCount = x.Drawings.SelectMany(x=> x.Defects).Count(),
            }).OrderByDescending(x => x.DefectCount).ToList();


            var recommendation = new Recommendation
            {
                Products = productRecommendations,
                Defects = defectRecommendations
            };

            return recommendation;
        }
    }
}
