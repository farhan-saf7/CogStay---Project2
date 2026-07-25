# 🔄 System Workflows & Operational Flowcharts

This document describes the step-by-step operational workflows in the **CogStay** application. It maps out how different roles interact and how room statuses transition through the booking-stay-checkout-cleaning cycle.

---

## 🗺 1. Main Navigation & Portal Entry

When a user visits the application, they land on the Hotel Homepage and can branch into Guest or Staff workflows:

```mermaid
graph TD
    Start([Visit Hotel Landing Page]) --> Choice{Portal Selection}
    Choice -->|Guest| GuestPortal[Guest Dashboard Portal]
    Choice -->|Staff| StaffPortal[Staff Portal Login Page]
    
    GuestPortal --> GuestLoginChoice{Registered?}
    GuestLoginChoice -->|No| RegisterGuest[Guest Registration Page]
    GuestLoginChoice -->|Yes| LoginGuest[Guest Login Page]
    RegisterGuest --> LoginGuest
    LoginGuest --> GuestHome[Guest Profile & Booking Area]

    StaffPortal --> SelectRole[Select Role & Enter Credentials]
    SelectRole --> ValidateCredentials{Verify Role & Hash}
    ValidateCredentials -->|Invalid| LoginFailed[Show Error / Access Denied]
    ValidateCredentials -->|Valid| StaffDashboard[Open Role-Specific Dashboard]
    
    StaffDashboard --> AdminDash[Admin Dashboard]
    StaffDashboard --> FrontDeskDash[Front Desk Dashboard]
    StaffDashboard --> HousekeepingDash[Housekeeping Dashboard]
    StaffDashboard --> ManagerDash[Manager Dashboard]
```

---

## 🛏 2. The Room Lifecycle & Status State Machine

A central feature of CogStay is the tracking of room states to coordinate bookings, stays, and turnovers.

```mermaid
stateDiagram-v2
    [*] --> Available : Room is Clean & Vacant
    Available --> Booked : Guest places Reservation
    Booked --> Occupied : Front Desk checks in Guest
    Occupied --> CheckoutPending : Guest requests checkout / checkout triggered
    CheckoutPending --> CleaningRequired : Payment processed & Check-out complete
    CleaningRequired --> CleaningInProgress : Housekeeping starts task
    CleaningInProgress --> Available : Cleaning completes (Task marked Completed)
    Available --> UnderMaintenance : Repairs needed
    UnderMaintenance --> Available : Maintenance resolved
```

---

## 📅 3. Guest Booking Workflow

This chart shows how a guest searches for rooms and confirms a reservation:

```mermaid
graph TD
    A[Guest logs in] --> B[Navigate to Available Rooms]
    B --> C[Browse pricing & type Single/Deluxe/Suite]
    C --> D[Select dates Check-In & Check-Out]
    D --> E{Collision Check - Room Booked?}
    E -->|Yes| F[Show Error: Room unavailable for dates]
    E -->|No| G[Save Reservation as Booked]
    G --> H[Update Room Status to Booked]
    H --> I[Booking Visible under Guest Dashboard]
```

---

## 🔑 4. Check-In & Check-Out Operational Flow

This flow covers guest arrival, service execution, billing invoice, and room turnover:

```mermaid
sequenceDiagram
    autonumber
    actor Guest
    actor FrontDesk as Front Desk Agent
    actor Housekeeping as Housekeeper
    participant System as CogStay Engine

    Note over Guest, FrontDesk: Check-In Phase
    Guest->>FrontDesk: Arrive at hotel & request check-in
    FrontDesk->>System: Locate Reservation & click Check-In
    System->>System: Verify status is Booked
    System->>System: Create active StayRecord
    System->>System: Set Room status to Occupied
    System-->>FrontDesk: Confirm Check-In Successful

    Note over Guest, Housekeeping: Stay Phase
    Guest->>System: Requests service or checkout
    FrontDesk->>System: Trigger Checkout Pending
    System->>System: Room status becomes CheckoutPending

    Note over FrontDesk, Housekeeping: Checkout & Billing Phase
    FrontDesk->>System: Generate Bill invoice
    System->>System: Calculate Price * Nights
    FrontDesk->>System: Process Payment details
    System->>System: Settle Bill (Set to Paid)
    System->>System: Update actual Check-Out timestamp
    System->>System: Set Room status to CleaningRequired (Dirty)
    System->>System: AUTOMATICALLY generate pending cleaning task
    System-->>FrontDesk: Settle Invoice & Complete checkout

    Note over Housekeeping, System: Housekeeping Turnover Phase
    Housekeeping->>System: View Pending tasks & click Start Cleaning
    System->>System: Set Room status to CleaningInProgress
    Housekeeping->>System: Finish cleaning & click Mark Completed
    System->>System: Set Task status to Completed
    System->>System: Revert Room status to Available (Publicly Bookable)
```

---
*Refer to the [USER_MANUAL.md](file:///c:/Users/farha/OneDrive/Desktop/CogStay--Final/CogStay---Project2/USER_MANUAL.md) for step-by-step navigation instructions.*
