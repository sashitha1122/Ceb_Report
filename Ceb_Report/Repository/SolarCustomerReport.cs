// Repository/SolarCustomerReport.cs
using Ceb_Report.Interfaces;
using Ceb_Report.Models;
using Informix.Net.Core;
using System.Data.Common;


namespace Ceb_Report.Repository
{
    public class SolarCustomerReport : ISolarCustomerReport
    {
        private readonly string _connectionString;

        public SolarCustomerReport(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("Informix_billsmry")!;
        }

        public async Task<IEnumerable<ReportEntry>> GetReportDataAsync(ReportFilter filter)
        {
            var reportEntries = new List<ReportEntry>();
            string query = BuildQuery(filter);

            try
            {
                using var connection = new IfxConnection(_connectionString);
                await connection.OpenAsync();

                using var command = new IfxCommand(query, connection);
                AddParameters(command, filter);

                using var reader = await command.ExecuteReaderAsync();

                while (await reader.ReadAsync())
                {
                    reportEntries.Add(MapReportEntry(reader));
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Failed to retrieve report data: {ex.Message}");
                throw;
            }

            return reportEntries;
        }

        private string BuildQuery(ReportFilter filter)
        {
            string baseQuery = @"
                SELECT 
                    n.area_code, n.acct_number, n.net_type, n.units_out, n.units_in, n.gen_cap, 
                    n.bill_cycle, n.tariff_code, n.bf_units, n.units_bill, n.period, n.kwh_chg,
                    n.fxd_chg, n.fac_chg, n.cf_units, n.rate, n.unitsale, n.kwh_sales,
                    n.bank_code, n.bran_code, n.bk_ac_no, n.agrmnt_date 
                FROM 
                    netmtcons n";

            string whereClause = "";
            string orderBy = "ORDER BY n.acct_number";

            switch (filter.ReportType.ToLower())
            {
                case "area":
                    baseQuery += filter.CycleType == "bill"
                        ? " WHERE n.area_code = ? AND n.net_type = ? AND n.bill_cycle = ?"
                        : " WHERE n.area_code = ? AND n.net_type = ? AND n.calc_cycle = ?";
                    break;

                case "province":
                    baseQuery += " INNER JOIN areas a ON n.area_code = a.area_code";
                    baseQuery += filter.CycleType == "bill"
                        ? " WHERE a.prov_code = ? AND n.net_type = ? AND n.bill_cycle = ?"
                        : " WHERE a.prov_code = ? AND n.net_type = ? AND n.calc_cycle = ?";
                    break;

                case "division":
                    baseQuery += " INNER JOIN areas a ON n.area_code = a.area_code";
                    baseQuery += filter.CycleType == "bill"
                        ? " WHERE a.region = ? AND n.net_type = ? AND n.bill_cycle = ?"
                        : " WHERE a.region = ? AND n.net_type = ? AND n.calc_cycle = ?";
                    break;

                case "entire":
                    baseQuery += filter.CycleType == "bill"
                        ? " WHERE n.net_type = ? AND n.bill_cycle = ?"
                        : " WHERE n.net_type = ? AND n.calc_cycle = ?";
                    break;

                default:
                    throw new ArgumentException("Invalid report type");
            }

            return baseQuery + " " + orderBy;
        }

        private void AddParameters(IfxCommand command, ReportFilter filter)
        {
            switch (filter.ReportType.ToLower())
            {
                case "area":
                    command.Parameters.Add(new IfxParameter { Value = filter.AreaCode });
                    command.Parameters.Add(new IfxParameter { Value = filter.NetType });
                    command.Parameters.Add(new IfxParameter { Value = filter.CycleNumber });
                    break;

                case "province":
                    command.Parameters.Add(new IfxParameter { Value = filter.ProvinceCode });
                    command.Parameters.Add(new IfxParameter { Value = filter.NetType });
                    command.Parameters.Add(new IfxParameter { Value = filter.CycleNumber });
                    break;

                case "division":
                    command.Parameters.Add(new IfxParameter { Value = filter.Region });
                    command.Parameters.Add(new IfxParameter { Value = filter.NetType });
                    command.Parameters.Add(new IfxParameter { Value = filter.CycleNumber });
                    break;

                case "entire":
                    command.Parameters.Add(new IfxParameter { Value = filter.NetType });
                    command.Parameters.Add(new IfxParameter { Value = filter.CycleNumber });
                    break;
            }
        }

        private ReportEntry MapReportEntry(DbDataReader reader)
        {
            if (reader == null)
            {
                throw new ArgumentNullException(nameof(reader));
            }
            // Create a new method to safely get string values
            string GetString(string columnName) =>
        reader[columnName] != DBNull.Value ? reader[columnName].ToString() : string.Empty;

            // Create a new method to safely get nullable decimals
            decimal? GetNullableDecimal(string columnName) =>
                reader[columnName] != DBNull.Value ? Convert.ToDecimal(reader[columnName]) : (decimal?)null;

            return new ReportEntry
            {
                area_code = GetString("area_code"),
                acct_number = GetString("acct_number"),
                net_type = GetString("net_type"),
                units_out = GetNullableDecimal("units_out") ?? 0,
                units_in = GetNullableDecimal("units_in") ?? 0,
                gen_cap = GetNullableDecimal("gen_cap") ?? 0,
                bill_cycle = reader["bill_cycle"] != DBNull.Value ? Convert.ToInt32(reader["bill_cycle"]) : 0,
                tariff_code = GetString("tariff_code"),
                bf_units = GetNullableDecimal("bf_units") ?? 0,
                units_bill = GetNullableDecimal("units_bill") ?? 0,
                period = GetString("period"),
                kwh_chg = GetNullableDecimal("kwh_chg") ?? 0,
                fxd_chg = GetNullableDecimal("fxd_chg") ?? 0,
                fac_chg = GetNullableDecimal("fac_chg") ?? 0,
                cf_units = GetNullableDecimal("cf_units") ?? 0,
                rate = GetNullableDecimal("rate") ?? 0,
                unitsale = GetNullableDecimal("unitsale") ?? 0,
                kwh_sales = GetNullableDecimal("kwh_sales") ?? 0,
                bank_code = GetString("bank_code"),
                bran_code = GetString("bran_code"),
                bk_ac_no = GetString("bk_ac_no"),
                agrmnt_date = reader["agrmnt_date"] != DBNull.Value ? Convert.ToDateTime(reader["agrmnt_date"]) : DateTime.MinValue
            };
        }

        public async Task<bool> IsDatabaseConnectedAsync()
        {
            try
            {
                using var connection = new IfxConnection(_connectionString);
                await connection.OpenAsync();
                await connection.CloseAsync();
                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"[ERROR] Database connection failed: {ex.Message}");
                return false;
            }
        }
    }
}