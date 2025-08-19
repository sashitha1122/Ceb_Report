using Ceb_Report.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ceb_Report.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BillCycleInfoController : ControllerBase
    {
        private readonly IBillCycleInfoRepository _repository;

        public BillCycleInfoController(IBillCycleInfoRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public async Task<IActionResult> GetRecent24Cycles()
        {
            var cycles = await _repository.GetLast24BillCyclesAsync();
            return Ok(cycles);
        }
    }
}
