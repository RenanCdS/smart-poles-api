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

            if (!result.IsSuccess)
            {
                return BadRequest(result.ErrorMessage);
            }
            return Ok(result.Value);
        }

        [HttpGet()]
        [Produces(typeof(IotDataResponse))]
        public async Task<IActionResult> GetMetricsByCondominium([FromQuery] double condominiumCode, [FromQuery] string metricName)
        {
            var result = await _prometheusRepository.GetMetricAverageAsync(condominiumCode, metricName);

            if (!result.IsSuccess)
            {
                return BadRequest(result.ErrorMessage);
            }
            return Ok(result.Value);
        }
    }
}