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
    /// Rows whose Name/Description starts with "QA " or "[QA-...]" are throwaway
    /// fixtures for the Postman collection: nothing else in the seed references
    /// them, so the update/delete requests can hit them without breaking the rows
    /// the read requests assert on.
    /// </summary>
    public static class DbSeeder
    {
        // Tables in FK-safe delete order (children before parents). Categories is
        // handled separately below since it self-references via ParentCategoryId.
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

            await context.Database.ExecuteSqlRawAsync("UPDATE [Categories] SET [ParentCategoryId] = NULL");
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
            var kitchens = new Category { NameEn = "Kitchens", NameAr = "مطابخ", Type = "Service" };
            var cabinets = new Category { NameEn = "Cabinets", NameAr = "خزائن", Type = "Service", ParentCategory = kitchens };
            var ceramics = new Category { NameEn = "Ceramics", NameAr = "سيراميك", Type = "Material" };
            var wallTiles = new Category { NameEn = "Wall Tiles", NameAr = "بلاط جداري", Type = "Material", ParentCategory = ceramics };
            var plumbing = new Category { NameEn = "Plumbing", NameAr = "سباكة", Type = "Service" };
            var electrical = new Category { NameEn = "Electrical", NameAr = "كهرباء", Type = "Service" };
            var flooring = new Category { NameEn = "Flooring", NameAr = "أرضيات", Type = "Material" };

            // Throwaway fixtures — nothing below references these two.
            var qaSpareCategory = new Category { NameEn = "QA Spare Category", NameAr = "فئة احتياطية", Type = "Service" };
            var qaScrapCategory = new Category { NameEn = "QA Scrap Category", NameAr = "فئة للحذف", Type = "Material" };

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
            // This is what QuoteRequest.ProjectId needs to reference — seed at least
            // one so QuoteRequest creation has a real, valid ProjectId to point at.
            var kitchenProject = new Project
            {
                ClientId = clientUser.UserId,
                Title = "Kitchen Renovation - Al Khuwair",
                Description = "Full kitchen renovation including cabinets, countertops, and ceramic flooring.",
                City = "Muscat",
                Budget = 3500m,
                Status = Enums.ProjectStatus.Active,
                CreatedAt = now
            };

            var bathroomProject = new Project
            {
                ClientId = clientUser.UserId,
                Title = "Bathroom Retiling - Qurum",
                Description = "Replace existing tiles with new ceramic flooring and wall tiles.",
                City = "Muscat",
                Budget = 1200m,
                Status = Enums.ProjectStatus.Active,
                CreatedAt = now
            };

            var flooringProject = new Project
            {
                ClientId = clientUser2.UserId,
                Title = "Majlis Flooring - Al Ansab",
                Description = "Supply and install new flooring for a 60 m2 majlis.",
                City = "Muscat",
                Budget = 2000m,
                Status = Enums.ProjectStatus.Active,
                CreatedAt = now.AddDays(-20)
            };

            var electricalProject = new Project
            {
                ClientId = clientUser2.UserId,
                Title = "Villa Electrical Rewiring - Bausher",
                Description = "Rewire the ground floor and replace all ceiling lighting.",
                City = "Muscat",
                Budget = 4200m,
                Status = Enums.ProjectStatus.Draft,
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
                Status = Enums.ProjectStatus.Draft,
                CreatedAt = now
            };

            context.Projects.AddRange(
                kitchenProject, bathroomProject, flooringProject, electricalProject, qaSandboxProject);
            await context.SaveChangesAsync(); // save now so generated ProjectIds exist for QuoteRequest.ProjectId

            // ---- VendorProfiles (one per vendor user — UserId is unique) ----
            var vendorProfile = new VendorProfile
            {
                UserId = vendorUser.UserId,
                CompanyName = "Al-Habsi Kitchens & Interiors",
                VendorType = Enums.VendorType.Contractor,
                City = "Muscat",
                IsVerified = true,
                AverageRating = 4.5m,
                Balance = 0m
            };

            var vendorProfile2 = new VendorProfile
            {
                UserId = vendorUser2.UserId,
                CompanyName = "Al-Riyami Ceramics & Tiles",
                VendorType = Enums.VendorType.Store,
                City = "Muscat",
                IsVerified = true,
                AverageRating = 4.0m,
                Balance = 0m
            };

            var vendorProfile3 = new VendorProfile
            {
                UserId = vendorUser3.UserId,
                CompanyName = "Al-Kindi Electrical Works",
                VendorType = Enums.VendorType.Contractor,
                City = "Sohar",
                IsVerified = false,
                AverageRating = 3.5m,
                Balance = 150m
            };

            context.VendorProfiles.AddRange(vendorProfile, vendorProfile2, vendorProfile3);
            await context.SaveChangesAsync(); // save now so VendorProfileIds exist for Products, Quotes, Invites, Reviews below

            // ---- Products (linked to vendorProfile + category) ----
            var cabinetProduct = new Product
            {
                VendorProfileId = vendorProfile.VendorProfileId,
                CategoryId = cabinets.CategoryId,
                Name = "Custom Kitchen Cabinet Set",
                Unit = "Set",
                CurrentPrice = 1200m,
                IsAvailable = true
            };

            var tileProduct = new Product
            {
                VendorProfileId = vendorProfile2.VendorProfileId,
                CategoryId = ceramics.CategoryId,
                Name = "Premium Ceramic Floor Tile",
                Unit = "m2",
                CurrentPrice = 8.5m,
                IsAvailable = true
            };

            var countertopProduct = new Product
            {
                VendorProfileId = vendorProfile.VendorProfileId,
                CategoryId = cabinets.CategoryId,
                Name = "Quartz Countertop Slab",
                Unit = "m2",
                CurrentPrice = 45m,
                IsAvailable = true
            };

            var wallTileProduct = new Product
            {
                VendorProfileId = vendorProfile2.VendorProfileId,
                CategoryId = wallTiles.CategoryId,
                Name = "Matte Wall Tile 30x60",
                Unit = "m2",
                CurrentPrice = 6.75m,
                IsAvailable = true
            };

            var spotlightProduct = new Product
            {
                VendorProfileId = vendorProfile3.VendorProfileId,
                CategoryId = electrical.CategoryId,
                Name = "LED Ceiling Spotlight 12W",
                Unit = "Piece",
                CurrentPrice = 12m,
                IsAvailable = true
            };

            var floorPanelProduct = new Product
            {
                VendorProfileId = vendorProfile2.VendorProfileId,
                CategoryId = flooring.CategoryId,
                Name = "PVC Floor Panel",
                Unit = "m2",
                CurrentPrice = 4.25m,
                IsAvailable = false
            };

            context.Products.AddRange(
                cabinetProduct, tileProduct, countertopProduct,
                wallTileProduct, spotlightProduct, floorPanelProduct);
            await context.SaveChangesAsync(); // save now so ProductIds exist for Reviews below

            // ---- QuoteRequests (linked to Project + Category) ----
            var kitchenQuoteRequest = new QuoteRequest
            {
                ProjectId = kitchenProject.ProjectId,
                CategoryId = cabinets.CategoryId,
                Description = "Need custom cabinets designed and installed for a full kitchen renovation.",
                Deadline = now.AddDays(30),
                VisibilityType = "Direct",
                Status = "Closed" // closed because it already turned into an accepted quote + contract below
            };

            var bathroomQuoteRequest = new QuoteRequest
            {
                ProjectId = bathroomProject.ProjectId,
                CategoryId = ceramics.CategoryId,
                Description = "Need ceramic floor and wall tiles supplied and installed for a bathroom retile.",
                Deadline = now.AddDays(20),
                VisibilityType = "Public",
                Status = "Open"
            };

            var flooringQuoteRequest = new QuoteRequest
            {
                ProjectId = flooringProject.ProjectId,
                CategoryId = flooring.CategoryId,
                Description = "Supply and install 60 m2 of flooring in a majlis, material choice open.",
                Deadline = now.AddDays(25),
                VisibilityType = "Public",
                Status = "Closed" // an accepted quote + contract hang off this one below
            };

            var electricalQuoteRequest = new QuoteRequest
            {
                ProjectId = electricalProject.ProjectId,
                CategoryId = electrical.CategoryId,
                Description = "Rewire ground floor and install 24 ceiling spotlights.",
                Deadline = now.AddDays(45),
                VisibilityType = "Direct",
                Status = "Open"
            };

            // Throwaway fixtures. The Status one is what "Update Quote Request Status"
            // flips; the delete one has no quotes and no invites, so deleting it can't
            // cascade into anything another request reads.
            var qaStatusQuoteRequest = new QuoteRequest
            {
                ProjectId = qaSandboxProject.ProjectId,
                CategoryId = plumbing.CategoryId,
                Description = "[QA-STATUS-TARGET] Throwaway quote request for the status-update request.",
                Deadline = now.AddDays(10),
                VisibilityType = "Public",
                Status = "Open"
            };

            var qaDeleteQuoteRequest = new QuoteRequest
            {
                ProjectId = qaSandboxProject.ProjectId,
                CategoryId = plumbing.CategoryId,
                Description = "[QA-DELETE-TARGET] Throwaway quote request for the delete request.",
                Deadline = now.AddDays(10),
                VisibilityType = "Public",
                Status = "Open"
            };

            context.QuoteRequests.AddRange(
                kitchenQuoteRequest, bathroomQuoteRequest, flooringQuoteRequest,
                electricalQuoteRequest, qaStatusQuoteRequest, qaDeleteQuoteRequest);
            await context.SaveChangesAsync(); // save now so QuoteRequestIds exist for Invites/Quotes below

            // ---- QuoteRequestInvites (linked to QuoteRequest + VendorProfile) ----
            // The last two hang off qaStatusQuoteRequest: one for the Status update,
            // one for the delete, so neither request touches an invite the reads use.
            var invites = new List<QuoteRequestInvite>
            {
                new()
                {
                    QuoteRequestId = kitchenQuoteRequest.QuoteRequestId,
                    VendorProfileId = vendorProfile.VendorProfileId,
                    InviteStatus = "Accepted"
                },
                new()
                {
                    QuoteRequestId = bathroomQuoteRequest.QuoteRequestId,
                    VendorProfileId = vendorProfile2.VendorProfileId,
                    InviteStatus = "Sent"
                },
                new()
                {
                    QuoteRequestId = flooringQuoteRequest.QuoteRequestId,
                    VendorProfileId = vendorProfile2.VendorProfileId,
                    InviteStatus = "Accepted"
                },
                new()
                {
                    QuoteRequestId = electricalQuoteRequest.QuoteRequestId,
                    VendorProfileId = vendorProfile3.VendorProfileId,
                    InviteStatus = "Sent"
                },
                new()
                {
                    QuoteRequestId = qaStatusQuoteRequest.QuoteRequestId,
                    VendorProfileId = vendorProfile.VendorProfileId,
                    InviteStatus = "Sent"
                },
                new()
                {
                    QuoteRequestId = qaStatusQuoteRequest.QuoteRequestId,
                    VendorProfileId = vendorProfile2.VendorProfileId,
                    InviteStatus = "Sent"
                }
            };

            context.QuoteRequestInvites.AddRange(invites);

            // ---- Quotes (linked to QuoteRequest + VendorProfile) ----
            var kitchenQuote = new Quote
            {
                QuoteRequestId = kitchenQuoteRequest.QuoteRequestId,
                VendorProfileId = vendorProfile.VendorProfileId,
                Price = 3200m,
                DurationDays = 21,
                Status = "Accepted",
                SubmittedAt = now.AddDays(-10)
            };

            var bathroomQuote = new Quote
            {
                QuoteRequestId = bathroomQuoteRequest.QuoteRequestId,
                VendorProfileId = vendorProfile2.VendorProfileId,
                Price = 950m,
                DurationDays = 7,
                Status = "Pending",
                SubmittedAt = now.AddDays(-2)
            };

            var flooringQuote = new Quote
            {
                QuoteRequestId = flooringQuoteRequest.QuoteRequestId,
                VendorProfileId = vendorProfile2.VendorProfileId,
                Price = 1400m,
                DurationDays = 10,
                Status = "Accepted",
                SubmittedAt = now.AddDays(-18)
            };

            var electricalQuote = new Quote
            {
                QuoteRequestId = electricalQuoteRequest.QuoteRequestId,
                VendorProfileId = vendorProfile3.VendorProfileId,
                Price = 2100m,
                DurationDays = 14,
                Status = "Pending",
                SubmittedAt = now.AddDays(-5)
            };

            // Throwaway fixture: deliberately left with no contract, because accepting
            // a quote creates one and Contract.QuoteId is unique — accepting a quote
            // that already has a contract blows up on the duplicate key.
            var qaAcceptQuote = new Quote
            {
                QuoteRequestId = flooringQuoteRequest.QuoteRequestId,
                VendorProfileId = vendorProfile.VendorProfileId,
                Price = 1550m,
                DurationDays = 12,
                Status = "Pending",
                SubmittedAt = now.AddDays(-17)
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
                    QuoteId = kitchenQuote.QuoteId,
                    ProposedPrice = 3000m,
                    ProposedDurationDays = "18",
                    Message = "Can you do 3000 OMR and finish a few days earlier?",
                    CreatedAt = now.AddDays(-9)
                },
                new()
                {
                    UserId = clientUser.UserId,
                    QuoteId = bathroomQuote.QuoteId,
                    ProposedPrice = 9m,
                    ProposedDurationDays = "5",
                    Message = "Could you shave two days off the schedule?",
                    CreatedAt = now.AddDays(-1)
                },
                new()
                {
                    UserId = clientUser2.UserId,
                    QuoteId = flooringQuote.QuoteId,
                    ProposedPrice = 8m,
                    ProposedDurationDays = "9",
                    Message = "Happy with the price, can the crew start next week?",
                    CreatedAt = now.AddDays(-17)
                }
            };

            context.QuoteNegotiations.AddRange(negotiations);

            // ---- Contracts (one-to-one with an accepted Quote — QuoteId is unique) ----
            var contract = new Contract
            {
                QuoteId = kitchenQuote.QuoteId,
                TotalAmount = kitchenQuote.Price,
                PaymentType = "PreMilestone",
                Status = "Active",
                SignedAt = now.AddDays(-8)
            };

            var flooringContract = new Contract
            {
                QuoteId = flooringQuote.QuoteId,
                TotalAmount = flooringQuote.Price,
                PaymentType = "PreMilestone",
                Status = "Completed",
                SignedAt = now.AddDays(-16)
            };

            context.Contracts.AddRange(contract, flooringContract);
            await context.SaveChangesAsync(); // save now so ContractIds exist for Milestones/Reviews below

            // ---- Milestones (linked to Contract) ----
            var demolition = new Milestone
            {
                ContractId = contract.ContractId,
                Title = "Demolition & Prep",
                Amount = 800m,
                OrderIndex = 1,
                Status = "Approved",
                EndDate = now.AddDays(-1),
                DueDate = now.AddDays(-2)
            };

            var installation = new Milestone
            {
                ContractId = contract.ContractId,
                Title = "Cabinet Installation",
                Amount = 1600m,
                OrderIndex = 2,
                Status = "InProgress",
                EndDate = now.AddDays(10),
                DueDate = now.AddDays(9)
            };

            var finishing = new Milestone
            {
                ContractId = contract.ContractId,
                Title = "Final Finishing & Countertops",
                Amount = 800m,
                OrderIndex = 3,
                Status = "Pending",
                EndDate = now.AddDays(20),
                DueDate = now.AddDays(19)
            };

            var flooringSupply = new Milestone
            {
                ContractId = flooringContract.ContractId,
                Title = "Material Supply",
                Amount = 600m,
                OrderIndex = 1,
                Status = "Approved",
                EndDate = now.AddDays(-12),
                DueDate = now.AddDays(-13)
            };

            var flooringInstall = new Milestone
            {
                ContractId = flooringContract.ContractId,
                Title = "Flooring Installation",
                Amount = 800m,
                OrderIndex = 2,
                Status = "Approved",
                EndDate = now.AddDays(-6),
                DueDate = now.AddDays(-7)
            };

            context.Milestones.AddRange(
                demolition, installation, finishing, flooringSupply, flooringInstall);
            await context.SaveChangesAsync(); // save now so MilestoneIds exist for EscrowTransactions below (MilestoneId is unique per transaction)

            // ---- EscrowTransactions (linked to Contract, optionally to a Milestone) ----
            var escrowTransactions = new List<EscrowTransaction>
            {
                new()
                {
                    ContractId = contract.ContractId,
                    MilestoneId = demolition.MilestoneId,
                    Amount = demolition.Amount,
                    Status = "Released",
                    HeldAt = now.AddDays(-8),
                    ReleasedAt = now.AddDays(-1)
                },
                new()
                {
                    ContractId = contract.ContractId,
                    MilestoneId = installation.MilestoneId,
                    Amount = installation.Amount,
                    Status = "Held",
                    HeldAt = now.AddDays(-8),
                    ReleasedAt = null
                },
                new()
                {
                    ContractId = contract.ContractId,
                    MilestoneId = finishing.MilestoneId,
                    Amount = finishing.Amount,
                    Status = "Held",
                    HeldAt = now.AddDays(-8),
                    ReleasedAt = null
                },
                new()
                {
                    ContractId = flooringContract.ContractId,
                    MilestoneId = flooringSupply.MilestoneId,
                    Amount = flooringSupply.Amount,
                    Status = "Released",
                    HeldAt = now.AddDays(-16),
                    ReleasedAt = now.AddDays(-12)
                },
                new()
                {
                    ContractId = flooringContract.ContractId,
                    MilestoneId = flooringInstall.MilestoneId,
                    Amount = flooringInstall.Amount,
                    Status = "Released",
                    HeldAt = now.AddDays(-16),
                    ReleasedAt = now.AddDays(-6)
                }
            };

            context.EscrowTransactions.AddRange(escrowTransactions);

            // ---- Notifications (linked to User) ----
            var notifications = new List<Notification>
            {
                new()
                {
                    UserId = vendorUser.UserId,
                    Title = "New Quote Request",
                    Type = "QuoteRequest",
                    IsRead = true,
                    CreatedAt = now.AddDays(-10)
                },
                new()
                {
                    UserId = clientUser.UserId,
                    Title = "Quote Received",
                    Type = "Quote",
                    IsRead = true,
                    CreatedAt = now.AddDays(-9)
                },
                new()
                {
                    UserId = clientUser.UserId,
                    Title = "Milestone Approved",
                    Type = "Milestone",
                    IsRead = false,
                    CreatedAt = now.AddDays(-1)
                },
                new()
                {
                    UserId = vendorUser.UserId,
                    Title = "Escrow Released",
                    Type = "Escrow",
                    IsRead = false,
                    CreatedAt = now.AddDays(-1)
                },
                new()
                {
                    UserId = vendorUser2.UserId,
                    Title = "Your quote was accepted",
                    Type = "QuoteAccepted",
                    IsRead = true,
                    CreatedAt = now.AddDays(-17)
                },
                new()
                {
                    UserId = vendorUser2.UserId,
                    Title = "New Quote Request",
                    Type = "QuoteRequest",
                    IsRead = false,
                    CreatedAt = now.AddDays(-2)
                },
                new()
                {
                    UserId = clientUser2.UserId,
                    Title = "Contract Completed",
                    Type = "Contract",
                    IsRead = true,
                    CreatedAt = now.AddDays(-6)
                },
                new()
                {
                    UserId = clientUser2.UserId,
                    Title = "Quote Received",
                    Type = "Quote",
                    IsRead = false,
                    CreatedAt = now.AddDays(-5)
                },
                new()
                {
                    UserId = vendorUser3.UserId,
                    Title = "New Quote Request",
                    Type = "QuoteRequest",
                    IsRead = false,
                    CreatedAt = now.AddDays(-5)
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
                    VendorProfileId = vendorProfile.VendorProfileId,
                    Rating = 5,
                    Comment = "Finished the kitchen renovation on time and on budget.",
                    ReviewDate = now.AddDays(-14)
                },
                new()
                {
                    ReviewerId = clientUser.UserId,
                    VendorProfileId = vendorProfile.VendorProfileId,
                    Rating = 4,
                    Comment = "Good communication throughout, minor delay on cabinet delivery.",
                    ReviewDate = now.AddDays(-3)
                },
                new()
                {
                    ReviewerId = clientUser.UserId,
                    ProductId = cabinetProduct.ProductId,
                    Rating = 5,
                    Comment = "Great quality cabinets, exactly as pictured.",
                    ReviewDate = now.AddDays(-5)
                },
                new()
                {
                    ReviewerId = clientUser.UserId,
                    ContractId = contract.ContractId,
                    Rating = 5,
                    Comment = "Smooth contract process from quote to signing.",
                    ReviewDate = now.AddDays(-7)
                },
                new()
                {
                    ReviewerId = clientUser2.UserId,
                    VendorProfileId = vendorProfile2.VendorProfileId,
                    Rating = 4,
                    Comment = "Flooring crew was tidy and finished ahead of schedule.",
                    ReviewDate = now.AddDays(-5)
                },
                new()
                {
                    ReviewerId = clientUser2.UserId,
                    ContractId = flooringContract.ContractId,
                    Rating = 4,
                    Comment = "Milestones were released without any back and forth.",
                    ReviewDate = now.AddDays(-4)
                },
                new()
                {
                    ReviewerId = clientUser2.UserId,
                    ProductId = floorPanelProduct.ProductId,
                    Rating = 3,
                    Comment = "Panels are fine for the price, but the colour is lighter than shown.",
                    ReviewDate = now.AddDays(-4)
                },
                new()
                {
                    ReviewerId = clientUser.UserId,
                    ProductId = wallTileProduct.ProductId,
                    Rating = 4,
                    Comment = "Matte finish looks great, one box arrived chipped.",
                    ReviewDate = now.AddDays(-2)
                },
                new()
                {
                    ReviewerId = clientUser2.UserId,
                    VendorProfileId = vendorProfile3.VendorProfileId,
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
