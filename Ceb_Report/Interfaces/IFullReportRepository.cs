using System.Collections.Generic;
using Ceb_Report.Models;

namespace Ceb_Report.Repositories
{
    public interface IFullReportRepository
    {
        IEnumerable<FullReport> GetAllReports();

        // Enhanced: Add out parameter for error message
        bool IsDatabaseConnected(out string? errorMessage);
    }
}
