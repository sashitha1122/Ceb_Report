// Controllers/SolarCustomerController.cs
using Ceb_Report.Interfaces;
using Ceb_Report.Models;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ceb_Report.Controllers
{
    [Route("api/solarcustomer")]
    [ApiController]
    public class SolarCustomerController : ControllerBase
    {
        private readonly ISolarCustomerReport _reportRepository;

        public SolarCustomerController(ISolarCustomerReport reportRepository)
        {
            _reportRepository = reportRepository;
        }

        [HttpGet("report")]
        public async Task<ActionResult<IEnumerable<ReportEntry>>> GetReportData(
            [FromQuery] string reportType,
            [FromQuery] string cycleType,
            [FromQuery] string areaCode = null,
            [FromQuery] string provinceCode = null,
            [FromQuery] string region = null,
            [FromQuery] string netType = null,
            [FromQuery] int cycleNumber = 0)
        {
            try
            {
                var filter = new ReportFilter
                {
                    ReportType = reportType,
                    CycleType = cycleType,
                    AreaCode = areaCode,
                    ProvinceCode = provinceCode,
                    Region = region,
                    NetType = netType,
                    CycleNumber = cycleNumber
                };

                ValidateFilter(filter);

                var reportData = await _reportRepository.GetReportDataAsync(filter);
                return Ok(reportData);
            }
            catch (ArgumentException ex)
            {
                return BadRequest(ex.Message);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
        }

        private void ValidateFilter(ReportFilter filter)
        {
            if (string.IsNullOrEmpty(filter.ReportType))
                throw new ArgumentException("Report type is required");

            if (string.IsNullOrEmpty(filter.CycleType))
                throw new ArgumentException("Cycle type is required");

            switch (filter.ReportType.ToLower())
            {
                case "area":
                    if (string.IsNullOrEmpty(filter.AreaCode))
                        throw new ArgumentException("Area code is required for area reports");
                    break;

                case "province":
                    if (string.IsNullOrEmpty(filter.ProvinceCode))
                        throw new ArgumentException("Province code is required for province reports");
                    break;

                case "division":
                    if (string.IsNullOrEmpty(filter.Region))
                        throw new ArgumentException("Region is required for division reports");
                    break;
            }

            if (string.IsNullOrEmpty(filter.NetType))
                throw new ArgumentException("Net type is required");

            if (filter.CycleNumber <= 0)
                throw new ArgumentException("Valid cycle number is required");
        }

        [HttpGet("check-db-connection")]
        public async Task<IActionResult> CheckDbConnection()
        {
            try
            {
                var isConnected = await _reportRepository.IsDatabaseConnectedAsync();
                return Ok(new { IsConnected = isConnected });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new
                {
                    IsConnected = false,
                    Error = $"Error checking database connection: {ex.Message}"
                });
            }
        }
    }
}