/* ==========================================
   CogStay Admin Portal Javascript Logic
   ========================================== */

document.addEventListener('DOMContentLoaded', () => {
    initSearchFilterTable();
    initSettingsToggles();
});

// 1. Search and Filter rooms/reservations in Tables
function initSearchFilterTable() {
    const searchInput = document.getElementById('admin-table-search');
    const statusFilter = document.getElementById('admin-table-filter-status');
    const tableRows = document.querySelectorAll('.cogstay-table tbody tr');

    if (tableRows.length && (searchInput || statusFilter)) {
        const performSearch = () => {
            const query = searchInput ? searchInput.value.toLowerCase().trim() : '';
            const status = statusFilter ? statusFilter.value.toLowerCase().trim() : 'all';

            tableRows.forEach(row => {
                let matchesSearch = true;
                let matchesStatus = true;

                // Match query against any text content in row
                if (query) {
                    matchesSearch = row.textContent.toLowerCase().includes(query);
                }

                // Match status filter
                if (status !== 'all') {
                    // Find cells containing badges or specific text representing status
                    const badges = row.querySelectorAll('.cogstay-badge');
                    let statusText = '';
                    if (badges.length) {
                        statusText = badges[0].textContent.toLowerCase().trim();
                    } else {
                        // Fallback check cell columns
                        statusText = row.textContent.toLowerCase();
                    }
                    matchesStatus = statusText.includes(status) || statusText === status;
                }

                if (matchesSearch && matchesStatus) {
                    row.style.display = '';
                } else {
                    row.style.display = 'none';
                }
            });
        };

        if (searchInput) searchInput.addEventListener('input', performSearch);
        if (statusFilter) statusFilter.addEventListener('change', performSearch);
    }
}

// 2. Settings switch toggles behavior
function initSettingsToggles() {
    const switches = document.querySelectorAll('.cogstay-switch input[type="checkbox"]');
    switches.forEach(sw => {
        sw.addEventListener('change', () => {
            const state = sw.checked ? 'ENABLED' : 'DISABLED';
            const settingLabel = sw.closest('.settings-toggle-container').querySelector('h4').innerText;
            showToast('Settings Updated', `${settingLabel} has been ${state}.`, 'success');
        });
    });
}

// 3. Remove Room from active inventory
function removeRoomRow(btn, roomNo) {
    if (confirm(`Are you sure you want to remove ${roomNo} from active inventory?`)) {
        const row = btn.closest('tr');
        row.style.transition = 'all 0.5s ease';
        row.style.opacity = 0;
        setTimeout(() => {
            row.remove();
            showToast('Room Removed', `${roomNo} was successfully removed from inventory.`, 'success');
        }, 500);
    }
}

// 4. Remove Employee and de-authenticate credentials
function removeStaffRow(btn, employeeName) {
    if (employeeName === 'John Doe') {
        showToast('Lock Alert', 'System administrator account cannot be deleted.', 'warning');
        return;
    }
    if (confirm(`Are you sure you want to revoke credentials and remove employee ${employeeName}?`)) {
        const row = btn.closest('tr');
        row.style.transition = 'all 0.5s ease';
        row.style.opacity = 0;
        setTimeout(() => {
            row.remove();
            showToast('Employee Removed', `${employeeName} was removed from the operations roster.`, 'success');
        }, 500);
    }
}
