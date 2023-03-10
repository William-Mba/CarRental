namespace CarRental.API.Utils
{
    public class ApiExceptionOptions
    {
        public Action<HttpContext, Exception, ApiError>? AddResponseDetails { get; set; }
    }
}
