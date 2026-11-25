/*
 * Tickets/Index: board de trabajo.
 *  - Filtros en el navegador: texto, "Mis tickets", asignado y prioridad (los contadores muestran lo visible).
 *  - Un ticket puede tener varias personas asignadas (data-asignado: ids separados por espacio).
 *  - Arrastre entre columnas con SortableJS: solo las tarjetas .sgei-ticket-movible (asignado o administrador).
 *    Al soltar se guarda el estado por AJAX (POST Tickets/Estado); si falla, la tarjeta vuelve a su lugar.
 */
(function ($) {
    "use strict";

    $(function () {
        var $board = $("#board");
        if (!$board.length) { return; }

        var urlEstado = $board.attr("data-url-estado");
        var $buscar = $("#board-buscar");
        var $mios = $("#board-mios");
        var $asignado = $("#board-asignado");
        var $prioridad = $("#board-prioridad");
        var usuario = $mios.attr("data-usuario");

        var normalizar = function (texto) {
            return String(texto || "").normalize("NFD").replace(/[̀-ͯ]/g, "").toLowerCase();
        };

        var actualizarColumnas = function () {
            $board.find(".sgei-board-lista").each(function () {
                var visibles = $(this).children(".sgei-ticket:not(.d-none)").length;
                $(this).toggleClass("sgei-board-lista-vacia", visibles === 0);
                $(this).closest(".sgei-board-columna").find("[data-contador]").text(visibles);
            });
        };

        var filtrar = function () {
            var texto = normalizar($buscar.val()).trim();
            var soloMios = $mios.is(":checked");
            var asignado = $asignado.val();
            var prioridad = $prioridad.val();
            $board.find(".sgei-ticket").each(function () {
                var $t = $(this);
                var asignados = ($t.attr("data-asignado") || "").split(" ");
                var ok = (!texto || normalizar($t.attr("data-buscar")).indexOf(texto) >= 0)
                    && (!soloMios || asignados.indexOf(usuario) >= 0)
                    && (!asignado || asignados.indexOf(asignado) >= 0)
                    && (!prioridad || $t.attr("data-prioridad") === prioridad);
                $t.toggleClass("d-none", !ok);
            });
            actualizarColumnas();
        };

        $buscar.on("input", filtrar);
        $mios.add($asignado).add($prioridad).on("change", filtrar);
        actualizarColumnas();

        // Más urgente primero; a igual prioridad, el que vence antes (mismo orden que el servidor).
        var ordenar = function (lista) {
            var tarjetas = $(lista).children(".sgei-ticket").get();
            tarjetas.sort(function (a, b) {
                return (Number(b.getAttribute("data-prioridad")) - Number(a.getAttribute("data-prioridad")))
                    || (Number(a.getAttribute("data-prevista")) - Number(b.getAttribute("data-prevista")))
                    || (Number(a.getAttribute("data-id")) - Number(b.getAttribute("data-id")));
            });
            $(lista).prepend(tarjetas);
        };

        var semaforos = ["vencido", "por-vencer", "en-plazo", "terminado", "terminado-tarde"];
        var iconos = { "vencido": "bi-exclamation-octagon-fill", "terminado": "bi-check2-circle", "terminado-tarde": "bi-check2-circle" };

        var actualizarTarjeta = function ($t, datos) {
            $.each(semaforos, function (i, s) { $t.removeClass("sgei-ticket-" + s); });
            $t.addClass("sgei-ticket-" + datos.semaforo);
            $t.find(".sgei-ticket-plazo-texto").text(datos.plazo);
            $t.find(".sgei-ticket-plazo > .bi").attr("class", "bi " + (iconos[datos.semaforo] || "bi-calendar-event"));
        };

        // Después de arrastrar, el navegador puede disparar el click del link de la tarjeta: se ignora.
        var ultimoArrastre = 0;
        $board.on("click", ".sgei-ticket a", function (e) {
            if (Date.now() - ultimoArrastre < 400) { e.preventDefault(); e.stopImmediatePropagation(); }
        });

        if (!window.Sortable) { return; }

        $board.find(".sgei-board-lista").each(function () {
            window.Sortable.create(this, {
                group: "tickets",
                draggable: ".sgei-ticket-movible",
                // El botón de los asignados abre su lista: no arranca el arrastre.
                filter: ".sgei-ticket-asignados",
                preventOnFilter: false,
                sort: false,
                animation: 150,
                // Arrastre propio de SortableJS (eventos de mouse/touch) en vez del drag & drop nativo:
                // se ve igual en todos los navegadores y la tarjeta sigue al cursor.
                forceFallback: true,
                fallbackTolerance: 4,
                delay: 120,
                delayOnTouchOnly: true,
                ghostClass: "sgei-ticket-fantasma",
                chosenClass: "sgei-ticket-elegido",
                dragClass: "sgei-ticket-arrastrado",
                onStart: function () { $board.addClass("sgei-board-arrastrando"); },
                onEnd: function (evt) {
                    ultimoArrastre = Date.now();
                    $board.removeClass("sgei-board-arrastrando");
                    if (evt.from === evt.to) { return; }

                    var $t = $(evt.item);
                    var $destino = $(evt.to);
                    var volver = function () {
                        $(evt.from).prepend(evt.item);
                        ordenar(evt.from);
                        actualizarColumnas();
                    };

                    ordenar(evt.to);
                    actualizarColumnas();
                    $t.addClass("sgei-ticket-guardando");

                    $.ajax({
                        url: urlEstado,
                        type: "POST",
                        data: { id: $t.attr("data-id"), estado: $destino.attr("data-estado") },
                        headers: { "X-Requested-With": "XMLHttpRequest" }
                    }).done(function (r) {
                        actualizarTarjeta($t, r);
                        SGEI.mostrarAviso(r.mensaje);
                    }).fail(function (xhr) {
                        volver();
                        var r = xhr.responseJSON;
                        SGEI.mostrarAviso(r && r.mensaje ? r.mensaje : "No se pudo cambiar el estado. Probá de nuevo.", "error");
                    }).always(function () {
                        $t.removeClass("sgei-ticket-guardando");
                    });
                }
            });
        });
    });
})(jQuery);
