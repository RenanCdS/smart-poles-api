using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using SmartPoles.Domain.Interfaces;

namespace SmartPoles.Data.Repositories
{
    public class PrometheusRepository : IMetricRepository
    {
        private readonly HttpClient _httpClient;
        private readonly ILogger<PrometheusRepository> _logger;
        public PrometheusRepository(IHttpClientFactory httpClientFactory, ILogger<PrometheusRepository> logger)
        {
            _httpClient = httpClientFactory.CreateClient("Prometheus");
            _logger = logger;
        }

        public async Task<double> GetAverageByMetricAndCondominium(string metric, string condominium)
        {
            var query = "avg(" + metric + "{" + "condominium="
             + condominium + "})";
            query = "avg(node_cpu_seconds_total)";
            //var endpoint = $"/api/v1/query?query={query}&start={DateTime.Now.AddMinutes(-15)}&end={DateTime.Now}";
            var endpoint = $"/api/v1/query?query={query}";
            var prometheusMetrics = await _httpClient.GetAsync(endpoint);
            _logger.LogInformation(endpoint);
            throw new NotImplementedException();
        }
    }
}
