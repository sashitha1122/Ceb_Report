using Ceb_Report.Models;

namespace Ceb_Report.Interfaces
{
    public interface ICalCycleRepository
    {
        Task<IEnumerable<CalCycleInfo>> GetLast24CalCyclesAsync();
    }
}
