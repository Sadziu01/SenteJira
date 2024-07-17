document.addEventListener("DOMContentLoaded", () => {
    loadAuthors();
});

let myChart;

async function loadAuthors() {
    const response = await fetch('../settings.json');
    const data = await response.json();
    console.log(data);
    
    const authorSelect = document.getElementById('author');
    data.authors.forEach(author => {
        const option = document.createElement('option');
        option.value = author;
        option.textContent = author;
        authorSelect.appendChild(option);
    });
}

async function getData() {
    const period = document.getElementById('period').value;
    const author = document.getElementById('author').value;
    const displayType = document.querySelector('input[name="displayType"]:checked').value;

    let url = `https://localhost:7007/JiraWorklogs/api/worklogs?period=${period}`;
    if (author) {
        url += `&author=${encodeURIComponent(author)}`;
    }

    const response = await fetch(url);
    if (response.status === 404 || response.status !== 200) {
        displayNoData();
        return;
    }

    const data = await response.json();

    if (data.TotalHours === 0) {
        displayNoData();
        return;
    }

    if (myChart instanceof Chart) {
        myChart.destroy();
    }

    const chartData = displayType === 'percentages'
        ? [
            data.percentageProductive,
            data.percentageSupport,
            data.percentageDevelopment,
            data.percentageNonProductive
        ]
        : [
            data.productiveHours,
            data.supportHours,
            data.developmentHours,
            data.nonProductiveHours
        ];

    const ctx = document.getElementById('myChart').getContext('2d');
    myChart = new Chart(ctx, {
        type: 'bar',
        data: {
            labels: ['Produktywne', 'Wsparciowe', 'Rozwojowe', 'Nieproduktywne'],
            datasets: [{
                label: displayType === 'percentages' ? 'Procenty godzin pracy' : 'Godziny pracy',
                data: chartData,
                backgroundColor: [
                    'rgba(54, 162, 235, 0.2)',
                    'rgba(255, 99, 132, 0.2)',
                    'rgba(75, 192, 192, 0.2)',
                    'rgba(255, 206, 86, 0.2)'
                ],
                borderColor: [
                    'rgba(54, 162, 235, 1)',
                    'rgba(255, 99, 132, 1)',
                    'rgba(75, 192, 192, 1)',
                    'rgba(255, 206, 86, 1)'
                ],
                borderWidth: 1
            }]
        },
        options: {
            scales: {
                y: {
                    beginAtZero: true
                }
            }
        }
    });

    displayData(data);
}

function displayNoData() {
    document.getElementById('myChart').style.display = 'none';
    document.getElementById('noDataMessage').classList.remove('d-none');
    document.getElementById('detailedStats').style.display = 'none';
}

function displayData(data) {
    document.getElementById('noDataMessage').classList.add('d-none');
    document.getElementById('myChart').style.display = 'block';
    document.getElementById('detailedStats').style.display = 'block';

    document.getElementById('detailedStats').innerHTML = `
        <p>RP: ${data.rP_Hours.toFixed(2)} godzin ${data.percentageRP_Hours.toFixed(2)} % (z produktywnych)</p>
        <p>R: ${data.r_Hours.toFixed(2)} godzin ${data.percentageR_Hours.toFixed(2)} % (z produktywnych)</p>
    `;
}
