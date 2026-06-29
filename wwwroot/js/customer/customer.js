/* ==========================================
   CogStay Customer Portal Javascript Logic
   ========================================== */

document.addEventListener('DOMContentLoaded', () => {
    initRoomFilters();
    initPriceCalculator();
    initStarRatings();
});

// 1. Filter Rooms in AvailableRooms.cshtml
function initRoomFilters() {
    const roomTypeFilter = document.getElementById('filter-room-type');
    const priceFilter = document.getElementById('filter-price');
    const cards = document.querySelectorAll('.customer-rooms-grid .cogstay-room-card');
    
    if (roomTypeFilter && cards.length) {
        const applyFilters = () => {
            const selectedType = roomTypeFilter.value;
            const selectedPrice = priceFilter.value; // e.g. "low", "high"
            
            cards.forEach(card => {
                const type = card.getAttribute('data-room-type');
                const price = parseFloat(card.getAttribute('data-room-price'));
                let matchesType = selectedType === 'all' || type === selectedType;
                
                if (matchesType) {
                    card.style.display = 'flex';
                } else {
                    card.style.display = 'none';
                }
            });

            // Sorting by price
            if (selectedPrice !== 'default') {
                const parent = cards[0].parentNode;
                const sortedCards = Array.from(cards).sort((a, b) => {
                    const priceA = parseFloat(a.getAttribute('data-room-price'));
                    const priceB = parseFloat(b.getAttribute('data-room-price'));
                    return selectedPrice === 'low' ? priceA - priceB : priceB - priceA;
                });
                sortedCards.forEach(c => parent.appendChild(c));
            }
        };

        roomTypeFilter.addEventListener('change', applyFilters);
        priceFilter.addEventListener('change', applyFilters);
    }
}

// 2. Booking checkout price calculation in BookRoom.cshtml
function initPriceCalculator() {
    const checkinInput = document.getElementById('checkin-date');
    const checkoutInput = document.getElementById('checkout-date');
    const roomTypeSelect = document.getElementById('checkout-room-type');
    
    // Summary elements
    const summaryNights = document.getElementById('summary-nights');
    const summaryRoomRate = document.getElementById('summary-room-rate');
    const summarySubtotal = document.getElementById('summary-subtotal');
    const summaryTax = document.getElementById('summary-tax');
    const summaryTotal = document.getElementById('summary-total');

    if (checkinInput && checkoutInput && roomTypeSelect) {
        const rates = {
            'standard': 11200,
            'deluxe': 14400,
            'suite': 28000
        };

        const calculateTotals = () => {
            const checkin = new Date(checkinInput.value);
            const checkout = new Date(checkoutInput.value);
            const selectedType = roomTypeSelect.value;
            
            if (checkinInput.value && checkoutInput.value && checkout > checkin) {
                const diffTime = Math.abs(checkout - checkin);
                const nights = Math.ceil(diffTime / (1000 * 60 * 60 * 24));
                const rate = rates[selectedType] || 11200;
                
                const subtotal = rate * nights;
                const tax = Math.round(subtotal * 0.12 * 100) / 100; // 12% tax
                const total = subtotal + tax;
                
                if (summaryNights) summaryNights.innerText = nights + ' nights';
                if (summaryRoomRate) summaryRoomRate.innerText = '₹' + rate.toLocaleString('en-IN');
                if (summarySubtotal) summarySubtotal.innerText = '₹' + subtotal.toLocaleString('en-IN');
                if (summaryTax) summaryTax.innerText = '₹' + tax.toLocaleString('en-IN');
                if (summaryTotal) summaryTotal.innerText = '₹' + total.toLocaleString('en-IN');
            } else {
                // Reset to default
                if (summaryNights) summaryNights.innerText = '0 nights';
                if (summaryRoomRate) summaryRoomRate.innerText = '₹' + (rates[selectedType] || 11200).toLocaleString('en-IN');
                if (summarySubtotal) summarySubtotal.innerText = '₹0';
                if (summaryTax) summaryTax.innerText = '₹0';
                if (summaryTotal) summaryTotal.innerText = '₹0';
            }
        };

        checkinInput.addEventListener('change', calculateTotals);
        checkoutInput.addEventListener('change', calculateTotals);
        roomTypeSelect.addEventListener('change', calculateTotals);
    }
}

// 3. Dynamic star selections in Feedback.cshtml
function initStarRatings() {
    const starContainer = document.querySelector('.rating-stars');
    const ratingInput = document.getElementById('guest-rating-val');
    
    if (starContainer && ratingInput) {
        const stars = starContainer.querySelectorAll('i');
        stars.forEach(star => {
            star.addEventListener('click', () => {
                const score = parseInt(star.getAttribute('data-value'));
                ratingInput.value = score;
                
                stars.forEach(s => {
                    const val = parseInt(s.getAttribute('data-value'));
                    if (val <= score) {
                        s.classList.add('active');
                        s.classList.replace('fa-regular', 'fa-solid');
                    } else {
                        s.classList.remove('active');
                        s.classList.replace('fa-solid', 'fa-regular');
                    }
                });
                showToast('Rating updated', `You selected ${score} stars.`, 'info');
            });
        });
    }
}

// 4. Download Receipt Simulator
function downloadReceipt(bookingId) {
    showToast('Preparing Download', `Generating invoice receipt for Booking #${bookingId}...`, 'info');
    
    setTimeout(() => {
        const receiptContent = `==================================================
              COGSTAY PREMIUM HOTELS
                 GUEST RECEIPT
==================================================
Booking Reference : #${bookingId}
Customer Name     : John Henderson
Room Assigned     : Room 304 - Penthouse Suite
Bill Status       : Paid in Full
Currency          : Indian Rupees (INR / ₹)
==================================================
Item Description                      Amount (INR)
--------------------------------------------------
Room Lodging Charges                  ₹1,25,440.00
Mini-Bar Order (Restock)               ₹4,000.00
In-Room Dining Charges                 ₹2,500.00
--------------------------------------------------
Taxes & Service Fees (12%)             ₹15,832.80
==================================================
GRAND TOTAL PAID                      ₹1,47,772.80
==================================================
Thank you for choosing CogStay!
We look forward to welcoming you again.
==================================================`;
        
        const blob = new Blob([receiptContent], { type: 'text/plain;charset=utf-8' });
        const url = URL.createObjectURL(blob);
        const link = document.createElement('a');
        link.href = url;
        link.download = `CogStay_Receipt_${bookingId}.txt`;
        document.body.appendChild(link);
        link.click();
        document.body.removeChild(link);
        URL.revokeObjectURL(url);
        
        showToast('Download Completed', `Receipt for Booking #${bookingId} saved.`, 'success');
    }, 1200);
}
