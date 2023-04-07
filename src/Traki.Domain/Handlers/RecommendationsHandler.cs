using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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
        private readonly IProductsRepository _productsRepository;
        private readonly IDefectsRepository _defectsRepository;

        public RecommendationsHandler(IProductsRepository productsRepository, IDefectsRepository defectsRepository)
        {
            _productsRepository = productsRepository;
            _defectsRepository = defectsRepository;
        }

        public async Task<Recommendation> GetRecommendation(int userId)
        {
            var products = await _productsRepository.GetProductByQuery(x => x.UserId == userId);
            var defects = await _defectsRepository.GetDefectsByQuery(x => x.UserId == userId);

            var defectRecommendations = defects.Select(x => new DefectRecomendation
            {
                Defect = x,
                ProductId = x.Drawing.ProductId,
                ProjectId = x.Drawing.Product.ProjectId
            });

            var recommendation = new Recommendation
            {
                Products = products,
                Defects = defectRecommendations
            };

            return recommendation;
        }
    }
}
