using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Traki.Domain.Models.Drawing;

namespace Traki.Domain.Repositories
{
    public interface IDefectsRepository
    {
        Task<Defect> CreateDefect(Defect defect);
        Task<Defect> GetDefect(int defectId);
    }
}
