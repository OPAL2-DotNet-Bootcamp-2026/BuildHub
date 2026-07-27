
using BuildHub.Repos;
using BuildHub.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi;

namespace BuildHub
{
    public class Program
    {
        public static void Main(string[] args)
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
            //builder.Services.AddScoped<ProjectRepo>();
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
            //category, milestone, product, QuoteNegotiation, QuoteRequestInvite, UserService
            
            //Authentication (JWT) & Authorization
            
            
            //controllers
            builder.Services.AddControllers();

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

            app.Run();
        }
    }
}
