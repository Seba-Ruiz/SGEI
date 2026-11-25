/*
 * Campana de notificaciones (Views/Shared/_Campana.cshtml), en todas las pantallas con sesión iniciada.
 *  - Muestra la cantidad sin leer en la campana y en el título de la pestaña: "(3) Tickets - SGEI".
 *  - Al abrirla pide las más nuevas sin leer (Notificaciones/Campana) y permite marcarlas como leídas.
 *  - SignalR (hub "notificacionesHub", /signalr): "nueva" cuando llega una notificación y "cantidad" cuando
 *    cambia la cantidad sin leer (por ejemplo, se leyó en otra pestaña). Si se corta, reintenta solo.
 */
(function ($) {
    "use strict";

    $(function () {
        var $campana = $("#sgei-campana");
        if (!$campana.length) { return; }

        var $cantidad = $campana.find("[data-cantidad]");
        var $boton = $campana.find(".sgei-campana-boton");
        var $lista = $campana.find("[data-lista]");
        var $leerTodas = $campana.find("[data-leer-todas]");
        var tituloOriginal = document.title.replace(/^\(\d+\+?\)\s*/, "");
        var cargada = false;
        var abierta = false;

        // ------------------------------------------------------------------ Cantidad sin leer

        var mostrarCantidad = function (n) {
            n = Math.max(0, parseInt(n, 10) || 0);
            $campana.data("cantidad", n);
            $cantidad.text(n > 99 ? "99+" : n).prop("hidden", n === 0);
            $leerTodas.prop("hidden", n === 0);
            $boton.attr("aria-label", n === 0
                ? "Notificaciones: no tenés notificaciones sin leer"
                : "Notificaciones: " + n + " sin leer");
            document.title = (n > 0 ? "(" + (n > 99 ? "99+" : n) + ") " : "") + tituloOriginal;
        };

        var sacudir = function () {
            $boton.removeClass("sgei-campana-nueva");
            // Fuerza el reflow para que la animación vuelva a empezar.
            void $boton[0].offsetWidth;
            $boton.addClass("sgei-campana-nueva");
        };

        // ------------------------------------------------------------------ Lista

        var relativa = function (iso) {
            var fecha = new Date(iso);
            var segundos = Math.round((Date.now() - fecha.getTime()) / 1000);
            if (isNaN(segundos)) { return ""; }
            if (segundos < 60) { return "recién"; }
            var minutos = Math.round(segundos / 60);
            if (minutos < 60) { return "hace " + minutos + " min"; }
            var horas = Math.round(minutos / 60);
            if (horas < 24) { return "hace " + horas + (horas === 1 ? " hora" : " horas"); }
            var dias = Math.round(horas / 24);
            if (dias < 7) { return dias === 1 ? "ayer" : "hace " + dias + " días"; }
            return fecha.toLocaleDateString("es-AR");
        };

        var armarItem = function (n) {
            var $item = $('<div class="sgei-campana-item" role="group">').attr("data-id", n.id);
            var $enlace = $('<a class="sgei-campana-enlace">').attr("href", n.url);
            $enlace.append($('<span class="sgei-campana-icono" aria-hidden="true">').append($("<i class='bi'>").addClass(n.icono)));
            var $texto = $('<span class="sgei-campana-texto">');
            $texto.append($('<span class="sgei-campana-titulo">').text(n.titulo));
            if (n.mensaje) { $texto.append($('<span class="sgei-campana-mensaje">').text(n.mensaje)); }
            var $meta = $('<span class="sgei-campana-meta">');
            $meta.append($("<time>").attr({ datetime: n.fecha, title: n.fechaTexto }).text(relativa(n.fecha) + " · " + n.fechaTexto));
            if (n.origen) { $meta.append(document.createTextNode(" · " + n.origen)); }
            $texto.append($meta);
            $enlace.append($texto);
            var $leer = $('<button type="button" class="btn btn-sm btn-outline-primary sgei-campana-leer" data-leer>')
                .attr("aria-label", "Marcar como leída: " + n.titulo)
                .append('<i class="bi bi-check2 me-1" aria-hidden="true"></i>Marcar como leída');
            return $item.append($enlace, $leer);
        };

        var mostrarVacia = function () {
            if (!$lista.find(".sgei-campana-item").length) {
                $lista.html('<div class="sgei-campana-estado"><i class="bi bi-bell-slash d-block fs-3 mb-1" aria-hidden="true"></i>No tenés notificaciones sin leer.</div>');
            }
        };

        var cargar = function () {
            return $.ajax({ url: $campana.data("url-lista"), cache: false, dataType: "json" })
                .done(function (r) {
                    $lista.empty();
                    $.each(r.items, function (i, n) { $lista.append(armarItem(n)); });
                    mostrarVacia();
                    mostrarCantidad(r.cantidad);
                    cargada = true;
                })
                .fail(function () {
                    $lista.html('<div class="sgei-campana-estado text-danger">No se pudieron cargar las notificaciones.</div>');
                });
        };

        $campana.on("show.bs.dropdown", function () {
            abierta = true;
            // Se recarga siempre al abrir: puede haber cambios hechos en otra pestaña.
            cargar();
        });
        $campana.on("hidden.bs.dropdown", function () { abierta = false; });

        var quitarItem = function ($item) {
            $item.slideUp(150, function () {
                $item.remove();
                mostrarVacia();
            });
        };

        $lista.on("click", "[data-leer]", function () {
            var $item = $(this).closest(".sgei-campana-item");
            $(this).prop("disabled", true);
            $.post($campana.data("url-leer"), { id: $item.data("id") })
                .done(function (r) {
                    quitarItem($item);
                    mostrarCantidad(r.cantidad);
                })
                .fail(function () {
                    SGEI.mostrarAviso("No se pudo marcar la notificación como leída.", "error");
                })
                .always(function () { $item.find("[data-leer]").prop("disabled", false); });
        });

        $leerTodas.on("click", function () {
            var $b = $(this).prop("disabled", true);
            $.post($campana.data("url-leer-todas"))
                .done(function () {
                    $lista.find(".sgei-campana-item").remove();
                    mostrarVacia();
                    mostrarCantidad(0);
                    $(document).trigger("sgei:notificaciones-leidas");
                })
                .fail(function () {
                    SGEI.mostrarAviso("No se pudieron marcar las notificaciones como leídas.", "error");
                })
                .always(function () { $b.prop("disabled", false); });
        });

        mostrarCantidad($campana.data("cantidad"));

        // ------------------------------------------------------------------ En vivo (SignalR)

        if (!$.hubConnection) { return; }

        var conexion = $.hubConnection($campana.data("url-signalr"), { useDefaultPath: false });
        var hub = conexion.createHubProxy("notificacionesHub");

        hub.on("nueva", function (n, cantidad) {
            mostrarCantidad(cantidad);
            sacudir();
            if (abierta || cargada) {
                $lista.find(".sgei-campana-estado").remove();
                $lista.find('.sgei-campana-item[data-id="' + n.id + '"]').remove();
                $lista.prepend(armarItem(n));
            }
            mostrarAvisoNueva(n);
            $(document).trigger("sgei:notificacion", [n]);
        });

        hub.on("cantidad", function (cantidad) {
            mostrarCantidad(cantidad);
            if (cargada) { cargar(); }
            $(document).trigger("sgei:notificaciones-cambiaron", [cantidad]);
        });

        var reintentos = 0;
        var conectar = function () {
            conexion.start().done(function () { reintentos = 0; }).fail(reconectar);
        };
        var reconectar = function () {
            reintentos++;
            // 5 s, 10 s, 20 s... hasta 1 minuto entre intentos.
            setTimeout(conectar, Math.min(60000, 5000 * Math.pow(2, Math.min(reintentos - 1, 4))));
        };
        conexion.disconnected(function () {
            reconectar();
        });
        // Al volver a conectarse puede haber llegado algo mientras tanto.
        conexion.reconnected(function () { cargar(); });
        conectar();

        // Aviso flotante con la notificación que acaba de llegar.
        function mostrarAvisoNueva(n) {
            var $toast = $('<div class="toast sgei-aviso sgei-aviso-notificacion" role="status" aria-live="polite" aria-atomic="true">'
                + '<div class="toast-header"><i class="bi bi-bell-fill text-primary me-2" aria-hidden="true"></i>'
                + '<strong class="me-auto">Nueva notificación</strong>'
                + '<button type="button" class="btn-close" data-bs-dismiss="toast" aria-label="Cerrar"></button></div>'
                + '<div class="toast-body"><a class="fw-semibold d-block"></a><span class="small text-body-secondary"></span></div></div>');
            $toast.find("a").attr("href", n.url).text(n.titulo);
            $toast.find(".toast-body span").text(n.mensaje || "");
            $toast.on("hidden.bs.toast", function () { $toast.remove(); });
            $("#sgei-avisos").append($toast);
            if (window.bootstrap && window.bootstrap.Toast) {
                window.bootstrap.Toast.getOrCreateInstance($toast[0], { delay: 8000 }).show();
            }
        }
    });
})(jQuery);
