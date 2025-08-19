using Ceb_Report.Models;

namespace Ceb_Report.Interfaces
{
    public interface IBillCycleInfoRepository
    {
        Task<IEnumerable<BillCycleInfo>> GetLast24BillCyclesAsync();
    }
}
