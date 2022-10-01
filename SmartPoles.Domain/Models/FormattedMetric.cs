namespace SmartPoles.Domain.Models
{
    public class FormattedMetric
    {
        public FormattedMetric(string name, double average)
        {
            MetricName = name;
            MetricAverage = average;
        }
        public string MetricName { get; set; }
        public double MetricAverage { get; set; }
    }
}