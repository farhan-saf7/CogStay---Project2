/* ==========================================
   CogStay Front Desk Javascript Logic
   ========================================== */

document.addEventListener('DOMContentLoaded', () => {
    initKeycardSwiper();
    initCheckoutCalculator();
});

// 1. Keycard Swiper programming simulation
function initKeycardSwiper() {
    const swiper = document.querySelector('.keycard-swiper-placeholder');
    if (swiper) {
        swiper.addEventListener('click', () => {
            swiper.innerHTML = '<i class="fa-solid fa-spinner fa-spin"></i> Programming RFID Keycard...';
            swiper.style.borderColor = 'var(--color-success)';
            swiper.style.color = 'var(--color-success)';
            swiper.style.backgroundColor = 'var(--color-success-light)';
            
            setTimeout(() => {
                swiper.innerHTML = '<i class="fa-solid fa-circle-check"></i> Keycard Programmed Successfully! (Room 304)';
                showToast('Keycard Programmed', 'RFID Room Key Card written for Room 304.', 'success');
            }, 1200);
        });
    }
}

// 2. Billing Checkout Calculator
function initCheckoutCalculator() {
    const minibarCostInput = document.getElementById('checkout-cost-minibar');
    const damagesCostInput = document.getElementById('checkout-cost-damages');
    const roomCostText = document.getElementById('checkout-summary-room');
    const totalCostText = document.getElementById('checkout-summary-total');

    if (minibarCostInput && damagesCostInput && roomCostText && totalCostText) {
        const updateTotals = () => {
            const minibarCost = parseFloat(minibarCostInput.value) || 0;
            const damagesCost = parseFloat(damagesCostInput.value) || 0;
            const roomCost = parseFloat(roomCostText.innerText.replace(/[^0-9.]/g, '')) || 0;
            
            const subtotal = roomCost + minibarCost + damagesCost;
            totalCostText.innerText = '₹' + subtotal.toFixed(2);
        };

        minibarCostInput.addEventListener('input', updateTotals);
        damagesCostInput.addEventListener('input', updateTotals);
    }
}
