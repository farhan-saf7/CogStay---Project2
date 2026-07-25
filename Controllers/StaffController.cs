using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CogStayMVC.DTOs;
using CogStayMVC.Enums;
using CogStayMVC.Services.Interfaces;
using TaskStatus = CogStayMVC.Enums.TaskStatus;

namespace CogStayMVC.Controllers;

/// <summary>
/// Controller for internal staff management and dashboard controls.
/// Handles login, admin registration, dashboard summaries, staff directory CRUD, check-in tracking, and sign out.
/// </summary>
public class StaffController : Controller
{
    private readonly IStaffService _staffService;
    private readonly IRoomService _roomService;
    private readonly IReservationService _reservationService;
    private readonly ICheckInService _checkInService;
    private readonly IHousekeepingService _housekeepingService;
    private readonly IBillingService _billingService;

    /// <summary>
    /// Initializes a new instance of the <see cref="StaffController"/> class.
    /// </summary>
    /// <param name="staffService">Service managing staff records.</param>
    /// <param name="roomService">Service managing room records.</param>
    /// <param name="reservationService">Service managing reservations.</param>
    /// <param name="checkInService">Service managing check-in stay records.</param>
    /// <param name="housekeepingService">Service managing housekeeping tasks.</param>
    /// <param name="billingService">Service managing bills.</param>
    public StaffController(
        IStaffService staffService,
        IRoomService roomService,
        IReservationService reservationService,
        ICheckInService checkInService,
        IHousekeepingService housekeepingService,
        IBillingService billingService)
    {
        _staffService = staffService;
        _roomService = roomService;
        _reservationService = reservationService;
        _checkInService = checkInService;
        _housekeepingService = housekeepingService;
        _billingService = billingService;
    }

    /// <summary>
    /// Renders the staff login form view.
    /// </summary>
    /// <returns>Staff login View.</returns>
    [HttpGet]
    public IActionResult Login() => View(new StaffLoginDTO());

    /// <summary>
    /// Validates staff credentials and initiates their dashboard session.
    /// </summary>
    /// <param name="dto">The staff login details DTO.</param>
    /// <returns>Redirects to staff Dashboard on success.</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(StaffLoginDTO dto)
    {
        if (!ModelState.IsValid) return View(dto);

        var staff = await _staffService.ValidateStaffLoginAsync(dto);
        if (staff == null)
        {
            ModelState.AddModelError(string.Empty, "Invalid credentials or unauthorized role access.");
            return View(dto);
        }

        HttpContext.Session.SetInt32("StaffId", staff.StaffId);
        HttpContext.Session.SetString("StaffName", staff.FullName);
        HttpContext.Session.SetString("StaffRole", staff.Role.ToString());

        return RedirectToAction(nameof(Dashboard), new { role = staff.Role.ToString() });
    }

    /// <summary>
    /// Renders the admin registration form (accessible only by existing Admins or if no admin exists).
    /// </summary>
    /// <returns>Admin registration View.</returns>
    [HttpGet]
    public IActionResult RegisterAdmin()
    {
        string? staffRole = HttpContext.Session.GetString("StaffRole");
        if (!string.IsNullOrEmpty(staffRole) && staffRole != "Admin")
        {
            return RedirectToAction("Dashboard", new { role = staffRole });
        }

        return View(new CreateStaffDTO { Role = StaffRole.Admin });
    }

    /// <summary>
    /// Processes admin registration form submissions.
    /// </summary>
    /// <param name="dto">The administrative staff registration details DTO.</param>
    /// <returns>Redirects to admin Dashboard on success.</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RegisterAdmin(CreateStaffDTO dto)
    {
        string? staffRole = HttpContext.Session.GetString("StaffRole");
        if (!string.IsNullOrEmpty(staffRole) && staffRole != "Admin")
        {
            return RedirectToAction("Dashboard", new { role = staffRole });
        }

        // Force role to Admin
        dto.Role = StaffRole.Admin;

        if (!ModelState.IsValid) return View(dto);

        try
        {
            var newAdmin = await _staffService.CreateStaffAsync(dto);

            HttpContext.Session.SetInt32("StaffId", newAdmin.StaffId);
            HttpContext.Session.SetString("StaffName", newAdmin.FullName);
            HttpContext.Session.SetString("StaffRole", newAdmin.Role.ToString());

            TempData["Success"] = "Admin account registered successfully!";
            return RedirectToAction(nameof(Dashboard), new { role = newAdmin.Role.ToString() });
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(dto);
        }
    }

