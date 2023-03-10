using CarRental.Application.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace CarRental.API.DtoValidations.Filters
{
    public class DtoValidationFilter : ActionFilterAttribute
    {
        public override void OnResultExecuting(ResultExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                var modelValidationErrors = context.ModelState.Values
                    .SelectMany(v => v.Errors.Select(b => b.ErrorMessage)).ToList();

                var dtoFailedValidationResult = new DtoFailedValidationResult(modelValidationErrors);

                context.Result = new JsonResult(dtoFailedValidationResult) { StatusCode = StatusCodes.Status400BadRequest };
            }
        }
    }
}
