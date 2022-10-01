using Microsoft.AspNetCore.Mvc;
using SmartPoles.Domain.DTOs;
using SmartPoles.Domain.Interfaces;

namespace SmartPoles.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CondominiumsController : ControllerBase
    {
        private readonly IStorageRepository _storageRepository;
        public CondominiumsController(IStorageRepository storageRepository)
        {
            _storageRepository = storageRepository;
        }
        
        [HttpGet()]
        [Produces(typeof(CondominiumsResponse))]
        public async Task<IActionResult> GetCondominiums()
        {
            var file = _storageRepository.GetFile("");
            // var result = await _prometheusRepository.GetAverageByMetricAndCondominiumAsync("node_cpu_seconds_total", condominiumName);
            return Ok();
        }
    }
}