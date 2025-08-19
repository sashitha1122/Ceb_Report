using Ceb_Report.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Ceb_Report.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProvinceController : ControllerBase
    {
        private readonly IProvinceRepository _provinceRepository;

        public ProvinceController(IProvinceRepository provinceRepository)
        {
            _provinceRepository = provinceRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var provinces = await _provinceRepository.GetAllProvincesAsync();
            return Ok(provinces);
        }

       
    }
}
