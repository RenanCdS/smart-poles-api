using SmartPoles.Domain.DTOs;

namespace SmartPoles.Domain.Interfaces
{
    public interface ICondominiumsService
    {
        public Task<CondominiumsResponse> GetCondominiumsAsync();
    }
}