    /// <summary>
    /// Renders the centralized Dashboard gathering metrics from rooms, housekeeping, stays, and billings.
    /// </summary>
    /// <param name="role">Role of the staff member viewing the dashboard.</param>
    /// <returns>Dashboard View with loaded metrics.</returns>
    [HttpGet]
    public async Task<IActionResult> Dashboard(string role = "Admin")
    {
        int? staffId = HttpContext.Session.GetInt32("StaffId");
        if (!staffId.HasValue) return RedirectToAction(nameof(Login));

        string? sessionRole = HttpContext.Session.GetString("StaffRole");
        if (string.IsNullOrEmpty(sessionRole)) return RedirectToAction(nameof(Login));

        ViewData["Role"] = sessionRole;
        ViewBag.StaffName = HttpContext.Session.GetString("StaffName") ?? "Staff Member";

        var rooms = await _roomService.GetAllRoomsAsync();
        var availableRooms = rooms.Where(r => r.Status == RoomStatus.Available).ToList();
        var reservations = await _reservationService.GetAllReservationsAsync();
        var activeStays = await _checkInService.GetAllStaysAsync();
        var currentActiveStays = activeStays.Where(s => !s.ActualCheckOut.HasValue).ToList();
        var housekeepingTasks = await _housekeepingService.GetAllTasksAsync();
        var bills = await _billingService.GetAllBillsAsync();
        var pendingBills = bills.Where(b => b.PaymentStatus == PaymentStatus.Pending).ToList();
        var staffList = await _staffService.GetAllStaffAsync();

        ViewBag.TotalRoomsCount = rooms.Count();
        ViewBag.TotalStaffCount = staffList.Count();
        ViewBag.AvailableRoomsCount = availableRooms.Count();
        ViewBag.ReservationsCount = reservations.Count(r => r.ReservationStatus == ReservationStatus.Booked);
        ViewBag.ActiveStaysCount = currentActiveStays.Count();
        ViewBag.PendingTasksCount = housekeepingTasks.Count(t => t.TaskStatus == TaskStatus.Pending);
        ViewBag.InProgressTasksCount = housekeepingTasks.Count(t => t.TaskStatus == TaskStatus.InProgress);
        ViewBag.CompletedTasksCount = housekeepingTasks.Count(t => t.TaskStatus == TaskStatus.Completed);
        ViewBag.PendingPaymentAmount = pendingBills.Sum(b => b.TotalAmount);
        ViewBag.PendingBillsCount = pendingBills.Count();

        ViewBag.ArrivalsList = reservations.Where(r => r.ReservationStatus == ReservationStatus.Booked).Take(5).ToList();
        ViewBag.HousekeepingTasksList = housekeepingTasks.Where(t => t.TaskStatus != TaskStatus.Completed).Take(5).ToList();

        return View();
    }

    /// <summary>
    /// Lists all staff members. Restricted to Admin.
    /// </summary>
    /// <returns>Index view showing staff directory.</returns>
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        string? staffRole = HttpContext.Session.GetString("StaffRole");
        if (string.IsNullOrEmpty(staffRole) || staffRole != "Admin")
        {
            if (string.IsNullOrEmpty(staffRole)) return RedirectToAction("Login");
            return RedirectToAction("Dashboard", new { role = staffRole });
        }

