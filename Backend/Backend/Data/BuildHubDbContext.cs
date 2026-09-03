using Backend.Models;
using Microsoft.EntityFrameworkCore;

namespace Backend.Data
{
    /// <summary>
    /// EF Core context for BuildHub. Keys, uniqueness, precision and string lengths
    /// live on the models as data annotations; only what annotations cannot express
    /// is configured here.
    /// </summary>
    public class BuildHubDbContext : DbContext
    {
        public BuildHubDbContext(DbContextOptions<BuildHubDbContext> options)
            : base(options)
        {
        }

        public DbSet<User> Users => Set<User>();
        public DbSet<VendorProfile> VendorProfiles => Set<VendorProfile>();
        public DbSet<Category> Categories => Set<Category>();
        public DbSet<Job> Jobs => Set<Job>();
        public DbSet<Offer> Offers => Set<Offer>();
        public DbSet<Agreement> Agreements => Set<Agreement>();
        public DbSet<Product> Products => Set<Product>();
        public DbSet<Review> Reviews => Set<Review>();
        public DbSet<Notification> Notifications => Set<Notification>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Delete behaviour cannot be set with data annotations, so it must be here.
            // Every required FK cascades by default, which both loses records we need to
            // keep and makes SQL Server reject the schema outright (error 1785: multiple
            // cascade paths reach Users - directly through Review.ReviewerId, and again
            // through Job -> Offer -> Agreement and through VendorProfile).

            // A category must never be able to wipe every job, vendor or product under it.
            modelBuilder.Entity<VendorProfile>()
                .HasOne(v => v.Category)
                .WithMany(c => c.VendorProfiles)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Job>()
                .HasOne(j => j.Category)
                .WithMany(c => c.Jobs)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Product>()
                .HasOne(p => p.Category)
                .WithMany(c => c.Products)
                .OnDelete(DeleteBehavior.Restrict);

            // Jobs and reviews outlive the account that made them: they carry money
            // and the rating history that VendorProfile.AverageRating is derived from.
            modelBuilder.Entity<Job>()
                .HasOne(j => j.Homeowner)
                .WithMany(u => u.Jobs)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Review>()
                .HasOne(r => r.Reviewer)
                .WithMany(u => u.Reviews)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Review>()
                .HasOne(r => r.VendorProfile)
                .WithMany(v => v.Reviews)
                .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Offer>()
                .HasOne(o => o.VendorProfile)
                .WithMany(v => v.Offers)
                .OnDelete(DeleteBehavior.Restrict);

            // An agreement holds escrowed money - deleting the offer must never take it.
            modelBuilder.Entity<Agreement>()
                .HasOne(a => a.Offer)
                .WithOne(o => o.Agreement)
                .HasForeignKey<Agreement>(a => a.OfferId)
                .OnDelete(DeleteBehavior.Restrict);

            // Deliberately left cascading:
            //   User -> VendorProfile, User -> Notification,
            //   Job  -> Offer,         VendorProfile -> Product,
            //   Agreement -> Review
        }
    }
}
