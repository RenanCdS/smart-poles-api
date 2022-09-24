using Microsoft.AspNetCore.Mvc;
using SmartPoles.Domain.Interfaces;

namespace SmartPoles.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MetricsController : ControllerBase
    {
        private readonly IMetricRepository _prometheusRepository; 
        public MetricsController(IMetricRepository prometheusRepository)
        {
            _prometheusRepository = prometheusRepository;
        }

        [HttpGet("{condominiumName}")]
        public async Task<IActionResult> GetMetricsByCondominium(string condominiumName)
        {
            var result = await _prometheusRepository.GetAverageByMetricAndCondominium("temperature", condominiumName);
            return Ok(result);
        }
    }
}