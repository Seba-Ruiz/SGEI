/*
 * Tickets/Formulario: el combo de equipos se llena según el tipo elegido (datos en <script id="datos-formulario">);
 * con "Sin equipo" se oculta. La descripción usa Summernote (SGEI.crearEditor, en sgei.js).
 */
(function ($) {
    "use strict";

    $(function () {
        var datos = JSON.parse($("#datos-formulario").text() || "{}");
        var $tipo = $("#TipoEquipo");
        var $equipo = $("#EquipoId");
        var $bloque = $("[data-con-equipo]");

        var aplicarTipo = function (cambioDeTipo) {
            var tipo = $tipo.val();
            var valor = cambioDeTipo ? "" : String($equipo.attr("data-valor") || "");
            $equipo.empty().append($("<option>").val("").text("Elegí el equipo…"));
            $.each((datos.equipos || {})[tipo] || [], function (i, e) {
                $equipo.append($("<option>").val(e.id).text(e.texto));
            });
            $equipo.val($equipo.find("option[value='" + valor + "']").length ? valor : "").removeAttr("data-valor").trigger("change.select2");
            $bloque.toggle(!!tipo);
            $equipo.prop("required", !!tipo);
        };

        $tipo.on("change", function () { aplicarTipo(true); });
        aplicarTipo(false);
    });
})(jQuery);
