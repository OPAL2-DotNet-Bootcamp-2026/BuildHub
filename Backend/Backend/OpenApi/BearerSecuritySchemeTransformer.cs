using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.OpenApi;
using Microsoft.OpenApi;

namespace Backend.OpenApi
{
    /// <summary>
    /// Declares the bearer scheme on the generated OpenAPI document, which is what
    /// gives Swagger UI its Authorize button. Without it the UI can render the
    /// endpoints but cannot send a token to any of them.
    /// </summary>
    public sealed class BearerSecuritySchemeTransformer : IOpenApiDocumentTransformer
    {
        private readonly IAuthenticationSchemeProvider _schemeProvider;

        public BearerSecuritySchemeTransformer(IAuthenticationSchemeProvider schemeProvider)
        {
            _schemeProvider = schemeProvider;
        }

        public async Task TransformAsync(
            OpenApiDocument document,
            OpenApiDocumentTransformerContext context,
            CancellationToken cancellationToken)
        {
            var schemes = await _schemeProvider.GetAllSchemesAsync();
            if (!schemes.Any(scheme => scheme.Name == "Bearer"))
            {
                return;
            }

            var bearer = new OpenApiSecurityScheme
            {
                Type = SecuritySchemeType.Http,
                Scheme = "bearer",
                BearerFormat = "JWT",
                In = ParameterLocation.Header,
                Description = "Paste the token from POST /api/Auth/login. No \"Bearer \" prefix needed."
            };

            document.Components ??= new OpenApiComponents();
            document.Components.SecuritySchemes ??= new Dictionary<string, IOpenApiSecurityScheme>();
            document.Components.SecuritySchemes["Bearer"] = bearer;
        }
    }
}
