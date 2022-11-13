using System.Text.Json;
using Microsoft.AspNetCore.Mvc;
using SmartPoles.Domain.DTOs;
using SmartPoles.Domain.Interfaces;

namespace SmartPoles.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CondominiumsController : ControllerBase
    {
        private readonly ICondominiumsService _condominiumsService;
        public CondominiumsController(ICondominiumsService condominiumsService)
        {
            _condominiumsService = condominiumsService;
        }

        [HttpGet]
        [ProducesResponseType(typeof(CondominiumsResponse), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCondominiums()
        {
            var response = await _condominiumsService.GetCondominiumsAsync();

            return Ok(response);
        }

        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> InsertCondominium(Condominium condominium)
        {
            var response = await _condominiumsService.InsertCondominiumAsync(condominium);

            if (!response.IsSuccess)
            {
                return BadRequest(response.ErrorMessage);
            }

            return Ok(response);
        }

        [HttpPut]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UpdateCondominium(Condominium condominium)
        {
            var response = await _condominiumsService.UpdateCondominiumAsync(condominium);

            if (!response.IsSuccess)
            {
                return BadRequest(response.ErrorMessage);
            }

            return Ok(response);
        }

        [HttpDelete("{condominiumId:int}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> DeleteCondominium(int condominiumId)
        {
            var response = await _condominiumsService.DeleteCondominiumAsync(condominiumId);

            return Ok(response);
        }
    }
}