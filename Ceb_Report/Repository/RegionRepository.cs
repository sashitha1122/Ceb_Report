using Ceb_Report.Interfaces;
using Ceb_Report.Models;
using Informix.Net.Core;

namespace Ceb_Report.Repository
{
    public class RegionRepository : IRegionRepository
    {
        private readonly string _connectionString;

        public RegionRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Informix_billsmry")!;
        }

        public async Task<IEnumerable<Region>> GetAllRegionsAsync()
        {
            var regions = new List<Region>();

            string query = @"SELECT DISTINCT region FROM areas";

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
                    var region = reader["region"].ToString() ?? string.Empty;
                    Console.WriteLine($"[DEBUG] Row {rowCount + 1}: Region = {region}");

                    regions.Add(new Region
                    {
                        RegionName = region
                    });

                    rowCount++;
                }

                Console.WriteLine($"[DEBUG] Total regions read: {rowCount}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Failed to retrieve regions: {ex.Message}");
            }

            return regions;
        }
    }
}
