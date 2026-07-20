using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CogStayMVC.Enums;
using CogStayMVC.Models;
using CogStayMVC.Services.Interfaces;

namespace CogStayMVC.Controllers;

[Authorize(Roles = "Admin,Manager,FrontDesk")]
public class CheckInController : Controller
{
    private readonly IStayRecordService _stayRecordService;
    private readonly IReservationService _reservationService;

    public CheckInController(IStayRecordService stayRecordService, IReservationService reservationService)
    {
        _stayRecordService = stayRecordService;
        _reservationService = reservationService;
    }

    [HttpGet]
    public async Task<IActionResult> ActiveStays()
    {
        var list = await _stayRecordService.GetActiveStaysAsync();
        return View(list);
    }

    [HttpGet]
    public async Task<IActionResult> CheckIn(int? reservationId)
    {
        ViewData["SelectedReservationId"] = reservationId;
        ViewData["PendingReservations"] = await GetPendingReservations();
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CheckIn(int reservationId, string? dummyString) // dummy param to differentiate
    {
        try
        {
            await _stayRecordService.CheckInAsync(reservationId);
            return RedirectToAction("ActiveStays");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            ViewData["PendingReservations"] = await GetPendingReservations();
            return View();
        }
    }

    [HttpGet]
    public async Task<IActionResult> CheckOut(int? stayId)
    {
        ViewData["SelectedStayId"] = stayId;
        ViewData["ActiveStays"] = await _stayRecordService.GetActiveStaysAsync();
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> CheckOut(int stayId, string? dummyString) // dummy param to differentiate
    {
        try
        {
            var stay = await _stayRecordService.CheckOutAsync(stayId);
            // Settle to Billing view for payment
            return RedirectToAction("Index", "Billing");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            ViewData["ActiveStays"] = await _stayRecordService.GetActiveStaysAsync();
            return View();
        }
    }

    private async Task<IEnumerable<Reservation>> GetPendingReservations()
    {
        var all = await _reservationService.GetAllReservationsAsync();
        var pending = new List<Reservation>();
        foreach (var r in all)
        {
            if (r.ReservationStatus == ReservationStatus.Booked && r.StayRecord == null)
            {
                pending.Add(r);
            }
        }
        return pending;
    }
}
