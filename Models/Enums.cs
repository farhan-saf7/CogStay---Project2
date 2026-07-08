namespace CogStayMVC.Models;

public enum RoomStatus
{
    Available,
    Occupied,
    Reserved,
    Cleaning,
    Maintenance,
    OutOfService
}

public enum ReservationStatus
{
    Pending,
    Confirmed,
    CheckedIn,
    CheckedOut,
    Cancelled,
    Completed
}

public enum PaymentStatus
{
    Pending,
    Paid,
    Failed,
    Refunded,
    Partial
}

public enum TaskStatus
{
    Pending,
    Assigned,
    InProgress,
    Completed,
    Cancelled
}

public enum TaskPriority
{
    Low,
    Medium,
    High,
    Urgent
}

public enum ServiceType
{
    Laundry,
    FoodService,
    Cleaning,
    WellnessSpa
}
