using Traki.Domain.Models.Drawing;

namespace Traki.Domain.Repositories
{
    public interface IDefectCommentRepository
    {
        Task<DefectComment> CreateDefectComment(DefectComment defectComment);
    }
}
