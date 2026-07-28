using BuildHub.Models;
using Microsoft.EntityFrameworkCore;


namespace BuildHub
{

    /// <summary>
    /// Runtime data seeder — inserts demo data via the DbContext (not migrations),
    /// so it can freely wire up real foreign keys (self-referencing categories,
    /// vendor -> user, review -> vendor/product/contract) instead of hardcoding IDs.
    /// Safe to call every startup: it checks for existing data first.
    /// </summary>
    public static class DbSeeder
    {
        public static async Task SeedAsync(ProjectContext context)
        {
            // Guard clause: only seed once. If Categories already has rows, assume
            // the whole seed already ran and skip everything below.
            if (await context.Categories.AnyAsync())
                return;

            // ---- Categories (self-referencing: parent -> children) ----
            var kitchens = new Category { nameEn = "Kitchens", nameAr = "مطابخ", type = "Service" };
            var cabinets = new Category { nameEn = "Cabinets", nameAr = "خزائن", type = "Service", ParentCategory = kitchens };
            var ceramics = new Category { nameEn = "Ceramics", nameAr = "سيراميك", type = "Material" };

            context.Categories.AddRange(kitchens, cabinets, ceramics);
            await context.SaveChangesAsync(); // save now so generated CategoryIds exist for anything referencing them below

            // ---- Users (one client, one vendor-owning user) ----
            var clientUser = new User
            {
                FullName = "Sara Al-Balushi",
                Email = "sara.client@example.com",
                PasswordHash = "seeded-no-login", // no auth in this build — placeholder only
                PhoneNumber = "+96890000001",
                Role = "Client",
                City = "Muscat",
                IsVerified = true,
                CreatedAt = DateTime.UtcNow
            };

            var vendorUser = new User
            {
                FullName = "Khalid Al-Habsi",
                Email = "khalid.vendor@example.com",
                PasswordHash = "seeded-no-login",
                PhoneNumber = "+96890000002",
                Role = "Contractor",
                City = "Muscat",
                IsVerified = true,
                CreatedAt = DateTime.UtcNow
            };

            context.Users.AddRange(clientUser, vendorUser);
            await context.SaveChangesAsync(); // save now so generated UserIds exist for Project.ClientId and VendorProfile.UserId below

            // ---- Projects (linked to clientUser) ----
            // This is what QuoteRequest.projectId needs to reference — seed at least
            // one so QuoteRequest creation has a real, valid ProjectId to point at.
            var kitchenProject = new Project
            {
                ClientId = clientUser.UserId,
                Title = "Kitchen Renovation - Al Khuwair",
                Description = "Full kitchen renovation including cabinets, countertops, and ceramic flooring.",
                City = "Muscat",
                Budget = 3500m,
                Status = enums.ProjectStatus.Active,
                CreatedAt = DateTime.UtcNow
            };

            var bathroomProject = new Project
            {
                ClientId = clientUser.UserId,
                Title = "Bathroom Retiling - Qurum",
                Description = "Replace existing tiles with new ceramic flooring and wall tiles.",
                City = "Muscat",
                Budget = 1200m,
                Status = enums.ProjectStatus.Active,
                CreatedAt = DateTime.UtcNow
            };

            context.Projects.AddRange(kitchenProject, bathroomProject);
            await context.SaveChangesAsync(); // save now so generated ProjectIds exist for QuoteRequest.projectId

            // ---- VendorProfile (linked to vendorUser) ----
            var vendorProfile = new VendorProfile
            {
                UserId = vendorUser.UserId,
                CompanyName = "Al-Habsi Kitchens & Interiors",
                VendorType = enums.VendorType.Contractor,
                City = "Muscat",
                AverageRating = 4.5m,
                Balance = 0m
            };

            vendorProfile.IsVerfied = true;

            context.VendorProfiles.Add(vendorProfile);
            await context.SaveChangesAsync(); // save now so VendorProfileId exists for Review.VendorProfileId below

            // ---- Reviews (linked to vendorProfile + clientUser as reviewer) ----
            var reviews = new List<Review>
        {
            new()
            {
                ReviewerId = clientUser.UserId,
                VendorProfileId = vendorProfile.VendorProfileID,
                Rating = 5,
                Comment = "Finished the kitchen renovation on time and on budget.",
                ReviewDate = DateTime.UtcNow.AddDays(-14)
            },
            new()
            {
                ReviewerId = clientUser.UserId,
                VendorProfileId = vendorProfile.VendorProfileID,
                Rating = 4,
                Comment = "Good communication throughout, minor delay on cabinet delivery.",
                ReviewDate = DateTime.UtcNow.AddDays(-3)
            }
        };

            context.Reviews.AddRange(reviews);
            await context.SaveChangesAsync();
        }
    }
}