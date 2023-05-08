using Traki.Domain.Models.Drawing;

namespace Traki.Domain.Repositories
{
    public interface IStatusChangeRepository
    {
        Task CreateStatusChange(StatusChange statusChange);
    }
}
