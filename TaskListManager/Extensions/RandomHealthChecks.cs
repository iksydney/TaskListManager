using Microsoft.Extensions.Diagnostics.HealthChecks;

namespace TaskListManager.API.Extensions
{
    public class RandomHealthChecks : IHealthCheck
    {
        private static readonly Random _random = new Random();
        public async Task<HealthCheckResult> CheckHealthAsync(HealthCheckContext context, CancellationToken cancellationToken = default)
        {
            try
            {
                var highest = _random.Next(5);
                var result = highest >= 0 && highest < 5
                    ? HealthCheckResult.Healthy() : HealthCheckResult.Unhealthy("Failed random check");
                return await Task.FromResult(result);
            }
            catch (Exception ex)
            {
                return HealthCheckResult.Unhealthy(
                    context.Registration.FailureStatus.ToString(),
                    exception: ex);
            }
        }
    }
}
