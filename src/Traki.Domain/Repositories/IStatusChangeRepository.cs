using Traki.Infrastructure.Entities.Drawing;

namespace Traki.Domain.Repositories
{
    public interface IStatusChangeRepository
    {
        Task CreateStatusChange(StatusChange statusChange);
    }
}
