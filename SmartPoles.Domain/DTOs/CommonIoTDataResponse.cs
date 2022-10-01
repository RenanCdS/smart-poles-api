namespace SmartPoles.Domain.DTOs
{
    public class CommonIoTDataResponse
    {
        public IotDataResponse Temperature { get; set; }
        public IotDataResponse Humidity { get; set; }
        public IotDataResponse Sound { get; set; }
    }

    public class IotDataResponse
    {
        public double Current { get; set; }
        public double HourAverage { get; set; }
        public double DayAverage { get; set; }
        public double WeekAverage { get; set; }
    }
}