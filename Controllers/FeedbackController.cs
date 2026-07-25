using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using CogStayMVC.DTOs;
using CogStayMVC.Services.Interfaces;

namespace CogStayMVC.Controllers;

/// <summary>
/// Handles customer feedback operations, allowing guests to submit reviews,
/// and hotel managers to view and moderate submitted feedback.
/// </summary>
public class FeedbackController : Controller
{
    private readonly IFeedbackService _feedbackService;

    /// <summary>
    /// Initializes a new instance of the <see cref="FeedbackController"/> class.
    /// </summary>
    /// <param name="feedbackService">Service for feedback submission and moderation logic.</param>
    public FeedbackController(IFeedbackService feedbackService)
    {
        _feedbackService = feedbackService;
    }

    /// <summary>
    /// Displays a list of all guest feedbacks for management review.
    /// Restricted to users with the 'Manager' staff role.
    /// </summary>
    /// <returns>Feedbacks view showing the feedback list, or redirection if unauthorized.</returns>
    [HttpGet]
    public async Task<IActionResult> Index()
    {
        string? staffRole = HttpContext.Session.GetString("StaffRole");
        if (string.IsNullOrEmpty(staffRole) || staffRole != "Manager")
        {
            if (string.IsNullOrEmpty(staffRole)) return RedirectToAction("Login", "Staff");
            return RedirectToAction("Dashboard", "Staff", new { role = staffRole });
        }

        ViewData["Role"] = "Manager";
        var feedbacks = await _feedbackService.GetAllFeedbacksAsync();
        return View(feedbacks);
    }

    /// <summary>
    /// Renders the feedback creation form for a specific reservation.
    /// Restricted to authenticated Guests.
    /// </summary>
    /// <param name="reservationId">The ID of the reservation being reviewed.</param>
    /// <returns>Create feedback form view.</returns>
    [HttpGet]
    public IActionResult Create(int? reservationId)
    {
        int? guestId = HttpContext.Session.GetInt32("GuestId");
        if (!guestId.HasValue) return RedirectToAction("Login", "Guest");

        ViewData["Role"] = "Guest";
        var dto = new CreateFeedbackDTO
        {
            GuestId = guestId.Value,
            ReservationId = reservationId,
            Rating = 5
        };
        return View(dto);
    }

    /// <summary>
    /// Processes the submission of feedback from a guest.
    /// </summary>
    /// <param name="dto">The feedback transfer object containing rating and comments.</param>
    /// <returns>Redirects to Guest Dashboard on success, or stays on form with validation errors.</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateFeedbackDTO dto)
    {
        int? guestId = HttpContext.Session.GetInt32("GuestId");
        if (!guestId.HasValue) return RedirectToAction("Login", "Guest");
        dto.GuestId = guestId.Value;

        ViewData["Role"] = "Guest";
        if (!ModelState.IsValid) return View(dto);

        try
        {
            await _feedbackService.SubmitFeedbackAsync(dto);
            TempData["Success"] = "Thank you for your feedback!";
            return RedirectToAction("Dashboard", "Guest");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError(string.Empty, ex.Message);
            return View(dto);
        }
    }

    /// <summary>
    /// Deletes a specific feedback entry by ID. Restricted to Manager staff.
    /// </summary>
    /// <param name="id">The ID of the feedback to delete.</param>
    /// <returns>Redirects back to Feedbacks Index view.</returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        string? staffRole = HttpContext.Session.GetString("StaffRole");
        if (string.IsNullOrEmpty(staffRole) || staffRole != "Manager")
        {
            if (string.IsNullOrEmpty(staffRole)) return RedirectToAction("Login", "Staff");
            return RedirectToAction("Dashboard", "Staff", new { role = staffRole });
        }

        try
        {
            await _feedbackService.DeleteFeedbackAsync(id);
            TempData["Success"] = "Feedback removed.";
        }
        catch (Exception ex)
        {
            TempData["Error"] = ex.Message;
        }
        return RedirectToAction(nameof(Index), new { role = "Manager" });
    }
}
