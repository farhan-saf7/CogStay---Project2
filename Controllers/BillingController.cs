using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CogStayMVC.DTOs;
using CogStayMVC.Services.Interfaces;

namespace CogStayMVC.Controllers;

/// <summary>
/// Controller handling billing-related operations, including bill generation,
/// payment processing, and bill history queries for hotel front desk and managers.
/// </summary>
public class BillingController : Controller
{
    private readonly IBillingService _billingService;
    private readonly ICheckInService _checkInService;

    /// <summary>
    /// Initializes a new instance of the <see cref="BillingController"/> class.
    /// </summary>
    /// <param name="billingService">Service handling billing generation and payments.</param>
    /// <param name="checkInService">Service managing check-in stays to calculate checkout fees.</param>
    public BillingController(
        IBillingService billingService,
        ICheckInService checkInService)
    {
        _billingService = billingService;
        _checkInService = checkInService;
    }

    /// <summary>
    /// Displays all existing billing records. Restricted to FrontDesk or Manager staff roles.
    /// </summary>
    /// <returns>Billing Index View displaying all bills.</returns>
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        string? staffRole = HttpContext.Session.GetString("StaffRole");
        if (string.IsNullOrEmpty(staffRole) || (staffRole != "FrontDesk" && staffRole != "Manager"))
        {
            if (string.IsNullOrEmpty(staffRole)) return RedirectToAction("Login", "Staff");
            return RedirectToAction("Dashboard", "Staff", new { role = staffRole });
        }

