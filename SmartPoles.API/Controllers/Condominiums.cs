using Microsoft.AspNetCore.Mvc;
using SmartPoles.Domain.DTOs;

namespace SmartPoles.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class Condominiums : ControllerBase
    {
        [HttpGet()]
        [Produces(typeof(CondominiumsResponse))]
        public async Task<IActionResult> GetCondominiums()
        {
            // var result = await _prometheusRepository.GetAverageByMetricAndCondominiumAsync("node_cpu_seconds_total", condominiumName);
            return Ok();
        }
    }
}