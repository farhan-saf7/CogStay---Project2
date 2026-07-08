using System;
using System.Collections.Generic;
using CogStayMVC.Models;

namespace CogStayMVC.Data;

public static class SampleData
{
    public static List<RoomType> RoomTypes = new()
    {
        new RoomType { Id = 1, Name = "Superior King", Description = "Luxury room with one King bed", BasePrice = 140.00m, MaxCapacity = 2 },
        new RoomType { Id = 2, Name = "Deluxe Executive", Description = "Spacious room with modern amenities", BasePrice = 180.00m, MaxCapacity = 4 },
        new RoomType { Id = 3, Name = "Penthouse Suite", Description = "Premium suite with panoramic city views", BasePrice = 350.00m, MaxCapacity = 2 },
        new RoomType { Id = 4, Name = "Presidential Suite", Description = "The ultimate luxury stay with personal services", BasePrice = 500.00m, MaxCapacity = 4 }
    };

    public static List<Room> Rooms = new()
    {
        new Room { Id = 1, RoomNumber = "Room 101", RoomTypeId = 1, PricePerNight = 140.00m, Capacity = 2, IsAvailable = true, Status = RoomStatus.Available, Cleanliness = "Clean" },
        new Room { Id = 2, RoomNumber = "Room 102", RoomTypeId = 1, PricePerNight = 140.00m, Capacity = 2, IsAvailable = false, Status = RoomStatus.Occupied, Cleanliness = "Dirty" },
        new Room { Id = 3, RoomNumber = "Room 201", RoomTypeId = 2, PricePerNight = 180.00m, Capacity = 2, IsAvailable = true, Status = RoomStatus.Available, Cleanliness = "Clean" },
        new Room { Id = 4, RoomNumber = "Room 202", RoomTypeId = 2, PricePerNight = 185.00m, Capacity = 4, IsAvailable = false, Status = RoomStatus.Occupied, Cleanliness = "Clean" },
        new Room { Id = 5, RoomNumber = "Room 304", RoomTypeId = 3, PricePerNight = 350.00m, Capacity = 2, IsAvailable = false, Status = RoomStatus.Occupied, Cleanliness = "Dirty" },
        new Room { Id = 6, RoomNumber = "Room 305", RoomTypeId = 3, PricePerNight = 350.00m, Capacity = 2, IsAvailable = false, Status = RoomStatus.Maintenance, Cleanliness = "Dirty" },
        new Room { Id = 7, RoomNumber = "Room 401", RoomTypeId = 4, PricePerNight = 360.00m, Capacity = 4, IsAvailable = true, Status = RoomStatus.Available, Cleanliness = "Clean" }
    };

    public static List<Guest> Guests = new()
    {
        new Guest { Id = 1, FirstName = "John", LastName = "Henderson", Email = "john.h@example.com", Phone = "555-0199", LoyaltyTier = "Platinum", LoyaltyId = "CS-LOY-901" },
        new Guest { Id = 2, FirstName = "Emily", LastName = "Watson", Email = "emily.w@example.com", Phone = "555-0211", LoyaltyTier = "Gold", LoyaltyId = "CS-LOY-402" },
        new Guest { Id = 3, FirstName = "Robert", LastName = "Downey", Email = "robert.d@example.com", Phone = "555-0322", LoyaltyTier = "Bronze", LoyaltyId = "CS-LOY-110" },
        new Guest { Id = 4, FirstName = "Albert", LastName = "Einstein", Email = "albert.e@example.com", Phone = "555-0455", LoyaltyTier = "Gold", LoyaltyId = "CS-LOY-772" },
        new Guest { Id = 5, FirstName = "Grace", LastName = "Hopper", Email = "grace.h@example.com", Phone = "555-0588", LoyaltyTier = "Platinum", LoyaltyId = "CS-LOY-881" },
        new Guest { Id = 6, FirstName = "Charles", LastName = "Babbage", Email = "charles.b@example.com", Phone = "555-0612", LoyaltyTier = "Silver", LoyaltyId = "CS-LOY-220" }
    };

    public static List<Staff> Staffs = new()
    {
        new Staff { Id = 1, FirstName = "John", LastName = "Doe", Email = "john.doe@hotel.com", Phone = "555-1122", ShiftSchedule = "24/7 Support", Status = "Active" },
        new Staff { Id = 2, FirstName = "Robert", LastName = "Downey", Email = "robert.mgr@hotel.com", Phone = "555-2233", ShiftSchedule = "Day Shift", Status = "Active" },
        new Staff { Id = 3, FirstName = "John", LastName = "Smith", Email = "john.smith@hotel.com", Phone = "555-3344", ShiftSchedule = "Night Shift", Status = "Active" },
        new Staff { Id = 4, FirstName = "Sarah", LastName = "Connor", Email = "sarah.c@hotel.com", Phone = "555-4455", ShiftSchedule = "Day Shift", Status = "Active" },
        new Staff { Id = 5, FirstName = "Mary", LastName = "Poppins", Email = "mary.p@hotel.com", Phone = "555-5566", ShiftSchedule = "Day Shift", Status = "Active" },
        new Staff { Id = 6, FirstName = "Bruce", LastName = "Wayne", Email = "bruce.w@hotel.com", Phone = "555-6677", ShiftSchedule = "Night Shift", Status = "Active" },
        new Staff { Id = 7, FirstName = "Peter", LastName = "Parker", Email = "peter.p@hotel.com", Phone = "555-7788", ShiftSchedule = "On Call", Status = "On Leave" }
    };