        ViewData["Role"] = staffRole;
        var bills = await _billingService.GetAllBillsAsync();
        return View(bills);
    }

    /// <summary>
    /// Renders the form to manually generate a bill for an active stay record.
    /// </summary>
    /// <param name="stayId">Optional ID of the stay record to generate the bill for.</param>
    /// <returns>Bill generation View.</returns>
    [HttpGet]
    public async Task<IActionResult> Create(int? stayId)
    {
        string? staffRole = HttpContext.Session.GetString("StaffRole");
        if (string.IsNullOrEmpty(staffRole) || (staffRole != "FrontDesk" && staffRole != "Manager"))
        {
            if (string.IsNullOrEmpty(staffRole)) return RedirectToAction("Login", "Staff");
            return RedirectToAction("Dashboard", "Staff", new { role = staffRole });
        }

        ViewData["Role"] = staffRole;
        ViewBag.ActiveStays = await _checkInService.GetAllStaysAsync();
        return View(new CreateBillDTO { StayId = stayId ?? 0 });
    }

    /// <summary>
    /// Processes the submission for generating a bill.
    /// </summary>
    /// <param name="dto">Data transfer object containing stay reference and bill parameters.</param>
    /// <returns>Redirects to Billing Index on success, or returns form on failure.</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateBillDTO dto)
    {
        string? staffRole = HttpContext.Session.GetString("StaffRole");
        if (string.IsNullOrEmpty(staffRole) || (staffRole != "FrontDesk" && staffRole != "Manager"))
        {
            if (string.IsNullOrEmpty(staffRole)) return RedirectToAction("Login", "Staff");
            return RedirectToAction("Dashboard", "Staff", new { role = staffRole });
        }

        ViewData["Role"] = staffRole;
        if (!ModelState.IsValid)
        {
            ViewBag.ActiveStays = await _checkInService.GetAllStaysAsync();
            return View(dto);
        }

        try
        {
            if (dto.TotalAmount > 0)
            {
                await _billingService.CreateBillAsync(dto);
            }
            else
            {
                await _billingService.GenerateBillForStayAsync(dto.StayId, dto.Remarks);
            }

            TempData["Success"] = "Bill generated successfully.";
            return RedirectToAction(nameof(Index), new { role = staffRole });
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            ViewBag.ActiveStays = await _checkInService.GetAllStaysAsync();
            return View(dto);
        }
    }

    /// <summary>
    /// Renders the payment processing form for a specific active bill or stay record.
    /// </summary>
    /// <param name="stayId">Optional stay ID reference to find or generate the bill.</param>
    /// <param name="billId">Optional direct bill ID reference.</param>
    /// <returns>Payment processing View containing invoice details.</returns>
    [HttpGet]
    public async Task<IActionResult> Payment(int? stayId, int? billId)
    {
        string? staffRole = HttpContext.Session.GetString("StaffRole");
        if (string.IsNullOrEmpty(staffRole) || (staffRole != "FrontDesk" && staffRole != "Manager"))
        {
            if (string.IsNullOrEmpty(staffRole)) return RedirectToAction("Login", "Staff");
            return RedirectToAction("Dashboard", "Staff", new { role = staffRole });
        }

        ViewData["Role"] = staffRole;
        BillingResponseDTO? bill = null;

        if (billId.HasValue)
        {
            bill = await _billingService.GetBillByIdAsync(billId.Value);
        }
        else if (stayId.HasValue)
        {
            bill = await _billingService.GetBillByStayIdAsync(stayId.Value) 
                   ?? await _billingService.GenerateBillForStayAsync(stayId.Value);
        }

        if (bill == null)
        {
            TempData["Error"] = "No billing record found for this stay.";
            return RedirectToAction(nameof(Index), new { role = staffRole });
        }

        var dto = new ProcessPaymentDTO
        {
            BillId = bill.BillId,
            Remarks = "Payment accepted at Front Desk"
        };

        ViewBag.Bill = bill;
        return View(dto);
    }

    /// <summary>
    /// Processes the submission of payment, completing the checkout flow.
    /// </summary>
    /// <param name="dto">The payment processing DTO containing remarks and bill references.</param>
    /// <returns>Redirects to Billing History on success.</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Payment(ProcessPaymentDTO dto)
    {
        string? staffRole = HttpContext.Session.GetString("StaffRole");
        if (string.IsNullOrEmpty(staffRole) || (staffRole != "FrontDesk" && staffRole != "Manager"))
        {
            if (string.IsNullOrEmpty(staffRole)) return RedirectToAction("Login", "Staff");
            return RedirectToAction("Dashboard", "Staff", new { role = staffRole });
        }

        ViewData["Role"] = staffRole;
        if (!ModelState.IsValid)
        {
            ViewBag.Bill = await _billingService.GetBillByIdAsync(dto.BillId);
            return View(dto);
        }

        try
        {
            await _billingService.ProcessPaymentAsync(dto);
            TempData["Success"] = "Payment accepted and checkout completed! Housekeeping cleaning request automatically generated.";
            return RedirectToAction(nameof(History), new { role = staffRole });
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            ViewBag.Bill = await _billingService.GetBillByIdAsync(dto.BillId);
            return View(dto);
        }
    }

    /// <summary>
    /// Displays a complete history of all billing records.
    /// </summary>
    /// <returns>Billing History View.</returns>
    [HttpGet]
    public async Task<IActionResult> History()
    {
        string? staffRole = HttpContext.Session.GetString("StaffRole");
        if (string.IsNullOrEmpty(staffRole) || (staffRole != "FrontDesk" && staffRole != "Manager"))
        {
            if (string.IsNullOrEmpty(staffRole)) return RedirectToAction("Login", "Staff");
            return RedirectToAction("Dashboard", "Staff", new { role = staffRole });
        }

        ViewData["Role"] = staffRole;
        var bills = await _billingService.GetAllBillsAsync();
        return View(bills);
    }

    /// <summary>
    /// Deletes a specific billing record. Restricted to authorized FrontDesk/Manager staff.
    /// </summary>
    /// <param name="id">The ID of the bill to delete.</param>
    /// <returns>Redirects back to Billing Index view.</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        string? staffRole = HttpContext.Session.GetString("StaffRole");
        if (string.IsNullOrEmpty(staffRole) || (staffRole != "FrontDesk" && staffRole != "Manager"))
        {
            if (string.IsNullOrEmpty(staffRole)) return RedirectToAction("Login", "Staff");
            return RedirectToAction("Dashboard", "Staff", new { role = staffRole });
        }

        ViewData["Role"] = staffRole;
        try
        {
            await _billingService.DeleteBillAsync(id);
            TempData["Success"] = "Bill record deleted.";
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
        }
        return RedirectToAction(nameof(Index), new { role = staffRole });
    }
}
