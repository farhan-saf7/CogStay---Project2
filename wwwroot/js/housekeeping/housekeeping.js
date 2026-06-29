/* ==========================================
   CogStay Housekeeping Javascript Logic
   ========================================== */

document.addEventListener('DOMContentLoaded', () => {
    initTaskChecklist();
});

// 1. Task card check item interaction and completion
function initTaskChecklist() {
    const cards = document.querySelectorAll('.housekeeper-task-card');
    
    cards.forEach(card => {
        const checkboxes = card.querySelectorAll('.task-card-item input[type="checkbox"]');
        const completeBtn = card.querySelector('.task-complete-btn');
        
        const updateButtonState = () => {
            if (completeBtn) {
                // Enable button only if all items are checked
                const allChecked = Array.from(checkboxes).every(cb => cb.checked);
                if (allChecked) {
                    completeBtn.removeAttribute('disabled');
                    completeBtn.style.opacity = '1';
                } else {
                    completeBtn.setAttribute('disabled', 'true');
                    completeBtn.style.opacity = '0.5';
                }
            }
        };

        checkboxes.forEach(cb => {
            cb.addEventListener('change', () => {
                const labelText = cb.nextElementSibling;
                if (cb.checked) {
                    labelText.style.textDecoration = 'line-through';
                    labelText.style.color = 'var(--color-text-muted)';
                } else {
                    labelText.style.textDecoration = 'none';
                    labelText.style.color = '';
                }
                updateButtonState();
            });
        });

        if (completeBtn) {
            completeBtn.addEventListener('click', () => {
                const roomNum = card.querySelector('.task-card-title').innerText;
                showToast('Task Completed', `${roomNum} cleaning log updated to CLEAN.`, 'success');
                
                card.style.opacity = '0.3';
                completeBtn.setAttribute('disabled', 'true');
                setTimeout(() => {
                    card.style.display = 'none';
                }, 800);
            });
            // Initial call
            updateButtonState();
        }
    });
}