    public static List<Reservation> Reservations = new()
    {
        new Reservation { Id = 9024, ReservationNumber = "CS-10024", GuestId = 1, RoomId = 5, CheckInDate = DateTime.Today, CheckOutDate = DateTime.Today.AddDays(4), GuestsCount = 2, TotalAmount = 1568.00m, Status = ReservationStatus.CheckedIn, SpecialRequests = "Dry cleaning required" },
        new Reservation { Id = 9288, ReservationNumber = "CS-10288", GuestId = 2, RoomId = 3, CheckInDate = DateTime.Today.AddDays(21), CheckOutDate = DateTime.Today.AddDays(24), GuestsCount = 1, TotalAmount = 604.80m, Status = ReservationStatus.Confirmed, SpecialRequests = "Late checkout check" },
        new Reservation { Id = 9402, ReservationNumber = "CS-10402", GuestId = 3, RoomId = 3, CheckInDate = DateTime.Today.AddDays(26), CheckOutDate = DateTime.Today.AddDays(31), GuestsCount = 2, TotalAmount = 1008.00m, Status = ReservationStatus.Pending, SpecialRequests = "Quiet room please" },
        new Reservation { Id = 8711, ReservationNumber = "CS-10711", GuestId = 4, RoomId = 3, CheckInDate = DateTime.Today.AddDays(-45), CheckOutDate = DateTime.Today.AddDays(-43), GuestsCount = 2, TotalAmount = 313.60m, Status = ReservationStatus.Completed, SpecialRequests = "" },
        new Reservation { Id = 8430, ReservationNumber = "CS-10430", GuestId = 5, RoomId = 4, CheckInDate = DateTime.Today.AddDays(-112), CheckOutDate = DateTime.Today.AddDays(-108), GuestsCount = 2, TotalAmount = 604.80m, Status = ReservationStatus.Completed, SpecialRequests = "" },
        new Reservation { Id = 8012, ReservationNumber = "CS-10012", GuestId = 6, RoomId = 3, CheckInDate = DateTime.Today.AddDays(-164), CheckOutDate = DateTime.Today.AddDays(-161), GuestsCount = 2, TotalAmount = 621.60m, Status = ReservationStatus.Cancelled, SpecialRequests = "" }
    };

    public static List<HousekeepingTask> HousekeepingTasks = new()
    {
        new HousekeepingTask { Id = 101, RoomId = 5, TaskType = "Checkout Clean", TaskDescription = "Full penthouse sanitize", AssignedToStaffId = 6, Priority = TaskPriority.Urgent, Status = Models.TaskStatus.InProgress, ChecklistItems = new() { "Strip bed linen and pillows", "Vacuum bedroom and dust lounge desk", "Sanitize bathroom and mop tile floors", "Restock toiletries, mini-bar, and fresh towels" } },
        new HousekeepingTask { Id = 102, RoomId = 2, TaskType = "Stay-Over Clean", TaskDescription = "Refresh room amenities", AssignedToStaffId = 5, Priority = TaskPriority.Medium, Status = Models.TaskStatus.Pending, ChecklistItems = new() { "Make bed neatly (replace sheet if requested)", "Empty trashbins and restock toilet paper", "Wipe surfaces and replace bath towels" } },
        new HousekeepingTask { Id = 103, RoomId = 4, TaskType = "Checkout Clean", TaskDescription = "Room cleaning after check out", AssignedToStaffId = 7, Priority = TaskPriority.Medium, Status = Models.TaskStatus.Pending, ChecklistItems = new() { "Strip bed linen", "Vacuum carpets", "Wipe all switches", "Sanitize toilets" } }
    };

    public static List<Feedback> Feedbacks = new()
    {
        new Feedback { Id = 1, GuestId = 1, RoomId = 5, Rating = 5, Category = "Room Comfort & Service", Comments = "The room service dry cleaning was returned in under 4 hours, and the Penthouse view was exceptional. CogStay system operates very smoothly.", IsApproved = true },
        new Feedback { Id = 2, GuestId = 2, RoomId = 3, Rating = 4, Category = "General Stay Value", Comments = "Very comfortable Deluxe room, excellent writing desk configuration. The AC unit was slightly loud but resolved quickly by maintenance.", IsApproved = true },
        new Feedback { Id = 3, GuestId = 4, RoomId = 3, Rating = 5, Category = "Amenities & Facilities", Comments = "Spa aromatherapy bath salts are highly recommended. Front desk agent processed our late checkout with extreme courtesy.", IsApproved = true }
    };
}
