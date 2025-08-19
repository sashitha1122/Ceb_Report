using Ceb_Report.Models;

namespace Ceb_Report.Interfaces
{
    public interface IAreaRepository
    {
        Task<IEnumerable<Area>> GetAllAreasAsync();
        bool IsDatabaseConnected(out string? errorMessage);

    }
}
