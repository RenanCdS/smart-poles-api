using System.Collections;

namespace SmartPoles.Domain.Models
{
    public class ResultMetric
    {
        public Metric Metric { get; set; }
        public object[] Value { get; set; }
    }
}