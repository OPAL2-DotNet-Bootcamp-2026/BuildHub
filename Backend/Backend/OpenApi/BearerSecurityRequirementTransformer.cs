using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace Backend.OpenApi
{
    /// <summary>
    /// Marks each protected operation as requiring the bearer scheme. Declaring the
    /// scheme is only half the job: Swagger UI attaches the token from the Authorize
    /// dialog to an operation only when that operation asks for it, so without this
    /// the requests go out anonymous and come back 401 while Postman - which sets the
    /// header by hand - succeeds. Endpoints marked [AllowAnonymous] are left alone;
    /// everything else is protected by the fallback policy in Program.
    /// </summary>
    public sealed class BearerSecurityRequirementTransformer : IOpenApiOperationTransformer
    {
        public Task TransformAsync(
            OpenApiOperation operation,
            OpenApiOperationTransformerContext context,
            CancellationToken cancellationToken)
        {
            var allowsAnonymous = context.Description.ActionDescriptor.EndpointMetadata
                .OfType<IAllowAnonymous>()
                .Any();

            if (allowsAnonymous)
            {
                return Task.CompletedTask;
            }

            operation.Security ??= new List<OpenApiSecurityRequirement>();
            operation.Security.Add(new OpenApiSecurityRequirement
            {
                [new OpenApiSecuritySchemeReference("Bearer", context.Document)] = new List<string>()
            });

            return Task.CompletedTask;
        }
    }
}