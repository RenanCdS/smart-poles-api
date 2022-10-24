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
            "temperature",
            "humidity",
            "sound"
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
            var query = "avg_over_time(" + metric + "{condominium=\"" + condominiumCode + "\"}["+ minutes +"m])";
            var endpoint = $"/api/v1/query?query={query}";
            _logger.LogInformation(query);
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

        public async Task<ResultObject<IotDataResponse>> GetMetricAverageAsync(double condominium, string metricName)
        {
            try
            {   
                var currentMetric = await GetAverageByMetricAndCondominiumAsync(condominium, metricName, 1);
                var hourMetric = await GetAverageByMetricAndCondominiumAsync(condominium, metricName, HOUR_IN_MINUTES);
                var dayMetric = await GetAverageByMetricAndCondominiumAsync(condominium, metricName, DAY_IN_MINUTES);
                var weekMetric = await GetAverageByMetricAndCondominiumAsync(condominium, metricName, WEEK_IN_MINUTES);

                var error = GetErrorResponse<FormattedMetric>(currentMetric, hourMetric, dayMetric, weekMetric);
                if (error is not null)
                {
                    return ResultObject<IotDataResponse>.Ok(new IotDataResponse());
                }

                var iotDataResponse = new IotDataResponse()
                {
                    Current = currentMetric.Value.MetricAverage,
                    HourAverage = hourMetric.Value.MetricAverage,
                    DayAverage = dayMetric.Value.MetricAverage,
                    WeekAverage = weekMetric.Value.MetricAverage,
                };
                return ResultObject<IotDataResponse>.Ok(iotDataResponse);
            }
            catch (Exception ex)
            {
                 _logger.LogError(ex, $"There has been an error recovering the metric {metricName}");
                throw;
            }
        }

        private ResultObject<T> GetErrorResponse<T>(params ResultObject<T>[] resultObjects)
        {

            var error = resultObjects.FirstOrDefault(metricResponse => !metricResponse.IsSuccess);
            if (error is not null)
            {
                return ResultObject<T>.Error(error.ErrorMessage);
            }

            return null;
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

                var error = GetErrorResponse<FormattedMetric>(currentTemperature, hourTemperature, dayTemperature, weekTemperature,
                    currentHumidity, hourHumidity, dayHumidity, weekHumidity,
                    currentSound, hourSound, daySound, weekSound);

                if (error is not null)
                {
                    return ResultObject<CommonIoTDataResponse>.Error(error.ErrorMessage);
                }

                var response = new CommonIoTDataResponse()
                {
                    Temperature = new IotDataResponse()
                    {
                        Current = currentTemperature.Value.MetricAverage,
                        HourAverage = hourTemperature.Value.MetricAverage,
                        DayAverage = dayTemperature.Value.MetricAverage,
                        WeekAverage = weekTemperature.Value.MetricAverage,
                    },
                    Humidity = new IotDataResponse()
                    {
                        Current = currentHumidity.Value.MetricAverage,
                        HourAverage = hourHumidity.Value.MetricAverage,
                        DayAverage = dayHumidity.Value.MetricAverage,
                        WeekAverage = weekHumidity.Value.MetricAverage,
                    },
                    Sound = new IotDataResponse()
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
