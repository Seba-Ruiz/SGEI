/* Inicio/Dashboard: gráfico de incidentes mensuales (Chart.js). */
(function ($) {
    "use strict";

    var COLORES_FONDO = ["rgba(54, 162, 235, 0.2)", "rgba(255, 99, 132, 0.2)", "rgba(255, 159, 64, 0.2)", "rgba(255, 205, 86, 0.2)", "rgba(75, 192, 192, 0.2)", "rgba(153, 102, 255, 0.2)", "rgba(201, 203, 207, 0.2)"];
    var COLORES_BORDE = ["rgb(54, 162, 235)", "rgb(255, 99, 132)", "rgb(255, 159, 64)", "rgb(255, 205, 86)", "rgb(75, 192, 192)", "rgb(153, 102, 255)", "rgb(201, 203, 207)"];

    function dibujar($canvas, datos) {
        new Chart($canvas.get(0).getContext("2d"), {
            type: "bar",
            data: {
                labels: datos[0],
                datasets: [{
                    label: "Incidentes",
                    data: datos[1],
                    fill: false,
                    backgroundColor: COLORES_FONDO,
                    borderColor: COLORES_BORDE,
                    borderWidth: 1
                }]
            },
            options: {
                responsive: true,
                title: { display: true, text: "INCIDENTES MENSUALES" },
                legend: { position: "bottom" },
                scales: {
                    xAxes: [{ gridLines: { display: false }, display: true }],
                    yAxes: [{ gridLines: { display: false }, display: true, ticks: { stepSize: 50, beginAtZero: true } }]
                }
            }
        });
    }

    $(function () {
        var $canvas = $("#myChart");
        if (!$canvas.length) {
            return;
        }

        $.ajax({
            type: "POST",
            url: $canvas.data("url"),
            contentType: "application/json; charset=utf-8",
            dataType: "json"
        }).done(function (datos) {
            dibujar($canvas, datos);
        });
    });
})(jQuery);
