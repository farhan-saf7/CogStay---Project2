using System;
using System.Collections.Generic;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CogStayMVC.Enums;
using CogStayMVC.Models;
using CogStayMVC.Services.Interfaces;

namespace CogStayMVC.Controllers;

public class StaffController : Controller
{
    private readonly IStaffService _staffService;
    private readonly IRoomService _roomService;
    private readonly IReservationService _reservationService;
    private readonly IStayRecordService _stayRecordService;
    private readonly IHousekeepingService _housekeepingService;
    private readonly IBillingService _billingService;

    public StaffController(
        IStaffService staffService,
        IRoomService roomService,
        IReservationService reservationService,
        IStayRecordService stayRecordService,
        IHousekeepingService housekeepingService,
        IBillingService billingService)
    {
        _staffService = staffService;
        _roomService = roomService;
        _reservationService = reservationService;
        _stayRecordService = stayRecordService;
        _housekeepingService = housekeepingService;
        _billingService = billingService;
    }

    [HttpGet]
    public IActionResult Login()
    {
        if (User.Identity?.IsAuthenticated == true && !User.IsInRole("Guest"))
        {
            return RedirectToAction("Dashboard");
        }
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(string email, string password)
    {
        var staff = await _staffService.AuthenticateAsync(email, password);
        if (staff == null)
        {
            ModelState.AddModelError("", "Invalid corporate email or security password.");
            return View();
        }

        if (!staff.IsActive)
        {
            ModelState.AddModelError("", "Your account has been deactivated. Please contact administration.");
            return View();
        }

        var claims = new List<Claim>
        {
            new Claim(ClaimTypes.Name, staff.FullName),
            new Claim(ClaimTypes.Email, staff.Email),
            new Claim(ClaimTypes.Role, staff.Role.ToString()),
            new Claim("UserId", staff.StaffId.ToString())
        };
        var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
        var principal = new ClaimsPrincipal(identity);
        await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

        return RedirectToAction("Dashboard", new { role = staff.Role.ToString() });
    }

    [Authorize(Roles = "Admin,Manager,FrontDesk,Housekeeping")]
    [HttpGet]
    public async Task<IActionResult> Dashboard(string? role)
    {
        // If role parameter is null or empty, get from Claims
        if (string.IsNullOrEmpty(role))
        {
            role = User.FindFirst(ClaimTypes.Role)?.Value ?? "Admin";
        }
        ViewData["Role"] = role;

        // Fetch shared dashboard metrics
        var allRooms = await _roomService.GetAllRoomsAsync();
        var allReservations = await _reservationService.GetAllReservationsAsync();
        var activeStays = await _stayRecordService.GetActiveStaysAsync();
        var allBillings = await _billingService.GetAllBillingsAsync();
        var allTasks = await _housekeepingService.GetAllTasksAsync();

        // 1. FrontDesk metrics
        int frontDeskTodayReservationsCount = 0;
        var expectedArrivals = new List<Reservation>();
        foreach (var r in allReservations)
        {
            if (r.CheckInDate.Date == DateTime.Today && r.ReservationStatus == ReservationStatus.Booked)
            {
                frontDeskTodayReservationsCount++;
                if (r.StayRecord == null) expectedArrivals.Add(r);
            }
        }

        int frontDeskAvailableRoomsCount = 0;
        int occupiedRoomsCount = 0;
        foreach (var r in allRooms)
        {
            if (r.Status == RoomStatus.Available) frontDeskAvailableRoomsCount++;
            else if (r.Status == RoomStatus.Occupied) occupiedRoomsCount++;
        }

        ViewData["FrontDesk_TodayReservationsCount"] = frontDeskTodayReservationsCount;
        ViewData["FrontDesk_ActiveStaysCount"] = System.Linq.Enumerable.Count(activeStays);
        ViewData["FrontDesk_AvailableRoomsCount"] = frontDeskAvailableRoomsCount;
        ViewData["FrontDesk_ExpectedArrivals"] = expectedArrivals;

        // 2. Housekeeping metrics
        int housekeepingPendingCount = 0;
        int housekeepingInProgressCount = 0;
        int housekeepingCompletedCount = 0;
        var activeTasks = new List<HousekeepingTask>();
        foreach (var t in allTasks)
        {
            if (t.TaskStatus == CogStayMVC.Enums.TaskStatus.Pending)
            {
                housekeepingPendingCount++;
                activeTasks.Add(t);
            }
            else if (t.TaskStatus == CogStayMVC.Enums.TaskStatus.InProgress)
            {
                housekeepingInProgressCount++;
                activeTasks.Add(t);
            }
            else if (t.TaskStatus == CogStayMVC.Enums.TaskStatus.Completed)
            {
                housekeepingCompletedCount++;
            }
        }

        ViewData["Housekeeping_PendingTasksCount"] = housekeepingPendingCount;
        ViewData["Housekeeping_InProgressTasksCount"] = housekeepingInProgressCount;
        ViewData["Housekeeping_CompletedTasksCount"] = housekeepingCompletedCount;
        ViewData["Housekeeping_ActiveTasks"] = activeTasks;

        // 3. Manager/Admin metrics
        decimal pendingPaymentsAmount = 0;
        int pendingPaymentsCount = 0;
        foreach (var b in allBillings)
        {
            if (b.PaymentStatus == PaymentStatus.Pending)
            {
                pendingPaymentsAmount += b.TotalAmount;
                pendingPaymentsCount++;
            }
        }

        ViewData["Manager_AvailableRoomsCount"] = frontDeskAvailableRoomsCount;
        ViewData["Manager_ActiveReservationsCount"] = System.Linq.Enumerable.Count(allReservations);
        ViewData["Manager_ActiveStaysCount"] = System.Linq.Enumerable.Count(activeStays);
        ViewData["Manager_PendingPaymentsAmount"] = pendingPaymentsAmount;
        ViewData["Manager_TotalRoomsCount"] = System.Linq.Enumerable.Count(allRooms);
        ViewData["Manager_OccupiedRoomsCount"] = occupiedRoomsCount;
        ViewData["Manager_OutstandingInvoicesCount"] = pendingPaymentsCount;

        int totalTasks = housekeepingPendingCount + housekeepingInProgressCount + housekeepingCompletedCount;
        double housekeepingCompletionPercentage = totalTasks > 0 ? (double)housekeepingCompletedCount / totalTasks * 100 : 100;
        ViewData["Manager_HousekeepingCompletionPercentage"] = Math.Round(housekeepingCompletionPercentage, 1);

        ViewData["Admin_TotalRoomsCount"] = System.Linq.Enumerable.Count(allRooms);
        ViewData["Admin_ActiveReservationsCount"] = System.Linq.Enumerable.Count(allReservations);
        ViewData["Admin_ActiveStaysCount"] = System.Linq.Enumerable.Count(activeStays);
        ViewData["Admin_PendingPaymentsAmount"] = pendingPaymentsAmount;

        return View();
    }

    // --- Admin-Only Staff CRUD Portal ---

    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var staffList = await _staffService.GetAllStaffAsync();
        return View(staffList);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    public IActionResult Create()
    {
        return View();
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Create(string fullName, string email, string phoneNumber, StaffRole role, string password)
    {
        var staff = new Staff
        {
            FullName = fullName,
            Email = email,
            PhoneNumber = phoneNumber,
            Role = role,
            IsActive = true
        };

        try
        {
            await _staffService.CreateStaffAsync(staff, password);
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            return View();
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var staff = await _staffService.GetByIdAsync(id);
        if (staff == null) return NotFound();
        return View(staff);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> Edit(int id, string fullName, string email, string phoneNumber, StaffRole role, bool isActive, string? newPassword)
    {
        var staff = new Staff
        {
            StaffId = id,
            FullName = fullName,
            Email = email,
            PhoneNumber = phoneNumber,
            Role = role,
            IsActive = isActive,
            PasswordHash = newPassword ?? ""
        };

        try
        {
            await _staffService.UpdateStaffAsync(staff);
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            var dbStaff = await _staffService.GetByIdAsync(id);
            return View(dbStaff);
        }
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IActionResult> Details(int id)
    {
        var staff = await _staffService.GetByIdAsync(id);
        if (staff == null) return NotFound();
        return View(staff);
    }

    [Authorize(Roles = "Admin")]
    [HttpGet]
    public async Task<IActionResult> Delete(int id)
    {
        var staff = await _staffService.GetByIdAsync(id);
        if (staff == null) return NotFound();
        return View(staff);
    }

    [Authorize(Roles = "Admin")]
    [HttpPost]
    public async Task<IActionResult> DeleteConfirmed(int staffId)
    {
        try
        {
            await _staffService.DeleteStaffAsync(staffId);
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            var staff = await _staffService.GetByIdAsync(staffId);
            return View("Delete", staff);
        }
    }

    [HttpGet]
    public async Task<IActionResult> Logout()
    {
        await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
        return RedirectToAction("Index", "Home");
    }
}
