using Ceb_Report.Models;

namespace Ceb_Report.Interfaces
{
    public interface IRegionRepository
    {
        Task<IEnumerable<Region>> GetAllRegionsAsync();
    }
}
