/*
 * Inicio/Index (Dashboard): dibuja los 4 gráficos con Chart.js 4 a partir de <script id="datos-dashboard">.
 *  - Incidentes por mes: barras apiladas por tipo de equipo.
 *  - Tickets por estado: dona con el total en el centro.
 *  - Tickets por usuario: barras horizontales apiladas por estado.
 *  - Computadoras por sistema operativo: barras horizontales con el valor al final de cada barra.
 * Cada gráfico tiene leyenda, tooltip y una tabla "Ver datos" (los colores no son la única forma de leerlo).
 */
(function ($) {
    "use strict";

    $(function () {
        var Chart = window.Chart;
        var datos = JSON.parse($("#datos-dashboard").text() || "{}");
        if (!Chart) { return; }

        var estilo = getComputedStyle(document.body);
        var TEXTO = "#52514e";
        var TEXTO_FUERTE = "#1f2a37";
        var GRILLA = "#eef0f3";
        var SUPERFICIE = "#ffffff";

        Chart.defaults.font.family = estilo.fontFamily;
        Chart.defaults.font.size = 12;
        Chart.defaults.color = TEXTO;
        Chart.defaults.plugins.legend.labels.usePointStyle = true;
        Chart.defaults.plugins.legend.labels.pointStyle = "rectRounded";
        Chart.defaults.plugins.legend.labels.boxWidth = 10;
        Chart.defaults.plugins.legend.labels.boxHeight = 10;
        Chart.defaults.plugins.tooltip.backgroundColor = "rgba(31, 42, 55, .94)";
        Chart.defaults.plugins.tooltip.padding = 10;
        Chart.defaults.plugins.tooltip.cornerRadius = 6;
        Chart.defaults.plugins.tooltip.boxPadding = 4;
        Chart.defaults.maintainAspectRatio = false;

        var formato = new Intl.NumberFormat("es-AR");
        var suma = function (lista) { return lista.reduce(function (a, b) { return a + b; }, 0); };
        var plural = function (n, uno, varios) { return formato.format(n) + " " + (n === 1 ? uno : varios); };

        var ejeValores = function (horizontal) {
            var eje = {
                beginAtZero: true,
                ticks: { precision: 0 },
                grid: { color: GRILLA },
                border: { display: false }
            };
            var ejeCategorias = { grid: { display: false }, border: { color: "#d5dae1" }, ticks: { color: TEXTO_FUERTE } };
            return horizontal ? { x: eje, y: ejeCategorias } : { x: ejeCategorias, y: eje };
        };

        // Tabla accesible con los mismos datos del gráfico.
        var armarTabla = function (id, titulo, etiquetas, series) {
            var $tabla = $('<table class="table table-sm mb-0">');
            var $fila = $("<tr>").append($("<th scope='col'>").text(titulo));
            $.each(series, function (i, s) { $fila.append($("<th scope='col' class='text-end'>").text(s.Nombre)); });
            if (series.length > 1) { $fila.append($("<th scope='col' class='text-end'>").text("Total")); }
            $tabla.append($("<thead>").append($fila));
            var $cuerpo = $("<tbody>");
            $.each(etiquetas, function (i, etiqueta) {
                var $f = $("<tr>").append($("<th scope='row' class='fw-normal'>").text(etiqueta));
                var total = 0;
                $.each(series, function (j, s) { total += s.Valores[i]; $f.append($("<td class='text-end'>").text(formato.format(s.Valores[i]))); });
                if (series.length > 1) { $f.append($("<td class='text-end fw-semibold'>").text(formato.format(total))); }
                $cuerpo.append($f);
            });
            $tabla.append($cuerpo);
            $("[data-tabla='" + id + "']").empty().append($tabla);
        };

        var sinDatos = function (id, total) {
            if (total > 0) { return false; }
            $("#grafico-" + id).attr("hidden", true);
            $("[data-vacio='" + id + "']").removeAttr("hidden");
            return true;
        };

        // Etiqueta con el valor al final de cada barra (gráfico de una sola serie).
        var valorAlFinal = {
            id: "valorAlFinal",
            afterDatasetsDraw: function (grafico) {
                var ctx = grafico.ctx;
                ctx.save();
                ctx.font = "600 12px " + Chart.defaults.font.family;
                ctx.fillStyle = TEXTO_FUERTE;
                ctx.textBaseline = "middle";
                grafico.getDatasetMeta(0).data.forEach(function (barra, i) {
                    ctx.fillText(formato.format(grafico.data.datasets[0].data[i]), barra.x + 6, barra.y);
                });
                ctx.restore();
            }
        };

        // Total en el centro de la dona.
        var totalAlCentro = {
            id: "totalAlCentro",
            afterDraw: function (grafico) {
                var area = grafico.chartArea;
                var x = (area.left + area.right) / 2;
                var y = (area.top + area.bottom) / 2;
                var total = suma(grafico.data.datasets[0].data);
                var ctx = grafico.ctx;
                ctx.save();
                ctx.textAlign = "center";
                ctx.fillStyle = TEXTO_FUERTE;
                ctx.font = "700 28px " + Chart.defaults.font.family;
                ctx.fillText(formato.format(total), x, y + 4);
                ctx.fillStyle = TEXTO;
                ctx.font = "12px " + Chart.defaults.font.family;
                ctx.fillText(total === 1 ? "ticket" : "tickets", x, y + 24);
                ctx.restore();
            }
        };

        // 1. Incidentes por mes: barras apiladas por tipo de equipo.
        (function () {
            var d = datos.incidentes;
            var total = d.Total;
            $("[data-total='incidentes']").text(plural(total, "incidente", "incidentes"));
            armarTabla("incidentes", "Mes", d.Etiquetas, d.Series);
            if (sinDatos("incidentes", total)) { return; }
            new Chart(document.getElementById("grafico-incidentes"), {
                type: "bar",
                data: {
                    labels: d.Etiquetas,
                    datasets: d.Series.map(function (s) {
                        return {
                            label: s.Nombre, data: s.Valores, backgroundColor: s.Color,
                            borderColor: SUPERFICIE, borderWidth: { top: 2 }, borderSkipped: "bottom",
                            borderRadius: 2, maxBarThickness: 36
                        };
                    })
                },
                options: {
                    scales: (function () { var e = ejeValores(false); e.x.stacked = true; e.y.stacked = true; return e; })(),
                    interaction: { mode: "index", intersect: false },
                    plugins: {
                        legend: { position: "bottom" },
                        tooltip: {
                            filter: function (item) { return item.raw > 0; },
                            itemSort: function (a, b) { return b.raw - a.raw; },
                            callbacks: {
                                footer: function (items) {
                                    var t = items.length ? suma(items[0].chart.data.datasets.map(function (ds) { return ds.data[items[0].dataIndex]; })) : 0;
                                    return "Total: " + formato.format(t);
                                }
                            }
                        }
                    }
                }
            });
        })();

        // 2. Tickets por estado: dona.
        (function () {
            var d = datos.estados;
            var valores = d.Series[0].Valores;
            var total = d.Total;
            $("[data-total='estados']").text(plural(total, "ticket", "tickets"));
            armarTabla("estados", "Estado", d.Etiquetas, d.Series);
            if (sinDatos("estados", total)) { return; }
            new Chart(document.getElementById("grafico-estados"), {
                type: "doughnut",
                data: {
                    labels: d.Etiquetas,
                    datasets: [{ data: valores, backgroundColor: datos.coloresEstado, borderColor: SUPERFICIE, borderWidth: 2, hoverOffset: 6 }]
                },
                options: {
                    cutout: "62%",
                    layout: { padding: 8 },
                    plugins: {
                        legend: {
                            position: "bottom",
                            // La leyenda muestra la cantidad: "En curso (7)".
                            labels: {
                                generateLabels: function (grafico) {
                                    return Chart.overrides.doughnut.plugins.legend.labels.generateLabels(grafico).map(function (l) {
                                        l.text = l.text + " (" + formato.format(valores[l.index]) + ")";
                                        return l;
                                    });
                                }
                            }
                        },
                        tooltip: {
                            callbacks: {
                                label: function (item) {
                                    return " " + plural(item.raw, "ticket", "tickets") + " (" + Math.round(item.raw * 100 / total) + "%)";
                                }
                            }
                        }
                    }
                },
                plugins: [totalAlCentro]
            });
        })();

        // 3. Tickets por usuario: barras horizontales apiladas por estado.
        (function () {
            var d = datos.usuarios;
            var total = d.Total;
            $("[data-total='usuarios']").text(plural(d.Etiquetas.length, "persona", "personas"));
            armarTabla("usuarios", "Asignado a", d.Etiquetas, d.Series);
            if (sinDatos("usuarios", total)) { return; }
            var lienzo = document.getElementById("grafico-usuarios");
            $(lienzo).parent().css("height", Math.max(220, 70 + d.Etiquetas.length * 42) + "px");
            new Chart(lienzo, {
                type: "bar",
                data: {
                    labels: d.Etiquetas,
                    datasets: d.Series.map(function (s) {
                        return {
                            label: s.Nombre, data: s.Valores, backgroundColor: s.Color,
                            borderColor: SUPERFICIE, borderWidth: { right: 2 }, borderSkipped: "left",
                            borderRadius: 2, maxBarThickness: 26
                        };
                    })
                },
                options: {
                    indexAxis: "y",
                    scales: (function () { var e = ejeValores(true); e.x.stacked = true; e.y.stacked = true; return e; })(),
                    interaction: { mode: "index", intersect: false, axis: "y" },
                    plugins: {
                        legend: { position: "bottom" },
                        tooltip: {
                            filter: function (item) { return item.raw > 0; },
                            callbacks: {
                                footer: function (items) {
                                    var t = items.length ? suma(items[0].chart.data.datasets.map(function (ds) { return ds.data[items[0].dataIndex]; })) : 0;
                                    return "Total: " + plural(t, "ticket", "tickets");
                                }
                            }
                        }
                    }
                }
            });
        })();

        // 4. Computadoras por sistema operativo: barras horizontales, una sola serie.
        (function () {
            var d = datos.sistemas;
            var s = d.Series[0];
            var total = d.Total;
            $("[data-total='sistemas']").text(plural(total, "computadora", "computadoras"));
            armarTabla("sistemas", "Sistema operativo", d.Etiquetas, d.Series);
            if (sinDatos("sistemas", total)) { return; }
            var lienzo = document.getElementById("grafico-sistemas");
            $(lienzo).parent().css("height", Math.max(220, 40 + d.Etiquetas.length * 34) + "px");
            new Chart(lienzo, {
                type: "bar",
                data: {
                    labels: d.Etiquetas,
                    datasets: [{ label: s.Nombre, data: s.Valores, backgroundColor: s.Color, borderRadius: 4, borderSkipped: "left", maxBarThickness: 22 }]
                },
                options: {
                    indexAxis: "y",
                    layout: { padding: { right: 36 } },
                    scales: ejeValores(true),
                    plugins: {
                        legend: { display: false },
                        tooltip: {
                            callbacks: {
                                label: function (item) {
                                    return " " + plural(item.raw, "computadora", "computadoras") + " (" + Math.round(item.raw * 100 / total) + "%)";
                                }
                            }
                        }
                    }
                },
                plugins: [valorAlFinal]
            });
        })();
    });
})(jQuery);
