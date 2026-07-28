
using BuildHub.Models;
using BuildHub.Repos;
using BuildHub.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;

namespace BuildHub
{
    public class Program
    {
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            
            // 1. SERVICE CONTAINER 
            
            // Add services to the container.
                        
            builder.Services.AddDbContext<ProjectContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
            
            //repos 
            builder.Services.AddScoped<QuoteRequestRepo>();
            builder.Services.AddScoped<QuoteRequestInviteRepo>();
            builder.Services.AddScoped<QuoteRepo>();
            builder.Services.AddScoped<EscrowTransactionRepo>();
            builder.Services.AddScoped<ReviewRepo>();
            builder.Services.AddScoped<UserRepo>();
            builder.Services.AddScoped<VendorProfileRepo>();
            builder.Services.AddScoped<ProjectRepo>();
            builder.Services.AddScoped<QuoteNegotiationRepo>();
            builder.Services.AddScoped<MilestoneRepo>();
            builder.Services.AddScoped<contractRepo>();
            builder.Services.AddScoped<NotificationRepo>();
            builder.Services.AddScoped<CategoryRepo>();
            builder.Services.AddScoped<ProductRepo>();

            //services 
            //builder.Services.AddScoped<AuthService>();
            builder.Services.AddScoped<EmailService>(); 
            builder.Services.AddScoped<QuoteRequestService>();
            builder.Services.AddScoped<QuoteService>();
            builder.Services.AddScoped<EscrowTransactionService>(); 
            builder.Services.AddScoped<VendorProfileService>();
            builder.Services.AddScoped<ContractService>();
            builder.Services.AddScoped<NotificationService>();
            builder.Services.AddScoped<UserService>();
            builder.Services.AddScoped<MilestoneService>();
            builder.Services.AddScoped<QuoteNegotiationService>();
            builder.Services.AddScoped<CategoryService>();
            builder.Services.AddScoped<ProductService>();
            builder.Services.AddScoped<QuoteRequestInviteService>();


            //Authentication (JWT) & Authorization


            //controllers
            builder.Services.AddControllers().AddJsonOptions(options =>
            {
                options.JsonSerializerOptions.ReferenceHandler =
                    System.Text.Json.Serialization.ReferenceHandler.IgnoreCycles;
            });

            //swagger 

            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen(options =>
            {
                options.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Contracting & Home Decor Platform API",
                    Version = "v1",
                    Description = "Demo path (no auth): browse vendors/reviews -> direct quote request -> quote -> accept -> contract -> escrow."
                });
            });
            
            
            var app = builder.Build();
            
            // 2. MIDDLEWARE PIPELINE
            
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI(options =>
                {
                    options.SwaggerEndpoint("/swagger/v1/swagger.json", "Platform API v1");
                });
                app.MapGet("/", () => Results.Redirect("/swagger")); //to directly go to swagger 
            }

            app.UseHttpsRedirection();

            //app.UseAuthorization();
            
            app.MapControllers();


            //dataseed 

            using (var scope = app.Services.CreateScope())
            {
                var db = scope.ServiceProvider.GetRequiredService<ProjectContext>();
                await db.Database.MigrateAsync();          // applies any pending migrations

                // Dev-only: set "Seeding:ResetOnStartup": true in appsettings.Development.json
                // to wipe all seeded tables and reseed from scratch on every startup.
                if (app.Environment.IsDevelopment() &&
                    builder.Configuration.GetValue<bool>("Seeding:ResetOnStartup"))
                {
                    await DbSeeder.ResetAsync(db);
                }

                await DbSeeder.SeedAsync(db);               // inserts demo data if not already present
            }

            app.Run();
        }
    }
}
