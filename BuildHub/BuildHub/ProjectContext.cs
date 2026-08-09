using BuildHub.Enums;
using BuildHub.Models;
using Microsoft.EntityFrameworkCore;

namespace BuildHub
{
    public class ProjectContext : DbContext
    {
        public DbSet<Category> Categories { get; set; }
        public DbSet<Contract> Contracts { get; set; }
        public DbSet<EscrowTransaction> EscrowTransactions { get; set; }
        public DbSet<Milestone> Milestones { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Product> Products { get; set; }
        public DbSet<Project> Projects { get; set; }
        public DbSet<Quote> Quotes { get; set; }
        public DbSet<QuoteNegotiation> QuoteNegotiations { get; set; }
        public DbSet<QuoteRequest> QuoteRequests { get; set; }
        public DbSet<QuoteRequestInvite> QuoteRequestInvites { get; set; }
        public DbSet<Review> Reviews { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<VendorProfile> VendorProfiles { get; set; }
        
        public ProjectContext(DbContextOptions<ProjectContext> options) : base(options)
        {
            
        }

        //this method convert the value from enum from number (index) to string
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<QuoteRequestInvite>()
                .HasOne(i => i.VendorProfile)
                .WithMany(v => v.QuoteRequestInvites) 
                .HasForeignKey(i => i.VendorProfileId)
                .OnDelete(DeleteBehavior.Restrict); // or Restrict
            
            modelBuilder.Entity<Quote>()
                .HasOne(q => q.VendorProfile)
                .WithMany(v => v.Quotes)
                .HasForeignKey(q => q.VendorProfileId)
                .OnDelete(DeleteBehavior.Restrict);
            
            modelBuilder.Entity<QuoteNegotiation>()
                .HasOne(n => n.User)
                .WithMany(u => u.QuoteNegotiations)
                .HasForeignKey(n => n.UserId)
                .OnDelete(DeleteBehavior.Restrict);
            
            modelBuilder.Entity<Project>()
                .Property(p => p.Status)
                .HasConversion<string>() // Converts enum to string for database operations
                .HasMaxLength(20) //navchar(20)
                .HasDefaultValue(ProjectStatus.Draft); //default value "draft"

            modelBuilder.Entity<VendorProfile>()
                .Property(v => v.VendorType)
                .HasConversion<string>() // Converts enum to string for database operations
                .HasMaxLength(20); //navchar(20)
            //default value "draft"
        }
    }
}
