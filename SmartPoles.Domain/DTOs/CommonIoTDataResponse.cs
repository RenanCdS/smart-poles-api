namespace SmartPoles.Domain.DTOs
{
    public class CommonIoTDataResponse
    {
        public CommonIoTData Temperature { get; set; }
        public CommonIoTData Humidity { get; set; }
        public CommonIoTData Sound { get; set; }
    }

    public class CommonIoTData 
    {
        public double Current { get; set; }
        public double DayAverage { get; set; }
        public double WeekAverage { get; set; }
    }
}