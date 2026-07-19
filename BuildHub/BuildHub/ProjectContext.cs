using BuildHub.Models;
using Microsoft.EntityFrameworkCore;

namespace BuildHub
{
    public class ProjectContext : DbContext
    {
        DbSet<User> Users { get; set; }

        public ProjectContext(DbContextOptions<ProjectContext> options) : base(options)
        {
            
        }
    }
}
