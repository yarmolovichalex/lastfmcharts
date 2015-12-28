Chart.defaults.global.responsive = true;
var listenersChartContext = new Chart(document.getElementById('listeners').getContext('2d'));
var playsChartContext = new Chart(document.getElementById('plays').getContext('2d'));

var listenersChart = null;
var playsChart = null;

var fillCharts = function (artistsInfo) {
    if (listenersChart != null) {
        listenersChart.destroy();
    }

    if (playsChart != null) {
        playsChart.destroy();
    }

    var labels = [];
    var listeners = [];
    var plays = [];

    var artistsInfoLength = artistsInfo.length;

    for (var i = 0; i < artistsInfoLength; i++) {
        var artistInfo = artistsInfo[i];
        labels.push(artistInfo.Name);
        listeners.push(artistInfo.Listeners);
        plays.push(artistInfo.Plays);
    };

    var listenersData = {
        labels: labels,
        datasets: [
            {
                fillColor: "rgba(220,220,220,0.5)",
                strokeColor: "rgba(220,220,220,0.8)",
                highlightFill: "rgba(220,220,220,0.75)",
                highlightStroke: "rgba(220,220,220,1)",
                data: listeners
            }
        ]
    };

    var playsData = {
        labels: labels,
        datasets: [
            {
                fillColor: "rgba(151,187,205,0.5)",
                strokeColor: "rgba(151,187,205,0.8)",
                highlightFill: "rgba(151,187,205,0.75)",
                highlightStroke: "rgba(151,187,205,1)",
                data: plays
            }
        ]
    };

    listenersChart = listenersChartContext.Bar(listenersData);
    playsChart = playsChartContext.Bar(playsData);
    window.dispatchEvent(new Event('resize'));
};