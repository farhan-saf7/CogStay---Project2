using System.Linq;
using Microsoft.EntityFrameworkCore;
using CogStayMVC.Models;

namespace CogStayMVC.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<RoomType> RoomTypes { get; set; } = null!;
    public DbSet<Room> Rooms { get; set; } = null!;
    public DbSet<RoomImage> RoomImages { get; set; } = null!;
    public DbSet<Amenity> Amenities { get; set; } = null!;
    public DbSet<RoomAmenity> RoomAmenities { get; set; } = null!;
    public DbSet<Guest> Guests { get; set; } = null!;
    public DbSet<Role> Roles { get; set; } = null!;
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Staff> Staffs { get; set; } = null!;
    public DbSet<Reservation> Reservations { get; set; } = null!;
    public DbSet<StayRecord> StayRecords { get; set; } = null!;
    public DbSet<Billing> Billings { get; set; } = null!;
    public DbSet<Payment> Payments { get; set; } = null!;
    public DbSet<HousekeepingTask> HousekeepingTasks { get; set; } = null!;
    public DbSet<ServiceRequest> ServiceRequests { get; set; } = null!;
    public DbSet<MaintenanceRequest> MaintenanceRequests { get; set; } = null!;
    public DbSet<Notification> Notifications { get; set; } = null!;
    public DbSet<Feedback> Feedbacks { get; set; } = null!;
    public DbSet<AuditLog> AuditLogs { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // 1. Composite Key for RoomAmenity (Many-to-Many Join Table)
        modelBuilder.Entity<RoomAmenity>()
            .HasKey(ra => new { ra.RoomId, ra.AmenityId });

        modelBuilder.Entity<RoomAmenity>()
            .HasOne(ra => ra.Room)
            .WithMany(r => r.RoomAmenities)
            .HasForeignKey(ra => ra.RoomId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<RoomAmenity>()
            .HasOne(ra => ra.Amenity)
            .WithMany(a => a.RoomAmenities)
            .HasForeignKey(ra => ra.AmenityId)
            .OnDelete(DeleteBehavior.Cascade);

        // 2. Configure 1-to-1 relationships
        
        // User <-> Guest (One-to-One)
        modelBuilder.Entity<Guest>()
            .HasOne(g => g.User)
            .WithOne(u => u.Guest)
            .HasForeignKey<Guest>(g => g.UserId)
            .OnDelete(DeleteBehavior.SetNull);

        // User <-> Staff (One-to-One)
        modelBuilder.Entity<Staff>()
            .HasOne(s => s.User)
            .WithOne(u => u.Staff)
            .HasForeignKey<Staff>(s => s.UserId)
            .OnDelete(DeleteBehavior.SetNull);

        // Reservation <-> StayRecord (One-to-One)
        modelBuilder.Entity<StayRecord>()
            .HasOne(sr => sr.Reservation)
            .WithOne(r => r.StayRecord)
            .HasForeignKey<StayRecord>(sr => sr.ReservationId)
            .OnDelete(DeleteBehavior.Restrict);

        // StayRecord <-> Billing (One-to-One)
        modelBuilder.Entity<Billing>()
            .HasOne(b => b.StayRecord)
            .WithOne(sr => sr.Billing)
            .HasForeignKey<Billing>(b => b.StayRecordId)
            .OnDelete(DeleteBehavior.Cascade);

        // 3. Configure Restrict Delete behaviors to prevent cyclical cascades in SQL Server
        
        modelBuilder.Entity<Reservation>()
            .HasOne(r => r.Guest)
            .WithMany(g => g.Reservations)
            .HasForeignKey(r => r.GuestId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Reservation>()
            .HasOne(r => r.Room)
            .WithMany(rm => rm.Reservations)
            .HasForeignKey(r => r.RoomId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<StayRecord>()
            .HasOne(sr => sr.Guest)
            .WithMany(g => g.StayRecords)
            .HasForeignKey(sr => sr.GuestId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<User>()
            .HasOne(u => u.Role)
            .WithMany(r => r.Users)
            .HasForeignKey(u => u.RoleId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Feedback>()
            .HasOne(f => f.Guest)
            .WithMany(g => g.Feedbacks)
            .HasForeignKey(f => f.GuestId)
            .OnDelete(DeleteBehavior.Cascade);

        modelBuilder.Entity<Feedback>()
            .HasOne(f => f.Room)
            .WithMany()
            .HasForeignKey(f => f.RoomId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<HousekeepingTask>()
            .HasOne(ht => ht.AssignedToStaff)
            .WithMany(s => s.AssignedTasks)
            .HasForeignKey(ht => ht.AssignedToStaffId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<ServiceRequest>()
            .HasOne(sr => sr.AssignedToStaff)
            .WithMany(s => s.AssignedServiceRequests)
            .HasForeignKey(sr => sr.AssignedToStaffId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<MaintenanceRequest>()
            .HasOne(mr => mr.ReportedByStaff)
            .WithMany(s => s.ReportedMaintenanceRequests)
            .HasForeignKey(mr => mr.ReportedByStaffId)
            .OnDelete(DeleteBehavior.SetNull);

        // 4. Configure Decimal Precision (18, 2) for all currency fields
        foreach (var property in modelBuilder.Model.GetEntityTypes()
            .SelectMany(t => t.GetProperties())
            .Where(p => p.ClrType == typeof(decimal) || p.ClrType == typeof(decimal?)))
        {
            property.SetPrecision(18);
            property.SetScale(2);
        }

        // 5. Unique Constraints and Core Indexes
        modelBuilder.Entity<Room>()
            .HasIndex(r => r.RoomNumber)
            .IsUnique();

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Username)
            .IsUnique();

        modelBuilder.Entity<User>()
            .HasIndex(u => u.Email)
            .IsUnique();

        modelBuilder.Entity<Role>()
            .HasIndex(r => r.Name)
            .IsUnique();

        // 6. Recommended Operational Indexes
        modelBuilder.Entity<Reservation>()
            .HasIndex(r => r.Status);
        
        modelBuilder.Entity<Reservation>()
            .HasIndex(r => new { r.CheckInDate, r.CheckOutDate });

        modelBuilder.Entity<Guest>()
            .HasIndex(g => new { g.FirstName, g.LastName });

        modelBuilder.Entity<Payment>()
            .HasIndex(p => p.TransactionId);

        // 7. Global Query Filters for Soft Deletion
        modelBuilder.Entity<RoomType>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Room>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<RoomImage>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Amenity>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Guest>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Role>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<User>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Staff>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Reservation>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<StayRecord>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Billing>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Payment>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<HousekeepingTask>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<ServiceRequest>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<MaintenanceRequest>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Notification>().HasQueryFilter(e => !e.IsDeleted);
        modelBuilder.Entity<Feedback>().HasQueryFilter(e => !e.IsDeleted);
    }
}
