using CogStayMVC.Models;

namespace CogStayMVC.Data;

public static class SampleData
{
    public static List<Room> Rooms = new()
    {
        new Room { Id = 1, RoomNumber = "Room 101", RoomType = "Superior King", PricePerNight = 140.00m, Capacity = 2, IsAvailable = true, Status = "Available", Cleanliness = "Clean" },
        new Room { Id = 2, RoomNumber = "Room 102", RoomType = "Superior King", PricePerNight = 140.00m, Capacity = 2, IsAvailable = false, Status = "Occupied", Cleanliness = "Dirty" },
        new Room { Id = 3, RoomNumber = "Room 201", RoomType = "Deluxe Executive", PricePerNight = 180.00m, Capacity = 2, IsAvailable = true, Status = "Available", Cleanliness = "Clean" },
        new Room { Id = 4, RoomNumber = "Room 202", RoomType = "Deluxe Executive", PricePerNight = 185.00m, Capacity = 4, IsAvailable = false, Status = "Occupied", Cleanliness = "Clean" },
        new Room { Id = 5, RoomNumber = "Room 304", RoomType = "Penthouse Suite", PricePerNight = 350.00m, Capacity = 2, IsAvailable = false, Status = "Occupied", Cleanliness = "Dirty" },
        new Room { Id = 6, RoomNumber = "Room 305", RoomType = "Penthouse Suite", PricePerNight = 350.00m, Capacity = 2, IsAvailable = false, Status = "Maintenance", Cleanliness = "Dirty" },
        new Room { Id = 7, RoomNumber = "Room 401", RoomType = "Presidential Suite", PricePerNight = 360.00m, Capacity = 4, IsAvailable = true, Status = "Available", Cleanliness = "Clean" }
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
        new StaffUser { Id = 1, Name = "John Doe", Username = "sysadmin", Role = "Admin", ShiftSchedule = "24/7 Support", Status = "Active" },
        new StaffUser { Id = 2, Name = "Robert Downey", Username = "manager_robert", Role = "Manager", ShiftSchedule = "Day Shift", Status = "Active" },
        new StaffUser { Id = 3, Name = "John Smith", Username = "frontdesk_john", Role = "Front Desk", ShiftSchedule = "Night Shift", Status = "Active" },
        new StaffUser { Id = 4, Name = "Sarah Connor", Username = "frontdesk_sarah", Role = "Front Desk", ShiftSchedule = "Day Shift", Status = "Active" },
        new StaffUser { Id = 5, Name = "Mary Poppins", Username = "housekeeping_mary", Role = "Housekeeper", ShiftSchedule = "Day Shift", Status = "Active" },
        new StaffUser { Id = 6, Name = "Bruce Wayne", Username = "housekeeping_bruce", Role = "Housekeeper", ShiftSchedule = "Night Shift", Status = "Active" },
        new StaffUser { Id = 7, Name = "Peter Parker", Username = "housekeeping_peter", Role = "Housekeeper", ShiftSchedule = "On Call", Status = "On Leave" }
    };

    public static List<HousekeepingTask> HousekeepingTasks = new()
    {
        new HousekeepingTask { Id = 101, RoomNumber = "Room 304", TaskType = "Checkout Clean", AssignedTo = "Bruce Wayne", Priority = "Urgent", Status = "In Progress", CreatedAt = DateTime.UtcNow, ChecklistItems = new() { "Strip bed linen and pillows", "Vacuum bedroom and dust lounge desk", "Sanitize bathroom and mop tile floors", "Restock toiletries, mini-bar, and fresh towels" } },
        new HousekeepingTask { Id = 102, RoomNumber = "Room 102", TaskType = "Stay-Over Clean", AssignedTo = "Mary Poppins", Priority = "Medium", Status = "Pending", CreatedAt = DateTime.UtcNow, ChecklistItems = new() { "Make bed neatly (replace sheet if requested)", "Empty trashbins and restock toilet paper", "Wipe surfaces and replace bath towels" } },
        new HousekeepingTask { Id = 103, RoomNumber = "Room 202", TaskType = "Checkout Clean", AssignedTo = "Peter Parker", Priority = "Medium", Status = "Pending", CreatedAt = DateTime.UtcNow, ChecklistItems = new() { "Strip bed linen", "Vacuum carpets", "Wipe all switches", "Sanitize toilets" } }
    };

    public static List<Feedback> Feedbacks = new()
    {
        new Feedback { Id = 1, GuestName = "John Henderson", RoomNumber = "Room 304", Rating = 5, Category = "Room Comfort & Service", Comments = "The room service dry cleaning was returned in under 4 hours, and the Penthouse view was exceptional. CogStay system operates very smoothly.", IsApproved = true },
        new Feedback { Id = 2, GuestName = "Emily Watson", RoomNumber = "Room 301", Rating = 4, Category = "General Stay Value", Comments = "Very comfortable Deluxe room, excellent writing desk configuration. The AC unit was slightly loud but resolved quickly by maintenance.", IsApproved = true },
        new Feedback { Id = 3, GuestName = "Albert Einstein", RoomNumber = "Room 201", Rating = 5, Category = "Amenities & Facilities", Comments = "Spa aromatherapy bath salts are highly recommended. Front desk agent processed our late checkout with extreme courtesy.", IsApproved = true }
    };
}
