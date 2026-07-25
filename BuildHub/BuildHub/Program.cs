
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
            builder.Services.AddScoped<UserRepo>();

            //services 
            builder.Services.AddScoped<UserService>();

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
