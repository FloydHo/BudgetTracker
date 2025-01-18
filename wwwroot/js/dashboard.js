
console.log(chartData);

var ctx = document.getElementById('lineChart').getContext('2d');
var lineChart = new Chart(ctx, {
    type: 'line',
    data: {
        labels: chartData.lineChartLabel,
        datasets: [{
            label: 'Income',
            data: chartData.incomeDataByMonth,
            borderColor: 'green',
            fill: false,
            tension: 0.1
        },
        {
            label: 'Savings',
            data: chartData.savingsDataByMonth,
            borderColor: 'rgba(153, 102, 255, 1)',
            fill: false,
            tension: 0.1
        },
        {
            label: 'Expenses',
            data: chartData.expenseDataByMonth,
            borderColor: 'rgba(255, 159, 64, 1)',
            fill: false,
            tension: 0.1
        }
        ]
    },
    options: {
        responsive: true,
        scales: {
            x: {
                beginAtZero: true
            },
            y: {
                beginAtZero: true
            }
        }
    }
});

var ctxd = document.getElementById('doughnutChart').getContext('2d');
var doughnutChart = new Chart(ctxd, {
    type: 'doughnut',
    data: {
        labels: chartData.expensesInMonthByCategoryLabels,
        datasets: [{
            label: 'My First Dataset',
            data: chartData.expensesInMonthByCategoryAmount,
            hoverOffset: 4
        }]
    },
    options: {
        responsive: true,
    }
});