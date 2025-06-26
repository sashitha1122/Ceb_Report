using Ceb_Report.Models;
using Ceb_Report.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Ceb_Report.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class FullReportController : ControllerBase
    {
        private readonly IFullReportRepository _reportRepository;

        public FullReportController(IFullReportRepository reportRepository)
        {
            _reportRepository = reportRepository;
        }

        [HttpGet]
        public ActionResult<IEnumerable<FullReport>> GetReports()
        {
            var data = _reportRepository.GetAllReports();
            return Ok(data);
        }

        [HttpGet("db-check")]
        public IActionResult CheckDatabaseConnection()
        {
            bool isConnected = _reportRepository.IsDatabaseConnected(out var errorMessage);
            if (isConnected)
                return Ok(new { connected = true });
            else
                return StatusCode(500, new { connected = false, error = errorMessage });
        }
    }
}
