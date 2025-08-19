namespace Ceb_Report.Models
{
    public class ReportFilter
    {
        public string ReportType { get; set; } // "area", "province", "division", "entire"
        public string CycleType { get; set; }  // "bill" or "calculation"
        public string AreaCode { get; set; }
        public string ProvinceCode { get; set; }
        public string Region { get; set; }
        public string NetType { get; set; }
        public int CycleNumber { get; set; }
    }
}
