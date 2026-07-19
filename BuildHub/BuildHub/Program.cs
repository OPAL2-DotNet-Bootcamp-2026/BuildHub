
namespace BuildHub
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            
            // 1. SERVICE CONTAINER 
            
            // Add services to the container.

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
