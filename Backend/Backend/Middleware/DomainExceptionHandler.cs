using Backend.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace Backend.Middleware
{
    /// <summary>
    /// Turns the service layer's domain exceptions into the HTTP status they mean, so
    /// no controller has to repeat the same try/catch. Anything else is left alone and
    /// still surfaces as a 500 - only the three deliberate signals are translated.
    /// </summary>
    public class DomainExceptionHandler : IExceptionHandler
    {
        private readonly IProblemDetailsService _problemDetailsService;

        public DomainExceptionHandler(IProblemDetailsService problemDetailsService)
        {
            _problemDetailsService = problemDetailsService;
        }

        public async ValueTask<bool> TryHandleAsync(
            HttpContext httpContext,
            Exception exception,
            CancellationToken cancellationToken)
        {
            var (status, title) = exception switch
            {
                NotFoundException => (StatusCodes.Status404NotFound, "Not found"),
                BadRequestException => (StatusCodes.Status400BadRequest, "Bad request"),
                ConflictException => (StatusCodes.Status409Conflict, "Conflict"),
                _ => (0, string.Empty)
            };

            // Not one of ours: return false so the default pipeline still reports it.
            if (status == 0) return false;

            httpContext.Response.StatusCode = status;

            return await _problemDetailsService.TryWriteAsync(new ProblemDetailsContext
            {
                HttpContext = httpContext,
                ProblemDetails = new ProblemDetails
                {
                    Status = status,
                    Title = title,
                    Detail = exception.Message
                }
            });
        }
    }
}
