using Ceb_Report.Models;

namespace Ceb_Report.Interfaces
{
    public interface IProvinceRepository
    {
        Task<IEnumerable<Province>> GetAllProvincesAsync();
       
    }
}
