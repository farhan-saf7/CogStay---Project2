# 📂 Project Structure Map

This document outlines the file layout and directory mapping of the **CogStay** codebase. Use this map to navigate the presentation, business logic, configuration, and static assets directories.

---

## 📁 Core Directory Layout

```
CogStay---Project2/
│
├── Controllers/            # Shared UI Controllers (Home & Staff Login)
│   ├── HomeController.cs
│   └── StaffController.cs
│
├── Services/               # Business Logic layer
│   ├── Admin/
│   │   └── AdminService.cs (Handles RoomService & StaffService)
│   ├── FrontDesk/
│   │   └── FrontDeskService.cs (ReservationService, CheckInService, BillingService)
│   ├── Guest/
│   │   └── GuestService.cs (Guest authentication and validations)
│   ├── Housekeeping/
│   │   └── HousekeepingService.cs (Clean tasks and room status triggers)
│   ├── Manager/
│   │   └── ManagerService.cs (FeedbackService moderation)
│   └── Interfaces/
│       └── IServiceInterfaces.cs (Core interfaces for all business layer classes)
│
├── Repositories/           # Data Persistence layer
│   ├── Admin/
│   │   └── AdminRepository.cs (RoomRepository & StaffRepository)
│   ├── FrontDesk/
│   │   └── FrontDeskRepository.cs (ReservationRepository, StayRecordRepository, BillingRepository)
│   ├── Guest/
│   │   └── GuestRepository.cs (GuestRepository)
│   ├── Housekeeping/
│   │   └── HousekeepingRepository.cs (HousekeepingTaskRepository)
│   ├── Manager/
│   │   └── ManagerRepository.cs (FeedbackRepository)
│   ├── Implementations/
│   │   └── Repository.cs (Generic CRUD repository base)
│   └── Interfaces/
│       ├── IRepository.cs (Generic CRUD interface contract)
│       └── EntityInterfaces.cs (Extensions for specialized entity queries)
│
├── Models/                 # EF Core Entity schemas (Database tables)
│   ├── Room.cs
│   ├── Guest.cs
│   ├── Reservation.cs
│   ├── StayRecord.cs
│   ├── Billing.cs
│   ├── Feedback.cs
│   ├── Staff.cs
│   └── HousekeepingTask.cs
│
├── DTOs/                   # Data Transfer Objects
│   ├── RoomDTOs.cs
│   ├── GuestDTOs.cs
│   ├── ReservationDTOs.cs
│   ├── StayRecordDTOs.cs
│   ├── BillingDTOs.cs
│   ├── FeedbackDTOs.cs
│   ├── StaffDTOs.cs
│   └── HousekeepingDTOs.cs
│
├── Enums/                  # System state flags (strongly typed integers)
│   ├── RoomStatus.cs
│   ├── PaymentStatus.cs
│   ├── ReservationStatus.cs
│   ├── StaffRole.cs
│   └── TaskStatus.cs
│
├── Data/                   # Database Context & Seeding scripts
│   ├── HotelDbContext.cs   # Entity relationships, Fluent API, decimal precisions
│   └── Seeding/            # (Initial SQL setup data)
│
├── Migrations/             # Entity Framework Core database migration logs
│
├── Views/                  # Razor HTML Views for Front Desk, Admin, Housekeeping, Manager, Guests
│   ├── Home/
│   │   └── Index.cshtml    # Hotel Landing Page UI
│   ├── Shared/
│   │   └── _Layout.cshtml  # Core Layout master file
│   └── (Feature Directories)
│
├── wwwroot/                # Static files
│   ├── css/                # Custom Stylesheets (index.css, site.css)
│   ├── js/                 # Javascript files
│   └── lib/                # Third-party libraries (Bootstrap, jQuery, Val)
│
├── appsettings.json        # Database Connection Strings & Logging options
├── Program.cs              # DI registrations, Middleware pipelines, & Application startup
└── CogStayMVC.csproj       # MSBuild project file configuration
```

---

## 🗃 File Responsibility Overview

*   **Models (`Models/`):** Contains pure structural properties representing database columns. There is no business logic in models.
*   **DTOs (`DTOs/`):** Contains properties tailored for specific UI view fields. Using DTOs prevents exposing database structures to the frontend client.
*   **Enums (`Enums/`):** Used to prevent hard-coded magic strings. For example, a room can only be in one of the states defined in `RoomStatus`.
*   **Repositories (`Repositories/`):** Dedicated to querying databases (e.g. running EF `Include(...)` to fetch related records).
*   **Services (`Services/`):** Contains business validations, math calculations, and logic routines.
*   **Controllers (`Controllers/`):** Intercepts input, verifies model states (`ModelState.IsValid`), and hands tasks to Services before returning HTML views.
*   **Views (`Views/`):** Direct visual elements compiled and rendered on the client browser.
*   **wwwroot (`wwwroot/`):** Raw stylesheet (CSS) tokens, JS micro-interactions, and assets loaded dynamically by the client browser.

---
*For schema configurations, check out [DATABASE.md](file:///c:/Users/farha/OneDrive/Desktop/CogStay--Final/CogStay---Project2/DATABASE.md).*
