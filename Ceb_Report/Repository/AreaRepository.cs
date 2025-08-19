using Ceb_Report.Interfaces;
using Ceb_Report.Models;
using Informix.Net.Core;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ceb_Report.Repository
{
    public class AreaRepository : IAreaRepository
    {
        private readonly string _connectionString;

        public AreaRepository(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Informix_billsmry")!;
        }

        public async Task<IEnumerable<Area>> GetAllAreasAsync()
        {
            var areas = new List<Area>();

            string query = "SELECT area_code, area_name FROM areas";

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
                    var areaCode = reader["area_code"].ToString() ?? string.Empty;
                    var areaName = reader["area_name"].ToString() ?? string.Empty;

                    Console.WriteLine($"[DEBUG] Row {rowCount + 1}: area_code = {areaCode}, area_name = {areaName}");

                    areas.Add(new Area
                    {
                        area_code = areaCode,
                        area_name = areaName
                    });

                    rowCount++;
                }

                Console.WriteLine($"[DEBUG] Total rows read: {rowCount}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Failed to retrieve areas: {ex.Message}");
                // Optional: log stack trace or rethrow if needed
            }

            return areas;
        }

        public bool IsDatabaseConnected(out string? errorMessage)
        {
            try
            {
                using var connection = new IfxConnection(_connectionString);
                connection.Open();
                connection.Close();
                errorMessage = null;
                return true;
            }
            catch (Exception ex)
            {
                errorMessage = ex.Message;
                return false;
            }
        }
    }
}
