using Microsoft.Extensions.Logging;
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
            "process_cpu_seconds_total"
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

        public async Task<ResultObject<FormattedMetric>> GetAverageByMetricAndCondominiumAsync(double condominiumCode, string metric, int minutes = 0)
        {
            var query = "sum(sum_over_time({__name__=\"" + metric + "\",condominium=\"" 
            + condominiumCode + "\"}[" + minutes + "m]))/sum(count_over_time({__name__=\"" + metric + "\", condominium=\""+ condominiumCode + "\"}[" + minutes +"m]))";
            var endpoint = $"/api/v1/query?query={query}";
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

            var metricResults = metrics.Data.Result;
            if (metricResults is null || metricResults.Count() == 0)
            {
                return ResultObject<FormattedMetric>.Error("Metric was not found.");
            }

            var metricResult = metricResults.FirstOrDefault();

            var metricAverage = Convert.ToDouble((metricResult.Value[1].ToString()));
            var formattedMetric = new FormattedMetric(metricResult.Metric.Name, Math.Round(metricAverage, 2));

            return ResultObject<FormattedMetric>.Ok(formattedMetric);
        }

        public async Task<ResultObject<CommonIoTDataResponse>> GetCommonIotDataByCondominiumAsync(double condominium)
        {
            try
            {
                var currentTemperature = await GetAverageByMetricAndCondominiumAsync(condominium, COMMON_IOT_DATA[0], 1);
                var hourTemperature = await GetAverageByMetricAndCondominiumAsync(condominium, COMMON_IOT_DATA[0], HOUR_IN_MINUTES);
                var dayTemperature = await GetAverageByMetricAndCondominiumAsync(condominium, COMMON_IOT_DATA[0], DAY_IN_MINUTES);
                var weekTemperature = await GetAverageByMetricAndCondominiumAsync(condominium, COMMON_IOT_DATA[0], WEEK_IN_MINUTES);

                var currentHumidity = await GetAverageByMetricAndCondominiumAsync(condominium, COMMON_IOT_DATA[1], 1);
                var hourHumidity = await GetAverageByMetricAndCondominiumAsync(condominium, COMMON_IOT_DATA[1], HOUR_IN_MINUTES);
                var dayHumidity = await GetAverageByMetricAndCondominiumAsync(condominium, COMMON_IOT_DATA[1], DAY_IN_MINUTES);
                var weekHumidity = await GetAverageByMetricAndCondominiumAsync(condominium, COMMON_IOT_DATA[1], WEEK_IN_MINUTES);

                var currentSound = await GetAverageByMetricAndCondominiumAsync(condominium, COMMON_IOT_DATA[2], 1);
                var hourSound = await GetAverageByMetricAndCondominiumAsync(condominium, COMMON_IOT_DATA[2], HOUR_IN_MINUTES);
                var daySound = await GetAverageByMetricAndCondominiumAsync(condominium, COMMON_IOT_DATA[2], DAY_IN_MINUTES);
                var weekSound = await GetAverageByMetricAndCondominiumAsync(condominium, COMMON_IOT_DATA[2], WEEK_IN_MINUTES);

                var metricsResponses = new List<ResultObject<FormattedMetric>>()
                {
                    currentTemperature, hourTemperature, dayTemperature, weekTemperature,
                    currentHumidity, hourHumidity, dayHumidity, weekHumidity,
                    currentSound, hourSound, daySound, weekSound
                };

                var error = metricsResponses.FirstOrDefault(metricResponse => !metricResponse.IsSuccess);
                if (error is not null)
                {
                    return ResultObject<CommonIoTDataResponse>.Error(error.ErrorMessage);
                }

                var response = new CommonIoTDataResponse()
                {
                    Temperature = new CommonIoTData()
                    {
                        Current = currentTemperature.Value.MetricAverage,
                        HourAverage = hourTemperature.Value.MetricAverage,
                        DayAverage = dayTemperature.Value.MetricAverage,
                        WeekAverage = weekTemperature.Value.MetricAverage,
                    },
                    Humidity = new CommonIoTData()
                    {
                        Current = currentHumidity.Value.MetricAverage,
                        HourAverage = hourHumidity.Value.MetricAverage,
                        DayAverage = dayHumidity.Value.MetricAverage,
                        WeekAverage = weekHumidity.Value.MetricAverage,
                    },
                    Sound = new CommonIoTData()
                    {
                        Current = currentSound.Value.MetricAverage,
                        HourAverage = hourSound.Value.MetricAverage,
                        DayAverage = daySound.Value.MetricAverage,
                        WeekAverage = weekSound.Value.MetricAverage,
                    }
                };
                
                return  ResultObject<CommonIoTDataResponse>.Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "There has been an error recovering the metrics.");
                throw;
            }
            
        }
    }
}
