
using System.Security.Claims;
using System.Text;
using Backend.Configuration;
using Backend.Data;
using Backend.Middleware;
using Backend.Models.Entities;
using Backend.OpenApi;
using Backend.Repositories.Implementations;
using Backend.Repositories.Interfaces;
using Backend.Services.Implementations;
using Backend.Services.Interfaces;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace Backend
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddDbContext<BuildHubDbContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("DefaultConnection")));

            // Repositories. Scoped, so each one shares the request's DbContext.
            builder.Services.AddScoped<IUserRepository, UserRepository>();
            builder.Services.AddScoped<IVendorProfileRepository, VendorProfileRepository>();
            builder.Services.AddScoped<ICategoryRepository, CategoryRepository>();
            builder.Services.AddScoped<IJobRepository, JobRepository>();
            builder.Services.AddScoped<IOfferRepository, OfferRepository>();
            builder.Services.AddScoped<IAgreementRepository, AgreementRepository>();
            builder.Services.AddScoped<IProductRepository, ProductRepository>();
            builder.Services.AddScoped<IReviewRepository, ReviewRepository>();
            builder.Services.AddScoped<INotificationRepository, NotificationRepository>();

            // Services.
            builder.Services.AddScoped<ICurrentUser, CurrentUser>();
            builder.Services.AddScoped<IUserService, UserService>();
            builder.Services.AddScoped<IVendorProfileService, VendorProfileService>();
            builder.Services.AddScoped<ICategoryService, CategoryService>();
            builder.Services.AddScoped<IJobService, JobService>();
            builder.Services.AddScoped<IOfferService, OfferService>();
            builder.Services.AddScoped<IAgreementService, AgreementService>();
            builder.Services.AddScoped<IProductService, ProductService>();
            builder.Services.AddScoped<IReviewService, ReviewService>();
            builder.Services.AddScoped<INotificationService, NotificationService>();

            // Password hashing (PBKDF2, salted per user) from ASP.NET Core Identity.
            builder.Services.AddSingleton<IPasswordHasher<User>, PasswordHasher<User>>();

            // Bearer tokens. Validated at startup so a missing or too-short signing
            // key stops the app here rather than producing tokens nobody can trust.
            var jwtSettings = builder.Configuration.GetSection(JwtSettings.SectionName)
                .Get<JwtSettings>() ?? new JwtSettings();
            jwtSettings.Validate();
            builder.Services.Configure<JwtSettings>(
                builder.Configuration.GetSection(JwtSettings.SectionName));
            builder.Services.AddSingleton<ITokenService, JwtTokenService>();
            builder.Services.AddScoped<IAuthService, AuthService>();

            builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidIssuer = jwtSettings.Issuer,
                        ValidateAudience = true,
                        ValidAudience = jwtSettings.Audience,
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(
                            Encoding.UTF8.GetBytes(jwtSettings.Key)),
                        ValidateLifetime = true,
                        // The default five minutes would keep expired tokens working.
                        ClockSkew = TimeSpan.FromSeconds(30),
                        NameClaimType = ClaimTypes.NameIdentifier,
                        RoleClaimType = ClaimTypes.Role
                    };
                });

            // Deny by default: an endpoint with no authorization attribute of its own
            // still requires a signed-in caller, so a controller added later is
            // protected without anyone having to remember. Genuinely public routes
            // opt out with [AllowAnonymous].
            builder.Services.AddAuthorization(options =>
                options.FallbackPolicy = new AuthorizationPolicyBuilder()
                    .RequireAuthenticatedUser()
                    .Build());

            builder.Services.AddHttpContextAccessor();

            // Maps the service layer's domain exceptions onto 404 / 400 / 409 in one
            // place, so controllers stay free of repeated try/catch.
            builder.Services.AddProblemDetails();
            builder.Services.AddExceptionHandler<DomainExceptionHandler>();

            builder.Services.AddControllers();
            // Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
            builder.Services.AddOpenApi(options =>
                options.AddDocumentTransformer<BearerSecuritySchemeTransformer>());

            var app = builder.Build();

            // Local convenience only, and opt-in: appsettings.Development.json sets
            // "SeedData": true. Applies any pending migrations, then fills an empty
            // database with sample data. Turn the flag off to start from nothing.
            if (app.Configuration.GetValue<bool>("SeedData"))
            {
                using var scope = app.Services.CreateScope();
                var context = scope.ServiceProvider.GetRequiredService<BuildHubDbContext>();
                await context.Database.MigrateAsync();
                await DataSeeder.SeedAsync(
                    context, scope.ServiceProvider.GetRequiredService<IPasswordHasher<User>>());
            }

            app.UseExceptionHandler();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                // Serves the generated OpenAPI document at /openapi/v1.json.
                // AllowAnonymous because the fallback policy above would otherwise
                // put the document itself behind a token, leaving Swagger UI empty.
                app.MapOpenApi().AllowAnonymous();

                // Swagger UI reads that same document - it only renders, it does not
                // generate, so the document stays the single source of truth.
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/openapi/v1.json", "BuildHub API v1");
                    options.DocumentTitle = "BuildHub API";
                });
            }

            app.UseHttpsRedirection();

            // Order matters: authentication establishes who the caller is, and
            // authorization then decides what they may do.
            app.UseAuthentication();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
