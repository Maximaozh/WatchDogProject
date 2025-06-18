using Dapper;
using DocumentFormat.OpenXml.Math;
using Microsoft.Data.Sqlite;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WatchDog
{
    public class AccessTime
    {
        public static double CalculateAvailabilityPercentage(DateTime startDate, DateTime endDate, string address, string connectionString)
        {
            using (var connection = new SqliteConnection(connectionString))
            {
                connection.Open();

                // SQL-запрос для получения всех записей за указанный период для конкретного адреса
                var sql = @"
            SELECT COUNT(*) AS TotalChecks, 
                   SUM(CASE WHEN IsAlive = 1 THEN 1 ELSE 0 END) AS AliveChecks 
            FROM CheckResults 
            WHERE Address = @Address AND Timestamp BETWEEN @StartDate AND @EndDate";

                var result = connection.QuerySingle(sql, new { Address = address, StartDate = startDate.ToString("o"), EndDate = endDate.ToString("o") });

                long totalChecks = result.TotalChecks is null ? 0 : result.TotalChecks;
                long aliveChecks = result.AliveChecks is null ? 0 : result.AliveChecks;

                if (totalChecks == 0)
                    return 0.0;

                // Вычисляем процент доступности
                double availabilityPercentage = (double)aliveChecks / totalChecks * 100;
                return availabilityPercentage;
            }
        }
    }
}
