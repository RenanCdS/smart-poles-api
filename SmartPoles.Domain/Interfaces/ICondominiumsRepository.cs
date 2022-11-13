using SmartPoles.Domain.DTOs;
using SmartPoles.Domain.Util;

namespace SmartPoles.Domain.Interfaces
{
    public interface ICondominiumsRepository
    {
        Task<List<Condominium>> GetCondominiumsAsync();
        Task<Condominium> GetCondominiumByCode(int condominiumCode);
        Task<ResultObject<bool>> InsertCondominiumAsync(Condominium condominium);
        Task<ResultObject<bool>> UpdateCondominiumAsync(Condominium condominiumToBeUpdated);
        Task<ResultObject<bool>> DeleteCondominiumAsync(int condominiumId);
    }
}