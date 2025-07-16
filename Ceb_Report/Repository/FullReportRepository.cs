using Ceb_Report.Models;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using Informix.Net.Core;

namespace Ceb_Report.Repositories
{
    public class FullReportRepository : IFullReportRepository
    {
        // ✅ Connection string from appsettings.json
        private readonly string _connectionString;

        // ✅ Constructor injects configuration
        public FullReportRepository(IConfiguration configuration)
        {
            // Ensure this key matches appsettings.json
            _connectionString = configuration.GetConnectionString("Informix");
        }

        // ✅ Main method to get all reports
        public IEnumerable<FullReport> GetAllReports()
        {
            var reports = new List<FullReport>();

            // ✅ SQL query (note the properly closed string)
            string query = @"
                SELECT 
                    COUNT(acc_no) AS no_of_payments,
                    SUM(trans_amt) AS amount
                FROM cus_tran
                WHERE trans_date >= '2025-05-01' 
                  AND trans_date <= '2025-05-31'
                  AND agent = 'CEBH' 
                  AND pay_mode = 'C' 
                  AND pay_type = 'B' 
                  AND count_no IN ('01','02','03','04','05','06','07','08','09','10','12') 
                  AND trans_type = 0
            ";

            try
            {
                Console.WriteLine("[DEBUG] Using connection string: " + (_connectionString?.Substring(0, Math.Min(_connectionString.Length, 50)) ?? "null") + "...");
                Console.WriteLine("[DEBUG] SQL Query: " + query);

                Console.WriteLine("[DEBUG] Connecting to database...");
                using var connection = new IfxConnection(_connectionString);
                connection.Open();
                Console.WriteLine("[DEBUG] Connection opened. Executing query...");

                // Use IfxCommand explicitly
                using var command = new IfxCommand(query, connection);

                using var reader = command.ExecuteReader();

                if (!reader.HasRows)
                {
                    Console.WriteLine("[DEBUG] No rows returned from the database. Check your WHERE conditions and data.");
                }

                int rowCount = 0;
                while (reader.Read())
                {
                    var noOfPayments = reader["no_of_payments"] != DBNull.Value ? reader["no_of_payments"].ToString() : "0";
                    var amount = reader["amount"] != DBNull.Value ? reader["amount"].ToString() : "0.00";

                    Console.WriteLine($"[DEBUG] Row {rowCount + 1}: no_of_payments = {noOfPayments}, amount = {amount}");

                    var report = new FullReport
                    {
                        NoOfPayments = noOfPayments,
                        Amount = amount
                    };

                    reports.Add(report);
                    rowCount++;
                }
                Console.WriteLine($"[DEBUG] Total rows read: {rowCount}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Failed to retrieve full reports: {ex.Message}");
            }

            return reports;
        }

        // Enhanced: Return error message as out parameter
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



