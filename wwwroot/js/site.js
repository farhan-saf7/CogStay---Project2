/* ==========================================
   CogStay Global Javascript Helper Functions
   ========================================== */

document.addEventListener('DOMContentLoaded', () => {
    initSidebarToggle();
    highlightActiveMenu();
    initModals();
    initForms();
});

// 1. Sidebar Toggling for Mobile Responsive Design
function initSidebarToggle() {
    const toggleBtn = document.querySelector('.cogstay-sidebar-toggle');
    const sidebar = document.querySelector('.cogstay-sidebar');
    
    if (toggleBtn && sidebar) {
        toggleBtn.addEventListener('click', (e) => {
            e.stopPropagation();
            sidebar.classList.toggle('open');
        });

        // Close sidebar if user clicks outside of it on mobile
        document.addEventListener('click', (e) => {
            if (sidebar.classList.contains('open') && !sidebar.contains(e.target) && !toggleBtn.contains(e.target)) {
                sidebar.classList.remove('open');
            }
        });
    }
}

// 2. Active Sidebar Link Highlighting based on Route
function highlightActiveMenu() {
    const currentPath = window.location.pathname.toLowerCase();
    const links = document.querySelectorAll('.cogstay-menu-link');
    
    links.forEach(link => {
        const href = link.getAttribute('href');
        if (href) {
            const normalizedHref = href.toLowerCase();
            // Match current page route or if path starts with href (to catch nested actions)
            if (currentPath === normalizedHref || (normalizedHref !== '/' && normalizedHref !== '/home/index' && currentPath.startsWith(normalizedHref))) {
                link.classList.add('active');
            } else {
                link.classList.remove('active');
            }
        }
    });
}

// 3. Reusable Modal Helpers (Open / Close)
function initModals() {
    const closeButtons = document.querySelectorAll('[data-close-modal]');
    closeButtons.forEach(btn => {
        btn.addEventListener('click', () => {
            const modal = btn.closest('.cogstay-modal');
            if (modal) closeModal(modal.id);
        });
    });

    // Close on background overlay click
    const modals = document.querySelectorAll('.cogstay-modal');
    modals.forEach(modal => {
        modal.addEventListener('click', (e) => {
            if (e.target === modal) {
                closeModal(modal.id);
            }
        });
    });
}

function openModal(modalId) {
    const modal = document.getElementById(modalId);
    if (modal) {
        modal.classList.add('show');
        document.body.style.overflow = 'hidden'; // Prevent background scrolling
    }
}

function closeModal(modalId) {
    const modal = document.getElementById(modalId);
    if (modal) {
        modal.classList.remove('show');
        document.body.style.overflow = ''; // Re-enable scrolling
    }
}

// 4. Dynamic Toast Notification Component
function showToast(title, message, type = 'info', duration = 4000) {
    let container = document.querySelector('.cogstay-toast-container');
    if (!container) {
        container = document.createElement('div');
        container.className = 'cogstay-toast-container';
        document.body.appendChild(container);
    }

    const toast = document.createElement('div');
    toast.className = `cogstay-toast ${type}`;

    let iconClass = 'fa-info-circle';
    if (type === 'success') iconClass = 'fa-check-circle';
    else if (type === 'warning') iconClass = 'fa-exclamation-triangle';
    else if (type === 'danger') iconClass = 'fa-exclamation-circle';

    toast.innerHTML = `
        <div class="cogstay-toast-icon">
            <i class="fas ${iconClass}"></i>
        </div>
        <div class="cogstay-toast-content">
            <div class="cogstay-toast-title">${title}</div>
            <div class="cogstay-toast-message">${message}</div>
        </div>
        <button class="cogstay-toast-close">&times;</button>
    `;

    container.appendChild(toast);

    // Close button handler
    const closeBtn = toast.querySelector('.cogstay-toast-close');
    closeBtn.addEventListener('click', () => {
        removeToast(toast);
    });

    // Auto dismiss
    setTimeout(() => {
        removeToast(toast);
    }, duration);
}

function removeToast(toast) {
    toast.style.animation = 'fadeOut 0.3s ease forwards';
    setTimeout(() => {
        if (toast.parentNode) {
            toast.parentNode.removeChild(toast);
        }
    }, 300);
}

// 5. Generic Form Verification and Interceptors
function initForms() {
    const forms = document.querySelectorAll('form');
    forms.forEach(form => {
        // Find if there's a custom submit interceptor
        form.addEventListener('submit', (e) => {
            const action = form.getAttribute('action');
            
            // If action is set but we are in static-only mode, we can validate and intercept
            if (action && (action.startsWith('/') || action.includes('Customer') || action.includes('Admin') || action.includes('Manager') || action.includes('FrontDesk') || action.includes('Housekeeping'))) {
                e.preventDefault();
                
                if (validateForm(form)) {
                    // Trigger success feedback
                    const successMsg = form.getAttribute('data-success-message') || 'Action completed successfully!';
                    
                    showToast('Success', successMsg, 'success');
                    
                    // Trigger custom callback or modal if registered, or redirect
                    const redirectUrl = form.getAttribute('data-redirect');
                    if (redirectUrl) {
                        setTimeout(() => {
                            window.location.href = redirectUrl;
                        }, 1500);
                    }
                    
                    // Clear form
                    form.reset();
                }
            }
        });
    });
}

function validateForm(form) {
    let isValid = true;
    const requiredInputs = form.querySelectorAll('[required]');
    
    // Reset previous invalid stylings
    form.querySelectorAll('.cogstay-form-control').forEach(ctrl => {
        ctrl.style.borderColor = '';
    });

    requiredInputs.forEach(input => {
        if (!input.value.trim()) {
            input.style.borderColor = 'var(--color-danger)';
            isValid = false;
            showToast('Validation Error', `${input.previousElementSibling ? input.previousElementSibling.innerText : 'Field'} is required.`, 'danger');
        } else if (input.type === 'email' && !validateEmail(input.value)) {
            input.style.borderColor = 'var(--color-danger)';
            isValid = false;
            showToast('Validation Error', 'Please enter a valid email address.', 'danger');
        } else if (input.type === 'tel' && !validatePhone(input.value)) {
            input.style.borderColor = 'var(--color-danger)';
            isValid = false;
            showToast('Validation Error', 'Please enter a valid phone number.', 'danger');
        }
    });

    // Check-in and check-out date validations if both are present in the form
    const checkin = form.querySelector('[name*="checkin"], [name*="CheckIn"]');
    const checkout = form.querySelector('[name*="checkout"], [name*="CheckOut"]');
    if (checkin && checkout && checkin.value && checkout.value) {
        const inDate = new Date(checkin.value);
        const outDate = new Date(checkout.value);
        if (outDate <= inDate) {
            checkout.style.borderColor = 'var(--color-danger)';
            isValid = false;
            showToast('Validation Error', 'Check-out date must be after check-in date.', 'danger');
        }
    }

    return isValid;
}

function validateEmail(email) {
    const re = /^[^\s@]+@[^\s@]+\.[^\s@]+$/;
    return re.test(email);
}

function validatePhone(phone) {
    const re = /^\+?[0-9\s-]{7,15}$/;
    return re.test(phone);
}
