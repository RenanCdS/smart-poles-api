using System.Collections;

namespace SmartPoles.Domain.Models
{
    public class ResultData
    {
        public string ResultType { get; set; }
        public IEnumerable<ResultMetric> Result { get; set; }
    }
}