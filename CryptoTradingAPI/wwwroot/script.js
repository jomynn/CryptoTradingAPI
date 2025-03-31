document.addEventListener("DOMContentLoaded", async function () {
    const response = await fetch("/api/trading/backtest");
    const backtestData = await response.json();

    const labels = backtestData.map(item => item.datetime);
    const balances = backtestData.map(item => item.balance);

    const ctx = document.getElementById('backtestChart').getContext('2d');
    new Chart(ctx, {
        type: 'line',
        data: {
            labels: labels,
            datasets: [{
                label: 'Balance Over Time',
                data: balances,
                borderColor: 'blue',
                borderWidth: 2,
                fill: false
            }]
        },
        options: {
            responsive: true,
            scales: {
                x: { display: true },
                y: { display: true }
            }
        }
    });
});
