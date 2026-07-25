using Microsoft.EntityFrameworkCore;
using CogStayMVC.Models;

namespace CogStayMVC.Data;

/// <summary>
/// Database context for the CogStay Hotel Management System.
/// Coordinates Entity Framework Core operations with the underlying SQL database,
/// managing tables, entity configurations, relationships, constraints, and cascade delete rules.
/// </summary>
public class HotelDbContext : DbContext
{
    /// <summary>
    /// Initializes a new instance of the <see cref="HotelDbContext"/> class with the specified options.
    /// </summary>
    /// <param name="options">The options to be used by the DbContext.</param>
    public HotelDbContext(DbContextOptions<HotelDbContext> options) : base(options)
    {
    }

    /// <summary>
    /// Gets or sets the database set for Guests.
    /// </summary>
    public DbSet<Guest> Guests { get; set; } = null!;

    /// <summary>
    /// Gets or sets the database set for Rooms.
    /// </summary>
    public DbSet<Room> Rooms { get; set; } = null!;

    /// <summary>
    /// Gets or sets the database set for Reservations.
    /// </summary>
    public DbSet<Reservation> Reservations { get; set; } = null!;

    /// <summary>
    /// Gets or sets the database set for Stay Records.
    /// </summary>
    public DbSet<StayRecord> StayRecords { get; set; } = null!;

    /// <summary>
    /// Gets or sets the database set for Housekeeping Tasks.
    /// </summary>
    public DbSet<HousekeepingTask> HousekeepingTasks { get; set; } = null!;

    /// <summary>
    /// Gets or sets the database set for Billings.
    /// </summary>
    public DbSet<Billing> Billings { get; set; } = null!;

    /// <summary>
    /// Gets or sets the database set for Staff.
    /// </summary>
    public DbSet<Staff> Staff { get; set; } = null!;

    /// <summary>
    /// Gets or sets the database set for Feedbacks.
    /// </summary>
    public DbSet<Feedback> Feedbacks { get; set; } = null!;

    /// <summary>
    /// Configures the EF Core schema, relationships, keys, indices, and constraints
    /// using the Fluent API.
    /// </summary>
    /// <param name="modelBuilder">The builder being used to construct the model for this context.</param>
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // --- Guest Configuration ---
        // Configures primary keys, property constraints, and indices for the Guest entity.
        modelBuilder.Entity<Guest>(entity =>
        {
            entity.HasKey(g => g.GuestId);
            entity.Property(g => g.FullName).IsRequired().HasMaxLength(100);
            entity.Property(g => g.Email).IsRequired().HasMaxLength(256);
            entity.Property(g => g.PhoneNumber).IsRequired().HasMaxLength(20);
            entity.Property(g => g.Address).IsRequired().HasMaxLength(500);
            entity.Property(g => g.PasswordHash).IsRequired().HasMaxLength(512);
            
            // Unique constraint on Email index to prevent duplicate accounts
            entity.HasIndex(g => g.Email).IsUnique();
        });

        // --- Room Configuration ---
        // Configures primary keys, string conversions, decimal precision, and unique indices for rooms.
        modelBuilder.Entity<Room>(entity =>
        {
            entity.HasKey(r => r.RoomId);
            entity.Property(r => r.RoomNumber).IsRequired().HasMaxLength(50);
            entity.Property(r => r.RoomType).IsRequired().HasMaxLength(100);
            entity.Property(r => r.PricePerNight).HasPrecision(18, 2);
            entity.Property(r => r.Status).HasConversion<string>().HasMaxLength(50);

            // Unique constraint on RoomNumber index
            entity.HasIndex(r => r.RoomNumber).IsUnique();
        });

