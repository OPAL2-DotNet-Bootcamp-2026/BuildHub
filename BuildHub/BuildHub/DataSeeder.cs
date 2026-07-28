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
        // Tables in FK-safe delete order (children before parents). Categories is
        // handled separately below since it self-references via parentCategoryId.
        private static readonly string[] TablesInDeleteOrder =
        {
            "Reviews",
            "EscrowTransactions",
            "Milestones",
            "QuoteNegotiations",
            "Contracts",
            "Notifications",
            "QuoteRequestInvites",
            "Quotes",
            "QuoteRequests",
            "Products",
            "VendorProfiles",
            "Projects",
            "Users"
        };

        /// <summary>
        /// Wipes every seeded table and resets identity counters, so SeedAsync can
        /// run again from a clean slate. Table names are hardcoded constants (not
        /// user input), so raw SQL string building here is safe.
        /// </summary>
        public static async Task ResetAsync(ProjectContext context)
        {
            // Table identifiers can't be SQL parameters, only values can — EF1002 is a
            // false positive here since TablesInDeleteOrder is a fixed internal constant.
#pragma warning disable EF1002
            foreach (var table in TablesInDeleteOrder)
                await context.Database.ExecuteSqlRawAsync($"DELETE FROM [{table}]");

            await context.Database.ExecuteSqlRawAsync("UPDATE [Categories] SET [parentCategoryId] = NULL");
            await context.Database.ExecuteSqlRawAsync("DELETE FROM [Categories]");

            // Reseeding to 0 makes the next inserted row get identity 1, EXCEPT the
            // very first time a table is ever reseeded from empty — SQL Server then
            // assigns the reseed value itself (0) to that one row. Harmless and
            // self-correcting: every reset after that first one starts IDs at 1.
            foreach (var table in TablesInDeleteOrder)
                await context.Database.ExecuteSqlRawAsync($"DBCC CHECKIDENT ('{table}', RESEED, 0)");
            await context.Database.ExecuteSqlRawAsync("DBCC CHECKIDENT ('Categories', RESEED, 0)");
#pragma warning restore EF1002
        }

        public static async Task SeedAsync(ProjectContext context)
        {
            // Guard clause: only seed once. If Categories already has rows, assume
            // the whole seed already ran and skip everything below.
            if (await context.Categories.AnyAsync())
                return;

            var now = DateTime.UtcNow;

            // ---- Categories (self-referencing: parent -> children) ----
            var kitchens = new Category { nameEn = "Kitchens", nameAr = "مطابخ", type = "Service" };
            var cabinets = new Category { nameEn = "Cabinets", nameAr = "خزائن", type = "Service", ParentCategory = kitchens };
            var ceramics = new Category { nameEn = "Ceramics", nameAr = "سيراميك", type = "Material" };

            context.Categories.AddRange(kitchens, cabinets, ceramics);
            await context.SaveChangesAsync(); // save now so generated CategoryIds exist for anything referencing them below

            // ---- Users (one client, two vendor-owning users) ----
            var clientUser = new User
            {
                FullName = "Sara Al-Balushi",
                Email = "sara.client@example.com",
                PasswordHash = "seeded-no-login", // no auth in this build — placeholder only
                PhoneNumber = "+96890000001",
                Role = "Client",
                City = "Muscat",
                IsVerified = true,
                CreatedAt = now
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
                CreatedAt = now
            };

            var vendorUser2 = new User
            {
                FullName = "Fatma Al-Riyami",
                Email = "fatma.vendor@example.com",
                PasswordHash = "seeded-no-login",
                PhoneNumber = "+96890000003",
                Role = "Store",
                City = "Muscat",
                IsVerified = true,
                CreatedAt = now
            };

            context.Users.AddRange(clientUser, vendorUser, vendorUser2);
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
                CreatedAt = now
            };

            var bathroomProject = new Project
            {
                ClientId = clientUser.UserId,
                Title = "Bathroom Retiling - Qurum",
                Description = "Replace existing tiles with new ceramic flooring and wall tiles.",
                City = "Muscat",
                Budget = 1200m,
                Status = enums.ProjectStatus.Active,
                CreatedAt = now
            };

            context.Projects.AddRange(kitchenProject, bathroomProject);
            await context.SaveChangesAsync(); // save now so generated ProjectIds exist for QuoteRequest.projectId

            // ---- VendorProfiles (one per vendor user — UserId is unique) ----
            var vendorProfile = new VendorProfile
            {
                UserId = vendorUser.UserId,
                CompanyName = "Al-Habsi Kitchens & Interiors",
                VendorType = enums.VendorType.Contractor,
                City = "Muscat",
                IsVerfied = true,
                AverageRating = 4.5m,
                Balance = 0m
            };

            var vendorProfile2 = new VendorProfile
            {
                UserId = vendorUser2.UserId,
                CompanyName = "Al-Riyami Ceramics & Tiles",
                VendorType = enums.VendorType.Store,
                City = "Muscat",
                IsVerfied = true,
                AverageRating = 4.0m,
                Balance = 0m
            };

            context.VendorProfiles.AddRange(vendorProfile, vendorProfile2);
            await context.SaveChangesAsync(); // save now so VendorProfileIds exist for Products, Quotes, Invites, Reviews below

            // ---- Products (linked to vendorProfile + category) ----
            var cabinetProduct = new Product
            {
                vendorProfileId = vendorProfile.VendorProfileID,
                categoryId = cabinets.categoryId,
                name = "Custom Kitchen Cabinet Set",
                unit = "Set",
                currentPrice = 1200m,
                isAvailable = true
            };

            var tileProduct = new Product
            {
                vendorProfileId = vendorProfile2.VendorProfileID,
                categoryId = ceramics.categoryId,
                name = "Premium Ceramic Floor Tile",
                unit = "m2",
                currentPrice = 8.5m,
                isAvailable = true
            };

            context.Products.AddRange(cabinetProduct, tileProduct);
            await context.SaveChangesAsync(); // save now so ProductIds exist for Reviews below

            // ---- QuoteRequests (linked to Project + Category) ----
            var kitchenQuoteRequest = new QuoteRequest
            {
                projectId = kitchenProject.ProjectId,
                categoryId = cabinets.categoryId,
                description = "Need custom cabinets designed and installed for a full kitchen renovation.",
                deadline = now.AddDays(30),
                visibilityType = "Direct",
                status = "Closed" // closed because it already turned into an accepted quote + contract below
            };

            var bathroomQuoteRequest = new QuoteRequest
            {
                projectId = bathroomProject.ProjectId,
                categoryId = ceramics.categoryId,
                description = "Need ceramic floor and wall tiles supplied and installed for a bathroom retile.",
                deadline = now.AddDays(20),
                visibilityType = "Public",
                status = "Open"
            };

            context.QuoteRequests.AddRange(kitchenQuoteRequest, bathroomQuoteRequest);
            await context.SaveChangesAsync(); // save now so QuoteRequestIds exist for Invites/Quotes below

            // ---- QuoteRequestInvites (linked to QuoteRequest + VendorProfile) ----
            var invites = new List<QuoteRequestInvite>
            {
                new()
                {
                    quoteRequestId = kitchenQuoteRequest.qutoeRequestId,
                    vendorProfileId = vendorProfile.VendorProfileID,
                    inviteStatus = "Accepted"
                },
                new()
                {
                    quoteRequestId = bathroomQuoteRequest.qutoeRequestId,
                    vendorProfileId = vendorProfile2.VendorProfileID,
                    inviteStatus = "Sent"
                }
            };

            context.QuoteRequestInvites.AddRange(invites);

            // ---- Quotes (linked to QuoteRequest + VendorProfile) ----
            var kitchenQuote = new Quote
            {
                quoteRequestId = kitchenQuoteRequest.qutoeRequestId,
                vendorProfileId = vendorProfile.VendorProfileID,
                price = 3200m,
                durationDays = 21,
                status = "Accepted",
                submittedAt = now.AddDays(-10)
            };

            var bathroomQuote = new Quote
            {
                quoteRequestId = bathroomQuoteRequest.qutoeRequestId,
                vendorProfileId = vendorProfile2.VendorProfileID,
                price = 950m,
                durationDays = 7,
                status = "Pending",
                submittedAt = now.AddDays(-2)
            };

            context.Quotes.AddRange(kitchenQuote, bathroomQuote);
            await context.SaveChangesAsync(); // save now so QuoteIds exist for QuoteNegotiation/Contract below

            // ---- QuoteNegotiation (linked to User + Quote) ----
            var negotiation = new QuoteNegotiation
            {
                UserId = clientUser.UserId,
                QuoteId = kitchenQuote.quoteId,
                proposedPrice = 3000m,
                proposedDurationDays = "18",
                message = "Can you do 3000 OMR and finish a few days earlier?",
                createIn = now.AddDays(-9)
            };

            context.QuoteNegotiations.Add(negotiation);

            // ---- Contract (one-to-one with the accepted Quote — quoteId is unique) ----
            var contract = new Contract
            {
                quoteId = kitchenQuote.quoteId,
                totalAmount = kitchenQuote.price,
                paymentType = "PreMilestone",
                status = "Active",
                signedAt = now.AddDays(-8)
            };

            context.Contracts.Add(contract);
            await context.SaveChangesAsync(); // save now so ContractId exists for Milestones/Reviews below

            // ---- Milestones (linked to Contract) ----
            var demolition = new Milestone
            {
                contractId = contract.contractId,
                title = "Demolition & Prep",
                amount = 800m,
                orderIndex = 1,
                status = "Approved",
                endDate = now.AddDays(-1),
                DueDate = now.AddDays(-2)
            };

            var installation = new Milestone
            {
                contractId = contract.contractId,
                title = "Cabinet Installation",
                amount = 1600m,
                orderIndex = 2,
                status = "InProgress",
                endDate = now.AddDays(10),
                DueDate = now.AddDays(9)
            };

            var finishing = new Milestone
            {
                contractId = contract.contractId,
                title = "Final Finishing & Countertops",
                amount = 800m,
                orderIndex = 3,
                status = "Pending",
                endDate = now.AddDays(20),
                DueDate = now.AddDays(19)
            };

            context.Milestones.AddRange(demolition, installation, finishing);
            await context.SaveChangesAsync(); // save now so MilestoneIds exist for EscrowTransactions below (milestoneId is unique per transaction)

            // ---- EscrowTransactions (linked to Contract, optionally to a Milestone) ----
            var escrowTransactions = new List<EscrowTransaction>
            {
                new()
                {
                    contractId = contract.contractId,
                    milestoneId = demolition.milestoneId,
                    amount = demolition.amount,
                    status = "Released",
                    heldAt = now.AddDays(-8),
                    releasedAt = now.AddDays(-1)
                },
                new()
                {
                    contractId = contract.contractId,
                    milestoneId = installation.milestoneId,
                    amount = installation.amount,
                    status = "Held",
                    heldAt = now.AddDays(-8),
                    releasedAt = null
                },
                new()
                {
                    contractId = contract.contractId,
                    milestoneId = finishing.milestoneId,
                    amount = finishing.amount,
                    status = "Held",
                    heldAt = now.AddDays(-8),
                    releasedAt = null
                }
            };

            context.EscrowTransactions.AddRange(escrowTransactions);

            // ---- Notifications (linked to User) ----
            var notifications = new List<Notification>
            {
                new()
                {
                    userId = vendorUser.UserId,
                    title = "New Quote Request",
                    type = "QuoteRequest",
                    isRead = true,
                    createdAt = now.AddDays(-10)
                },
                new()
                {
                    userId = clientUser.UserId,
                    title = "Quote Received",
                    type = "Quote",
                    isRead = true,
                    createdAt = now.AddDays(-9)
                },
                new()
                {
                    userId = clientUser.UserId,
                    title = "Milestone Approved",
                    type = "Milestone",
                    isRead = false,
                    createdAt = now.AddDays(-1)
                },
                new()
                {
                    userId = vendorUser.UserId,
                    title = "Escrow Released",
                    type = "Escrow",
                    isRead = false,
                    createdAt = now.AddDays(-1)
                }
            };

            context.Notifications.AddRange(notifications);
            await context.SaveChangesAsync();

            // ---- Reviews (linked to reviewer + vendorProfile/product/contract) ----
            var reviews = new List<Review>
            {
                new()
                {
                    ReviewerId = clientUser.UserId,
                    VendorProfileId = vendorProfile.VendorProfileID,
                    Rating = 5,
                    Comment = "Finished the kitchen renovation on time and on budget.",
                    ReviewDate = now.AddDays(-14)
                },
                new()
                {
                    ReviewerId = clientUser.UserId,
                    VendorProfileId = vendorProfile.VendorProfileID,
                    Rating = 4,
                    Comment = "Good communication throughout, minor delay on cabinet delivery.",
                    ReviewDate = now.AddDays(-3)
                },
                new()
                {
                    ReviewerId = clientUser.UserId,
                    ProductId = cabinetProduct.productId,
                    Rating = 5,
                    Comment = "Great quality cabinets, exactly as pictured.",
                    ReviewDate = now.AddDays(-5)
                },
                new()
                {
                    ReviewerId = clientUser.UserId,
                    ContractId = contract.contractId,
                    Rating = 5,
                    Comment = "Smooth contract process from quote to signing.",
                    ReviewDate = now.AddDays(-7)
                }
            };

            context.Reviews.AddRange(reviews);
            await context.SaveChangesAsync();
        }
    }
}
