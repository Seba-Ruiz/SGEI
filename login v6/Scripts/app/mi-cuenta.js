/*
 * Modal "Mi cuenta" (menú del usuario, Views/Shared/_Layout.cshtml).
 *  - Al hacer clic en "Mi cuenta" abre la modal y pide su contenido a Manage/MiCuenta (Views/Manage/_MiCuenta.cshtml).
 *  - "Editar" pasa a modo edición (nombre, apellido y teléfono); "Guardar" los manda por AJAX a Manage/GuardarMiCuenta,
 *    muestra el aviso verde y actualiza el nombre y las iniciales de la barra superior sin recargar la página.
 */
(function ($) {
    "use strict";

    $(function () {
        var $modal = $("#modal-mi-cuenta");
        if (!$modal.length || !window.bootstrap) { return; }
        var $contenido = $modal.find("[data-contenido]");
        var modal = window.bootstrap.Modal.getOrCreateInstance($modal[0]);

        var cargando = '<div class="modal-body text-center py-5 text-body-secondary">'
            + '<span class="spinner-border spinner-border-sm text-primary me-2" role="status" aria-hidden="true"></span>Cargando tu cuenta...</div>';

        var modo = function (m) {
            $contenido.find("[data-modo]").each(function () {
                this.hidden = this.getAttribute("data-modo") !== m;
            });
            if (m === "editar") { $contenido.find("#mi-cuenta-nombre").trigger("focus"); }
        };

        var limpiarErrores = function () {
            $contenido.find(".is-invalid").removeClass("is-invalid");
            $contenido.find("[data-error]").text("");
        };

        var cargar = function () {
            $contenido.html(cargando);
            $.ajax({ url: $modal.attr("data-url"), cache: false })
                .done(function (html) { $contenido.html(html); })
                .fail(function () {
                    $contenido.html('<div class="modal-body"><div class="alert alert-danger mb-0">No se pudieron cargar los datos de tu cuenta. Probá de nuevo.</div></div>'
                        + '<div class="modal-footer"><button type="button" class="btn btn-outline-secondary" data-bs-dismiss="modal">Cerrar</button></div>');
                });
        };

        $(document).on("click", "[data-mi-cuenta]", function (e) {
            e.preventDefault();
            cargar();
            modal.show();
        });

        $contenido.on("click", "[data-mi-cuenta-editar]", function () { modo("editar"); });

        $contenido.on("click", "[data-mi-cuenta-cancelar]", function () {
            limpiarErrores();
            var $form = $contenido.find("[data-mi-cuenta-form]");
            $form[0].reset();
            modo("ver");
        });

        $contenido.on("submit", "[data-mi-cuenta-form]", function (e) {
            e.preventDefault();
            var $form = $(this);
            var $boton = $form.find("[data-mi-cuenta-guardar]");
            limpiarErrores();
            $boton.prop("disabled", true);

            var datos = $form.serializeArray();
            datos.push({ name: "__RequestVerificationToken", value: SGEI.tokenAntiforgery() });

            $.ajax({ url: $form.attr("action"), type: "POST", data: $.param(datos) })
                .done(function (r) {
                    // Pasa los valores guardados al modo ver y al valor inicial del formulario (para "Cancelar").
                    ["nombre", "apellido", "telefono"].forEach(function (campo) {
                        var $input = $form.find("[name='" + campo + "']");
                        var valor = $.trim($input.val());
                        $input.val(valor).attr("value", valor);
                        $contenido.find("[data-ver='" + campo + "']").text(valor || "—");
                    });
                    $contenido.find("[data-mi-cuenta-nombre]").text(r.nombre);
                    $contenido.find("[data-mi-cuenta-iniciales]").text(r.iniciales);
                    $("[data-usuario-nombre]").text(r.nombre);
                    $("[data-usuario-iniciales]").text(r.iniciales);
                    modo("ver");
                    SGEI.mostrarAviso(r.mensaje || "Se guardaron los datos de tu cuenta.");
                })
                .fail(function (xhr) {
                    var r = xhr.responseJSON || {};
                    $.each(r.errores || {}, function (campo, mensaje) {
                        $form.find("[name='" + campo + "']").addClass("is-invalid");
                        $form.find("[data-error='" + campo + "']").text(mensaje);
                    });
                    $form.find(".is-invalid").first().trigger("focus");
                    SGEI.mostrarAviso(r.mensaje || "No se pudieron guardar los datos. Probá de nuevo.", "error");
                })
                .always(function () { $boton.prop("disabled", false); });
        });

        // Al cerrarla se descarta lo que no se guardó: la próxima vez se vuelve a pedir.
        $modal.on("hidden.bs.modal", function () { $contenido.empty(); });
    });
})(jQuery);
