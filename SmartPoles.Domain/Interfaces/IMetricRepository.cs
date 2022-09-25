using System.Threading.Tasks;
using SmartPoles.Domain.DTOs;
using SmartPoles.Domain.Util;

namespace SmartPoles.Domain.Interfaces
{
    public interface IMetricRepository
    {
        public Task<ResultObject<double>> GetAverageByMetricAndCondominiumAsync(string[] metricTags,
                                                                   string condominium,
                                                                   int minutes = 0);

        public Task<CommonIoTDataResponse> GetCommonIotDataByCondominiumAsync(string condominium);
    }
}