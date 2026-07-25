# 📖 CogStay User Manual & Operating Guide

Welcome to the **CogStay Hotel Management System**. This guide is written for non-technical users (Hotel Staff and Guests) to understand how to log in, navigate the portal dashboards, and run daily hotel operations.

---

## 👥 Role Profiles & Access Levels

The system divides hotel functions into five distinct roles. Each role has access to specific pages and functions:

| User Role | Main Responsibilities | Access Level / Features |
| :--- | :--- | :--- |
| **Guest (Customer)** | Discover rooms, check prices, reserve rooms, view billing, submit reviews. | Guest Dashboard, Profile, Feedback submission. |
| **Front Desk** | Check in incoming guests, request checkouts, record room changes. | Front Desk Dashboard, Stay List, Payments. |
| **Housekeeping** | Clean vacant dirty rooms, start/complete cleaning assignments. | Cleaning lists, Room clean/dirty toggles. |
| **Manager** | View feedback sheets, review occupancy ratios, monitor billing totals. | Manager Dashboard, Feedback moderation, Occupancy stats. |
| **Admin** | Register employee accounts, add/remove rooms, adjust prices. | Admin Panel, Staff Register, Room CRUD. |

---

## 🖥 How to Access the Portals

1.  **Open Browser:** Launch Google Chrome, Microsoft Edge, or Safari.
2.  **Navigate to Link:** Go to the URL provided by your administrator (typically `https://localhost:7147` in development).
3.  **Choose Login Type:**
    *   **Guests:** Click the **Guest Portal / Book Now** button on the homepage to register or log in.
    *   **Staff:** Navigate to the footer or top navigation bar and select **Staff Login**. Select your appropriate role (FrontDesk, Housekeeping, Manager, Admin) to proceed.

---

## 🔄 Daily Operational Workflows

### 1. Guest Booking Workflow (For Guests)
1.  Log in to the **Guest Portal**.
2.  Select **Available Rooms** from the dashboard.
3.  Choose your desired Room, enter **Check-In** and **Check-Out** dates, and click **Confirm Booking**.
4.  Once booked, the reservation details will appear under **My Bookings** as *Booked*.

### 2. Arrival & Check-In Workflow (For Front Desk Staff)
1.  Log in to the **Staff Portal** with the **Front Desk** role selected.
2.  Under **Bookings / Reservations**, find the guest's name or reservation reference.
3.  Click the **Check-In** button.
4.  The room status automatically updates to **Occupied**. The guest is now checked in!

### 3. Check-Out Request (For Front Desk / Guests)
1.  When a guest wants to leave, they can request checkout from their Guest Dashboard.
2.  The Front Desk staff will see the stay record status shift to **Checkout Pending**.

### 4. Billing Settlement & Payment (For Front Desk Staff)
1.  Navigate to **Billing & Payments** on the Front Desk Dashboard.
2.  Find the active stay record and click **Generate Bill**. The system automatically computes the total based on: `Price Per Night * Number of Nights`.
3.  Click **Process Payment**. Enter the payment method (Cash, Card, or Online Transfer) and hit Save.
4.  Upon payment completion, the stay record is completed and closed.
5.  **Workflow Trigger:** The room status shifts to **Cleaning Required (Dirty)**, and a pending cleaning task is automatically sent to the Housekeeping department.

### 5. Room Cleaning & Turnover (For Housekeeping Staff)
1.  Log in to the **Staff Portal** as **Housekeeping**.
2.  Your dashboard lists all dirty rooms under **Pending Cleaning Tasks**.
3.  Select a task and click **Start Cleaning**. The room status shifts to **Cleaning In Progress**.
4.  Once the room is cleaned, click **Mark Completed**. The room status instantly reverts to **Available**, making it visible on the Guest booking page.

---

## 🧭 Dashboard Navigation Help

*   **Header Navigation:** Use the header menu to hop between different views (e.g. Profile, Bookings, Feedbacks).
*   **Sidebar Navigation (Staff Area):** Click sidebar items to jump straight to Check-In lists, Housekeeping assignments, or Staff registrations.
*   **Session Display:** Your name and current role are shown at the top right of the navigation bar.

---

## 🛠 Troubleshooting Common Issues

### 1. "Invalid Credentials" Error
*   Ensure that you have entered your email correctly (emails are case-sensitive).
*   Ensure that you selected the correct role from the login drop-down menu. If you try to log in as a *Manager* using *Front Desk* credentials, access will be blocked.

### 2. Can't Log Out / Staged in Previous View
*   Click the **Logout** button on the top right navigation bar.
*   If the session persists, clear your browser cookies and cache (Press `Ctrl + Shift + Delete` on Windows or `Cmd + Shift + Delete` on Mac) and reload the website.

### 3. A Room Isn't Showing on Booking Page
*   The room might be occupied, already booked for that date range, or under maintenance.
*   If the room was recently vacated, ensure that the Housekeeping staff has clicked **Mark Completed** on their cleaning task.

---
*For software support, contact your system administrator.*
