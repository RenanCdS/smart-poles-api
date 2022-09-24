using System.Threading.Tasks;

namespace SmartPoles.Domain.Interfaces
{
    public interface IMetricRepository
    {
        public Task<double> GetAverageByMetricAndCondominium(string metric,
                                                                   string condominium);
    }
}