Chart.defaults.global.responsive = true;
var listenersChartContext = new Chart(document.getElementById('listeners').getContext('2d'));
var playsChartContext = new Chart(document.getElementById('plays').getContext('2d'));

var listenersChart = null;
var playsChart = null;

var addCommas = function(nStr) {
    nStr += '';
    x = nStr.split('.');
    x1 = x[0];
    x2 = x.length > 1 ? '.' + x[1] : '';
    var rgx = /(\d+)(\d{3})/;
    while (rgx.test(x1)) {
        x1 = x1.replace(rgx, '$1' + ',' + '$2');
    }
    return x1 + x2;
}

var initCharts = function() {
    if (listenersChart != null) {
        listenersChart.destroy();
    }

    if (playsChart != null) {
        playsChart.destroy();
    }
};

var getValues = function(arr, property) {
    return arr.map(function(item) {
        return item[property];
    });
};

var fillCharts = function (artistsInfo) {

    initCharts();

    artistsInfo.sort(function(a, b) {
        return a.Listeners < b.Listeners;
    });

    var listenersData = {
        labels: getValues(artistsInfo, "Name"),
        datasets: [
            {
                fillColor: "rgba(220,220,220,0.5)",
                strokeColor: "rgba(220,220,220,0.8)",
                highlightFill: "rgba(220,220,220,0.75)",
                highlightStroke: "rgba(220,220,220,1)",
                data: getValues(artistsInfo, "Listeners")
            }
        ]
    };

    artistsInfo.sort(function (a, b) {
        return a.Plays < b.Plays;
    });

    var playsData = {
        labels: getValues(artistsInfo, "Name"),
        datasets: [
            {
                fillColor: "rgba(151,187,205,0.5)",
                strokeColor: "rgba(151,187,205,0.8)",
                highlightFill: "rgba(151,187,205,0.75)",
                highlightStroke: "rgba(151,187,205,1)",
                data: getValues(artistsInfo, "Plays")
            }
        ]
    };

    listenersChart = listenersChartContext.Bar(listenersData, {
        scaleLabel: "<%=addCommas(value)%>",
        tooltipTemplate: "<%if (label){%><%=label%>: <%}%><%= addCommas(value) %>"
    });
    playsChart = playsChartContext.Bar(playsData, {
        scaleLabel: "<%=addCommas(value)%>",
        tooltipTemplate: "<%if (label){%><%=label%>: <%}%><%= addCommas(value) %>"
    });

    window.dispatchEvent(new Event('resize'));
};