using FluentValidation;
using Microsoft.AspNetCore.Diagnostics;

public class GlobalExceptionHandler : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        var (statusCode, title) = exception switch
        {
            ValidationException =>
                (StatusCodes.Status400BadRequest, "Validation failed."),

            ExternalServiceException =>
                (StatusCodes.Status502BadGateway,
                    "Text improvement service is temporarily unavailable."),

            _ =>
                (StatusCodes.Status500InternalServerError,
                    "An unexpected error occurred.")
        };

        await Results.Problem(
            statusCode: statusCode,
            title: title
        ).ExecuteAsync(httpContext);

        return true;
    }
}