using System.Text.Json;
using System.Text.Json.Serialization;

namespace SmartPoles.Domain.Models
{
    public class Metric
    {
        [JsonPropertyName("__name__")]
        public string Name { get; set; }
    }
}