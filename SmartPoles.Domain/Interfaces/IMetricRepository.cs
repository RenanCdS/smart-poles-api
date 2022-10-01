using System.Threading.Tasks;
using SmartPoles.Domain.DTOs;
using SmartPoles.Domain.Models;
using SmartPoles.Domain.Util;

namespace SmartPoles.Domain.Interfaces
{
    public interface IMetricRepository
    {
        public Task<ResultObject<FormattedMetric>> GetAverageByMetricAndCondominiumAsync(double condominiumCode, string metric, int minutes = 0);

        public Task<ResultObject<CommonIoTDataResponse>> GetCommonIotDataByCondominiumAsync(double condominiumCode);
        public Task<ResultObject<IotDataResponse>> GetMetricAverageAsync(double condominium, string metricName);
    }
}