        // --- Reservation Configuration ---
        // Configures check-in/out dates, status conversions, and foreign keys.
        modelBuilder.Entity<Reservation>(entity =>
        {
            entity.HasKey(res => res.ReservationId);
            entity.Property(res => res.GuestName).IsRequired().HasMaxLength(100).HasDefaultValue("");
            entity.Property(res => res.ReservationStatus).HasConversion<string>().HasMaxLength(50);

            // One Guest -> Many Reservations relationship config (DeleteBehavior.Restrict to prevent orphan deletions)
            entity.HasOne(res => res.Guest)
                .WithMany(g => g.Reservations)
                .HasForeignKey(res => res.GuestId)
                .OnDelete(DeleteBehavior.Restrict);

            // One Room -> Many Reservations relationship config
            entity.HasOne(res => res.Room)
                .WithMany(r => r.Reservations)
                .HasForeignKey(res => res.RoomId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // --- StayRecord Configuration ---
        // Configures one-to-many relationship with Guest and one-to-one with Reservation.
        modelBuilder.Entity<StayRecord>(entity =>
        {
            entity.HasKey(s => s.StayId);

            entity.Property(s => s.GuestName).IsRequired().HasMaxLength(100).HasDefaultValue("");
            entity.Property(s => s.BookingReference).HasMaxLength(100);
            entity.Property(s => s.BillingReference).HasMaxLength(100);
            entity.Property(s => s.StayDetails).HasMaxLength(500);

            // Many StayRecords -> One Guest (One-to-Many)
            entity.HasOne(s => s.Guest)
                .WithMany(g => g.StayRecords)
                .HasForeignKey(s => s.GuestId)
                .OnDelete(DeleteBehavior.Restrict);

            // One Reservation -> One StayRecord (One-to-One)
            entity.HasOne(s => s.Reservation)
                .WithOne(res => res.StayRecord)
                .HasForeignKey<StayRecord>(s => s.ReservationId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // --- HousekeepingTask Configuration ---
        // Configures cleaning assignments linked to rooms.
        modelBuilder.Entity<HousekeepingTask>(entity =>
        {
            entity.HasKey(t => t.TaskId);
            entity.Property(t => t.TaskDescription).IsRequired().HasMaxLength(1000);
            entity.Property(t => t.TaskStatus).HasConversion<string>().HasMaxLength(50);

            // One Room -> Many HousekeepingTasks
            entity.HasOne(t => t.Room)
                .WithMany(r => r.HousekeepingTasks)
                .HasForeignKey(t => t.RoomId)
                .OnDelete(DeleteBehavior.Restrict);
        });

        // --- Billing Configuration ---
        // Configures billing record constraints and its one-to-one link to a StayRecord.
        modelBuilder.Entity<Billing>(entity =>
        {
            entity.HasKey(b => b.BillId);
            entity.Property(b => b.GuestId).IsRequired().HasDefaultValue(0);
            entity.Property(b => b.GuestName).IsRequired().HasMaxLength(100).HasDefaultValue("");
            entity.Property(b => b.TotalAmount).HasPrecision(18, 2);
            entity.Property(b => b.PaymentStatus).HasConversion<string>().HasMaxLength(50);
            entity.Property(b => b.Remarks).HasMaxLength(500);

            // One StayRecord -> One Billing (One-to-One)
            // Cascade delete: when stay record is deleted, its billing info is cleared.
            entity.HasOne(b => b.StayRecord)
                .WithOne(s => s.Billing)
                .HasForeignKey<Billing>(b => b.StayId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // --- Staff Configuration ---
        // Configures staff identifiers, roles, and unique email logins.
        modelBuilder.Entity<Staff>(entity =>
        {
            entity.HasKey(s => s.StaffId);
            entity.Property(s => s.FullName).IsRequired().HasMaxLength(100);
            entity.Property(s => s.Email).IsRequired().HasMaxLength(256);
            entity.Property(s => s.PhoneNumber).IsRequired().HasMaxLength(20);
            entity.Property(s => s.PasswordHash).IsRequired().HasMaxLength(512);
            entity.Property(s => s.Role).HasConversion<string>().HasMaxLength(50);

            // Unique constraint on Email
            entity.HasIndex(s => s.Email).IsUnique();
        });

        // --- Feedback Configuration ---
        // Configures customer comments and rating links.
        modelBuilder.Entity<Feedback>(entity =>
        {
            entity.HasKey(f => f.FeedbackId);
            entity.Property(f => f.Comments).IsRequired().HasMaxLength(1000);
            entity.Property(f => f.Rating).IsRequired();

            entity.HasOne(f => f.Guest)
                .WithMany()
                .HasForeignKey(f => f.GuestId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasOne(f => f.Reservation)
                .WithMany()
                .HasForeignKey(f => f.ReservationId)
                .OnDelete(DeleteBehavior.SetNull);
        });
    }

}
