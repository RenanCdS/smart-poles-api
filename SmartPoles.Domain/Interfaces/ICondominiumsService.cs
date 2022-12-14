using SmartPoles.Domain.DTOs;
using SmartPoles.Domain.Util;

namespace SmartPoles.Domain.Interfaces
{
    public interface ICondominiumsService
    {
        Task<CondominiumsResponse> GetCondominiumsAsync();   
        Task<ResultObject<bool>> InsertCondominiumAsync(Condominium condominium);
        Task<ResultObject<bool>> UpdateCondominiumAsync(Condominium condominiumToBeUpdated);
        Task<ResultObject<bool>> DeleteCondominiumAsync(int condominiumId);
    }
}