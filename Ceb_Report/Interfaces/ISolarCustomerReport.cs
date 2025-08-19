using Ceb_Report.Models;

namespace Ceb_Report.Interfaces
{
    public interface ISolarCustomerReport
    {
        Task<IEnumerable<ReportEntry>> GetReportDataAsync(ReportFilter filter);
        Task<bool> IsDatabaseConnectedAsync();
    }
}
