using System.Text.Json;
using SmartPoles.Domain.DTOs;
using SmartPoles.Domain.Interfaces;

namespace SmartPoles.BLL.Services
{
    public class CondominiumsService : ICondominiumsService
    {
        private readonly IStorageRepository _storageRepository;
        public CondominiumsService(IStorageRepository storageRepository)
        {
            _storageRepository = storageRepository;
        }
        public async Task<CondominiumsResponse> GetCondominiumsAsync()
        {
            var json = await _storageRepository.GetFileAsync("condominiums.json");
            var condominiumsResult = JsonSerializer.Deserialize<CondominiumsResponse>(json, new JsonSerializerOptions() {
                PropertyNameCaseInsensitive = true,
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            }); 
           
            return condominiumsResult;
        }
    }
}