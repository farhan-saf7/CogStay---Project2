using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using CogStayMVC.Enums;
using CogStayMVC.Models;
using CogStayMVC.Services.Interfaces;

namespace CogStayMVC.Controllers;

[Authorize(Roles = "Admin,Manager")]
public class BillingController : Controller
{
    private readonly IBillingService _billingService;
    private readonly IStayRecordService _stayRecordService;

    public BillingController(IBillingService billingService, IStayRecordService stayRecordService)
    {
        _billingService = billingService;
        _stayRecordService = stayRecordService;
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var list = await _billingService.GetAllBillingsAsync();
        // Filter down to pending bills for Index
        var pending = new System.Collections.Generic.List<Billing>();
        foreach (var b in list)
        {
            if (b.PaymentStatus == PaymentStatus.Pending)
            {
                pending.Add(b);
            }
        }
        return View(pending);
    }

    [HttpGet]
    public async Task<IActionResult> History()
    {
        var list = await _billingService.GetAllBillingsAsync();
        // Filter down to paid bills for History
        var paid = new System.Collections.Generic.List<Billing>();
        foreach (var b in list)
        {
            if (b.PaymentStatus == PaymentStatus.Paid)
            {
                paid.Add(b);
            }
        }
        return View(paid);
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        ViewData["ActiveStays"] = await _stayRecordService.GetAllStaysAsync();
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Create(int stayId)
    {
        try
        {
            await _billingService.GenerateInvoiceAsync(stayId);
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            ViewData["ActiveStays"] = await _stayRecordService.GetAllStaysAsync();
            return View();
        }
    }

    [HttpGet]
    public async Task<IActionResult> Payment(int id)
    {
        var billing = await _billingService.GetByIdAsync(id);
        if (billing == null) return NotFound();
        return View(billing);
    }

    [HttpPost]
    public async Task<IActionResult> Payment(int id, PaymentStatus paymentStatus, string? remarks)
    {
        try
        {
            await _billingService.ProcessPaymentAsync(id, paymentStatus, remarks);
            return RedirectToAction("Index");
        }
        catch (Exception ex)
        {
            ModelState.AddModelError("", ex.Message);
            var billing = await _billingService.GetByIdAsync(id);
            return View(billing);
        }
    }
}
