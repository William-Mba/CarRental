using Microsoft.AspNetCore.Http;

namespace CarRental.Application.Dtos
{
    public record DtoFailedValidationResult
    {
        public int StatusCode { get; } = StatusCodes.Status400BadRequest;

        public IList<string> ModelValidationErrors { get; }

        public DtoFailedValidationResult(IList<string> modelValidationErrors)
        {
            ModelValidationErrors = modelValidationErrors;
        }
    }
}