        ViewData["Role"] = "Admin";
        var staffList = await _staffService.GetAllStaffAsync();
        return View(staffList);
    }

    /// <summary>
    /// Displays details of a specific staff member.
    /// </summary>
    /// <param name="id">Staff identifier.</param>
    /// <returns>Details View, or 404.</returns>
    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        string? staffRole = HttpContext.Session.GetString("StaffRole");
        if (string.IsNullOrEmpty(staffRole) || staffRole != "Admin")
        {
            if (string.IsNullOrEmpty(staffRole)) return RedirectToAction("Login");
            return RedirectToAction("Dashboard", new { role = staffRole });
        }

        ViewData["Role"] = "Admin";
        var staff = await _staffService.GetStaffByIdAsync(id);
        if (staff == null) return NotFound();
        return View(staff);
    }

    /// <summary>
    /// Renders the staff registration form (for Admin use to add employees).
    /// </summary>
    /// <returns>Create staff form View.</returns>
    [HttpGet]
    public IActionResult Create()
    {
        string? staffRole = HttpContext.Session.GetString("StaffRole");
        if (string.IsNullOrEmpty(staffRole) || staffRole != "Admin")
        {
            if (string.IsNullOrEmpty(staffRole)) return RedirectToAction("Login");
            return RedirectToAction("Dashboard", new { role = staffRole });
        }

        ViewData["Role"] = "Admin";
        return View(new CreateStaffDTO());
    }

    /// <summary>
    /// Processes employee creation submissions.
    /// </summary>
    /// <param name="dto">The new staff DTO parameters.</param>
    /// <returns>Redirects to staff list Index on success.</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateStaffDTO dto)
    {
        string? staffRole = HttpContext.Session.GetString("StaffRole");
        if (string.IsNullOrEmpty(staffRole) || staffRole != "Admin")
        {
            if (string.IsNullOrEmpty(staffRole)) return RedirectToAction("Login");
            return RedirectToAction("Dashboard", new { role = staffRole });
        }

        ViewData["Role"] = "Admin";
        if (!ModelState.IsValid) return View(dto);

        try
        {
            await _staffService.CreateStaffAsync(dto);
            TempData["Success"] = "Staff member created successfully!";
            return RedirectToAction(nameof(Index), new { role = "Admin" });
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(dto);
        }
    }

    /// <summary>
    /// Renders the edit form for updating a staff member's details.
    /// </summary>
    /// <param name="id">Staff identifier.</param>
    /// <returns>Edit form View, or 404.</returns>
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        string? staffRole = HttpContext.Session.GetString("StaffRole");
        if (string.IsNullOrEmpty(staffRole) || staffRole != "Admin")
        {
            if (string.IsNullOrEmpty(staffRole)) return RedirectToAction("Login");
            return RedirectToAction("Dashboard", new { role = staffRole });
        }

        ViewData["Role"] = "Admin";
        var staff = await _staffService.GetStaffByIdAsync(id);
        if (staff == null) return NotFound();

        var dto = new UpdateStaffDTO
        {
            StaffId = staff.StaffId,
            FullName = staff.FullName,
            Email = staff.Email,
            PhoneNumber = staff.PhoneNumber,
            Role = staff.Role,
            IsActive = staff.IsActive
        };
        return View(dto);
    }

    /// <summary>
    /// Processes staff detail updates.
    /// </summary>
    /// <param name="id">Staff identifier.</param>
    /// <param name="dto">Updated details DTO.</param>
    /// <returns>Redirects back to staff Index view.</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, UpdateStaffDTO dto)
    {
        string? staffRole = HttpContext.Session.GetString("StaffRole");
        if (string.IsNullOrEmpty(staffRole) || staffRole != "Admin")
        {
            if (string.IsNullOrEmpty(staffRole)) return RedirectToAction("Login");
            return RedirectToAction("Dashboard", new { role = staffRole });
        }

        ViewData["Role"] = "Admin";
        if (id != dto.StaffId) return BadRequest();
        if (!ModelState.IsValid) return View(dto);

        try
        {
            await _staffService.UpdateStaffAsync(dto);
            TempData["Success"] = "Staff updated successfully!";
            return RedirectToAction(nameof(Index), new { role = "Admin" });
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(dto);
        }
    }

    /// <summary>
    /// Renders the deletion confirmation form for a staff member.
    /// </summary>
    /// <param name="id">Staff identifier.</param>
    /// <returns>Delete confirmation View, or 404.</returns>
    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        string? staffRole = HttpContext.Session.GetString("StaffRole");
        if (string.IsNullOrEmpty(staffRole) || staffRole != "Admin")
        {
            if (string.IsNullOrEmpty(staffRole)) return RedirectToAction("Login");
            return RedirectToAction("Dashboard", new { role = staffRole });
        }

        ViewData["Role"] = "Admin";
        var staff = await _staffService.GetStaffByIdAsync(id);
        if (staff == null) return NotFound();
        return View(staff);
    }

    /// <summary>
    /// Processes staff deletions.
    /// </summary>
    /// <param name="id">Staff identifier.</param>
    /// <returns>Redirects back to staff Index view.</returns>
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        string? staffRole = HttpContext.Session.GetString("StaffRole");
        if (string.IsNullOrEmpty(staffRole) || staffRole != "Admin")
        {
            if (string.IsNullOrEmpty(staffRole)) return RedirectToAction("Login");
            return RedirectToAction("Dashboard", new { role = staffRole });
        }

        ViewData["Role"] = "Admin";
        try
        {
            await _staffService.DeleteStaffAsync(id);
            TempData["Success"] = "Staff member deleted successfully!";
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
        }
        return RedirectToAction(nameof(Index), new { role = "Admin" });
    }

    /// <summary>
    /// Renders a list of all stay records to track active check-ins. Restricted to Admin.
    /// </summary>
    /// <returns>CheckInStatus View.</returns>
    [HttpGet]
    public async Task<IActionResult> CheckInStatus()
    {
        string? staffRole = HttpContext.Session.GetString("StaffRole");
        if (string.IsNullOrEmpty(staffRole) || staffRole != "Admin")
        {
            if (string.IsNullOrEmpty(staffRole)) return RedirectToAction("Login", "Staff");
            return RedirectToAction("Dashboard", "Staff", new { role = staffRole });
        }

        ViewData["Role"] = "Admin";
        var stays = await _checkInService.GetAllStaysAsync();
        return View(stays);
    }

    /// <summary>
    /// Submits a request to checkout an active stay.
    /// </summary>
    /// <param name="stayId">Stay record ID.</param>
    /// <returns>Redirects back to CheckInStatus View.</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> RequestCheckOut(int stayId)
    {
        string? staffRole = HttpContext.Session.GetString("StaffRole");
        if (string.IsNullOrEmpty(staffRole) || staffRole != "Admin")
        {
            if (string.IsNullOrEmpty(staffRole)) return RedirectToAction("Login", "Staff");
            return RedirectToAction("Dashboard", "Staff", new { role = staffRole });
        }

        ViewData["Role"] = "Admin";
        await _checkInService.RequestCheckOutAsync(stayId);
        TempData["Success"] = "Checkout requested successfully.";
        return RedirectToAction(nameof(CheckInStatus));
    }

    /// <summary>
    /// Logs out staff members and clears their active session context.
    /// </summary>
    /// <returns>Redirects to staff Login page.</returns>
    [HttpGet]
    public IActionResult Logout()
    {
        HttpContext.Session.Clear();
        return RedirectToAction(nameof(Login));
    }
}
