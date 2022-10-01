using Microsoft.AspNetCore.Mvc;
using SmartPoles.Domain.DTOs;
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

        [HttpGet("{condominiumCode}")]
        [Produces(typeof(CommonIoTDataResponse))]
        public async Task<IActionResult> GetMetricsByCondominium(double condominiumCode)
        {
            var result = await _prometheusRepository.GetCommonIotDataByCondominiumAsync(condominiumCode);
            return Ok(result);
        }
    }
}