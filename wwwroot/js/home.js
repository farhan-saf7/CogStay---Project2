/* ==========================================
   CogStay Home Landing Page Script
   ========================================== */

document.addEventListener('DOMContentLoaded', () => {
    // Add scroll animation triggers or simple loggers
    console.log("CogStay Public Homepage Loaded");
    
    // Quick demonstration feature: room booking click redirects to customer login/booking
    const bookButtons = document.querySelectorAll('.room-card-book-now');
    bookButtons.forEach(btn => {
        btn.addEventListener('click', (e) => {
            e.preventDefault();
            showToast('Redirecting', 'Directing to guest booking checkout...', 'info');
            setTimeout(() => {
                window.location.href = '/Customer/Customer/Login';
            }, 1000);
        });
    });
});
