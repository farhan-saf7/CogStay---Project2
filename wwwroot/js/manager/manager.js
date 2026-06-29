/* ==========================================
   CogStay Manager Portal Javascript Logic
   ========================================== */

document.addEventListener('DOMContentLoaded', () => {
    initChartFilters();
});

// 1. Chart filter tabs in manager occupancy/revenue screens
function initChartFilters() {
    const filterTabs = document.querySelectorAll('.chart-filter-tab');
    if (filterTabs.length) {
        filterTabs.forEach(tab => {
            tab.addEventListener('click', (e) => {
                e.preventDefault();
                // Find all tabs in the same card to avoid cross-updating other cards
                const card = tab.closest('.cogstay-card');
                if (!card) return;

                const tabs = card.querySelectorAll('.chart-filter-tab');
                tabs.forEach(t => t.classList.remove('active-tab'));
                tab.classList.add('active-tab');
                
                const timePeriod = tab.getAttribute('data-period');
                const isRevenue = card.querySelector('.fa-chart-line') !== null || card.innerHTML.includes('Revenue') || card.innerHTML.includes('Returns');
                
                showToast('Updating Chart', `Refreshing statistics for time period: ${timePeriod}`, 'info');
                
                const bars = card.querySelectorAll('.chart-bar-wrapper');
                
                let labels = [];
                let heights = [];
                let values = [];

                if (timePeriod === 'weekly') {
                    labels = ["Mon", "Tue", "Wed", "Thu", "Fri", "Sat", "Sun"];
                    if (isRevenue) {
                        heights = [72, 84, 78, 90, 88, 96, 70];
                        values = ["₹960k", "₹1,120k", "₹1,040k", "₹1,200k", "₹1,160k", "₹1,280k", "₹944k"];
                    } else { // Occupancy
                        heights = [75, 80, 85, 92, 88, 95, 78];
                        values = ["75%", "80%", "85%", "92%", "88%", "95%", "78%"];
                    }
                } else { // monthly
                    labels = ["Jan", "Feb", "Mar", "Apr", "May", "Jun", "Jul"];
                    if (isRevenue) {
                        heights = [65, 70, 75, 85, 90, 95, 98];
                        values = ["₹10,400k", "₹11,200k", "₹12,000k", "₹13,600k", "₹14,400k", "₹15,200k", "₹15,840k"];
                    } else { // Occupancy
                        heights = [70, 72, 76, 82, 85, 89, 92];
                        values = ["70%", "72%", "76%", "82%", "85%", "89%", "92%"];
                    }
                }

                bars.forEach((barWrapper, index) => {
                    if (index < labels.length) {
                        const bar = barWrapper.querySelector('.chart-bar');
                        const valSpan = barWrapper.querySelector('.chart-bar-val');
                        const labelSpan = barWrapper.querySelector('.chart-bar-label');

                        if (bar) bar.style.height = heights[index] + '%';
                        if (valSpan) valSpan.innerText = values[index];
                        if (labelSpan) labelSpan.innerText = labels[index];
                    }
                });
            });
        });
    }
}
