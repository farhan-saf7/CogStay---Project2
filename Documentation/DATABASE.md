# 🗄 Database Design & Schema Mappings

This document details the database architecture of the **CogStay** Integrated Hotel Management System. It covers entity relationships, column types, constraints, unique keys, and cascading delete behaviors configured in `HotelDbContext.cs`.

---

## 📊 Entity Relationship Diagram (ERD)

The following Mermaid diagram visualizes how entities relate, including primary/foreign key mappings and cardinalities:

```mermaid
erDiagram
    GUEST ||--o{ RESERVATION : "makes"
    GUEST ||--o{ STAY_RECORD : "undergoes"
    ROOM ||--o{ RESERVATION : "assigned to"
    ROOM ||--o{ HOUSEKEEPING_TASK : "has"
    RESERVATION ||--|| STAY_RECORD : "links to"
    STAY_RECORD ||--|| BILLING : "generates"
    GUEST ||--o{ FEEDBACK : "writes"
    RESERVATION ||--o{ FEEDBACK : "reviews"

    GUEST {
        int GuestId PK
        string FullName
        string Email UK
        string PhoneNumber
        string Address
        string PasswordHash
        DateTime CreatedAt
    }

    ROOM {
        int RoomId PK
        string RoomNumber UK
        string RoomType
        decimal PricePerNight
        RoomStatus Status
    }

    RESERVATION {
        int ReservationId PK
        int GuestId FK
        string GuestName
        int RoomId FK
        DateTime CheckInDate
        DateTime CheckOutDate
        ReservationStatus ReservationStatus
    }

    STAY_RECORD {
        int StayId PK
        int GuestId FK
        string GuestName
        int ReservationId FK-UK
        string BookingReference
        string BillingReference
        string StayDetails
        DateTime ActualCheckIn
        DateTime ActualCheckOut
    }

    BILLING {
        int BillId PK
        int StayId FK-UK
        int GuestId FK
        string GuestName
        decimal TotalAmount
        PaymentStatus PaymentStatus
        string Remarks
    }

    HOUSEKEEPING_TASK {
        int TaskId PK
        int RoomId FK
        string TaskDescription
        TaskStatus TaskStatus
    }

    FEEDBACK {
        int FeedbackId PK
        int GuestId FK
        int ReservationId FK
        int Rating
        string Comments
        DateTime CreatedAt
    }

    STAFF {
        int StaffId PK
        string FullName
        string Email UK
        string PhoneNumber
        string PasswordHash
        StaffRole Role
        bool IsActive
        DateTime CreatedAt
    }
```

---

## 📁 Table Schema Details

### 1. `Guests` (Table: `Guests`)
Stores registered customer accounts:
*   `GuestId` (Int, Identity PK)
*   `FullName` (NVarChar(100), Required)
*   `Email` (NVarChar(256), Required, **Unique Index**)
*   `PhoneNumber` (NVarChar(20), Required)
*   `Address` (NVarChar(500), Required)
*   `PasswordHash` (NVarChar(MAX), Required)
*   `CreatedAt` (DateTime2, Required)

### 2. `Rooms` (Table: `Rooms`)
Stores room inventory specifications:
*   `RoomId` (Int, Identity PK)
*   `RoomNumber` (NVarChar(50), Required, **Unique Index**)
*   `RoomType` (NVarChar(100), Required)
*   `PricePerNight` (Decimal(18,2), Required, **Explicit Precision 18,2**)
*   `Status` (Int/Enum, Required) - Maps to `RoomStatus`

### 3. `Reservations` (Table: `Reservations`)
Stores booking records:
*   `ReservationId` (Int, Identity PK)
*   `GuestId` (Int, FK to `Guests`)
*   `GuestName` (NVarChar(MAX), Nullable)
*   `RoomId` (Int, FK to `Rooms`)
*   `CheckInDate` (DateTime2, Required)
*   `CheckOutDate` (DateTime2, Required)
*   `ReservationStatus` (Int/Enum, Required) - Maps to `ReservationStatus`

### 4. `StayRecords` (Table: `StayRecords`)
Stores active stay intervals (check-ins):
*   `StayId` (Int, Identity PK)
*   `GuestId` (Int, FK to `Guests`)
*   `GuestName` (NVarChar(MAX), Nullable)
*   `ReservationId` (Int, FK to `Reservations`, **Unique FK Constraint**)
*   `BookingReference` (NVarChar(MAX), Nullable)
*   `BillingReference` (NVarChar(MAX), Nullable)
*   `StayDetails` (NVarChar(MAX), Nullable)
*   `ActualCheckIn` (DateTime2, Nullable)
*   `ActualCheckOut` (DateTime2, Nullable)

### 5. `Billings` (Table: `Billings`)
Stores invoices generated:
*   `BillId` (Int, Identity PK)
*   `StayId` (Int, FK to `StayRecords`, **Unique FK Constraint**)
*   `GuestId` (Int, FK to `Guests`)
*   `GuestName` (NVarChar(MAX), Nullable)
*   `TotalAmount` (Decimal(18,2), Required, **Explicit Precision 18,2**)
*   `PaymentStatus` (Int/Enum, Required) - Maps to `PaymentStatus`
*   `Remarks` (NVarChar(MAX), Nullable)

### 6. `HousekeepingTasks` (Table: `HousekeepingTasks`)
Stores cleaning requests:
*   `TaskId` (Int, Identity PK)
*   `RoomId` (Int, FK to `Rooms`)
*   `TaskDescription` (NVarChar(1000), Required)
*   `TaskStatus` (Int/Enum, Required) - Maps to `TaskStatus`

### 7. `Feedbacks` (Table: `Feedbacks`)
Stores ratings/reviews submitted by guests:
*   `FeedbackId` (Int, Identity PK)
*   `GuestId` (Int, FK to `Guests`)
*   `ReservationId` (Int, Nullable FK to `Reservations`)
*   `Rating` (Int, Required, Range 1 to 5)
*   `Comments` (NVarChar(1000), Required)
*   `CreatedAt` (DateTime2, Required)

### 8. `Staff` (Table: `Staff`)
Stores employee logins:
*   `StaffId` (Int, Identity PK)
*   `FullName` (NVarChar(100), Required)
*   `Email` (NVarChar(256), Required, **Unique Index**)
*   `PhoneNumber` (NVarChar(20), Required)
*   `PasswordHash` (NVarChar(MAX), Required)
*   `Role` (Int/Enum, Required) - Maps to `StaffRole`
*   `IsActive` (Bit/Bool, Required)
*   `CreatedAt` (DateTime2, Required)

---

## 🔗 Cascading & Delete Rules

To prevent accidental deletions of transaction logs and histories, rules are defined in `HotelDbContext.OnModelCreating`:

*   **Restrict Deletions (`DeleteBehavior.Restrict`):**
    *   `Guest` deletion is restricted if linked to any `Reservation` or `StayRecord` entities.
    *   `Room` deletion is restricted if linked to any `Reservation` or `HousekeepingTask` entities.
    *   `Reservation` deletion is restricted if linked to an active `StayRecord`.
    This prevents users from deleting accounts or rooms that have operational histories linked to them.

*   **Cascade Deletions (`DeleteBehavior.Cascade`):**
    *   Deleting a `StayRecord` automatically cascades to delete the linked `Billing` invoice. This maintains tight coupling between a guest stay and its billing ledger.

---
*For runtime details, see the [BACKEND_GUIDE.md](file:///c:/Users/farha/OneDrive/Desktop/CogStay--Final/CogStay---Project2/BACKEND_GUIDE.md) document.*
