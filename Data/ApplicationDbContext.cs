using Microsoft.EntityFrameworkCore;
using CogStayMVC.Models.Admin;
using CogStayMVC.Models.Customer;
using CogStayMVC.Models.FrontDesk;
using CogStayMVC.Models.Housekeeping;
using CogStayMVC.Models.Billing;
using CogStayMVC.Models.Room;
using CogStayMVC.Models.Common;
using System.Text.Json;
using System.Collections.Generic;

namespace CogStayMVC.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Customer> Customers { get; set; }
    public DbSet<StaffUser> StaffUsers { get; set; }
    public DbSet<Room> Rooms { get; set; }
    public DbSet<RoomAmenity> RoomAmenities { get; set; }
    public DbSet<RoomImage> RoomImages { get; set; }
    public DbSet<Reservation> Reservations { get; set; }
    public DbSet<StayRecord> StayRecords { get; set; }
    public DbSet<Billing> Billings { get; set; }
    public DbSet<Payment> Payments { get; set; }
    public DbSet<Feedback> Feedbacks { get; set; }
    public DbSet<HousekeepingTask> HousekeepingTasks { get; set; }
    public DbSet<ServiceRequest> ServiceRequests { get; set; }
    public DbSet<LaundryRequest> LaundryRequests { get; set; }
    public DbSet<FoodOrder> FoodOrders { get; set; }
    public DbSet<WellnessSpaRequest> WellnessSpaRequests { get; set; }
    public DbSet<MaintenanceRequest> MaintenanceRequests { get; set; }
    public DbSet<Notification> Notifications { get; set; }
    public DbSet<LoginHistory> LoginHistories { get; set; }
    public DbSet<RefreshToken> RefreshTokens { get; set; }
    public DbSet<AuditLog> AuditLogs { get; set; }
    public DbSet<ContactMessage> ContactMessages { get; set; }
    public DbSet<OTP> Otps { get; set; }
    public DbSet<Booking> Bookings { get; set; } // Compatibility table

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // 1. User & Role Config
        modelBuilder.Entity<User>()
            .HasIndex(u => u.Username)
            .IsUnique();

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<Role>()
            .HasIndex(r => r.RoleName)
            .IsUnique();

        modelBuilder.Entity<User>()
            .HasOne(u => u.Role)
            .WithMany(r => r.Users)
            .HasForeignKey(u => u.RoleId)
            .OnDelete(DeleteBehavior.Restrict);

        // 2. Customer & User (One-to-One)
        modelBuilder.Entity<Customer>()
            .HasOne(c => c.User)
            .WithOne(u => u.CustomerProfile)
            .HasForeignKey<Customer>(c => c.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Customer>()
            .HasIndex(c => c.Email)
            .IsUnique();

        // 3. StaffUser & User (One-to-One)
        modelBuilder.Entity<StaffUser>()
            .HasOne(s => s.User)
            .WithOne(u => u.StaffProfile)
            .HasForeignKey<StaffUser>(s => s.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        // 4. Room Config
        modelBuilder.Entity<Room>()
            .HasIndex(r => r.RoomNumber)
            .IsUnique();

        modelBuilder.Entity<Room>()
            .Property(r => r.Status)
            .HasConversion<string>();

        // 5. Room relations
        modelBuilder.Entity<RoomAmenity>()
            .HasOne(ra => ra.Room)
            .WithMany(r => r.RoomAmenities)
            .HasForeignKey(ra => ra.RoomId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<RoomImage>()
            .HasOne(ri => ri.Room)
            .WithMany(r => r.RoomImages)
            .HasForeignKey(ri => ri.RoomId)
            .OnDelete(DeleteBehavior.Cascade);

        // 6. Reservation Relations
        modelBuilder.Entity<Reservation>()
            .Property(r => r.ReservationStatus)
            .HasConversion<string>();

        modelBuilder.Entity<Reservation>()
            .HasOne(r => r.Guest)
            .WithMany(c => c.Reservations)
            .HasForeignKey(r => r.GuestId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Reservation>()
            .HasOne(r => r.Room)
            .WithMany(room => room.Reservations)
            .HasForeignKey(r => r.RoomId)
            .OnDelete(DeleteBehavior.Restrict);

        // 7. StayRecord Relations
        modelBuilder.Entity<StayRecord>()
            .HasOne(sr => sr.Guest)
            .WithMany(c => c.StayRecords)
            .HasForeignKey(sr => sr.GuestId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<StayRecord>()
            .HasOne(sr => sr.Reservation)
            .WithOne(r => r.StayRecord)
            .HasForeignKey<StayRecord>(sr => sr.ReservationId)
            .OnDelete(DeleteBehavior.Restrict);

        // 8. Billing Relations
        modelBuilder.Entity<Billing>()
            .Property(b => b.PaymentStatus)
            .HasConversion<string>();

        modelBuilder.Entity<Billing>()
            .HasOne(b => b.StayRecord)
            .WithOne(sr => sr.Billing)
            .HasForeignKey<Billing>(b => b.StayId)
            .OnDelete(DeleteBehavior.Restrict);

        // 9. Payment Relations
        modelBuilder.Entity<Payment>()
            .HasOne(p => p.Billing)
            .WithMany(b => b.Payments)
            .HasForeignKey(p => p.BillingId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Payment>()
            .HasOne(p => p.Reservation)
            .WithMany(r => r.Payments)
            .HasForeignKey(p => p.ReservationId)
            .OnDelete(DeleteBehavior.Restrict);

        // 10. Feedback Relations
        modelBuilder.Entity<Feedback>()
            .HasOne(f => f.Guest)
            .WithMany(c => c.Feedbacks)
            .HasForeignKey(f => f.CustomerId)
            .OnDelete(DeleteBehavior.Restrict);

        // 11. HousekeepingTask Relations
        modelBuilder.Entity<HousekeepingTask>()
            .Property(h => h.TaskStatus)
            .HasConversion<string>();

        modelBuilder.Entity<HousekeepingTask>()
            .HasOne(h => h.Room)
            .WithMany(r => r.HousekeepingTasks)
            .HasForeignKey(h => h.RoomId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<HousekeepingTask>()
            .HasOne(h => h.AssignedStaff)
            .WithMany(s => s.HousekeepingTasks)
            .HasForeignKey(h => h.AssignedToStaffId)
            .OnDelete(DeleteBehavior.Restrict);

        // Value converter and comparer for List<string> to JSON string database field
        var checklistComparer = new Microsoft.EntityFrameworkCore.ChangeTracking.ValueComparer<List<string>>(
            (c1, c2) => c1 != null && c2 != null ? System.Linq.Enumerable.SequenceEqual(c1, c2) : c1 == c2,
            c => c.GetHashCode(),
            c => new List<string>(c)
        );

        modelBuilder.Entity<HousekeepingTask>()
            .Property(t => t.ChecklistItems)
            .HasConversion(
                v => JsonSerializer.Serialize(v, (JsonSerializerOptions)null!),
                v => JsonSerializer.Deserialize<List<string>>(v, (JsonSerializerOptions)null!) ?? new List<string>()
            )
            .Metadata.SetValueComparer(checklistComparer);

        // 12. General Service & Department Requests
        modelBuilder.Entity<ServiceRequest>()
            .HasOne(s => s.Room)
            .WithMany()
            .HasForeignKey(s => s.RoomId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<LaundryRequest>()
            .HasOne(l => l.Room)
            .WithMany()
            .HasForeignKey(l => l.RoomId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<FoodOrder>()
            .HasOne(f => f.Room)
            .WithMany()
            .HasForeignKey(f => f.RoomId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<WellnessSpaRequest>()
            .HasOne(w => w.Room)
            .WithMany()
            .HasForeignKey(w => w.RoomId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<MaintenanceRequest>()
            .HasOne(m => m.Room)
            .WithMany()
            .HasForeignKey(m => m.RoomId)
            .OnDelete(DeleteBehavior.Restrict);

        // 13. Audit & Logging Cascade Configs
        modelBuilder.Entity<Notification>()
            .HasOne(n => n.User)
            .WithMany(u => u.Notifications)
            .HasForeignKey(n => n.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<LoginHistory>()
            .HasOne(lh => lh.User)
            .WithMany(u => u.LoginHistories)
            .HasForeignKey(lh => lh.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<RefreshToken>()
            .HasOne(rt => rt.User)
            .WithMany(u => u.RefreshTokens)
            .HasForeignKey(rt => rt.UserId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<AuditLog>()
            .HasOne(al => al.User)
            .WithMany(u => u.AuditLogs)
            .HasForeignKey(al => al.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}
