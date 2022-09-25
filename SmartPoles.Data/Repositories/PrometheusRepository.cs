using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using SmartPoles.Domain.Interfaces;
using System.Text.Json;
using SmartPoles.Domain.Models;
using SmartPoles.Domain.DTOs;
using SmartPoles.Domain.Util;

namespace SmartPoles.Data.Repositories
{
    public class PrometheusRepository : IMetricRepository
    {
        private readonly string[] COMMON_IOT_DATA = new string[] {
            "node_cpu_seconds_total",
            "node_memory_Active_bytes",
            "node_cpu_seconds_total"
        };

        private readonly int HOUR_IN_MINUTES = 60;
        private readonly int DAY_IN_MINUTES = 1440;
        private readonly int WEEK_IN_MINUTES = 10080;
        private readonly HttpClient _httpClient;
        private readonly ILogger<PrometheusRepository> _logger;
        public PrometheusRepository(IHttpClientFactory httpClientFactory, ILogger<PrometheusRepository> logger)
        {
            _httpClient = httpClientFactory.CreateClient("Prometheus");
            _logger = logger;
        }

        public async Task<ResultObject<double>> GetAverageByMetricAndCondominiumAsync(string[] metricTags, string condominium, int minutes = 0)
        {
            // var query = "avg(" + metric + "{" + "condominium="
            //  + condominium + "})";
            var query = $"avg("+ "{__name__=~" + String.Join('|', metricTags) + "}) by (__name__)";
            var endpoint = $"/api/v1/query?query={query}&start={DateTime.Now.AddMinutes(-minutes)}&end={DateTime.Now}";
            var prometheusMetrics = await _httpClient.GetAsync(endpoint);
            if (!prometheusMetrics.IsSuccessStatusCode)
            {
                throw new Exception("Prometheus server returned not successful status code.");
            }

            var metricsAsString = await prometheusMetrics.Content.ReadAsStringAsync();

            var metrics = JsonSerializer.Deserialize<Result>(metricsAsString, new JsonSerializerOptions() {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            var metricStringResponse = metrics.Data.Result.FirstOrDefault()?.Value[1].ToString();
            if (metricStringResponse is null)
            {
                return ResultObject<double>.Error("Metric was not found.");
            }

            var metricNumberResponse = Convert.ToDouble(metricStringResponse);          
            return ResultObject<double>.Ok(Math.Round(metricNumberResponse, 2));
        }

        public async Task<CommonIoTDataResponse> GetCommonIotDataByCondominiumAsync(string condominium)
        {
            var requests = new List<Task<ResultObject<double>>>();
            
            requests.Add(GetAverageByMetricAndCondominiumAsync(COMMON_IOT_DATA, condominium, 0));
            requests.Add(GetAverageByMetricAndCondominiumAsync(COMMON_IOT_DATA, condominium, HOUR_IN_MINUTES));
            requests.Add(GetAverageByMetricAndCondominiumAsync(COMMON_IOT_DATA, condominium, DAY_IN_MINUTES));
            requests.Add(GetAverageByMetricAndCondominiumAsync(COMMON_IOT_DATA, condominium, WEEK_IN_MINUTES));

            await Task.WhenAll(requests);

            var response = new CommonIoTDataResponse();
            
            return response;
        }
    }
}
