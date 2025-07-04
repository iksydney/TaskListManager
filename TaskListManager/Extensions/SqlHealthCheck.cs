using Microsoft.Data.SqlClient;
using Microsoft.Extensions.Diagnostics.HealthChecks;
using System.Diagnostics;

namespace TaskListManager.API.Extensions
{
    public class SqlHealthCheck : IHealthCheck
    {
        private readonly string _connectionString;
        public SqlHealthCheck(IConfiguration configuration)
        {
            _connectionString = configuration.GetConnectionString("DefaultConnection");
        }
        public async Task<HealthCheckResult> CheckHealthAsync(
            HealthCheckContext context,
            CancellationToken cancellationToken = default)
        {
            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();
            try
            {
                using var sqlConnection = new SqlConnection(_connectionString);
                await sqlConnection.OpenAsync(cancellationToken);
                using var command = sqlConnection.CreateCommand();
                command.CommandText = "SELECT 1";
                await command.ExecuteScalarAsync(cancellationToken);
                stopWatch.Stop();
                TimeSpan ts = stopWatch.Elapsed;
                return ts.Seconds > 1 ? HealthCheckResult.Degraded() : HealthCheckResult.Healthy();
            }
            catch (Exception ex)
            {
                stopWatch.Stop();
                return HealthCheckResult.Unhealthy(
                    context.Registration.FailureStatus.ToString(),
                    exception: ex);
            }
        }
    }
}
