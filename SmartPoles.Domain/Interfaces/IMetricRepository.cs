using System.Threading.Tasks;
using SmartPoles.Domain.DTOs;
using SmartPoles.Domain.Models;
using SmartPoles.Domain.Util;

namespace SmartPoles.Domain.Interfaces
{
    public interface IMetricRepository
    {
        public Task<ResultObject<FormattedMetric>> GetMetricAndCondominiumAsync(double condominiumCode, string metric, int minutes = 0, bool isMaxMetric = false);

        public Task<ResultObject<CommonIoTDataResponse>> GetCommonIotDataByCondominiumAsync(double condominiumCode);
        public Task<ResultObject<IotDataResponse>> GetMetricAsync(double condominium, string metricName, bool isMaxMetric = false);
    }
}