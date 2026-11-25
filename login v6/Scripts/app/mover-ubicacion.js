/*
 * Mover un equipo de ubicación desde su grilla con una modal (Views/Shared/_MoverUbicacion.cshtml).
 *  - El botón [data-mover-ubicacion] de la fila abre la modal con el nombre del equipo y su ubicación actual (que no
 *    se puede elegir como destino).
 *  - "Aceptar" manda id e idUbicacion por AJAX; si sale bien, actualiza la fila de la grilla (ubicación, fecha y
 *    responsable, si la grilla tiene esas columnas), cierra la modal y muestra el aviso verde.
 */
(function ($) {
    "use strict";

    $(function () {
        var $modal = $("#modal-mover-ubicacion");
        if (!$modal.length || !window.bootstrap) { return; }
        var modal = window.bootstrap.Modal.getOrCreateInstance($modal[0]);
        var $form = $modal.find("[data-mover-form]");
        var $destino = $modal.find("#mover-ubicacion-destino");
        var $error = $modal.find("[data-mover-error]");
        var $aceptar = $modal.find("[data-mover-aceptar]");
        var $fila = null;
        var idEquipo = null;

        var normalizar = function (texto) {
            return String(texto || "").normalize("NFD").replace(/[̀-ͯ]/g, "").trim().toLowerCase();
        };

        var mostrarError = function (mensaje) {
            $error.text(mensaje || "").prop("hidden", !mensaje);
            $destino.toggleClass("is-invalid", !!mensaje);
        };

        $(document).on("click", "[data-mover-ubicacion]", function (e) {
            e.preventDefault();
            var $boton = $(this);
            $fila = $boton.closest("tr");
            idEquipo = $boton.attr("data-id");
            var actual = $boton.attr("data-ubicacion") || "";

            $modal.find("[data-mover-nombre]").text($boton.attr("data-nombre") || "");
            $modal.find("[data-mover-actual]").text(actual || "sin ubicación");
            // La ubicación actual queda deshabilitada como destino.
            $destino.find("option").each(function () {
                this.disabled = !!this.value && normalizar(this.text) === normalizar(actual);
            });
            $destino.val("").trigger("change");
            mostrarError(null);
            modal.show();
        });

        $destino.on("change", function () {
            if ($destino.val()) { mostrarError(null); }
        });

        $form.on("submit", function (e) {
            e.preventDefault();
            if (!$destino.val()) {
                mostrarError("Elegí la nueva ubicación.");
                return;
            }
            $aceptar.prop("disabled", true);
            $.ajax({
                url: $modal.attr("data-url"),
                type: "POST",
                data: { id: idEquipo, idUbicacion: $destino.val(), __RequestVerificationToken: SGEI.tokenAntiforgery() }
            })
                .done(function (r) {
                    actualizarFila(r);
                    modal.hide();
                    SGEI.mostrarAviso(r.mensaje);
                })
                .fail(function (xhr) {
                    var r = xhr.responseJSON || {};
                    mostrarError(r.mensaje || "No se pudo mover. Probá de nuevo.");
                })
                .always(function () { $aceptar.prop("disabled", false); });
        });

        /** Pone los datos nuevos en la fila (con la API de DataTables, para que el buscador y los filtros los vean). */
        var actualizarFila = function (r) {
            if (!$fila || !$fila.length) { return; }
            var $tabla = $fila.closest("table");
            var columnas = { ".sgei-col-ubicacion": r.ubicacion, ".sgei-col-fecha-ubicacion": r.fecha, ".sgei-col-responsable": r.responsable };
            var api = $.fn.dataTable && $.fn.dataTable.isDataTable($tabla[0]) ? $tabla.DataTable() : null;
            $.each(columnas, function (selector, valor) {
                if (valor === undefined || valor === null) { return; }
                var $th = $tabla.find("thead th" + selector);
                if (!$th.length) { return; }
                if (api) {
                    api.cell($fila[0], api.column($th[0]).index()).data(valor);
                } else {
                    $fila.children().eq($th.index()).text(valor);
                }
            });
            if (api) { api.draw(false); }
            $fila.find("[data-mover-ubicacion]").attr("data-ubicacion", r.ubicacion);
            $fila.addClass("sgei-fila-resaltada");
            window.setTimeout(function () { $fila.removeClass("sgei-fila-resaltada"); }, 2500);
        };
    });
})(jQuery);
