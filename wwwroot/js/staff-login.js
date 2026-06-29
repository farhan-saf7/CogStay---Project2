/* ==========================================
   CogStay Staff Login Script
   ========================================== */

document.addEventListener('DOMContentLoaded', () => {
    // Setup click redirects for the role select cards
    const adminCard = document.querySelector('.role-select-card.admin');
    if (adminCard) {
        adminCard.addEventListener('click', () => {
            window.location.href = '/Admin/Admin/Login';
        });
    }

    const managerCard = document.querySelector('.role-select-card.manager');
    if (managerCard) {
        managerCard.addEventListener('click', () => {
            window.location.href = '/Manager/Manager/Login';
        });
    }

    const frontDeskCard = document.querySelector('.role-select-card.frontdesk');
    if (frontDeskCard) {
        frontDeskCard.addEventListener('click', () => {
            window.location.href = '/FrontDesk/FrontDesk/Login';
        });
    }

    const housekeepingCard = document.querySelector('.role-select-card.housekeeping');
    if (housekeepingCard) {
        housekeepingCard.addEventListener('click', () => {
            window.location.href = '/Housekeeping/Housekeeping/Login';
        });
    }
});
