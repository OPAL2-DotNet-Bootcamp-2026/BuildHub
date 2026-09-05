using Backend.Models;
using Backend.Models.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace Backend.Data
{
    /// <summary>
    /// Fills an empty database with a coherent slice of BuildHub: categories, accounts,
    /// and jobs at every stage of the flow - open, hired, completed and cancelled.
    ///
    /// The data respects the same invariants the services enforce, so it is safe to
    /// browse and to build on: exactly one Accepted offer per hired job, an agreement
    /// only behind an accepted offer, released escrow reflected in the vendor's balance,
    /// reviews only on completed agreements by that job's own homeowner, and
    /// AverageRating equal to the mean of the reviews actually present.
    /// </summary>
    public static class DataSeeder
    {
        /// <summary>The password every seeded account shares, for local testing.</summary>
        public const string SeedPassword = "Password123!";

        /// <summary>
        /// Does nothing if any user already exists, so it is safe to call on every start.
        /// </summary>
        public static async Task SeedAsync(BuildHubDbContext context, IPasswordHasher<User> passwordHasher)
        {
            if (await context.Users.AnyAsync()) return;

            var now = DateTime.UtcNow;

            User NewUser(string fullName, string email, string phone, UserRole role, string city, int daysAgo)
            {
                var user = new User
                {
                    FullName = fullName,
                    Email = email,
                    PhoneNumber = phone,
                    Role = role,
                    City = city,
                    CreatedAt = now.AddDays(-daysAgo)
                };
                user.PasswordHash = passwordHasher.HashPassword(user, SeedPassword);
                return user;
            }

            // --- Categories -------------------------------------------------------
            var kitchens = new Category { NameAr = "مطابخ", NameEn = "Kitchens" };
            var ceramics = new Category { NameAr = "سيراميك", NameEn = "Ceramics" };
            var electrical = new Category { NameAr = "كهرباء", NameEn = "Electrical" };
            var plumbing = new Category { NameAr = "سباكة", NameEn = "Plumbing" };
            var painting = new Category { NameAr = "أصباغ", NameEn = "Painting" };
            var interiors = new Category { NameAr = "تصميم داخلي", NameEn = "Interior Design" };

            // --- Accounts ---------------------------------------------------------
            var admin = NewUser("Layla Al Saadi", "admin@buildhub.om", "+96895000001", UserRole.Admin, "Muscat", 120);

            var salim = NewUser("Salim Al Harthy", "salim@example.om", "+96895000010", UserRole.Homeowner, "Muscat", 90);
            var fatma = NewUser("Fatma Al Balushi", "fatma@example.om", "+96895000011", UserRole.Homeowner, "Salalah", 75);
            var yousef = NewUser("Yousef Al Rashdi", "yousef@example.om", "+96895000012", UserRole.Homeowner, "Sohar", 60);
            var mariam = NewUser("Mariam Al Hinai", "mariam@example.om", "+96895000013", UserRole.Homeowner, "Nizwa", 50);

            var aishaUser = NewUser("Aisha Al Kindi", "aisha@example.om", "+96895000020", UserRole.Vendor, "Muscat", 100);
            var khalidUser = NewUser("Khalid Al Amri", "khalid@example.om", "+96895000021", UserRole.Vendor, "Salalah", 95);
            var nasserUser = NewUser("Nasser Al Siyabi", "nasser@example.om", "+96895000022", UserRole.Vendor, "Sohar", 85);
            var hudaUser = NewUser("Huda Al Zadjali", "huda@example.om", "+96895000023", UserRole.Vendor, "Nizwa", 80);
            var talalUser = NewUser("Talal Al Maskari", "talal@example.om", "+96895000024", UserRole.Vendor, "Muscat", 70);

            // --- Vendor profiles --------------------------------------------------
            // Balance and AverageRating are filled in to match the agreements and
            // reviews below, not invented.
            var aisha = new VendorProfile
            {
                User = aishaUser, CompanyName = "Muscat Modern Kitchens", VendorType = VendorType.Contractor,
                Category = kitchens, City = "Muscat", IsVerified = true,
                Bio = "Fitted kitchens and cabinetry across the capital area since 2014.",
                AverageRating = 4.50m,          // mean of the 5 and 4 left below
                Balance = 3670.000m             // 2350.000 + 1320.000 released
            };
            var khalid = new VendorProfile
            {
                User = khalidUser, CompanyName = "Dhofar Interiors", VendorType = VendorType.Designer,
                Category = interiors, City = "Salalah", IsVerified = true,
                Bio = "Majlis and living space design with a Dhofari palette.",
                AverageRating = null, Balance = 0m
            };
            var nasser = new VendorProfile
            {
                User = nasserUser, CompanyName = "Sohar Ceramics Store", VendorType = VendorType.Store,
                Category = ceramics, City = "Sohar", IsVerified = true,
                Bio = "Imported floor and wall tile, sold by the square metre.",
                AverageRating = null, Balance = 0m
            };
            var huda = new VendorProfile
            {
                User = hudaUser, CompanyName = "Nizwa Electric Works", VendorType = VendorType.Contractor,
                Category = electrical, City = "Nizwa", IsVerified = false,
                Bio = "Domestic rewiring and lighting installation.",
                AverageRating = null, Balance = 0m
            };
            var talal = new VendorProfile
            {
                User = talalUser, CompanyName = "Capital Plumbing", VendorType = VendorType.Contractor,
                Category = plumbing, City = "Muscat", IsVerified = false,
                Bio = "Bathroom refits, leak tracing and pipework.",
                AverageRating = null, Balance = 0m
            };

            // --- Job 1: the full flow, finished and reviewed -----------------------
            var job1 = new Job
            {
                Homeowner = salim, Category = kitchens, Title = "Kitchen renovation in Al Khuwair",
                Description = "Strip out the existing kitchen and fit new units, worktop and sink.",
                City = "Muscat", Budget = 2500.000m, Deadline = now.AddDays(-10),
                Status = JobStatus.Completed, CreatedAt = now.AddDays(-45)
            };
            var offer1Won = new Offer
            {
                Job = job1, VendorProfile = aisha, Price = 2350.000m, DurationDays = 21,
                Message = "Includes removal of the old units and two-year workmanship cover.",
                Status = OfferStatus.Accepted, SubmittedAt = now.AddDays(-43)
            };
            var offer1Lost = new Offer
            {
                Job = job1, VendorProfile = talal, Price = 2600.000m, DurationDays = 18,
                Message = "Can start next week.",
                Status = OfferStatus.NotSelected, SubmittedAt = now.AddDays(-42)
            };
            var agreement1 = new Agreement
            {
                Offer = offer1Won, TotalAmount = 2350.000m,
                Status = AgreementStatus.Completed, PaymentStatus = PaymentStatus.Released,
                HeldAt = now.AddDays(-40), ReleasedAt = now.AddDays(-12), StartedAt = now.AddDays(-40)
            };
            var review1 = new Review
            {
                Reviewer = salim, VendorProfile = aisha, Agreement = agreement1, Rating = 5,
                Comment = "Finished ahead of the date they quoted and left the flat spotless.",
                ReviewDate = now.AddDays(-11)
            };

            // --- Job 2: hired, work under way, money still in escrow ---------------
            var job2 = new Job
            {
                Homeowner = fatma, Category = interiors, Title = "Majlis interior design",
                Description = "Full design for a 40 square metre majlis, including furniture selection.",
                City = "Salalah", Budget = 1800.000m, Deadline = now.AddDays(25),
                Status = JobStatus.Hired, CreatedAt = now.AddDays(-20)
            };
            var offer2Won = new Offer
            {
                Job = job2, VendorProfile = khalid, Price = 1750.500m, DurationDays = 30,
                Message = "Two concept rounds and a final furniture list.",
                Status = OfferStatus.Accepted, SubmittedAt = now.AddDays(-18)
            };
            var offer2Lost = new Offer
            {
                Job = job2, VendorProfile = aisha, Price = 1900.000m, DurationDays = 24,
                Message = null, Status = OfferStatus.NotSelected, SubmittedAt = now.AddDays(-17)
            };
            var agreement2 = new Agreement
            {
                Offer = offer2Won, TotalAmount = 1750.500m,
                Status = AgreementStatus.Active, PaymentStatus = PaymentStatus.Held,
                HeldAt = now.AddDays(-15), ReleasedAt = null, StartedAt = now.AddDays(-15)
            };

            // --- Job 3: open, two vendors waiting on an answer ---------------------
            var job3 = new Job
            {
                Homeowner = yousef, Category = electrical, Title = "Rewire a two-bedroom flat",
                Description = "Complete rewire including a new consumer unit and eight sockets.",
                City = "Sohar", Budget = 900.000m, Deadline = now.AddDays(40),
                Status = JobStatus.Open, CreatedAt = now.AddDays(-6)
            };
            var offer3A = new Offer
            {
                Job = job3, VendorProfile = huda, Price = 850.750m, DurationDays = 10,
                Message = "Certified consumer unit included.",
                Status = OfferStatus.Pending, SubmittedAt = now.AddDays(-5)
            };
            var offer3B = new Offer
            {
                Job = job3, VendorProfile = talal, Price = 920.000m, DurationDays = 7,
                Message = null, Status = OfferStatus.Pending, SubmittedAt = now.AddDays(-4)
            };

            // --- Job 4: open, no offers yet ----------------------------------------
            var job4 = new Job
            {
                Homeowner = mariam, Category = ceramics, Title = "Bathroom re-tiling",
                Description = "Re-tile floor and walls of a family bathroom, roughly 18 square metres.",
                City = "Nizwa", Budget = 600.000m, Deadline = now.AddDays(30),
                Status = JobStatus.Open, CreatedAt = now.AddDays(-2)
            };

            // --- Job 5: cancelled before anyone was hired --------------------------
            var job5 = new Job
            {
                Homeowner = salim, Category = painting, Title = "Villa exterior painting",
                Description = "Two coats of weather-resistant paint on a two-storey villa.",
                City = "Muscat", Budget = 1200.000m, Deadline = null,
                Status = JobStatus.Cancelled, CreatedAt = now.AddDays(-35)
            };

            // --- Job 6: a second completed job, giving Aisha two ratings -----------
            var job6 = new Job
            {
                Homeowner = mariam, Category = kitchens, Title = "Kitchen cabinet replacement",
                Description = "Replace upper and lower cabinet doors, keep the existing carcasses.",
                City = "Nizwa", Budget = 1400.000m, Deadline = now.AddDays(-5),
                Status = JobStatus.Completed, CreatedAt = now.AddDays(-30)
            };
            var offer6Won = new Offer
            {
                Job = job6, VendorProfile = aisha, Price = 1320.000m, DurationDays = 12,
                Message = "Matching handles included.",
                Status = OfferStatus.Accepted, SubmittedAt = now.AddDays(-28)
            };
            var agreement3 = new Agreement
            {
                Offer = offer6Won, TotalAmount = 1320.000m,
                Status = AgreementStatus.Completed, PaymentStatus = PaymentStatus.Released,
                HeldAt = now.AddDays(-27), ReleasedAt = now.AddDays(-6), StartedAt = now.AddDays(-27)
            };
            var review2 = new Review
            {
                Reviewer = mariam, VendorProfile = aisha, Agreement = agreement3, Rating = 4,
                Comment = "Good work overall, though delivery of the doors slipped by a few days.",
                ReviewDate = now.AddDays(-5)
            };

            // --- Products: the store's catalogue, for price comparison only --------
            var products = new[]
            {
                new Product { VendorProfile = nasser, Category = ceramics, Name = "Porcelain floor tile 60x60",
                    Unit = ProductUnit.SquareMeter, Price = 4.750m, IsAvailable = true },
                new Product { VendorProfile = nasser, Category = ceramics, Name = "Ceramic wall tile 30x60",
                    Unit = ProductUnit.SquareMeter, Price = 3.250m, IsAvailable = true },
                new Product { VendorProfile = nasser, Category = ceramics, Name = "Marble threshold strip",
                    Unit = ProductUnit.Piece, Price = 12.000m, IsAvailable = true },
                new Product { VendorProfile = nasser, Category = ceramics, Name = "Mosaic border set",
                    Unit = ProductUnit.Set, Price = 85.500m, IsAvailable = false },
                new Product { VendorProfile = talal, Category = plumbing, Name = "PVC pipe 4 inch",
                    Unit = ProductUnit.Piece, Price = 2.100m, IsAvailable = true },
                new Product { VendorProfile = talal, Category = plumbing, Name = "Mixer tap, brushed steel",
                    Unit = ProductUnit.Piece, Price = 28.900m, IsAvailable = true }
            };

            // --- Notifications: the trail the flow above would have left -----------
            var notifications = new[]
            {
                new Notification { User = salim, Message = "Muscat Modern Kitchens sent an offer on your kitchen renovation.",
                    Type = NotificationType.OfferReceived, IsRead = true, CreatedAt = now.AddDays(-43) },
                new Notification { User = aishaUser, Message = "Your offer on the kitchen renovation was accepted.",
                    Type = NotificationType.OfferAccepted, IsRead = true, CreatedAt = now.AddDays(-40) },
                new Notification { User = talalUser, Message = "Another offer was selected for the kitchen renovation.",
                    Type = NotificationType.OfferNotSelected, IsRead = true, CreatedAt = now.AddDays(-40) },
                new Notification { User = aishaUser, Message = "Payment of 2350.000 OMR was released to your balance.",
                    Type = NotificationType.PaymentReleased, IsRead = true, CreatedAt = now.AddDays(-12) },
                new Notification { User = aishaUser, Message = "Salim Al Harthy left you a 5 star review.",
                    Type = NotificationType.ReviewReceived, IsRead = false, CreatedAt = now.AddDays(-11) },
                new Notification { User = fatma, Message = "Your majlis design agreement is active and payment is held.",
                    Type = NotificationType.AgreementStarted, IsRead = false, CreatedAt = now.AddDays(-15) },
                new Notification { User = yousef, Message = "Nizwa Electric Works sent an offer on your rewiring job.",
                    Type = NotificationType.OfferReceived, IsRead = false, CreatedAt = now.AddDays(-5) },
                new Notification { User = mariam, Message = "Your kitchen cabinet job is complete.",
                    Type = NotificationType.JobCompleted, IsRead = false, CreatedAt = now.AddDays(-6) }
            };

            context.AddRange(kitchens, ceramics, electrical, plumbing, painting, interiors);
            context.AddRange(admin, salim, fatma, yousef, mariam);
            context.AddRange(aisha, khalid, nasser, huda, talal);
            context.AddRange(job1, job2, job3, job4, job5, job6);
            context.AddRange(offer1Won, offer1Lost, offer2Won, offer2Lost, offer3A, offer3B, offer6Won);
            context.AddRange(agreement1, agreement2, agreement3);
            context.AddRange(review1, review2);
            context.AddRange(products);
            context.AddRange(notifications);

            await context.SaveChangesAsync();

            // RelatedId deep-links point at rows whose ids only exist after the insert.
            notifications[0].RelatedId = offer1Won.OfferId;
            notifications[1].RelatedId = offer1Won.OfferId;
            notifications[2].RelatedId = offer1Lost.OfferId;
            notifications[3].RelatedId = agreement1.AgreementId;
            notifications[4].RelatedId = review1.ReviewId;
            notifications[5].RelatedId = agreement2.AgreementId;
            notifications[6].RelatedId = offer3A.OfferId;
            notifications[7].RelatedId = job6.JobId;

            await context.SaveChangesAsync();
        }
    }
}
