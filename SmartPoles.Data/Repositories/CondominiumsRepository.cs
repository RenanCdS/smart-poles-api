using System.Text.Json;
using SmartPoles.Domain.DTOs;
using SmartPoles.Domain.Interfaces;
using SmartPoles.Domain.Util;

namespace SmartPoles.Data.Repositories
{
    public class CondominiumsRepository : ICondominiumsRepository
    {
        private readonly IStorageRepository _storageRepository;
        public CondominiumsRepository(IStorageRepository storageRepository)
        {
            _storageRepository = storageRepository;
        }
        public async Task<ResultObject<bool>> DeleteCondominiumAsync(int condominiumCode)
        {
            var condominiums = await GetCondominiumsAsync();

            var condominiumsWithDeletedCondominium = condominiums.Where(condominium => condominium.Code != condominiumCode)
                                                    .ToList();

            await UpdateCondominiumFileAsync(condominiumsWithDeletedCondominium);

            return ResultObject<bool>.Ok(true);
        }

        public async Task<List<Condominium>> GetCondominiumsAsync()
        {
            var json = await _storageRepository.GetFileAsync("condominiums.json");
            var condominiumsResult = JsonSerializer.Deserialize<CondominiumsResponse>(json, new JsonSerializerOptions() {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            }); 

            return condominiumsResult?.Condominiums is null ? new List<Condominium>() : condominiumsResult.Condominiums;
        }

        public async Task<ResultObject<bool>> InsertCondominiumAsync(Condominium condominium)
        {
            var condominiums = await GetCondominiumsAsync();

            condominiums.Add(condominium);

            await UpdateCondominiumFileAsync(condominiums);

            return ResultObject<bool>.Ok(true);
        }

        public async Task<Condominium> GetCondominiumByCode(int condominiumCode)
        {
            var condominiums = await GetCondominiumsAsync();

            return condominiums.FirstOrDefault(condominium => condominium.Code == condominiumCode);
        }

        public async Task<ResultObject<bool>> UpdateCondominiumAsync(Condominium condominiumToBeUpdated)
        {
            var condominiums = await GetCondominiumsAsync();

            var updatedCondiminiums = condominiums.Select(condominium => {
                if (condominium.Code == condominiumToBeUpdated.Code)
                {
                    return condominiumToBeUpdated;
                }
                return condominium;
            }).ToList();

            await UpdateCondominiumFileAsync(updatedCondiminiums);

            return ResultObject<bool>.Ok(true);
        }

        private async Task UpdateCondominiumFileAsync(List<Condominium> condominiums)
        {
             var condominiumsResult = JsonSerializer
                .Serialize<CondominiumsResponse>(new CondominiumsResponse(condominiums), new JsonSerializerOptions() {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            }); 

            await _storageRepository.UpdateFileAsync("condominiums.json", condominiumsResult);
        }
    }
}