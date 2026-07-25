# 🏨 CogStay - Integrated Hotel Management System

[![Build Status](https://img.shields.io/badge/build-passing-brightgreen.svg)](#)
[![Framework](https://img.shields.io/badge/framework-ASP.NET%20Core%2010.0-blue.svg)](#)
[![Database](https://img.shields.io/badge/database-SQL%20Server-red.svg)](#)
[![Architecture](https://img.shields.io/badge/architecture-MVC%20%2F%20Repository-orange.svg)](#)

CogStay is a modern, enterprise-grade Integrated Hotel Management System (HMS) built on **ASP.NET Core MVC**. It provides a fully digitized, automated operational workflow for hotel guests and staff departments. It coordinates the lifecycle of booking, stay execution, billing invoices, payments, and housekeeping cleaning cycles.

---

## 🌟 Key Features

*   **Secure Authentication:** Hashed credentials (SHA-256) for both Guests and Staff members.
*   **Role-Based Access Control:** Separate portals and session scopes for Guests, Front Desk, Housekeeping, Managers, and Admins.
*   **Dynamic Room Allocation & Booking:** Validation logic preventing date collisions and double-bookings.
*   **Integrated Billing System:** Nightly rate calculations and automated invoice generation upon checkout or custom billing entries.
*   **Service & Cleaning Workflows:** Room clean status state machine that updates Room availability automatically when housekeeping tasks are resolved.

---

## 🏗 System Architecture & Directory Structure

CogStay implements a clear **N-Tier Architecture** coupled with the **Repository and Service Pattern** to decouple the Presentation, Business Logic, and Data Access Layers.

```
CogStay---Project2/
│
├── Controllers/                 # Presentation layer entry points (MVC Routing)
├── Services/                    # Core Business Logic Layer (Interfaces & Implementations)
├── Repositories/                # Persistence & Data Access Layer (Generic/Entity Repositories)
├── Data/                        # DBContext configurations, seeding scripts, and migrations
├── Models/                      # Relational Entity Schemas (Room, StayRecord, Billing, etc.)
├── DTOs/                        # Data Transfer Objects for decoupled parameter passing
├── Enums/                       # Strongly typed system states (RoomStatus, PaymentStatus, etc.)
├── Views/                       # Razor View UI pages (Home, Shared, Layouts)
└── wwwroot/                     # Static UI assets (CSS stylesheets, JS scripts, icons, libraries)
```

---

## 🛠 Technology Stack

*   **Backend:** ASP.NET Core 10.0 (MVC Framework)
*   **ORM / Data Layer:** Entity Framework Core 10.0
*   **Database:** Microsoft SQL Server (LocalDB / SQLExpress)
*   **Frontend UI:** Razor Views, HTML5, CSS3, Bootstrap 5, JavaScript
*   **Security:** Cryptographic SHA-256 password hashing

---

## 🚀 Getting Started

### 📋 Prerequisites

1.  [.NET 10.0 SDK](https://dotnet.microsoft.com/download/dotnet/10.0) installed on your system.
2.  [Microsoft SQL Server](https://www.microsoft.com/sql-server/) (SQLExpress or Developer edition) running locally.
3.  [SQL Server Management Studio (SSMS)](https://learn.microsoft.com/sql/ssms/download-sql-server-management-studio-ssms) or SQL command line tools.

### ⚙ Configuration & Connection String

1.  Open the `appsettings.json` file in the project root.
2.  Modify the `ConnectionStrings.DefaultConnection` value to target your local SQL Server instance:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=YOUR_SERVER_NAME;Initial Catalog=Cogstay;Integrated Security=True;TrustServerCertificate=true;"
  }
}
```

> [ll] NOTE
> Ensure that your SQL Server service is running and that your Windows account has appropriate permissions to create and manage the database.

---

## 🔄 Database Setup & Migrations

To apply database tables, constraints, relationship configurations, and initial data seeding, execute the following commands in your shell from the project root:

### 1. Restore Dependencies
```powershell
dotnet restore
```

### 2. Apply Migrations
Apply the EF Core migrations to automatically generate the database and seed initial admin, staff, and room inventory:
```powershell
dotnet ef database update
```

> [ll] TIP
> If you do not have the EF Core command-line tool installed, install it globally using:
> `dotnet tool install --global dotnet-ef`

---

## 💻 Building and Running the Application

### Build the Project
To compile the project and check for syntax or type errors:
```powershell
dotnet build CogStayMVC.csproj
```

### Run Locally
To spin up the local development web server:
```powershell
dotnet run
```

The application will launch and listen on:
*   `https://localhost:7147` (Default SSL port)
*   `http://localhost:5242` (Default HTTP port)

Open your web browser and navigate to `https://localhost:7147` to access the hotel landing page.

---

## 📖 Complete Documentation Suite

For deep-dives into workflows, database details, program structures, and interview prep, please refer to the files in the directory:

*   📄 **[USER_MANUAL.md](file:///c:/Users/farha/OneDrive/Desktop/CogStay--Final/CogStay---Project2/USER_MANUAL.md):** End-user guide for hotel roles, login procedures, check-in, checkout, and housekeeping.
*   📄 **[FLOW.md](file:///c:/Users/farha/OneDrive/Desktop/CogStay--Final/CogStay---Project2/FLOW.md):** Full-page visual flowcharts detailing customer, front desk, and housekeeping operational paths.
*   📄 **[BACKEND_GUIDE.md](file:///c:/Users/farha/OneDrive/Desktop/CogStay--Final/CogStay---Project2/BACKEND_GUIDE.md):** Deep explanation of DbContext, DI registrations, Middleware, and core file structures.
*   📄 **[PROJECT_STRUCTURE.md](file:///c:/Users/farha/OneDrive/Desktop/CogStay--Final/CogStay---Project2/PROJECT_STRUCTURE.md):** Directory-by-directory mapping of files, views, controllers, models, and scripts.
*   📄 **[DATABASE.md](file:///c:/Users/farha/OneDrive/Desktop/CogStay--Final/CogStay---Project2/DATABASE.md):** Entity-Relationship diagrams (Mermaid), tables, indexes, constraints, and cascade delete configurations.
*   📄 **[INTERVIEW_GUIDE.md](file:///c:/Users/farha/OneDrive/Desktop/CogStay--Final/CogStay---Project2/INTERVIEW_GUIDE.md):** A preparation kit for interviews, viva voice, architectural pitches (2/5/10 mins), and key questions.

---
*Developed as part of the CogStay Integrated Hotel operational software series.*
