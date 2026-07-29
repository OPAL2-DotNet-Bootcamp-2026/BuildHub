using BuildHub.Models;
using Microsoft.EntityFrameworkCore;


namespace BuildHub
{

    /// <summary>
    /// Runtime data seeder — inserts demo data via the DbContext (not migrations),
    /// so it can freely wire up real foreign keys (self-referencing categories,
    /// vendor -> user, review -> vendor/product/contract) instead of hardcoding IDs.
    /// Safe to call every startup: it checks for existing data first.
    ///
    /// Rows whose name/description starts with "QA " or "[QA-...]" are throwaway
    /// fixtures for the Postman collection: nothing else in the seed references
    /// them, so the update/delete requests can hit them without breaking the rows
    /// the read requests assert on.
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

            foreach (var table in TablesInDeleteOrder)
                await context.Database.ExecuteSqlRawAsync(ReseedSql(table));
            await context.Database.ExecuteSqlRawAsync(ReseedSql("Categories"));
#pragma warning restore EF1002
        }

        /// <summary>
        /// RESEED to 0 makes the next inserted row get identity 1 — but only once the
        /// table has an identity value to reset. On a table no row was ever inserted
        /// into (last_value IS NULL, i.e. a freshly migrated database), SQL Server
        /// hands the reseed value itself to the first row, so every table would start
        /// at 0 and every hardcoded "1" in the Postman collection would 404. Skipping
        /// the reseed in that case leaves the original seed (1) in place, so IDs start
        /// at 1 on the first run and on every run after it.
        /// </summary>
        private static string ReseedSql(string table) =>
            $@"IF (SELECT last_value FROM sys.identity_columns WHERE object_id = OBJECT_ID('[{table}]')) IS NOT NULL
                   DBCC CHECKIDENT ('{table}', RESEED, 0)";

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
            var wallTiles = new Category { nameEn = "Wall Tiles", nameAr = "بلاط جداري", type = "Material", ParentCategory = ceramics };
            var plumbing = new Category { nameEn = "Plumbing", nameAr = "سباكة", type = "Service" };
            var electrical = new Category { nameEn = "Electrical", nameAr = "كهرباء", type = "Service" };
            var flooring = new Category { nameEn = "Flooring", nameAr = "أرضيات", type = "Material" };

            // Throwaway fixtures — nothing below references these two.
            var qaSpareCategory = new Category { nameEn = "QA Spare Category", nameAr = "فئة احتياطية", type = "Service" };
            var qaScrapCategory = new Category { nameEn = "QA Scrap Category", nameAr = "فئة للحذف", type = "Material" };

            context.Categories.AddRange(
                kitchens, cabinets, ceramics, wallTiles, plumbing, electrical, flooring,
                qaSpareCategory, qaScrapCategory);
            await context.SaveChangesAsync(); // save now so generated CategoryIds exist for anything referencing them below

            // ---- Users (two clients, three vendor-owning users, two QA fixtures) ----
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

            var clientUser2 = new User
            {
                FullName = "Ahmed Al-Saidi",
                Email = "ahmed.client@example.com",
                PasswordHash = "seeded-no-login",
                PhoneNumber = "+96890000004",
                Role = "Client",
                City = "Sohar",
                IsVerified = true,
                CreatedAt = now.AddDays(-30)
            };

            var vendorUser3 = new User
            {
                FullName = "Noor Al-Kindi",
                Email = "noor.vendor@example.com",
                PasswordHash = "seeded-no-login",
                PhoneNumber = "+96890000005",
                Role = "Contractor",
                City = "Sohar",
                IsVerified = false,
                CreatedAt = now.AddDays(-25)
            };

            // Throwaway fixtures — no projects, vendor profile, notifications,
            // negotiations or reviews point at these, so they can be renamed or
            // deleted without cascading into anything the other requests read.
            var qaEmailUser = new User
            {
                FullName = "QA Email Update User",
                Email = "qa.email.target@example.com",
                PasswordHash = "seeded-no-login",
                PhoneNumber = "+96890000098",
                Role = "Client",
                City = "Muscat",
                IsVerified = false,
                CreatedAt = now
            };

            var qaDeleteUser = new User
            {
                FullName = "QA Delete Target User",
                Email = "qa.delete.target@example.com",
                PasswordHash = "seeded-no-login",
                PhoneNumber = "+96890000099",
                Role = "Client",
                City = "Muscat",
                IsVerified = false,
                CreatedAt = now
            };

            context.Users.AddRange(
                clientUser, vendorUser, vendorUser2, clientUser2, vendorUser3,
                qaEmailUser, qaDeleteUser);
            await context.SaveChangesAsync(); // save now so generated UserIds exist for Project.ClientId and VendorProfile.UserId below

            // ---- Projects (linked to the client users) ----
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

            var flooringProject = new Project
            {
                ClientId = clientUser2.UserId,
                Title = "Majlis Flooring - Al Ansab",
                Description = "Supply and install new flooring for a 60 m2 majlis.",
                City = "Muscat",
                Budget = 2000m,
                Status = enums.ProjectStatus.Active,
                CreatedAt = now.AddDays(-20)
            };

            var electricalProject = new Project
            {
                ClientId = clientUser2.UserId,
                Title = "Villa Electrical Rewiring - Bausher",
                Description = "Rewire the ground floor and replace all ceiling lighting.",
                City = "Muscat",
                Budget = 4200m,
                Status = enums.ProjectStatus.Draft,
                CreatedAt = now.AddDays(-15)
            };

            // Holder project for the throwaway quote requests below, so the QA rows
            // never sit under a project the other requests read.
            var qaSandboxProject = new Project
            {
                ClientId = clientUser.UserId,
                Title = "QA Sandbox Project",
                Description = "Holds the throwaway quote requests used by the Postman update/delete requests.",
                City = "Muscat",
                Budget = 100m,
                Status = enums.ProjectStatus.Draft,
                CreatedAt = now
            };

            context.Projects.AddRange(
                kitchenProject, bathroomProject, flooringProject, electricalProject, qaSandboxProject);
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

            var vendorProfile3 = new VendorProfile
            {
                UserId = vendorUser3.UserId,
                CompanyName = "Al-Kindi Electrical Works",
                VendorType = enums.VendorType.Contractor,
                City = "Sohar",
                IsVerfied = false,
                AverageRating = 3.5m,
                Balance = 150m
            };

            context.VendorProfiles.AddRange(vendorProfile, vendorProfile2, vendorProfile3);
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

            var countertopProduct = new Product
            {
                vendorProfileId = vendorProfile.VendorProfileID,
                categoryId = cabinets.categoryId,
                name = "Quartz Countertop Slab",
                unit = "m2",
                currentPrice = 45m,
                isAvailable = true
            };

            var wallTileProduct = new Product
            {
                vendorProfileId = vendorProfile2.VendorProfileID,
                categoryId = wallTiles.categoryId,
                name = "Matte Wall Tile 30x60",
                unit = "m2",
                currentPrice = 6.75m,
                isAvailable = true
            };

            var spotlightProduct = new Product
            {
                vendorProfileId = vendorProfile3.VendorProfileID,
                categoryId = electrical.categoryId,
                name = "LED Ceiling Spotlight 12W",
                unit = "Piece",
                currentPrice = 12m,
                isAvailable = true
            };

            var floorPanelProduct = new Product
            {
                vendorProfileId = vendorProfile2.VendorProfileID,
                categoryId = flooring.categoryId,
                name = "PVC Floor Panel",
                unit = "m2",
                currentPrice = 4.25m,
                isAvailable = false
            };

            context.Products.AddRange(
                cabinetProduct, tileProduct, countertopProduct,
                wallTileProduct, spotlightProduct, floorPanelProduct);
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

            var flooringQuoteRequest = new QuoteRequest
            {
                projectId = flooringProject.ProjectId,
                categoryId = flooring.categoryId,
                description = "Supply and install 60 m2 of flooring in a majlis, material choice open.",
                deadline = now.AddDays(25),
                visibilityType = "Public",
                status = "Closed" // an accepted quote + contract hang off this one below
            };

            var electricalQuoteRequest = new QuoteRequest
            {
                projectId = electricalProject.ProjectId,
                categoryId = electrical.categoryId,
                description = "Rewire ground floor and install 24 ceiling spotlights.",
                deadline = now.AddDays(45),
                visibilityType = "Direct",
                status = "Open"
            };

            // Throwaway fixtures. The status one is what "Update Quote Request Status"
            // flips; the delete one has no quotes and no invites, so deleting it can't
            // cascade into anything another request reads.
            var qaStatusQuoteRequest = new QuoteRequest
            {
                projectId = qaSandboxProject.ProjectId,
                categoryId = plumbing.categoryId,
                description = "[QA-STATUS-TARGET] Throwaway quote request for the status-update request.",
                deadline = now.AddDays(10),
                visibilityType = "Public",
                status = "Open"
            };

            var qaDeleteQuoteRequest = new QuoteRequest
            {
                projectId = qaSandboxProject.ProjectId,
                categoryId = plumbing.categoryId,
                description = "[QA-DELETE-TARGET] Throwaway quote request for the delete request.",
                deadline = now.AddDays(10),
                visibilityType = "Public",
                status = "Open"
            };

            context.QuoteRequests.AddRange(
                kitchenQuoteRequest, bathroomQuoteRequest, flooringQuoteRequest,
                electricalQuoteRequest, qaStatusQuoteRequest, qaDeleteQuoteRequest);
            await context.SaveChangesAsync(); // save now so QuoteRequestIds exist for Invites/Quotes below

            // ---- QuoteRequestInvites (linked to QuoteRequest + VendorProfile) ----
            // The last two hang off qaStatusQuoteRequest: one for the status update,
            // one for the delete, so neither request touches an invite the reads use.
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
                },
                new()
                {
                    quoteRequestId = flooringQuoteRequest.qutoeRequestId,
                    vendorProfileId = vendorProfile2.VendorProfileID,
                    inviteStatus = "Accepted"
                },
                new()
                {
                    quoteRequestId = electricalQuoteRequest.qutoeRequestId,
                    vendorProfileId = vendorProfile3.VendorProfileID,
                    inviteStatus = "Sent"
                },
                new()
                {
                    quoteRequestId = qaStatusQuoteRequest.qutoeRequestId,
                    vendorProfileId = vendorProfile.VendorProfileID,
                    inviteStatus = "Sent"
                },
                new()
                {
                    quoteRequestId = qaStatusQuoteRequest.qutoeRequestId,
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

            var flooringQuote = new Quote
            {
                quoteRequestId = flooringQuoteRequest.qutoeRequestId,
                vendorProfileId = vendorProfile2.VendorProfileID,
                price = 1400m,
                durationDays = 10,
                status = "Accepted",
                submittedAt = now.AddDays(-18)
            };

            var electricalQuote = new Quote
            {
                quoteRequestId = electricalQuoteRequest.qutoeRequestId,
                vendorProfileId = vendorProfile3.VendorProfileID,
                price = 2100m,
                durationDays = 14,
                status = "Pending",
                submittedAt = now.AddDays(-5)
            };

            // Throwaway fixture: deliberately left with no contract, because accepting
            // a quote creates one and Contract.quoteId is unique — accepting a quote
            // that already has a contract blows up on the duplicate key.
            var qaAcceptQuote = new Quote
            {
                quoteRequestId = flooringQuoteRequest.qutoeRequestId,
                vendorProfileId = vendorProfile.VendorProfileID,
                price = 1550m,
                durationDays = 12,
                status = "Pending",
                submittedAt = now.AddDays(-17)
            };

            context.Quotes.AddRange(
                kitchenQuote, bathroomQuote, flooringQuote, electricalQuote, qaAcceptQuote);
            await context.SaveChangesAsync(); // save now so QuoteIds exist for QuoteNegotiation/Contract below

            // ---- QuoteNegotiations (linked to User + Quote) ----
            var negotiations = new List<QuoteNegotiation>
            {
                new()
                {
                    UserId = clientUser.UserId,
                    QuoteId = kitchenQuote.quoteId,
                    proposedPrice = 3000m,
                    proposedDurationDays = "18",
                    message = "Can you do 3000 OMR and finish a few days earlier?",
                    createIn = now.AddDays(-9)
                },
                new()
                {
                    UserId = clientUser.UserId,
                    QuoteId = bathroomQuote.quoteId,
                    proposedPrice = 9m,
                    proposedDurationDays = "5",
                    message = "Could you shave two days off the schedule?",
                    createIn = now.AddDays(-1)
                },
                new()
                {
                    UserId = clientUser2.UserId,
                    QuoteId = flooringQuote.quoteId,
                    proposedPrice = 8m,
                    proposedDurationDays = "9",
                    message = "Happy with the price, can the crew start next week?",
                    createIn = now.AddDays(-17)
                }
            };

            context.QuoteNegotiations.AddRange(negotiations);

            // ---- Contracts (one-to-one with an accepted Quote — quoteId is unique) ----
            var contract = new Contract
            {
                quoteId = kitchenQuote.quoteId,
                totalAmount = kitchenQuote.price,
                paymentType = "PreMilestone",
                status = "Active",
                signedAt = now.AddDays(-8)
            };

            var flooringContract = new Contract
            {
                quoteId = flooringQuote.quoteId,
                totalAmount = flooringQuote.price,
                paymentType = "PreMilestone",
                status = "Completed",
                signedAt = now.AddDays(-16)
            };

            context.Contracts.AddRange(contract, flooringContract);
            await context.SaveChangesAsync(); // save now so ContractIds exist for Milestones/Reviews below

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

            var flooringSupply = new Milestone
            {
                contractId = flooringContract.contractId,
                title = "Material Supply",
                amount = 600m,
                orderIndex = 1,
                status = "Approved",
                endDate = now.AddDays(-12),
                DueDate = now.AddDays(-13)
            };

            var flooringInstall = new Milestone
            {
                contractId = flooringContract.contractId,
                title = "Flooring Installation",
                amount = 800m,
                orderIndex = 2,
                status = "Approved",
                endDate = now.AddDays(-6),
                DueDate = now.AddDays(-7)
            };

            context.Milestones.AddRange(
                demolition, installation, finishing, flooringSupply, flooringInstall);
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
                },
                new()
                {
                    contractId = flooringContract.contractId,
                    milestoneId = flooringSupply.milestoneId,
                    amount = flooringSupply.amount,
                    status = "Released",
                    heldAt = now.AddDays(-16),
                    releasedAt = now.AddDays(-12)
                },
                new()
                {
                    contractId = flooringContract.contractId,
                    milestoneId = flooringInstall.milestoneId,
                    amount = flooringInstall.amount,
                    status = "Released",
                    heldAt = now.AddDays(-16),
                    releasedAt = now.AddDays(-6)
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
                },
                new()
                {
                    userId = vendorUser2.UserId,
                    title = "Your quote was accepted",
                    type = "QuoteAccepted",
                    isRead = true,
                    createdAt = now.AddDays(-17)
                },
                new()
                {
                    userId = vendorUser2.UserId,
                    title = "New Quote Request",
                    type = "QuoteRequest",
                    isRead = false,
                    createdAt = now.AddDays(-2)
                },
                new()
                {
                    userId = clientUser2.UserId,
                    title = "Contract Completed",
                    type = "Contract",
                    isRead = true,
                    createdAt = now.AddDays(-6)
                },
                new()
                {
                    userId = clientUser2.UserId,
                    title = "Quote Received",
                    type = "Quote",
                    isRead = false,
                    createdAt = now.AddDays(-5)
                },
                new()
                {
                    userId = vendorUser3.UserId,
                    title = "New Quote Request",
                    type = "QuoteRequest",
                    isRead = false,
                    createdAt = now.AddDays(-5)
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
                },
                new()
                {
                    ReviewerId = clientUser2.UserId,
                    VendorProfileId = vendorProfile2.VendorProfileID,
                    Rating = 4,
                    Comment = "Flooring crew was tidy and finished ahead of schedule.",
                    ReviewDate = now.AddDays(-5)
                },
                new()
                {
                    ReviewerId = clientUser2.UserId,
                    ContractId = flooringContract.contractId,
                    Rating = 4,
                    Comment = "Milestones were released without any back and forth.",
                    ReviewDate = now.AddDays(-4)
                },
                new()
                {
                    ReviewerId = clientUser2.UserId,
                    ProductId = floorPanelProduct.productId,
                    Rating = 3,
                    Comment = "Panels are fine for the price, but the colour is lighter than shown.",
                    ReviewDate = now.AddDays(-4)
                },
                new()
                {
                    ReviewerId = clientUser.UserId,
                    ProductId = wallTileProduct.productId,
                    Rating = 4,
                    Comment = "Matte finish looks great, one box arrived chipped.",
                    ReviewDate = now.AddDays(-2)
                },
                new()
                {
                    ReviewerId = clientUser2.UserId,
                    VendorProfileId = vendorProfile3.VendorProfileID,
                    Rating = 3,
                    Comment = "Quote was competitive but took a while to arrive.",
                    ReviewDate = now.AddDays(-1)
                }
            };

            context.Reviews.AddRange(reviews);
            await context.SaveChangesAsync();
        }
    }
}
