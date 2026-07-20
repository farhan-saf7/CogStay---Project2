using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CogStayMVC.Enums;
using CogStayMVC.Models;
using CogStayMVC.Repositories.Interfaces;
using CogStayMVC.Services.Interfaces;

namespace CogStayMVC.Services;

public class BillingService : IBillingService
{
    private readonly IBillingRepository _billingRepository;
    private readonly IStayRecordRepository _stayRecordRepository;

    public BillingService(IBillingRepository billingRepository, IStayRecordRepository stayRecordRepository)
    {
        _billingRepository = billingRepository;
        _stayRecordRepository = stayRecordRepository;
    }

    public async Task<Billing?> GetByIdAsync(int id)
    {
        return await _billingRepository.GetByIdAsync(id);
    }

    public async Task<Billing?> GetByStayIdAsync(int stayId)
    {
        return await _billingRepository.GetByStayIdAsync(stayId);
    }

    public async Task<IEnumerable<Billing>> GetAllBillingsAsync()
    {
        return await _billingRepository.GetAllAsync();
    }

    public async Task GenerateInvoiceAsync(int stayId)
    {
        var stay = await _stayRecordRepository.GetByIdAsync(stayId);
        if (stay == null)
        {
            throw new KeyNotFoundException("Stay record not found.");
        }

        var existingBill = await _billingRepository.GetByStayIdAsync(stayId);
        if (existingBill != null)
        {
            return; // Invoice already compiled
        }

        var checkInDate = stay.ActualCheckIn ?? stay.Reservation.CheckInDate;
        var checkOutDate = stay.ActualCheckOut ?? DateTime.Now;

        var totalDays = (checkOutDate.Date - checkInDate.Date).Days;
        if (totalDays <= 0) totalDays = 1;

        decimal roomPrice = stay.Reservation.Room.PricePerNight;
        decimal totalAmount = totalDays * roomPrice;

        var billing = new Billing
        {
            StayId = stayId,
            TotalAmount = totalAmount,
            PaymentStatus = PaymentStatus.Pending,
            Remarks = $"Auto-generated invoice for {totalDays} night stay."
        };

        await _billingRepository.AddAsync(billing);
    }

    public async Task ProcessPaymentAsync(int billId, PaymentStatus status, string? remarks)
    {
        var billing = await _billingRepository.GetByIdAsync(billId);
        if (billing == null)
        {
            throw new KeyNotFoundException("Invoice record not found.");
        }

        billing.PaymentStatus = status;
        if (!string.IsNullOrEmpty(remarks))
        {
            billing.Remarks = remarks;
        }

        await _billingRepository.UpdateAsync(billing);
    }
}
