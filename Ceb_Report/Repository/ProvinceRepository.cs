using Ceb_Report.Interfaces;
using Ceb_Report.Models;
using Informix.Net.Core;

namespace Ceb_Report.Repository
{
    public class ProvinceRepository : IProvinceRepository
    {
        private readonly string _connectionString;

        public ProvinceRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Informix_billsmry")!;
        }

        public async Task<IEnumerable<Province>> GetAllProvincesAsync()
        {
            var provinces = new List<Province>();

            string query = @"SELECT prov_code, prov_name FROM provinces 
                             WHERE prov_name <> 'Head Office' 
                             ORDER BY prov_name";

            try
            {
                Console.WriteLine("[DEBUG] Using connection string: " + (_connectionString?.Substring(0, Math.Min(_connectionString.Length, 50)) ?? "null") + "...");
                Console.WriteLine("[DEBUG] SQL Query: " + query);

                using var connection = new IfxConnection(_connectionString);
                await connection.OpenAsync();
                Console.WriteLine("[DEBUG] Connection opened.");

                using var command = new IfxCommand(query, connection);
                using var reader = await command.ExecuteReaderAsync();

                int rowCount = 0;
                while (await reader.ReadAsync())
                {
                    var provCode = reader["prov_code"].ToString() ?? string.Empty;
                    var provName = reader["prov_name"].ToString() ?? string.Empty;

                    Console.WriteLine($"[DEBUG] Row {rowCount + 1}: ProvCode = {provCode}, ProvName = {provName}");

                    provinces.Add(new Province
                    {
                        ProvCode = provCode,
                        ProvName = provName
                    });

                    rowCount++;
                }

                Console.WriteLine($"[DEBUG] Total rows read: {rowCount}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Failed to retrieve provinces: {ex.Message}");
            }

            return provinces;
        }

       
    }
}
