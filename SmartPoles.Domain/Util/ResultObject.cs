namespace SmartPoles.Domain.Util
{
    public class ResultObject<T>
    {
        public bool IsSuccess { get; set; }
        public T Value { get; set; }
        public string ErrorMessage { get; set; }

        public ResultObject(T value)
        {
            Value = value;
            IsSuccess = true;
        }
        public ResultObject()
        {}

        public static ResultObject<T> Ok(T value) 
        {
            return new ResultObject<T>(value);
        }

        public static ResultObject<T> Error(string error)
        {
            return new ResultObject<T>()
            {
                IsSuccess = false,
                ErrorMessage = error
            };
        }
    }
}