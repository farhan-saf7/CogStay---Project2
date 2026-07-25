# 🎓 Viva & Technical Interview Preparation Guide

This guide is designed to prepare you for project presentations, viva voce, college faculty assessments, and developer technical interviews regarding the **CogStay** application.

---

## ⏱ The Elevator Pitches

### 🎙 1. The 2-Minute Operational Pitch (For Evaluators/Managers)
> "CogStay is an Integrated Hotel Management System built on ASP.NET Core MVC. It digitizes hotel operations from room booking to check-in, checkout, automated billing, and housekeeping turnovers. The core value is automation: when a guest checks out and pays, the system automatically marks the room as dirty and creates a cleaning task for housekeeping. Once housekeeping finishes, the room transitions back to available in real-time on the guest booking page. It manages role-based access for Guests, Front Desk, Housekeeping, Managers, and Admins to streamline coordination."

### 🎙 2. The 5-Minute Technical Pitch (For Software Engineers)
> "CogStay is a .NET 10.0 MVC application structured using the Repository and Service Pattern to achieve separation of concerns. It features generic repositories for generic CRUD operations, extended by specialized entity interfaces to handle detailed queries. The Service Layer handles all business rules, ensuring that controller actions remain lightweight. Security is enforced through SHA-256 password hashing. Entity relationships are configured via EF Core's Fluent API with strict deletion rules, unique indexes, and decimal precision definitions. We use Session state variables to securely track role logins without heavy state management overhead."

### 🎙 3. The 10-Minute Deep-Dive Pitch (For Faculty/Architects)
> "CogStay is an enterprise-grade N-Tier MVC architecture backed by SQL Server. Request flows begin at the routing layer, mapping requests to controllers where model validation attributes check input parameters. Controllers delegate to Scoped Services containing domain validation (like checking for overlapping booking dates). Services interact with Repositories using EF Core. We utilize specialized repository inclusions to prevent database lazy-loading issues. The database schema in `HotelDbContext` overrides the standard model configurations by enforcing custom decimal precision (18,2) and explicit unique indexes. Deletion relationships are set to 'Restrict' on primary entities to prevent orphan cascades, but set to 'Cascade' on stay-to-billing mappings. This clean architecture allows us to swap database layers or scale services independently."

---

## 🔄 The HTTP Request Lifecycle (Step-by-Step)

Here is exactly how a request flows through CogStay when a Guest clicks "Confirm Booking":

```
[Browser Client]
       │  1. HTTP POST Request (Form Data)
       ▼
[Routing Middleware]
       │  2. Map route to ReservationController.Create action
       ▼
[Model Binder]
       │  3. Convert Form inputs into CreateReservationDTO & validate annotations
       ▼
[ReservationController]
       │  4. Invoke IReservationService.BookRoomAsync(dto)
       ▼
[ReservationService]
       │  5. Perform validation (Check-In in past?, Date overlap collision checks)
       │  6. Invoke IReservationRepository & IRoomRepository
       ▼
[Repositories & EF Core]
       │  7. Translate SQL queries & insert new Reservation records
       ▼
[SQL Server Database]
       │  8. Write record, enforce primary/foreign keys & unique constraints
       ▼
[View Engine / Action Result]
       │  9. Redirect user to Guest Dashboard with success confirmation
       ▼
[Browser Client] (HTML Rendered Response)
```

---

## 💬 Anticipated Q&A (Frequently Asked Questions)

### Q1: Why did you use the Repository and Service Pattern instead of calling DbContext directly inside Controllers?
*   **Answer:** "Calling DbContext directly inside controllers tightly couples the presentation layer to EF Core, making testing and maintenance difficult. By using the Repository Pattern, we abstract all data access. The Service Layer isolates business rules, meaning if we swap out EF Core for Dapper or another ORM, or edit a business rule, we do not need to touch our controllers."

### Q2: What is the difference between `Transient`, `Scoped`, and `Singleton` service lifetimes, and what did you use?
*   **Answer:**
    *   `Transient`: A new instance is created every time it is requested.
    *   `Scoped`: A single instance is created once per HTTP request lifecycle. (We used **Scoped** for all Services and Repositories so that database state is shared consistently during a single request).
    *   `Singleton`: One single instance is created once and shared globally.

### Q3: How do you handle password security?
*   **Answer:** "We use SHA-256 cryptographic hashing. During registration, the plain password is combined, hashed, and converted to a Base64 string. During login, we hash the incoming password and compare the hashes. We never store plain text passwords, protecting credentials from database leak vulnerabilities."

### Q4: How does the room status transition from "Dirty" back to "Available"?
*   **Answer:** "This is an automated workflow. When a guest pays, the `BillingService` sets the room status to `CleaningRequired` and creates a `HousekeepingTask` with a `Pending` status. Housekeeping staff log in, change the task to `InProgress` (which sets the room to `CleaningInProgress`), and finally to `Completed` (which resets the room status back to `Available`)."

### Q5: What is Model Binding in ASP.NET Core?
*   **Answer:** "Model binding extracts values from HTTP requests (query strings, route parameters, form inputs) and maps them directly to C# action parameters or DTO objects, while performing automatic data conversions."

---
*Keep this guide handy during viva voice and code walk-through sessions.*
