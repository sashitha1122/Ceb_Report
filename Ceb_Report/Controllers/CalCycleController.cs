using Ceb_Report.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ceb_Report.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CalCycleController : ControllerBase
    {
        private readonly ICalCycleRepository _repository;

        public CalCycleController(ICalCycleRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetRecent24Cycles()
        {
            var cycles = await _repository.GetLast24CalCyclesAsync();
            return Ok(cycles);
        }
    }
}
