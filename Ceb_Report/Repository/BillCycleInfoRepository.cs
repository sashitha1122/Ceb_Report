using Ceb_Report.Interfaces;
using Ceb_Report.Models;
using Informix.Net.Core;


namespace Ceb_Report.Repository
{
    public class BillCycleInfoRepository : IBillCycleInfoRepository
    {
        private readonly string _connectionString;

        public BillCycleInfoRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Informix_billsmry")!;
        }

        public async Task<IEnumerable<BillCycleInfo>> GetLast24BillCyclesAsync()
        {
            var result = new List<BillCycleInfo>();
            int maxCycle = 0;

            try
            {
                using var connection = new IfxConnection(_connectionString);
                await connection.OpenAsync();

                using var command = new IfxCommand("SELECT MAX(bill_cycle) FROM netmtcons", connection);
                var scalar = await command.ExecuteScalarAsync();

                maxCycle = scalar != null ? Convert.ToInt32(scalar) : 0;

                Console.WriteLine($"[DEBUG] Max Bill Cycle: {maxCycle}");

                for (int i = maxCycle; i > maxCycle - 24; i--)
                {
                    result.Add(new BillCycleInfo
                    {
                        BillCycle = i,
                        MonthYear = ConvertToMonthYear(i)
                    });
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Failed to retrieve bill cycles: {ex.Message}");
            }

            return result;
        }

        private string ConvertToMonthYear(int i)
        {
            int mnth = (i - 100) % 12;
            int m_mnth = mnth;
            int yr = 97 + (i - 100) / 12;
            string yr1;

            if (mnth == 0)
            {
                yr -= 1;
                m_mnth = 12;
            }

            yr1 = (yr % 100).ToString("D2");

            string mnth1 = m_mnth switch
            {
                1 => "Jan",
                2 => "Feb",
                3 => "Mar",
                4 => "Apr",
                5 => "May",
                6 => "Jun",
                7 => "Jul",
                8 => "Aug",
                9 => "Sep",
                10 => "Oct",
                11 => "Nov",
                12 => "Dec",
                _ => "Unknown"
            };

            return $"{mnth1} {yr1}";
        }
    }
}
