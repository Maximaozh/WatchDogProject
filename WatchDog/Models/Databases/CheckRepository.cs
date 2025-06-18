using Dapper;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WatchDog
{


    public class CheckRepository
    {
        public string ConnectionString { get; set; }

        public void CreateDatabase()
        {
            using (var connection = new SqliteConnection(ConnectionString))
            {
                connection.Open();
                string sql = @"
                CREATE TABLE IF NOT EXISTS CheckResults (
                    Id INTEGER PRIMARY KEY AUTOINCREMENT,
                    MethodName TEXT NOT NULL,
                    Address TEXT NOT NULL,
                    IsAlive INTEGER NOT NULL,
                    ResponseTime TEXT,
                    ExtraMessage TEXT,
                    Timestamp TEXT NOT NULL DEFAULT (datetime('now'))  -- Добавлен столбец Timestamp
                )";

                using (var command = new SqliteCommand(sql, connection))
                {
                    command.ExecuteNonQuery();
                }
                connection.Close();
            }
        }

        public IEnumerable<CheckDataBaseItem> GetLastRecords(int count)
        {
            using (var connection = new SqliteConnection(ConnectionString))
            {
                connection.Open();
                var sql = "SELECT * FROM CheckResults ORDER BY Id DESC LIMIT @Count";
                return connection.Query<CheckDataBaseItem>(sql, new { Count = count });
            }
        }
        public IEnumerable<CheckDataBaseItem> GetResultsByAddress(string address, int count = 5)
        {
            using (var connection = new SqliteConnection(ConnectionString))
            {
                connection.Open();
                var sql = "SELECT * FROM CheckResults WHERE Address = @Address LIMIT @Count";
                return connection.Query<CheckDataBaseItem>(sql, new { Address = address, Count = count });
            }
        }

        public IEnumerable<CheckDataBaseItem> GetErrorResults(int count = 50)
        {
            using (var connection = new SqliteConnection(ConnectionString))
            {
                connection.Open();
                var sql = "SELECT * FROM CheckResults WHERE IsAlive = 0 LIMIT @Count";
                return connection.Query<CheckDataBaseItem>(sql, new { Count = count });
            }
        }

    public List<CheckResultsReport> GetUpStateInfoInRange(DateTime startDate, DateTime endDate)
    {
        using (var connection = new SqliteConnection(ConnectionString))
        {
            connection.Open();

            var addresses = connection.Query<string>("SELECT DISTINCT Address FROM CheckResults WHERE Timestamp BETWEEN @StartDate AND @EndDate",
                new { StartDate = startDate.ToString("o"), EndDate = endDate.ToString("o") });

            List<CheckResultsReport> report = new List<CheckResultsReport>();

            foreach (var address in addresses)
            {
                var results = connection.Query<CheckDataBaseItem>("SELECT * FROM CheckResults WHERE Address = @Address AND Timestamp BETWEEN @StartDate AND @EndDate",
                    new { Address = address, StartDate = startDate.ToString("o"), EndDate = endDate.ToString("o") });

                int totalChecks = results.Count();

                // Вычисляем процент доступности с использованием класса AccessTime
                double availabilityPercentage = AccessTime.CalculateAvailabilityPercentage(startDate, endDate, address, ConnectionString);

                report.Add(new CheckResultsReport
                {
                    Address = address,
                    TotalChecks = totalChecks,
                    AvailabilityPercentage = availabilityPercentage
                });
            }
            return report;
        }
    }

    public async void SaveAsync(CheckResult result)
        {
            using (var connection = new SqliteConnection(ConnectionString))
            {
                connection.Open();
                string sql = "INSERT INTO CheckResults (MethodName, Address, IsAlive, ResponseTime, ExtraMessage, Timestamp) VALUES (@MethodName, @Address, @IsAlive, @ResponseTime, @ExtraMessage, @Timestamp)";
                using (var command = new SqliteCommand(sql, connection))
                {
                    command.Parameters.AddWithValue("@MethodName", result.MethodName);
                    command.Parameters.AddWithValue("@Address", result.Address);
                    command.Parameters.AddWithValue("@IsAlive", result.IsAlive ? 1 : 0);
                    command.Parameters.AddWithValue("@ResponseTime", result.ResponseTime);
                    command.Parameters.AddWithValue("@ExtraMessage", result.ExtraMessage);
                    command.Parameters.AddWithValue("@Timestamp", DateTime.UtcNow.ToString("o")); // Установите текущее время в формате ISO 8601
                    await command.ExecuteNonQueryAsync();
                }
                connection.Close();
            }

        }
    }
}
