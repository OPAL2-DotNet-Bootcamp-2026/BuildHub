
using BuildHub.Repos;
using BuildHub.Services;
using Microsoft.EntityFrameworkCore;

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

            var app = builder.Build();
            
            // 2. MIDDLEWARE PIPELINE
            
            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                
            }

            app.UseHttpsRedirection();

            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
