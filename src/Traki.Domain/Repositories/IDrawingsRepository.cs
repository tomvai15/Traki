using Traki.Domain.Models.Drawing;

namespace Traki.Domain.Repositories
{
    public interface IDrawingsRepository
    {
        public Task<IEnumerable<Drawing>> GetDrawings(int productId);
        public Task<Drawing> GetDrawing(int drawingId);
    }
}
