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
        
        [HttpGet()]
        [Produces(typeof(CondominiumsResponse))]
        public async Task<IActionResult> GetCondominiums()
        {
            var condominiumsService = await _condominiumsService.GetCondominiumsAsync();

            return Ok(condominiumsService);
        }
    }
}