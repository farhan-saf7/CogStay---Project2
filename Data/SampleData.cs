using CogStayMVC.Models.Admin;
using CogStayMVC.Models.Customer;
using CogStayMVC.Models.FrontDesk;
using CogStayMVC.Models.Housekeeping;
using CogStayMVC.Models.Billing;
using CogStayMVC.Models.Room;
using CogStayMVC.Models.Common;
using System;
using System.Collections.Generic;

namespace CogStayMVC.Data;

public static class SampleData
{
    public static List<Room> Rooms = new()
    {
        new Room { RoomId = 1, RoomNumber = "Room 101", RoomType = "Superior King", PricePerNight = 140.00m, Capacity = 2, IsAvailable = true, Status = RoomStatus.AVAILABLE, Cleanliness = "Clean" },
        new Room { RoomId = 2, RoomNumber = "Room 102", RoomType = "Superior King", PricePerNight = 140.00m, Capacity = 2, IsAvailable = false, Status = RoomStatus.OCCUPIED, Cleanliness = "Dirty" },
        new Room { RoomId = 3, RoomNumber = "Room 201", RoomType = "Deluxe Executive", PricePerNight = 180.00m, Capacity = 2, IsAvailable = true, Status = RoomStatus.AVAILABLE, Cleanliness = "Clean" },
        new Room { RoomId = 4, RoomNumber = "Room 202", RoomType = "Deluxe Executive", PricePerNight = 185.00m, Capacity = 4, IsAvailable = false, Status = RoomStatus.OCCUPIED, Cleanliness = "Clean" },
        new Room { RoomId = 5, RoomNumber = "Room 304", RoomType = "Penthouse Suite", PricePerNight = 350.00m, Capacity = 2, IsAvailable = false, Status = RoomStatus.OCCUPIED, Cleanliness = "Dirty" },
        new Room { RoomId = 6, RoomNumber = "Room 305", RoomType = "Penthouse Suite", PricePerNight = 350.00m, Capacity = 2, IsAvailable = false, Status = RoomStatus.UNDER_MAINTENANCE, Cleanliness = "Dirty" },
        new Room { RoomId = 7, RoomNumber = "Room 401", RoomType = "Presidential Suite", PricePerNight = 360.00m, Capacity = 4, IsAvailable = true, Status = RoomStatus.AVAILABLE, Cleanliness = "Clean" }
    };

    public static List<Booking> Bookings = new()
    {
        new Booking { Id = 9024, CustomerName = "John Henderson", RoomNumber = "Room 304", CheckInDate = DateTime.Today, CheckOutDate = DateTime.Today.AddDays(4), GuestsCount = 2, TotalAmount = 1568.00m, BookingStatus = "Checked In", PaymentStatus = "Partial" },
        new Booking { Id = 9288, CustomerName = "Emily Watson", RoomNumber = "Room 301", CheckInDate = DateTime.Today.AddDays(21), CheckOutDate = DateTime.Today.AddDays(24), GuestsCount = 1, TotalAmount = 604.80m, BookingStatus = "Confirmed", PaymentStatus = "Paid" },
        new Booking { Id = 9402, CustomerName = "Robert Downey", RoomNumber = "Room 201", CheckInDate = DateTime.Today.AddDays(26), CheckOutDate = DateTime.Today.AddDays(31), GuestsCount = 2, TotalAmount = 1008.00m, BookingStatus = "Pending", PaymentStatus = "Unpaid" },
        new Booking { Id = 8711, CustomerName = "Albert Einstein", RoomNumber = "Room 201", CheckInDate = DateTime.Today.AddDays(-45), CheckOutDate = DateTime.Today.AddDays(-43), GuestsCount = 2, TotalAmount = 313.60m, BookingStatus = "Completed", PaymentStatus = "Paid" },
        new Booking { Id = 8430, CustomerName = "Grace Hopper", RoomNumber = "Room 202", CheckInDate = DateTime.Today.AddDays(-112), CheckOutDate = DateTime.Today.AddDays(-108), GuestsCount = 2, TotalAmount = 604.80m, BookingStatus = "Completed", PaymentStatus = "Paid" },
        new Booking { Id = 8012, CustomerName = "Charles Babbage", RoomNumber = "Room 302", CheckInDate = DateTime.Today.AddDays(-164), CheckOutDate = DateTime.Today.AddDays(-161), GuestsCount = 2, TotalAmount = 621.60m, BookingStatus = "Cancelled", PaymentStatus = "Refunded" }
    };

