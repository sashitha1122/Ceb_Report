using Ceb_Report.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ceb_Report.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
   
    public class AreaController : ControllerBase
    {
        private readonly IAreaRepository _areaRepository;

        public AreaController(IAreaRepository areaRepository)
        {
            _areaRepository = areaRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var areas = await _areaRepository.GetAllAreasAsync();
            return Ok(areas);
        }

        [HttpGet("db-check")]
        public IActionResult CheckDatabaseConnection()
        {
            bool isConnected = _areaRepository.IsDatabaseConnected(out var errorMessage);
            if (isConnected)
                return Ok(new { connected = true });
            else
                return StatusCode(500, new { connected = false, error = errorMessage });
        }

    }

}
