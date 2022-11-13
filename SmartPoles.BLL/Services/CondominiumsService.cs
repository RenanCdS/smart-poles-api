using System.Text.Json;
using Microsoft.Extensions.Logging;
using SmartPoles.Domain.DTOs;
using SmartPoles.Domain.Interfaces;
using SmartPoles.Domain.Util;

namespace SmartPoles.BLL.Services
{
    public class CondominiumsService : ICondominiumsService
    {
        private readonly ICondominiumsRepository _condominiumsRepository;
        private readonly ILogger<CondominiumsService> _logger;
        public CondominiumsService(ILogger<CondominiumsService> logger, ICondominiumsRepository condominiumsRepository)
        {
            _condominiumsRepository = condominiumsRepository;
            _logger = logger;
        }

        public async Task<ResultObject<bool>> DeleteCondominiumAsync(int condominiumId)
        {
            try
            {
                await _condominiumsRepository.DeleteCondominiumAsync(condominiumId);

                return ResultObject<bool>.Ok(true);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "An error occurred trying to delete the condominium.");
                throw;
            }
        }

        public async Task<CondominiumsResponse> GetCondominiumsAsync()
        {
            try
            {
                var condominiums = await _condominiumsRepository.GetCondominiumsAsync();
           
                return new CondominiumsResponse(condominiums);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "An error occurred trying to get the condominiums.");
                throw;
            }
        }

        public async Task<ResultObject<bool>> InsertCondominiumAsync(Condominium condominium)
        {
            try
            {
                var insertedCondominium = await _condominiumsRepository.GetCondominiumByCode(condominium.Code);

                if (insertedCondominium is not null)
                {
                    return ResultObject<bool>.Error("The condominium code already exists.");
                }

                await _condominiumsRepository.InsertCondominiumAsync(condominium);

                return ResultObject<bool>.Ok(true);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "An error occurred trying to insert the condominium.");
                throw;
            }
        }

        public async Task<ResultObject<bool>> UpdateCondominiumAsync(Condominium condominiumToBeUpdated)
        {
            try
            {
                var insertedCondominium = await _condominiumsRepository.GetCondominiumByCode(condominiumToBeUpdated.Code);

                if (insertedCondominium is null)
                {
                    return ResultObject<bool>.Error("The condominium does not exist.");
                }

                await _condominiumsRepository.UpdateCondominiumAsync(condominiumToBeUpdated);

                return ResultObject<bool>.Ok(true);
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, "An error occurred trying to update the condominium.");
                throw;
            }
        }
    }
}