    public static List<StaffUser> StaffUsers = new()
    {
        new StaffUser { StaffUserId = 1, UserId = 1, Name = "John Doe", ShiftSchedule = "24/7 Support", Status = "Active" },
        new StaffUser { StaffUserId = 2, UserId = 2, Name = "Robert Downey", ShiftSchedule = "Day Shift", Status = "Active" },
        new StaffUser { StaffUserId = 3, UserId = 3, Name = "John Smith", ShiftSchedule = "Night Shift", Status = "Active" },
        new StaffUser { StaffUserId = 4, UserId = 4, Name = "Sarah Connor", ShiftSchedule = "Day Shift", Status = "Active" },
        new StaffUser { StaffUserId = 5, UserId = 5, Name = "Mary Poppins", ShiftSchedule = "Day Shift", Status = "Active" },
        new StaffUser { StaffUserId = 6, UserId = 6, Name = "Bruce Wayne", ShiftSchedule = "Night Shift", Status = "Active" },
        new StaffUser { StaffUserId = 7, UserId = 7, Name = "Peter Parker", ShiftSchedule = "On Call", Status = "On Leave" }
    };

    public static List<HousekeepingTask> HousekeepingTasks = new()
    {
        new HousekeepingTask { TaskId = 101, RoomId = 5, TaskType = "Checkout Clean", Priority = "Urgent", TaskStatus = HousekeepingTaskStatus.IN_PROGRESS, AssignedToStaffId = 6, CreatedAt = DateTime.UtcNow, ChecklistItems = new() { "Strip bed linen and pillows", "Vacuum bedroom and dust lounge desk", "Sanitize bathroom and mop tile floors", "Restock toiletries, mini-bar, and fresh towels" } },
        new HousekeepingTask { TaskId = 102, RoomId = 2, TaskType = "Stay-Over Clean", Priority = "Medium", TaskStatus = HousekeepingTaskStatus.PENDING, AssignedToStaffId = 5, CreatedAt = DateTime.UtcNow, ChecklistItems = new() { "Make bed neatly (replace sheet if requested)", "Empty trashbins and restock toilet paper", "Wipe surfaces and replace bath towels" } },
        new HousekeepingTask { TaskId = 103, RoomId = 4, TaskType = "Checkout Clean", Priority = "Medium", TaskStatus = HousekeepingTaskStatus.PENDING, AssignedToStaffId = 7, CreatedAt = DateTime.UtcNow, ChecklistItems = new() { "Strip bed linen", "Vacuum carpets", "Wipe all switches", "Sanitize toilets" } }
    };

    public static List<Feedback> Feedbacks = new()
    {
        new Feedback { FeedbackId = 1, GuestName = "John Henderson", RoomNumber = "Room 304", Rating = 5, Category = "Room Comfort & Service", Comments = "The room service dry cleaning was returned in under 4 hours, and the Penthouse view was exceptional. CogStay system operates very smoothly.", IsApproved = true },
        new Feedback { FeedbackId = 2, GuestName = "Emily Watson", RoomNumber = "Room 301", Rating = 4, Category = "General Stay Value", Comments = "Very comfortable Deluxe room, excellent writing desk configuration. The AC unit was slightly loud but resolved quickly by maintenance.", IsApproved = true },
        new Feedback { FeedbackId = 3, GuestName = "Albert Einstein", RoomNumber = "Room 201", Rating = 5, Category = "Amenities & Facilities", Comments = "Spa aromatherapy bath salts are highly recommended. Front desk agent processed our late checkout with extreme courtesy.", IsApproved = true }
    };
}
